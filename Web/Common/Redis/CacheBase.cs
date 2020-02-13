using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ServiceStack.Redis;
using System.Data;

namespace Utils
{
    public static class CacheBase<T>
    {
        #region Redis
        //redis IP地址
        private static string RedisIP = System.Configuration.ConfigurationSettings.AppSettings["RedisIP"];
        //redis密码（不填表示没有密码）
        private static string RedisPassword = System.Configuration.ConfigurationSettings.AppSettings["RedisPassword"];
        //redis端口（不填默认6379）
        private static int RedisPort = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["RedisPort"]);
        //redis库索引号（整数，默认有5个库，从0开始，不填表示0）
        private static int DbIndex = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["RedisDbIndex"]);
        //redis 是否使用缓存开关
        private static int isOpenCache = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["IsOpenRedis"]);

        private static PooledRedisClientManager prcm = CreateManager(
        new string[] { (RedisPassword.Trim() == string.Empty ? "" : RedisPassword + "@") + RedisIP + ":" + RedisPort + " " },
        new string[] { (RedisPassword.Trim() == string.Empty ? "" : RedisPassword + "@") + RedisIP + ":" + RedisPort + " " });
        private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            // 支持读写分离，均衡负载 
            RedisClientManagerConfig clientConfig = new RedisClientManagerConfig();

            clientConfig.MaxWritePoolSize = 10000;
            clientConfig.MaxReadPoolSize = 10000;
            clientConfig.AutoStart = true;
            clientConfig.DefaultDb = DbIndex;
            PooledRedisClientManager clientManager = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, clientConfig);
            return clientManager;
        }

        /// <summary>
        /// 是否使用缓存开关
        /// </summary>
        private static bool IsOpenCache
        {
            get
            {
                if (isOpenCache != 1) return false;
                return true;
            }
        }

        /// <summary>
        /// 加入缓存
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">Key(键值命名规范：RK_字段名_表名_条件字段1_值_条件字段n_值......,键值全部小写,表名不加dbo前缀)</param>
        /// <param name="value">对象</param>
        /// <param name="Timer">缓存时间(除了XianZhiAccounts的过期时间为一小时，其余的过期时间都为两天)</param>
        /// <returns>是否缓存成功</returns>
        /// 对于datatable 的缓存 需要特殊处理
        public static bool SaveCaChe(string key, T value)
        {
            bool result = false;
            try
            {
                if (IsOpenCache)
                {
                    if (!(value is DataTable) && !(value is DataSet) && !(value is DataRow))
                    {
                        using (IRedisClient Redis = prcm.GetClient())
                        {
                            result = Redis.Set<T>(key, value, DateTime.Now.AddHours(24));
                        }
                    }
                    else
                    {
                        using (IRedisClient Redis = prcm.GetClient())
                        {
                            result = Redis.Set<byte[]>(key, SetBytesFormT(value), DateTime.Now.AddHours(24));
                        }
                    }
                }
            }
            catch { }
            return result;
        }
        public static bool SaveCaChe(string key, T value, int Timer)
        {
            bool result = false;
            try
            {
                if (IsOpenCache)
                {
                    if (!(value is DataTable) && !(value is DataSet) && !(value is DataRow))
                    {
                        using (IRedisClient Redis = prcm.GetClient())
                        {
                            result = Redis.Set<T>(key, value, TimeSpan.FromMinutes(Timer));
                        }
                    }
                    else
                    {
                        using (IRedisClient Redis = prcm.GetClient())
                        {
                            result = Redis.Set<byte[]>(key, SetBytesFormT(value), DateTime.Now.AddHours(24));
                        }
                    }
                }
            }
            catch { }
            return result;
        }
        public static bool SaveBaseCaChe(string key, T value)
        {
            bool result = false;
            try
            {
                if (IsOpenCache)
                {
                    if (!(value is DataTable) && !(value is DataSet) && !(value is DataRow))
                    {
                        using (IRedisClient Redis = prcm.GetClient())
                        {
                            result = Redis.Set<T>(key, value, DateTime.Now.AddHours(24));
                        }
                    }
                    else
                    {
                        using (IRedisClient Redis = prcm.GetClient())
                        {
                            result = Redis.Set<byte[]>(key, SetBytesFormT(value), DateTime.Now.AddHours(24));
                        }
                    }
                }
            }
            catch { }
            return result;
        }
        public static bool SaveBaseCaChe(string key, T value, int Timer)
        {
            bool result = false;
            try
            {
                if (IsOpenCache)
                {
                    if (!(value is DataTable) && !(value is DataSet) && !(value is DataRow))
                    {
                        using (IRedisClient Redis = prcm.GetClient())
                        {
                            result = Redis.Set<T>(key, value, TimeSpan.FromMinutes(Timer));
                        }
                    }
                    else
                    {
                        using (IRedisClient Redis = prcm.GetClient())
                        {
                            result = Redis.Set<byte[]>(key, SetBytesFormT(value), TimeSpan.FromMinutes(Timer));
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 获取缓存内容
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static T GetCaChe(string key)
        {
            try
            {
                if (!IsOpenCache) return default(T);
                using (IRedisClient Redis = prcm.GetClient())
                {
                    return Redis.Get<T>(key);
                }
            }
            catch
            {
                string str = string.Empty;
                return default(T);
            }
        }
        public static T GetBaseCaChe(string key)
        {
            try
            {
                if (!IsOpenCache) return default(T);
                using (IRedisClient Redis = prcm.GetClient())
                {
                    return Redis.Get<T>(key);
                }
            }
            catch
            {
                string str = string.Empty;
                return default(T);
            }
        }
        /// <summary>
        /// 对于datatable 的缓存 需要特殊处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetBaseCaChe<T>(string key) where T : class
        {
            try
            {
                if (!IsOpenCache) return default(T);
                if (!(typeof(T).ToString() == "System.Data.DataTable")
                    && !(typeof(T).ToString() == "System.Data.DataSet")
                    && !(typeof(T).ToString() == "System.Data.DataRow"))
                {
                    using (IRedisClient Redis = prcm.GetClient())
                    {
                        return Redis.Get<T>(key);
                    }
                }
                else
                {
                    using (IRedisClient Redis = prcm.GetClient())
                    {
                        byte[] buffer = Redis.Get<byte[]>(key);
                        return GetObjFromBytes(buffer) as T;
                    }
                }
            }
            catch
            {
                string str = string.Empty;
                return default(T);
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static bool DeleteCache(string key)
        {
            try
            {
                if (!IsOpenCache) return false;
                using (IRedisClient Redis = prcm.GetClient())
                {
                    return Redis.Remove(key);
                }
            }
            catch { return false; }
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys">Key</param>
        /// <returns></returns>
        public static void DeleteCache(List<string> keys)
        {
            try
            {
                if (!IsOpenCache) return;
                using (IRedisClient Redis = prcm.GetClient())
                {
                    Redis.RemoveAll(keys);
                }
            }
            catch { return; }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void ClearCache()
        {
            try
            {
                if (!IsOpenCache) return;
                using (IRedisClient Redis = prcm.GetClient())
                {
                    Redis.FlushAll();
                }
            }
            catch { return; }
        }
        /// <summary>
        /// 是否包含KEY缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsContain(string key)
        {
            try
            {
                if (!IsOpenCache) return false;
                using (IRedisClient Redis = prcm.GetClient())
                {
                    return Redis.ContainsKey(key);
                }
            }
            catch { return false; }
        }

        public static byte[] SetBytesFormT(T t)
        {
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();//定义BinaryFormatter以序列化DataSet对象   
            System.IO.MemoryStream ms = new System.IO.MemoryStream();//创建内存流对象   
            formatter.Serialize(ms, t);//把DataSet对象序列化到内存流   
            byte[] buffer = ms.ToArray();//把内存流对象写入字节数组   
            ms.Close();//关闭内存流对象   
            ms.Dispose();//释放资源   
            return buffer;
        }

        private static object GetObjFromBytes(byte[] buffer)
        {
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer))
            {
                stream.Position = 0;
                System.Runtime.Serialization.IFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Object reobj = bf.Deserialize(stream);
                return reobj;
            }
        }
        #endregion
    }
}