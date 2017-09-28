/*
 *功能: 主界面 
 * InPut: None
 * OutPut: None
 * 回调事件:
 *          1)MainFrm_Load   界面初次打开的加载界面过程
 *          2)StartTask_Click  点击开始升级
 *          3)toolStripBtn_Stop_Click 停止升级
 *          4)toolStripBtn_Uart_Click  打开串口设置
 *          5)toolStripBtn_Setting_Click 工具设置
 *          6)MainFrm_Closed   关闭界面
 * 包含关系: 1)TaskManager.cs  管理CCU和PCU的升级任务
 */

using DownLoadManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ReserchDownLoad
{
    public partial class Form1 : Form
    {
        #region 私有变量
        private string filepath { get; set; }
        private TaskManager mTaskManager = null;
        private SerialPortParameter mSerialParam;
        private ProgressBar ccuProgressBar = new ProgressBar();
        private ProgressBar pcuProgressBar = new ProgressBar();
        private TaskParameters mTaskParam;
        private ListViewItem mCcuItem;
        private ListViewItem mPcuItem;

        System.Timers.Timer CcuTestTimeTicker;
        System.Timers.Timer PcuTestTimeTicker;

        private int CcuTimeCounts { get; set; }
        private int PcuTimeCounts { get; set; }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            //版本号
            this.Text = String.Format("串口升级工具 V{0}", AssemblyFileVersion());
            mTaskParam = new TaskParameters();
            mSerialParam = new SerialPortParameter();
            mPcuItem = new ListViewItem();
            mPcuItem.UseItemStyleForSubItems = false;
            mCcuItem = new ListViewItem();
            mCcuItem.UseItemStyleForSubItems = false;
            //加载配置文件     
            LoadSetting();
            mTaskManager = new TaskManager(mTaskParam);
            //设置参数
            ccuProgressBar.Value = 0;
            ccuProgressBar.Maximum = 100;
            pcuProgressBar.Value = 0;
            pcuProgressBar.Maximum = 100;
            mTaskManager.ccuProgressBarHandler += (object _sender, EventArgs _e) =>
            {
                ProgressArgs mArgs = _e as ProgressArgs;
                if (mArgs != null)
                {
                    UpdateCcuProgressBar(mArgs.percentage);
                }
            };
            mTaskManager.pcuProgressBarHandler += (object _sender, EventArgs _e) =>
            {
                ProgressArgs mArgs = _e as ProgressArgs;
                if (mArgs != null)
                {
                    UpdatePcuProgressBar(mArgs.percentage);
                }
            };
            mTaskManager.ccuListViewItemHandler += (object _sender, EventArgs _e) =>
            {
                TaskArgs mArgs = _e as TaskArgs;
                if (mArgs != null)
                {
                    UpdateCcuItems(mArgs.msg, mArgs.level);
                }
            };

            mTaskManager.pcuListViewItemHandler += (object _sender, EventArgs _e) =>
            {
                TaskArgs mArgs = _e as TaskArgs;
                if (mArgs != null)
                {
                    UpdatePcuItems(mArgs.msg, mArgs.level);
                }
            };

            CcuTimeCounts = 0;
            CcuTestTimeTicker = new System.Timers.Timer(1000);
            CcuTestTimeTicker.Enabled = false;
            CcuTestTimeTicker.Elapsed += new ElapsedEventHandler((object source, ElapsedEventArgs ElapsedEventArgs) =>
            {
                CcuTimeCounts++;
                SetTimeTicker(Devices.CCU, CcuTimeCounts);
            });

            PcuTimeCounts = 0;
            PcuTestTimeTicker = new System.Timers.Timer(1000);
            PcuTestTimeTicker.Enabled = false;
            PcuTestTimeTicker.Elapsed += new ElapsedEventHandler((object source, ElapsedEventArgs ElapsedEventArgs) =>
            {
                PcuTimeCounts++;
                SetTimeTicker(Devices.PCU, PcuTimeCounts);
            });

            string msg = String.Format("当前串口:{0} ",
                                            Const.COM_PORT);
            UpdateStatusBar(0, msg, "");

            this.toolStripBtn_Stop.Enabled = false;            
        }

        #region 加载配置参数
        private void LoadSetting()
        {
            mSerialParam.com_index = int.Parse(ConfigurationManager.AppSettings["COM_INDEX"].ToString());          
            mSerialParam.mPort = ConfigurationManager.AppSettings["COM_PORT"].ToString();
            Const.COM_PORT = mSerialParam.mPort;
            mTaskParam.bCCU = Convert.ToBoolean(ConfigurationManager.AppSettings["CCU"].ToString());
            mTaskParam.bPCU = Convert.ToBoolean(ConfigurationManager.AppSettings["PCU"].ToString());
            mTaskParam.mCcuFilePath = ConfigurationManager.AppSettings["CCU_FILE_PATH"].ToString();
            mTaskParam.mPcuFilePath = ConfigurationManager.AppSettings["PCU_FILE_PATH"].ToString();
        }
        #endregion

        #region 保存配置参数
        private void SaveSetting()
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // 修改
            cfa.AppSettings.Settings["CCU"].Value = mTaskParam.bCCU.ToString();
            cfa.AppSettings.Settings["PCU"].Value = mTaskParam.bPCU.ToString();
            cfa.AppSettings.Settings["CCU_FILE_PATH"].Value = mTaskParam.mCcuFilePath;
            cfa.AppSettings.Settings["PCU_FILE_PATH"].Value = mTaskParam.mPcuFilePath;

            cfa.AppSettings.Settings["COM_INDEX"].Value = mSerialParam.com_index.ToString();            
            cfa.AppSettings.Settings["COM_PORT"].Value = Const.COM_PORT;
            
      
            // 最后调用当前的配置文件更新成功。
            cfa.Save();
            // 刷新命名节，在下次检索它时将从磁盘重新读取它。记住应用程序要刷新节点
            ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion
       
        #region 更新statusbar
        delegate void UpdateStatusBarCallback(int no, string msg, string version);
        private void UpdateStatusBar(int no, string msg, string version)
        {
            if (this.statusStrip_settings.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.statusStrip_settings.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.statusStrip_settings.Disposing || this.statusStrip_settings.IsDisposed)
                        return;
                }
                UpdateStatusBarCallback d = new UpdateStatusBarCallback(UpdateStatusBar);
                this.statusStrip_settings.Invoke(d, new object[] {no,  msg , version});
            }
            else
            {
                switch(no)
                {
                    case 0:
                        this.toolStripStatusLabel_Uart.Text = msg;
                        break;
                    case 1:
                        this.toolStripStatusLabel_mode.Text = version;
                        break;
                }            
            }
        }
        #endregion

        #region 更新CCU进度条
        delegate void UpdateCcuProgressBarCallback(int percenttage);
        private void UpdateCcuProgressBar(int percentage)
        {
            if (this.ccuProgressBar.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.ccuProgressBar.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.ccuProgressBar.Disposing || this.ccuProgressBar.IsDisposed)
                        return;
                }
                UpdateCcuProgressBarCallback d = new UpdateCcuProgressBarCallback(UpdateCcuProgressBar);
                this.ccuProgressBar.Invoke(d, new object[] { percentage });
            }
            else
            {
                this.ccuProgressBar.Value = percentage;
            }
        }
        #endregion

        #region 更新PCU进度条
        delegate void UpdatePcuProgressBarCallback(int percenttage);
        private void UpdatePcuProgressBar(int percentage)
        {
            if (this.pcuProgressBar.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.pcuProgressBar.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.pcuProgressBar.Disposing || this.pcuProgressBar.IsDisposed)
                        return;
                }
                UpdatePcuProgressBarCallback d = new UpdatePcuProgressBarCallback(UpdatePcuProgressBar);
                this.pcuProgressBar.Invoke(d, new object[] { percentage });
            }
            else
            {
                this.pcuProgressBar.Value = percentage;
            }
        }
        #endregion

        #region 更新CCU Item
        delegate void UpdateCcuItemsCallback(string msg, Task_Level level);
        private void UpdateCcuItems(string msg, Task_Level level)
        {
            if (this.listView1.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.listView1.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.listView1.Disposing || this.listView1.IsDisposed)
                        return;
                }
                UpdateCcuItemsCallback d = new UpdateCcuItemsCallback(UpdateCcuItems);
                this.listView1.Invoke(d, new object[] {msg, level });
            }
            else
            {
                this.listView1.BeginUpdate();
                switch(level)
                {
                    case Task_Level.TASK_INIT:
                        if (mTaskParam.bPCU)
                            listView1.AddEmbeddedControl(ccuProgressBar, 2, 1);
                        else
                            listView1.AddEmbeddedControl(ccuProgressBar, 2, 0);
                        break;
                    case Task_Level.TASK_PROCESS:
                        mCcuItem.SubItems[4].Text = "";
                        mCcuItem.SubItems[1].ForeColor = Color.Blue;
                        mCcuItem.SubItems[1].Text = msg;
                        break;
                    case Task_Level.TASK_FAIL:
                        this.CcuTestTimeTicker.Enabled = false;
                        this.toolStripBtn_Stop.Enabled = false;
                        this.toolStripBtn_Uart.Enabled = true;
                        this.toolStripBtn_File.Enabled = true;
                        this.toolStripBtn_Start.Enabled = true;
                        mCcuItem.SubItems[1].ForeColor = Color.Red;
                        mCcuItem.SubItems[4].ForeColor = Color.Red;
                        mCcuItem.SubItems[4].Text = "失败";
                        mCcuItem.SubItems[1].Text = msg;
                        break;
                    case Task_Level.TASK_PASS:
                        this.CcuTestTimeTicker.Enabled = false;
                        this.toolStripBtn_Stop.Enabled = false;
                        this.toolStripBtn_Uart.Enabled = true;
                        this.toolStripBtn_File.Enabled = true;
                        this.toolStripBtn_Start.Enabled = true;
                        mCcuItem.SubItems[4].Text = "成功";
                        mCcuItem.SubItems[1].ForeColor = Color.Green;
                        mCcuItem.SubItems[4].ForeColor = Color.Green;
                        mCcuItem.SubItems[1].Text = msg;
                        break;
                }                       
                this.listView1.EndUpdate();
            }
        }
        #endregion

        #region 更新PCU
        delegate void UpdatePcuItemsCallback(string msg, Task_Level level);
        private void UpdatePcuItems(string msg, Task_Level level)
        {
            if (this.listView1.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.listView1.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.listView1.Disposing || this.listView1.IsDisposed)
                        return;
                }
                UpdatePcuItemsCallback d = new UpdatePcuItemsCallback(UpdatePcuItems);
                this.listView1.Invoke(d, new object[] { msg,  level });
            }
            else
            {
                this.listView1.BeginUpdate();
                switch(level)
                {
                    case Task_Level.TASK_INIT:
                        this.listView1.AddEmbeddedControl(pcuProgressBar, 2, 0);
                        break;
                    case Task_Level.TASK_PROCESS:
                        mPcuItem.SubItems[4].Text = "";
                        mPcuItem.SubItems[1].ForeColor = Color.Blue;
                        mPcuItem.SubItems[1].Text = msg;
                        break;
                    case Task_Level.TASK_FAIL:
                        this.PcuTestTimeTicker.Enabled = false;
                        this.toolStripBtn_Stop.Enabled = false;
                        this.toolStripBtn_Uart.Enabled = true;
                        this.toolStripBtn_File.Enabled = true;
                        this.toolStripBtn_Start.Enabled = true;
                        mPcuItem.SubItems[1].ForeColor = Color.Red;
                        mPcuItem.SubItems[4].ForeColor = Color.Red;
                        mPcuItem.SubItems[4].Text = "失败";
                        mPcuItem.SubItems[1].Text = msg;
                        break;
                    case Task_Level.TASK_PASS:
                        this.PcuTestTimeTicker.Enabled = false;
                        //如果CCU 不升级
                        if (!mTaskParam.bCCU)
                        {
                            this.toolStripBtn_Stop.Enabled = false;
                            this.toolStripBtn_Uart.Enabled = true;
                            this.toolStripBtn_File.Enabled = true;
                            this.toolStripBtn_Start.Enabled = true;
                        }
                        else
                        {
                            this.CcuTestTimeTicker.Enabled = true;
                        }
                        mPcuItem.SubItems[4].Text = "成功";
                        mPcuItem.SubItems[1].ForeColor = Color.Green;
                        mPcuItem.SubItems[4].ForeColor = Color.Green;
                        mPcuItem.SubItems[1].Text = msg;                        
                        break;                    
                }
                
                this.listView1.EndUpdate();
            }
        }
        #endregion

        delegate void SetTimeTickerCallback(Devices device, int value);
        private void SetTimeTicker(Devices device, int value)
        {
            if (this.listView1.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.listView1.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.listView1.Disposing || this.listView1.IsDisposed)
                        return;
                }
                SetTimeTickerCallback d = new SetTimeTickerCallback(SetTimeTicker);
                this.listView1.Invoke(d, new object[] { device, value });
            }
            else
            {
                this.listView1.BeginUpdate();
                switch (device)
                {
                    case Devices.PCU:
                        mPcuItem.SubItems[3].Text = value.ToString();
                        break;
                    case Devices.CCU:
                        mCcuItem.SubItems[3].Text = value.ToString();
                        break;
                }
                this.listView1.EndUpdate();
            }
        }

        private void StartTask_Click(object sender, EventArgs e)
        {
            if(!mTaskParam.bCCU && !mTaskParam.bPCU)
            {
                MessageBox.Show("未选择升级设备类型!");
                return;
            }
            if (mTaskManager.bTaskRunning)
            {
                MessageBox.Show("升级正在运行!");
                return;
            }
            this.listView1.Items.Clear();
            this.listView1.BeginUpdate();
            if (!getFileVersions())
                return;
            if (mTaskParam.bPCU)
            {
                mPcuItem.SubItems[1].Text = "等待中...";                                                              
                this.PcuTestTimeTicker.Enabled = true;
            }
            if (mTaskParam.bCCU)
            {
                if (mTaskParam.bPCU)
                {
                    mCcuItem.SubItems[1].Text = "";
                    this.CcuTestTimeTicker.Enabled = false;
                }
                else
                {
                    mCcuItem.SubItems[1].Text = "等待中...";
                    this.CcuTestTimeTicker.Enabled = true;
                }             
            }
            this.listView1.EndUpdate();
            if (ccuProgressBar != null)
                this.ccuProgressBar.Value = 0;
            if (pcuProgressBar != null)
                this.pcuProgressBar.Value = 0;
            PcuTimeCounts = 0;
            CcuTimeCounts = 0;
            this.toolStripBtn_Stop.Enabled = true;
            this.toolStripBtn_Uart.Enabled = false;
            this.toolStripBtn_File.Enabled = false;
            this.toolStripBtn_Start.Enabled = false;
            if (mTaskManager != null)
            {
                mTaskManager.ExcuteTask();
            }                                   
        }

        private void toolStripBtn_Stop_Click(object sender, EventArgs e)
        {
            mTaskManager.StopTask();
            int WaitTime = 200;
            //等待线程结束完成
            while(mTaskManager.bTaskRunning && WaitTime-- > 0)
            {
                Thread.Sleep(10);
            }
            if(WaitTime <= 0)
            {
                MessageBox.Show("请重新点击停止按钮!");
                return;
            }
            //熟悉
            CcuTimeCounts = 0;
            PcuTimeCounts = 0;
            this.listView1.Items.Clear();
            this.CcuTestTimeTicker.Enabled = false;
            this.PcuTestTimeTicker.Enabled = false;
            this.toolStripBtn_Stop.Enabled = false;
            this.toolStripBtn_Uart.Enabled = true;
            this.toolStripBtn_File.Enabled = true;
            this.toolStripBtn_Start.Enabled = true;
            this.ccuProgressBar.Value = 0;
            this.pcuProgressBar.Value = 0;      
        }

        #region 打开串口
        private void toolStripBtn_Uart_Click(object sender, EventArgs e)
        {
            SerialPortForm frm = new SerialPortForm(mSerialParam);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                Const.COM_PORT = mSerialParam.mPort;
                string msg = String.Format("当前串口:{0} ", Const.COM_PORT);
                UpdateStatusBar(0, msg, "");
            }            
        }
        #endregion
        
        #region 工具设置
        private void toolStripBtn_Setting_Click(object sender, EventArgs e)
        {
            ToolSetting frm = new ToolSetting(mTaskParam);
            if (frm.ShowDialog() == DialogResult.OK)
            {                
                mTaskManager.mParam = mTaskParam;
                this.listView1.Items.Clear();
                if (!getFileVersions())
                    return;                           
            }
        }
        #endregion

        #region 获得版本号
        private bool getFileVersions()
        {
            this.listView1.BeginUpdate();
            //分析下载文件
            if (mTaskParam.bPCU)
            {
                this.listView1.RemoveEmbeddedControl(pcuProgressBar);
                if (File.Exists(mTaskParam.mPcuFilePath))
                {
                    byte[] mSource = new byte[] { };
                    FileStream mFile = new FileStream(mTaskParam.mPcuFilePath, FileMode.Open, FileAccess.Read);
                    BinaryReader mBinreader = new BinaryReader(mFile);
                    int fileSize = (int)mFile.Length;//获取bin文件长度
                    if (fileSize != 0)
                    {
                        mSource = mBinreader.ReadBytes(fileSize);//读取bin文件到字节数组
                    }
                    byte[] hwArray = new byte[4] { mSource[Const.Dev_0], mSource[Const.Dev_1], mSource[Const.Dev_2], mSource[Const.Dev_3] };
                    string HW_Version = String.Format("RK{0}", System.Text.Encoding.Default.GetString(hwArray));
                    string SW_Version = String.Format("W{0}{1}.{2}", mSource[Const.VER_1].ToString("D02"), mSource[Const.VER_2].ToString("D02"), mSource[Const.VER_3].ToString("D02"));                    
                    mFile.Dispose();
                    mBinreader.Dispose();
                    mFile.Close();
                    mBinreader.Close();
                    if (mPcuItem != null)
                    {
                        mPcuItem.SubItems.Clear();
                        mPcuItem.Text = HW_Version + "_" + SW_Version;
                        mPcuItem.SubItems.Add("");
                        mPcuItem.SubItems.Add("");
                        mPcuItem.SubItems.Add("");
                        mPcuItem.SubItems.Add("");
                        mPcuItem.SubItems[1].ForeColor = Color.Blue;
                        mPcuItem.SubItems[2].ForeColor = Color.Blue;
                        mPcuItem.SubItems[3].ForeColor = Color.Blue;
                        mPcuItem.SubItems[4].ForeColor = Color.Green;
                        this.listView1.Items.Add(mPcuItem);
                    }
                    else
                    {
                        MessageBox.Show("ListView Item未初始化成功");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("请加载正确的电源管理器（PCU）升级文件！");
                    return false;
                }
            }
            if (mTaskParam.bCCU)
            {
                this.listView1.RemoveEmbeddedControl(ccuProgressBar);
                if (File.Exists(mTaskParam.mCcuFilePath))
                {                    
                    if (mCcuItem != null)
                    {
                        mCcuItem.SubItems.Clear();
                        string text = Path.GetFileName(Path.GetDirectoryName(mTaskParam.mCcuFilePath));
                        string[] textArray = text.Split('_');
                        if (text != "" && textArray.Length >=2)
                        {
                            mCcuItem.Text = textArray[0] + "_" + textArray[1];
                        }
                        else
                        {
                            mCcuItem.Text = "获取不到版本";
                        }                       
                        mCcuItem.SubItems.Add("");
                        mCcuItem.SubItems.Add("");
                        mCcuItem.SubItems.Add("");
                        mCcuItem.SubItems.Add("");
                        mCcuItem.SubItems[1].ForeColor = Color.Blue;
                        mCcuItem.SubItems[2].ForeColor = Color.Blue;
                        mCcuItem.SubItems[3].ForeColor = Color.Blue;
                        mCcuItem.SubItems[4].ForeColor = Color.Green;
                        this.listView1.Items.Add(mCcuItem);
                    }
                }
                else
                {
                    MessageBox.Show("请加载正确的中控（CCU）升级文件！");
                    return false;
                }
            }
            this.listView1.EndUpdate();
            return true;
        }
        #endregion
           
        private void MainFrm_Closed(object sender, FormClosedEventArgs e)
        {
            SaveSetting();
        }

        #region 获得版本号
        private string AssemblyFileVersion()
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            else
            {
                return ((AssemblyFileVersionAttribute)attributes[0]).Version;
            }
        }
        #endregion


        #region 点击帮助文件
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string str = System.IO.Directory.GetCurrentDirectory();
            System.Diagnostics.Process.Start(str + "\\help.chm");
        }
        #endregion
    }
}
