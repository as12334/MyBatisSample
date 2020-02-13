using LotterySystem.Common.Redis;

namespace Agent.Web
{
    using Agent.Web.WebBase;
    using LotterySystem.Common;
    using LotterySystem.Model;
    using System;
    using System.Configuration;
    using System.Data;

    public class Index : MemberPageBase
    {
        protected DataTable lotteryDT = null;
        protected string userName = "";
        protected string navString = "";
        protected string online_type = "";
        protected string url = "";
        protected string zodiacData = "";
        protected string saleuser = "";
        protected string negative_sale = "";
        protected string masterids = "";
        protected string skin = "";
        protected string sysName = PageBase.get_GetLottorySystemName();
        protected agent_userinfo_session uModel;
        protected bool isChildSytem = false;
        protected string browserCode = "";
        protected string ajaxErrorLogSwitch = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ajaxErrorLogSwitch = FileCacheHelper.get_AjaxErrorLogSwitch();
            string str = ConfigurationManager.AppSettings["CloseIndexRefresh"];
//            if ((str != "true") && PageBase.IsNeedPopBrower())
             if(this.Session["user_name"] == null)
            {
                this.Session.Abandon();
                base.Response.Write("<script>top.location.href='/'</script>");
                base.Response.End();
            }
            this.lotteryDT = base.GetLotteryList();
            this.navString = base.GetNav();
            DataRow row = this.lotteryDT.Rows[0];
            this.zodiacData = base.get_YearLianArray();
            this.uModel = this.Session[this.Session["user_name"] + "lottery_session_user_info"] as agent_userinfo_session;
            this.skin = this.uModel.get_u_skin();
            this.online_type = this.uModel.get_u_type().Trim();
            if (CallBLL.CzAdminSubsystemService.GetModel().get_flag().Equals(1))
            {
                this.isChildSytem = true;
            }
            this.saleuser = this.saleuser + "{";
            this.saleuser = this.saleuser + "\"saleuser\": {";
            if (this.uModel.get_u_type().Trim().Equals("zj"))
            {
                DataTable saleSetUser = CallBLL.CzSalesetSixService.GetSaleSetUser();
                if (saleSetUser != null)
                {
                    for (int i = 0; i < saleSetUser.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            this.saleuser = this.saleuser + String.Format("{0}:{1}",saleSetUser.Rows[i]["u_name"],saleSetUser.Rows[i]["flag"]);
                        }
                        else
                        {
                            this.saleuser = this.saleuser + String.Format(",{0}:{1}",saleSetUser.Rows[i]["u_name"],saleSetUser.Rows[i]["flag"]);
                        }
                    }
                }
                this.negative_sale = this.uModel.get_negative_sale();
            }
            this.saleuser = this.saleuser + "}}";
            int num2 = 1;
            if (this.Session["user_state"].ToString().Equals(num2.ToString()))
            {
                this.url = string.Format("/Report.aspx", new object[0]);
            }
            num2 = 1;
            if (row["master_id"].ToString().Equals(num2.ToString()))
            {
                this.url = "";
            }
//            this.masterids = base.GetLotteryMasterID(this.lotteryDT);
            if (str != "true")
            {
                this.browserCode = Utils.Number(4);
                PageBase.SetBrowerFlag(this.browserCode);
            }
        }
    }
}

