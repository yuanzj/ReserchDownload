namespace ReserchDownLoad
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip_Stop = new System.Windows.Forms.ToolStrip();
            this.toolStripBtn_File = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtn_Uart = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtn_Start = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtn_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip_settings = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_Uart = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_mode = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listView1 = new ReserchDownLoad.ListViewEx();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip_Stop.SuspendLayout();
            this.statusStrip_settings.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip_Stop
            // 
            this.toolStrip_Stop.AutoSize = false;
            this.toolStrip_Stop.BackColor = System.Drawing.Color.SteelBlue;
            this.toolStrip_Stop.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.toolStrip_Stop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtn_File,
            this.toolStripSeparator2,
            this.toolStripBtn_Uart,
            this.toolStripSeparator1,
            this.toolStripBtn_Start,
            this.toolStripBtn_Stop,
            this.toolStripSeparator4,
            this.toolStripButton1});
            this.toolStrip_Stop.Location = new System.Drawing.Point(0, 0);
            this.toolStrip_Stop.Name = "toolStrip_Stop";
            this.toolStrip_Stop.Size = new System.Drawing.Size(1068, 72);
            this.toolStrip_Stop.TabIndex = 2;
            this.toolStrip_Stop.Text = "帮助文档";
            // 
            // toolStripBtn_File
            // 
            this.toolStripBtn_File.AutoSize = false;
            this.toolStripBtn_File.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStripBtn_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtn_File.Image = global::ReserchDownLoad.Properties.Resources.new_file;
            this.toolStripBtn_File.ImageTransparentColor = System.Drawing.Color.BlueViolet;
            this.toolStripBtn_File.Name = "toolStripBtn_File";
            this.toolStripBtn_File.Size = new System.Drawing.Size(64, 64);
            this.toolStripBtn_File.ToolTipText = "加载文件";
            this.toolStripBtn_File.Click += new System.EventHandler(this.toolStripBtn_Setting_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 72);
            // 
            // toolStripBtn_Uart
            // 
            this.toolStripBtn_Uart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtn_Uart.Image = global::ReserchDownLoad.Properties.Resources.串口设置;
            this.toolStripBtn_Uart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn_Uart.Name = "toolStripBtn_Uart";
            this.toolStripBtn_Uart.Size = new System.Drawing.Size(68, 69);
            this.toolStripBtn_Uart.Text = "toolStripButton4";
            this.toolStripBtn_Uart.ToolTipText = "串口设置";
            this.toolStripBtn_Uart.Click += new System.EventHandler(this.toolStripBtn_Uart_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 72);
            // 
            // toolStripBtn_Start
            // 
            this.toolStripBtn_Start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtn_Start.Image = global::ReserchDownLoad.Properties.Resources.启动;
            this.toolStripBtn_Start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn_Start.Name = "toolStripBtn_Start";
            this.toolStripBtn_Start.Size = new System.Drawing.Size(68, 69);
            this.toolStripBtn_Start.ToolTipText = "开始升级";
            this.toolStripBtn_Start.Click += new System.EventHandler(this.StartTask_Click);
            // 
            // toolStripBtn_Stop
            // 
            this.toolStripBtn_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtn_Stop.Image = global::ReserchDownLoad.Properties.Resources.停止;
            this.toolStripBtn_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn_Stop.Name = "toolStripBtn_Stop";
            this.toolStripBtn_Stop.Size = new System.Drawing.Size(68, 69);
            this.toolStripBtn_Stop.Text = "toolStripButton5";
            this.toolStripBtn_Stop.ToolTipText = "停止升级";
            this.toolStripBtn_Stop.Click += new System.EventHandler(this.toolStripBtn_Stop_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 72);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::ReserchDownLoad.Properties.Resources.help2;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(68, 69);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "帮助文档";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // statusStrip_settings
            // 
            this.statusStrip_settings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_Uart,
            this.toolStripStatusLabel_mode});
            this.statusStrip_settings.Location = new System.Drawing.Point(0, 611);
            this.statusStrip_settings.Name = "statusStrip_settings";
            this.statusStrip_settings.Size = new System.Drawing.Size(1068, 22);
            this.statusStrip_settings.TabIndex = 2;
            this.statusStrip_settings.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_Uart
            // 
            this.toolStripStatusLabel_Uart.AutoSize = false;
            this.toolStripStatusLabel_Uart.Name = "toolStripStatusLabel_Uart";
            this.toolStripStatusLabel_Uart.Size = new System.Drawing.Size(600, 17);
            this.toolStripStatusLabel_Uart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel_mode
            // 
            this.toolStripStatusLabel_mode.AutoSize = false;
            this.toolStripStatusLabel_mode.Name = "toolStripStatusLabel_mode";
            this.toolStripStatusLabel_mode.Size = new System.Drawing.Size(350, 17);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(38, 38);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("华文彩云", 42F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(3, 433);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1062, 106);
            this.label1.TabIndex = 3;
            this.label1.Text = "必须先上电，再运行！";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1068, 539);
            this.panel1.TabIndex = 4;
            // 
            // listView1
            // 
            this.listView1.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listView1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader1});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1068, 539);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Device";
            this.columnHeader7.Width = 210;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Step";
            this.columnHeader9.Width = 210;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Progress";
            this.columnHeader10.Width = 480;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Time(s)";
            this.columnHeader11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader11.Width = 85;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Result";
            this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader1.Width = 75;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoEllipsis = true;
            this.label2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Enabled = false;
            this.label2.Font = new System.Drawing.Font("华文彩云", 42F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(3, 327);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1062, 106);
            this.label2.TabIndex = 4;
            this.label2.Text = "禁止接入充电器";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 633);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip_settings);
            this.Controls.Add(this.toolStrip_Stop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "串口升级工具";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_Closed);
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.toolStrip_Stop.ResumeLayout(false);
            this.toolStrip_Stop.PerformLayout();
            this.statusStrip_settings.ResumeLayout(false);
            this.statusStrip_settings.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip_Stop;
        private System.Windows.Forms.ToolStripButton toolStripBtn_File;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripBtn_Start;
        private System.Windows.Forms.ToolStripButton toolStripBtn_Uart;
        private System.Windows.Forms.StatusStrip statusStrip_settings;
        private ListViewEx listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Uart;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_mode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripBtn_Stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Label label2;
    }
}

