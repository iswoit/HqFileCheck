using System;
using System.Collections.Generic;
using System.Text;

namespace HqFileCheck
{
    public class HqFile
    {
        private string _module;                 // 模块
        private string _desc;                   // 描述
        private string _path;                   // 文件路径
        private bool _required;               // 是否必须
        private DateTime? _startTime;      // 开始检查时间

        private bool _isRunning;                // 是否正在检查
        private bool _isOK;                     // 是否就绪
        private Status _status;                 // 状态

        public HqFile(string module, string desc, string path, string isRequired, string startTime, DateTime dtNow)
        {
            _module = module;
            _desc = desc;
            _path = Util.ReplaceStringWithDateFormat(path, dtNow);
            if (!bool.TryParse(isRequired, out _required))
                _required = true;

            DateTime tmpDT;
            if (DateTime.TryParse(startTime, out tmpDT))
                _startTime = tmpDT;

            _isRunning = false;
            _isOK = false;
            _status = Status.未开始;
        }



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
        文件已就绪 = 2,
        文件非当日文件 = 3
    }
}
