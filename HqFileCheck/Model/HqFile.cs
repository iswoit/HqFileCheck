using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Text;

namespace HqFileCheck
{
    public class HqFile
    {
        #region 变量
        private string _module;                 // 模块
        private string _desc;                   // 描述
        private string _path;                   // 文件路径
        private bool _required;                 // 是否检查
        private DateTime? _startTime;           // 开始检查时间
        private string _extraType;               // 额外检查-文件类型
        private string _extraFormat;             // 额外检查-字段位置

        private bool _isRunning;                // 是否正在检查
        private bool _isOK;                     // 是否就绪
        private Status _status;                 // 状态
        #endregion 变量



        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="module"></param>
        /// <param name="desc"></param>
        /// <param name="path"></param>
        /// <param name="startTime"></param>
        /// <param name="dtNow"></param>
        /// <param name="extraType"></param>
        /// <param name="extraFormat"></param>
        public HqFile(string module, string desc, string path, string startTime, DateTime dtNow, string extraType, string extraFormat)
        {
            _module = module;
            _desc = desc;
            _path = Util.ReplaceStringWithDateFormat(path, dtNow);
            _required = true;

            DateTime tmpDT;
            if (DateTime.TryParse(startTime, out tmpDT))
                _startTime = tmpDT;

            _extraType = extraType;
            _extraFormat = extraFormat;

            _isRunning = false;
            _isOK = false;
            _status = Status.未开始;
        }


