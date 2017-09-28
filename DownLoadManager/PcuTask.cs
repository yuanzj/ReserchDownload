/**
 * 功能: PCU的升级任务
 * Input: PCU升级文件的路径
 * Output: 升级结果
 * 回调事件:
 *          1)TaskDoneHandler  升级结束事件
 *          2)pcuProgressBarHandler   进度汇报委托事件
 *          3)PcuLviHandler  Items 委托事件
 * 包含关系:
 *          在TaskManager.cs中被调用
 */
using CommonUtils;
using DownLoadManager.Entity.CCU_Entity.request;
using DownLoadManager.Entity.CCU_Entity.response;
using DownLoadManager.Entity.PCU_Entity.request;
using DownLoadManager.Entity.PCU_Entity.response;
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
    public class PcuTask : ITaskManager
    {
        /**
         *  运行线程
         */ 
        private System.ComponentModel.BackgroundWorker pcuWorkTask;
        /**
         *  任务当前的状态
         */ 
        private Task_Level mTaskStatus { get; set; }
        /**
         *  文件路径
         */
        private string mfilePath { get; set; }
        /**
         *  运行停止回调
         */
        public event EventHandler TaskDoneHandler;
        /**
         * PCU下载参数 
         */
        private FileStream mPcuStream = null;
        /**
         *  PCU文件大小
         */
        public static int PCU_FILE_LENGTH = 0;
        /**
         *  PCU 每包大小
         */
        public static int PCU_PACKAGE_SIZE = 1024;
        /**
         *  PCU 帧大小
         */
        public static int PCU_FRAME_SIZE = 59;
        /**
         *  信号事件
         */
        public static AutoResetEvent mPcuEvent;
        /**
         *  进度汇报委托事件
         */
        public event EventHandler pcuProgressBarHandler;
        /**
         *  Items 委托事件
         */
        public event EventHandler PcuLviHandler;        
        /**
         *  升级请求报文
         */
        private SimpleSerialPortTask<PCU_UpgradeStartReq, PCU_UpgradeStartRsp> mPcuUpgradeStartTask;
        private PCU_UpgradeStartReq mPcuStartParam;
        /**
         *  单包升级开始报文
         */
        private SimpleSerialPortTask<PCU_UpgradeRequestReq, PCU_UpgradeRequestRsp> mPcuUpgradeRequestTask;
        private PCU_UpgradeRequestReq mPcuRequestParam;
        /**
         *  单包升级处理报文
         */
        private SimpleSerialPortTask<PCU_UpgradeProcessReq, PCU_UpgradeProcessRsp> mPcuUpgradeProcessTask;
        private PCU_UpgradeProcessReq mPcuProcessParam;
        /**
         *  单包升级结束报文
         */
        private SimpleSerialPortTask<PCU_UpgradeDoneReq, PCU_UpgradeDoneRsp> mPcuUpgradeDoneTask;
        private PCU_UpgradeDoneReq mPcuDoneParam;
        /**
         *  跳转报文
         */
        private SimpleSerialPortTask<PCU_UpgradeRunningReq, PCU_UpgradeRunningRsp> mPcuUpgradeRunningTask;
        private PCU_UpgradeRunningReq mPcuRunningParam;
        
        public PcuTask(string path)
        {
            this.mfilePath = path;
            TaskBuilder();
        }

        private void TaskBuilder()
        {
            //等待信号
            mPcuEvent = new AutoResetEvent(false);
            //升级PCU Task
            this.pcuWorkTask = new System.ComponentModel.BackgroundWorker();
            this.pcuWorkTask.WorkerReportsProgress = true;
            this.pcuWorkTask.WorkerSupportsCancellation = true;
            this.pcuWorkTask.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bk_doPcuWork);
            this.pcuWorkTask.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bk_ProgressPcuReport);
            this.pcuWorkTask.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bk_PcuCompleted);
            //PCU 升级请求
            mPcuUpgradeStartTask = new SimpleSerialPortTask<PCU_UpgradeStartReq, PCU_UpgradeStartRsp>();
            mPcuStartParam = mPcuUpgradeStartTask.GetRequestEntity();
            mPcuUpgradeStartTask.RetryMaxCnts = 500;
            mPcuUpgradeStartTask.Timerout = 20;//10s超时
            mPcuStartParam.deviceType = 0x06;
            mPcuStartParam.Param1 = 0x0;
            mPcuStartParam.Param2 = 0x11;
            mPcuStartParam.Param3 = 0x22;
            mPcuStartParam.Param4 = 0x33;
            mPcuUpgradeStartTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<PCU_UpgradeStartRsp> mEventArgs = e as SerialPortEventArgs<PCU_UpgradeStartRsp>;
                if (mEventArgs.Data != null)
                {
                    mPcuEvent.Set();
                }
                else
                {
                    UpdateLviStatus(this, "开始升级请求无回应", Task_Level.TASK_FAIL);
                }
            };
            //PCU 升级开始
            mPcuUpgradeRequestTask = new SimpleSerialPortTask<PCU_UpgradeRequestReq, PCU_UpgradeRequestRsp>();
            mPcuRequestParam = mPcuUpgradeRequestTask.GetRequestEntity();
            mPcuUpgradeRequestTask.RetryMaxCnts = 3;
            mPcuUpgradeRequestTask.Timerout = 5 * 1000;//15s超时
            mPcuUpgradeRequestTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<PCU_UpgradeRequestRsp> mEventArgs = e as SerialPortEventArgs<PCU_UpgradeRequestRsp>;
                if (mEventArgs.Data != null)
                {
                    if (mEventArgs.Data.status == 0)
                        mPcuEvent.Set();
                    else
                    {
                        pcuWorkTask.CancelAsync();
                        Thread.Sleep(1);
                        mPcuEvent.Set();
                        UpdateLviStatus(sender, "单包升级开始失败", Task_Level.TASK_FAIL);
                        return;
                    }
                }  
                else
                {
                    UpdateLviStatus(this, "单包升级开始无回应", Task_Level.TASK_FAIL);                    
                }            
            };
            //PCU 升级处理
            mPcuUpgradeProcessTask = new SimpleSerialPortTask<PCU_UpgradeProcessReq, PCU_UpgradeProcessRsp>();
            mPcuProcessParam = mPcuUpgradeProcessTask.GetRequestEntity();
            mPcuUpgradeProcessTask.RetryMaxCnts = 5;
            mPcuUpgradeProcessTask.Timerout = 3 * 1000;//10s超时
            mPcuUpgradeProcessTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<PCU_UpgradeProcessRsp> mEventArgs = e as SerialPortEventArgs<PCU_UpgradeProcessRsp>;
                if (mEventArgs.Data != null)
                {
                    if (mEventArgs.Data.status == 0)
                    {
                        mPcuEvent.Set();
                    }
                    else if (mEventArgs.Data.status == 1)
                    {
                        pcuWorkTask.CancelAsync();
                        Thread.Sleep(1);
                        mPcuEvent.Set();
                        UpdateLviStatus(sender, "升级处理:校验和错误", Task_Level.TASK_FAIL);
                    }
                    else if (mEventArgs.Data.status == 2)
                    {
                        pcuWorkTask.CancelAsync();
                        Thread.Sleep(1);
                        mPcuEvent.Set();
                        UpdateLviStatus(sender, "升级处理:写入错误", Task_Level.TASK_FAIL);
                    }
                } 
                else
                {
                    UpdateLviStatus(sender, "单帧处理无回应", Task_Level.TASK_FAIL);
                }              
            };
            //PCU 单次升级结束
            mPcuUpgradeDoneTask = new SimpleSerialPortTask<PCU_UpgradeDoneReq, PCU_UpgradeDoneRsp>();
            mPcuDoneParam = mPcuUpgradeDoneTask.GetRequestEntity();
            mPcuUpgradeDoneTask.RetryMaxCnts = 4;
            mPcuUpgradeDoneTask.Timerout = 3 * 1000;//10s超时
            mPcuUpgradeDoneTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<PCU_UpgradeDoneRsp> mEventArgs = e as SerialPortEventArgs<PCU_UpgradeDoneRsp>;
                if (mEventArgs.Data != null)
                {
                    if (mEventArgs.Data.command == 0)
                    {
                        mPcuEvent.Set();
                    }
                    else
                    {
                        UpdateLviStatus(sender, "单包升级结束失败", Task_Level.TASK_FAIL);
                    }
                }
                else
                {
                    UpdateLviStatus(sender, "单包升级结束无回应", Task_Level.TASK_FAIL);
                }
            };
            //PCU 跳转运行
            mPcuUpgradeRunningTask = new SimpleSerialPortTask<PCU_UpgradeRunningReq, PCU_UpgradeRunningRsp>();
            mPcuRunningParam = mPcuUpgradeRunningTask.GetRequestEntity();
            mPcuUpgradeRunningTask.RetryMaxCnts = 10;
            mPcuUpgradeRunningTask.Timerout = 1000;//5s超时
            mPcuRunningParam.devType = 0x06;
            mPcuUpgradeRunningTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<PCU_UpgradeRunningRsp> mEventArgs = e as SerialPortEventArgs<PCU_UpgradeRunningRsp>;
                if (mEventArgs.Data != null)
                {
                    mPcuEvent.Set();                    
                }
                else
                {
                    UpdateLviStatus(sender, "跳转失败", Task_Level.TASK_FAIL);
                }
            };
        }

        #region 执行线程
        private void bk_doPcuWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            mPcuUpgradeStartTask.Excute();
            if (!mPcuEvent.WaitOne())
                return;
            if (this.pcuWorkTask.CancellationPending)
                return;
            //开始显示进度条
            UpdateLviStatus(sender, "", Task_Level.TASK_INIT);
            BinaryReader mBinStream = new BinaryReader(mPcuStream);
            //多少个包
            int uploadedFileLength = 0;
            int packageCnts = (int)((PCU_FILE_LENGTH / PCU_PACKAGE_SIZE) + (PCU_FILE_LENGTH % PCU_PACKAGE_SIZE != 0 ? 1 : 0));
            for (int curPackageNo = 0; curPackageNo < packageCnts; curPackageNo++)
            {
                if (!this.pcuWorkTask.CancellationPending)
                {
                    string str = String.Format("数据包传输中({0}/{1})", curPackageNo + 1, packageCnts);
                    UpdateLviStatus(sender, str, Task_Level.TASK_PROCESS);
                    int curPackageSize = 0;
                    if ((curPackageNo == packageCnts - 1) && (PCU_FILE_LENGTH % PCU_PACKAGE_SIZE != 0))
                    {
                        curPackageSize = (int)(PCU_FILE_LENGTH % PCU_PACKAGE_SIZE);
                    }
                    else
                    {
                        curPackageSize = PCU_PACKAGE_SIZE;
                    }                    
                    //Step1： 发送升级请求
                    mPcuRequestParam.currentPage = curPackageNo; //第几K ID
                    mPcuRequestParam.year = 0x0F; //年
                    mPcuRequestParam.month = 0x29;//周数
                    mPcuRequestParam.version = 0x01;//版本好
                    mPcuRequestParam.fileLength = PCU_FILE_LENGTH;//总包长度 
                    mPcuRequestParam.singlepagesize = curPackageSize;//单个包的长度
                    mPcuUpgradeRequestTask.Excute();
                    if (!mPcuEvent.WaitOne())
                    {
                        mBinStream.Dispose();
                        mBinStream.Close();
                        mBinStream = null;
                        return;
                    }                    
                    mPcuEvent.Reset();
                    //Step2:  发送帧数据
                    Thread.Sleep(5);
                    int frameCnts = (int)((curPackageSize / PCU_FRAME_SIZE) + (curPackageSize % PCU_FRAME_SIZE != 0 ? 1 : 0));
                    byte[] packageContent = new byte[curPackageSize];
                    byte[] arrBytes = new byte[PCU_FRAME_SIZE];
                    int readByteLen = 0;
                    int offset = 0;
                    for (int frameNo = 0; frameNo < frameCnts; frameNo++)
                    {
                        if (!this.pcuWorkTask.CancellationPending)
                        {
                            Array.Clear(arrBytes, 0, arrBytes.Length);
                            if (curPackageSize - offset < PCU_FRAME_SIZE)
                            {
                                mBinStream.Read(arrBytes, 0, curPackageSize - offset);
                                readByteLen = curPackageSize - offset;
                                mPcuProcessParam.buff = arrBytes;
                                mPcuProcessParam.length = readByteLen;
                                mPcuProcessParam.currentPage = curPackageNo;
                                mPcuProcessParam.currentSeg = frameNo;
                            }
                            else
                            {
                                mBinStream.Read(arrBytes, 0, PCU_FRAME_SIZE);
                                readByteLen = PCU_FRAME_SIZE;
                                mPcuProcessParam.buff = arrBytes;
                                mPcuProcessParam.length = readByteLen;
                                mPcuProcessParam.currentPage = curPackageNo;
                                mPcuProcessParam.currentSeg = frameNo;
                            }
                            //发送数据
                            mPcuUpgradeProcessTask.Excute();
                            if (!mPcuEvent.WaitOne())
                            {
                                mBinStream.Dispose();
                                mBinStream.Close();
                                mBinStream = null;
                                return;
                            }                           
                            mPcuEvent.Reset();
                            //把当前一包拷贝到数组里
                            for (int j = 0; j < readByteLen; j++)
                            {
                                packageContent[offset + j] = arrBytes[j];
                            }
                            offset += readByteLen;
                            uploadedFileLength += readByteLen;
                            Thread.Sleep(5);
                        }
                        else
                        {
                            mBinStream.Dispose();
                            mBinStream.Close();
                            mBinStream = null;
                            return;
                        }                        
                    }                   
                    //Step3:  发送升级结束
                    mPcuDoneParam.currentPage = curPackageNo;
                    mPcuDoneParam.pageChecksum = Util.getChkSum(packageContent);
                    mPcuUpgradeDoneTask.Excute();
                    if (!mPcuEvent.WaitOne())
                    {
                        mBinStream.Dispose();
                        mBinStream.Close();
                        mBinStream = null;
                        return;
                    }
                    mPcuEvent.Reset();

                    bw.ReportProgress(uploadedFileLength * 100 / PCU_FILE_LENGTH);
                    Thread.Sleep(10);
                }
                else
                {
                    mBinStream.Dispose();
                    mBinStream.Close();
                    mBinStream = null;
                    return;
                }                
            }

            //执行PCU跳转
            UpdateLviStatus(sender, "跳转中", Task_Level.TASK_PROCESS);
            mPcuUpgradeRunningTask.Excute();
            if (!mPcuEvent.WaitOne())
            {
                mBinStream.Dispose();
                mBinStream.Close();
                mBinStream = null;
                return;
            }
            mPcuEvent.Reset();            
            mBinStream.Dispose();
            mBinStream.Close();
            mBinStream = null;
            mPcuStream.Dispose();
            mPcuStream.Close();
            mPcuStream = null;            
            UpdateLviStatus(sender, "数据传输成功", Task_Level.TASK_PASS);
        }
        #endregion

        #region 进度
        private void bk_ProgressPcuReport(object sender, ProgressChangedEventArgs e)
        {
            TaskStatusNotify(sender, "", e.ProgressPercentage, Task_Level.TASK_PROGRESS);
        }
        #endregion

        #region PCU线程完成
        private void bk_PcuCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mPcuUpgradeStartTask.StopTask();
            mPcuUpgradeProcessTask.StopTask();
            mPcuUpgradeDoneTask.StopTask();
            mPcuUpgradeRunningTask.StopTask();
            mPcuUpgradeRequestTask.StopTask();
            if (mPcuStream != null)
            {
                mPcuStream.Dispose();
                if (mPcuStream != null)
                    mPcuStream.Close();
                else
                    mPcuStream = null;
            }
            TaskStatusNotify(sender, "", 0, Task_Level.TASK_DONE);
        }
        #endregion

        #region 更新ListViewItems 状态
        private void UpdateLviStatus(object sender, string msg, Task_Level level)
        {
            if(level == Task_Level.TASK_PASS)
                mTaskStatus = level;
            if (level == Task_Level.TASK_FAIL)
            {
                mTaskStatus = level;                
                this.pcuWorkTask.CancelAsync();
                Thread.Sleep(5);
                mPcuEvent.Set();                           
            }
            if (PcuLviHandler != null)
            {
                TaskArgs mArgs = new TaskArgs();
                mArgs.msg = msg;
                mArgs.level = level;
                PcuLviHandler(sender, mArgs);
            }
        }
        #endregion

        #region 任务运行状态
        private void TaskStatusNotify(object sender, string msg, int value, Task_Level level)
        {
            if(level == Task_Level.TASK_INIT)
            {
                mTaskStatus = Task_Level.TASK_INIT;
                mPcuUpgradeStartTask.InitTask();
                mPcuUpgradeProcessTask.InitTask();
                mPcuUpgradeDoneTask.InitTask();
                mPcuUpgradeRunningTask.InitTask();
                mPcuUpgradeRequestTask.InitTask();
                mPcuEvent.Reset();
                if (mPcuStream != null)
                {
                    mPcuStream.Dispose();
                    if (mPcuStream != null)
                        mPcuStream.Close();
                    else
                        mPcuStream = null;
                }
                mPcuStream = new FileStream(this.mfilePath, FileMode.Open, FileAccess.Read);
                PCU_FILE_LENGTH = (int)mPcuStream.Length;
            } 
            else if(level == Task_Level.TASK_PROGRESS)
            {
                if (pcuProgressBarHandler != null)
                {
                    ProgressArgs mArgs = new ProgressArgs();
                    mArgs.percentage = value;
                    pcuProgressBarHandler(sender, mArgs);
                }
            } 
            else if(level == Task_Level.TASK_DONE)
            {
                if (TaskDoneHandler != null)
                {
                    TaskArgs mArgs = new TaskArgs();
                    mArgs.level = mTaskStatus;
                    TaskDoneHandler(sender, mArgs);
                }
            }                      
        }
        #endregion

        public void StopTask()
        {
            pcuWorkTask.CancelAsync();
            Thread.Sleep(5);
            mPcuEvent.Set();
        }

        public bool isTaskBusy()
        {
            return this.pcuWorkTask.IsBusy;
        }

        #region 执行Task
        public void ExcuteTask()
        {
            TaskStatusNotify(this, "", 0, Task_Level.TASK_INIT);            
            
            if (!this.pcuWorkTask.IsBusy)
            {
                UpdateLviStatus(this, "开始升级请求", Task_Level.TASK_PROCESS);
                this.pcuWorkTask.RunWorkerAsync();                
            }
            else
            {
                UpdateLviStatus(this, "操作异常", Task_Level.TASK_FAIL);
                MessageBox.Show("PCU升级线程异常运行!");
                return;
            }
        }
        #endregion
    }
}
