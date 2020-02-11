using LotterySystem.Common.Redis;

namespace Agent.Web
{
    using Agent.Web.WebBase;
    using System;

    public class Quit : MemberPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = this.Session["user_name"].ToString();
            string str2 = this.Session["user_type"].ToString();
            string str3 = base.get_children_name();
            if (str3 != "")
            {
                str = str3;
            }
            if (FileCacheHelper.get_RedisStatOnline().Equals(0))
            {
                MemberPageBase.update_online_user(str);
            }
            this.Session.Abandon();
            base.Response.Write(" <SCRIPT type=\"text/javascript\">top.location.href = '/';</script>");
            base.Response.End();
        }
    }
}

