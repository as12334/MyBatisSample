using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using BuilderDALSQL;
using Entity;
using LotterySystem.Common;
using LotterySystem.Common.Redis;
using LotterySystem.DBUtility;
using LotterySystem.Model;
using ServiceStack.Redis;
namespace Agent.Web.WebBase
{
    public class MemberPageBase : PageBase
    {
        public MemberPageBase()
        {
            base.Load += new EventHandler(this.MemberPageBase_Load);
        }

        public MemberPageBase(string mobile)
        {
        }
        public string get_children_name()
        {
            if (HttpContext.Current.Session["child_user_name"] != null)
            {
                return HttpContext.Current.Session["child_user_name"].ToString();
            }
            return "";
        }
        public void MemberPageBase_Load(object sender, EventArgs e)
        {
            DateTime now;
            cz_stat_online _online;
            cz_stat_online _online2;
            if (HttpContext.Current.Session["user_name"] == null)
            {
                HttpContext.Current.Response.Write("<script>top.location.href='/'</script>");
                HttpContext.Current.Response.End();
            }
            string str = this.get_children_name();
            if (FileCacheHelper.get_RedisStatOnline().Equals(1))
            {
                if (HttpContext.Current.Request.Path.ToLower().IndexOf("resetpasswd.aspx") > 0)
                {
                    now = DateTime.Now;
                    _online = new cz_stat_online();
                    _online.set_u_name((str == "") ? HttpContext.Current.Session["user_name"].ToString() : str);
                    _online.set_is_out(0);
                    _online.set_u_type(HttpContext.Current.Session["user_type"].ToString());
                    _online.set_ip(LSRequest.GetIP());
                    _online.set_first_time(new DateTime?(now));
                    _online.set_last_time(new DateTime?(now));
//                    CallBLL.redisHelper.HashGet<cz_stat_online>("useronline:list", (str == "") ? HttpContext.Current.Session["user_name"].ToString() : str, _online);
                }
//                if (CallBLL.redisHelper.HashExists("useronline:list", (str == "") ? HttpContext.Current.Session["user_name"].ToString() : str))
//                {
//                    _online2 = CallBLL.redisHelper.HashGet<cz_stat_online>("useronline:list", (str == "") ? HttpContext.Current.Session["user_name"].ToString() : str);
//                    if ((_online2 != null) && _online2.get_is_out().Equals(1))
//                    {
//                        HttpContext.Current.Session.Abandon();
//                        HttpContext.Current.Response.Write("<script>top.location.href='/'</script>");
//                        HttpContext.Current.Response.End();
//                    }
//                }
                if (PageBase.IsNeedPopBrower((str == "") ? HttpContext.Current.Session["user_name"].ToString() : str))
                {
                    HttpContext.Current.Session.Abandon();
                    HttpContext.Current.Response.Write("<script>top.location.href='/'</script>");
                    HttpContext.Current.Response.End();
                }
            }
            else if (FileCacheHelper.get_RedisStatOnline().Equals(2))
            {
                RedisClient client;
                if (HttpContext.Current.Request.Path.ToLower().IndexOf("resetpasswd.aspx") > 0)
                {
                    now = DateTime.Now;
                    _online = new cz_stat_online();
                    _online.set_u_name((str == "") ? HttpContext.Current.Session["user_name"].ToString() : str);
                    _online.set_is_out(0);
                    _online.set_u_type(HttpContext.Current.Session["user_type"].ToString());
                    _online.set_ip(LSRequest.GetIP());
                    _online.set_first_time(new DateTime?(now));
                    _online.set_last_time(new DateTime?(now));
                    using (client = new RedisClient(RedisConnectSplit.get_RedisIP(), RedisConnectSplit.get_RedisPort(), RedisConnectSplit.get_RedisPassword(), (long) FileCacheHelper.get_GetRedisDBIndex()))
                    {
                        client.ConnectTimeout = int.Parse(RedisConnectSplit.get_RedisConnectTimeout());
                        client.SetEntryInHash("useronline:list", (str == "") ? HttpContext.Current.Session["user_name"].ToString() : str, JsonHandle.ObjectToJson(_online));
                    }
                }
                using (client = new RedisClient(RedisConnectSplit.get_RedisIP(), RedisConnectSplit.get_RedisPort(), RedisConnectSplit.get_RedisPassword(), (long) FileCacheHelper.get_GetRedisDBIndex()))
                {
                    client.ConnectTimeout = int.Parse(RedisConnectSplit.get_RedisConnectTimeout());
                    if (client.HashContainsEntry("useronline:list", (str == "") ? HttpContext.Current.Session["user_name"].ToString() : str))
                    {
                        _online2 = JsonHandle.JsonToObject<cz_stat_online>(client.GetValueFromHash("useronline:list", (str == "") ? HttpContext.Current.Session["user_name"].ToString() : str)) as cz_stat_online;
                        if ((_online2 != null) && _online2.get_is_out().Equals(1))
                        {
                            HttpContext.Current.Session.Abandon();
                            HttpContext.Current.Response.Write("<script>top.location.href='/'</script>");
                            HttpContext.Current.Response.End();
                        }
                    }
                }
                if (PageBase.IsNeedPopBrower((str == "") ? HttpContext.Current.Session["user_name"].ToString() : str))
                {
                    HttpContext.Current.Session.Abandon();
                    HttpContext.Current.Response.Write("<script>top.location.href='/'</script>");
                    HttpContext.Current.Response.End();
                }
            }
            else if (base.IsUserOut((str == "") ? HttpContext.Current.Session["user_name"].ToString() : str) || PageBase.IsNeedPopBrower((str == "") ? HttpContext.Current.Session["user_name"].ToString() : str))
            {
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Write("<script>top.location.href='/'</script>");
                HttpContext.Current.Response.End();
            }
            string str3 = HttpContext.Current.Session["user_name"].ToString();
            this.UserCurrentSkin = (HttpContext.Current.Session[str3 + "lottery_session_user_info"] as agent_userinfo_session).get_u_skin();
            this.ForcedModifyPassword();
            this.AgentCurrentLottery();
            this.RedirectReport();
            this.IsOpenLottery();
            if (((((HttpContext.Current.Request.Path.ToLower().IndexOf("fgs_list") > -1) || (HttpContext.Current.Request.Path.ToLower().IndexOf("gd_list") > -1)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("zd_list") > -1) || (HttpContext.Current.Request.Path.ToLower().IndexOf("dl_list") > -1))) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("hy_list") > -1) || (HttpContext.Current.Request.Path.ToLower().IndexOf("child_list") > -1))) || (HttpContext.Current.Request.Path.ToLower().IndexOf("filluser_list") > -1))
            {
                CookieHelper.SetCookie("userreturnbackurl", HttpContext.Current.Request.ServerVariables["Path_Info"] + "?" + HttpContext.Current.Request.ServerVariables["Query_String"]);
            }
        }
        public static void stat_top_online(string loginname)
        {
            string str = "Online_Stat";
            string str2 = "Online_Stat_CheckTime";
            DateTime now = DateTime.Now;
            string str3 = $"update  cz_stat_online  set  first_time = '{now}',last_time= '{now}'  where u_name =@u_name ";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@u_name", SqlDbType.NVarChar) };
            parameterArray[0].Value = loginname;
            CallBLL.CzStatOnlineService.executte_sql(str3, parameterArray);
            string str4 = "";
            if (HttpContext.Current.Application[str] == null)
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[str2] = now;
                HttpContext.Current.Application.UnLock();
                str4 = $" select * from cz_stat_top_online  with(NOLOCK) where update_time > '{DateTime.Today}' and update_time < '{DateTime.Today.AddHours(24.0)}' ";
                if (CallBLL.CzStatTopOnlineService.query_sql(str4).Rows.Count <= 0)
                {
                    str4 = $"insert into cz_stat_top_online values({1},'{DateTime.Now}') ";
                    CallBLL.CzStatTopOnlineService.executte_sql(str4);
                }
            }
            else
            {
                int num = 1;
                string str5 = $" select count(1)  from cz_stat_online  with(NOLOCK) where last_time > '{now.AddMinutes(-3.0)}' ";
                DataTable table2 = CallBLL.CzStatOnlineService.query_sql(str5, parameterArray);
                if (table2.Rows.Count > 0)
                {
                    num = int.Parse(table2.Rows[0][0].ToString());
                }
                str4 = $"select * from cz_stat_top_online with(NOLOCK) where update_time > '{DateTime.Today}' and update_time < '{DateTime.Today.AddHours(24.0)}' ";
                DataTable table3 = CallBLL.CzStatTopOnlineService.query_sql(str4);
                if (table3.Rows.Count > 0)
                {
                    string s = table3.Rows[0]["top_cnt"].ToString();
                    if (num > int.Parse(s))
                    {
                        str4 = $"update cz_stat_top_online set top_cnt = {num}, update_time = '{now}' where update_time > '{DateTime.Today}' and update_time < '{DateTime.Today.AddHours(24.0)}' ";
                        CallBLL.CzStatTopOnlineService.executte_sql(str4);
                    }
                }
                else
                {
                    str4 = $"insert into cz_stat_top_online values({num},'{now}') ";
                    CallBLL.CzStatTopOnlineService.executte_sql(str4);
                }
            }
        }
         protected static void insert_online(string userIP, string user, string user_type, DateTime first_time, DateTime last_time)
        {
            string str = " select u_name from cz_stat_online with(NOLOCK) where u_name =@u_name ";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@u_name", SqlDbType.NVarChar) };
            parameterArray[0].Value = user.Trim();
            if (CallBLL.CzStatOnlineService.query_sql(str, parameterArray).Rows.Count > 0)
            {
                str = "update cz_stat_online set ip=@ip, first_time=@first_time, last_time=@last_time where u_name =@u_name ";
                SqlParameter[] parameterArray2 = new SqlParameter[] { new SqlParameter("@ip", SqlDbType.NVarChar), new SqlParameter("@first_time", SqlDbType.DateTime), new SqlParameter("@last_time", SqlDbType.DateTime), new SqlParameter("@u_name", SqlDbType.NVarChar) };
                parameterArray2[0].Value = userIP;
                parameterArray2[1].Value = first_time;
                parameterArray2[2].Value = last_time;
                parameterArray2[3].Value = user.Trim();
                CallBLL.CzStatOnlineService.executte_sql(str, parameterArray2);
            }
            else
            {
                string str2 = string.Format("insert into cz_stat_online (u_name,u_type,first_time,last_time,ip) values(@u_name,@user_type,@first_time,@last_time,@userIP)", new object[0]);
                SqlParameter[] parameterArray3 = new SqlParameter[] { new SqlParameter("@u_name", SqlDbType.NVarChar), new SqlParameter("@user_type", SqlDbType.NVarChar), new SqlParameter("@first_time", SqlDbType.DateTime), new SqlParameter("@last_time", SqlDbType.DateTime), new SqlParameter("@userIP", SqlDbType.NVarChar) };
                parameterArray3[0].Value = user.Trim();
                parameterArray3[1].Value = user_type;
                parameterArray3[2].Value = first_time;
                parameterArray3[3].Value = last_time;
                parameterArray3[4].Value = userIP;
                try
                {
                    CallBLL.CzStatOnlineService.executte_sql(str2, parameterArray3);
                }
                catch (SqlException exception)
                {
                    if ((exception.Number != 0xa29) && (exception.Number != 0xa43))
                    {
                        throw exception;
                    }
                }
            }
        }
        public static void stat_online(string user, string user_type)
        {
            ConcurrentDictionary<string, object> dictionary;
            string str = "Online_Stat";
            string str2 = "Online_Stat_CheckTime";
            string str3 = "online_User_Key";
            string iP = LSRequest.GetIP();
            DateTime now = DateTime.Now;
            if (HttpContext.Current.Application[str2] == null)
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[str2] = now;
                HttpContext.Current.Application.UnLock();
            }
            if (HttpContext.Current.Application[str] == null)
            {
                dictionary = new ConcurrentDictionary<string, object>();
                ArrayList list = new ArrayList {
                    iP,
                    now
                };
                dictionary.GetOrAdd(user, list);
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[str] = dictionary;
                HttpContext.Current.Application.UnLock();
                insert_online(iP, user, user_type, now, now);
            }
            else
            {
                dictionary = HttpContext.Current.Application[str] as ConcurrentDictionary<string, object>;
                if (dictionary.ContainsKey(user))
                {
                    if (HttpContext.Current.Application[str3] == null)
                    {
                        ConcurrentDictionary<string, object> dictionary2 = new ConcurrentDictionary<string, object>();
                        HttpContext.Current.Application[str3] = dictionary2;
                    }
                    ConcurrentDictionary<string, object> dictionary3 = HttpContext.Current.Application[str3] as ConcurrentDictionary<string, object>;
                    ArrayList infoList = new ArrayList {
                        iP,
                        DateTime.Now
                    };
                    dictionary3.AddOrUpdate(user, infoList, (key, oldValue) => infoList);
                    dictionary.AddOrUpdate(user, infoList, (key, oldValue) => infoList);
                }
                else
                {
                    ArrayList list2 = new ArrayList {
                        iP,
                        now
                    };
                    dictionary.GetOrAdd(user, list2);
                    insert_online(iP, user, user_type, now, now);
                }
                if (DateTime.Compare(Convert.ToDateTime(HttpContext.Current.Application[str2].ToString()).AddMinutes(1.0), DateTime.Now) < 0)
                {
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application[str2] = DateTime.Now;
                    HttpContext.Current.Application.UnLock();
                    update_online();
                }
            }
        }
        
        protected static void update_online()
        {
            string str = "online_User_Key";
            HttpContext.Current.Application.Lock();
            ConcurrentDictionary<string, object> dictionary = HttpContext.Current.Application[str] as ConcurrentDictionary<string, object>;
            HttpContext.Current.Application.UnLock();
            List<CommandInfo> list = new List<CommandInfo>();
            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                ArrayList list2 = pair.Value as ArrayList;
                string str3 = string.Format("update  cz_stat_online set ip=@ip, last_time=@last_time where u_name =@u_name ", new object[0]);
                SqlParameter[] parameterArray2 = new SqlParameter[3];
                SqlParameter parameter = new SqlParameter("@ip", SqlDbType.NVarChar) {
                    Value = list2[0].ToString()
                };
                parameterArray2[0] = parameter;
                SqlParameter parameter2 = new SqlParameter("@last_time", SqlDbType.DateTime) {
                    Value = list2[1].ToString()
                };
                parameterArray2[1] = parameter2;
                SqlParameter parameter3 = new SqlParameter("@u_name", SqlDbType.NVarChar) {
                    Value = pair.Key
                };
                parameterArray2[2] = parameter3;
                SqlParameter[] parameterArray = parameterArray2;
                CommandInfo item = new CommandInfo {
                    CommandText = str3,
                    Parameters = parameterArray
                };
                list.Add(item);
                dictionary.TryRemove(pair.Key, out _);
            }
            if (list.Count > 0)
            {
                CallBLL.CzStatOnlineService.executte_sql(list);
            }
        }
        protected bool IsLotteryExist(string lotteryId)
        {
            DataTable lotteryList = this.GetLotteryList();
            foreach (DataRow row in lotteryList.Rows)
            {
                if (row["id"].ToString().Equals(lotteryId))
                {
                    return true;
                }
            }
            return false;
        }
        public DataTable GetLotteryList()
        {
            if (CacheHelper.GetCache("cz_lottery_FileCacheKey") != null)
            {
                return (CacheHelper.GetCache("cz_lottery_FileCacheKey") as DataTable);
            }
            DataTable table = CallBLL.cz_lottery_bll.GetList().Tables[0];
            CacheHelper.SetCache("cz_lottery_FileCacheKey", table);
            CacheHelper.SetPublicFileCache("cz_lottery_FileCacheKey", table, PageBase.GetPublicForderPath(FileCacheHelper.get_LotteryCachesFileName()));
            return table;
        }
        protected void IsLotteryExist(string lotteryId, string msgCode, string isSuccess, string url)
        {
            bool flag = false;
            DataTable cache = CacheHelper.GetCache("cz_lottery_FileCacheKey") as DataTable;
            if (cache == null)
            {
                cache = CallBLL.cz_lottery_bll.GetList().Tables[0];
                CacheHelper.SetCache("cz_lottery_FileCacheKey", cache);
                CacheHelper.SetPublicFileCache("cz_lottery_FileCacheKey", cache, PageBase.GetPublicForderPath(FileCacheHelper.get_LotteryCachesFileName()));
            }
            foreach (DataRow row in cache.Rows)
            {
                if (row["id"].ToString().Equals(lotteryId))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                HttpContext.Current.Response.Redirect($"/MessagePage.aspx?code={msgCode}&issuccess={isSuccess}&url={HttpContext.Current.Server.UrlEncode(url)}");
                HttpContext.Current.Response.End();
            }
        }
        protected void IsOpenLottery()
        {
            if (((HttpContext.Current.Request.Path.ToLower().IndexOf("betimes_") >= 0) && (HttpContext.Current.Request.Path.ToLower().IndexOf("l_six/betimes_") <= -1)) && (HttpContext.Current.Request.Path.ToLower().IndexOf("index.aspx") <= -1))
            {
                string lotteryId = LSRequest.qq("lid");
                this.IsLotteryExist(lotteryId, "u100032", "1", "");
                int num = Convert.ToInt32(lotteryId);
                DataTable table = null;
                switch (num)
                {
                    case 0:
                        table = CallBLL.cz_phase_kl10_bll.IsPhaseClose();
                        break;

//                    case 1:
//                        table = CallBLL.cz_phase_cqsc_bll.IsPhaseClose();
//                        break;
//
//                    case 2:
//                        table = CallBLL.cz_phase_pk10_bll.IsPhaseClose();
//                        break;
//
//                    case 3:
//                        table = CallBLL.cz_phase_xync_bll.IsPhaseClose();
//                        break;
//
//                    case 4:
//                        table = CallBLL.cz_phase_jsk3_bll.IsPhaseClose();
//                        break;
//
//                    case 5:
//                        table = CallBLL.cz_phase_kl8_bll.IsPhaseClose();
//                        break;
//
//                    case 6:
//                        table = CallBLL.cz_phase_k8sc_bll.IsPhaseClose();
//                        break;
//
//                    case 7:
//                        table = CallBLL.cz_phase_pcdd_bll.IsPhaseClose();
//                        break;
//
//                    case 8:
//                        table = CallBLL.cz_phase_pkbjl_bll.IsPhaseClose();
//                        break;
//
//                    case 9:
//                        table = CallBLL.cz_phase_xyft5_bll.IsPhaseClose();
//                        break;
//
//                    case 10:
//                        table = CallBLL.cz_phase_jscar_bll.IsPhaseClose();
//                        break;
//
//                    case 11:
//                        table = CallBLL.cz_phase_speed5_bll.IsPhaseClose();
//                        break;
//
//                    case 12:
//                        table = CallBLL.cz_phase_jspk10_bll.IsPhaseClose();
//                        break;
//
//                    case 13:
//                        table = CallBLL.cz_phase_jscqsc_bll.IsPhaseClose();
//                        break;
//
//                    case 14:
//                        table = CallBLL.cz_phase_jssfc_bll.IsPhaseClose();
//                        break;
//
//                    case 15:
//                        table = CallBLL.cz_phase_jsft2_bll.IsPhaseClose();
//                        break;
//
//                    case 0x10:
//                        table = CallBLL.cz_phase_car168_bll.IsPhaseClose();
//                        break;
//
//                    case 0x11:
//                        table = CallBLL.cz_phase_ssc168_bll.IsPhaseClose();
//                        break;
//
//                    case 0x12:
//                        table = CallBLL.cz_phase_vrcar_bll.IsPhaseClose();
//                        break;
//
//                    case 0x13:
//                        table = CallBLL.cz_phase_vrssc_bll.IsPhaseClose();
//                        break;
//
//                    case 20:
//                        table = CallBLL.cz_phase_xyftoa_bll.IsPhaseClose();
//                        break;
//
//                    case 0x15:
//                        table = CallBLL.cz_phase_xyftsg_bll.IsPhaseClose();
//                        break;
//
//                    case 0x16:
//                        table = CallBLL.cz_phase_happycar_bll.IsPhaseClose();
//                        break;
                }
                DataRow[] rowArray = this.GetLotteryList().Select($"id={num}");
                if (table.Rows[0]["isopen"].ToString().Equals("0"))
                {
                    string str2 = HttpContext.Current.Request.FilePath.ToLower();
                    FileInfo info = new FileInfo(HttpContext.Current.Request.FilePath);
                    string str3 = info.Extension.ToLower();
                    if ((str2.Substring(str2.LastIndexOf("/") + 1).Trim().IndexOf("betimes_") > -1) && (str3.IndexOf("aspx") > -1))
                    {
                        string str5 = HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath);
                        HttpContext.Current.Response.Redirect($"/noopen.aspx?lid={num}&path={str5}", true);
                        HttpContext.Current.Response.End();
                    }
                }
            }
        }
        public string UserCurrentSkin { get; set; }

        protected void RedirectReport()
        {
            int num = 1;
            if (HttpContext.Current.Session["user_state"].ToString().Equals(num.ToString()) && ((((((((HttpContext.Current.Request.Path.ToLower().IndexOf("l_six") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_cqsc") > 0)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("l_k3") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_kl10") > 0))) || (((HttpContext.Current.Request.Path.ToLower().IndexOf("l_kl8") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_pk10") > 0)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("l_xync") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_k8sc") > 0)))) || ((((HttpContext.Current.Request.Path.ToLower().IndexOf("l_pcdd") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_xyft5") > 0)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("l_pkbjl") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_jscar") > 0))) || (((HttpContext.Current.Request.Path.ToLower().IndexOf("l_speed5") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_jspk10") > 0)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("l_jscqsc") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_jssfc") > 0))))) || (((((HttpContext.Current.Request.Path.ToLower().IndexOf("l_jsft2") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_car168") > 0)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("l_ssc168") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_vrcar") > 0))) || (((HttpContext.Current.Request.Path.ToLower().IndexOf("l_vrssc") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_xyftoa") > 0)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("l_xyftsg") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("l_happycar") > 0)))) || ((((HttpContext.Current.Request.Path.ToLower().IndexOf("account") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("viewbill") > 0)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("autolet") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("autolet_kc") > 0))) || (((HttpContext.Current.Request.Path.ToLower().IndexOf("autolet_six") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("oddswt.aspx") > 0)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("systemset_kc") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("systemset_six") > 0)))))) || (((((HttpContext.Current.Request.Path.ToLower().IndexOf("tradingset") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("tradingset") > 0)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("oddsset_kc") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("oddsset_six") > 0))) || (((HttpContext.Current.Request.Path.ToLower().IndexOf("news_add") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("news_edit") > 0)) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("news_list") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("awardperiod") > 0)))) || ((HttpContext.Current.Request.Path.ToLower().IndexOf("reportbackupmanage") > 0) || (HttpContext.Current.Request.Path.ToLower().IndexOf("billbackupmanage") > 0)))) || (HttpContext.Current.Request.Path.ToLower().IndexOf("viewonlineuser") > 0)))
            {
                string str = LSRequest.qq("lid");
                HttpContext.Current.Response.Redirect($"/ReportSearch/Report.aspx?lid={str}", true);
                HttpContext.Current.Response.End();
            }
        }
        protected void ForcedModifyPassword()
        {
            string str = HttpContext.Current.Request.Path.ToLower();
            if (!(string.IsNullOrEmpty(HttpContext.Current.Session["modifypassword"].ToString()) || (str.IndexOf("resetpasswd.aspx") >= 0)))
            {
                HttpContext.Current.Response.Redirect(string.Format("ResetPasswd.aspx", new object[0]), true);
                HttpContext.Current.Response.End();
            }
        }
        public void AgentCurrentLottery()
        {
            int num;
            string str = HttpContext.Current.Request.Path.ToLower();
            if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_six") > -1)
            {
                num = 1;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 100;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_kl10") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 0;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_cqsc") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 1;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_pk10") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 2;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_xync") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 3;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_k3") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 4;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_kl8") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 5;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_k8sc") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 6;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_pcdd") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 7;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_xyft5") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 9;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_pkbjl") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 8;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_jscar") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 10;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_speed5") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 11;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_jscqsc") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 13;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_jspk10") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 12;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_jssfc") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 14;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_jsft2") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 15;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_car168") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 0x10;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_ssc168") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 0x11;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_vrcar") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 0x12;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_vrssc") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 0x13;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_xyftoa") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 20;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_xyftsg") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                num = 0x15;
                CacheHelper.SetCache("cachecurrentlid", num.ToString());
            }
            else if (HttpContext.Current.Request.Path.ToLower().IndexOf("l_happycar") > -1)
            {
                num = 2;
                CacheHelper.SetCache("cachecurrentmlid", num.ToString());
                CacheHelper.SetCache("cachecurrentlid", 0x16.ToString());
            }
        }
        public string GetAlert(string message)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script>");
            builder.AppendFormat(" alert('{0}');", message);
            builder.Append("</script>");
            return builder.ToString();
        }
        protected string GetNav()
        {
            string str = HttpContext.Current.Session["user_name"].ToString();
            agent_userinfo_session uModel = HttpContext.Current.Session[str + "lottery_session_user_info"] as agent_userinfo_session;
            StringBuilder builder = new StringBuilder();
            builder.Append("            {");
            if (HttpContext.Current.Session["user_state"].ToString().Equals("0"))
            {
                if (HttpContext.Current.Session["child_user_name"] != null)
                {
                    if (HttpContext.Current.Session["user_type"].ToString().Equals("zj"))
                    {
                        if (CallBLL.CzUsersChildService.GetPermissionsName(HttpContext.Current.Session["child_user_name"].ToString()).IndexOf("po_1_1") > -1)
                        {
                            builder.Append("    \"即時注單\": {");
                            builder.Append(this.GetHtml_JSZD(uModel));
                            builder.Append("    },");
                        }
                    }
                    else if (CallBLL.CzUsersChildService.GetPermissionsName(HttpContext.Current.Session["child_user_name"].ToString()).IndexOf("po_5_1") > -1)
                    {
                        builder.Append("    \"即時注單\": {");
                        builder.Append(this.GetHtml_JSZD(uModel));
                        builder.Append("    },");
                    }
                }
                else
                {
                    builder.Append("    \"即時注單\": {");
                    builder.Append(this.GetHtml_JSZD(uModel));
                    builder.Append("    },");
                }
            }
            if (HttpContext.Current.Session["user_state"].ToString().Equals("0"))
            {
                if (HttpContext.Current.Session["child_user_name"] != null)
                {
                    if (HttpContext.Current.Session["user_type"].ToString().Equals("zj"))
                    {
                        if (CallBLL.CzUsersChildService.GetPermissionsName(HttpContext.Current.Session["child_user_name"].ToString()).IndexOf("po_2_1") > -1)
                        {
                            builder.Append("    \"用戶管理\": {");
                            builder.Append(this.GetHtml_YHGL(uModel));
                            builder.Append("    },");
                        }
                    }
                    else if (CallBLL.CzUsersChildService.GetPermissionsName(HttpContext.Current.Session["child_user_name"].ToString()).IndexOf("po_6_1") > -1)
                    {
                        builder.Append("    \"用戶管理\": {");
                        builder.Append(this.GetHtml_YHGL(uModel));
                        builder.Append("    },");
                    }
                }
                else
                {
                    builder.Append("    \"用戶管理\": {");
                    builder.Append(this.GetHtml_YHGL(uModel));
                    builder.Append("    },");
                }
            }
            if (HttpContext.Current.Session["user_type"].ToString().Equals("zj"))
            {
                builder.Append("    \"内部管理\": {");
                builder.Append(this.GetHtml_NBGL(uModel));
                builder.Append("    },");
            }
            builder.Append("    \"個人管理\": {");
            builder.Append(this.GetHtml_GRGL(uModel));
            builder.Append("    },");
            builder.Append("    \"報表查詢\": {");
            builder.Append(this.GetHtml_BBCX(uModel));
            builder.Append("    },");
            builder.Append("    \"歷史開獎\": {");
            builder.Append("        \"ut\": [");
            builder.Append("            \"歷史開獎|/LotteryPeriod/HistoryLottery.aspx\"");
            builder.Append("        ]");
            builder.Append("    },");
            builder.Append("    \"站内消息\": {");
            builder.Append("        \"ut\": [");
            builder.Append("            \"站内消息|/NewsManage/NewsList.aspx\"");
            builder.Append("        ]");
            builder.Append("    },");
            builder.Append("    \"安全退出\": {");
            builder.Append("        \"ut\": [");
            builder.Append("            \"Quit.aspx\"");
            builder.Append("        ]");
            builder.Append("    }");
            builder.Append("}");
            return builder.ToString();
        }
        private string GetHtml_YHGL(agent_userinfo_session uModel)
        {
            StringBuilder builder = new StringBuilder();
            if (!HttpContext.Current.Session["user_type"].ToString().Equals("zj"))
            {
                if (uModel.get_u_type().Equals("fgs"))
                {
                    builder.Append("        \"ut\": [");
                    builder.Append("            \"股东|account/gd_list.aspx\",");
                    builder.Append("            \"總代理|account/zd_list.aspx\",");
                    builder.Append("            \"代理|account/dl_list.aspx\",");
                    builder.Append("            \"會員|account/hy_list.aspx\"");
                    if (uModel.get_users_child_session() == null)
                    {
                        builder.Append("            ,\"子賬號|account/child_list.aspx\"");
                    }
                    builder.Append("        ]");
                }
                else if (uModel.get_u_type().Equals("gd"))
                {
                    builder.Append("        \"ut\": [");
                    builder.Append("            \"總代理|account/zd_list.aspx\",");
                    builder.Append("            \"代理|account/dl_list.aspx\",");
                    builder.Append("            \"會員|account/hy_list.aspx\"");
                    if (uModel.get_users_child_session() == null)
                    {
                        builder.Append("            ,\"子賬號|account/child_list.aspx\"");
                    }
                    builder.Append("        ]");
                }
                else if (uModel.get_u_type().Equals("zd"))
                {
                    builder.Append("        \"ut\": [");
                    builder.Append("            \"代理|account/dl_list.aspx\",");
                    builder.Append("            \"會員|account/hy_list.aspx\"");
                    if (uModel.get_users_child_session() == null)
                    {
                        builder.Append("            ,\"子賬號|account/child_list.aspx\"");
                    }
                    builder.Append("        ]");
                }
                else if (uModel.get_u_type().Equals("dl"))
                {
                    builder.Append("        \"ut\": [");
                    builder.Append("            \"會員|account/hy_list.aspx\"");
                    if (uModel.get_users_child_session() == null)
                    {
                        builder.Append("            ,\"子賬號|account/child_list.aspx\"");
                    }
                    builder.Append("        ]");
                }
                else
                {
                    builder.Append("        \"ut\": [ ]");
                }
            }
            else
            {
                builder.Append("        \"ut\": [");
                builder.Append("            \"分公司|account/fgs_list.aspx\",");
                builder.Append("            \"股东|account/gd_list.aspx\",");
                builder.Append("            \"總代理|account/zd_list.aspx\",");
                builder.Append("            \"代理|account/dl_list.aspx\",");
                builder.Append("            \"會員|account/hy_list.aspx\"");
                if (uModel.get_users_child_session() == null)
                {
                    builder.Append("            ,\"子賬號|account/child_list.aspx\"");
                }
                DataTable table = this.GetLotteryList().DefaultView.ToTable(true, new string[] { "master_id" });
                string str = "";
                foreach (DataRow row in table.Rows)
                {
                    int num = 1;
                    if (row["master_id"].ToString().Equals(num.ToString()))
                    {
                        str = "            ,\"出貨會員|account/filluser_list.aspx\"";
                        break;
                    }
                }
                if (uModel.get_users_child_session() != null)
                {
                    if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_2_3") > -1)
                    {
                        builder.Append(str);
                    }
                }
                else
                {
                    builder.Append(str);
                }
                builder.Append("        ]");
            }
            return builder.ToString();
        }
        private string GetHtml_BBCX(agent_userinfo_session uModel)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("        \"ut\": [");
            if (FileCacheHelper.get_IsViewNewReportMenu().Equals("1"))
            {
                builder.Append("            \"(新)報表查詢|ReportSearch/ReportNew.aspx\",");
            }
            builder.Append("            \"報表查詢|ReportSearch/Report.aspx\"");
            builder.Append("        ]");
            return builder.ToString();
        }

        private string GetHtml_GRGL(agent_userinfo_session uModel)
        {
            StringBuilder builder = new StringBuilder();
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.Append("        \"ut\": [");
                builder.Append("            \"登陸日誌|ViewLog/LoginLog.aspx\",");
                builder.Append("            \"變更密碼|EditPwd.aspx|0\"");
                builder.Append("        ]");
            }
            else
            {
                builder.Append("        \"ut\": [");
                builder.Append("            \"信用資料|CreditInfo.aspx\",");
                if (uModel.get_users_child_session() == null)
                {
                    builder.Append("            \"登陸日誌|ViewLog/LoginLog.aspx\",");
                }
                else if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_6_1") > -1)
                {
                    builder.Append("            \"登陸日誌|ViewLog/LoginLog.aspx\",");
                }
                if (uModel.get_u_type().Equals("fgs"))
                {
                    if ((uModel.get_users_child_session() == null) && (uModel.get_six_op_odds().Equals(1) || uModel.get_kc_op_odds().Equals(1)))
                    {
                        builder.Append("            \"操盤日誌|ViewLog/ViewFgsOptOddsLog.aspx\",");
                    }
                    else if (((uModel.get_users_child_session() != null) && (uModel.get_six_op_odds().Equals(1) || uModel.get_kc_op_odds().Equals(1))) && (uModel.get_users_child_session().get_permissions_name().IndexOf("po_5_3") > -1))
                    {
                        builder.Append("            \"操盤日誌|ViewLog/ViewFgsOptOddsLog.aspx\",");
                    }
                }
                builder.Append("            \"變更密碼|EditPwd.aspx|0\",");
                if (HttpContext.Current.Session["user_state"].ToString().Equals("0"))
                {
                    if (uModel.get_users_child_session() == null)
                    {
                        builder.Append("            \"自動補貨設定|/AutoLet/AutoLet_kc.aspx\",");
                    }
                    else if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_5_2") > -1)
                    {
                        builder.Append("            \"自動補貨設定|/AutoLet/AutoLet_kc.aspx\",");
                    }
                }
                builder.Append("            \"自動補貨變更記錄|/ViewLog/ViewAutoSaleLog.aspx\"");
                if ((uModel.get_u_type().Equals("fgs") && (uModel.get_six_op_odds().Equals(1) || uModel.get_kc_op_odds().Equals(1))) && uModel.get_a_state().Equals(0))
                {
                    if (uModel.get_users_child_session() == null)
                    {
                        builder.Append("            ,\"微調列表|/OddsSet/OddsWT.aspx\"");
                    }
                    else if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_5_3") > -1)
                    {
                        builder.Append("            ,\"微調列表|/OddsSet/OddsWT.aspx\"");
                    }
                }
                builder.Append("        ]");
            }
            return builder.ToString();
        }
        public cz_admin_subsystem IsChildSystem() => 
            CallBLL.CzAdminSubsystemService.GetModel();

        public bool IsChildSync()
        {
            cz_admin_subsystem _subsystem = this.IsChildSystem();
            if (_subsystem == null)
            {
                return false;
            }
            if (!_subsystem.get_flag().Equals(1))
            {
                return false;
            }
//            DbHelperSQL_Ex.connectionString = string.Format(PubConstant.get_ConnectionStringExtend(), _subsystem.get_conn());
            DataSet set = DbHelperSQL.Query($"select top 1 * from zk_subsys where sys_id='{_subsystem.get_sys_id()}'".ToString(), null);
            return ((((set != null) && (set.Tables.Count > 0)) && (set.Tables[0].Rows.Count > 0)) && set.Tables[0].Rows[0]["sync"].ToString().Equals("1"));
        }
       private string GetHtml_NBGL(agent_userinfo_session uModel)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("        \"ut\": [");
            if (HttpContext.Current.Session["child_user_name"] == null)
            {
                builder.Append("            \"注單搜索|BillSearch.aspx\",");
            }
            else if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_3_5") > -1)
            {
                builder.Append("            \"注單搜索|BillSearch.aspx\",");
            }
            int num = 0;
            if (HttpContext.Current.Session["user_state"].ToString().Equals(num.ToString()))
            {
                if (uModel.get_users_child_session() == null)
                {
                    if (!this.IsChildSync())
                    {
                        builder.Append("            \"彩種配置|LotteryConfig.aspx\",");
                    }
                    builder.Append("            \"系統初始設定|/SystemSet/SystemSet_kc.aspx\",");
                }
                else
                {
                    if ((uModel.get_users_child_session().get_permissions_name().IndexOf("po_2_2") > -1) && !this.IsChildSync())
                    {
                        builder.Append("            \"彩種配置|LotteryConfig.aspx\",");
                    }
                    if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_3_2") > -1)
                    {
                        builder.Append("            \"系統初始設定|/SystemSet/SystemSet_kc.aspx\",");
                    }
                }
                if (FileCacheHelper.get_ManageZJProfit().Equals("1") && ((uModel.get_users_child_session() == null) || uModel.get_users_child_session().get_is_admin().Equals(1)))
                {
                    builder.Append("            \"總監盈利設置|/ManageZJProfit/Manage_ZJ_Profit.aspx\",");
                }
                if (uModel.get_users_child_session() == null)
                {
                    builder.Append("            \"交易設定|TradingSet.aspx\",");
                    builder.Append("            \"賠率設定|/OddsSet/OddsSet_kc.aspx\",");
                }
                else if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_3_3") > -1)
                {
                    builder.Append("            \"交易設定|TradingSet.aspx\",");
                    builder.Append("            \"賠率設定|/OddsSet/OddsSet_kc.aspx\",");
                }
                if (uModel.get_u_type().Equals("fgs") && (uModel.get_six_op_odds().Equals(1) || uModel.get_six_op_odds().Equals(1)))
                {
                    if (uModel.get_users_child_session() == null)
                    {
                        builder.Append("            \"微調列表|/OddsSet/OddsWT.aspx\",");
                    }
                    else if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_5_3") > -1)
                    {
                        builder.Append("            \"微調列表|/OddsSet/OddsWT.aspx\",");
                    }
                }
                if (uModel.get_users_child_session() == null)
                {
                    builder.Append("            \"站內消息管理|/NewsManage/news_list.aspx\",");
                }
                else if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_3_4") > -1)
                {
                    builder.Append("            \"站內消息管理|/NewsManage/news_list.aspx\",");
                }
                if (uModel.get_users_child_session() == null)
                {
                    builder.Append("            \"獎期管理|/LotteryPeriod/AwardPeriod.aspx\",");
                }
                else if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_3_1") > -1)
                {
                    builder.Append("            \"獎期管理|/LotteryPeriod/AwardPeriod.aspx\",");
                }
                builder.Append("            \"報表備份|/ReportBackupManage/ReportBackup.aspx|1\",");
                builder.Append("            \"注單備份|/BillBackupManage/BillBackup.aspx|1\",");
            }
            if (uModel.get_users_child_session() == null)
            {
                builder.Append("            \"操盤日誌|/ViewLog/LogOddsChange.aspx\",");
            }
            else if (uModel.get_users_child_session().get_permissions_name().IndexOf("po_3_1") > -1)
            {
                builder.Append("            \"操盤日誌|/ViewLog/LogOddsChange.aspx\",");
            }
            builder.Append("            \"系統日誌|/ViewLog/LogSystem.aspx\"");
            builder.Append("        ]");
            return builder.ToString();
        }

        private string GetHtml_JSZD(agent_userinfo_session uModel)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_SIX\": [");
            }
            else
            {
                builder.Append("        \"L_SIX\": [");
            }
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.Append("            \"特碼|Betimes_tmZX2.aspx\",");
            }
            else
            {
                builder.Append("            \"特碼|Betimes_tmZX2.aspx\",");
            }
            builder.Append("            \"正碼|Betimes_zm.aspx\",");
            builder.Append("            \"正碼特|Betimes_zmt1.aspx\",");
            builder.Append("            \"連碼|Betimes_lm.aspx\",");
            builder.Append("            \"不中|Betimes_bz.aspx\",");
            builder.Append("            \"正碼1-6|Betimes_zm1-6.aspx\",");
            builder.Append("            \"特碼生肖色波|Betimes_tmsxsb.aspx\",");
            builder.Append("            \"生肖尾數|Betimes_sxws.aspx\",");
            builder.Append("            \"半波|Betimes_bb.aspx\",");
            builder.Append("            \"六肖...連|Betimes_lxl.aspx\",");
            builder.Append("            \"龍虎-特碼攤子|Betimes_lhtmtz.aspx\",");
            builder.Append("            \"七碼五行|Betimes_qmwx.aspx\",");
            builder.Append("            \"帳單|../L_SIX/Bill.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|NewBet_six.aspx|1\"", 100);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_KL10\": [");
            }
            else
            {
                builder.Append("        \"L_KL10\": [");
            }
            builder.Append("            \"第一球|Betimes_1.aspx\",");
            builder.Append("            \"第二球|Betimes_2.aspx\",");
            builder.Append("            \"第三球|Betimes_3.aspx\",");
            builder.Append("            \"第四球|Betimes_4.aspx\",");
            builder.Append("            \"第五球|Betimes_5.aspx\",");
            builder.Append("            \"第六球|Betimes_6.aspx\",");
            builder.Append("            \"第七球|Betimes_7.aspx\",");
            builder.Append("            \"第八球|Betimes_8.aspx\",");
            builder.Append("            \"總和、龍虎|Betimes_lh.aspx\",");
            builder.Append("            \"連碼|Betimes_lm.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 0);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_CQSC\": [");
            }
            else
            {
                builder.Append("        \"L_CQSC\": [");
            }
            builder.Append("            \"總項盤口|Betimes_zx.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 1);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_PK10\": [");
            }
            else
            {
                builder.Append("        \"L_PK10\": [");
            }
            builder.Append("            \"冠、亞軍 組合|Betimes_1.aspx\",");
            builder.Append("            \"三、四、伍、六名|Betimes_2.aspx\",");
            builder.Append("            \"七、八、九、十名|Betimes_3.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 2);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_XYNC\": [");
            }
            else
            {
                builder.Append("        \"L_XYNC\": [");
            }
            builder.Append("            \"第一球|Betimes_1.aspx\",");
            builder.Append("            \"第二球|Betimes_2.aspx\",");
            builder.Append("            \"第三球|Betimes_3.aspx\",");
            builder.Append("            \"第四球|Betimes_4.aspx\",");
            builder.Append("            \"第五球|Betimes_5.aspx\",");
            builder.Append("            \"第六球|Betimes_6.aspx\",");
            builder.Append("            \"第七球|Betimes_7.aspx\",");
            builder.Append("            \"第八球|Betimes_8.aspx\",");
            builder.Append("            \"總和、家禽野獸|Betimes_zh.aspx\",");
            builder.Append("            \"連碼|Betimes_lm.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 3);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_K3\": [");
            }
            else
            {
                builder.Append("        \"L_K3\": [");
            }
            builder.Append("            \"總項盤口|Betimes_zx.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 4);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_KL8\": [");
            }
            else
            {
                builder.Append("        \"L_KL8\": [");
            }
            builder.Append("            \"總和、比數、五行|Betimes_zh.aspx\",");
            builder.Append("            \"正碼|Betimes_zm.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 5);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_K8SC\": [");
            }
            else
            {
                builder.Append("        \"L_K8SC\": [");
            }
            builder.Append("            \"總項盤口|Betimes_zx.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 6);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_PCDD\": [");
            }
            else
            {
                builder.Append("        \"L_PCDD\": [");
            }
            builder.Append("            \"總項盤口|Betimes_zx.aspx\",");
            builder.Append("            \"特碼包三|Betimes_lm.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 7);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_XYFT5\": [");
            }
            else
            {
                builder.Append("        \"L_XYFT5\": [");
            }
            builder.Append("            \"冠、亞軍 組合|Betimes_1.aspx\",");
            builder.Append("            \"三、四、伍、六名|Betimes_2.aspx\",");
            builder.Append("            \"七、八、九、十名|Betimes_3.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 9);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_PKBJL\": [");
            }
            else
            {
                builder.Append("        \"L_PKBJL\": [");
            }
            builder.Append("            \"總項盤口|Betimes_1.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 8);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_JSCAR\": [");
            }
            else
            {
                builder.Append("        \"L_JSCAR\": [");
            }
            builder.Append("            \"冠、亞軍 組合|Betimes_1.aspx\",");
            builder.Append("            \"三、四、伍、六名|Betimes_2.aspx\",");
            builder.Append("            \"七、八、九、十名|Betimes_3.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 10);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_SPEED5\": [");
            }
            else
            {
                builder.Append("        \"L_SPEED5\": [");
            }
            builder.Append("            \"總項盤口|Betimes_zx.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 11);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_JSCQSC\": [");
            }
            else
            {
                builder.Append("        \"L_JSCQSC\": [");
            }
            builder.Append("            \"總項盤口|Betimes_zx.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 13);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_JSPK10\": [");
            }
            else
            {
                builder.Append("        \"L_JSPK10\": [");
            }
            builder.Append("            \"冠、亞軍 組合|Betimes_1.aspx\",");
            builder.Append("            \"三、四、伍、六名|Betimes_2.aspx\",");
            builder.Append("            \"七、八、九、十名|Betimes_3.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 12);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_JSSFC\": [");
            }
            else
            {
                builder.Append("        \"L_JSSFC\": [");
            }
            builder.Append("            \"第一球|Betimes_1.aspx\",");
            builder.Append("            \"第二球|Betimes_2.aspx\",");
            builder.Append("            \"第三球|Betimes_3.aspx\",");
            builder.Append("            \"第四球|Betimes_4.aspx\",");
            builder.Append("            \"第五球|Betimes_5.aspx\",");
            builder.Append("            \"第六球|Betimes_6.aspx\",");
            builder.Append("            \"第七球|Betimes_7.aspx\",");
            builder.Append("            \"第八球|Betimes_8.aspx\",");
            builder.Append("            \"總和、龍虎|Betimes_lh.aspx\",");
            builder.Append("            \"連碼|Betimes_lm.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 14);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_JSFT2\": [");
            }
            else
            {
                builder.Append("        \"L_JSFT2\": [");
            }
            builder.Append("            \"冠、亞軍 組合|Betimes_1.aspx\",");
            builder.Append("            \"三、四、伍、六名|Betimes_2.aspx\",");
            builder.Append("            \"七、八、九、十名|Betimes_3.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 15);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_CAR168\": [");
            }
            else
            {
                builder.Append("        \"L_CAR168\": [");
            }
            builder.Append("            \"冠、亞軍 組合|Betimes_1.aspx\",");
            builder.Append("            \"三、四、伍、六名|Betimes_2.aspx\",");
            builder.Append("            \"七、八、九、十名|Betimes_3.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 0x10);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_SSC168\": [");
            }
            else
            {
                builder.Append("        \"L_SSC168\": [");
            }
            builder.Append("            \"總項盤口|Betimes_zx.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 0x11);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_VRCAR\": [");
            }
            else
            {
                builder.Append("        \"L_VRCAR\": [");
            }
            builder.Append("            \"冠、亞軍 組合|Betimes_1.aspx\",");
            builder.Append("            \"三、四、伍、六名|Betimes_2.aspx\",");
            builder.Append("            \"七、八、九、十名|Betimes_3.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 0x12);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_VRSSC\": [");
            }
            else
            {
                builder.Append("        \"L_VRSSC\": [");
            }
            builder.Append("            \"總項盤口|Betimes_zx.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 0x13);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_XYFTOA\": [");
            }
            else
            {
                builder.Append("        \"L_XYFTOA\": [");
            }
            builder.Append("            \"冠、亞軍 組合|Betimes_1.aspx\",");
            builder.Append("            \"三、四、伍、六名|Betimes_2.aspx\",");
            builder.Append("            \"七、八、九、十名|Betimes_3.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 20);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_XYFTSG\": [");
            }
            else
            {
                builder.Append("        \"L_XYFTSG\": [");
            }
            builder.Append("            \"冠、亞軍 組合|Betimes_1.aspx\",");
            builder.Append("            \"三、四、伍、六名|Betimes_2.aspx\",");
            builder.Append("            \"七、八、九、十名|Betimes_3.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 0x15);
            }
            builder.Append("        ]");
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                builder.Append("        ,\"L_HAPPYCAR\": [");
            }
            else
            {
                builder.Append("        \"L_HAPPYCAR\": [");
            }
            builder.Append("            \"冠、亞軍 組合|Betimes_1.aspx\",");
            builder.Append("            \"三、四、伍、六名|Betimes_2.aspx\",");
            builder.Append("            \"七、八、九、十名|Betimes_3.aspx\",");
            builder.Append("            \"帳單|../Bill_kc.aspx|1\",");
            builder.Append("            \"備份|../BillBackup_kc.aspx|1\"");
            if (uModel.get_u_type().Equals("zj"))
            {
                builder.AppendFormat("            ,\"實時滾單|../NewBet_kc.aspx|1\"", 0x16);
            }
            builder.Append("        ]");
            return builder.ToString();
        }
        
        public string GetAlert(string message, string okStr, string closeStr, string openStr)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script>");
            builder.Append("seajs.use('alert',function(myAlert){");
            builder.Append("myAlert({");
            builder.AppendFormat("content: '{0}',", message);
            builder.Append("okCallBack: function () { " + okStr + "},");
            builder.Append("closeCallBack:function () { " + closeStr + "},");
            builder.Append("openCallBack: function () { " + openStr + "}");
            builder.Append("})");
            builder.Append("});");
            builder.Append("</script>");
            return builder.ToString();
        }

        protected String get_GetPasswordLU()
        {
            //todo 是否开启高级密码
            return "";
        }

        protected void log_user_reset_password(string toString, string p1, string empty, object p3)
        {
            //todo 修改密码日志
            
        }

        public string LocationHref(string url)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script>");
            builder.AppendFormat(" location.href='{0}' ", url);
            builder.Append("</script>");
            return builder.ToString();
        }

       public static void update_online_user(string u_name)
       {
           string str = string.Format("update  cz_stat_online set  last_time=last_time-0.1 where u_name=@u_name ", new object[0]);
           SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@u_name", SqlDbType.NVarChar) };
           parameterArray[0].Value = u_name;
           CallBLL.CzStatOnlineService.executte_sql(str, parameterArray);
       }

       protected string get_YearLianArray()
       {
           throw new NotImplementedException();
       }

       protected string GetLotteryMasterID(DataTable lotteryDt)
       {
           throw new NotImplementedException();
       }
    }
}