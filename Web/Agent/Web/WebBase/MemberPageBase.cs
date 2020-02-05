using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
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
            CallBLL.cz_stat_online_bll.executte_sql(str3, parameterArray);
            string str4 = "";
            if (HttpContext.Current.Application[str] == null)
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[str2] = now;
                HttpContext.Current.Application.UnLock();
                str4 = $" select * from cz_stat_top_online  with(NOLOCK) where update_time > '{DateTime.Today}' and update_time < '{DateTime.Today.AddHours(24.0)}' ";
                if (CallBLL.cz_stat_top_online_bll.query_sql(str4).Rows.Count <= 0)
                {
                    str4 = $"insert into cz_stat_top_online values({1},'{DateTime.Now}') ";
                    CallBLL.cz_stat_top_online_bll.executte_sql(str4);
                }
            }
            else
            {
                int num = 1;
                string str5 = $" select count(1)  from cz_stat_online  with(NOLOCK) where last_time > '{now.AddMinutes(-3.0)}' ";
                DataTable table2 = CallBLL.cz_stat_online_bll.query_sql(str5, parameterArray);
                if (table2.Rows.Count > 0)
                {
                    num = int.Parse(table2.Rows[0][0].ToString());
                }
                str4 = $"select * from cz_stat_top_online with(NOLOCK) where update_time > '{DateTime.Today}' and update_time < '{DateTime.Today.AddHours(24.0)}' ";
                DataTable table3 = CallBLL.cz_stat_top_online_bll.query_sql(str4);
                if (table3.Rows.Count > 0)
                {
                    string s = table3.Rows[0]["top_cnt"].ToString();
                    if (num > int.Parse(s))
                    {
                        str4 = $"update cz_stat_top_online set top_cnt = {num}, update_time = '{now}' where update_time > '{DateTime.Today}' and update_time < '{DateTime.Today.AddHours(24.0)}' ";
                        CallBLL.cz_stat_top_online_bll.executte_sql(str4);
                    }
                }
                else
                {
                    str4 = $"insert into cz_stat_top_online values({num},'{now}') ";
                    CallBLL.cz_stat_top_online_bll.executte_sql(str4);
                }
            }
        }
         protected static void insert_online(string userIP, string user, string user_type, DateTime first_time, DateTime last_time)
        {
            string str = " select u_name from cz_stat_online with(NOLOCK) where u_name =@u_name ";
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@u_name", SqlDbType.NVarChar) };
            parameterArray[0].Value = user.Trim();
            if (CallBLL.cz_stat_online_bll.query_sql(str, parameterArray).Rows.Count > 0)
            {
                str = "update cz_stat_online set ip=@ip, first_time=@first_time, last_time=@last_time where u_name =@u_name ";
                SqlParameter[] parameterArray2 = new SqlParameter[] { new SqlParameter("@ip", SqlDbType.NVarChar), new SqlParameter("@first_time", SqlDbType.DateTime), new SqlParameter("@last_time", SqlDbType.DateTime), new SqlParameter("@u_name", SqlDbType.NVarChar) };
                parameterArray2[0].Value = userIP;
                parameterArray2[1].Value = first_time;
                parameterArray2[2].Value = last_time;
                parameterArray2[3].Value = user.Trim();
                CallBLL.cz_stat_online_bll.executte_sql(str, parameterArray2);
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
                    CallBLL.cz_stat_online_bll.executte_sql(str2, parameterArray3);
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
                CallBLL.cz_stat_online_bll.executte_sql(list);
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


    }
}