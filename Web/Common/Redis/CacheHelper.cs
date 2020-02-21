using System.Data;
using System.Web;
using LotterySystem.Model;
using redisTool;
using ServiceStack.Redis;

namespace LotterySystem.Common.Redis
{
    public class CacheHelper
    {
        private static RedisClient client;
        public static void SetCache(string cachecurrentmlid, string toString)
        {
            throw new System.NotImplementedException();
        }    
        public static void SetCache(string cachecurrentmlid, DataTable dataTable)
        {
            CacheBase<DataTable>.SaveBaseCaChe(cachecurrentmlid, dataTable);
        }

        public static DataTable GetCache(string czLotteryFilecachekey)
        {
            return CacheBase<DataTable>.GetBaseCaChe<DataTable>(czLotteryFilecachekey);
        }

        public static void SetPublicFileCache(string czLotteryFilecachekey, DataTable table, object getPublicForderPath)
        {
            //todo 
        }

        public static void SetPublicFileCacheDependency(string allAutoJpFilecachekey, object dataTable, object getPublicForderPath)
        {
            throw new System.NotImplementedException();
        }
    }
}