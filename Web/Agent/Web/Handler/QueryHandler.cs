using Entity;
using LotterySystem.Common.Redis;

namespace Agent.Web.Handler
{
    using Agent.Web.WebBase;
    using LotterySystem.Common;
    using LotterySystem.Model;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class QueryHandler : BaseHandler
    {
        public void get_ad(HttpContext context, ref string strResult)
        {
            ReturnResult result = new ReturnResult();
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                { 
                    "type",
                    "get_ad"
                }
            };
            agent_userinfo_session _session = new agent_userinfo_session();
            string str = context.Session["user_name"].ToString();
            _session = context.Session[str + "lottery_session_user_info"] as agent_userinfo_session;
            string str2 = _session.get_u_type();
            string str3 = LSRequest.qq("browserCode");
            string str4 = HttpContext.Current.Session["lottery_session_img_code_brower"];
            if (!((str3 != null) && str3.Equals(str4)))
            {
                this.Session.Abandon();
                result.set_success(300);
                result.set_data(dictionary);
                strResult = JsonHandle.ObjectToJson(result);
            }
            else
            {
                int num = base.get_current_master_id();
                string str5 = num.ToString();
                if (num == 0)
                {
                    str5 = "0,1,2";
                }
                else if (num == 1)
                {
                    str5 = "0,1";
                }
                else
                {
                    str5 = "0,2";
                }
                List<object> list = base.get_ad_level(str2, 3, str5);
                if (list == null)
                {
                    dictionary.Add("ad", new List<object>());
                }
                else
                {
                    dictionary.Add("ad", list);
                }
                if (_session.get_u_type().Equals("zj"))
                {
                    string str6 = "";
                    if (FileCacheHelper.get_RedisStatOnline().Equals(1))
                    {
                        str6 = base.get_online_cnt("redis");
                    }
                    else if (FileCacheHelper.get_RedisStatOnline().Equals(2))
                    {
                        str6 = base.get_online_cntStack("redis");
                    }
                    else
                    {
                        str6 = base.get_online_cnt();
                    }
                    dictionary.Add("online_cnt", str6);
                }
                string str7 = base.get_children_name();
                if (FileCacheHelper.get_RedisStatOnline().Equals(1) || FileCacheHelper.get_RedisStatOnline().Equals(2))
                {
                    bool flag = false;
                    if ((_session.get_users_child_session() != null) && _session.get_users_child_session().get_is_admin().Equals(1))
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        if (FileCacheHelper.get_RedisStatOnline().Equals(1))
                        {
                            base.CheckIsOut((str7 == "") ? str : str7);
                            base.stat_online_redis((str7 == "") ? str : str7, str2);
                        }
                        else if (FileCacheHelper.get_RedisStatOnline().Equals(2))
                        {
                            base.CheckIsOutStack((str7 == "") ? str : str7);
                            base.stat_online_redisStack((str7 == "") ? str : str7, str2);
                        }
                    }
                    if (FileCacheHelper.get_RedisStatOnline().Equals(1))
                    {
                        base.stat_online_redis_timer();
                    }
                    else if (FileCacheHelper.get_RedisStatOnline().Equals(2))
                    {
                        base.stat_online_redis_timerStack();
                    }
                }
                else
                {
                    MemberPageBase.stat_online((str7 == "") ? str : str7, str2);
                }
                string compareTime = LSRequest.qq("oldTime");
                DateTime now = DateTime.Now;
                Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                if (base.IsAutoUpdate(context.Session["user_name"].ToString(), compareTime))
                {
                    List<object> autoJPForAd = base.GetAutoJPForAd(compareTime, ref now);
                    if (autoJPForAd != null)
                    {
                        dictionary2.Add("tipsList", autoJPForAd);
                    }
                    else
                    {
                        dictionary2.Add("tipsList", new List<object>());
                    }
                }
                else
                {
                    dictionary2.Add("tipsList", new List<object>());
                }
                dictionary2.Add("timestamp", Utils.DateTimeToStamp(now));
                dictionary.Add("autoJP", dictionary2);
                result.set_data(dictionary);
                strResult = JsonHandle.ObjectToJson(result);
            }
        }

        public void get_currentphase(HttpContext context, ref string strResult)
        {
            ReturnResult result = new ReturnResult();
            result.set_success(400);
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                { 
                    "type",
                    "get_currentphase"
                }
            };
            if (context.Session["user_name"] == null)
            {
                context.Response.End();
            }
            else
            {
                List<object> list;
                DataSet currentByPhase;
                List<string> list2;
                DataTable table;
                int num;
                Dictionary<string, object> dictionary3;
                string str4;
                string str5;
                DataRow row;
                string str6;
                string str10;
                string str11;
                string str15;
                int num2;
                string str16;
                int num3;
                string str17;
                string str18;
                string str19;
                string str20;
                string str21;
                string str22;
                string str23;
                string str24;
                string str25;
                string str26;
                int num8;
                int num9;
                int num10;
                int num11;
                int num12;
                string str27;
                string str28;
                string str29;
                string str30;
                string str31;
                string str32;
                string str33;
                string str34;
                int num13;
                int num14;
                int num15;
                int num16;
                int num17;
                IEnumerator enumerator;
                int num35;
                string s = LSRequest.qq("lid");
                string str2 = LSRequest.qq("phase");
                string str3 = LSRequest.qq("tabletype");
                Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
                switch (int.Parse(s))
                {
                    case 0:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_kl10_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 8;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str19 = "";
                                        str15 = "";
                                        str16 = "";
                                        str20 = "";
                                        str21 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num3 = 1;
                                        while (num3 <= num)
                                        {
                                            str17 = "n" + num3;
                                            list2.Add(row[str17].ToString());
                                            num3++;
                                        }
                                        str19 = KL10Phase.get_zh(list2).ToString();
                                        str15 = KL10Phase.get_cl_zhdx(str19);
                                        str15 = base.SetWordColor(str15);
                                        str16 = KL10Phase.get_cl_zhds(str19);
                                        str16 = base.SetWordColor(str16);
                                        str20 = KL10Phase.get_cl_zhwsdx(str19);
                                        str20 = base.SetWordColor(str20);
                                        str21 = KL10Phase.get_cl_lh(list2[0].ToString(), list2[num - 1].ToString());
                                        str21 = base.SetWordColor(str21);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list4 = new List<string> {
                                            str19,
                                            str15,
                                            str16,
                                            str20,
                                            str21
                                        };
                                        dictionary3.Add("total", list4);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 1:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_cqsc_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str22 = "";
                                        str15 = "";
                                        str16 = "";
                                        str23 = "";
                                        str24 = "";
                                        str25 = "";
                                        str26 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        list2.Add(num8.ToString());
                                        list2.Add(num9.ToString());
                                        list2.Add(num10.ToString());
                                        list2.Add(num11.ToString());
                                        list2.Add(num12.ToString());
                                        num35 = (((num8 + num9) + num10) + num11) + num12;
                                        str22 = num35.ToString();
                                        str15 = CQSCPhase.get_cl_zhdx(str22);
                                        str15 = base.SetWordColor(str15);
                                        str16 = CQSCPhase.get_cl_zhds(str22);
                                        str16 = base.SetWordColor(str16);
                                        str23 = CQSCPhase.get_cl_lh(num8.ToString(), num12.ToString());
                                        str23 = base.SetWordColor(str23);
                                        str24 = CQSCPhase.get_cqsc_str(string.Concat(new object[] { num8, ",", num9, ",", num10 }));
                                        str25 = CQSCPhase.get_cqsc_str(string.Concat(new object[] { num9, ",", num10, ",", num11 }));
                                        str26 = CQSCPhase.get_cqsc_str(string.Concat(new object[] { num10, ",", num11, ",", num12 }));
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list5 = new List<string> {
                                            str22,
                                            str15,
                                            str16,
                                            str23,
                                            str24,
                                            str25,
                                            str26
                                        };
                                        dictionary3.Add("total", list5);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 2:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_pk10_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str27 = "";
                                        str28 = "";
                                        str29 = "";
                                        str30 = "";
                                        str31 = "";
                                        str32 = "";
                                        str33 = "";
                                        str34 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        num35 = num8 + num9;
                                        str27 = num35.ToString();
                                        if (Convert.ToInt32(str27) <= 11)
                                        {
                                            str28 = "小";
                                        }
                                        else
                                        {
                                            str28 = "大";
                                        }
                                        str28 = base.SetWordColor(str28);
                                        if ((Convert.ToInt32(str27) % 2) == 0)
                                        {
                                            str29 = "雙";
                                        }
                                        else
                                        {
                                            str29 = "單";
                                        }
                                        str29 = base.SetWordColor(str29);
                                        if (num8 > num17)
                                        {
                                            str30 = "龍";
                                        }
                                        else
                                        {
                                            str30 = "虎";
                                        }
                                        str30 = base.SetWordColor(str30);
                                        if (num9 > num16)
                                        {
                                            str31 = "龍";
                                        }
                                        else
                                        {
                                            str31 = "虎";
                                        }
                                        str31 = base.SetWordColor(str31);
                                        if (num10 > num15)
                                        {
                                            str32 = "龍";
                                        }
                                        else
                                        {
                                            str32 = "虎";
                                        }
                                        str32 = base.SetWordColor(str32);
                                        if (num11 > num14)
                                        {
                                            str33 = "龍";
                                        }
                                        else
                                        {
                                            str33 = "虎";
                                        }
                                        str33 = base.SetWordColor(str33);
                                        if (num12 > num13)
                                        {
                                            str34 = "龍";
                                        }
                                        else
                                        {
                                            str34 = "虎";
                                        }
                                        str34 = base.SetWordColor(str34);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list6 = new List<string> {
                                            str27,
                                            str28,
                                            str29,
                                            str30,
                                            str31,
                                            str32,
                                            str33,
                                            str34
                                        };
                                        dictionary3.Add("total", list6);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 3:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_xync_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            str6 = "";
                            str18 = "";
                            num2 = 0;
                            str15 = "";
                            str16 = "";
                            str20 = "";
                            string str35 = "";
                            table = currentByPhase.Tables[0];
                            num = 8;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        for (num3 = 1; num3 <= num; num3++)
                                        {
                                            str17 = "n" + num3;
                                            list2.Add(row[str17].ToString());
                                            num2 += Convert.ToInt32(row[str17]);
                                        }
                                        if (num2 == 0x54)
                                        {
                                            str15 = "和";
                                        }
                                        else if (num2 <= 0x53)
                                        {
                                            str15 = "小";
                                        }
                                        else
                                        {
                                            str15 = "大";
                                        }
                                        str15 = base.SetWordColor(str15);
                                        if ((num2 % 2) == 0)
                                        {
                                            str16 = "雙";
                                        }
                                        else
                                        {
                                            str16 = "單";
                                        }
                                        str16 = base.SetWordColor(str16);
                                        if ((num2 % 10) <= 4)
                                        {
                                            str20 = "尾小";
                                        }
                                        else
                                        {
                                            str20 = "尾大";
                                        }
                                        str20 = base.SetWordColor(str20);
                                        if (int.Parse(row["N1"].ToString().Trim()) > int.Parse(row["N8"].ToString().Trim()))
                                        {
                                            str35 = "家禽";
                                        }
                                        else
                                        {
                                            str35 = "野獸";
                                        }
                                        str35 = base.SetWordColor(str35);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list7 = new List<string> {
                                            num2.ToString(),
                                            str15,
                                            str16,
                                            str20,
                                            str35
                                        };
                                        dictionary3.Add("total", list7);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 4:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_jsk3_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str22 = "";
                                        str15 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        list2.Add(num8.ToString());
                                        list2.Add(num9.ToString());
                                        list2.Add(num10.ToString());
                                        num35 = (num8 + num9) + num10;
                                        str22 = num35.ToString();
                                        if ((num8 == num9) && (num9 == num10))
                                        {
                                            str15 = "通吃";
                                        }
                                        else if (Convert.ToInt32(str22) <= 10)
                                        {
                                            str15 = "小";
                                        }
                                        else
                                        {
                                            str15 = "大";
                                        }
                                        str15 = base.SetWordColor(str15);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list8 = new List<string> {
                                            str22,
                                            str15
                                        };
                                        dictionary3.Add("total", list8);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 5:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_kl8_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            dictionary3 = new Dictionary<string, object>();
                            num3 = 0;
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str22 = "";
                                        str15 = "";
                                        str16 = "";
                                        string str36 = "";
                                        string str37 = "";
                                        string str38 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        int num18 = Convert.ToInt32(row["n11"].ToString());
                                        int num19 = Convert.ToInt32(row["n12"].ToString());
                                        int num20 = Convert.ToInt32(row["n13"].ToString());
                                        int num21 = Convert.ToInt32(row["n14"].ToString());
                                        int num22 = Convert.ToInt32(row["n15"].ToString());
                                        int num23 = Convert.ToInt32(row["n16"].ToString());
                                        int num24 = Convert.ToInt32(row["n17"].ToString());
                                        int num25 = Convert.ToInt32(row["n18"].ToString());
                                        int num26 = Convert.ToInt32(row["n19"].ToString());
                                        int num27 = Convert.ToInt32(row["n20"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        list2.Add((num18 < 10) ? ("0" + num18) : num18.ToString());
                                        list2.Add((num19 < 10) ? ("0" + num19) : num19.ToString());
                                        list2.Add((num20 < 10) ? ("0" + num20) : num20.ToString());
                                        list2.Add((num21 < 10) ? ("0" + num21) : num21.ToString());
                                        list2.Add((num22 < 10) ? ("0" + num22) : num22.ToString());
                                        list2.Add((num23 < 10) ? ("0" + num23) : num23.ToString());
                                        list2.Add((num24 < 10) ? ("0" + num24) : num24.ToString());
                                        list2.Add((num25 < 10) ? ("0" + num25) : num25.ToString());
                                        list2.Add((num26 < 10) ? ("0" + num26) : num26.ToString());
                                        list2.Add((num27 < 10) ? ("0" + num27) : num27.ToString());
                                        num35 = ((((((((((((((((((num8 + num9) + num10) + num11) + num12) + num13) + num14) + num15) + num16) + num17) + num18) + num19) + num20) + num21) + num22) + num23) + num24) + num25) + num26) + num27;
                                        str22 = num35.ToString();
                                        if (Convert.ToInt32(str22) == 810)
                                        {
                                            str16 = "和";
                                        }
                                        else if ((Convert.ToInt32(str22) % 2) == 0)
                                        {
                                            str16 = "雙";
                                        }
                                        else
                                        {
                                            str16 = "單";
                                        }
                                        str16 = base.SetWordColor(str16);
                                        if (Convert.ToInt32(str22) > 810)
                                        {
                                            str15 = "大";
                                        }
                                        else if (Convert.ToInt32(str22) < 810)
                                        {
                                            str15 = "小";
                                        }
                                        else
                                        {
                                            str15 = "和";
                                        }
                                        str15 = base.SetWordColor(str15);
                                        int num28 = 0;
                                        int num29 = 0;
                                        int num30 = 0;
                                        while (num30 < 20)
                                        {
                                            if ((int.Parse(table.Rows[num3]["n" + (num30 + 1)].ToString().Trim()) % 2) == 0)
                                            {
                                                num29++;
                                            }
                                            else
                                            {
                                                num28++;
                                            }
                                            num30++;
                                        }
                                        str36 = KL8Phase.get_cl_dsh(num28.ToString(), num29.ToString());
                                        str36 = base.SetWordColor(str36);
                                        int num31 = 0;
                                        int num32 = 0;
                                        for (num30 = 0; num30 < 20; num30++)
                                        {
                                            if (int.Parse(table.Rows[num3]["n" + (num30 + 1)].ToString().Trim()) <= 40)
                                            {
                                                num31++;
                                            }
                                            else
                                            {
                                                num32++;
                                            }
                                        }
                                        str37 = KL8Phase.get_cl_qhh(num31.ToString(), num32.ToString());
                                        str37 = base.SetWordColor(str37);
                                        str38 = KL8Phase.get_cl_wh(str22.ToString());
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list9 = new List<string> {
                                            str22,
                                            str16,
                                            str15,
                                            str36,
                                            str37,
                                            str38
                                        };
                                        dictionary3.Add("total", list9);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                        num3++;
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 6:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_k8sc_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str22 = "";
                                        str15 = "";
                                        str16 = "";
                                        str23 = "";
                                        str24 = "";
                                        str25 = "";
                                        str26 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        list2.Add(num8.ToString());
                                        list2.Add(num9.ToString());
                                        list2.Add(num10.ToString());
                                        list2.Add(num11.ToString());
                                        list2.Add(num12.ToString());
                                        num35 = (((num8 + num9) + num10) + num11) + num12;
                                        str22 = num35.ToString();
                                        str15 = K8SCPhase.get_cl_zhdx(str22);
                                        str15 = base.SetWordColor(str15);
                                        str16 = K8SCPhase.get_cl_zhds(str22);
                                        str16 = base.SetWordColor(str16);
                                        str23 = K8SCPhase.get_cl_lh(num8.ToString(), num12.ToString());
                                        str23 = base.SetWordColor(str23);
                                        str24 = K8SCPhase.get_k8sc_str(string.Concat(new object[] { num8, ",", num9, ",", num10 }));
                                        str25 = K8SCPhase.get_k8sc_str(string.Concat(new object[] { num9, ",", num10, ",", num11 }));
                                        str26 = K8SCPhase.get_k8sc_str(string.Concat(new object[] { num10, ",", num11, ",", num12 }));
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list10 = new List<string> {
                                            str22,
                                            str15,
                                            str16,
                                            str23,
                                            str24,
                                            str25,
                                            str26
                                        };
                                        dictionary3.Add("total", list10);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 7:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_pcdd_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str11 = "";
                                        str10 = "";
                                        string str39 = "";
                                        string str40 = "";
                                        string str41 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        int num33 = Convert.ToInt32(row["sn"].ToString());
                                        list2.Add(num8.ToString());
                                        list2.Add(num9.ToString());
                                        list2.Add(num10.ToString());
                                        list2.Add(num33.ToString());
                                        if (num33 < 14)
                                        {
                                            str11 = base.SetWordColor("小");
                                        }
                                        else
                                        {
                                            str11 = base.SetWordColor("大");
                                        }
                                        if ((num33 % 2) == 0)
                                        {
                                            str10 = base.SetWordColor("雙");
                                        }
                                        else
                                        {
                                            str10 = base.SetWordColor("單");
                                        }
                                        str39 = base.SetWordColor(this.get_pcdd_bs_str(num33.ToString()));
                                        if (num33 <= 4)
                                        {
                                            str40 = base.SetWordColor("極小");
                                        }
                                        else if (num33 >= 0x17)
                                        {
                                            str40 = base.SetWordColor("極大");
                                        }
                                        else
                                        {
                                            str40 = "-";
                                        }
                                        if (((num8 == num9) && (num8 == num10)) && (num9 == num10))
                                        {
                                            str41 = base.SetWordColor("豹子");
                                        }
                                        else
                                        {
                                            str41 = "-";
                                        }
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list11 = new List<string> {
                                            str11,
                                            str10,
                                            str39,
                                            str40,
                                            str41
                                        };
                                        dictionary3.Add("total", list11);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 8:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_pkbjl_bll.GetCurrentByPhase(str2, str3);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        string str42 = row["ten_poker"].ToString();
                                        string str43 = row["xian_nn"].ToString();
                                        string str44 = row["zhuang_nn"].ToString();
                                        string pKBJLBalanceMaxMin = Utils.GetPKBJLBalanceMaxMin(str44, str43);
                                        bool pKBJLBalanceIsDuizi = Utils.GetPKBJLBalanceIsDuizi(str43);
                                        bool flag2 = Utils.GetPKBJLBalanceIsDuizi(str44);
                                        string str46 = "-";
                                        string str47 = "-";
                                        string pKBJLBalanceZXH = Utils.GetPKBJLBalanceZXH(str44, str43);
                                        if (pKBJLBalanceMaxMin.Equals("min"))
                                        {
                                            pKBJLBalanceMaxMin = base.SetWordColor("小");
                                        }
                                        else
                                        {
                                            pKBJLBalanceMaxMin = base.SetWordColor("大");
                                        }
                                        if (pKBJLBalanceIsDuizi)
                                        {
                                            str46 = base.SetWordColor("閑對");
                                        }
                                        if (flag2)
                                        {
                                            str47 = base.SetWordColor("莊對");
                                        }
                                        if (pKBJLBalanceZXH.Equals("zhuang"))
                                        {
                                            pKBJLBalanceZXH = base.SetWordColor("莊");
                                        }
                                        else if (pKBJLBalanceZXH.Equals("xian"))
                                        {
                                            pKBJLBalanceZXH = base.SetWordColor("閑");
                                        }
                                        else
                                        {
                                            pKBJLBalanceZXH = base.SetWordColor("和");
                                        }
                                        string pKBJLBalanceDianshu = Utils.GetPKBJLBalanceDianshu(str43);
                                        string str50 = Utils.GetPKBJLBalanceDianshu(str44);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        dictionary3.Add("pokerList", str42);
                                        dictionary3.Add("xian_nn", str43);
                                        dictionary3.Add("zhuang_nn", str44);
                                        dictionary3.Add("xian_dian", pKBJLBalanceDianshu);
                                        dictionary3.Add("zhuang_dian", str50);
                                        List<string> list13 = new List<string> {
                                            pKBJLBalanceZXH,
                                            pKBJLBalanceMaxMin,
                                            str46,
                                            str47
                                        };
                                        dictionary3.Add("total", list13);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 9:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_xyft5_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str27 = "";
                                        str28 = "";
                                        str29 = "";
                                        str30 = "";
                                        str31 = "";
                                        str32 = "";
                                        str33 = "";
                                        str34 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        num35 = num8 + num9;
                                        str27 = num35.ToString();
                                        if (Convert.ToInt32(str27) <= 11)
                                        {
                                            str28 = "小";
                                        }
                                        else
                                        {
                                            str28 = "大";
                                        }
                                        str28 = base.SetWordColor(str28);
                                        if ((Convert.ToInt32(str27) % 2) == 0)
                                        {
                                            str29 = "雙";
                                        }
                                        else
                                        {
                                            str29 = "單";
                                        }
                                        str29 = base.SetWordColor(str29);
                                        if (num8 > num17)
                                        {
                                            str30 = "龍";
                                        }
                                        else
                                        {
                                            str30 = "虎";
                                        }
                                        str30 = base.SetWordColor(str30);
                                        if (num9 > num16)
                                        {
                                            str31 = "龍";
                                        }
                                        else
                                        {
                                            str31 = "虎";
                                        }
                                        str31 = base.SetWordColor(str31);
                                        if (num10 > num15)
                                        {
                                            str32 = "龍";
                                        }
                                        else
                                        {
                                            str32 = "虎";
                                        }
                                        str32 = base.SetWordColor(str32);
                                        if (num11 > num14)
                                        {
                                            str33 = "龍";
                                        }
                                        else
                                        {
                                            str33 = "虎";
                                        }
                                        str33 = base.SetWordColor(str33);
                                        if (num12 > num13)
                                        {
                                            str34 = "龍";
                                        }
                                        else
                                        {
                                            str34 = "虎";
                                        }
                                        str34 = base.SetWordColor(str34);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list12 = new List<string> {
                                            str27,
                                            str28,
                                            str29,
                                            str30,
                                            str31,
                                            str32,
                                            str33,
                                            str34
                                        };
                                        dictionary3.Add("total", list12);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 10:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_jscar_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str27 = "";
                                        str28 = "";
                                        str29 = "";
                                        str30 = "";
                                        str31 = "";
                                        str32 = "";
                                        str33 = "";
                                        str34 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        num35 = num8 + num9;
                                        str27 = num35.ToString();
                                        if (Convert.ToInt32(str27) <= 11)
                                        {
                                            str28 = "小";
                                        }
                                        else
                                        {
                                            str28 = "大";
                                        }
                                        str28 = base.SetWordColor(str28);
                                        if ((Convert.ToInt32(str27) % 2) == 0)
                                        {
                                            str29 = "雙";
                                        }
                                        else
                                        {
                                            str29 = "單";
                                        }
                                        str29 = base.SetWordColor(str29);
                                        if (num8 > num17)
                                        {
                                            str30 = "龍";
                                        }
                                        else
                                        {
                                            str30 = "虎";
                                        }
                                        str30 = base.SetWordColor(str30);
                                        if (num9 > num16)
                                        {
                                            str31 = "龍";
                                        }
                                        else
                                        {
                                            str31 = "虎";
                                        }
                                        str31 = base.SetWordColor(str31);
                                        if (num10 > num15)
                                        {
                                            str32 = "龍";
                                        }
                                        else
                                        {
                                            str32 = "虎";
                                        }
                                        str32 = base.SetWordColor(str32);
                                        if (num11 > num14)
                                        {
                                            str33 = "龍";
                                        }
                                        else
                                        {
                                            str33 = "虎";
                                        }
                                        str33 = base.SetWordColor(str33);
                                        if (num12 > num13)
                                        {
                                            str34 = "龍";
                                        }
                                        else
                                        {
                                            str34 = "虎";
                                        }
                                        str34 = base.SetWordColor(str34);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list14 = new List<string> {
                                            str27,
                                            str28,
                                            str29,
                                            str30,
                                            str31,
                                            str32,
                                            str33,
                                            str34
                                        };
                                        dictionary3.Add("total", list14);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 11:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_speed5_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str22 = "";
                                        str15 = "";
                                        str16 = "";
                                        str23 = "";
                                        str24 = "";
                                        str25 = "";
                                        str26 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        list2.Add(num8.ToString());
                                        list2.Add(num9.ToString());
                                        list2.Add(num10.ToString());
                                        list2.Add(num11.ToString());
                                        list2.Add(num12.ToString());
                                        num35 = (((num8 + num9) + num10) + num11) + num12;
                                        str22 = num35.ToString();
                                        str15 = SPEED5Phase.get_cl_zhdx(str22);
                                        str15 = base.SetWordColor(str15);
                                        str16 = SPEED5Phase.get_cl_zhds(str22);
                                        str16 = base.SetWordColor(str16);
                                        str23 = SPEED5Phase.get_cl_lh(num8.ToString(), num12.ToString());
                                        str23 = base.SetWordColor(str23);
                                        str24 = SPEED5Phase.get_speed5_str(string.Concat(new object[] { num8, ",", num9, ",", num10 }));
                                        str25 = SPEED5Phase.get_speed5_str(string.Concat(new object[] { num9, ",", num10, ",", num11 }));
                                        str26 = SPEED5Phase.get_speed5_str(string.Concat(new object[] { num10, ",", num11, ",", num12 }));
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list15 = new List<string> {
                                            str22,
                                            str15,
                                            str16,
                                            str23,
                                            str24,
                                            str25,
                                            str26
                                        };
                                        dictionary3.Add("total", list15);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 12:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_jspk10_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str27 = "";
                                        str28 = "";
                                        str29 = "";
                                        str30 = "";
                                        str31 = "";
                                        str32 = "";
                                        str33 = "";
                                        str34 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        num35 = num8 + num9;
                                        str27 = num35.ToString();
                                        if (Convert.ToInt32(str27) <= 11)
                                        {
                                            str28 = "小";
                                        }
                                        else
                                        {
                                            str28 = "大";
                                        }
                                        str28 = base.SetWordColor(str28);
                                        if ((Convert.ToInt32(str27) % 2) == 0)
                                        {
                                            str29 = "雙";
                                        }
                                        else
                                        {
                                            str29 = "單";
                                        }
                                        str29 = base.SetWordColor(str29);
                                        if (num8 > num17)
                                        {
                                            str30 = "龍";
                                        }
                                        else
                                        {
                                            str30 = "虎";
                                        }
                                        str30 = base.SetWordColor(str30);
                                        if (num9 > num16)
                                        {
                                            str31 = "龍";
                                        }
                                        else
                                        {
                                            str31 = "虎";
                                        }
                                        str31 = base.SetWordColor(str31);
                                        if (num10 > num15)
                                        {
                                            str32 = "龍";
                                        }
                                        else
                                        {
                                            str32 = "虎";
                                        }
                                        str32 = base.SetWordColor(str32);
                                        if (num11 > num14)
                                        {
                                            str33 = "龍";
                                        }
                                        else
                                        {
                                            str33 = "虎";
                                        }
                                        str33 = base.SetWordColor(str33);
                                        if (num12 > num13)
                                        {
                                            str34 = "龍";
                                        }
                                        else
                                        {
                                            str34 = "虎";
                                        }
                                        str34 = base.SetWordColor(str34);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list17 = new List<string> {
                                            str27,
                                            str28,
                                            str29,
                                            str30,
                                            str31,
                                            str32,
                                            str33,
                                            str34
                                        };
                                        dictionary3.Add("total", list17);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 13:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_jscqsc_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str22 = "";
                                        str15 = "";
                                        str16 = "";
                                        str23 = "";
                                        str24 = "";
                                        str25 = "";
                                        str26 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        list2.Add(num8.ToString());
                                        list2.Add(num9.ToString());
                                        list2.Add(num10.ToString());
                                        list2.Add(num11.ToString());
                                        list2.Add(num12.ToString());
                                        num35 = (((num8 + num9) + num10) + num11) + num12;
                                        str22 = num35.ToString();
                                        str15 = JSCQSCPhase.get_cl_zhdx(str22);
                                        str15 = base.SetWordColor(str15);
                                        str16 = JSCQSCPhase.get_cl_zhds(str22);
                                        str16 = base.SetWordColor(str16);
                                        str23 = JSCQSCPhase.get_cl_lh(num8.ToString(), num12.ToString());
                                        str23 = base.SetWordColor(str23);
                                        str24 = JSCQSCPhase.get_jscqsc_str(string.Concat(new object[] { num8, ",", num9, ",", num10 }));
                                        str25 = JSCQSCPhase.get_jscqsc_str(string.Concat(new object[] { num9, ",", num10, ",", num11 }));
                                        str26 = JSCQSCPhase.get_jscqsc_str(string.Concat(new object[] { num10, ",", num11, ",", num12 }));
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list16 = new List<string> {
                                            str22,
                                            str15,
                                            str16,
                                            str23,
                                            str24,
                                            str25,
                                            str26
                                        };
                                        dictionary3.Add("total", list16);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 14:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_jssfc_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 8;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str19 = "";
                                        str15 = "";
                                        str16 = "";
                                        str20 = "";
                                        str21 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        for (num3 = 1; num3 <= num; num3++)
                                        {
                                            str17 = "n" + num3;
                                            list2.Add(row[str17].ToString());
                                        }
                                        str19 = JSSFCPhase.get_zh(list2).ToString();
                                        str15 = JSSFCPhase.get_cl_zhdx(str19);
                                        str15 = base.SetWordColor(str15);
                                        str16 = JSSFCPhase.get_cl_zhds(str19);
                                        str16 = base.SetWordColor(str16);
                                        str20 = JSSFCPhase.get_cl_zhwsdx(str19);
                                        str20 = base.SetWordColor(str20);
                                        str21 = JSSFCPhase.get_cl_lh(list2[0].ToString(), list2[num - 1].ToString());
                                        str21 = base.SetWordColor(str21);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list18 = new List<string> {
                                            str19,
                                            str15,
                                            str16,
                                            str20,
                                            str21
                                        };
                                        dictionary3.Add("total", list18);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 15:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_jsft2_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str27 = "";
                                        str28 = "";
                                        str29 = "";
                                        str30 = "";
                                        str31 = "";
                                        str32 = "";
                                        str33 = "";
                                        str34 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        num35 = num8 + num9;
                                        str27 = num35.ToString();
                                        if (Convert.ToInt32(str27) <= 11)
                                        {
                                            str28 = "小";
                                        }
                                        else
                                        {
                                            str28 = "大";
                                        }
                                        str28 = base.SetWordColor(str28);
                                        if ((Convert.ToInt32(str27) % 2) == 0)
                                        {
                                            str29 = "雙";
                                        }
                                        else
                                        {
                                            str29 = "單";
                                        }
                                        str29 = base.SetWordColor(str29);
                                        if (num8 > num17)
                                        {
                                            str30 = "龍";
                                        }
                                        else
                                        {
                                            str30 = "虎";
                                        }
                                        str30 = base.SetWordColor(str30);
                                        if (num9 > num16)
                                        {
                                            str31 = "龍";
                                        }
                                        else
                                        {
                                            str31 = "虎";
                                        }
                                        str31 = base.SetWordColor(str31);
                                        if (num10 > num15)
                                        {
                                            str32 = "龍";
                                        }
                                        else
                                        {
                                            str32 = "虎";
                                        }
                                        str32 = base.SetWordColor(str32);
                                        if (num11 > num14)
                                        {
                                            str33 = "龍";
                                        }
                                        else
                                        {
                                            str33 = "虎";
                                        }
                                        str33 = base.SetWordColor(str33);
                                        if (num12 > num13)
                                        {
                                            str34 = "龍";
                                        }
                                        else
                                        {
                                            str34 = "虎";
                                        }
                                        str34 = base.SetWordColor(str34);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list19 = new List<string> {
                                            str27,
                                            str28,
                                            str29,
                                            str30,
                                            str31,
                                            str32,
                                            str33,
                                            str34
                                        };
                                        dictionary3.Add("total", list19);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 0x10:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_car168_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str27 = "";
                                        str28 = "";
                                        str29 = "";
                                        str30 = "";
                                        str31 = "";
                                        str32 = "";
                                        str33 = "";
                                        str34 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        num35 = num8 + num9;
                                        str27 = num35.ToString();
                                        if (Convert.ToInt32(str27) <= 11)
                                        {
                                            str28 = "小";
                                        }
                                        else
                                        {
                                            str28 = "大";
                                        }
                                        str28 = base.SetWordColor(str28);
                                        if ((Convert.ToInt32(str27) % 2) == 0)
                                        {
                                            str29 = "雙";
                                        }
                                        else
                                        {
                                            str29 = "單";
                                        }
                                        str29 = base.SetWordColor(str29);
                                        if (num8 > num17)
                                        {
                                            str30 = "龍";
                                        }
                                        else
                                        {
                                            str30 = "虎";
                                        }
                                        str30 = base.SetWordColor(str30);
                                        if (num9 > num16)
                                        {
                                            str31 = "龍";
                                        }
                                        else
                                        {
                                            str31 = "虎";
                                        }
                                        str31 = base.SetWordColor(str31);
                                        if (num10 > num15)
                                        {
                                            str32 = "龍";
                                        }
                                        else
                                        {
                                            str32 = "虎";
                                        }
                                        str32 = base.SetWordColor(str32);
                                        if (num11 > num14)
                                        {
                                            str33 = "龍";
                                        }
                                        else
                                        {
                                            str33 = "虎";
                                        }
                                        str33 = base.SetWordColor(str33);
                                        if (num12 > num13)
                                        {
                                            str34 = "龍";
                                        }
                                        else
                                        {
                                            str34 = "虎";
                                        }
                                        str34 = base.SetWordColor(str34);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list20 = new List<string> {
                                            str27,
                                            str28,
                                            str29,
                                            str30,
                                            str31,
                                            str32,
                                            str33,
                                            str34
                                        };
                                        dictionary3.Add("total", list20);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 0x11:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_ssc168_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str22 = "";
                                        str15 = "";
                                        str16 = "";
                                        str23 = "";
                                        str24 = "";
                                        str25 = "";
                                        str26 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        list2.Add(num8.ToString());
                                        list2.Add(num9.ToString());
                                        list2.Add(num10.ToString());
                                        list2.Add(num11.ToString());
                                        list2.Add(num12.ToString());
                                        num35 = (((num8 + num9) + num10) + num11) + num12;
                                        str22 = num35.ToString();
                                        str15 = SSC168Phase.get_cl_zhdx(str22);
                                        str15 = base.SetWordColor(str15);
                                        str16 = SSC168Phase.get_cl_zhds(str22);
                                        str16 = base.SetWordColor(str16);
                                        str23 = SSC168Phase.get_cl_lh(num8.ToString(), num12.ToString());
                                        str23 = base.SetWordColor(str23);
                                        str24 = SSC168Phase.get_ssc168_str(string.Concat(new object[] { num8, ",", num9, ",", num10 }));
                                        str25 = SSC168Phase.get_ssc168_str(string.Concat(new object[] { num9, ",", num10, ",", num11 }));
                                        str26 = SSC168Phase.get_ssc168_str(string.Concat(new object[] { num10, ",", num11, ",", num12 }));
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list21 = new List<string> {
                                            str22,
                                            str15,
                                            str16,
                                            str23,
                                            str24,
                                            str25,
                                            str26
                                        };
                                        dictionary3.Add("total", list21);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 0x12:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_vrcar_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str27 = "";
                                        str28 = "";
                                        str29 = "";
                                        str30 = "";
                                        str31 = "";
                                        str32 = "";
                                        str33 = "";
                                        str34 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        num35 = num8 + num9;
                                        str27 = num35.ToString();
                                        if (Convert.ToInt32(str27) <= 11)
                                        {
                                            str28 = "小";
                                        }
                                        else
                                        {
                                            str28 = "大";
                                        }
                                        str28 = base.SetWordColor(str28);
                                        if ((Convert.ToInt32(str27) % 2) == 0)
                                        {
                                            str29 = "雙";
                                        }
                                        else
                                        {
                                            str29 = "單";
                                        }
                                        str29 = base.SetWordColor(str29);
                                        if (num8 > num17)
                                        {
                                            str30 = "龍";
                                        }
                                        else
                                        {
                                            str30 = "虎";
                                        }
                                        str30 = base.SetWordColor(str30);
                                        if (num9 > num16)
                                        {
                                            str31 = "龍";
                                        }
                                        else
                                        {
                                            str31 = "虎";
                                        }
                                        str31 = base.SetWordColor(str31);
                                        if (num10 > num15)
                                        {
                                            str32 = "龍";
                                        }
                                        else
                                        {
                                            str32 = "虎";
                                        }
                                        str32 = base.SetWordColor(str32);
                                        if (num11 > num14)
                                        {
                                            str33 = "龍";
                                        }
                                        else
                                        {
                                            str33 = "虎";
                                        }
                                        str33 = base.SetWordColor(str33);
                                        if (num12 > num13)
                                        {
                                            str34 = "龍";
                                        }
                                        else
                                        {
                                            str34 = "虎";
                                        }
                                        str34 = base.SetWordColor(str34);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list22 = new List<string> {
                                            str27,
                                            str28,
                                            str29,
                                            str30,
                                            str31,
                                            str32,
                                            str33,
                                            str34
                                        };
                                        dictionary3.Add("total", list22);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 0x13:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_vrssc_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str22 = "";
                                        str15 = "";
                                        str16 = "";
                                        str23 = "";
                                        str24 = "";
                                        str25 = "";
                                        str26 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        list2.Add(num8.ToString());
                                        list2.Add(num9.ToString());
                                        list2.Add(num10.ToString());
                                        list2.Add(num11.ToString());
                                        list2.Add(num12.ToString());
                                        num35 = (((num8 + num9) + num10) + num11) + num12;
                                        str22 = num35.ToString();
                                        str15 = VRSSCPhase.get_cl_zhdx(str22);
                                        str15 = base.SetWordColor(str15);
                                        str16 = VRSSCPhase.get_cl_zhds(str22);
                                        str16 = base.SetWordColor(str16);
                                        str23 = VRSSCPhase.get_cl_lh(num8.ToString(), num12.ToString());
                                        str23 = base.SetWordColor(str23);
                                        str24 = VRSSCPhase.get_vrssc_str(string.Concat(new object[] { num8, ",", num9, ",", num10 }));
                                        str25 = VRSSCPhase.get_vrssc_str(string.Concat(new object[] { num9, ",", num10, ",", num11 }));
                                        str26 = VRSSCPhase.get_vrssc_str(string.Concat(new object[] { num10, ",", num11, ",", num12 }));
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list23 = new List<string> {
                                            str22,
                                            str15,
                                            str16,
                                            str23,
                                            str24,
                                            str25,
                                            str26
                                        };
                                        dictionary3.Add("total", list23);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 20:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_xyftoa_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str27 = "";
                                        str28 = "";
                                        str29 = "";
                                        str30 = "";
                                        str31 = "";
                                        str32 = "";
                                        str33 = "";
                                        str34 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        num35 = num8 + num9;
                                        str27 = num35.ToString();
                                        if (Convert.ToInt32(str27) <= 11)
                                        {
                                            str28 = "小";
                                        }
                                        else
                                        {
                                            str28 = "大";
                                        }
                                        str28 = base.SetWordColor(str28);
                                        if ((Convert.ToInt32(str27) % 2) == 0)
                                        {
                                            str29 = "雙";
                                        }
                                        else
                                        {
                                            str29 = "單";
                                        }
                                        str29 = base.SetWordColor(str29);
                                        if (num8 > num17)
                                        {
                                            str30 = "龍";
                                        }
                                        else
                                        {
                                            str30 = "虎";
                                        }
                                        str30 = base.SetWordColor(str30);
                                        if (num9 > num16)
                                        {
                                            str31 = "龍";
                                        }
                                        else
                                        {
                                            str31 = "虎";
                                        }
                                        str31 = base.SetWordColor(str31);
                                        if (num10 > num15)
                                        {
                                            str32 = "龍";
                                        }
                                        else
                                        {
                                            str32 = "虎";
                                        }
                                        str32 = base.SetWordColor(str32);
                                        if (num11 > num14)
                                        {
                                            str33 = "龍";
                                        }
                                        else
                                        {
                                            str33 = "虎";
                                        }
                                        str33 = base.SetWordColor(str33);
                                        if (num12 > num13)
                                        {
                                            str34 = "龍";
                                        }
                                        else
                                        {
                                            str34 = "虎";
                                        }
                                        str34 = base.SetWordColor(str34);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list24 = new List<string> {
                                            str27,
                                            str28,
                                            str29,
                                            str30,
                                            str31,
                                            str32,
                                            str33,
                                            str34
                                        };
                                        dictionary3.Add("total", list24);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 0x15:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_xyftsg_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        str18 = "";
                                        str27 = "";
                                        str28 = "";
                                        str29 = "";
                                        str30 = "";
                                        str31 = "";
                                        str32 = "";
                                        str33 = "";
                                        str34 = "";
                                        str6 = row["phase"].ToString();
                                        str18 = row["play_open_date"].ToString();
                                        num8 = Convert.ToInt32(row["n1"].ToString());
                                        num9 = Convert.ToInt32(row["n2"].ToString());
                                        num10 = Convert.ToInt32(row["n3"].ToString());
                                        num11 = Convert.ToInt32(row["n4"].ToString());
                                        num12 = Convert.ToInt32(row["n5"].ToString());
                                        num13 = Convert.ToInt32(row["n6"].ToString());
                                        num14 = Convert.ToInt32(row["n7"].ToString());
                                        num15 = Convert.ToInt32(row["n8"].ToString());
                                        num16 = Convert.ToInt32(row["n9"].ToString());
                                        num17 = Convert.ToInt32(row["n10"].ToString());
                                        list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                        list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                        list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                        list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                        list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                        list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                        list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                        list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                        list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                        list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                        num35 = num8 + num9;
                                        str27 = num35.ToString();
                                        if (Convert.ToInt32(str27) <= 11)
                                        {
                                            str28 = "小";
                                        }
                                        else
                                        {
                                            str28 = "大";
                                        }
                                        str28 = base.SetWordColor(str28);
                                        if ((Convert.ToInt32(str27) % 2) == 0)
                                        {
                                            str29 = "雙";
                                        }
                                        else
                                        {
                                            str29 = "單";
                                        }
                                        str29 = base.SetWordColor(str29);
                                        if (num8 > num17)
                                        {
                                            str30 = "龍";
                                        }
                                        else
                                        {
                                            str30 = "虎";
                                        }
                                        str30 = base.SetWordColor(str30);
                                        if (num9 > num16)
                                        {
                                            str31 = "龍";
                                        }
                                        else
                                        {
                                            str31 = "虎";
                                        }
                                        str31 = base.SetWordColor(str31);
                                        if (num10 > num15)
                                        {
                                            str32 = "龍";
                                        }
                                        else
                                        {
                                            str32 = "虎";
                                        }
                                        str32 = base.SetWordColor(str32);
                                        if (num11 > num14)
                                        {
                                            str33 = "龍";
                                        }
                                        else
                                        {
                                            str33 = "虎";
                                        }
                                        str33 = base.SetWordColor(str33);
                                        if (num12 > num13)
                                        {
                                            str34 = "龍";
                                        }
                                        else
                                        {
                                            str34 = "虎";
                                        }
                                        str34 = base.SetWordColor(str34);
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", str18);
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list25 = new List<string> {
                                            str27,
                                            str28,
                                            str29,
                                            str30,
                                            str31,
                                            str32,
                                            str33,
                                            str34
                                        };
                                        dictionary3.Add("total", list25);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 0x16:
                        list = new List<object>();
                        list2 = new List<string>();
                        currentByPhase = CallBLL.cz_phase_happycar_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            table = currentByPhase.Tables[0];
                            num = 5;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                foreach (DataRow row in table.Rows)
                                {
                                    str6 = "";
                                    str18 = "";
                                    str27 = "";
                                    str28 = "";
                                    str29 = "";
                                    str30 = "";
                                    str31 = "";
                                    str32 = "";
                                    str33 = "";
                                    str34 = "";
                                    str6 = row["phase"].ToString();
                                    str18 = row["play_open_date"].ToString();
                                    num8 = Convert.ToInt32(row["n1"].ToString());
                                    num9 = Convert.ToInt32(row["n2"].ToString());
                                    num10 = Convert.ToInt32(row["n3"].ToString());
                                    num11 = Convert.ToInt32(row["n4"].ToString());
                                    num12 = Convert.ToInt32(row["n5"].ToString());
                                    num13 = Convert.ToInt32(row["n6"].ToString());
                                    num14 = Convert.ToInt32(row["n7"].ToString());
                                    num15 = Convert.ToInt32(row["n8"].ToString());
                                    num16 = Convert.ToInt32(row["n9"].ToString());
                                    num17 = Convert.ToInt32(row["n10"].ToString());
                                    list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
                                    list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
                                    list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
                                    list2.Add((num11 < 10) ? ("0" + num11) : num11.ToString());
                                    list2.Add((num12 < 10) ? ("0" + num12) : num12.ToString());
                                    list2.Add((num13 < 10) ? ("0" + num13) : num13.ToString());
                                    list2.Add((num14 < 10) ? ("0" + num14) : num14.ToString());
                                    list2.Add((num15 < 10) ? ("0" + num15) : num15.ToString());
                                    list2.Add((num16 < 10) ? ("0" + num16) : num16.ToString());
                                    list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
                                    str27 = (num8 + num9).ToString();
                                    if (Convert.ToInt32(str27) <= 11)
                                    {
                                        str28 = "小";
                                    }
                                    else
                                    {
                                        str28 = "大";
                                    }
                                    str28 = base.SetWordColor(str28);
                                    if ((Convert.ToInt32(str27) % 2) == 0)
                                    {
                                        str29 = "雙";
                                    }
                                    else
                                    {
                                        str29 = "單";
                                    }
                                    str29 = base.SetWordColor(str29);
                                    if (num8 > num17)
                                    {
                                        str30 = "龍";
                                    }
                                    else
                                    {
                                        str30 = "虎";
                                    }
                                    str30 = base.SetWordColor(str30);
                                    if (num9 > num16)
                                    {
                                        str31 = "龍";
                                    }
                                    else
                                    {
                                        str31 = "虎";
                                    }
                                    str31 = base.SetWordColor(str31);
                                    if (num10 > num15)
                                    {
                                        str32 = "龍";
                                    }
                                    else
                                    {
                                        str32 = "虎";
                                    }
                                    str32 = base.SetWordColor(str32);
                                    if (num11 > num14)
                                    {
                                        str33 = "龍";
                                    }
                                    else
                                    {
                                        str33 = "虎";
                                    }
                                    str33 = base.SetWordColor(str33);
                                    if (num12 > num13)
                                    {
                                        str34 = "龍";
                                    }
                                    else
                                    {
                                        str34 = "虎";
                                    }
                                    str34 = base.SetWordColor(str34);
                                    dictionary3.Add("phase", str6);
                                    dictionary3.Add("play_open_date", str18);
                                    dictionary3.Add("draw_num", new List<string>(list2));
                                    List<string> list26 = new List<string> {
                                        str27,
                                        str28,
                                        str29,
                                        str30,
                                        str31,
                                        str32,
                                        str33,
                                        str34
                                    };
                                    dictionary3.Add("total", list26);
                                    list.Add(new Dictionary<string, object>(dictionary3));
                                    dictionary3.Clear();
                                    list2.Clear();
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;

                    case 100:
                        list = new List<object>();
                        currentByPhase = CallBLL.cz_phase_six_bll.GetCurrentByPhase(str2);
                        if (currentByPhase != null)
                        {
                            list2 = new List<string>();
                            table = currentByPhase.Tables[0];
                            num = 6;
                            dictionary3 = new Dictionary<string, object>();
                            str4 = table.Rows[0]["is_closed"];
                            str5 = table.Rows[0]["is_opendata"];
                            if (str4.Equals("1") && str5.Equals("1"))
                            {
                                using (enumerator = table.Rows.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        str6 = "";
                                        string str7 = "";
                                        string str8 = "";
                                        string str9 = "";
                                        str10 = "";
                                        str11 = "";
                                        string str12 = "";
                                        string str13 = "";
                                        string str14 = "";
                                        str15 = "";
                                        num2 = 0;
                                        str16 = "";
                                        str6 = row["phase"].ToString();
                                        str7 = row["sn_stop_date"].ToString();
                                        for (num3 = 1; num3 <= num; num3++)
                                        {
                                            str17 = "n" + num3;
                                            list2.Add(row[str17].ToString());
                                            num2 += Convert.ToInt32(row[str17]);
                                            if (num3.Equals(1))
                                            {
                                                str8 = str8 + row[str17].ToString();
                                            }
                                            else
                                            {
                                                str8 = str8 + "," + row[str17].ToString();
                                            }
                                        }
                                        list2.Add(row["sn"].ToString());
                                        num2 += Convert.ToInt32(row["sn"].ToString());
                                        str9 = row["sn"].ToString();
                                        if (double.Parse(row["sn"].ToString().Trim()) == 49.0)
                                        {
                                            str10 = base.SetWordColor("和");
                                        }
                                        else if ((double.Parse(row["sn"].ToString().Trim()) % 2.0) == 0.0)
                                        {
                                            str10 = base.SetWordColor("雙");
                                        }
                                        else
                                        {
                                            str10 = base.SetWordColor("單");
                                        }
                                        if (double.Parse(row["sn"].ToString().Trim()) <= 24.0)
                                        {
                                            str11 = base.SetWordColor("小");
                                        }
                                        else if (double.Parse(row["sn"].ToString().Trim()) == 49.0)
                                        {
                                            str11 = base.SetWordColor("和");
                                        }
                                        else
                                        {
                                            str11 = base.SetWordColor("大");
                                        }
                                        int num4 = 0;
                                        num4 = int.Parse(row["sn"].ToString().Trim()) % 10;
                                        if (double.Parse(row["sn"].ToString().Trim()) == 49.0)
                                        {
                                            str12 = base.SetWordColor("和");
                                        }
                                        else if (num4 <= 4)
                                        {
                                            str12 = base.SetWordColor("小");
                                        }
                                        else
                                        {
                                            str12 = base.SetWordColor("大");
                                        }
                                        str13 = this.get_tz(row["sn"].ToString().Trim());
                                        int num5 = int.Parse(row["sn"].ToString().Trim()) / 10;
                                        int num6 = int.Parse(row["sn"].ToString().Trim()) % 10;
                                        int num7 = num5 + num6;
                                        if (int.Parse(row["sn"].ToString().Trim()) == 0x31)
                                        {
                                            str14 = base.SetWordColor("和");
                                        }
                                        else if ((num7 % 2) == 0)
                                        {
                                            str14 = base.SetWordColor("雙");
                                        }
                                        else
                                        {
                                            str14 = base.SetWordColor("單");
                                        }
                                        if ((Convert.ToInt32(num2) % 2) == 0)
                                        {
                                            str16 = base.SetWordColor("雙");
                                        }
                                        else
                                        {
                                            str16 = base.SetWordColor("單");
                                        }
                                        if (Convert.ToInt32(num2) <= 0xae)
                                        {
                                            str15 = base.SetWordColor("小");
                                        }
                                        else
                                        {
                                            str15 = base.SetWordColor("大");
                                        }
                                        dictionary3.Add("phase", str6);
                                        dictionary3.Add("play_open_date", Convert.ToDateTime(str7).ToString("yyyy/MM/dd"));
                                        dictionary3.Add("draw_num", new List<string>(list2));
                                        List<string> list3 = new List<string> {
                                            str8,
                                            str9,
                                            str10,
                                            str11,
                                            str12,
                                            str13,
                                            str14,
                                            num2.ToString(),
                                            str16,
                                            str15
                                        };
                                        dictionary3.Add("total", list3);
                                        list.Add(new Dictionary<string, object>(dictionary3));
                                        dictionary3.Clear();
                                        list2.Clear();
                                    }
                                }
                            }
                        }
                        dictionary.Add("jqkj", list);
                        break;
                }
                result.set_success(200);
                result.set_data(dictionary);
                strResult = JsonHandle.ObjectToJson(result);
            }
        }

        public void get_gamehall(HttpContext context, ref string strResult)
        {
            ReturnResult result = new ReturnResult();
            result.set_success(400);
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                { 
                    "type",
                    "get_gamehall"
                }
            };
            if (context.Session["user_name"] == null)
            {
                context.Response.End();
            }
            else
            {
                string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int num = int.Parse(LSRequest.qq("lid"));
                Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                DataTable lotteryList = base.GetLotteryList();
                string str2 = "0";
                string str3 = "";
                if (num.Equals(100))
                {
                    DataTable currentPhase = CallBLL.cz_phase_six_bll.GetCurrentPhase("1");
                    if ((currentPhase != null) || (currentPhase.Rows.Count > 0))
                    {
                        DateTime now = DateTime.Now;
                        string s = currentPhase.Rows[0]["sn_stop_date"].ToString();
                        if (now < DateTime.Parse(s))
                        {
                            str2 = "2";
                            str3 = Convert.ToDateTime(s).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            str2 = "0";
                            str3 = "-";
                        }
                    }
                }
                else
                {
                    DataTable table3 = null;
                    switch (num)
                    {
                        case 0:
                            table3 = CallBLL.cz_phase_kl10_bll.IsPhaseClose();
                            break;

                        case 1:
                            table3 = CallBLL.cz_phase_cqsc_bll.IsPhaseClose();
                            break;

                        case 2:
                            table3 = CallBLL.cz_phase_pk10_bll.IsPhaseClose();
                            break;

                        case 3:
                            table3 = CallBLL.cz_phase_xync_bll.IsPhaseClose();
                            break;

                        case 4:
                            table3 = CallBLL.cz_phase_jsk3_bll.IsPhaseClose();
                            break;

                        case 5:
                            table3 = CallBLL.cz_phase_kl8_bll.IsPhaseClose();
                            break;

                        case 6:
                            table3 = CallBLL.cz_phase_k8sc_bll.IsPhaseClose();
                            break;

                        case 7:
                            table3 = CallBLL.cz_phase_pcdd_bll.IsPhaseClose();
                            break;

                        case 8:
                            table3 = CallBLL.cz_phase_pkbjl_bll.IsPhaseClose();
                            break;

                        case 9:
                            table3 = CallBLL.cz_phase_xyft5_bll.IsPhaseClose();
                            break;

                        case 10:
                            table3 = CallBLL.cz_phase_jscar_bll.IsPhaseClose();
                            break;

                        case 11:
                            table3 = CallBLL.cz_phase_speed5_bll.IsPhaseClose();
                            break;

                        case 12:
                            table3 = CallBLL.cz_phase_jspk10_bll.IsPhaseClose();
                            break;

                        case 13:
                            table3 = CallBLL.cz_phase_jscqsc_bll.IsPhaseClose();
                            break;

                        case 14:
                            table3 = CallBLL.cz_phase_jssfc_bll.IsPhaseClose();
                            break;

                        case 15:
                            table3 = CallBLL.cz_phase_jsft2_bll.IsPhaseClose();
                            break;

                        case 0x10:
                            table3 = CallBLL.cz_phase_car168_bll.IsPhaseClose();
                            break;

                        case 0x11:
                            table3 = CallBLL.cz_phase_ssc168_bll.IsPhaseClose();
                            break;

                        case 0x12:
                            table3 = CallBLL.cz_phase_vrcar_bll.IsPhaseClose();
                            break;

                        case 0x13:
                            table3 = CallBLL.cz_phase_vrssc_bll.IsPhaseClose();
                            break;

                        case 20:
                            table3 = CallBLL.cz_phase_xyftoa_bll.IsPhaseClose();
                            break;

                        case 0x15:
                            table3 = CallBLL.cz_phase_xyftsg_bll.IsPhaseClose();
                            break;

                        case 0x16:
                            table3 = CallBLL.cz_phase_happycar_bll.IsPhaseClose();
                            break;
                    }
                    if ((table3 != null) && (table3.Rows.Count > 0))
                    {
                        string str5 = table3.Rows[0]["isopen"].ToString();
                        string str6 = table3.Rows[0]["openning"].ToString();
                        string str7 = table3.Rows[0]["opendate"].ToString();
                        string str8 = table3.Rows[0]["endtime"].ToString();
                        DateTime time2 = Convert.ToDateTime(lotteryList.Select($" id={num} ")[0]["begintime"].ToString());
                        if (str5.Equals("0"))
                        {
                            str2 = "0";
                            DateTime now = DateTime.Now;
                            string introduced23 = now.ToString("yyyy-MM-dd");
                            if (introduced23 == now.AddHours(7.0).ToString("yyyy-MM-dd"))
                            {
                                str3 = now.ToString("yyyy-MM-dd ") + time2.ToString("HH:mm:ss");
                            }
                            else
                            {
                                str3 = now.AddDays(1.0).ToString("yyyy-MM-dd ") + time2.ToString("HH:mm:ss");
                            }
                        }
                        else
                        {
                            if (str6.Equals("n"))
                            {
                                str2 = "1";
                            }
                            else
                            {
                                str2 = "2";
                            }
                            str3 = Convert.ToDateTime(str8).ToString("HH:mm:ss");
                        }
                    }
                }
                dictionary2.Add(num.ToString(), str2 + "," + str3 + "," + str);
                dictionary.Add("isopendata", dictionary2);
                result.set_data(dictionary);
                result.set_success(200);
                strResult = JsonHandle.ObjectToJson(result);
            }
        }

        public void get_newbetlist(HttpContext context, ref string strResult)
        {
            ReturnResult result = new ReturnResult();
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                { 
                    "type",
                    "get_newbetlist"
                }
            };
            agent_userinfo_session model = new agent_userinfo_session();
            string str = context.Session["user_name"].ToString();
            model = context.Session[str + "lottery_session_user_info"] as agent_userinfo_session;
            if (!model.get_u_type().Trim().Equals("zj"))
            {
                context.Response.Write(PageBase.GetMessageByCache("u100027", "MessageHint"));
                context.Response.End();
            }
            if (!string.IsNullOrEmpty(base.Permission_Ashx_ZJ(model, "po_1_1")))
            {
                context.Response.Write(PageBase.GetMessageByCache("u100027", "MessageHint"));
                context.Response.End();
            }
            string str3 = LSRequest.qq("top");
            string str4 = LSRequest.qq("amount");
            string str5 = LSRequest.qq("oldId");
            string str6 = LSRequest.qq("lids");
            if (!string.IsNullOrEmpty(str6))
            {
                string compareTime = LSRequest.qq("oldTime");
                if (string.IsNullOrEmpty(str4))
                {
                    str4 = "0";
                }
                int num = 0;
                if (!string.IsNullOrEmpty(str5))
                {
                    num = Convert.ToInt32(str5);
                }
                int num2 = num;
                DataTable table = CallBLL.cz_bet_kc_bll.GetNewBet(str3, str5, str6, str4);
                if (table != null)
                {
                    Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
                        DataRow row = table.Rows[i];
                        string str8 = row["u_name"].ToString();
                        int num4 = Convert.ToInt32(row["bet_id"].ToString());
                        if (num4 > num2)
                        {
                            num2 = num4;
                        }
                        if ((row["isLM"]).Equals("1"))
                        {
                            string str9 = "";
                            int num5 = 0;
                            if (!(string.IsNullOrEmpty(row["unit_cnt"]) || (int.Parse(row["unit_cnt"].ToString()) <= 0)))
                            {
                                num5 = decimal.ToInt32(Convert.ToDecimal(row["amount"])) / int.Parse(row["unit_cnt"].ToString());
                            }
                            else
                            {
                                num5 = decimal.ToInt32(Convert.ToDecimal(row["amount"]));
                            }
                            if (!string.IsNullOrEmpty(row["unit_cnt"]) && (int.Parse(row["unit_cnt"].ToString()) > 0))
                            {
                                if ((row["sale_type"]).Equals("1"))
                                {
                                    str9 = "-" + Math.Round(Convert.ToDecimal(row["amount"].ToString()), 0).ToString();
                                }
                                else
                                {
                                    str9 = $"<span>￥{num5}</span>×<span>{row["unit_cnt"].ToString()}</span>&nbsp;<br>{$"{row["amount"]:F0}"}&nbsp;";
                                }
                            }
                            else
                            {
                                str9 = num5.ToString();
                            }
                            dictionary3.Add("amounttext", str9);
                        }
                        else
                        {
                            dictionary3.Add("amounttext", Math.Round(Convert.ToDecimal(row["amount"].ToString()), 0).ToString());
                        }
                        if ((row["sale_type"]).Equals("1"))
                        {
                            if (model.get_u_name().Equals(str8))
                            {
                                dictionary3.Add("amount", "-" + Math.Round(Convert.ToDecimal(row["amount"].ToString()), 0).ToString());
                            }
                            else if ((row["sale_type"]).Equals("1"))
                            {
                                dictionary3.Add("amount", "-" + Math.Round(Convert.ToDecimal(row["amount"].ToString()), 0).ToString());
                            }
                            else
                            {
                                dictionary3.Add("amount", Math.Round(Convert.ToDecimal(row["amount"].ToString()), 0).ToString());
                            }
                        }
                        else
                        {
                            dictionary3.Add("amount", Math.Round(Convert.ToDecimal(row["amount"].ToString()), 0).ToString());
                        }
                        dictionary3.Add("szamount", row["ds"].ToString());
                        dictionary3.Add("isdelete", row["isDelete"].ToString());
                        string str10 = row["play_id"].ToString();
                        string str11 = row["odds_id"].ToString();
                        dictionary3.Add("week", Utils.GetWeekByDate(Convert.ToDateTime(row["bet_time"].ToString())));
                        dictionary3.Add("ordernum", row["order_num"].ToString());
                        dictionary3.Add("phase", row["phase"].ToString());
                        dictionary3.Add("bettime", row["bet_time"].ToString());
                        dictionary3.Add("lottery_type", row["lottery_type"].ToString());
                        dictionary3.Add("lottery_name", base.GetGameNameByID(row["lottery_type"].ToString()));
                        dictionary3.Add("islm", row["isLM"].ToString());
                        dictionary3.Add("lmtype", row["lm_type"].ToString());
                        dictionary3.Add("hyname", row["u_name"].ToString());
                        dictionary3.Add("dlname", row["dl_name"]);
                        dictionary3.Add("zdname", row["zd_name"]);
                        dictionary3.Add("gdname", row["gd_name"]);
                        dictionary3.Add("fgsname", row["fgs_name"]);
                        dictionary3.Add("hydrawback", row["hy_drawback"]);
                        dictionary3.Add("dldrawback", row["dl_drawback"]);
                        dictionary3.Add("zddrawback", row["zd_drawback"]);
                        dictionary3.Add("gddrawback", row["gd_drawback"]);
                        dictionary3.Add("fgsdrawback", row["fgs_drawback"]);
                        dictionary3.Add("zjdrawback", row["zj_drawback"]);
                        dictionary3.Add("dlrate", row["dl_rate"]);
                        dictionary3.Add("zdrate", row["zd_rate"]);
                        dictionary3.Add("gdrate", row["gd_rate"]);
                        dictionary3.Add("fgsrate", row["fgs_rate"]);
                        dictionary3.Add("zjrate", row["zj_rate"]);
                        dictionary3.Add("unitcnt", row["unit_cnt"]);
                        dictionary3.Add("pk", row["kind"].ToString());
                        string str12 = "";
                        int num7 = 8;
                        if (row["lottery_type"].ToString().Equals(num7.ToString()))
                        {
                            str12 = $" #[第{row["table_type"].ToString()}桌 {Utils.GetPKBJLPlaytypeColorTxt(row["play_type"].ToString())}] ";
                        }
                        dictionary3.Add("tabletype", str12);
                        string str13 = "";
                        if (!(string.IsNullOrEmpty(row["unit_cnt"].ToString()) || (int.Parse(row["unit_cnt"].ToString()) <= 1)))
                        {
                            str13 = base.GroupShowHrefString(2, row["order_num"].ToString(), row["is_payment"].ToString(), "1", "1");
                        }
                        if (row["fgs_name"].ToString().Equals(str))
                        {
                            dictionary3.Add("saletype", "2");
                        }
                        else if ((row["sale_type"]).Equals("1"))
                        {
                            dictionary3.Add("saletype", "1");
                        }
                        else
                        {
                            dictionary3.Add("saletype", "0");
                        }
                        if (((((("329".Equals(str11) || "330".Equals(str11)) || ("331".Equals(str11) || "1181".Equals(str11))) || (("1200".Equals(str11) || "1201".Equals(str11)) || ("1202".Equals(str11) || "1203".Equals(str11)))) || ((("330".Equals(str11) || "331".Equals(str11)) || ("1181".Equals(str11) || "1202".Equals(str11))) || (("1203".Equals(str11) || "72055".Equals(str11)) || ("329".Equals(str11) || "330".Equals(str11))))) || ((("331".Equals(str11) || "1181".Equals(str11)) || ("1200".Equals(str11) || "1201".Equals(str11))) || "1202".Equals(str11))) || "1203".Equals(str11))
                        {
                            string str14 = row["lm_type"].ToString();
                            dictionary3.Add("playname", row["play_name"].ToString());
                            string str15 = model.get_u_type().Equals("zj") ? row["odds_zj"].ToString() : row["odds"].ToString();
                            dictionary3.Add("odds", str15);
                            if (str14.Equals("0"))
                            {
                                string str16 = "";
                                str16 = str16 + $"<br />【{row["unit_cnt"].ToString()}組】 {str13}" + $"<br />{row["bet_val"].ToString()}";
                                dictionary3.Add("betval", str16);
                            }
                        }
                        else
                        {
                            dictionary3.Add("playname", row["play_name"].ToString() + "【" + row["bet_val"].ToString() + "】");
                            dictionary3.Add("odds", model.get_u_type().Equals("zj") ? row["odds_zj"].ToString() : row["odds"].ToString());
                            dictionary3.Add("betval", row["bet_val"].ToString());
                        }
                        dictionary2.Add(num4.ToString(), dictionary3);
                    }
                    dictionary.Add("newbetlist", dictionary2);
                }
                Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
                dictionary.Add("maxidvalid", num2);
                DateTime now = DateTime.Now;
                List<object> list = base.GetAutoJPForTable(str6, compareTime, ref now);
                Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
                if (list != null)
                {
                    dictionary5.Add("tipsList", list);
                }
                else
                {
                    dictionary5.Add("tipsList", new List<object>());
                }
                dictionary5.Add("timestamp", Utils.DateTimeToStamp(now));
                dictionary.Add("autoJP", dictionary5);
                result.set_data(dictionary);
                strResult = JsonHandle.ObjectToJson(result);
            }
        }

        protected string get_pcdd_bs_str(string num)
        {
            string str = "03,06,09,12,15,18,21,24";
            string str2 = "02,05,08,11,17,20,23,26";
            string str3 = "01,04,07,10,16,19,22,25";
            string s = num;
            if (int.Parse(s) < 10)
            {
                s = "0" + int.Parse(s);
            }
            if (str.IndexOf(s) > -1)
            {
                return "紅波";
            }
            if (str2.IndexOf(s) > -1)
            {
                return "藍波";
            }
            if (str3.IndexOf(s) > -1)
            {
                return "綠波";
            }
            return "-";
        }

        public void get_reportcookies(HttpContext context, ref string strResult)
        {
            ReturnResult result = new ReturnResult();
            result.set_success(400);
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                { 
                    "type",
                    "get_reportcookies"
                }
            };
            if (context.Session["user_name"] == null)
            {
                context.Response.End();
            }
            else
            {
                HttpCookie reportCookies = LSRequest.GetReportCookies();
                Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                if (reportCookies != null)
                {
                    dictionary2.Add("t_TYPE", reportCookies.Values["t_TYPE"]);
                    dictionary2.Add("t_LT", reportCookies.Values["t_LT"]);
                    dictionary2.Add("gID", reportCookies.Values["gID"]);
                    dictionary2.Add("sltTabletype", reportCookies.Values["sltTabletype"]);
                    dictionary2.Add("sltPlaytype", reportCookies.Values["sltPlaytype"]);
                    dictionary2.Add("t_LID", reportCookies.Values["t_LID"]);
                    dictionary2.Add("isQs", reportCookies.Values["isQs"]);
                    dictionary2.Add("t_FT", reportCookies.Values["t_FT"]);
                    dictionary2.Add("BeginDate", reportCookies.Values["BeginDate"]);
                    dictionary2.Add("EndDate", reportCookies.Values["EndDate"]);
                    dictionary2.Add("ReportType", reportCookies.Values["ReportType"]);
                    dictionary2.Add("t_Balance", reportCookies.Values["t_Balance"]);
                }
                dictionary.Add("cookies", dictionary2);
                result.set_data(dictionary);
                result.set_success(200);
                strResult = JsonHandle.ObjectToJson(result);
            }
        }

        public void get_six_date(HttpContext context, ref string strResult)
        {
            string s = base.get_six_schedule();
            HttpContext.Current.Response.AddHeader("content-type", "text/xml");
            if (s == "")
            {
                HttpContext.Current.Response.Write("<script>alert('接口获取数据失败!');</script>");
            }
            else
            {
                context.Response.Write(s);
            }
            context.Response.End();
        }

        protected string get_tz(string hm)
        {
            int num = 0;
            if (hm.Trim() == "49")
            {
                return "和";
            }
            num = int.Parse(hm) % 4;
            if (num == 0)
            {
                return "4";
            }
            int num2 = int.Parse(hm) % 4;
            return num2.ToString();
        }

        public void get_user_rate(HttpContext context, ref string strResult)
        {
            ReturnResult result = new ReturnResult();
            result.set_success(400);
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                { 
                    "type",
                    "get_user_rate"
                }
            };
            if (context.Session["user_name"] == null)
            {
                context.Response.End();
            }
            else
            {
                DataTable rateByUserID;
                string str3;
                string str4;
                string str5;
                string str6;
                string str7;
                string str8;
                string str9;
                string str10;
                string str11;
                string str12;
                string str13;
                string str14;
                Dictionary<string, string> dictionary3;
                string str = LSRequest.qq("uid");
                string str2 = context.Session["user_name"].ToString();
                agent_userinfo_session _session = context.Session[str2 + "lottery_session_user_info"] as agent_userinfo_session;
                cz_users userInfoByUID = CallBLL.cz_users_bll.GetUserInfoByUID(str);
                if (!base.IsUpperLowerLevels(userInfoByUID.get_u_name(), _session.get_u_type(), _session.get_u_name()))
                {
                    context.Response.Write(PageBase.GetMessageByCache("u100014", "MessageHint"));
                    context.Response.End();
                }
                DataTable lotteryList = base.GetLotteryList();
                DataRow[] rowArray = lotteryList.Select($" master_id={1} ");
                DataRow[] rowArray2 = lotteryList.Select($" master_id={2} ");
                Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
                if (rowArray.Length > 0)
                {
                    rateByUserID = CallBLL.cz_rate_six_bll.GetRateByUserID(str);
                    str3 = rateByUserID.Rows[0]["u_type"].ToString();
                    str4 = rateByUserID.Rows[0]["su_type"].ToString();
                    str5 = "";
                    str6 = "";
                    str7 = "";
                    str8 = "";
                    str9 = "";
                    str10 = "";
                    str11 = "";
                    str12 = "";
                    str13 = "";
                    str14 = "";
                    str5 = _session.get_zjname();
                    str6 = rateByUserID.Rows[0]["zj_rate"];
                    str7 = rateByUserID.Rows[0]["fgs_name"];
                    str8 = rateByUserID.Rows[0]["fgs_rate"];
                    if (str4.Equals("dl"))
                    {
                        str9 = rateByUserID.Rows[0]["gd_name"];
                        str10 = rateByUserID.Rows[0]["gd_rate"];
                        str11 = rateByUserID.Rows[0]["zd_name"];
                        str12 = rateByUserID.Rows[0]["zd_rate"];
                        str13 = rateByUserID.Rows[0]["dl_name"];
                        str14 = rateByUserID.Rows[0]["dl_rate"];
                    }
                    if (str4.Equals("zd"))
                    {
                        str9 = rateByUserID.Rows[0]["gd_name"];
                        str10 = rateByUserID.Rows[0]["gd_rate"];
                        str11 = rateByUserID.Rows[0]["zd_name"];
                        str12 = rateByUserID.Rows[0]["zd_rate"];
                    }
                    if (str4.Equals("gd"))
                    {
                        str9 = rateByUserID.Rows[0]["gd_name"];
                        str10 = rateByUserID.Rows[0]["gd_rate"];
                    }
                    dictionary3 = new Dictionary<string, string>();
                    switch (_session.get_u_type())
                    {
                        case "zj":
                            if (!string.IsNullOrEmpty(str5))
                            {
                                if (_session.get_users_child_session() != null)
                                {
                                    str5 = "-";
                                }
                                dictionary3.Add("zj", $"{str5},{str6}");
                            }
                            if (!string.IsNullOrEmpty(str7))
                            {
                                dictionary3.Add("fgs", $"{str7},{str8}");
                            }
                            if (!string.IsNullOrEmpty(str9))
                            {
                                dictionary3.Add("gd", $"{str9},{str10}");
                            }
                            if (!string.IsNullOrEmpty(str11))
                            {
                                dictionary3.Add("zd", $"{str11},{str12}");
                            }
                            if (!string.IsNullOrEmpty(str13))
                            {
                                dictionary3.Add("dl", $"{str13},{str14}");
                            }
                            break;

                        case "fgs":
                            if (!string.IsNullOrEmpty(str7))
                            {
                                if (_session.get_users_child_session() != null)
                                {
                                    str7 = "-";
                                }
                                dictionary3.Add("fgs", $"{str7},{str8}");
                            }
                            if (!string.IsNullOrEmpty(str9))
                            {
                                dictionary3.Add("gd", $"{str9},{str10}");
                            }
                            if (!string.IsNullOrEmpty(str11))
                            {
                                dictionary3.Add("zd", $"{str11},{str12}");
                            }
                            if (!string.IsNullOrEmpty(str13))
                            {
                                dictionary3.Add("dl", $"{str13},{str14}");
                            }
                            break;

                        case "gd":
                            if (!string.IsNullOrEmpty(str9))
                            {
                                if (_session.get_users_child_session() != null)
                                {
                                    str9 = "-";
                                }
                                dictionary3.Add("gd", $"{str9},{str10}");
                            }
                            if (!string.IsNullOrEmpty(str11))
                            {
                                dictionary3.Add("zd", $"{str11},{str12}");
                            }
                            if (!string.IsNullOrEmpty(str13))
                            {
                                dictionary3.Add("dl", $"{str13},{str14}");
                            }
                            break;

                        case "zd":
                            if (!string.IsNullOrEmpty(str11))
                            {
                                if (_session.get_users_child_session() != null)
                                {
                                    str11 = "-";
                                }
                                dictionary3.Add("zd", $"{str11},{str12}");
                            }
                            if (!string.IsNullOrEmpty(str13))
                            {
                                dictionary3.Add("dl", $"{str13},{str14}");
                            }
                            break;

                        case "dl":
                            if (!string.IsNullOrEmpty(str13))
                            {
                                if (_session.get_users_child_session() != null)
                                {
                                    str13 = "-";
                                }
                                dictionary3.Add("dl", $"{str13},{str14}");
                            }
                            break;
                    }
                    dictionary2.Add("six", dictionary3);
                }
                if (rowArray2.Length > 0)
                {
                    rateByUserID = CallBLL.cz_rate_kc_bll.GetRateByUserID(str);
                    if (rateByUserID == null)
                    {
                        return;
                    }
                    str3 = rateByUserID.Rows[0]["u_type"].ToString();
                    str4 = rateByUserID.Rows[0]["su_type"].ToString();
                    str5 = "";
                    str6 = "";
                    str7 = "";
                    str8 = "";
                    str9 = "";
                    str10 = "";
                    str11 = "";
                    str12 = "";
                    str13 = "";
                    str14 = "";
                    str5 = _session.get_zjname();
                    str6 = rateByUserID.Rows[0]["zj_rate"];
                    str7 = rateByUserID.Rows[0]["fgs_name"];
                    str8 = rateByUserID.Rows[0]["fgs_rate"];
                    if (str4.Equals("dl"))
                    {
                        str9 = rateByUserID.Rows[0]["gd_name"];
                        str10 = rateByUserID.Rows[0]["gd_rate"];
                        str11 = rateByUserID.Rows[0]["zd_name"];
                        str12 = rateByUserID.Rows[0]["zd_rate"];
                        str13 = rateByUserID.Rows[0]["dl_name"];
                        str14 = rateByUserID.Rows[0]["dl_rate"];
                    }
                    if (str4.Equals("zd"))
                    {
                        str9 = rateByUserID.Rows[0]["gd_name"];
                        str10 = rateByUserID.Rows[0]["gd_rate"];
                        str11 = rateByUserID.Rows[0]["zd_name"];
                        str12 = rateByUserID.Rows[0]["zd_rate"];
                    }
                    if (str4.Equals("gd"))
                    {
                        str9 = rateByUserID.Rows[0]["gd_name"];
                        str10 = rateByUserID.Rows[0]["gd_rate"];
                    }
                    dictionary3 = new Dictionary<string, string>();
                    switch (_session.get_u_type())
                    {
                        case "zj":
                            if (!string.IsNullOrEmpty(str5))
                            {
                                if (_session.get_users_child_session() != null)
                                {
                                    str5 = "-";
                                }
                                dictionary3.Add("zj", $"{str5},{str6}");
                            }
                            if (!string.IsNullOrEmpty(str7))
                            {
                                dictionary3.Add("fgs", $"{str7},{str8}");
                            }
                            if (!string.IsNullOrEmpty(str9))
                            {
                                dictionary3.Add("gd", $"{str9},{str10}");
                            }
                            if (!string.IsNullOrEmpty(str11))
                            {
                                dictionary3.Add("zd", $"{str11},{str12}");
                            }
                            if (!string.IsNullOrEmpty(str13))
                            {
                                dictionary3.Add("dl", $"{str13},{str14}");
                            }
                            break;

                        case "fgs":
                            if (!string.IsNullOrEmpty(str7))
                            {
                                if (_session.get_users_child_session() != null)
                                {
                                    str7 = "-";
                                }
                                dictionary3.Add("fgs", $"{str7},{str8}");
                            }
                            if (!string.IsNullOrEmpty(str9))
                            {
                                dictionary3.Add("gd", $"{str9},{str10}");
                            }
                            if (!string.IsNullOrEmpty(str11))
                            {
                                dictionary3.Add("zd", $"{str11},{str12}");
                            }
                            if (!string.IsNullOrEmpty(str13))
                            {
                                dictionary3.Add("dl", $"{str13},{str14}");
                            }
                            break;

                        case "gd":
                            if (!string.IsNullOrEmpty(str9))
                            {
                                if (_session.get_users_child_session() != null)
                                {
                                    str9 = "-";
                                }
                                dictionary3.Add("gd", $"{str9},{str10}");
                            }
                            if (!string.IsNullOrEmpty(str11))
                            {
                                dictionary3.Add("zd", $"{str11},{str12}");
                            }
                            if (!string.IsNullOrEmpty(str13))
                            {
                                dictionary3.Add("dl", $"{str13},{str14}");
                            }
                            break;

                        case "zd":
                            if (!string.IsNullOrEmpty(str11))
                            {
                                if (_session.get_users_child_session() != null)
                                {
                                    str11 = "-";
                                }
                                dictionary3.Add("zd", $"{str11},{str12}");
                            }
                            if (!string.IsNullOrEmpty(str13))
                            {
                                dictionary3.Add("dl", $"{str13},{str14}");
                            }
                            break;

                        case "dl":
                            if (!string.IsNullOrEmpty(str13))
                            {
                                if (_session.get_users_child_session() != null)
                                {
                                    str13 = "-";
                                }
                                dictionary3.Add("dl", $"{str13},{str14}");
                            }
                            break;
                    }
                    dictionary2.Add("kc", dictionary3);
                }
                dictionary.Add("tips", dictionary2);
                result.set_success(200);
                result.set_data(dictionary);
                strResult = JsonHandle.ObjectToJson(result);
            }
        }

        public void GetPhaseByLottery(HttpContext context, ref string strResult)
        {
            ReturnResult result = new ReturnResult();
            result.set_success(200);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (context.Session["user_name"] == null)
            {
                result.set_success(300);
                dictionary.Add("type", "get_phasebylottery");
                result.set_data(dictionary);
                strResult = JsonHandle.ObjectToJson(result);
                context.Response.End();
            }
            else
            {
                int num2;
                int num = Convert.ToInt32(LSRequest.qq("lid"));
                string str2 = DateTime.Now.AddHours((double) -int.Parse(base.get_BetTime_KC())).ToString("yyyy-MM-dd");
                DataTable phaseByQueryDate = null;
                DataTable play = null;
                switch (num)
                {
                    case 0:
                        phaseByQueryDate = CallBLL.cz_phase_kl10_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_kl10_bll.GetPlay();
                        break;

                    case 1:
                        phaseByQueryDate = CallBLL.cz_phase_cqsc_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_cqsc_bll.GetPlay();
                        break;

                    case 2:
                        phaseByQueryDate = CallBLL.cz_phase_pk10_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_pk10_bll.GetPlay();
                        break;

                    case 3:
                        phaseByQueryDate = CallBLL.cz_phase_xync_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_xync_bll.GetPlay();
                        break;

                    case 4:
                        phaseByQueryDate = CallBLL.cz_phase_jsk3_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_jsk3_bll.GetPlay();
                        break;

                    case 5:
                        phaseByQueryDate = CallBLL.cz_phase_kl8_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_kl8_bll.GetPlay();
                        break;

                    case 6:
                        phaseByQueryDate = CallBLL.cz_phase_k8sc_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_k8sc_bll.GetPlay();
                        break;

                    case 7:
                        phaseByQueryDate = CallBLL.cz_phase_pcdd_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_pcdd_bll.GetPlay();
                        break;

                    case 8:
                        phaseByQueryDate = CallBLL.cz_phase_pkbjl_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_pkbjl_bll.GetPlay();
                        break;

                    case 9:
                        phaseByQueryDate = CallBLL.cz_phase_xyft5_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_xyft5_bll.GetPlay();
                        break;

                    case 10:
                        phaseByQueryDate = CallBLL.cz_phase_jscar_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_jscar_bll.GetPlay();
                        break;

                    case 11:
                        phaseByQueryDate = CallBLL.cz_phase_speed5_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_speed5_bll.GetPlay();
                        break;

                    case 12:
                        phaseByQueryDate = CallBLL.cz_phase_jspk10_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_jspk10_bll.GetPlay();
                        break;

                    case 13:
                        phaseByQueryDate = CallBLL.cz_phase_jscqsc_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_jscqsc_bll.GetPlay();
                        break;

                    case 14:
                        phaseByQueryDate = CallBLL.cz_phase_jssfc_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_jssfc_bll.GetPlay();
                        break;

                    case 15:
                        phaseByQueryDate = CallBLL.cz_phase_jsft2_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_jsft2_bll.GetPlay();
                        break;

                    case 0x10:
                        phaseByQueryDate = CallBLL.cz_phase_car168_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_car168_bll.GetPlay();
                        break;

                    case 0x11:
                        phaseByQueryDate = CallBLL.cz_phase_ssc168_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_ssc168_bll.GetPlay();
                        break;

                    case 0x12:
                        phaseByQueryDate = CallBLL.cz_phase_vrcar_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_vrcar_bll.GetPlay();
                        break;

                    case 0x13:
                        phaseByQueryDate = CallBLL.cz_phase_vrssc_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_vrssc_bll.GetPlay();
                        break;

                    case 20:
                        phaseByQueryDate = CallBLL.cz_phase_xyftoa_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_xyftoa_bll.GetPlay();
                        break;

                    case 0x15:
                        phaseByQueryDate = CallBLL.cz_phase_xyftsg_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_xyftsg_bll.GetPlay();
                        break;

                    case 0x16:
                        phaseByQueryDate = CallBLL.cz_phase_happycar_bll.GetPhaseByQueryDate(str2);
                        play = CallBLL.cz_play_happycar_bll.GetPlay();
                        break;

                    case 100:
                        phaseByQueryDate = CallBLL.cz_phase_six_bll.GetCurrentPhase("20");
                        if (!FileCacheHelper.get_IsShowLM_B())
                        {
                            play = CallBLL.cz_play_six_bll.GetPlay("91060,91061,91062,91063,91064,91065");
                        }
                        else
                        {
                            play = CallBLL.cz_play_six_bll.GetPlay();
                        }
                        break;
                }
                dictionary.Add("type", "get_phasebylottery");
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                if (phaseByQueryDate != null)
                {
                    if (num.Equals(8))
                    {
                        Dictionary<string, string> source = new Dictionary<string, string>();
                        for (num2 = 0; num2 < phaseByQueryDate.Rows.Count; num2++)
                        {
                            string str3 = phaseByQueryDate.Rows[num2]["p_id"].ToString();
                            string key = phaseByQueryDate.Rows[num2]["phase"].ToString();
                            if (!source.ContainsKey(key))
                            {
                                source.Add(key, str3);
                            }
                        }
                        for (num2 = 0; num2 < source.Count; num2++)
                        {
                            if (num2 == 0)
                            {
                                builder.Append(source.ElementAt<KeyValuePair<string, string>>(num2).Value + "," + source.ElementAt<KeyValuePair<string, string>>(num2).Key + " 期");
                            }
                            else
                            {
                                builder.Append("|" + source.ElementAt<KeyValuePair<string, string>>(num2).Value + "," + source.ElementAt<KeyValuePair<string, string>>(num2).Key + " 期");
                            }
                        }
                    }
                    else
                    {
                        num2 = 0;
                        while (num2 < phaseByQueryDate.Rows.Count)
                        {
                            if (num2 == 0)
                            {
                                builder.Append(phaseByQueryDate.Rows[num2]["p_id"].ToString().Trim() + "," + phaseByQueryDate.Rows[num2]["phase"].ToString().Trim() + " 期");
                            }
                            else
                            {
                                builder.Append("|" + phaseByQueryDate.Rows[num2]["p_id"].ToString().Trim() + "," + phaseByQueryDate.Rows[num2]["phase"].ToString().Trim() + " 期");
                            }
                            num2++;
                        }
                    }
                    dictionary.Add("phaseoption", builder.ToString());
                }
                if (play != null)
                {
                    for (num2 = 0; num2 < play.Rows.Count; num2++)
                    {
                        if (num2 == 0)
                        {
                            builder2.Append(play.Rows[num2]["play_id"].ToString().Trim() + "," + play.Rows[num2]["play_name"].ToString().Trim());
                        }
                        else
                        {
                            builder2.Append("|" + play.Rows[num2]["play_id"].ToString().Trim() + "," + play.Rows[num2]["play_name"].ToString().Trim());
                        }
                    }
                    dictionary.Add("playoption", builder2.ToString());
                }
                if (num.Equals(8))
                {
                    dictionary.Add("tabletype", string.Format("p1,第1桌|p2,第2桌|p3,第3桌|p4,第4桌|p5,第5桌", new object[0]));
                    dictionary.Add("playtype", string.Format("1,一般|0,免傭", new object[0]));
                }
                result.set_data(dictionary);
                strResult = JsonHandle.ObjectToJson(result);
            }
        }

        public void GetPlayByLottery(HttpContext context, ref string strResult)
        {
            ReturnResult result = new ReturnResult();
            result.set_success(200);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (context.Session["user_name"] == null)
            {
                result.set_success(300);
                dictionary.Add("type", "get_playbylottery");
                result.set_data(dictionary);
                strResult = JsonHandle.ObjectToJson(result);
                context.Response.End();
            }
            else if (!context.Session["user_type"].ToString().Equals("zj"))
            {
                result.set_success(400);
                dictionary.Add("type", "get_playbylottery");
                result.set_data(dictionary);
                result.set_tipinfo(PageBase.GetMessageByCache("u100014", "MessageHint"));
                strResult = JsonHandle.ObjectToJson(result);
                context.Response.End();
            }
            else
            {
                int num = Convert.ToInt32(LSRequest.qq("lid"));
                DataTable play = null;
                switch (num)
                {
                    case 0:
                        play = CallBLL.cz_play_kl10_bll.GetPlay();
                        break;

                    case 1:
                        play = CallBLL.cz_play_cqsc_bll.GetPlay();
                        break;

                    case 2:
                        play = CallBLL.cz_play_pk10_bll.GetPlay();
                        break;

                    case 3:
                        play = CallBLL.cz_play_xync_bll.GetPlay();
                        break;

                    case 4:
                        play = CallBLL.cz_play_jsk3_bll.GetPlay();
                        break;

                    case 5:
                        play = CallBLL.cz_play_kl8_bll.GetPlay();
                        break;

                    case 6:
                        play = CallBLL.cz_play_k8sc_bll.GetPlay();
                        break;

                    case 7:
                        play = CallBLL.cz_play_pcdd_bll.GetPlay();
                        break;

                    case 8:
                        play = CallBLL.cz_play_pkbjl_bll.GetPlay();
                        break;

                    case 9:
                        play = CallBLL.cz_play_xyft5_bll.GetPlay();
                        break;

                    case 10:
                        play = CallBLL.cz_play_jscar_bll.GetPlay();
                        break;

                    case 11:
                        play = CallBLL.cz_play_speed5_bll.GetPlay();
                        break;

                    case 12:
                        play = CallBLL.cz_play_jspk10_bll.GetPlay();
                        break;

                    case 13:
                        play = CallBLL.cz_play_jscqsc_bll.GetPlay();
                        break;

                    case 14:
                        play = CallBLL.cz_play_jssfc_bll.GetPlay();
                        break;

                    case 15:
                        play = CallBLL.cz_play_jsft2_bll.GetPlay();
                        break;

                    case 0x10:
                        play = CallBLL.cz_play_car168_bll.GetPlay();
                        break;

                    case 0x11:
                        play = CallBLL.cz_play_ssc168_bll.GetPlay();
                        break;

                    case 0x12:
                        play = CallBLL.cz_play_vrcar_bll.GetPlay();
                        break;

                    case 0x13:
                        play = CallBLL.cz_play_vrssc_bll.GetPlay();
                        break;

                    case 20:
                        play = CallBLL.cz_play_xyftoa_bll.GetPlay();
                        break;

                    case 0x15:
                        play = CallBLL.cz_play_xyftsg_bll.GetPlay();
                        break;

                    case 0x16:
                        play = CallBLL.cz_play_happycar_bll.GetPlay();
                        break;

                    case 100:
                        if (!FileCacheHelper.get_IsShowLM_B())
                        {
                            play = CallBLL.cz_play_six_bll.GetPlay("91060,91061,91062,91063,91064,91065");
                        }
                        else
                        {
                            play = CallBLL.cz_play_six_bll.GetPlay();
                        }
                        break;
                }
                StringBuilder builder = new StringBuilder();
                if (play != null)
                {
                    for (int i = 0; i < play.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            builder.Append(play.Rows[i]["play_id"].ToString().Trim() + "," + play.Rows[i]["play_name"].ToString().Trim());
                        }
                        else
                        {
                            builder.Append("|" + play.Rows[i]["play_id"].ToString().Trim() + "," + play.Rows[i]["play_name"].ToString().Trim());
                        }
                    }
                    dictionary.Add("type", "get_playbylottery");
                    dictionary.Add("playoption", builder.ToString());
                    result.set_data(dictionary);
                    strResult = JsonHandle.ObjectToJson(result);
                }
            }
        }

        public void IsOpenLottery(HttpContext context, ref string strResult)
        {
            ReturnResult result = new ReturnResult();
            result.set_success(200);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (context.Session["user_name"] == null)
            {
                result.set_success(300);
                dictionary.Add("type", "isopenlottery");
                result.set_data(dictionary);
                strResult = JsonHandle.ObjectToJson(result);
                context.Response.End();
            }
            else
            {
                Dictionary<string, object> dictionary2;
                int num = Convert.ToInt32(LSRequest.qq("lid"));
                DataTable table = null;
                switch (num)
                {
                    case 0:
                        table = CallBLL.cz_phase_kl10_bll.IsPhaseClose();
                        break;

                    case 1:
                        table = CallBLL.cz_phase_cqsc_bll.IsPhaseClose();
                        break;

                    case 2:
                        table = CallBLL.cz_phase_pk10_bll.IsPhaseClose();
                        break;

                    case 3:
                        table = CallBLL.cz_phase_xync_bll.IsPhaseClose();
                        break;

                    case 4:
                        table = CallBLL.cz_phase_jsk3_bll.IsPhaseClose();
                        break;

                    case 5:
                        table = CallBLL.cz_phase_kl8_bll.IsPhaseClose();
                        break;

                    case 6:
                        table = CallBLL.cz_phase_k8sc_bll.IsPhaseClose();
                        break;

                    case 7:
                        table = CallBLL.cz_phase_pcdd_bll.IsPhaseClose();
                        break;

                    case 8:
                        table = CallBLL.cz_phase_pkbjl_bll.IsPhaseClose();
                        break;

                    case 9:
                        table = CallBLL.cz_phase_xyft5_bll.IsPhaseClose();
                        break;

                    case 10:
                        table = CallBLL.cz_phase_jscar_bll.IsPhaseClose();
                        break;

                    case 11:
                        table = CallBLL.cz_phase_speed5_bll.IsPhaseClose();
                        break;

                    case 12:
                        table = CallBLL.cz_phase_jspk10_bll.IsPhaseClose();
                        break;

                    case 13:
                        table = CallBLL.cz_phase_jscqsc_bll.IsPhaseClose();
                        break;

                    case 14:
                        table = CallBLL.cz_phase_jssfc_bll.IsPhaseClose();
                        break;

                    case 15:
                        table = CallBLL.cz_phase_jsft2_bll.IsPhaseClose();
                        break;

                    case 0x10:
                        table = CallBLL.cz_phase_car168_bll.IsPhaseClose();
                        break;

                    case 0x11:
                        table = CallBLL.cz_phase_ssc168_bll.IsPhaseClose();
                        break;

                    case 0x12:
                        table = CallBLL.cz_phase_vrcar_bll.IsPhaseClose();
                        break;

                    case 0x13:
                        table = CallBLL.cz_phase_vrssc_bll.IsPhaseClose();
                        break;

                    case 20:
                        table = CallBLL.cz_phase_xyftoa_bll.IsPhaseClose();
                        break;

                    case 0x15:
                        table = CallBLL.cz_phase_xyftsg_bll.IsPhaseClose();
                        break;

                    case 0x16:
                        table = CallBLL.cz_phase_happycar_bll.IsPhaseClose();
                        break;
                }
                if (table.Rows[0]["isopen"].ToString().Equals("0"))
                {
                    dictionary.Add("type", "isopenlottery");
                    dictionary2 = new Dictionary<string, object> {
                        { 
                            "isopen",
                            "-1"
                        }
                    };
                    dictionary.Add("isopenvalue", dictionary2);
                    result.set_data(dictionary);
                    result.set_success(200);
                    strResult = JsonHandle.ObjectToJson(result);
                }
                else
                {
                    dictionary.Add("type", "isopenlottery");
                    dictionary2 = new Dictionary<string, object> {
                        { 
                            "isopen",
                            "1"
                        }
                    };
                    dictionary.Add("isopenvalue", dictionary2);
                    result.set_data(dictionary);
                    result.set_success(200);
                    strResult = JsonHandle.ObjectToJson(result);
                }
            }
        }

        public override void ProcessRequest(HttpContext context)
        {
            base.checkLoginByHandler(0);
            string str = LSRequest.qq("action");
            string strResult = "";
            if (!string.IsNullOrEmpty(str))
            {
                switch (str)
                {
                    case "get_ad":
                        this.get_ad(context, ref strResult);
                        break;

                    case "get_newbetlist":
                        this.get_newbetlist(context, ref strResult);
                        break;

                    case "get_openlottery":
                        this.IsOpenLottery(context, ref strResult);
                        break;

                    case "get_playbylottery":
                        this.GetPlayByLottery(context, ref strResult);
                        break;

                    case "get_phasebylottery":
                        this.GetPhaseByLottery(context, ref strResult);
                        break;

                    case "set_skin":
                        this.set_skin(context, ref strResult);
                        break;

                    case "get_six_date":
                        this.get_six_date(context, ref strResult);
                        break;

                    case "get_user_rate":
                        this.get_user_rate(context, ref strResult);
                        break;

                    case "get_currentphase":
                        this.get_currentphase(context, ref strResult);
                        break;

                    case "get_gamehall":
                        this.get_gamehall(context, ref strResult);
                        break;

                    case "get_reptcookies":
                        this.get_reportcookies(context, ref strResult);
                        break;
                }
                context.Response.ContentType = "text/json";
                context.Response.Write(strResult);
            }
        }

        public void set_skin(HttpContext context, ref string strResult)
        {
            ReturnResult result = new ReturnResult();
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                { 
                    "type",
                    "set_skin"
                }
            };
            string str = LSRequest.qq("skin");
            if (context.Session["user_name"] == null)
            {
                context.Response.End();
            }
            else
            {
                agent_userinfo_session _session = new agent_userinfo_session();
                string str2 = context.Session["user_name"].ToString();
                _session = context.Session[str2 + "lottery_session_user_info"] as agent_userinfo_session;
                string str3 = _session.get_u_id().ToString();
                int num = -1;
                if (context.Session["child_user_name"] != null)
                {
                    str3 = _session.get_users_child_session().get_u_id().ToString();
                    num = CallBLL.cz_users_child_bll.UpdateSkin(str3, str);
                }
                else
                {
                    num = CallBLL.cz_users_bll.UpdateSkin(str3, str);
                }
                if (num > 0)
                {
                    result.set_success(200);
                    _session.set_u_skin(str);
                    context.Session[str2 + "lottery_session_user_info"] = _session;
                }
                else
                {
                    result.set_success(400);
                    result.set_tipinfo(PageBase.GetMessageByCache("u100059", "MessageHint"));
                }
                result.set_data(dictionary);
                strResult = JsonHandle.ObjectToJson(result);
            }
        }

        public bool IsReusable =>
            false;
    }
}

