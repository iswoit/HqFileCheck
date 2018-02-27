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
                // 1.生成manager对象
                _manager = Manager.GetInstance();

                // 2.数据库查询交易日，更新HqFile的是否交易
                try
                {
                    _manager.UpdateMarketStatus();
                    string strMessage = "获取数据库交易日完成";
                    Print_Message(strMessage);
                }
                catch (Exception ex)
                {
                    Print_Message("获取数据库交易日失败(默认当天为交易日). 原因: " + ex.Message);
                }
                InitLvHq();

                toolStripDate.Text = string.Format("当前日期：{0}", _manager.DtNow.ToString("yyyy-MM-dd"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + System.Environment.NewLine + "请确认配置文件正确后重启程序!");
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
                lvi.SubItems.Add(tmpHqFile.Market);
                lvi.SubItems.Add(tmpHqFile.IsTradingDay ? "√" : "×");
                lvi.SubItems.Add(tmpHqFile.StartTime.HasValue ? tmpHqFile.StartTime.Value.ToString("HH:mm:ss") : "任意");
                lvi.SubItems.Add(tmpHqFile.Required ? "√" : "×");
                lvi.SubItems.Add(tmpHqFile.IsOK ? "√" : "×");
                lvi.SubItems.Add(tmpHqFile.Status.ToString());
                lvi.Tag = tmpHqFile;

                lvHq.Items.Add(lvi);

                if (tmpHqFile.Required)
                {
                    if (tmpHqFile.IsOK)
                        lvHq.Items[i].BackColor = SystemColors.Window;
                    else
                        lvHq.Items[i].BackColor = Color.Pink;
                }
                else
                {
                    if (tmpHqFile.IsOK)
                        lvHq.Items[i].BackColor = SystemColors.Window;
                    else
                        lvHq.Items[i].BackColor = Color.LightYellow;
                }

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
                    lvHq.Items[i].SubItems[7].Text = tmpHqFile.Required ? "√" : "×";
                    lvHq.Items[i].SubItems[8].Text = tmpHqFile.IsOK ? "√" : "×";              // 标志到齐
                    lvHq.Items[i].SubItems[9].Text = tmpHqFile.Status.ToString();             // 状态


                    if (tmpHqFile.IsRunning)
                    {
                        lvHq.Items[i].BackColor = Color.LightBlue;
                        lvHq.Items[i].EnsureVisible();
                    }
                    else
                    {
                        if (tmpHqFile.Required)
                        {
                            if (tmpHqFile.IsOK)
                                lvHq.Items[i].BackColor = SystemColors.Window;
                            else
                                lvHq.Items[i].BackColor = Color.Pink;
                        }
                        else
                        {
                            if (tmpHqFile.IsOK)
                                lvHq.Items[i].BackColor = SystemColors.Window;
                            else
                                lvHq.Items[i].BackColor = Color.LightYellow;
                        }
                    }
                }


                if (_manager.IsAllOK)
                {
                    lbIsAllOK.Text = "是";
                    lbIsAllOK.ForeColor = Color.Green;
                }
                else
                {
                    lbIsAllOK.Text = "否";
                    lbIsAllOK.ForeColor = Color.Red;
                }
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
            try
            {
                BackgroundWorker bgWorker = sender as BackgroundWorker;

                UserState us;   // 日志输出对象
                us = new UserState(true, @"****检查开始****");
                bgWorker.ReportProgress(1, us);

                foreach (HqFile tmpHqFile in _manager.ListHqFile)
                {
                    // 取消事件处理
                    if (true == bgWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }


                    tmpHqFile.IsRunning = true;
                    bgWorker.ReportProgress(1);
                    Thread.Sleep(50);


                    // 如果项有时间，更新是否检查标志
                    if (tmpHqFile.IsTradingDay == true)
                    {
                        if (tmpHqFile.StartTime.HasValue)
                        {
                            DateTime dtNow = DateTime.Now;
                            DateTime dtTmp = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, tmpHqFile.StartTime.Value.Hour, tmpHqFile.StartTime.Value.Minute, tmpHqFile.StartTime.Value.Second);
                            if (dtNow >= dtTmp)
                            {
                                tmpHqFile.Required = true;
                            }
                            else
                            {
                                tmpHqFile.Required = false;
                            }
                        }
                        else
                            tmpHqFile.Required = true;
                    }
                    else
                        tmpHqFile.Required = false;


                    // 判断文件是否存在
                    if (File.Exists(tmpHqFile.Path))
                    {
                        tmpHqFile.Status = Status.文件已就绪;
                        tmpHqFile.IsOK = true;

                        bgWorker.ReportProgress(1);
                    }
                    else
                    {
                        if (tmpHqFile.IsTradingDay == true)
                        {
                            if (tmpHqFile.Required == true)
                                tmpHqFile.Status = Status.文件不存在;
                            else
                                tmpHqFile.Status = Status.文件不存在_时间点未到;
                        }
                        else
                            tmpHqFile.Status = Status.文件不存在_非交易日;



                        tmpHqFile.IsOK = false;
                        tmpHqFile.IsRunning = false;

                        bgWorker.ReportProgress(1);
                        continue;
                    }


                    /* 额外检查项
                     * 文件内容中的日期是否是当天（针对文件名没有日期标志的，分为txt和dbf）
                     * txt：第几行，从第几个字符开始，截取几个字符。日期格式
                     * dbf：第几行，哪一列。日期格式
                     */

                    try
                    {
                        DateTime dtFile;
                        bool isFileCurDate = tmpHqFile.IsHqFileToday(_manager.DtNow, out dtFile);
                        if (isFileCurDate)
                        {
                            tmpHqFile.Status = Status.文件已就绪;
                            tmpHqFile.IsOK = true;

                            bgWorker.ReportProgress(1);
                        }
                        else
                        {
                            if (tmpHqFile.IsTradingDay == true)
                            {
                                tmpHqFile.Status = Status.文件非当日文件;

                                tmpHqFile.IsOK = false;
                                us = new UserState(true, string.Format(@"[{0}][{1}] 文件日期为{2}, 并非当天文件!", tmpHqFile.Module, tmpHqFile.Path, dtFile.ToString("yyyy-MM-dd")));
                                bgWorker.ReportProgress(1, us);
                            }
                            else
                            {
                                tmpHqFile.IsOK = false;
                                tmpHqFile.Status = Status.文件不存在_非交易日;
                                bgWorker.ReportProgress(1);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tmpHqFile.Status = Status.异常;
                        tmpHqFile.IsOK = false;
                        tmpHqFile.IsRunning = false;

                        us = new UserState(true, ex.Message);
                        bgWorker.ReportProgress(1, us);
                        continue;
                    }



                    // 结束
                    tmpHqFile.IsRunning = false;
                    bgWorker.ReportProgress(1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            if (e.Error != null)    // 未处理的异常，需要弹框
            {
                Print_Message(e.Error.Message);

                //lbStatus.Text = "异常停止";
                //lbStatus.BackColor = Color.Red;
            }
            else if (e.Cancelled)
            {
                Print_Message("任务被手工取消");

                //lbStatus.Text = "手工停止";
                //lbStatus.BackColor = Color.Red;

                // 状态清空
                for (int i = 0; i < _manager.ListHqFile.Count; i++)
                {
                    _manager.ListHqFile[i].IsRunning = false;
                }
            }
            else
            {
                UpdateLvHq();
                Print_Message(string.Format(@"****检查完毕 [文件进度: 所有文件({2}/{3}), 必检文件({0}/{1}), 是否已就绪: {4}]****", _manager.GetFinishedRequiredCnt, _manager.GetAllRequiredCnt, _manager.GetFinishedCnt, _manager.GetAllCnt, _manager.IsAllOK ? "是" : "否"));
                //UpdateFileSourceInfo();
                //UpdateFileListInfo();

                //// 处理状态标签
                //lbStatus.Text = "完成，等待下一轮";
                //lbStatus.BackColor = Color.ForestGreen;
            }

            //lbIsHqRunning.Text = "运行完毕";

            //btnHqExecute.Text = "检查";
        }


        private void Print_Message(string message)
        {
            tbLog.Text = string.Format("{0}:{1}", DateTime.Now.ToString("HH:mm:ss"), message) + System.Environment.NewLine + tbLog.Text;
        }
    }
}
