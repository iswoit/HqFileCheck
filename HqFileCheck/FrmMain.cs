using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HqFileCheck
{
    public partial class FrmMain : Form
    {
        Manager _manager;
        public FrmMain()
        {
            InitializeComponent();

            MaximizeBox = false;
           

            // 加载配置文件
            try
            {
                _manager = Manager.GetInstance();
                InitLvHq();

                toolStripDate.Text = string.Format("当前日期：{0}", _manager.DtNow.ToString("yyyy-MM-dd"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitLvHq()
        {
            lvHq.Items.Clear();
            lvHq.BeginUpdate();

            int i = 0;
            foreach (HqFile tmpHqFile in _manager.ListHqFile)
            {
                ListViewItem lvi = new ListViewItem((i + 1).ToString());
                lvi.SubItems.Add(tmpHqFile.Module);
                lvi.SubItems.Add(tmpHqFile.Desc);
                lvi.SubItems.Add(tmpHqFile.Path);
                lvi.SubItems.Add(tmpHqFile.Required ? "√" : "×");
                lvi.SubItems.Add(tmpHqFile.StartTime.HasValue ? tmpHqFile.StartTime.Value.ToString("HH:mm:ss") : string.Empty);
                lvi.SubItems.Add(tmpHqFile.IsOK ? "√" : "×");
                lvi.SubItems.Add(tmpHqFile.Status.ToString());
                lvi.Tag = tmpHqFile;

                lvHq.Items.Add(lvi);

                i++;
            }

            lvHq.Columns[0].Width = -1;
            lvHq.Columns[1].Width = -1;
            lvHq.Columns[2].Width = -1;

            lvHq.EndUpdate();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {

        }
    }
}
