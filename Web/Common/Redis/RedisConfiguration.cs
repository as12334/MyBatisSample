    using System;
    using System.Configuration;

    /// <summary>
    /// 表示配置文件中的 Redis 配置节。
    /// </summary>
    public sealed class RedisConfiguration : ConfigurationSection
    {
        /// <summary>
        /// 检索当前应用程序默认配置的 Redis 配置节。
        /// </summary>
        /// <returns>指定的 Redis 配置节对象，或者，如果该节不存在，则为 null。</returns>
        public static RedisConfiguration GetConfig()
        {
            RedisConfiguration section = (RedisConfiguration)ConfigurationManager.GetSection("RedisConfig");
            return section;
        }
 
        /// <summary>
        /// 检索当前应用程序默认配置的 Redis 配置节。
        /// </summary>
        /// <param name="sectionName">配置节的路径和名称。</param>
        /// <returns>指定的 Redis 配置节对象，或者，如果该节不存在，则为 null。</returns>
        public static RedisConfiguration GetConfig(string sectionName)
        {
            RedisConfiguration section = (RedisConfiguration)ConfigurationManager.GetSection("RedisConfig");
            if (section == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
            return section;
        }
        /// <summary>
        /// 获取或设置用于写入的 Redis 服务器地址。
        /// </summary>
        [ConfigurationProperty("WriteServerList", IsRequired = false)]
        public string WriteServerHosts
        {
            get
            {
                return (string)base["WriteServerList"];
            }
            set
            {
                base["WriteServerList"] = value;
            }
        }
 
        /// <summary>
        /// 获取或设置用于读取的 Redis 服务器的主机地址。
        /// </summary>
        [ConfigurationProperty("ReadServerList", IsRequired = false)]
        public string ReadServerHosts
        {
            get
            {
                return (string)base["ReadServerList"];
            }
            set
            {
                base["ReadServerList"] = value;
            }
        }
 
        /// <summary>
        /// 获取或设置 Redis Sentinel 服务器的主机地址。
        /// </summary>
        [ConfigurationProperty("SentinelServerList", IsRequired = false)]
        public string SentinelServerHosts
        {
            get
            {
                return (string)base["SentinelServerList"];
            }
            set
            {
                base["SentinelServerList"] = value;
            }
        }
 
        /// <summary>
        /// 获取或设置 Redis Sentinel 服务器的密码。
        /// </summary>
        [ConfigurationProperty("SentinelPassword", IsRequired = false)]
        public string SentinelPassword
        {
            get
            {
                return (string)base["SentinelPassword"];
            }
            set
            {
                base["SentinelPassword"] = value;
            }
        }
 
        /// <summary>
        /// 获取或设置 Redis 主服务器的名称。
        /// </summary>
        [ConfigurationProperty("MasterName", IsRequired = false)]
        public string MasterName
        {
            get
            {
                string masterName = (string)base["MasterName"];
                return String.IsNullOrEmpty(masterName) ? "master" : masterName;
            }
            set
            {
                base["MasterName"] = value;
            }
        }
 
        /// <summary>
        /// 获取或设置 Sentinel 的默认数据库。
        /// </summary>
        [ConfigurationProperty("SentinelDb", IsRequired = false, DefaultValue = 0)]
        public int SentinelDb
        {
            get
            {
                int sentinelDb = (int)base["SentinelDb"];
                return sentinelDb >= 0 ? sentinelDb : 0;
            }
            set
            {
                base["SentinelDb"] = value;
            }
        }
 
        /// <summary>
        /// 最大写入连接池大小。
        /// </summary>
        [ConfigurationProperty("MaxWritePoolSize", IsRequired = false, DefaultValue = 5)]
        public int MaxWritePoolSize
        {
            get
            {
                int maxWritePoolSize = (int)base["MaxWritePoolSize"];
                return maxWritePoolSize > 0 ? maxWritePoolSize : 5;
            }
            set
            {
                base["MaxWritePoolSize"] = value;
            }
        }
 
        /// <summary>
        /// 最大读取连接池大小。
        /// </summary>
        [ConfigurationProperty("MaxReadPoolSize", IsRequired = false, DefaultValue = 5)]
        public int MaxReadPoolSize
        {
            get
            {
                int maxReadPoolSize = (int)base["MaxReadPoolSize"];
                return maxReadPoolSize > 0 ? maxReadPoolSize : 5;
            }
            set
            {
                base["MaxReadPoolSize"] = value;
            }
        }
 
        /// <summary>
        /// 自动重启。
        /// </summary>
        [ConfigurationProperty("AutoStart", IsRequired = false, DefaultValue = true)]
        public bool AutoStart
        {
            get
            {
                return (bool)base["AutoStart"];
            }
            set
            {
                base["AutoStart"] = value;
            }
        }
 
        /// <summary>
        /// 本地缓存到期时间，单位：秒。
        /// </summary>
        [ConfigurationProperty("CacheExpires", IsRequired = false, DefaultValue = 36000)]
        public int CacheExpires
        {
            get
            {
                return (int)base["CacheExpires"];
            }
            set
            {
                base["CacheExpires"] = value;
            }
        }
 
 
        /// <summary>
        /// 是否记录日志，该设置仅用于排查 Redis 运行时出现的问题，如 Redis 工作正常，请关闭该项。
        /// </summary>
        [ConfigurationProperty("RecordeLog", IsRequired = false, DefaultValue = false)]
        public bool RecordeLog
        {
            get
            {
                return (bool)base["RecordeLog"];
            }
            set
            {
                base["RecordeLog"] = value;
            }
        }
 
    }