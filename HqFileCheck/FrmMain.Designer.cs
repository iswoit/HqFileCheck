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
            this.lvHq = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvHq
            // 
            this.lvHq.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.lvHq.GridLines = true;
            this.lvHq.Location = new System.Drawing.Point(4, 5);
            this.lvHq.Name = "lvHq";
            this.lvHq.Size = new System.Drawing.Size(777, 293);
            this.lvHq.TabIndex = 0;
            this.lvHq.UseCompatibleStateImageBehavior = false;
            this.lvHq.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "No";
            this.columnHeader1.Width = 53;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "模块";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "文件说明";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "文件路径";
            this.columnHeader4.Width = 212;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "是否必须";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "检查开始时间";
            this.columnHeader6.Width = 85;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "是否就绪";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "说明";
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(706, 309);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 1;
            this.btnCheck.Text = "检查";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 344);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.lvHq);
            this.Name = "FrmMain";
            this.Text = "行情文件检查";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvHq;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Button btnCheck;
    }
}

