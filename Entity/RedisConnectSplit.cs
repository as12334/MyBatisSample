namespace LotterySystem.Model
{
    public class RedisConnectSplit
    {
        public static string get_RedisIP()
        {
            return "192.168.0.88";
        }

        public static int get_RedisPort()
        {
            return 6379;
        }

        public static string get_RedisPassword()
        {
            return "";
        }

        public static string get_RedisConnectTimeout()
        {
            return "1800";
        }
    }
}