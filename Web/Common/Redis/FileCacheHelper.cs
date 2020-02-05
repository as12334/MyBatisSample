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
            throw new System.NotImplementedException();
        }

        public static long get_GetRedisDBIndex()
        {
            throw new System.NotImplementedException();
        }

        public static object get_LotteryCachesFileName()
        {
            throw new System.NotImplementedException();
        }
    }
}