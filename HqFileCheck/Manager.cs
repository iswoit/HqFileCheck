using System;
using System.Collections.Generic;
using System.Text;

namespace HqFileCheck
{
    public class Manager
    {
        private List<HqFile> _listHqFile;

        private static Manager instance;

        /// <summary>
        /// 构造函数
        /// </summary>
        private Manager()
        {

        }

        /// <summary>
        /// 单例方法
        /// </summary>
        /// <returns></returns>
        public static Manager GetInstance()
        {
            if (instance == null)
                instance = new Manager();

            return instance;
        }
    }
}
