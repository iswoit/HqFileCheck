using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Data.OracleClient;
using System.Windows.Forms;

namespace HqFileCheck
{
    public class Manager
    {
        private static Manager instance;                                            // 单例模式对象

        DateTime _dtNow;                                                            // 启动程序的时间(程序左下角也如此)

        private string ip = string.Empty;
        private string port = string.Empty;
        private string service = string.Empty;
        private string acc = string.Empty;
        private string pwd = string.Empty;
        private string table = string.Empty;
        private string coldate = string.Empty;
        private string colmarket = string.Empty;
        private string colstatus = string.Empty;
        private bool _marketStatusAcquired = false;                                         // 市场状态是否获取完成
        private Dictionary<string, bool> dicMarketStatus = new Dictionary<string, bool>();  // 市场交易状态(通过oracle查恒生数据库获取)

        private List<HqFile> _listHqFile = new List<HqFile>();                      // 文件检查列表


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

                // 检查根节点
                XmlNode rootNode = doc.SelectSingleNode("config");   // 根节点
                if (rootNode == null)
                    throw new Exception("无法找到根配置节点<config>，请检查配置文件格式是否正确!");


                #region 交易日数据库获取(这里只负责获取配置，真正的读记录放在后面)

                // 检查marketday节点
                XmlNode marketDayNode = rootNode.SelectSingleNode("//marketday");
                if (marketDayNode == null)
                    throw new Exception("无法找到文件列表节点<marketday>，请检查配置文件格式是否正确!");

                _marketStatusAcquired = false;
                dicMarketStatus = new Dictionary<string, bool>();

                foreach (XmlNode xnFile in marketDayNode.ChildNodes) // 循环talist下的子节点
                {
                    string tmpKey = xnFile.Name.ToLower().Trim();
                    string tmpValue = xnFile.InnerText.Trim();
                    switch (tmpKey)
                    {
                        case "ip":
                            ip = tmpValue;
                            break;
                        case "port":
                            port = tmpValue;
                            break;
                        case "service":
                            service = tmpValue;
                            break;
                        case "acc":
                            acc = tmpValue;
                            break;
                        case "pwd":
                            pwd = tmpValue;
                            break;
                        case "table":
                            table = tmpValue;
                            break;
                        case "coldate":
                            coldate = tmpValue;
                            break;
                        case "colmarket":
                            colmarket = tmpValue;
                            break;
                        case "colstatus":
                            colstatus = tmpValue;
                            break;
                    }//eof switch
                }



                XmlNode marketList = rootNode.SelectSingleNode("//marketlist");
                if (marketList == null)
                    throw new Exception("无法找到文件列表节点<marketlist>，请检查配置文件格式是否正确!");
                foreach (XmlNode xnFile in marketList.ChildNodes) // 循环talist下的子节点
                {
                    if (string.Equals(xnFile.Name.Trim().ToLower(), "market", StringComparison.CurrentCultureIgnoreCase))   // <file>节点
                    {
                        // 临时变量
                        string id = string.Empty;           // 市场ID
                        string desc = string.Empty;         // 市场描述


                        foreach (XmlNode xnChildAttr in xnFile)
                        {
                            string tmpKey = xnChildAttr.Name.ToLower().Trim();
                            string tmpValue = xnChildAttr.InnerText.Trim();
                            switch (tmpKey)
                            {
                                case "id":
                                    id = tmpValue;
                                    break;
                                case "desc":
                                    desc = tmpValue;
                                    break;
                            }//eof switch
                        }//eof foreach

                        if (!dicMarketStatus.ContainsKey(id))
                            dicMarketStatus.Add(id, false);
                    }//eof if
                }


                #endregion 交易日数据库获取


                #region 文件列表获取逻辑

