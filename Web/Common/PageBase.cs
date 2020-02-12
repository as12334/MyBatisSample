using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using BuilderDALSQL;
using Data.Implements;
using Data.Interface;
using Entity;

namespace LotterySystem.Common
{
    public class PageBase : Page
    {
        public static  bool is_ip_locked()
        {
            return false;
        }

        public static string GetMessageByCache(string code,string type)
        {
            Dictionary<string, string> message = new Dictionary<string, string>();
            message.Add("u100001","验证码错误!");
            message.Add("u100002","用户名错误!");
            message.Add("u100003","密码错误!");
            message.Add("u100004","帐号被冻结!");
            message.Add("u100005","帐号被停用!");
            message.Add("u100006","您的上级帐号已经被冻结,请与管理员联系！");
            return message[code];
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
//            throw LSRequest.GetIP();
        }

        public static bool IsErrTimesAbove(ref DateTime? time, string str5)
        {
            cz_user_psw_err_logService service = new cz_user_psw_err_logService();
            IList<cz_user_psw_err_log>  users = service.GetListByWhere(String.Format(" u_name = '{0}' and err_times > 0", str5));
            if (users.Count > 0)
            {
                time = users[0].get_update_date();
                return true;
            }
            return false;

        }

        public static bool IsErrTimeout(DateTime? time)
        {
            //验证码有效时间
            return time > DateTime.Now.AddHours(-1);
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
        
        protected static bool IsNeedPopBrower()
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

        protected static string get_GetLottorySystemName()
        {
            //测试系统
            return "测试系统";
        }

        protected static void SetBrowerFlag(string browserCode)
        {
            throw new NotImplementedException();
        }
    }
}