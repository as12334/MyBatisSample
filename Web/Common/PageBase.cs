using System;

namespace LotterySystem.Common
{
    public class PageBase
    {
        public static  bool is_ip_locked()
        {
            return false;
        }

        public static string GetMessageByCache(string code,string type)
        {
            return "";
        }

        public static bool IsLockedTimeout(string str5, string child)
        {
            throw new System.NotImplementedException();
        }

        public static void zero_retry_times_children(string str5)
        {
            throw new System.NotImplementedException();
        }

        public static void inc_retry_times_children(string str5)
        {
            throw new System.NotImplementedException();
        }

        public static void login_error_ip()
        {
            throw new System.NotImplementedException();
        }

        public static bool IsErrTimesAbove(ref DateTime time, string str5)
        {
            throw new NotImplementedException();
        }

        public static bool IsErrTimeout(DateTime time)
        {
            throw new NotImplementedException();
        }

        public static string upper_user_status(string getParentUName)
        {
            throw new NotImplementedException();
        }

        public static void zero_retry_times(string str5)
        {
            throw new NotImplementedException();
        }

        public static void inc_retry_times(string str5)
        {
            throw new NotImplementedException();
        }

        public static void SetAppcationFlag(string str5)
        {
            throw new NotImplementedException();
        }

        public static void ZeroIsOutFlag(string str5)
        {
            throw new NotImplementedException();
        }

        public static int PasswordExpire()
        {
            throw new NotImplementedException();
        }

        public EventHandler Load { get; set; }

        protected static bool IsNeedPopBrower(string toString)
        {
            throw new NotImplementedException();
        }

        protected bool IsUserOut(string toString)
        {
            throw new NotImplementedException();
        }

        protected static object GetPublicForderPath(object getLotteryCachesFileName)
        {
            throw new NotImplementedException();
        }
    }
}