                // 检查filelist节点
                XmlNode fileListNode = rootNode.SelectSingleNode("//filelist");
                if (fileListNode == null)
                    throw new Exception("无法找到文件列表节点<filelist>，请检查配置文件格式是否正确!");
                // 直接找filelist子节点，形成HqFile对象，加入列表
                foreach (XmlNode xnFile in fileListNode.ChildNodes) // 循环talist下的子节点
                {
                    if (string.Equals(xnFile.Name.Trim().ToLower(), "file", StringComparison.CurrentCultureIgnoreCase))   // <file>节点
                    {
                        // 临时变量
                        string module = string.Empty;       // 模块名
                        string desc = string.Empty;         // 文件描述
                        string path = string.Empty;         // 文件路径
                        string market = string.Empty;       // 市场
                        string startTime = string.Empty;    // 开始检查时间
                        string extraType = string.Empty;    // 额外检查-文件类型（只有txt和dbf）
                        string extraFormat = string.Empty;  // 额外检查-格式（txt:行号,起始字符,长度  dbf:行号,列名）


                        foreach (XmlNode xnChildAttr in xnFile)
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
                                case "market":
                                    market = tmpValue;
                                    break;
                                case "starttime":
                                    startTime = tmpValue;
                                    break;
                                case "extra":
                                    {
                                        foreach (XmlNode xnChildExtraAttr in xnChildAttr)
                                        {
                                            string tmpChildKey = xnChildExtraAttr.Name.ToLower().Trim();
                                            string tmpChildValue = xnChildExtraAttr.InnerText.Trim();
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
                        _listHqFile.Add(new HqFile(module, desc, path, market, startTime, DtNow, extraType, extraFormat));

                    }//eof if ta
                }//eof foreach
                #endregion 文件列表获取逻辑

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


        /// <summary>
        /// 更新市场状态
        /// </summary>
        public void UpdateMarketStatus()
        {
            /*连接数据库，更新dicMarketStatus
             */

            string connString = string.Format(@"User ID={0};Password={1};Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = {2})(PORT = {3}))) (CONNECT_DATA = (SERVICE_NAME = {4})))",
                acc,
                pwd,
                ip,
                port,
                service);
            using (OracleCommand cmd = new OracleCommand())
            {
                using (OracleConnection conn = new OracleConnection(connString))
                {
                    conn.Open();
                    cmd.Connection = conn;

                    List<string> tmpKeys = new List<string>(dicMarketStatus.Keys);
                    for (int i = 0; i < tmpKeys.Count; i++)
                    {
                        string tmpKey = tmpKeys[i];
                        bool tmpValue = true;
                        //bool tmpValue = dicMarketStatus[tmpKey];
                        cmd.CommandText = string.Format(@"select {0} from {1} where {2}='{3}' and {4}='{5}'", colstatus, table, coldate, DtNow.ToString("yyyyMMdd"), colmarket, tmpKey);
                        object result = cmd.ExecuteScalar();
                        if (result == null)
                        {
                            tmpValue = false;
                        }
                        else
                        {
                            if (result.ToString() == "1")
                                tmpValue = true;
                            else
                                tmpValue = false;
                        }
                        dicMarketStatus[tmpKey] = tmpValue;
                    }
                }
            }

            // 更新list
            foreach (HqFile tmpHqFile in _listHqFile)
            {
                if (dicMarketStatus.ContainsKey(tmpHqFile.Market))
                {
                    tmpHqFile.IsTradingDay = dicMarketStatus[tmpHqFile.Market];
                }
                else // 找不到的市场默认是交易日
                {
                    tmpHqFile.IsTradingDay = true;
                }
            }
        }


        #region 属性

        /// <summary>
        /// 检查日期
        /// </summary>
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

        /// <summary>
        /// 必检项总数
        /// </summary>
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

        /// <summary>
        /// 必检项完成数
        /// </summary>
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

        /// <summary>
        /// 所有文件数
        /// </summary>
        public int GetAllCnt
        {
            get
            {
                return _listHqFile.Count;
            }
        }

        /// <summary>
        /// 所有文件完成数
        /// </summary>
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
        /// 判断所有行情文件都就绪
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


        public string IP
        {
            get { return ip; }
        }
        public string Port
        {
            get { return port; }
        }
        public string Service
        {
            get { return service; }
        }
        public string Acc
        {
            get { return acc; }
        }
        public string Pwd
        {
            get { return pwd; }
        }
        public string Table
        {
            get { return table; }
        }
        public string ColMarket
        {
            get { return colmarket; }
        }
        public string ColStatus
        {
            get { return colstatus; }
        }
        public bool MarketStatusAcquired
        {
            get { return _marketStatusAcquired; }
            set { _marketStatusAcquired = value; }
        }
        public Dictionary<string, bool> DicMarketStatus
        {
            get { return dicMarketStatus; }
            set { dicMarketStatus = value; }
        }



        #endregion 属性
    }
}
