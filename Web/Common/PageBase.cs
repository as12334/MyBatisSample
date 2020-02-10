using System;
using System.Collections.Generic;
using BuilderDALSQL;
using Data.Implements;
using Data.Interface;
using Entity;

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

        public static bool IsLockedTimeout(string loginName, string type)
        {
            string sql = String.Format("update cz_users set a_state = {0} where u_name = '{1}'",1,loginName);
            if (type.Equals("child"))
            {
                sql = String.Format("update cz_users_child set status = {0} where u_name = '{1}'",1,loginName);
            }

            int executteSql = DbHelperSQL.executte_sql(sql);

            if (executteSql > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void zero_retry_times_children(string str5)
        {
            var sql = String.Format("update cz_users_child set retry_times = 0 where u_name = '{0}'",str5);
            DbHelperSQL.executte_sql(sql);
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

//        ?????
        public static string upper_user_status(string getParentUName)
        {
            Icz_usersService czUsersService = new cz_usersService();
            IList<cz_users>  users = czUsersService.GetListByWhere(String.Format(" u_name = '{0}'",getParentUName));
            if (users.Count > 0)
            {
                return users[0].get_a_state().ToString();
            }
            return null;
        }

        public static void zero_retry_times(string str5)
        {
            Icz_usersService czUsersService = new cz_usersService();
            czUsersService.UpdateFields("retry_times = 0", String.Format("u_name = '{0}'",str5));
        }

        public static void inc_retry_times(string str5)
        {
            Icz_usersService czUsersService = new cz_usersService();
            czUsersService.UpdateFields("retry_times = retry_times + 1", String.Format("u_name = '{0}'",str5));
        }

        public static void SetAppcationFlag(string str5)
        {
            //todo ????δ???
        }

        public static void ZeroIsOutFlag(string str5)
        {
            //todo 未清楚流程
//            throw new NotImplementedException();
        }

        public static int PasswordExpire()
        {
            //密码过期时间/天
            return 7;
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