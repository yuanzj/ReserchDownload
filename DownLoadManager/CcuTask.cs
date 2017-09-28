/**
 * 功能: CCU的升级任务
 * Input: CCU升级文件的路径
 * Output: 升级结果
 * 回调事件:
 *          1)TaskDoneHandler  升级结束事件
 *          2)CcuProgressBarHandler   进度汇报委托事件
 *          3)CcuLviHandler  Items 委托事件
 * 包含关系:
 *          在TaskManager.cs中被调用
 */

using CommonUtils;
using DownLoadManager.Entity.CCU_Entity.request;
using DownLoadManager.Entity.CCU_Entity.response;
using Roky;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownLoadManager
{
    public class CcuTask : ITaskManager
    {
        /**
         *  运行线程
         */
        private System.ComponentModel.BackgroundWorker CcuWorkTask;
        /**
         *  文件路径
         */ 
        private string mfilePath { get; set; }
        /**
         *  运行状态
         */ 
        private Task_Level mTaskStatus { get; set; }
        /**
         *  运行停止回调
         */
        public event EventHandler TaskDoneHandler;
        /**
         *  进度汇报委托事件
         */
        public event EventHandler CcuProgressBarHandler;
        /**
         *  Items 委托事件
         */
        public event EventHandler CcuLviHandler;
        /**
         * 文件总长度
         */
        public static int CCU_FILE_LENGTH = 0;
        /**
        * 单个包大小(KB)
        */
        public static int CCU_PACKAGE_SIZE_KB = 8;
        /**
         * 单个包大小(B)
         */
        public static int CCU_PACKAGE_SIZE = CCU_PACKAGE_SIZE_KB * 1024;
        /**
         * 帧大小(B)
         */
        public static int CCU_FRAME_SIZE = 60;
        /**
         *  文件流
         */
        private FileStream mCcuStream = null;
        /**
         *  信号事件
         */
        public static AutoResetEvent mCcuEvent;
        /**
         *  升级请求报文
         */
        private SimpleSerialPortTask<CCU_UpgradeRequest, CCU_UpgradeResponse> mUpgradeRequestTask;
        private CCU_UpgradeRequest mUpgradeRequestParam;
        /**
         *  单包升级开始报文
         */
        private SimpleSerialPortTask<CCU_UpgradeStartReq, CCU_UpgradeStartRsp> mUpgradeStartReqTask;
        private CCU_UpgradeStartReq mUpgradeStartReqParam;
        /**
         *  单包处理报文
         */
        private SimpleSerialPortTask<CCU_UpgradeProcessReq, CCU_UpgradeProcessRsp> mUpgradeProcessReqTask;
        private CCU_UpgradeProcessReq mUpgradeProcessReqParam;
        /**
         *  单包处理结束报文
         */
        private SimpleSerialPortTask<CCU_UpgradeEndReq, CCU_UpgradeEndRsp> mUpgradeEndReqTask;
        private CCU_UpgradeEndReq mUpgradeEndReqParam;
        /**
         *  MD5校验报文
         */
        private SimpleSerialPortTask<CCU_UpgradeMD5Req, CCU_UpgradeMD5Rsp> mUpgradeMD5ReqTask;
        private CCU_UpgradeMD5Req mUpgradeMD5ReqParam;
        #region 构造函数
        public CcuTask(string path)
        {
            this.mfilePath = path;
            TaskBuilder();
        }
        #endregion

        #region 初始化
        private void TaskBuilder()
        {
            //等待信号
            mCcuEvent = new AutoResetEvent(false);
            //升级PCU Task
            this.CcuWorkTask = new System.ComponentModel.BackgroundWorker();
            this.CcuWorkTask.WorkerReportsProgress = true;
            this.CcuWorkTask.WorkerSupportsCancellation = true;
            this.CcuWorkTask.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bk_doCcuWork);
            this.CcuWorkTask.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bk_ProgressCcuReport);
            this.CcuWorkTask.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bk_CcuCompleted);                       
            //CCU 升级请求
            mUpgradeRequestTask = new SimpleSerialPortTask<CCU_UpgradeRequest, CCU_UpgradeResponse>();
            mUpgradeRequestTask.RetryMaxCnts = 2;
            mUpgradeRequestTask.Timerout = 20 * 1000;
            mUpgradeRequestParam = mUpgradeRequestTask.GetRequestEntity();
            mUpgradeRequestTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<CCU_UpgradeResponse> mEventArgs = e as SerialPortEventArgs<CCU_UpgradeResponse>;
                if (mEventArgs.Data != null)
                {
                    if(mEventArgs.Data.UpgradeResult == Const.DO_UPGRADE)
                    {
                        mCcuEvent.Set();
                    }                   
                }
                else
                {
                    UpdateLviStatus(sender, "升级请求无回应", Task_Level.TASK_FAIL);
                }
            };
            //CCU 升级开始
            mUpgradeStartReqTask = new SimpleSerialPortTask<CCU_UpgradeStartReq, CCU_UpgradeStartRsp>();
            mUpgradeStartReqParam = mUpgradeStartReqTask.GetRequestEntity();
            mUpgradeStartReqTask.RetryMaxCnts = 2;
            mUpgradeStartReqTask.Timerout = 1 * 1000;
            mUpgradeStartReqTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<CCU_UpgradeStartRsp> mEventArgs = e as SerialPortEventArgs<CCU_UpgradeStartRsp>;
                if (mEventArgs.Data != null)
                {
                    switch (mEventArgs.Data.Result)
                    {
                        case Const.DO_NOT_UPGRADE:

                            break;
                        case Const.DO_UPGRADE:
                            mCcuEvent.Set();
                            break;
                        case Const.CONTINUE_UPGRADE:
                            mCcuEvent.Set();
                            break;
                    }
                }
                else
                {
                    UpdateLviStatus(sender, "单包开始升级无回应", Task_Level.TASK_FAIL);
                }
            };
            //CCU 升级处理
            mUpgradeProcessReqTask = new SimpleSerialPortTask<CCU_UpgradeProcessReq, CCU_UpgradeProcessRsp>();
            mUpgradeProcessReqParam = mUpgradeProcessReqTask.GetRequestEntity();
            mUpgradeProcessReqTask.Timerout = 2*1000;
            mUpgradeProcessReqTask.RetryMaxCnts = 30;
            mUpgradeProcessReqTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<CCU_UpgradeProcessRsp> mEventArgs = e as SerialPortEventArgs<CCU_UpgradeProcessRsp>;
                if (mEventArgs.Data != null)
                {
                    if (mEventArgs.Data.Result == Const.RECV_DATA_OK)
                    {
                        mCcuEvent.Set();
                    }
                    else
                    {
                        if (mEventArgs.Data.Cause == Const.RECV_DATA_FAIL)
                        {                                                   
                            UpdateLviStatus(sender, "单帧处理:接受数据错误", Task_Level.TASK_FAIL);
                        }
                    }
                }
                else
                {
                    UpdateLviStatus(sender, "单帧升级处理无回应", Task_Level.TASK_FAIL);
                }
            };
            //CCU 升级结束
            mUpgradeEndReqTask = new SimpleSerialPortTask<CCU_UpgradeEndReq, CCU_UpgradeEndRsp>();
            mUpgradeEndReqParam = mUpgradeEndReqTask.GetRequestEntity();
            mUpgradeEndReqTask.Timerout = 1 * 1000;
            mUpgradeEndReqTask.RetryMaxCnts = 2;
            mUpgradeEndReqTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<CCU_UpgradeEndRsp> mEventArgs = e as SerialPortEventArgs<CCU_UpgradeEndRsp>;
                if (mEventArgs.Data != null)
                {
                    if (mEventArgs.Data.Result == Const.RECV_DATA_OK)
                    {
                        mCcuEvent.Set();
                    }
                    else
                    {                        
                        switch (mEventArgs.Data.Cause)
                        {
                            case Const.NONE_ERROR:
                                break;
                            case Const.CRC_ERROR:
                                UpdateLviStatus(sender, "单包处理:CRC校验错误", Task_Level.TASK_FAIL);
                                break;
                            case Const.WRITE_FLASH_ERROR:
                                UpdateLviStatus(sender, "单包处理:写FLASH错误", Task_Level.TASK_FAIL);
                                break;
                            case Const.PACKAGE_ID_ERROR:
                                UpdateLviStatus(sender, "单包处理:包号错误", Task_Level.TASK_FAIL);
                                break;
                            case Const.DATA_ERROR:
                                UpdateLviStatus(sender, "单包处理:数据错误", Task_Level.TASK_FAIL);
                                break;
                        }
                    }
                }
                else
                {
                    UpdateLviStatus(sender, "单包结束升级无回应", Task_Level.TASK_FAIL);
                }
            };
            //CCU MD5校验
            mUpgradeMD5ReqTask = new SimpleSerialPortTask<CCU_UpgradeMD5Req, CCU_UpgradeMD5Rsp>();
            mUpgradeMD5ReqParam = mUpgradeMD5ReqTask.GetRequestEntity();
            mUpgradeMD5ReqTask.Timerout = 2 * 1000;
            mUpgradeMD5ReqTask.RetryMaxCnts = 1;
            mUpgradeMD5ReqTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<CCU_UpgradeMD5Rsp> mEventArgs = e as SerialPortEventArgs<CCU_UpgradeMD5Rsp>;
                if (mEventArgs.Data != null)
                {
                    if (mEventArgs.Data.Result == Const.ARGEE_UPGRADE)
                    {
                        mCcuEvent.Set();                                               
                    }
                    else
                    {
                        UpdateLviStatus(sender, "MD5校验异常", Task_Level.TASK_FAIL);
                    }
                }
                else
                {
                    UpdateLviStatus(sender, "MD5校验通讯异常", Task_Level.TASK_FAIL);
                }
            };
        }
        #endregion

        #region 执行线程
        private void bk_doCcuWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            mUpgradeRequestTask.Excute();
            if (!mCcuEvent.WaitOne())
                return;
            mCcuEvent.Reset();
            if (this.CcuWorkTask.CancellationPending)
                return;
            //开始显示进度条
            UpdateLviStatus(sender, "", Task_Level.TASK_INIT);
            BinaryReader mBinStream = new BinaryReader(mCcuStream);
            int uploadedFileLength = 0;
            int packageCount = (int)((CCU_FILE_LENGTH / CCU_PACKAGE_SIZE) + (CCU_FILE_LENGTH % CCU_PACKAGE_SIZE != 0 ? 1 : 0));
            //当前包号
            for (int curPackageNo = 0; curPackageNo < packageCount; curPackageNo++)
            {
                string str = String.Format("数据包({0}/{1})", curPackageNo + 1, packageCount);
                UpdateLviStatus(sender, str, Task_Level.TASK_PROCESS);
                if (!this.CcuWorkTask.CancellationPending)//取消线程
                {                   
                    int curPackageSize = 0;
                    if ((curPackageNo == packageCount - 1) && (CCU_FILE_LENGTH % CCU_PACKAGE_SIZE != 0))
                    {
                        curPackageSize = (int)(CCU_FILE_LENGTH % CCU_PACKAGE_SIZE);
                    }
                    else
                    {
                        curPackageSize = CCU_PACKAGE_SIZE;
                    }

                    mUpgradeStartReqParam.PackageID = curPackageNo;
                    mUpgradeStartReqParam.PackageLen = curPackageSize;
                    mUpgradeStartReqParam.AlreadyDownLoadLen = uploadedFileLength;
                    /****************4.发送包开始请求********************/
                    mUpgradeStartReqTask.Excute();
                    if (!mCcuEvent.WaitOne())
                    {
                        mBinStream.Close();
                        mBinStream.Dispose();
                        mBinStream = null;
                        return;
                    }
                    mCcuEvent.Reset();
                    //等待
                    Thread.Sleep(100);
                    byte[] packageContent = new byte[curPackageSize];
                    byte[] arrBytes = new byte[CCU_FRAME_SIZE];
                    int readByteLen = 0;
                    int offset = 0;
                    while (offset < curPackageSize)
                    {
                        if (!this.CcuWorkTask.CancellationPending)
                        {
                            Array.Clear(arrBytes, 0, arrBytes.Length);
                            if (curPackageSize - offset < CCU_FRAME_SIZE)
                            {
                                mBinStream.Read(arrBytes, 0, curPackageSize - offset);
                                readByteLen = curPackageSize - offset;
                                mUpgradeProcessReqParam.FrameData = arrBytes;
                                mUpgradeProcessReqParam.length = curPackageSize - offset;
                            }
                            else
                            {
                                mBinStream.Read(arrBytes, 0, CCU_FRAME_SIZE);
                                readByteLen = CCU_FRAME_SIZE;
                                mUpgradeProcessReqParam.FrameData = arrBytes;
                                mUpgradeProcessReqParam.length = CCU_FRAME_SIZE;
                            }
                            /************************5.发送帧数据************************/
                            mUpgradeProcessReqTask.Excute();
                            if (!mCcuEvent.WaitOne())
                            {
                                mBinStream.Close();
                                mBinStream.Dispose();
                                mBinStream = null;
                                return;
                            }
                            mCcuEvent.Reset();
                            for (int j = 0; j < readByteLen; j++)
                            {
                                packageContent[offset + j] = arrBytes[j];
                            }
                            offset += readByteLen;
                            uploadedFileLength += readByteLen;
                            Thread.Sleep(3);
                        }
                        else
                        {
                            mBinStream.Close();
                            mBinStream.Dispose();
                            mBinStream = null;
                            return;
                        }                        
                    }
                    /************************5.发送包结束************************/
                    Thread.Sleep(10);
                    mUpgradeEndReqParam.PackageID = curPackageNo;
                    mUpgradeEndReqParam.CRC = CRC16.GetCrc16(packageContent);
                    mUpgradeEndReqTask.Excute();
                    if (!mCcuEvent.WaitOne())
                    {
                        mBinStream.Dispose();
                        mBinStream.Close();
                        mBinStream = null;
                        return;
                    }
                    mCcuEvent.Reset();
                    //通知进度条
                    bw.ReportProgress(uploadedFileLength * 100 / CCU_FILE_LENGTH);
                }
                else
                {
                    mBinStream.Close();
                    mBinStream.Dispose();
                    mBinStream = null;
                    return;
                }               
            }
            Thread.Sleep(20);
            UpdateLviStatus(sender, "MD5验证", Task_Level.TASK_PROCESS);
            mUpgradeMD5ReqTask.Excute();
            if (!mCcuEvent.WaitOne())
            {
                mBinStream.Dispose();
                mBinStream.Close();
                mBinStream = null;
                return;
            }
            mCcuEvent.Reset();            
            mBinStream.Close();
            mBinStream.Dispose();
            mBinStream = null;
            mCcuStream.Close();
            mCcuStream.Dispose();
            mCcuStream = null;            
            UpdateLviStatus(sender, "数据传输完成", Task_Level.TASK_PASS);
            SetProgressValue(sender, 100);
            return;
        }
        #endregion

        #region 执行进度
        private void bk_ProgressCcuReport(object sender, ProgressChangedEventArgs e)
        {
            SetProgressValue(sender, e.ProgressPercentage);
        }
        #endregion

        #region 执行结束
        private void bk_CcuCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mUpgradeStartReqTask.StopTask();
            mUpgradeRequestTask.StopTask();
            mUpgradeProcessReqTask.StopTask();
            mUpgradeMD5ReqTask.StopTask();
            mUpgradeEndReqTask.StopTask();
            if (mCcuStream != null)
            {
                mCcuStream.Dispose();
                if (mCcuStream != null)
                    mCcuStream.Close();
                mCcuStream = null;
            }
            TerminateTask(sender, mTaskStatus);
        }
        #endregion

        #region ListViewItems状态
        private void UpdateLviStatus(object sender, string msg, Task_Level level)
        {
            if (level == Task_Level.TASK_PASS)
            {
                mTaskStatus = level;
            }
            if (level == Task_Level.TASK_FAIL)
            {
                mTaskStatus = level;
                if(this.CcuWorkTask.IsBusy)//关闭正在运行的线程
                {
                    CcuWorkTask.CancelAsync();
                    Thread.Sleep(5);
                    mCcuEvent.Set();
                }                              
            }
            if (CcuLviHandler != null)
            {
                TaskArgs mArgs = new TaskArgs();
                mArgs.msg = msg;
                mArgs.level = level;
                CcuLviHandler(sender, mArgs);
            }
        }
        #endregion
       
        #region 刷新进度条
        private void SetProgressValue(object sender, int value)
        {
            if (CcuProgressBarHandler != null)
            {
                ProgressArgs mArgs = new ProgressArgs();
                mArgs.percentage = value;
                CcuProgressBarHandler(sender, mArgs);
            }
        }
        #endregion

        #region 获取任务是否在运行
        public bool isTaskBusy()
        {
            return this.CcuWorkTask.IsBusy;
        }
        #endregion

        private void InitTask()
        {
            mUpgradeStartReqTask.InitTask();
            mUpgradeRequestTask.InitTask();
            mUpgradeProcessReqTask.InitTask();
            mUpgradeMD5ReqTask.InitTask();
            mUpgradeEndReqTask.InitTask();
            if (mCcuStream != null)
            {
                mCcuStream.Dispose();
                mCcuStream.Close();
                mCcuStream = null;
            }
            mCcuEvent.Reset();
            mCcuStream = new FileStream(this.mfilePath, FileMode.Open, FileAccess.Read);
            CCU_FILE_LENGTH = (int)mCcuStream.Length;
            byte[] mVersion = new byte[4];
            mVersion[0] = 0x10;//16
            mVersion[1] = 0x31;//49
            mVersion[2] = 0x01;//01
            mVersion[3] = 0xFF;
            mUpgradeRequestParam.firmVersion = mVersion;
            mUpgradeRequestParam.fileLen = (int)CCU_FILE_LENGTH;
            //单个包的大小
            mUpgradeRequestParam.singlePackageLen = CCU_PACKAGE_SIZE_KB;//单位KB
            mUpgradeRequestParam.singleFrameLen = CCU_FRAME_SIZE;
            //是否强制升级           
            mUpgradeRequestParam.ForceUpgrade = Const.DO_UPGRADE;
            //MD5参数
            mUpgradeMD5ReqParam.FirmVersion = mVersion;
            mUpgradeMD5ReqParam.FileLen = CCU_FILE_LENGTH;
            mUpgradeMD5ReqParam.FirmUpgrade = Const.DO_UPGRADE;
            mUpgradeMD5ReqParam.MD5 = MD5.getMD5(new FileStream(this.mfilePath, FileMode.Open, FileAccess.Read));
        }

        //中止线程
        private void TerminateTask(object sender, Task_Level level)
        {
            if (TaskDoneHandler != null)
            {
                TaskArgs mArgs = new TaskArgs();
                mArgs.level = level;
                TaskDoneHandler(sender, mArgs);
            }
        }

        public void StopTask()
        {
            CcuWorkTask.CancelAsync();
            Thread.Sleep(5);
            mCcuEvent.Set();         
        }

        #region 开始执行
        public void ExcuteTask()
        {
            InitTask();
            //升级开始
            UpdateLviStatus(this, "正在擦除flash", Task_Level.TASK_PROCESS);            
            if (!this.CcuWorkTask.IsBusy)
            {
                this.CcuWorkTask.RunWorkerAsync();
            }
            else
            {
                UpdateLviStatus(this, "操作异常", Task_Level.TASK_FAIL);
                MessageBox.Show("CCU升级线程异常运行!");
            }
        }
        #endregion
    }
}
