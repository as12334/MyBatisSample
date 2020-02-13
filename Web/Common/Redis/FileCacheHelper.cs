using ServiceStack.Redis;

namespace LotterySystem.Common.Redis
{
    public sealed class FileCacheHelper
    {
        private static RedisClient redisclient;
        static FileCacheHelper()
        {
            CreateManager();
        }
        private static void CreateManager()
        {
            if (redisclient == null)
            {
                redisclient = new RedisClient("192.168.0.88:6379");
            }
        }

        public static string get_GetLockedPasswordCount()
        {
            return "1";
        }

        public static string get_GetLockedUserCount()
        {
//            todo 系统设定的密码错误次数
            return "5";
        }

        public static int get_RedisStatOnline()
        {
            //0:未使用缓存  1：单节点  2：集群
            return 0;
            throw new System.NotImplementedException();
        }

        public static long get_GetRedisDBIndex()
        {
            return 0;
        }

        public static object get_LotteryCachesFileName()
        {
            //todo
            return "";
        }

        public static string get_AjaxErrorLogSwitch()
        {
            //todo
            return "";
        }

        public static string get_IsViewNewReportMenu()
        {
            //todo
            return "1";
        }

        public static string get_ManageZJProfit()
        {
            //todo 未完善
            return "1";
        }
        
        
        
    }
}