        /// <summary>
        /// 判断行情文件是否是今天的文件
        /// </summary>
        /// <param name="expetedDate">程序运行时间</param>
        /// <param name="fileDate">文件的时间，返回值</param>
        /// <returns></returns>
        public bool IsHqFileToday(DateTime expetedDate, out DateTime fileDate)
        {
            fileDate = DateTime.Now;
            switch (ExtraType)  // 目前只判断txt和dbf
            {
                case "":
                    return true;
                case "txt":
                    {
                        // 分解extraFormat
                        string[] strExtraFormat = ExtraFormat.Trim().Split(new char[] { ',', '，', '|' });
                        if (strExtraFormat.Length != 3)
                            throw new Exception(string.Format(@"[{0}][{1}] 额外参数:{2} 配置文件格式不正确(txt文件格式为:行号,起始字符串,长度)! 无法判断是否是当天文件!", Module, Path, ExtraFormat));
                        int lineIdx = 0;
                        int spanCnt = 0;
                        int lengthCnt = 0;
                        if (!int.TryParse(strExtraFormat[0].Trim(), out lineIdx))
                            throw new Exception(string.Format(@"[{0}][{1}] 额外参数:{2} 配置文件格式不正确(txt文件格式为:行号,起始字符串,长度)! 无法判断是否是当天文件!", Module, Path, ExtraFormat));
                        if (!int.TryParse(strExtraFormat[1].Trim(), out spanCnt))
                            throw new Exception(string.Format(@"[{0}][{1}] 额外参数:{2} 配置文件格式不正确(txt文件格式为:行号,起始字符串,长度)! 无法判断是否是当天文件!", Module, Path, ExtraFormat));
                        if (!int.TryParse(strExtraFormat[2].Trim(), out lengthCnt))
                            throw new Exception(string.Format(@"[{0}][{1}] 额外参数:{2} 配置文件格式不正确(txt文件格式为:行号,起始字符串,长度)! 无法判断是否是当天文件!", Module, Path, ExtraFormat));

                        using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (StreamReader sr = new StreamReader(fs))
                            {
                                int curLineIdx = 0;                        // 当前行号
                                string strContent = string.Empty;   // 行内容

                                while ((strContent = sr.ReadLine()) != null)
                                {
                                    if (curLineIdx != lineIdx - 1)  // 不是判断行号，跳过
                                    {
                                        curLineIdx++;
                                        continue;
                                    }

                                    string strDate = strContent.Substring(spanCnt - 1, lengthCnt);
                                    DateTime dtDate;
                                    if (!DateTime.TryParseExact(strDate, "yyyyMMdd", new CultureInfo("zh-CN", true), DateTimeStyles.None, out dtDate))
                                        throw new Exception(string.Format(@"[{0}][{1}] 第{2}行字符串({3}-{4})({5})并非日期字段! 无法判断是否是当天文件!", Module, Path, lineIdx, spanCnt, spanCnt + lengthCnt, strDate));


                                    fileDate = dtDate;
                                    if (dtDate.Date == expetedDate.Date)  // 如果时间相同，返回true
                                        return true;
                                    else
                                        return false;
                                }

                                sr.Close();
                            }//eof sr
                            fs.Close();
                        }//eof fs

                        throw new Exception(string.Format(@"[{0}][{1}] 文件不够{2}行! 无法判断是否是当天文件!", Module, Path, lineIdx));

                        break;
                    }
                case "dbf":
                    {
                        // 分解extraFormat
                        string[] strExtraFormat = ExtraFormat.Trim().Split(new char[] { ',', '，', '|' });
                        if (strExtraFormat.Length != 2)
                            throw new Exception(string.Format(@"[{0}][{1}] 额外参数:{2} 配置文件格式不正确(dbf文件格式为:行号,列名)! 无法判断是否是当天文件!", Module, Path, ExtraFormat));
                        int lineIdx = 0;
                        string colName = strExtraFormat[1].Trim();
                        if (!int.TryParse(strExtraFormat[0].Trim(), out lineIdx))
                            throw new Exception(string.Format(@"[{0}][{1}] 额外参数:{2} 配置文件格式不正确(dbf文件格式为:行号,列名)! 无法判断是否是当天文件!", Module, Path, ExtraFormat));

                        int curLineIdx = 0;                        // 当前行号
                        using (OleDbCommand cmd = new OleDbCommand())
                        {
                            using (OleDbConnection conn = new OleDbConnection())
                            {
                                string connStr = string.Format(@"Provider=VFPOLEDB.1;Data Source={0};Collating Sequence=MACHINE", Path);

                                cmd.CommandText = string.Format(@"select * from {0}", Path);
                                conn.ConnectionString = connStr;
                                conn.Open();
                                cmd.Connection = conn;

                                OleDbDataReader dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    while (dr.Read())
                                    {
                                        if (curLineIdx != lineIdx - 1)
                                        {
                                            curLineIdx++;
                                            continue;
                                        }

                                        string strDate = dr[colName].ToString().Trim();
                                        DateTime dtDate;
                                        if (!DateTime.TryParseExact(strDate, "yyyyMMdd", new CultureInfo("zh-CN", true), DateTimeStyles.None, out dtDate))
                                            throw new Exception(string.Format(@"[{0}][{1}] 第{2}行列{3}({4})并非日期字段! 无法判断是否是当天文件!", Module, Path, lineIdx, colName, strDate));


                                        fileDate = dtDate;
                                        if (dtDate.Date == expetedDate.Date)  // 如果时间相同，返回true
                                            return true;
                                        else
                                            return false;
                                    }
                                }
                                else
                                {
                                    throw new Exception(string.Format(@"[{0}][{1}] 文件没有记录! 无法判断是否是当天文件!", Module, Path, ExtraFormat));
                                }
                            }
                        }

                        throw new Exception(string.Format(@"[{0}][{1}] 文件不够{2}行! 无法判断是否是当天文件!", Module, Path, lineIdx));
                        //OleDbDataAdapter da = new OleDbDataAdapter(sql, conn);
                        //DataTable dt = new DataTable();
                        //da.Fill(dt);


                        break;
                    }
            }

            return false;
        }

        #endregion


        #region 属性

        public string Module
        {
            get { return _module; }
        }

        public string Desc
        {
            get { return _desc; }
        }

        public string Path
        {
            get { return _path; }
        }

        public bool Required
        {
            get { return _required; }
            set { _required = value; }
        }

        public DateTime? StartTime
        {
            get { return _startTime; }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }

        public bool IsOK
        {
            get { return _isOK; }
            set { _isOK = value; }
        }

        public Status Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string ExtraType
        {
            get { return _extraType; }
        }

        public string ExtraFormat
        {
            get { return _extraFormat; }
        }

        #endregion 属性

    }


    /// <summary>
    /// 文件检查状态
    /// </summary>
    public enum Status
    {
        异常 = -1,
        未开始 = 0,
        文件不存在 = 1,
        文件不存在_时间点未到 = 2,
        文件不存在_非交易日 = 3,
        文件已就绪 = 4,
        文件非当日文件 = 5
    }
}
