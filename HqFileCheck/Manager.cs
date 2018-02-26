using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace HqFileCheck
{
    public class Manager
    {
        DateTime _dtNow;                 // 启动程序的时间
        private List<HqFile> _listHqFile;   // 文件列表


        private static Manager instance;

        /// <summary>
        /// 构造函数
        /// </summary>
        private Manager()
        {
            _dtNow = DateTime.Now;
            _listHqFile = new List<HqFile>();

            // 判断配置文件是否存在，不存在抛出异常
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "cfg.xml")))
                throw new Exception("未能找到配置文件cfg.xml，请重新配置该文件后重启程序!");

            // 读取配置文件
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;     //忽略文档里面的注释
            using (XmlReader reader = XmlReader.Create(@"cfg.xml", settings))
            {
                doc.Load(reader);

                // 检查filelist节点
                XmlNode rootNode = doc.SelectSingleNode("filelist");   // 根节点
                if (rootNode == null)
                    throw new Exception("无法找到配置文件节点<filelist>，请检查配置文件格式是否正确!");


                // 直接找filelist子节点，形成HqFile对象，加入列表
                foreach (XmlNode xnChild in rootNode.ChildNodes) // 循环talist下的子节点
                {
                    if (string.Equals(xnChild.Name.Trim().ToLower(), "file", StringComparison.CurrentCultureIgnoreCase))   // <file>节点
                    {
                        // 临时变量
                        string module = string.Empty;                                   // ta代码
                        string desc = string.Empty;                                 // 备注（仅仅显示）
                        string path = string.Empty;
                        string startTime = string.Empty;
                        string extraType = string.Empty;
                        string extraFormat = string.Empty;


                        foreach (XmlNode xnChildAttr in xnChild)
                        {
                            string tmpKey = xnChildAttr.Name.ToLower().Trim();
                            string tmpValue = xnChildAttr.InnerText.Trim();
                            switch (tmpKey)
                            {
                                case "module":
                                    module = tmpValue;
                                    break;
                                case "desc":
                                    desc = tmpValue;
                                    break;
                                case "path":
                                    path = tmpValue;
                                    break;
                                case "starttime":
                                    startTime = tmpValue;
                                    break;
                                case "extra":
                                    {
                                        foreach (XmlNode xnChildChildAttr in xnChildAttr)
                                        {
                                            string tmpChildKey = xnChildChildAttr.Name.ToLower().Trim();
                                            string tmpChildValue = xnChildChildAttr.InnerText.Trim();
                                            switch (tmpChildKey)
                                            {
                                                case "type":
                                                    extraType = tmpChildValue;
                                                    break;
                                                case "format":
                                                    extraFormat = tmpChildValue;
                                                    break;
                                            }

                                        }
                                        break;
                                    }
                            }//eof switch
                        }//eof foreach


                        // 生成对象，加入列表
                        _listHqFile.Add(new HqFile(module, desc, path, startTime, DtNow, extraType, extraFormat));

                    }//eof if ta
                }//eof foreach
            }//eof using
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


        #region 属性

        public DateTime DtNow
        {
            get { return _dtNow; }
        }

        /// <summary>
        /// 行情文件列表
        /// </summary>
        public List<HqFile> ListHqFile
        {
            get { return _listHqFile; }
        }


        public int GetAllRequiredCnt
        {
            get
            {
                int ret = 0;
                foreach (HqFile tmpHqFile in _listHqFile)
                {
                    if (tmpHqFile.Required)
                        ret++;
                }

                return ret;
            }
        }


        public int GetFinishedRequiredCnt
        {
            get
            {
                int ret = 0;
                foreach (HqFile tmpHqFile in _listHqFile)
                {
                    if (tmpHqFile.Required && tmpHqFile.IsOK)
                        ret++;
                }

                return ret;
            }
        }


        public int GetAllCnt
        {
            get
            {
                return _listHqFile.Count;
            }
        }

        public int GetFinishedCnt
        {
            get
            {
                int ret = 0;
                foreach (HqFile tmpHqFile in _listHqFile)
                {
                    if (tmpHqFile.IsOK)
                        ret++;
                }

                return ret;
            }
        }

        /// <summary>
        /// 所有行情文件都就绪
        /// </summary>
        public bool IsAllOK
        {
            get
            {
                foreach (HqFile tmpHqFile in _listHqFile)
                {
                    if (tmpHqFile.Required)
                    {
                        if (!tmpHqFile.IsOK)
                            return false;
                    }
                }

                return true;
            }
        }

        #endregion 属性
    }
}
