using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

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
            lvHq.Columns[3].Width = -1;

            lvHq.EndUpdate();
        }


        private void UpdateLvHq()
        {
            lvHq.BeginUpdate();
            // 进度列表
            try
            {
                for (int i = 0; i < _manager.ListHqFile.Count; i++)
                {
                    HqFile tmpHqFile = (HqFile)lvHq.Items[i].Tag;   // 配置对象
                    lvHq.Items[i].SubItems[6].Text = tmpHqFile.IsOK ? "√" : "×";              // 标志到齐

                    lvHq.Items[i].SubItems[7].Text = tmpHqFile.Status.ToString();             // 状态


                    if (tmpHqFile.IsRunning)
                    {
                        lvHq.Items[i].BackColor = Color.LightBlue;
                        lvHq.Items[i].EnsureVisible();
                    }
                    else
                    {
                        lvHq.Items[i].BackColor = SystemColors.Window;
                    }
                }

                //lbIsHqAllOK.Text = _taManager.IsHqAllOK ? "是" : "否";
            }
            catch (Exception)
            {
                // ui异常过滤
            }

            lvHq.EndUpdate();
        }


        /// <summary>
        /// 检查按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (!bgWorker.IsBusy)
            {
                bgWorker.RunWorkerAsync();
            }
            else
            {
                bgWorker.CancelAsync();
            }
        }


        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgWorker = sender as BackgroundWorker;

            foreach (HqFile tmpHqFile in _manager.ListHqFile)
            {
                // 取消事件处理
                if (true == bgWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }


                // 判断文件是否存在
                tmpHqFile.IsRunning = true;
                bgWorker.ReportProgress(1);
                Thread.Sleep(50);

                if (File.Exists(tmpHqFile.Path))
                {
                    tmpHqFile.Status = Status.文件已就绪;
                    tmpHqFile.IsOK = true;

                    bgWorker.ReportProgress(1);
                }
                else
                {
                    tmpHqFile.Status = Status.文件不存在;
                    tmpHqFile.IsOK = false;
                    tmpHqFile.IsRunning = false;

                    bgWorker.ReportProgress(1);
                    continue;
                }


                // 结束
                tmpHqFile.IsRunning = false;
                bgWorker.ReportProgress(1);
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // 如果有错误日志，输出
            if (e.UserState != null)
            {
                UserState us = (UserState)e.UserState;
                if (us.HasError)
                    Print_Message(us.ErrorMsg);
            }

            // 更新listView
            try
            {
                UpdateLvHq();
            }
            catch (Exception ex)
            {
                Print_Message(ex.Message);
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }


        private void Print_Message(string message)
        {
            tbLog.Text = string.Format("{0}:{1}", DateTime.Now.ToString("HH:mm:ss"), message) + System.Environment.NewLine + tbLog.Text;
        }
    }
}
