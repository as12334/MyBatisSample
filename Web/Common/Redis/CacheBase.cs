using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ServiceStack.Redis;
using System.Data;

namespace redisTool
{
    public static class CacheBase<T>
    {
        #region Redis
        //redis IP��ַ
        private static string RedisIP = System.Configuration.ConfigurationSettings.AppSettings["RedisIP"];
        //redis���루�����ʾû�����룩
        private static string RedisPassword = System.Configuration.ConfigurationSettings.AppSettings["RedisPassword"];
        //redis�˿ڣ�����Ĭ��6379��
        private static int RedisPort = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["RedisPort"]);
        //redis�������ţ�������Ĭ����5���⣬��0��ʼ�������ʾ0��
        private static int DbIndex = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["RedisDbIndex"]);
        //redis �Ƿ�ʹ�û��濪��
        private static int isOpenCache = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["IsOpenRedis"]);

        private static PooledRedisClientManager prcm = CreateManager(
        new string[] { (RedisPassword.Trim() == string.Empty ? "" : RedisPassword + "@") + RedisIP + ":" + RedisPort + " " },
        new string[] { (RedisPassword.Trim() == string.Empty ? "" : RedisPassword + "@") + RedisIP + ":" + RedisPort + " " });
        private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            // ֧�ֶ�д���룬���⸺�� 
            RedisClientManagerConfig clientConfig = new RedisClientManagerConfig();

            clientConfig.MaxWritePoolSize = 10000;
            clientConfig.MaxReadPoolSize = 10000;
            clientConfig.AutoStart = true;
            clientConfig.DefaultDb = DbIndex;
            PooledRedisClientManager clientManager = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, clientConfig);
            return clientManager;
        }

        /// <summary>
        /// �Ƿ�ʹ�û��濪��
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
        /// ���뻺��
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="key">Key(��ֵ�����淶��RK_�ֶ���_����_�����ֶ�1_ֵ_�����ֶ�n_ֵ......,��ֵȫ��Сд,��������dboǰ׺)</param>
        /// <param name="value">����</param>
        /// <param name="Timer">����ʱ��(����XianZhiAccounts�Ĺ���ʱ��ΪһСʱ������Ĺ���ʱ�䶼Ϊ����)</param>
        /// <returns>�Ƿ񻺴�ɹ�</returns>
        /// ����datatable �Ļ��� ��Ҫ���⴦��
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
        /// ��ȡ��������
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
        /// ����datatable �Ļ��� ��Ҫ���⴦��
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
        /// ɾ������
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
        /// ����ɾ������
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
        /// ��ջ���
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
        /// �Ƿ����KEY����
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
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();//����BinaryFormatter�����л�DataSet����   
            System.IO.MemoryStream ms = new System.IO.MemoryStream();//�����ڴ�������   
            formatter.Serialize(ms, t);//��DataSet�������л����ڴ���   
            byte[] buffer = ms.ToArray();//���ڴ�������д���ֽ�����   
            ms.Close();//�ر��ڴ�������   
            ms.Dispose();//�ͷ���Դ   
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