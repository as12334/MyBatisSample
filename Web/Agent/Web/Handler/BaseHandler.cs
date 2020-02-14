using System.Collections.Generic;
using Entity;

namespace Agent.Web.Handler
{
    using Agent.Web.WebBase;
    using LotterySystem.Model;
    using System;
    using System.Web;
    using System.Web.SessionState;

    public class BaseHandler : MemberPageBase, IHttpHandler, IRequiresSessionState
    {
        public bool IsFGSWT_Opt(int lid)
        {
            string str = HttpContext.Current.Session["user_name"].ToString();
            if (HttpContext.Current.Session["user_type"].ToString().Equals("fgs"))
            {
                cz_users userInfoByUName = CallBLL.CzUsersService.GetUserInfoByUName(str);
                agent_userinfo_session _session = HttpContext.Current.Session[str + "lottery_session_user_info"] as agent_userinfo_session;
                if (!(userInfoByUName.get_six_op_odds().Equals(_session.get_six_op_odds()) && userInfoByUName.get_kc_op_odds().Equals(_session.get_kc_op_odds())))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsReusable =>
            false;

        protected int get_current_master_id()
        {
            throw new NotImplementedException();
        }

        protected List<object> get_ad_level(string str2, int i, string str5)
        {
            throw new NotImplementedException();
        }

        protected string get_online_cnt(string redis)
        {
            throw new NotImplementedException();
        }
        
        protected string get_online_cnt()
        {
            throw new NotImplementedException();
        }

        protected string get_online_cntStack(string redis)
        {
            throw new NotImplementedException();
        }

        protected void CheckIsOut(string str7)
        {
            throw new NotImplementedException();
        }

        protected void stat_online_redis(string str7, string str2)
        {
            throw new NotImplementedException();
        }

        protected void CheckIsOutStack(string str7)
        {
            throw new NotImplementedException();
        }

        protected void stat_online_redisStack(string str7, string str2)
        {
            throw new NotImplementedException();
        }

        protected void stat_online_redis_timer()
        {
            throw new NotImplementedException();
        }

        protected void stat_online_redis_timerStack()
        {
            throw new NotImplementedException();
        }

        protected bool IsAutoUpdate(string toString, string compareTime)
        {
            throw new NotImplementedException();
        }

        protected List<object> GetAutoJPForAd(string compareTime, ref DateTime now)
        {
            throw new NotImplementedException();
        }

        protected string SetWordColor(string str16)
        {
            throw new NotImplementedException();
        }
    }
}

