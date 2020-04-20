namespace GlscEBMProject.UC
{
    partial class AreaScan
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbYdirec = new System.Windows.Forms.RadioButton();
            this.rbXdirec = new System.Windows.Forms.RadioButton();
            this.txtVolt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFocusOffs = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.txtpreHeatFreq = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtpreHeatSpeed = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtLineSize = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txLineOffset = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtLineOrder = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtpreheatCout = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox14.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox14
            // 
            this.groupBox14.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox14.Controls.Add(this.groupBox1);
            this.groupBox14.Controls.Add(this.txtVolt);
            this.groupBox14.Controls.Add(this.label1);
            this.groupBox14.Controls.Add(this.txtFocusOffs);
            this.groupBox14.Controls.Add(this.label2);
            this.groupBox14.Controls.Add(this.btnApply);
            this.groupBox14.Controls.Add(this.txtpreHeatFreq);
            this.groupBox14.Controls.Add(this.label18);
            this.groupBox14.Controls.Add(this.txtpreHeatSpeed);
            this.groupBox14.Controls.Add(this.label19);
            this.groupBox14.Controls.Add(this.txtLineSize);
            this.groupBox14.Controls.Add(this.label16);
            this.groupBox14.Controls.Add(this.txLineOffset);
            this.groupBox14.Controls.Add(this.label17);
            this.groupBox14.Controls.Add(this.txtLineOrder);
            this.groupBox14.Controls.Add(this.label15);
            this.groupBox14.Controls.Add(this.txtpreheatCout);
            this.groupBox14.Controls.Add(this.label14);
            this.groupBox14.Location = new System.Drawing.Point(0, 20);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(447, 334);
            this.groupBox14.TabIndex = 20;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "预热设置";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbYdirec);
            this.groupBox1.Controls.Add(this.rbXdirec);
            this.groupBox1.Location = new System.Drawing.Point(76, 202);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(310, 55);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "预热方向";
            // 
            // rbYdirec
            // 
            this.rbYdirec.AutoSize = true;
            this.rbYdirec.Location = new System.Drawing.Point(146, 19);
            this.rbYdirec.Margin = new System.Windows.Forms.Padding(2);
            this.rbYdirec.Name = "rbYdirec";
            this.rbYdirec.Size = new System.Drawing.Size(65, 16);
            this.rbYdirec.TabIndex = 27;
            this.rbYdirec.Text = "y向预热";
            this.rbYdirec.UseVisualStyleBackColor = true;
            // 
            // rbXdirec
            // 
            this.rbXdirec.AutoSize = true;
            this.rbXdirec.Checked = true;
            this.rbXdirec.Location = new System.Drawing.Point(32, 19);
            this.rbXdirec.Margin = new System.Windows.Forms.Padding(2);
            this.rbXdirec.Name = "rbXdirec";
            this.rbXdirec.Size = new System.Drawing.Size(65, 16);
            this.rbXdirec.TabIndex = 26;
            this.rbXdirec.TabStop = true;
            this.rbXdirec.Text = "x向预热";
            this.rbXdirec.UseVisualStyleBackColor = true;
            // 
            // txtVolt
            // 
            this.txtVolt.Location = new System.Drawing.Point(258, 154);
            this.txtVolt.MaxLength = 5;
            this.txtVolt.Name = "txtVolt";
            this.txtVolt.Size = new System.Drawing.Size(82, 21);
            this.txtVolt.TabIndex = 32;
            this.txtVolt.Text = "32767";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(202, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 31;
            this.label1.Text = "束流:";
            // 
            // txtFocusOffs
            // 
            this.txtFocusOffs.Location = new System.Drawing.Point(106, 154);
            this.txtFocusOffs.MaxLength = 5;
            this.txtFocusOffs.Name = "txtFocusOffs";
            this.txtFocusOffs.Size = new System.Drawing.Size(82, 21);
            this.txtFocusOffs.TabIndex = 30;
            this.txtFocusOffs.Text = "2000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "聚焦补偿:";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(161, 276);
            this.btnApply.Margin = new System.Windows.Forms.Padding(2);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(78, 38);
            this.btnApply.TabIndex = 25;
            this.btnApply.Text = "参数应用";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtpreHeatFreq
            // 
            this.txtpreHeatFreq.Location = new System.Drawing.Point(258, 120);
            this.txtpreHeatFreq.MaxLength = 5;
            this.txtpreHeatFreq.Name = "txtpreHeatFreq";
            this.txtpreHeatFreq.Size = new System.Drawing.Size(82, 21);
            this.txtpreHeatFreq.TabIndex = 17;
            this.txtpreHeatFreq.Text = "80000";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(202, 124);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 12);
            this.label18.TabIndex = 16;
            this.label18.Text = "频率:";
            // 
            // txtpreHeatSpeed
            // 
            this.txtpreHeatSpeed.Location = new System.Drawing.Point(106, 120);
            this.txtpreHeatSpeed.MaxLength = 6;
            this.txtpreHeatSpeed.Name = "txtpreHeatSpeed";
            this.txtpreHeatSpeed.Size = new System.Drawing.Size(82, 21);
            this.txtpreHeatSpeed.TabIndex = 15;
            this.txtpreHeatSpeed.Text = "80000";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(55, 124);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(35, 12);
            this.label19.TabIndex = 14;
            this.label19.Text = "速度:";
            // 
            // txtLineSize
            // 
            this.txtLineSize.Location = new System.Drawing.Point(258, 85);
            this.txtLineSize.MaxLength = 5;
            this.txtLineSize.Name = "txtLineSize";
            this.txtLineSize.Size = new System.Drawing.Size(82, 21);
            this.txtLineSize.TabIndex = 13;
            this.txtLineSize.Text = "40000";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(202, 88);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 12);
            this.label16.TabIndex = 12;
            this.label16.Text = "区域大小:";
            // 
            // txLineOffset
            // 
            this.txLineOffset.Location = new System.Drawing.Point(106, 83);
            this.txLineOffset.MaxLength = 5;
            this.txLineOffset.Name = "txLineOffset";
            this.txLineOffset.Size = new System.Drawing.Size(82, 21);
            this.txLineOffset.TabIndex = 11;
            this.txLineOffset.Text = "200";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(46, 88);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 12);
            this.label17.TabIndex = 10;
            this.label17.Text = "扫描间隔:";
            // 
            // txtLineOrder
            // 
            this.txtLineOrder.Location = new System.Drawing.Point(274, 45);
            this.txtLineOrder.MaxLength = 5;
            this.txtLineOrder.Name = "txtLineOrder";
            this.txtLineOrder.Size = new System.Drawing.Size(66, 21);
            this.txtLineOrder.TabIndex = 9;
            this.txtLineOrder.Text = "100";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(202, 47);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 12);
            this.label15.TabIndex = 8;
            this.label15.Text = "分割线数目:";
            // 
            // txtpreheatCout
            // 
            this.txtpreheatCout.Location = new System.Drawing.Point(106, 45);
            this.txtpreheatCout.MaxLength = 5;
            this.txtpreheatCout.Name = "txtpreheatCout";
            this.txtpreheatCout.Size = new System.Drawing.Size(81, 21);
            this.txtpreheatCout.TabIndex = 7;
            this.txtpreheatCout.Text = "20";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(45, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 12);
            this.label14.TabIndex = 6;
            this.label14.Text = "预热次数:";
            // 
            // AreaScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Controls.Add(this.groupBox14);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AreaScan";
            this.Size = new System.Drawing.Size(450, 380);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.TextBox txtpreHeatFreq;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtpreHeatSpeed;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtLineSize;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txLineOffset;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtLineOrder;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtpreheatCout;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbYdirec;
        private System.Windows.Forms.RadioButton rbXdirec;
        private System.Windows.Forms.TextBox txtVolt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFocusOffs;
        private System.Windows.Forms.Label label2;
    }
}
