/**
 * 功能: 工具设置界面
 * Input: 任务参数（TaskParameters）
 * Output: 任务参数，含有CCU和PCU的升级文件的路径
 * 回调事件: 
 *             1)ToolSetting_Closed  加载设置界面
 *             2)OnConfirm_Clicked   确认按钮
 *             3)button2_Click    取消按钮
 * 包含关系     
 *             1)在Form1.cs被New
 * 
 */
using DownLoadManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReserchDownLoad
{
    public partial class ToolSetting : Form
    {
        TaskParameters mParam;
        public ToolSetting(TaskParameters _param)
        {
            InitializeComponent();
            mParam = _param;
        }

        #region 加载
        private void ToolSetting_Load(object sender, EventArgs e)
        {
            if (mParam.bCCU)
            {
                this.chkCCU.Checked = true;
                this.textBox_CCUbinFile.Text = mParam.mCcuFilePath;
                this.btnSelectCcuFile.Enabled = true;
            }
            else
            {
                this.textBox_CCUbinFile.Text = mParam.mCcuFilePath;
                this.chkCCU.Checked = false;
                this.btnSelectCcuFile.Enabled = false;
            }
                
            if (mParam.bPCU)
            {
                this.chkPcu.Checked = true;
                this.textBox_PCUbinFile.Text = mParam.mPcuFilePath;
                this.btnSelectPcuFile.Enabled = true;
            }
            else
            {
                this.textBox_PCUbinFile.Text = mParam.mPcuFilePath;
                this.chkPcu.Checked = false;
                this.btnSelectPcuFile.Enabled = false;
            }            
        }
        #endregion

        private void ToolSetting_Closed(object sender, FormClosedEventArgs e)
        {
            
        }

        #region 按键保存
        private void OnConfirm_Clicked(object sender, EventArgs e)
        {
            mParam.bCCU = this.chkCCU.Checked;
            mParam.bPCU = this.chkPcu.Checked;
            mParam.mCcuFilePath = this.textBox_CCUbinFile.Text;
            mParam.mPcuFilePath = this.textBox_PCUbinFile.Text;
            if(mParam.bCCU)
            {
                if (!File.Exists(mParam.mCcuFilePath))
                {
                    MessageBox.Show("请加载正确的中控（CCU）升级文件！");
                    return;
                }
            }
            if(mParam.bPCU)
            {
                if (!File.Exists(mParam.mPcuFilePath))
                {
                    MessageBox.Show("请加载正确的电源管理器（PCU）升级文件！");
                    return;
                }
            }
            
            this.DialogResult = DialogResult.OK;
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void chkPcu_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkPcu.Checked)
                this.btnSelectPcuFile.Enabled = true;
            else
                this.btnSelectPcuFile.Enabled = false;
        }

        private void chkCCU_CheckedChanged(object sender, EventArgs e)
        {
            if(this.chkCCU.Checked)
                this.btnSelectCcuFile.Enabled = true;
            else
                this.btnSelectCcuFile.Enabled = false;
        }

        #region 选择CCU下载文件
        private void btnSelectCcuFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "bin文件|*.bin|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox_CCUbinFile.Text = openFileDialog.FileName;
                //this.textBox_CCUbinFile.Text = System.IO.Path.GetFileNameWithoutExtension(mParam.mCcuFilePath);
            }
        }
        #endregion

        #region 选择PCU下载文件
        private void btnSelectPcuFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "bin文件|*.bin|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox_PCUbinFile.Text = openFileDialog.FileName;
                //this.textBox_PCUbinFile.Text = System.IO.Path.GetFileNameWithoutExtension(mParam.mPcuFilePath);
            }
        }
        #endregion
    }
}
