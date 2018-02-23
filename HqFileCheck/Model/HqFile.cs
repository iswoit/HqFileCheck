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
        private bool _isRequired;               // 是否必须
        private DateTime? _startCheckTime;      // 开始检查时间
        private bool _isOK;                     // 是否就绪
        private Status _status;                 // 状态

        public HqFile(string module, string desc, string path, string isRequired, DateTime? startCheckTime)
        {
            _module = module;
            _desc = desc;
            _path = path;
            if (!bool.TryParse(isRequired, out _isRequired))
                _isRequired = true;
            _startCheckTime = startCheckTime;
            _isOK = false;
            _status = Status.未开始;
        }

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
