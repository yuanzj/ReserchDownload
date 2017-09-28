namespace ReserchDownLoad
{
    partial class ToolSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkCCU = new System.Windows.Forms.CheckBox();
            this.btnSelectPcuFile = new System.Windows.Forms.Button();
            this.btnSelectCcuFile = new System.Windows.Forms.Button();
            this.textBox_CCUbinFile = new System.Windows.Forms.TextBox();
            this.chkPcu = new System.Windows.Forms.CheckBox();
            this.textBox_PCUbinFile = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnComfirm = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkCCU);
            this.groupBox1.Controls.Add(this.btnSelectPcuFile);
            this.groupBox1.Controls.Add(this.btnSelectCcuFile);
            this.groupBox1.Controls.Add(this.textBox_CCUbinFile);
            this.groupBox1.Controls.Add(this.chkPcu);
            this.groupBox1.Controls.Add(this.textBox_PCUbinFile);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(654, 164);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件路径";
            // 
            // chkCCU
            // 
            this.chkCCU.AutoSize = true;
            this.chkCCU.Checked = true;
            this.chkCCU.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCCU.Location = new System.Drawing.Point(7, 100);
            this.chkCCU.Name = "chkCCU";
            this.chkCCU.Size = new System.Drawing.Size(113, 23);
            this.chkCCU.TabIndex = 0;
            this.chkCCU.Text = "中控（CCU）";
            this.chkCCU.UseVisualStyleBackColor = true;
            this.chkCCU.CheckedChanged += new System.EventHandler(this.chkCCU_CheckedChanged);
            // 
            // btnSelectPcuFile
            // 
            this.btnSelectPcuFile.Location = new System.Drawing.Point(619, 56);
            this.btnSelectPcuFile.Name = "btnSelectPcuFile";
            this.btnSelectPcuFile.Size = new System.Drawing.Size(32, 29);
            this.btnSelectPcuFile.TabIndex = 5;
            this.btnSelectPcuFile.Text = "..";
            this.btnSelectPcuFile.UseVisualStyleBackColor = true;
            this.btnSelectPcuFile.Click += new System.EventHandler(this.btnSelectPcuFile_Click);
            // 
            // btnSelectCcuFile
            // 
            this.btnSelectCcuFile.Location = new System.Drawing.Point(619, 129);
            this.btnSelectCcuFile.Name = "btnSelectCcuFile";
            this.btnSelectCcuFile.Size = new System.Drawing.Size(32, 29);
            this.btnSelectCcuFile.TabIndex = 4;
            this.btnSelectCcuFile.Text = "..";
            this.btnSelectCcuFile.UseVisualStyleBackColor = true;
            this.btnSelectCcuFile.Click += new System.EventHandler(this.btnSelectCcuFile_Click);
            // 
            // textBox_CCUbinFile
            // 
            this.textBox_CCUbinFile.Enabled = false;
            this.textBox_CCUbinFile.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_CCUbinFile.Location = new System.Drawing.Point(7, 129);
            this.textBox_CCUbinFile.Name = "textBox_CCUbinFile";
            this.textBox_CCUbinFile.Size = new System.Drawing.Size(607, 29);
            this.textBox_CCUbinFile.TabIndex = 2;
            // 
            // chkPcu
            // 
            this.chkPcu.AutoSize = true;
            this.chkPcu.Location = new System.Drawing.Point(7, 25);
            this.chkPcu.Name = "chkPcu";
            this.chkPcu.Size = new System.Drawing.Size(155, 23);
            this.chkPcu.TabIndex = 1;
            this.chkPcu.Text = "电源管理器（PCU）";
            this.chkPcu.UseVisualStyleBackColor = true;
            this.chkPcu.CheckedChanged += new System.EventHandler(this.chkPcu_CheckedChanged);
            // 
            // textBox_PCUbinFile
            // 
            this.textBox_PCUbinFile.Enabled = false;
            this.textBox_PCUbinFile.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_PCUbinFile.Location = new System.Drawing.Point(7, 54);
            this.textBox_PCUbinFile.Name = "textBox_PCUbinFile";
            this.textBox_PCUbinFile.Size = new System.Drawing.Size(607, 29);
            this.textBox_PCUbinFile.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(337, 188);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(145, 63);
            this.button2.TabIndex = 4;
            this.button2.Text = "取 消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnComfirm
            // 
            this.btnComfirm.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnComfirm.Location = new System.Drawing.Point(171, 188);
            this.btnComfirm.Name = "btnComfirm";
            this.btnComfirm.Size = new System.Drawing.Size(150, 63);
            this.btnComfirm.TabIndex = 3;
            this.btnComfirm.Text = "确  认";
            this.btnComfirm.UseVisualStyleBackColor = true;
            this.btnComfirm.Click += new System.EventHandler(this.OnConfirm_Clicked);
            // 
            // ToolSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 263);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnComfirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ToolSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ToolSetting_Closed);
            this.Load += new System.EventHandler(this.ToolSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCCU;
        private System.Windows.Forms.CheckBox chkPcu;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnComfirm;
        private System.Windows.Forms.TextBox textBox_PCUbinFile;
        private System.Windows.Forms.TextBox textBox_CCUbinFile;
        private System.Windows.Forms.Button btnSelectPcuFile;
        private System.Windows.Forms.Button btnSelectCcuFile;
    }
}