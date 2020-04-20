namespace GlscEBMProject
{
    partial class MainFm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStriptn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnCADLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbCAD = new System.Windows.Forms.RadioButton();
            this.rbDirec = new System.Windows.Forms.RadioButton();
            this.rbPreheat = new System.Windows.Forms.RadioButton();
            this.btnStoptScan = new System.Windows.Forms.Button();
            this.btnStartScan = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStriptn,
            this.toolStripButton3,
            this.toolStripBtnCADLoad,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripButton5});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(736, 80);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStriptn
            // 
            this.toolStriptn.AutoSize = false;
            this.toolStriptn.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStriptn.Image = global::GlscEBMProject.Properties.Resources.Beam_32px;
            this.toolStriptn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStriptn.Name = "toolStriptn";
            this.toolStriptn.Size = new System.Drawing.Size(100, 100);
            this.toolStriptn.Tag = "ManuScan";
            this.toolStriptn.Text = "直流与校准";
            this.toolStriptn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStriptn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStriptn.Click += new System.EventHandler(this.toolStriptn_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.AutoSize = false;
            this.toolStripButton3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripButton3.Image = global::GlscEBMProject.Properties.Resources.BottomLeftVerticalInside_32x321;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(100, 100);
            this.toolStripButton3.Tag = "AreaScan";
            this.toolStripButton3.Text = "预热扫描";
            this.toolStripButton3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton3.Click += new System.EventHandler(this.toolStriptn_Click);
            // 
            // toolStripBtnCADLoad
            // 
            this.toolStripBtnCADLoad.AutoSize = false;
            this.toolStripBtnCADLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripBtnCADLoad.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnCADLoad.Image")));
            this.toolStripBtnCADLoad.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripBtnCADLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnCADLoad.Name = "toolStripBtnCADLoad";
            this.toolStripBtnCADLoad.Size = new System.Drawing.Size(100, 100);
            this.toolStripBtnCADLoad.Tag = "CADScan";
            this.toolStripBtnCADLoad.Text = "CAD扫描";
            this.toolStripBtnCADLoad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripBtnCADLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripBtnCADLoad.Click += new System.EventHandler(this.toolStriptn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 80);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.AutoSize = false;
            this.toolStripButton2.Enabled = false;
            this.toolStripButton2.Image = global::GlscEBMProject.Properties.Resources.Show_32x321;
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(100, 100);
            this.toolStripButton2.Text = "图形显示";
            this.toolStripButton2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.AutoSize = false;
            this.toolStripButton5.Enabled = false;
            this.toolStripButton5.Image = global::GlscEBMProject.Properties.Resources.Properties_32x321;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(100, 100);
            this.toolStripButton5.Text = "参数设置";
            this.toolStripButton5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 520);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(736, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(80, 17);
            this.toolStripStatusLabel1.Text = "桂林实创科技";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(83, 17);
            this.toolStripStatusLabel2.Text = "      信息提示:";
            // 
            // toolStripStatusInfo
            // 
            this.toolStripStatusInfo.BackColor = System.Drawing.Color.Moccasin;
            this.toolStripStatusInfo.Name = "toolStripStatusInfo";
            this.toolStripStatusInfo.Size = new System.Drawing.Size(89, 17);
            this.toolStripStatusInfo.Text = "程序准备完毕...";
            this.toolStripStatusInfo.ToolTipText = "提示信息";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(66, 255);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(130, 63);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 0;
            this.buttonX1.Text = "buttonX1";
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 80);
            this.splitMain.Margin = new System.Windows.Forms.Padding(2);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.groupBox1);
            this.splitMain.Panel2.Padding = new System.Windows.Forms.Padding(2);
            this.splitMain.Size = new System.Drawing.Size(736, 440);
            this.splitMain.SplitterDistance = 449;
            this.splitMain.SplitterWidth = 3;
            this.splitMain.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnStoptScan);
            this.groupBox1.Controls.Add(this.btnStartScan);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(1, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8, 24, 8, 2);
            this.groupBox1.Size = new System.Drawing.Size(281, 436);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输出控制";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbCAD);
            this.groupBox2.Controls.Add(this.rbDirec);
            this.groupBox2.Controls.Add(this.rbPreheat);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(8, 47);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(265, 80);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "模式";
            // 
            // rbCAD
            // 
            this.rbCAD.AutoSize = true;
            this.rbCAD.Location = new System.Drawing.Point(165, 38);
            this.rbCAD.Margin = new System.Windows.Forms.Padding(2);
            this.rbCAD.Name = "rbCAD";
            this.rbCAD.Size = new System.Drawing.Size(57, 24);
            this.rbCAD.TabIndex = 5;
            this.rbCAD.Text = "CAD";
            this.rbCAD.UseVisualStyleBackColor = true;
            // 
            // rbDirec
            // 
            this.rbDirec.AutoSize = true;
            this.rbDirec.Checked = true;
            this.rbDirec.Location = new System.Drawing.Point(27, 38);
            this.rbDirec.Margin = new System.Windows.Forms.Padding(2);
            this.rbDirec.Name = "rbDirec";
            this.rbDirec.Size = new System.Drawing.Size(67, 24);
            this.rbDirec.TabIndex = 3;
            this.rbDirec.TabStop = true;
            this.rbDirec.Text = "直流";
            this.rbDirec.UseVisualStyleBackColor = true;
            // 
            // rbPreheat
            // 
            this.rbPreheat.AutoSize = true;
            this.rbPreheat.Location = new System.Drawing.Point(100, 38);
            this.rbPreheat.Margin = new System.Windows.Forms.Padding(2);
            this.rbPreheat.Name = "rbPreheat";
            this.rbPreheat.Size = new System.Drawing.Size(67, 24);
            this.rbPreheat.TabIndex = 4;
            this.rbPreheat.Text = "预热";
            this.rbPreheat.UseVisualStyleBackColor = true;
            // 
            // btnStoptScan
            // 
            this.btnStoptScan.Image = ((System.Drawing.Image)(resources.GetObject("btnStoptScan.Image")));
            this.btnStoptScan.Location = new System.Drawing.Point(153, 299);
            this.btnStoptScan.Margin = new System.Windows.Forms.Padding(2);
            this.btnStoptScan.Name = "btnStoptScan";
            this.btnStoptScan.Size = new System.Drawing.Size(77, 62);
            this.btnStoptScan.TabIndex = 2;
            this.btnStoptScan.UseVisualStyleBackColor = true;
            this.btnStoptScan.Click += new System.EventHandler(this.btnStoptScan_Click);
            // 
            // btnStartScan
            // 
            this.btnStartScan.Image = ((System.Drawing.Image)(resources.GetObject("btnStartScan.Image")));
            this.btnStartScan.Location = new System.Drawing.Point(34, 299);
            this.btnStartScan.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartScan.Name = "btnStartScan";
            this.btnStartScan.Size = new System.Drawing.Size(77, 62);
            this.btnStartScan.TabIndex = 0;
            this.btnStartScan.UseVisualStyleBackColor = true;
            this.btnStartScan.Click += new System.EventHandler(this.btnStartScan_Click);
            // 
            // MainFm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(736, 542);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainFm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "电子束控制程序";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripBtnCADLoad;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStriptn;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Button btnStoptScan;
        private System.Windows.Forms.Button btnStartScan;
        private System.Windows.Forms.RadioButton rbCAD;
        private System.Windows.Forms.RadioButton rbPreheat;
        private System.Windows.Forms.RadioButton rbDirec;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusInfo;
    }
}

