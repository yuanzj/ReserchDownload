/*
 * 功能: CCU下载Task和PCU下载Task的管理者
 * Input: 运行参数（TaskParameters）
 * OutPut: 执行结果
 * 回调事件:
 *          1)pcuListViewItemHandler   PCU LVI回调
 *          2)pcuProgressBarHandler    PCU 进度条回调
 *          3)ccuListViewItemHandler   CCU LVI回调
 *          4)ccuProgressBarHandler    CCU 进度条回调
 * 包含关系
 *          1)PcuTask.cs  PCU升级任务
 *          2)CcuTask.cs  CCU升级任务
 *          3)在Form1.cs被调用
 */

using DownLoadManager.Entity.CCU_Entity.request;
using DownLoadManager.Entity.CCU_Entity.response;
using Roky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownLoadManager
{
    public class TaskManager : ITaskManager
    {
        /**
         *  任务运行标志 
         */
         public bool bTaskRunning { get; set; }
        /**
         *  任务参数
         */ 
        public TaskParameters mParam { get; set; }
        /**
         *  PCU升级任务
         */
        private PcuTask mPcuTask = null;
        /**
         *  CCU升级任务 
         */
        private CcuTask mCcuTask = null;
        /**
         *  PCU LVI回调
         */
        public event EventHandler pcuListViewItemHandler;
        /**
         *  PCU 进度条回调
         */
        public event EventHandler pcuProgressBarHandler;
        /**
         *  CCU LVI回调
         */
        public event EventHandler ccuListViewItemHandler;
        /**
         *  CCU 进度条回调
         */
        public event EventHandler ccuProgressBarHandler;
        /**
         *  停止同步报文
         */
        private SimpleSerialPortTask<pcTakeOverRsp, NullEntity> mStopSyncTask;
        private pcTakeOverRsp mStopSyncParam;
        /**
         *  重启
         */
        private SimpleSerialPortTask<ResetEcuReq, NullEntity> mResetEcuTask;
        private ResetEcuReq mResetEcuParam;
        /**
         *   同步报文        
         */
        private SimpleSerialPortTask<NullEntity, pcTakeoverReq> mBroadCastTask;


        #region 构造函数
        public TaskManager(TaskParameters param)
        {
            this.mParam = param;
        }
        #endregion

        #region 初始化任务
        private void TaskBuilder()
        {
            //停止同步报文
            mStopSyncTask = new SimpleSerialPortTask<pcTakeOverRsp, NullEntity>();
            mStopSyncParam = mStopSyncTask.GetRequestEntity();
            mStopSyncTask.RetryMaxCnts = 0;
            mStopSyncTask.Timerout = 1000;
            mStopSyncParam.DeviceType = 0xF1;
            mStopSyncParam.HardWareID = 0xF1;
           // mStopSyncParam.FirmID = 17;
            //mStopSyncParam.FirmID1 = 35;
//mStopSyncParam.FirmID2 = 4;
            //重启指令
            mResetEcuTask = new SimpleSerialPortTask<ResetEcuReq, NullEntity>();
            mResetEcuParam = mResetEcuTask.GetRequestEntity();
            mResetEcuTask.RetryMaxCnts = 0;
            mResetEcuTask.Timerout = 1000;
            mResetEcuParam.deviceType = 0XF1;
            mResetEcuParam.cmdCode = 0x02;
            mResetEcuParam.param1 = 0x11223344;
            //等待同步报文
            mBroadCastTask = new SimpleSerialPortTask<NullEntity, pcTakeoverReq>();
            mBroadCastTask.RetryMaxCnts = 0;
            mBroadCastTask.Timerout = 500;
            mBroadCastTask.SimpleSerialPortTaskOnPostExecute += (object sender, EventArgs e) =>
            {
                SerialPortEventArgs<pcTakeoverReq> mEventArgs = e as SerialPortEventArgs<pcTakeoverReq>;
                if (mEventArgs.Data != null)
                {

                    byte hw1 = (byte)(mEventArgs.Data.hardwareID >> 24);
                    byte hw2 = (byte)(mEventArgs.Data.hardwareID >> 16);
                    byte hw3 = (byte)(mEventArgs.Data.hardwareID >> 8);

                    string softVersion = String.Format("W{0}{1:D2}.{2:D2}", (byte)(mEventArgs.Data.hardwareID >> 24),
                                                                       (byte)(mEventArgs.Data.hardwareID >> 16),
                                                                       (byte)(mEventArgs.Data.hardwareID >> 8));

                    mStopSyncParam.FirmID = hw1;
                    mStopSyncParam.FirmID1 = hw2;
                    mStopSyncParam.FirmID2 = hw3;

                    //PC接管报文的响应
                    mStopSyncTask.Excute();
                    Thread.Sleep(5);
                    mStopSyncTask.Excute();
                    Thread.Sleep(5);
                    mStopSyncTask.Excute();
                    Thread.Sleep(5);
                    if (mParam.bPCU)
                    {
                        mPcuTask.ExcuteTask();//开始升级PCU
                    }
                    else if (mParam.bCCU)
                    {
                        mCcuTask.ExcuteTask();//开始升级CCU
                    }
                }
                else
                {
                    if (mParam.bPCU)
                    {
                        mPcuTask.ExcuteTask();//开始升级PCU
                    }
                    else if (mParam.bCCU)
                    {
                        //设备未上电
                        TaskArgs mArgs = new TaskArgs();
                        mArgs.msg = "中控未上电";
                        mArgs.level = Task_Level.TASK_FAIL;
                        ccuListViewItemHandler(sender, mArgs);
                        bTaskRunning = false;
                        mResetEcuParam.cmdCode = 0x02; //升级失败或者其他，恢复同步指令
                        mResetEcuTask.Excute();
                        Thread.Sleep(10);
                        mResetEcuTask.Excute();
                        Thread.Sleep(10);
                        mResetEcuTask.Excute();
                        Thread.Sleep(10);
                        mBroadCastTask.ClosePort();
                    }
                }
            };
        
            mPcuTask = new PcuTask(this.mParam.mPcuFilePath);
            //执行结束回调
            mPcuTask.TaskDoneHandler += (object sender, EventArgs e) =>
            {
                TaskArgs mArgs = e as TaskArgs;                
                if (mParam.bCCU && mArgs.level == Task_Level.TASK_PASS)
                {                                                
                    if (mCcuTask != null)
                        mCcuTask.ExcuteTask();                                   
                }
                else
                {
                    bTaskRunning = false;
                    mResetEcuParam.cmdCode = 0x02; //升级失败或者其他，恢复同步指令
                    mResetEcuTask.Excute();
                    Thread.Sleep(10);
                    mResetEcuTask.Excute();
                    Thread.Sleep(10);
                    mResetEcuTask.Excute();
                    Thread.Sleep(10);
                    mBroadCastTask.ClosePort();
                }
            };
            //PCU Item
            mPcuTask.PcuLviHandler += (object sender, EventArgs e) =>
            {                    
                pcuListViewItemHandler(sender, e);
            };
            //PCU 进度回调
            mPcuTask.pcuProgressBarHandler += (object sender, EventArgs e) =>
            {
                pcuProgressBarHandler(sender, e);
            };

            mCcuTask = new CcuTask(this.mParam.mCcuFilePath);
            //执行结束后回调
            mCcuTask.TaskDoneHandler += (object sender, EventArgs e) =>
            {
                bTaskRunning = false;
                TaskArgs mArgs = e as TaskArgs;
                if(mArgs.level == Task_Level.TASK_PASS)
                    mResetEcuParam.cmdCode = 0x01; //升级成功后，进行重启
                else
                    mResetEcuParam.cmdCode = 0x02; //升级失败或者其他，恢复同步指令
                mResetEcuTask.Excute();
                Thread.Sleep(10);
                mResetEcuTask.Excute();
                Thread.Sleep(10);
                mResetEcuTask.Excute();
                Thread.Sleep(10);
                mBroadCastTask.ClosePort();
            };
            //CCU Item
            mCcuTask.CcuLviHandler += (object sender, EventArgs e) =>
            {
                ccuListViewItemHandler(sender, e);
            };
            //CCU 进度回调
            mCcuTask.CcuProgressBarHandler += (object sender, EventArgs e) =>
            {
                ccuProgressBarHandler(sender, e);
            };           
        }
        #endregion

        public void StopTask()
        {            
            if(mPcuTask.isTaskBusy() || mCcuTask.isTaskBusy())
            {
                mPcuTask.StopTask();
                mCcuTask.StopTask();
                mBroadCastTask.StopTask();
            }
            else
            {
                bTaskRunning = false;
                mBroadCastTask.StopTask();                
                mResetEcuParam.cmdCode = 0x02; //升级失败或者其他，恢复同步指令
                mResetEcuTask.Excute();
                Thread.Sleep(10);
                mResetEcuTask.Excute();
                Thread.Sleep(10);
                mResetEcuTask.Excute();
                Thread.Sleep(10);
                mBroadCastTask.ClosePort();
            }
            //
        }
  
        public void ExcuteTask()
        {
            bTaskRunning = true;
            TaskBuilder();
            mResetEcuParam.cmdCode = 0x02;
            mResetEcuTask.Excute();
            if (mParam.bPCU)
            {
                mBroadCastTask.RetryMaxCnts = 0;
                mBroadCastTask.Timerout = 500;
               // mPcuTask.ExcuteTask();
            }
            else if(mParam.bCCU)
            {
                mBroadCastTask.RetryMaxCnts = 0;
                mBroadCastTask.Timerout = 30*1000;
            }
            mBroadCastTask.Excute();
        }
    }
}
