namespace HqFileCheck
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnCheck = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbIsAllOK = new System.Windows.Forms.Label();
            this.lvHq = new HqFileCheck.DoubleBufferListView();
            this.clNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clModule = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clMarket = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clIsTradingDay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clStartTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clRequired = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clIsOK = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(889, 527);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(98, 30);
            this.btnCheck.TabIndex = 1;
            this.btnCheck.Text = "检查";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDate});
            this.statusStrip.Location = new System.Drawing.Point(0, 592);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1012, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripDate
            // 
            this.toolStripDate.Name = "toolStripDate";
            this.toolStripDate.Size = new System.Drawing.Size(142, 17);
            this.toolStripDate.Text = "当前日期：yyyy-MM-dd";
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorker_ProgressChanged);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.SystemColors.Window;
            this.tbLog.Location = new System.Drawing.Point(4, 480);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(859, 103);
            this.tbLog.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 465);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "日志:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "文件列表:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(872, 498);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "是否已就绪:";
            // 
            // lbIsAllOK
            // 
            this.lbIsAllOK.AutoSize = true;
            this.lbIsAllOK.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbIsAllOK.Location = new System.Drawing.Point(963, 495);
            this.lbIsAllOK.Name = "lbIsAllOK";
            this.lbIsAllOK.Size = new System.Drawing.Size(35, 16);
            this.lbIsAllOK.TabIndex = 7;
            this.lbIsAllOK.Text = "N/A";
            // 
            // lvHq
            // 
            this.lvHq.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clNo,
            this.clModule,
            this.clDesc,
            this.clPath,
            this.clMarket,
            this.clIsTradingDay,
            this.clStartTime,
            this.clRequired,
            this.clIsOK,
            this.clStatus});
            this.lvHq.FullRowSelect = true;
            this.lvHq.GridLines = true;
            this.lvHq.Location = new System.Drawing.Point(4, 24);
            this.lvHq.Name = "lvHq";
            this.lvHq.Size = new System.Drawing.Size(1003, 424);
            this.lvHq.TabIndex = 0;
            this.lvHq.UseCompatibleStateImageBehavior = false;
            this.lvHq.View = System.Windows.Forms.View.Details;
            // 
            // clNo
            // 
            this.clNo.Text = "No";
            this.clNo.Width = 53;
            // 
            // clModule
            // 
            this.clModule.Text = "模块";
            // 
            // clDesc
            // 
            this.clDesc.Text = "文件说明";
            this.clDesc.Width = 65;
            // 
            // clPath
            // 
            this.clPath.Text = "文件路径";
            this.clPath.Width = 212;
            // 
            // clMarket
            // 
            this.clMarket.Text = "市场";
            this.clMarket.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clMarket.Width = 40;
            // 
            // clIsTradingDay
            // 
            this.clIsTradingDay.Text = "是否交易日";
            this.clIsTradingDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clIsTradingDay.Width = 75;
            // 
            // clStartTime
            // 
            this.clStartTime.Text = "检查开始时间";
            this.clStartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clStartTime.Width = 85;
            // 
            // clRequired
            // 
            this.clRequired.Text = "是否检查";
            this.clRequired.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // clIsOK
            // 
            this.clIsOK.Text = "是否就绪";
            this.clIsOK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // clStatus
            // 
            this.clStatus.Text = "说明";
            this.clStatus.Width = 215;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 614);
            this.Controls.Add(this.lbIsAllOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.lvHq);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "行情文件检查";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferListView lvHq;
        private System.Windows.Forms.ColumnHeader clNo;
        private System.Windows.Forms.ColumnHeader clModule;
        private System.Windows.Forms.ColumnHeader clDesc;
        private System.Windows.Forms.ColumnHeader clPath;
        private System.Windows.Forms.ColumnHeader clRequired;
        private System.Windows.Forms.ColumnHeader clStartTime;
        private System.Windows.Forms.ColumnHeader clIsOK;
        private System.Windows.Forms.ColumnHeader clStatus;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripDate;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbIsAllOK;
        private System.Windows.Forms.ColumnHeader clIsTradingDay;
        private System.Windows.Forms.ColumnHeader clMarket;
    }
}

