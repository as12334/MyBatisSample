using System;
using System.Web;

namespace LotterySystem.Common
{
    public class Utils
    {
        public static string GetBrowserInfo(HttpContext current)
        {
            throw new System.NotImplementedException();
        }

        public static string Number(int i)
        {
            throw new System.NotImplementedException();
        }

        public static string GetRamSalt(int p0)
        {
            string str  = @"0123456789abcdefghigklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ";            
            string result = "";
            Random random = new Random();
            for (int i = 0; i < p0; i++)
            {
                result +=  str.Substring(10+random.Next(26),1);
            }
            return result;
        }

        public static object DateTimeToStamp(DateTime now)
        {
            throw new NotImplementedException();
        }
    }
}