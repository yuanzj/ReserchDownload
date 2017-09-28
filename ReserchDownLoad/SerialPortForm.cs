/**
 * 功能: 串口设置界面
 * Input: 串口参数
 * Output: 串口参数
 * 回调事件: 
 *             1)SerialPortSetting_Load  加载设置界面
 *             2)btnConfirm_Click   确认按钮
 *             3)btnCancel_Click    取消按钮
 * 包含关系     
 *             1)在Form1.cs被New
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReserchDownLoad
{
    public partial class SerialPortForm : Form
    {
        private SerialPortParameter mParam;
        public SerialPortForm(SerialPortParameter _param)
        {
            InitializeComponent();
            mParam = _param;
        }

        #region 检测串口的热插拔
        public const int WM_DEVICE_CHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICE_REMOVE_COMPLETE = 0x8004;
        public const UInt32 DBT_DEVTYP_PORT = 0x00000003;
        [StructLayout(LayoutKind.Sequential)]
        struct DEV_BROADCAST_HDR
        {
            public UInt32 dbch_size;
            public UInt32 dbch_devicetype;
            public UInt32 dbch_reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct DEV_BROADCAST_PORT_Fixed
        {
            public uint dbcp_size;
            public uint dbcp_devicetype;
            public uint dbcp_reserved;
            // Variable?length field dbcp_name is declared here in the C header file.
        }

        /// <summary>
        /// 检测USB串口的拔插
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            DEV_BROADCAST_HDR dbhdr;
            if (m.Msg == WM_DEVICE_CHANGE)        // 捕获USB设备的拔出消息WM_DEVICECHANGE
            {
                switch (m.WParam.ToInt32())
                {
                    case DBT_DEVICE_REMOVE_COMPLETE:    // USB拔出  
                        dbhdr = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                        if (dbhdr.dbch_devicetype == DBT_DEVTYP_PORT)
                        {
                            this.ccbPort.DataSource = System.IO.Ports.SerialPort.GetPortNames();
                        }
                        break;
                    case DBT_DEVICEARRIVAL:             // USB插入获取对应串口名称                        
                        this.ccbPort.DataSource = System.IO.Ports.SerialPort.GetPortNames();
                        break;
                }
            }

            base.WndProc(ref m);
        }
        #endregion

        #region 加载数据
        private void SerialPortSetting_Load(object sender, EventArgs e)
        {
            this.ccbPort.DataSource = System.IO.Ports.SerialPort.GetPortNames();
            foreach(string p in (String[])this.ccbPort.DataSource)
            {
                if(p == mParam.mPort)
                {
                    this.ccbPort.SelectedItem = p;
                    return;
                }
            }
            if (this.ccbPort.Items.Count > 1)
                this.ccbPort.SelectedIndex = mParam.com_index;
            else if(this.ccbPort.Items.Count <= 1)
                this.ccbPort.SelectedIndex = 0;         
        }
        #endregion

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(this.ccbPort.Items.Count != 0)
            {
                mParam.mPort = (String)this.ccbPort.SelectedItem;
            }               
            else
            {
                mParam.mPort = null;
            }
                        
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
