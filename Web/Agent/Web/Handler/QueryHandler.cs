using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Agent.Web.WebBase;
using Entity;
using LotterySystem.Common;
using LotterySystem.Common.Redis;
using LotterySystem.Model;

namespace Agent.Web.Handler
{
	public class QueryHandler : BaseHandler
	{
		public new bool IsReusable => false;

		public void get_ad(HttpContext context, ref string strResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			ReturnResult val = new ReturnResult();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("type", "get_ad");
			new agent_userinfo_session();
			string text = context.Session["user_name"].ToString();
			agent_userinfo_session val2 = context.Session[text + "lottery_session_user_info"] as agent_userinfo_session;
			string u_type = val2.get_u_type();
			string text2 = LSRequest.qq("browserCode");
			string value = string.Concat(HttpContext.Current.Session["lottery_session_img_code_brower"]);
			if (!(text2?.Equals(value) ?? false))
			{
				Session.Abandon();
				val.set_success(300);
				val.set_data(dictionary);
				strResult = JsonHandle.ObjectToJson(val);
				return;
			}
			int current_master_id = get_current_master_id();
			current_master_id.ToString();
			string text3;
			switch (current_master_id)
			{
			case 0:
				text3 = "0,1,2";
				break;
			case 1:
				text3 = "0,1";
				break;
			default:
				text3 = "0,2";
				break;
			}
			List<object> list = this.get_ad_level(u_type, 3, text3);
			if (list == null)
			{
				dictionary.Add("ad", new List<object>());
			}
			else
			{
				dictionary.Add("ad", list);
			}
			if (val2.get_u_type().Equals("zj"))
			{
				string value2 = FileCacheHelper.get_RedisStatOnline().Equals(1) ? this.get_online_cnt("redis") : ((!FileCacheHelper.get_RedisStatOnline().Equals(2)) ? get_online_cnt() : this.get_online_cntStack("redis"));
				dictionary.Add("online_cnt", value2);
			}
			string children_name = get_children_name();
			if (FileCacheHelper.get_RedisStatOnline().Equals(1) || FileCacheHelper.get_RedisStatOnline().Equals(2))
			{
				bool flag = false;
				if (val2.get_users_child_session() != null && val2.get_users_child_session().get_is_admin().Equals(1))
				{
					flag = true;
				}
				if (!flag)
				{
					if (FileCacheHelper.get_RedisStatOnline().Equals(1))
					{
						this.CheckIsOut((children_name == "") ? text : children_name);
						this.stat_online_redis((children_name == "") ? text : children_name, u_type);
					}
					else if (FileCacheHelper.get_RedisStatOnline().Equals(2))
					{
						this.CheckIsOutStack((children_name == "") ? text : children_name);
						this.stat_online_redisStack((children_name == "") ? text : children_name, u_type);
					}
				}
				if (FileCacheHelper.get_RedisStatOnline().Equals(1))
				{
					this.stat_online_redis_timer();
				}
				else if (FileCacheHelper.get_RedisStatOnline().Equals(2))
				{
					this.stat_online_redis_timerStack();
				}
			}
			else
			{
				MemberPageBase.stat_online((children_name == "") ? text : children_name, u_type);
			}
			string compareTime = LSRequest.qq("oldTime");
			DateTime dt = DateTime.Now;
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			if (IsAutoUpdate(context.Session["user_name"].ToString(), compareTime))
			{
				List<object> autoJPForAd = GetAutoJPForAd(compareTime, ref dt);
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
			dictionary2.Add("timestamp", Utils.DateTimeToStamp(dt));
			dictionary.Add("autoJP", dictionary2);
			val.set_data(dictionary);
			strResult = JsonHandle.ObjectToJson(val);
		}

		public void get_newbetlist(HttpContext context, ref string strResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			ReturnResult val = new ReturnResult();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("type", "get_newbetlist");
			new agent_userinfo_session();
			string text = context.Session["user_name"].ToString();
			agent_userinfo_session val2 = context.Session[text + "lottery_session_user_info"] as agent_userinfo_session;
			if (!val2.get_u_type().Trim().Equals("zj"))
			{
				context.Response.Write(PageBase.GetMessageByCache("u100027", "MessageHint"));
				context.Response.End();
			}
			string value = Permission_Ashx_ZJ(val2, "po_1_1");
			if (!string.IsNullOrEmpty(value))
			{
				context.Response.Write(PageBase.GetMessageByCache("u100027", "MessageHint"));
				context.Response.End();
			}
			string text2 = LSRequest.qq("top");
			string text3 = LSRequest.qq("amount");
			string text4 = LSRequest.qq("oldId");
			string text5 = LSRequest.qq("lids");
			if (string.IsNullOrEmpty(text5))
			{
				return;
			}
			string compareTime = LSRequest.qq("oldTime");
			if (string.IsNullOrEmpty(text3))
			{
				text3 = "0";
			}
			int num = 0;
			if (!string.IsNullOrEmpty(text4))
			{
				num = Convert.ToInt32(text4);
			}
			int num2 = num;
			DataTable newBet = CallBLL.CzBetKcService.GetNewBet(text2, text4, text5, text3);
			if (newBet != null)
			{
				Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
				for (int i = 0; i < newBet.Rows.Count; i++)
				{
					Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
					DataRow dataRow = newBet.Rows[i];
					string value2 = dataRow["u_name"].ToString();
					int num3 = Convert.ToInt32(dataRow["bet_id"].ToString());
					if (num3 > num2)
					{
						num2 = num3;
					}
					if (string.Concat(dataRow["isLM"]).Equals("1"))
					{
						int num4 = (string.IsNullOrEmpty(string.Concat(dataRow["unit_cnt"])) || int.Parse(dataRow["unit_cnt"].ToString()) <= 0) ? decimal.ToInt32(Convert.ToDecimal(dataRow["amount"])) : (decimal.ToInt32(Convert.ToDecimal(dataRow["amount"])) / int.Parse(dataRow["unit_cnt"].ToString()));
						string value3 = (string.IsNullOrEmpty(string.Concat(dataRow["unit_cnt"])) || int.Parse(dataRow["unit_cnt"].ToString()) <= 0) ? num4.ToString() : ((!string.Concat(dataRow["sale_type"]).Equals("1")) ? string.Format("<span>￥{0}</span>×<span>{1}</span>&nbsp;<br>{2}&nbsp;", num4, dataRow["unit_cnt"].ToString(), string.Format("{0:F0}", dataRow["amount"])) : ("-" + Math.Round(Convert.ToDecimal(dataRow["amount"].ToString()), 0).ToString()));
						dictionary3.Add("amounttext", value3);
					}
					else
					{
						dictionary3.Add("amounttext", Math.Round(Convert.ToDecimal(dataRow["amount"].ToString()), 0).ToString());
					}
					if (string.Concat(dataRow["sale_type"]).Equals("1"))
					{
						if (val2.get_u_name().Equals(value2))
						{
							dictionary3.Add("amount", "-" + Math.Round(Convert.ToDecimal(dataRow["amount"].ToString()), 0).ToString());
						}
						else if (string.Concat(dataRow["sale_type"]).Equals("1"))
						{
							dictionary3.Add("amount", "-" + Math.Round(Convert.ToDecimal(dataRow["amount"].ToString()), 0).ToString());
						}
						else
						{
							dictionary3.Add("amount", Math.Round(Convert.ToDecimal(dataRow["amount"].ToString()), 0).ToString());
						}
					}
					else
					{
						dictionary3.Add("amount", Math.Round(Convert.ToDecimal(dataRow["amount"].ToString()), 0).ToString());
					}
					dictionary3.Add("szamount", dataRow["ds"].ToString());
					dictionary3.Add("isdelete", dataRow["isDelete"].ToString());
					dataRow["play_id"].ToString();
					string value4 = dataRow["odds_id"].ToString();
					dictionary3.Add("week", Utils.GetWeekByDate(Convert.ToDateTime(dataRow["bet_time"].ToString())));
					dictionary3.Add("ordernum", dataRow["order_num"].ToString());
					dictionary3.Add("phase", dataRow["phase"].ToString());
					dictionary3.Add("bettime", dataRow["bet_time"].ToString());
					dictionary3.Add("lottery_type", dataRow["lottery_type"].ToString());
					dictionary3.Add("lottery_name", this.GetGameNameByID(dataRow["lottery_type"].ToString()));
					dictionary3.Add("islm", dataRow["isLM"].ToString());
					dictionary3.Add("lmtype", dataRow["lm_type"].ToString());
					dictionary3.Add("hyname", IsHideUser_kc(dataRow["u_name"].ToString()));
					dictionary3.Add("dlname", IsHideUser_kc(string.Concat(dataRow["dl_name"])));
					dictionary3.Add("zdname", IsHideUser_kc(string.Concat(dataRow["zd_name"])));
					dictionary3.Add("gdname", IsHideUser_kc(string.Concat(dataRow["gd_name"])));
					dictionary3.Add("fgsname", IsHideUser_kc(string.Concat(dataRow["fgs_name"])));
					dictionary3.Add("hydrawback", string.Concat(dataRow["hy_drawback"]));
					dictionary3.Add("dldrawback", string.Concat(dataRow["dl_drawback"]));
					dictionary3.Add("zddrawback", string.Concat(dataRow["zd_drawback"]));
					dictionary3.Add("gddrawback", string.Concat(dataRow["gd_drawback"]));
					dictionary3.Add("fgsdrawback", string.Concat(dataRow["fgs_drawback"]));
					dictionary3.Add("zjdrawback", string.Concat(dataRow["zj_drawback"]));
					dictionary3.Add("dlrate", string.Concat(dataRow["dl_rate"]));
					dictionary3.Add("zdrate", string.Concat(dataRow["zd_rate"]));
					dictionary3.Add("gdrate", string.Concat(dataRow["gd_rate"]));
					dictionary3.Add("fgsrate", string.Concat(dataRow["fgs_rate"]));
					dictionary3.Add("zjrate", string.Concat(dataRow["zj_rate"]));
					dictionary3.Add("unitcnt", string.Concat(dataRow["unit_cnt"]));
					dictionary3.Add("pk", dataRow["kind"].ToString());
					string value5 = "";
					if (dataRow["lottery_type"].ToString().Equals(8.ToString()))
					{
						value5 = string.Format(" #[第{0}桌 {1}] ", dataRow["table_type"].ToString(), Utils.GetPKBJLPlaytypeColorTxt(dataRow["play_type"].ToString()));
					}
					dictionary3.Add("tabletype", value5);
					string arg = "";
					if (!string.IsNullOrEmpty(dataRow["unit_cnt"].ToString()) && int.Parse(dataRow["unit_cnt"].ToString()) > 1)
					{
						arg = GroupShowHrefString(2, dataRow["order_num"].ToString(), dataRow["is_payment"].ToString(), "1", "1");
					}
					if (dataRow["fgs_name"].ToString().Equals(text))
					{
						dictionary3.Add("saletype", "2");
					}
					else if (string.Concat(dataRow["sale_type"]).Equals("1"))
					{
						dictionary3.Add("saletype", "1");
					}
					else
					{
						dictionary3.Add("saletype", "0");
					}
					if ("329".Equals(value4) || "330".Equals(value4) || "331".Equals(value4) || "1181".Equals(value4) || "1200".Equals(value4) || "1201".Equals(value4) || "1202".Equals(value4) || "1203".Equals(value4) || "330".Equals(value4) || "331".Equals(value4) || "1181".Equals(value4) || "1202".Equals(value4) || "1203".Equals(value4) || "72055".Equals(value4) || "329".Equals(value4) || "330".Equals(value4) || "331".Equals(value4) || "1181".Equals(value4) || "1200".Equals(value4) || "1201".Equals(value4) || "1202".Equals(value4) || "1203".Equals(value4))
					{
						string text6 = dataRow["lm_type"].ToString();
						dictionary3.Add("playname", dataRow["play_name"].ToString());
						string value6 = val2.get_u_type().Equals("zj") ? dataRow["odds_zj"].ToString() : dataRow["odds"].ToString();
						dictionary3.Add("odds", value6);
						if (text6.Equals("0"))
						{
							string str = "";
							str += string.Format("<br />【{0}組】 {1}", dataRow["unit_cnt"].ToString(), arg);
							str += string.Format("<br />{0}", dataRow["bet_val"].ToString());
							dictionary3.Add("betval", str);
						}
					}
					else
					{
						dictionary3.Add("playname", dataRow["play_name"].ToString() + "【" + dataRow["bet_val"].ToString() + "】");
						dictionary3.Add("odds", val2.get_u_type().Equals("zj") ? dataRow["odds_zj"].ToString() : dataRow["odds"].ToString());
						dictionary3.Add("betval", dataRow["bet_val"].ToString());
					}
					dictionary2.Add(num3.ToString(), dictionary3);
				}
				dictionary.Add("newbetlist", dictionary2);
			}
			new Dictionary<string, object>();
			dictionary.Add("maxidvalid", num2);
			DateTime dt = DateTime.Now;
			List<object> autoJPForTable = GetAutoJPForTable(text5, compareTime, ref dt);
			Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
			if (autoJPForTable != null)
			{
				dictionary4.Add("tipsList", autoJPForTable);
			}
			else
			{
				dictionary4.Add("tipsList", new List<object>());
			}
			dictionary4.Add("timestamp", Utils.DateTimeToStamp(dt));
			dictionary.Add("autoJP", dictionary4);
			val.set_data(dictionary);
			strResult = JsonHandle.ObjectToJson(val);
		}

		
		public void IsOpenLottery(HttpContext context, ref string strResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			ReturnResult val = new ReturnResult();
			val.set_success(200);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (context.Session["user_name"] == null)
			{
				val.set_success(300);
				dictionary.Add("type", "isopenlottery");
				val.set_data(dictionary);
				strResult = JsonHandle.ObjectToJson(val);
				context.Response.End();
				return;
			}
			string value = LSRequest.qq("lid");
			int num = Convert.ToInt32(value);
			DataTable dataTable = null;
			switch (num)
			{
			case 0:
				dataTable = CallBLL.cz_phase_kl10_bll.IsPhaseClose();
				break;
//			case 1:
//				dataTable = CallBLL.cz_phase_cqsc_bll.IsPhaseClose();
//				break;
//			case 2:
//				dataTable = CallBLL.cz_phase_pk10_bll.IsPhaseClose();
//				break;
//			case 3:
//				dataTable = CallBLL.cz_phase_xync_bll.IsPhaseClose();
//				break;
//			case 4:
//				dataTable = CallBLL.cz_phase_jsk3_bll.IsPhaseClose();
//				break;
//			case 5:
//				dataTable = CallBLL.cz_phase_kl8_bll.IsPhaseClose();
//				break;
//			case 6:
//				dataTable = CallBLL.cz_phase_k8sc_bll.IsPhaseClose();
//				break;
//			case 7:
//				dataTable = CallBLL.cz_phase_pcdd_bll.IsPhaseClose();
//				break;
//			case 9:
//				dataTable = CallBLL.cz_phase_xyft5_bll.IsPhaseClose();
//				break;
//			case 8:
//				dataTable = CallBLL.cz_phase_pkbjl_bll.IsPhaseClose();
//				break;
//			case 10:
//				dataTable = CallBLL.cz_phase_jscar_bll.IsPhaseClose();
//				break;
//			case 11:
//				dataTable = CallBLL.cz_phase_speed5_bll.IsPhaseClose();
//				break;
//			case 13:
//				dataTable = CallBLL.cz_phase_jscqsc_bll.IsPhaseClose();
//				break;
//			case 12:
//				dataTable = CallBLL.cz_phase_jspk10_bll.IsPhaseClose();
//				break;
//			case 14:
//				dataTable = CallBLL.cz_phase_jssfc_bll.IsPhaseClose();
//				break;
//			case 15:
//				dataTable = CallBLL.cz_phase_jsft2_bll.IsPhaseClose();
//				break;
//			case 16:
//				dataTable = CallBLL.cz_phase_car168_bll.IsPhaseClose();
//				break;
//			case 17:
//				dataTable = CallBLL.cz_phase_ssc168_bll.IsPhaseClose();
//				break;
//			case 18:
//				dataTable = CallBLL.cz_phase_vrcar_bll.IsPhaseClose();
//				break;
//			case 19:
//				dataTable = CallBLL.cz_phase_vrssc_bll.IsPhaseClose();
//				break;
//			case 20:
//				dataTable = CallBLL.cz_phase_xyftoa_bll.IsPhaseClose();
//				break;
//			case 21:
//				dataTable = CallBLL.cz_phase_xyftsg_bll.IsPhaseClose();
//				break;
//			case 22:
//				dataTable = CallBLL.cz_phase_happycar_bll.IsPhaseClose();
//				break;
			}
			if (dataTable.Rows[0]["isopen"].ToString().Equals("0"))
			{
				dictionary.Add("type", "isopenlottery");
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2.Add("isopen", "-1");
				dictionary.Add("isopenvalue", dictionary2);
				val.set_data(dictionary);
				val.set_success(200);
				strResult = JsonHandle.ObjectToJson(val);
			}
			else
			{
				dictionary.Add("type", "isopenlottery");
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2.Add("isopen", "1");
				dictionary.Add("isopenvalue", dictionary2);
				val.set_data(dictionary);
				val.set_success(200);
				strResult = JsonHandle.ObjectToJson(val);
			}
		}

		public void GetPlayByLottery(HttpContext context, ref string strResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			ReturnResult val = new ReturnResult();
			val.set_success(200);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (context.Session["user_name"] == null)
			{
				val.set_success(300);
				dictionary.Add("type", "get_playbylottery");
				val.set_data(dictionary);
				strResult = JsonHandle.ObjectToJson(val);
				context.Response.End();
				return;
			}
			if (!context.Session["user_type"].ToString().Equals("zj"))
			{
				val.set_success(400);
				dictionary.Add("type", "get_playbylottery");
				val.set_data(dictionary);
				val.set_tipinfo(PageBase.GetMessageByCache("u100014", "MessageHint"));
				strResult = JsonHandle.ObjectToJson(val);
				context.Response.End();
				return;
			}
			string value = LSRequest.qq("lid");
			int num = Convert.ToInt32(value);
			DataTable dataTable = null;
			switch (num)
			{
			case 0:
				dataTable = CallBLL.cz_play_kl10_bll.GetPlay();
				break;
//			case 1:
//				dataTable = CallBLL.cz_play_cqsc_bll.GetPlay();
//				break;
//			case 2:
//				dataTable = CallBLL.cz_play_pk10_bll.GetPlay();
//				break;
//			case 3:
//				dataTable = CallBLL.cz_play_xync_bll.GetPlay();
//				break;
//			case 4:
//				dataTable = CallBLL.cz_play_jsk3_bll.GetPlay();
//				break;
//			case 5:
//				dataTable = CallBLL.cz_play_kl8_bll.GetPlay();
//				break;
//			case 100:
//				dataTable = (FileCacheHelper.get_IsShowLM_B() ? CallBLL.cz_play_six_bll.GetPlay() : CallBLL.cz_play_six_bll.GetPlay("91060,91061,91062,91063,91064,91065"));
//				break;
//			case 6:
//				dataTable = CallBLL.cz_play_k8sc_bll.GetPlay();
//				break;
//			case 7:
//				dataTable = CallBLL.cz_play_pcdd_bll.GetPlay();
//				break;
//			case 9:
//				dataTable = CallBLL.cz_play_xyft5_bll.GetPlay();
//				break;
//			case 8:
//				dataTable = CallBLL.cz_play_pkbjl_bll.GetPlay();
//				break;
//			case 10:
//				dataTable = CallBLL.cz_play_jscar_bll.GetPlay();
//				break;
//			case 11:
//				dataTable = CallBLL.cz_play_speed5_bll.GetPlay();
//				break;
//			case 13:
//				dataTable = CallBLL.cz_play_jscqsc_bll.GetPlay();
//				break;
//			case 12:
//				dataTable = CallBLL.cz_play_jspk10_bll.GetPlay();
//				break;
//			case 14:
//				dataTable = CallBLL.cz_play_jssfc_bll.GetPlay();
//				break;
//			case 15:
//				dataTable = CallBLL.cz_play_jsft2_bll.GetPlay();
//				break;
//			case 16:
//				dataTable = CallBLL.cz_play_car168_bll.GetPlay();
//				break;
//			case 17:
//				dataTable = CallBLL.cz_play_ssc168_bll.GetPlay();
//				break;
//			case 18:
//				dataTable = CallBLL.cz_play_vrcar_bll.GetPlay();
//				break;
//			case 19:
//				dataTable = CallBLL.cz_play_vrssc_bll.GetPlay();
//				break;
//			case 20:
//				dataTable = CallBLL.cz_play_xyftoa_bll.GetPlay();
//				break;
//			case 21:
//				dataTable = CallBLL.cz_play_xyftsg_bll.GetPlay();
//				break;
//			case 22:
//				dataTable = CallBLL.cz_play_happycar_bll.GetPlay();
//				break;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (dataTable == null)
			{
				return;
			}
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				if (i == 0)
				{
					stringBuilder.Append(dataTable.Rows[i]["play_id"].ToString().Trim() + "," + dataTable.Rows[i]["play_name"].ToString().Trim());
				}
				else
				{
					stringBuilder.Append("|" + dataTable.Rows[i]["play_id"].ToString().Trim() + "," + dataTable.Rows[i]["play_name"].ToString().Trim());
				}
			}
			dictionary.Add("type", "get_playbylottery");
			dictionary.Add("playoption", stringBuilder.ToString());
			val.set_data(dictionary);
			strResult = JsonHandle.ObjectToJson(val);
		}

		public void GetPhaseByLottery(HttpContext context, ref string strResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			ReturnResult val = new ReturnResult();
			val.set_success(200);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (context.Session["user_name"] == null)
			{
				val.set_success(300);
				dictionary.Add("type", "get_phasebylottery");
				val.set_data(dictionary);
				strResult = JsonHandle.ObjectToJson(val);
				context.Response.End();
				return;
			}
			string value = LSRequest.qq("lid");
			int num = Convert.ToInt32(value);
			string text = DateTime.Now.AddHours(-int.Parse(this.get_BetTime_KC())).ToString("yyyy-MM-dd");
			DataTable dataTable = null;
			DataTable dataTable2 = null;
			switch (num)
			{
			case 0:
				dataTable = CallBLL.cz_phase_kl10_bll.GetPhaseByQueryDate(text);
				dataTable2 = CallBLL.cz_play_kl10_bll.GetPlay();
				break;
//			case 1:
//				dataTable = CallBLL.cz_phase_cqsc_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_cqsc_bll.GetPlay();
//				break;
//			case 2:
//				dataTable = CallBLL.cz_phase_pk10_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_pk10_bll.GetPlay();
//				break;
//			case 3:
//				dataTable = CallBLL.cz_phase_xync_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_xync_bll.GetPlay();
//				break;
//			case 4:
//				dataTable = CallBLL.cz_phase_jsk3_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_jsk3_bll.GetPlay();
//				break;
//			case 5:
//				dataTable = CallBLL.cz_phase_kl8_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_kl8_bll.GetPlay();
//				break;
//			case 100:
//				dataTable = CallBLL.cz_phase_six_bll.GetCurrentPhase("20");
//				dataTable2 = (FileCacheHelper.get_IsShowLM_B() ? CallBLL.cz_play_six_bll.GetPlay() : CallBLL.cz_play_six_bll.GetPlay("91060,91061,91062,91063,91064,91065"));
//				break;
//			case 6:
//				dataTable = CallBLL.cz_phase_k8sc_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_k8sc_bll.GetPlay();
//				break;
//			case 7:
//				dataTable = CallBLL.cz_phase_pcdd_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_pcdd_bll.GetPlay();
//				break;
//			case 9:
//				dataTable = CallBLL.cz_phase_xyft5_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_xyft5_bll.GetPlay();
//				break;
//			case 8:
//				dataTable = CallBLL.cz_phase_pkbjl_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_pkbjl_bll.GetPlay();
//				break;
//			case 10:
//				dataTable = CallBLL.cz_phase_jscar_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_jscar_bll.GetPlay();
//				break;
//			case 11:
//				dataTable = CallBLL.cz_phase_speed5_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_speed5_bll.GetPlay();
//				break;
//			case 13:
//				dataTable = CallBLL.cz_phase_jscqsc_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_jscqsc_bll.GetPlay();
//				break;
//			case 12:
//				dataTable = CallBLL.cz_phase_jspk10_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_jspk10_bll.GetPlay();
//				break;
//			case 14:
//				dataTable = CallBLL.cz_phase_jssfc_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_jssfc_bll.GetPlay();
//				break;
//			case 15:
//				dataTable = CallBLL.cz_phase_jsft2_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_jsft2_bll.GetPlay();
//				break;
//			case 16:
//				dataTable = CallBLL.cz_phase_car168_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_car168_bll.GetPlay();
//				break;
//			case 17:
//				dataTable = CallBLL.cz_phase_ssc168_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_ssc168_bll.GetPlay();
//				break;
//			case 18:
//				dataTable = CallBLL.cz_phase_vrcar_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_vrcar_bll.GetPlay();
//				break;
//			case 19:
//				dataTable = CallBLL.cz_phase_vrssc_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_vrssc_bll.GetPlay();
//				break;
//			case 20:
//				dataTable = CallBLL.cz_phase_xyftoa_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_xyftoa_bll.GetPlay();
//				break;
//			case 21:
//				dataTable = CallBLL.cz_phase_xyftsg_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_xyftsg_bll.GetPlay();
//				break;
//			case 22:
//				dataTable = CallBLL.cz_phase_happycar_bll.GetPhaseByQueryDate(text);
//				dataTable2 = CallBLL.cz_play_happycar_bll.GetPlay();
//				break;
			}
			dictionary.Add("type", "get_phasebylottery");
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (dataTable != null)
			{
				if (num.Equals(8))
				{
					Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						string value2 = dataTable.Rows[i]["p_id"].ToString();
						string key = dataTable.Rows[i]["phase"].ToString();
						if (!dictionary2.ContainsKey(key))
						{
							dictionary2.Add(key, value2);
						}
					}
					for (int i = 0; i < dictionary2.Count; i++)
					{
						if (i == 0)
						{
							stringBuilder.Append(dictionary2.ElementAt(i).Value + "," + dictionary2.ElementAt(i).Key + " 期");
						}
						else
						{
							stringBuilder.Append("|" + dictionary2.ElementAt(i).Value + "," + dictionary2.ElementAt(i).Key + " 期");
						}
					}
				}
				else
				{
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						if (i == 0)
						{
							stringBuilder.Append(dataTable.Rows[i]["p_id"].ToString().Trim() + "," + dataTable.Rows[i]["phase"].ToString().Trim() + " 期");
						}
						else
						{
							stringBuilder.Append("|" + dataTable.Rows[i]["p_id"].ToString().Trim() + "," + dataTable.Rows[i]["phase"].ToString().Trim() + " 期");
						}
					}
				}
				dictionary.Add("phaseoption", stringBuilder.ToString());
			}
			if (dataTable2 != null)
			{
				for (int i = 0; i < dataTable2.Rows.Count; i++)
				{
					if (i == 0)
					{
						stringBuilder2.Append(dataTable2.Rows[i]["play_id"].ToString().Trim() + "," + dataTable2.Rows[i]["play_name"].ToString().Trim());
					}
					else
					{
						stringBuilder2.Append("|" + dataTable2.Rows[i]["play_id"].ToString().Trim() + "," + dataTable2.Rows[i]["play_name"].ToString().Trim());
					}
				}
				dictionary.Add("playoption", stringBuilder2.ToString());
			}
			if (num.Equals(8))
			{
				dictionary.Add("tabletype", $"p1,第1桌|p2,第2桌|p3,第3桌|p4,第4桌|p5,第5桌");
				dictionary.Add("playtype", $"1,一般|0,免傭");
			}
			val.set_data(dictionary);
			strResult = JsonHandle.ObjectToJson(val);
		}

		
		public void set_skin(HttpContext context, ref string strResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			ReturnResult val = new ReturnResult();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("type", "set_skin");
			string text = LSRequest.qq("skin");
			if (context.Session["user_name"] == null)
			{
				context.Response.End();
				return;
			}
			new agent_userinfo_session();
			string str = context.Session["user_name"].ToString();
			agent_userinfo_session val2 = context.Session[str + "lottery_session_user_info"] as agent_userinfo_session;
			string text2 = val2.get_u_id().ToString();
			int num;
			if (context.Session["child_user_name"] != null)
			{
				text2 = val2.get_users_child_session().get_u_id().ToString();
				num = CallBLL.cz_users_child_bll.UpdateSkin(text2, text);
			}
			else
			{
				num = CallBLL.cz_users_bll.UpdateSkin(text2, text);
			}
			if (num > 0)
			{
				val.set_success(200);
				val2.set_u_skin(text);
				context.Session[str + "lottery_session_user_info"] = val2;
			}
			else
			{
				val.set_success(400);
				val.set_tipinfo(PageBase.GetMessageByCache("u100059", "MessageHint"));
			}
			val.set_data(dictionary);
			strResult = JsonHandle.ObjectToJson(val);
		}

		public void get_six_date(HttpContext context, ref string strResult)
		{
			string six_schedule = this.get_six_schedule();
			HttpContext.Current.Response.AddHeader("content-type", "text/xml");
			if (six_schedule == "")
			{
				HttpContext.Current.Response.Write("<script>alert('接口获取数据失败!');</script>");
			}
			else
			{
				context.Response.Write(six_schedule);
			}
			context.Response.End();
		}

		public void get_user_rate(HttpContext context, ref string strResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			ReturnResult val = new ReturnResult();
			val.set_success(400);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("type", "get_user_rate");
			if (context.Session["user_name"] == null)
			{
				context.Response.End();
				return;
			}
			string text = LSRequest.qq("uid");
			string str = context.Session["user_name"].ToString();
			agent_userinfo_session val2 = context.Session[str + "lottery_session_user_info"] as agent_userinfo_session;
			cz_users userInfoByUID = CallBLL.CzUsersService.GetUserInfoByUID(text);
			if (!IsUpperLowerLevels(userInfoByUID.get_u_name(), val2.get_u_type(), val2.get_u_name()))
			{
				context.Response.Write(PageBase.GetMessageByCache("u100014", "MessageHint"));
				context.Response.End();
			}
			DataTable lotteryList = GetLotteryList();
			DataRow[] array = lotteryList.Select($" master_id={1} ");
			DataRow[] array2 = lotteryList.Select($" master_id={2} ");
			Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
			if (array.Length > 0)
			{
				DataTable rateByUserID = CallBLL.CzRateSixService.GetRateByUserID(text);
				rateByUserID.Rows[0]["u_type"].ToString();
				string text2 = rateByUserID.Rows[0]["su_type"].ToString();
				string text3 = "";
				string arg = "";
				string text4 = "";
				string arg2 = "";
				string text5 = "";
				string arg3 = "";
				string text6 = val2.get_zjname();
				string arg4 = string.Concat(rateByUserID.Rows[0]["zj_rate"]);
				string text7 = string.Concat(rateByUserID.Rows[0]["fgs_name"]);
				string arg5 = string.Concat(rateByUserID.Rows[0]["fgs_rate"]);
				if (text2.Equals("dl"))
				{
					text3 = string.Concat(rateByUserID.Rows[0]["gd_name"]);
					arg = string.Concat(rateByUserID.Rows[0]["gd_rate"]);
					text4 = string.Concat(rateByUserID.Rows[0]["zd_name"]);
					arg2 = string.Concat(rateByUserID.Rows[0]["zd_rate"]);
					text5 = string.Concat(rateByUserID.Rows[0]["dl_name"]);
					arg3 = string.Concat(rateByUserID.Rows[0]["dl_rate"]);
				}
				if (text2.Equals("zd"))
				{
					text3 = string.Concat(rateByUserID.Rows[0]["gd_name"]);
					arg = string.Concat(rateByUserID.Rows[0]["gd_rate"]);
					text4 = string.Concat(rateByUserID.Rows[0]["zd_name"]);
					arg2 = string.Concat(rateByUserID.Rows[0]["zd_rate"]);
				}
				if (text2.Equals("gd"))
				{
					text3 = string.Concat(rateByUserID.Rows[0]["gd_name"]);
					arg = string.Concat(rateByUserID.Rows[0]["gd_rate"]);
				}
				Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
				switch (val2.get_u_type())
				{
				case "zj":
					if (!string.IsNullOrEmpty(text6))
					{
						if (val2.get_users_child_session() != null)
						{
							text6 = "-";
						}
						dictionary3.Add("zj", $"{text6},{arg4}");
					}
					if (!string.IsNullOrEmpty(text7))
					{
						dictionary3.Add("fgs", $"{text7},{arg5}");
					}
					if (!string.IsNullOrEmpty(text3))
					{
						dictionary3.Add("gd", $"{text3},{arg}");
					}
					if (!string.IsNullOrEmpty(text4))
					{
						dictionary3.Add("zd", $"{text4},{arg2}");
					}
					if (!string.IsNullOrEmpty(text5))
					{
						dictionary3.Add("dl", $"{text5},{arg3}");
					}
					break;
				case "fgs":
					if (!string.IsNullOrEmpty(text7))
					{
						if (val2.get_users_child_session() != null)
						{
							text7 = "-";
						}
						dictionary3.Add("fgs", $"{text7},{arg5}");
					}
					if (!string.IsNullOrEmpty(text3))
					{
						dictionary3.Add("gd", $"{text3},{arg}");
					}
					if (!string.IsNullOrEmpty(text4))
					{
						dictionary3.Add("zd", $"{text4},{arg2}");
					}
					if (!string.IsNullOrEmpty(text5))
					{
						dictionary3.Add("dl", $"{text5},{arg3}");
					}
					break;
				case "gd":
					if (!string.IsNullOrEmpty(text3))
					{
						if (val2.get_users_child_session() != null)
						{
							text3 = "-";
						}
						dictionary3.Add("gd", $"{text3},{arg}");
					}
					if (!string.IsNullOrEmpty(text4))
					{
						dictionary3.Add("zd", $"{text4},{arg2}");
					}
					if (!string.IsNullOrEmpty(text5))
					{
						dictionary3.Add("dl", $"{text5},{arg3}");
					}
					break;
				case "zd":
					if (!string.IsNullOrEmpty(text4))
					{
						if (val2.get_users_child_session() != null)
						{
							text4 = "-";
						}
						dictionary3.Add("zd", $"{text4},{arg2}");
					}
					if (!string.IsNullOrEmpty(text5))
					{
						dictionary3.Add("dl", $"{text5},{arg3}");
					}
					break;
				case "dl":
					if (!string.IsNullOrEmpty(text5))
					{
						if (val2.get_users_child_session() != null)
						{
							text5 = "-";
						}
						dictionary3.Add("dl", $"{text5},{arg3}");
					}
					break;
				}
				dictionary2.Add("six", dictionary3);
			}
			if (array2.Length > 0)
			{
				DataTable rateByUserID = CallBLL.cz_rate_kc_bll.GetRateByUserID(text);
				if (rateByUserID == null)
				{
					return;
				}
				rateByUserID.Rows[0]["u_type"].ToString();
				string text2 = rateByUserID.Rows[0]["su_type"].ToString();
				string text3 = "";
				string arg = "";
				string text4 = "";
				string arg2 = "";
				string text5 = "";
				string arg3 = "";
				string text6 = val2.get_zjname();
				string arg4 = string.Concat(rateByUserID.Rows[0]["zj_rate"]);
				string text7 = string.Concat(rateByUserID.Rows[0]["fgs_name"]);
				string arg5 = string.Concat(rateByUserID.Rows[0]["fgs_rate"]);
				if (text2.Equals("dl"))
				{
					text3 = string.Concat(rateByUserID.Rows[0]["gd_name"]);
					arg = string.Concat(rateByUserID.Rows[0]["gd_rate"]);
					text4 = string.Concat(rateByUserID.Rows[0]["zd_name"]);
					arg2 = string.Concat(rateByUserID.Rows[0]["zd_rate"]);
					text5 = string.Concat(rateByUserID.Rows[0]["dl_name"]);
					arg3 = string.Concat(rateByUserID.Rows[0]["dl_rate"]);
				}
				if (text2.Equals("zd"))
				{
					text3 = string.Concat(rateByUserID.Rows[0]["gd_name"]);
					arg = string.Concat(rateByUserID.Rows[0]["gd_rate"]);
					text4 = string.Concat(rateByUserID.Rows[0]["zd_name"]);
					arg2 = string.Concat(rateByUserID.Rows[0]["zd_rate"]);
				}
				if (text2.Equals("gd"))
				{
					text3 = string.Concat(rateByUserID.Rows[0]["gd_name"]);
					arg = string.Concat(rateByUserID.Rows[0]["gd_rate"]);
				}
				Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
				switch (val2.get_u_type())
				{
				case "zj":
					if (!string.IsNullOrEmpty(text6))
					{
						if (val2.get_users_child_session() != null)
						{
							text6 = "-";
						}
						dictionary3.Add("zj", $"{text6},{arg4}");
					}
					if (!string.IsNullOrEmpty(text7))
					{
						dictionary3.Add("fgs", $"{text7},{arg5}");
					}
					if (!string.IsNullOrEmpty(text3))
					{
						dictionary3.Add("gd", $"{text3},{arg}");
					}
					if (!string.IsNullOrEmpty(text4))
					{
						dictionary3.Add("zd", $"{text4},{arg2}");
					}
					if (!string.IsNullOrEmpty(text5))
					{
						dictionary3.Add("dl", $"{text5},{arg3}");
					}
					break;
				case "fgs":
					if (!string.IsNullOrEmpty(text7))
					{
						if (val2.get_users_child_session() != null)
						{
							text7 = "-";
						}
						dictionary3.Add("fgs", $"{text7},{arg5}");
					}
					if (!string.IsNullOrEmpty(text3))
					{
						dictionary3.Add("gd", $"{text3},{arg}");
					}
					if (!string.IsNullOrEmpty(text4))
					{
						dictionary3.Add("zd", $"{text4},{arg2}");
					}
					if (!string.IsNullOrEmpty(text5))
					{
						dictionary3.Add("dl", $"{text5},{arg3}");
					}
					break;
				case "gd":
					if (!string.IsNullOrEmpty(text3))
					{
						if (val2.get_users_child_session() != null)
						{
							text3 = "-";
						}
						dictionary3.Add("gd", $"{text3},{arg}");
					}
					if (!string.IsNullOrEmpty(text4))
					{
						dictionary3.Add("zd", $"{text4},{arg2}");
					}
					if (!string.IsNullOrEmpty(text5))
					{
						dictionary3.Add("dl", $"{text5},{arg3}");
					}
					break;
				case "zd":
					if (!string.IsNullOrEmpty(text4))
					{
						if (val2.get_users_child_session() != null)
						{
							text4 = "-";
						}
						dictionary3.Add("zd", $"{text4},{arg2}");
					}
					if (!string.IsNullOrEmpty(text5))
					{
						dictionary3.Add("dl", $"{text5},{arg3}");
					}
					break;
				case "dl":
					if (!string.IsNullOrEmpty(text5))
					{
						if (val2.get_users_child_session() != null)
						{
							text5 = "-";
						}
						dictionary3.Add("dl", $"{text5},{arg3}");
					}
					break;
				}
				dictionary2.Add("kc", dictionary3);
			}
			dictionary.Add("tips", dictionary2);
			val.set_success(200);
			val.set_data(dictionary);
			strResult = JsonHandle.ObjectToJson(val);
		}

		public void get_currentphase(HttpContext context, ref string strResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			ReturnResult val = new ReturnResult();
			val.set_success(400);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("type", "get_currentphase");
			if (context.Session["user_name"] == null)
			{
				context.Response.End();
				return;
			}
			string s = LSRequest.qq("lid");
			string text = LSRequest.qq("phase");
			string text2 = LSRequest.qq("tabletype");
			new Dictionary<string, string>();
			switch (int.Parse(s))
			{
//			case 100:
//			{
//				List<object> list = new List<object>();
//				DataSet currentByPhase = CallBLL.cz_phase_six_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					List<string> list2 = new List<string>();
//					DataTable dataTable = currentByPhase.Tables[0];
//					int num11 = 6;
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row in dataTable.Rows)
//						{
//							string text8 = "";
//							int num12 = 0;
//							string value = row["phase"].ToString();
//							string value3 = row["sn_stop_date"].ToString();
//							for (int i = 1; i <= num11; i++)
//							{
//								string columnName = "n" + i;
//								list2.Add(row[columnName].ToString());
//								num12 += Convert.ToInt32(row[columnName]);
//								text8 = ((!i.Equals(1)) ? (text8 + "," + row[columnName].ToString()) : (text8 + row[columnName].ToString()));
//							}
//							list2.Add(row["sn"].ToString());
//							num12 += Convert.ToInt32(row["sn"].ToString());
//							string item4 = row["sn"].ToString();
//							string item5 = (double.Parse(row["sn"].ToString().Trim()) != 49.0) ? ((double.Parse(row["sn"].ToString().Trim()) % 2.0 != 0.0) ? this.SetWordColor("單") : this.SetWordColor("雙")) : this.SetWordColor("和");
//							string item6 = (!(double.Parse(row["sn"].ToString().Trim()) <= 24.0)) ? ((double.Parse(row["sn"].ToString().Trim()) != 49.0) ? this.SetWordColor("大") : this.SetWordColor("和")) : this.SetWordColor("小");
//							int num13 = int.Parse(row["sn"].ToString().Trim()) % 10;
//							string item7 = (double.Parse(row["sn"].ToString().Trim()) != 49.0) ? ((num13 > 4) ? this.SetWordColor("大") : this.SetWordColor("小")) : this.SetWordColor("和");
//							string item8 = get_tz(row["sn"].ToString().Trim());
//							int num14 = int.Parse(row["sn"].ToString().Trim()) / 10;
//							int num15 = int.Parse(row["sn"].ToString().Trim()) % 10;
//							int num16 = num14 + num15;
//							string item9 = (int.Parse(row["sn"].ToString().Trim()) != 49) ? ((num16 % 2 != 0) ? this.SetWordColor("單") : this.SetWordColor("雙")) : this.SetWordColor("和");
//							string wordColor9 = (Convert.ToInt32(num12) % 2 != 0) ? this.SetWordColor("單") : this.SetWordColor("雙");
//							string wordColor8 = (Convert.ToInt32(num12) > 174) ? this.SetWordColor("大") : this.SetWordColor("小");
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", Convert.ToDateTime(value3).ToString("yyyy/MM/dd"));
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text8,
//								item4,
//								item5,
//								item6,
//								item7,
//								item8,
//								item9,
//								num12.ToString(),
//								wordColor9,
//								wordColor8
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
			case 0:
			{
				List<object> list = new List<object>();
				List<string> list2 = new List<string>();
				DataSet currentByPhase = CallBLL.cz_phase_kl10_bll.GetCurrentByPhase(text);
				if (currentByPhase != null)
				{
					DataTable dataTable = currentByPhase.Tables[0];
					int num11 = 8;
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
					if (text3.Equals("1") && text4.Equals("1"))
					{
						foreach (DataRow row2 in dataTable.Rows)
						{
							string value = row2["phase"].ToString();
							string value2 = row2["play_open_date"].ToString();
							for (int i = 1; i <= num11; i++)
							{
								string columnName = "n" + i;
								list2.Add(row2[columnName].ToString());
							}
//							string text7 = KL10Phase.get_zh(list2).ToString();
//							string wordColor8 = KL10Phase.get_cl_zhdx(text7);
//							wordColor8 = this.SetWordColor(wordColor8);
//							string wordColor9 = KL10Phase.get_cl_zhds(text7);
//							wordColor9 = this.SetWordColor(wordColor9);
//							string wordColor11 = KL10Phase.get_cl_zhwsdx(text7);
//							wordColor11 = this.SetWordColor(wordColor11);
//							string wordColor12 = KL10Phase.get_cl_lh(list2[0].ToString(), list2[num11 - 1].ToString());
//							wordColor12 = this.SetWordColor(wordColor12);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text7,
//								wordColor8,
//								wordColor9,
//								wordColor11,
//								wordColor12
//							});
							list.Add(new Dictionary<string, object>(dictionary2));
							dictionary2.Clear();
							list2.Clear();
						}
					}
				}
				dictionary.Add("jqkj", list);
				break;
			}
//			case 1:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_cqsc_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row3 in dataTable.Rows)
//						{
//							string value = row3["phase"].ToString();
//							string value2 = row3["play_open_date"].ToString();
//							int num = Convert.ToInt32(row3["n1"].ToString());
//							int num2 = Convert.ToInt32(row3["n2"].ToString());
//							int num3 = Convert.ToInt32(row3["n3"].ToString());
//							int num4 = Convert.ToInt32(row3["n4"].ToString());
//							int num5 = Convert.ToInt32(row3["n5"].ToString());
//							list2.Add(num.ToString());
//							list2.Add(num2.ToString());
//							list2.Add(num3.ToString());
//							list2.Add(num4.ToString());
//							list2.Add(num5.ToString());
//							string text6 = (num + num2 + num3 + num4 + num5).ToString();
//							string wordColor8 = CQSCPhase.get_cl_zhdx(text6);
//							wordColor8 = this.SetWordColor(wordColor8);
//							string wordColor9 = CQSCPhase.get_cl_zhds(text6);
//							wordColor9 = this.SetWordColor(wordColor9);
//							string wordColor10 = CQSCPhase.get_cl_lh(num.ToString(), num5.ToString());
//							wordColor10 = this.SetWordColor(wordColor10);
//							string item = CQSCPhase.get_cqsc_str(num + "," + num2 + "," + num3);
//							string item2 = CQSCPhase.get_cqsc_str(num2 + "," + num3 + "," + num4);
//							string item3 = CQSCPhase.get_cqsc_str(num3 + "," + num4 + "," + num5);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text6,
//								wordColor8,
//								wordColor9,
//								wordColor10,
//								item,
//								item2,
//								item3
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 2:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_pk10_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row4 in dataTable.Rows)
//						{
//							string value = row4["phase"].ToString();
//							string value2 = row4["play_open_date"].ToString();
//							int num = Convert.ToInt32(row4["n1"].ToString());
//							int num2 = Convert.ToInt32(row4["n2"].ToString());
//							int num3 = Convert.ToInt32(row4["n3"].ToString());
//							int num4 = Convert.ToInt32(row4["n4"].ToString());
//							int num5 = Convert.ToInt32(row4["n5"].ToString());
//							int num6 = Convert.ToInt32(row4["n6"].ToString());
//							int num7 = Convert.ToInt32(row4["n7"].ToString());
//							int num8 = Convert.ToInt32(row4["n8"].ToString());
//							int num9 = Convert.ToInt32(row4["n9"].ToString());
//							int num10 = Convert.ToInt32(row4["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string text5 = (num + num2).ToString();
//							string wordColor = (Convert.ToInt32(text5) > 11) ? "大" : "小";
//							wordColor = this.SetWordColor(wordColor);
//							string wordColor2 = (Convert.ToInt32(text5) % 2 != 0) ? "單" : "雙";
//							wordColor2 = this.SetWordColor(wordColor2);
//							string wordColor3 = (num <= num10) ? "虎" : "龍";
//							wordColor3 = this.SetWordColor(wordColor3);
//							string wordColor4 = (num2 <= num9) ? "虎" : "龍";
//							wordColor4 = this.SetWordColor(wordColor4);
//							string wordColor5 = (num3 <= num8) ? "虎" : "龍";
//							wordColor5 = this.SetWordColor(wordColor5);
//							string wordColor6 = (num4 <= num7) ? "虎" : "龍";
//							wordColor6 = this.SetWordColor(wordColor6);
//							string wordColor7 = (num5 <= num6) ? "虎" : "龍";
//							wordColor7 = this.SetWordColor(wordColor7);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text5,
//								wordColor,
//								wordColor2,
//								wordColor3,
//								wordColor4,
//								wordColor5,
//								wordColor6,
//								wordColor7
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 3:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_xync_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					int num12 = 0;
//					DataTable dataTable = currentByPhase.Tables[0];
//					int num11 = 8;
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row5 in dataTable.Rows)
//						{
//							string value = row5["phase"].ToString();
//							string value2 = row5["play_open_date"].ToString();
//							for (int i = 1; i <= num11; i++)
//							{
//								string columnName = "n" + i;
//								list2.Add(row5[columnName].ToString());
//								num12 += Convert.ToInt32(row5[columnName]);
//							}
//							string wordColor8 = (num12 != 84) ? ((num12 > 83) ? "大" : "小") : "和";
//							wordColor8 = this.SetWordColor(wordColor8);
//							string wordColor9 = (num12 % 2 != 0) ? "單" : "雙";
//							wordColor9 = this.SetWordColor(wordColor9);
//							string wordColor11 = (num12 % 10 > 4) ? "尾大" : "尾小";
//							wordColor11 = this.SetWordColor(wordColor11);
//							string wordColor15 = (int.Parse(row5["N1"].ToString().Trim()) <= int.Parse(row5["N8"].ToString().Trim())) ? "野獸" : "家禽";
//							wordColor15 = this.SetWordColor(wordColor15);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								num12.ToString(),
//								wordColor8,
//								wordColor9,
//								wordColor11,
//								wordColor15
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 4:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_jsk3_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row6 in dataTable.Rows)
//						{
//							string value = row6["phase"].ToString();
//							string value2 = row6["play_open_date"].ToString();
//							int num = Convert.ToInt32(row6["n1"].ToString());
//							int num2 = Convert.ToInt32(row6["n2"].ToString());
//							int num3 = Convert.ToInt32(row6["n3"].ToString());
//							list2.Add(num.ToString());
//							list2.Add(num2.ToString());
//							list2.Add(num3.ToString());
//							string text6 = (num + num2 + num3).ToString();
//							string wordColor8 = (num != num2 || num2 != num3) ? ((Convert.ToInt32(text6) > 10) ? "大" : "小") : "通吃";
//							wordColor8 = this.SetWordColor(wordColor8);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text6,
//								wordColor8
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 5:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_kl8_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					int i = 0;
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row7 in dataTable.Rows)
//						{
//							string value = row7["phase"].ToString();
//							string value2 = row7["play_open_date"].ToString();
//							int num = Convert.ToInt32(row7["n1"].ToString());
//							int num2 = Convert.ToInt32(row7["n2"].ToString());
//							int num3 = Convert.ToInt32(row7["n3"].ToString());
//							int num4 = Convert.ToInt32(row7["n4"].ToString());
//							int num5 = Convert.ToInt32(row7["n5"].ToString());
//							int num6 = Convert.ToInt32(row7["n6"].ToString());
//							int num7 = Convert.ToInt32(row7["n7"].ToString());
//							int num8 = Convert.ToInt32(row7["n8"].ToString());
//							int num9 = Convert.ToInt32(row7["n9"].ToString());
//							int num10 = Convert.ToInt32(row7["n10"].ToString());
//							int num17 = Convert.ToInt32(row7["n11"].ToString());
//							int num18 = Convert.ToInt32(row7["n12"].ToString());
//							int num19 = Convert.ToInt32(row7["n13"].ToString());
//							int num20 = Convert.ToInt32(row7["n14"].ToString());
//							int num21 = Convert.ToInt32(row7["n15"].ToString());
//							int num22 = Convert.ToInt32(row7["n16"].ToString());
//							int num23 = Convert.ToInt32(row7["n17"].ToString());
//							int num24 = Convert.ToInt32(row7["n18"].ToString());
//							int num25 = Convert.ToInt32(row7["n19"].ToString());
//							int num26 = Convert.ToInt32(row7["n20"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							list2.Add((num17 < 10) ? ("0" + num17) : num17.ToString());
//							list2.Add((num18 < 10) ? ("0" + num18) : num18.ToString());
//							list2.Add((num19 < 10) ? ("0" + num19) : num19.ToString());
//							list2.Add((num20 < 10) ? ("0" + num20) : num20.ToString());
//							list2.Add((num21 < 10) ? ("0" + num21) : num21.ToString());
//							list2.Add((num22 < 10) ? ("0" + num22) : num22.ToString());
//							list2.Add((num23 < 10) ? ("0" + num23) : num23.ToString());
//							list2.Add((num24 < 10) ? ("0" + num24) : num24.ToString());
//							list2.Add((num25 < 10) ? ("0" + num25) : num25.ToString());
//							list2.Add((num26 < 10) ? ("0" + num26) : num26.ToString());
//							string text6 = (num + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9 + num10 + num17 + num18 + num19 + num20 + num21 + num22 + num23 + num24 + num25 + num26).ToString();
//							string wordColor9 = (Convert.ToInt32(text6) == 810) ? "和" : ((Convert.ToInt32(text6) % 2 != 0) ? "單" : "雙");
//							wordColor9 = this.SetWordColor(wordColor9);
//							string wordColor8 = (Convert.ToInt32(text6) > 810) ? "大" : ((Convert.ToInt32(text6) >= 810) ? "和" : "小");
//							wordColor8 = this.SetWordColor(wordColor8);
//							int num27 = 0;
//							int num28 = 0;
//							for (int j = 0; j < 20; j++)
//							{
//								if (int.Parse(dataTable.Rows[i]["n" + (j + 1)].ToString().Trim()) % 2 == 0)
//								{
//									num28++;
//								}
//								else
//								{
//									num27++;
//								}
//							}
//							string wordColor13 = KL8Phase.get_cl_dsh(num27.ToString(), num28.ToString());
//							wordColor13 = this.SetWordColor(wordColor13);
//							int num29 = 0;
//							int num30 = 0;
//							for (int j = 0; j < 20; j++)
//							{
//								if (int.Parse(dataTable.Rows[i]["n" + (j + 1)].ToString().Trim()) <= 40)
//								{
//									num29++;
//								}
//								else
//								{
//									num30++;
//								}
//							}
//							string wordColor14 = KL8Phase.get_cl_qhh(num29.ToString(), num30.ToString());
//							wordColor14 = this.SetWordColor(wordColor14);
//							string item12 = KL8Phase.get_cl_wh(text6.ToString());
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text6,
//								wordColor9,
//								wordColor8,
//								wordColor13,
//								wordColor14,
//								item12
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//							i++;
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 6:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_k8sc_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row8 in dataTable.Rows)
//						{
//							string value = row8["phase"].ToString();
//							string value2 = row8["play_open_date"].ToString();
//							int num = Convert.ToInt32(row8["n1"].ToString());
//							int num2 = Convert.ToInt32(row8["n2"].ToString());
//							int num3 = Convert.ToInt32(row8["n3"].ToString());
//							int num4 = Convert.ToInt32(row8["n4"].ToString());
//							int num5 = Convert.ToInt32(row8["n5"].ToString());
//							list2.Add(num.ToString());
//							list2.Add(num2.ToString());
//							list2.Add(num3.ToString());
//							list2.Add(num4.ToString());
//							list2.Add(num5.ToString());
//							string text6 = (num + num2 + num3 + num4 + num5).ToString();
//							string wordColor8 = K8SCPhase.get_cl_zhdx(text6);
//							wordColor8 = this.SetWordColor(wordColor8);
//							string wordColor9 = K8SCPhase.get_cl_zhds(text6);
//							wordColor9 = this.SetWordColor(wordColor9);
//							string wordColor10 = K8SCPhase.get_cl_lh(num.ToString(), num5.ToString());
//							wordColor10 = this.SetWordColor(wordColor10);
//							string item = K8SCPhase.get_k8sc_str(num + "," + num2 + "," + num3);
//							string item2 = K8SCPhase.get_k8sc_str(num2 + "," + num3 + "," + num4);
//							string item3 = K8SCPhase.get_k8sc_str(num3 + "," + num4 + "," + num5);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text6,
//								wordColor8,
//								wordColor9,
//								wordColor10,
//								item,
//								item2,
//								item3
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 7:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_pcdd_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row9 in dataTable.Rows)
//						{
//							string value = row9["phase"].ToString();
//							string value2 = row9["play_open_date"].ToString();
//							int num = Convert.ToInt32(row9["n1"].ToString());
//							int num2 = Convert.ToInt32(row9["n2"].ToString());
//							int num3 = Convert.ToInt32(row9["n3"].ToString());
//							int num31 = Convert.ToInt32(row9["sn"].ToString());
//							list2.Add(num.ToString());
//							list2.Add(num2.ToString());
//							list2.Add(num3.ToString());
//							list2.Add(num31.ToString());
//							string item6 = (num31 >= 14) ? this.SetWordColor("大") : this.SetWordColor("小");
//							string item5 = (num31 % 2 != 0) ? this.SetWordColor("單") : this.SetWordColor("雙");
//							string item13 = this.SetWordColor(get_pcdd_bs_str(num31.ToString()));
//							string item14 = (num31 > 4) ? ((num31 < 23) ? "-" : this.SetWordColor("極大")) : this.SetWordColor("極小");
//							string item15 = (num != num2 || num != num3 || num2 != num3) ? "-" : this.SetWordColor("豹子");
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								item6,
//								item5,
//								item13,
//								item14,
//								item15
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 9:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_xyft5_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row10 in dataTable.Rows)
//						{
//							string value = row10["phase"].ToString();
//							string value2 = row10["play_open_date"].ToString();
//							int num = Convert.ToInt32(row10["n1"].ToString());
//							int num2 = Convert.ToInt32(row10["n2"].ToString());
//							int num3 = Convert.ToInt32(row10["n3"].ToString());
//							int num4 = Convert.ToInt32(row10["n4"].ToString());
//							int num5 = Convert.ToInt32(row10["n5"].ToString());
//							int num6 = Convert.ToInt32(row10["n6"].ToString());
//							int num7 = Convert.ToInt32(row10["n7"].ToString());
//							int num8 = Convert.ToInt32(row10["n8"].ToString());
//							int num9 = Convert.ToInt32(row10["n9"].ToString());
//							int num10 = Convert.ToInt32(row10["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string text5 = (num + num2).ToString();
//							string wordColor = (Convert.ToInt32(text5) > 11) ? "大" : "小";
//							wordColor = this.SetWordColor(wordColor);
//							string wordColor2 = (Convert.ToInt32(text5) % 2 != 0) ? "單" : "雙";
//							wordColor2 = this.SetWordColor(wordColor2);
//							string wordColor3 = (num <= num10) ? "虎" : "龍";
//							wordColor3 = this.SetWordColor(wordColor3);
//							string wordColor4 = (num2 <= num9) ? "虎" : "龍";
//							wordColor4 = this.SetWordColor(wordColor4);
//							string wordColor5 = (num3 <= num8) ? "虎" : "龍";
//							wordColor5 = this.SetWordColor(wordColor5);
//							string wordColor6 = (num4 <= num7) ? "虎" : "龍";
//							wordColor6 = this.SetWordColor(wordColor6);
//							string wordColor7 = (num5 <= num6) ? "虎" : "龍";
//							wordColor7 = this.SetWordColor(wordColor7);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text5,
//								wordColor,
//								wordColor2,
//								wordColor3,
//								wordColor4,
//								wordColor5,
//								wordColor6,
//								wordColor7
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 8:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_pkbjl_bll.GetCurrentByPhase(text, text2);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row11 in dataTable.Rows)
//						{
//							string value = row11["phase"].ToString();
//							string value2 = row11["play_open_date"].ToString();
//							int num = Convert.ToInt32(row11["n1"].ToString());
//							int num2 = Convert.ToInt32(row11["n2"].ToString());
//							int num3 = Convert.ToInt32(row11["n3"].ToString());
//							int num4 = Convert.ToInt32(row11["n4"].ToString());
//							int num5 = Convert.ToInt32(row11["n5"].ToString());
//							int num6 = Convert.ToInt32(row11["n6"].ToString());
//							int num7 = Convert.ToInt32(row11["n7"].ToString());
//							int num8 = Convert.ToInt32(row11["n8"].ToString());
//							int num9 = Convert.ToInt32(row11["n9"].ToString());
//							int num10 = Convert.ToInt32(row11["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string value4 = row11["ten_poker"].ToString();
//							string text9 = row11["xian_nn"].ToString();
//							string text10 = row11["zhuang_nn"].ToString();
//							string pKBJLBalanceMaxMin = Utils.GetPKBJLBalanceMaxMin(text10, text9);
//							bool pKBJLBalanceIsDuizi = Utils.GetPKBJLBalanceIsDuizi(text9);
//							bool pKBJLBalanceIsDuizi2 = Utils.GetPKBJLBalanceIsDuizi(text10);
//							string item10 = "-";
//							string item11 = "-";
//							string pKBJLBalanceZXH = Utils.GetPKBJLBalanceZXH(text10, text9);
//							pKBJLBalanceMaxMin = ((!pKBJLBalanceMaxMin.Equals("min")) ? this.SetWordColor("大") : this.SetWordColor("小"));
//							if (pKBJLBalanceIsDuizi)
//							{
//								item10 = this.SetWordColor("閑對");
//							}
//							if (pKBJLBalanceIsDuizi2)
//							{
//								item11 = this.SetWordColor("莊對");
//							}
//							pKBJLBalanceZXH = ((!pKBJLBalanceZXH.Equals("zhuang")) ? ((!pKBJLBalanceZXH.Equals("xian")) ? this.SetWordColor("和") : this.SetWordColor("閑")) : this.SetWordColor("莊"));
//							string pKBJLBalanceDianshu = Utils.GetPKBJLBalanceDianshu(text9);
//							string pKBJLBalanceDianshu2 = Utils.GetPKBJLBalanceDianshu(text10);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("pokerList", value4);
//							dictionary2.Add("xian_nn", text9);
//							dictionary2.Add("zhuang_nn", text10);
//							dictionary2.Add("xian_dian", pKBJLBalanceDianshu);
//							dictionary2.Add("zhuang_dian", pKBJLBalanceDianshu2);
//							dictionary2.Add("total", new List<string>
//							{
//								pKBJLBalanceZXH,
//								pKBJLBalanceMaxMin,
//								item10,
//								item11
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 10:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_jscar_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row12 in dataTable.Rows)
//						{
//							string value = row12["phase"].ToString();
//							string value2 = row12["play_open_date"].ToString();
//							int num = Convert.ToInt32(row12["n1"].ToString());
//							int num2 = Convert.ToInt32(row12["n2"].ToString());
//							int num3 = Convert.ToInt32(row12["n3"].ToString());
//							int num4 = Convert.ToInt32(row12["n4"].ToString());
//							int num5 = Convert.ToInt32(row12["n5"].ToString());
//							int num6 = Convert.ToInt32(row12["n6"].ToString());
//							int num7 = Convert.ToInt32(row12["n7"].ToString());
//							int num8 = Convert.ToInt32(row12["n8"].ToString());
//							int num9 = Convert.ToInt32(row12["n9"].ToString());
//							int num10 = Convert.ToInt32(row12["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string text5 = (num + num2).ToString();
//							string wordColor = (Convert.ToInt32(text5) > 11) ? "大" : "小";
//							wordColor = this.SetWordColor(wordColor);
//							string wordColor2 = (Convert.ToInt32(text5) % 2 != 0) ? "單" : "雙";
//							wordColor2 = this.SetWordColor(wordColor2);
//							string wordColor3 = (num <= num10) ? "虎" : "龍";
//							wordColor3 = this.SetWordColor(wordColor3);
//							string wordColor4 = (num2 <= num9) ? "虎" : "龍";
//							wordColor4 = this.SetWordColor(wordColor4);
//							string wordColor5 = (num3 <= num8) ? "虎" : "龍";
//							wordColor5 = this.SetWordColor(wordColor5);
//							string wordColor6 = (num4 <= num7) ? "虎" : "龍";
//							wordColor6 = this.SetWordColor(wordColor6);
//							string wordColor7 = (num5 <= num6) ? "虎" : "龍";
//							wordColor7 = this.SetWordColor(wordColor7);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text5,
//								wordColor,
//								wordColor2,
//								wordColor3,
//								wordColor4,
//								wordColor5,
//								wordColor6,
//								wordColor7
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 11:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_speed5_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row13 in dataTable.Rows)
//						{
//							string value = row13["phase"].ToString();
//							string value2 = row13["play_open_date"].ToString();
//							int num = Convert.ToInt32(row13["n1"].ToString());
//							int num2 = Convert.ToInt32(row13["n2"].ToString());
//							int num3 = Convert.ToInt32(row13["n3"].ToString());
//							int num4 = Convert.ToInt32(row13["n4"].ToString());
//							int num5 = Convert.ToInt32(row13["n5"].ToString());
//							list2.Add(num.ToString());
//							list2.Add(num2.ToString());
//							list2.Add(num3.ToString());
//							list2.Add(num4.ToString());
//							list2.Add(num5.ToString());
//							string text6 = (num + num2 + num3 + num4 + num5).ToString();
//							string wordColor8 = SPEED5Phase.get_cl_zhdx(text6);
//							wordColor8 = this.SetWordColor(wordColor8);
//							string wordColor9 = SPEED5Phase.get_cl_zhds(text6);
//							wordColor9 = this.SetWordColor(wordColor9);
//							string wordColor10 = SPEED5Phase.get_cl_lh(num.ToString(), num5.ToString());
//							wordColor10 = this.SetWordColor(wordColor10);
//							string item = SPEED5Phase.get_speed5_str(num + "," + num2 + "," + num3);
//							string item2 = SPEED5Phase.get_speed5_str(num2 + "," + num3 + "," + num4);
//							string item3 = SPEED5Phase.get_speed5_str(num3 + "," + num4 + "," + num5);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text6,
//								wordColor8,
//								wordColor9,
//								wordColor10,
//								item,
//								item2,
//								item3
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 13:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_jscqsc_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row14 in dataTable.Rows)
//						{
//							string value = row14["phase"].ToString();
//							string value2 = row14["play_open_date"].ToString();
//							int num = Convert.ToInt32(row14["n1"].ToString());
//							int num2 = Convert.ToInt32(row14["n2"].ToString());
//							int num3 = Convert.ToInt32(row14["n3"].ToString());
//							int num4 = Convert.ToInt32(row14["n4"].ToString());
//							int num5 = Convert.ToInt32(row14["n5"].ToString());
//							list2.Add(num.ToString());
//							list2.Add(num2.ToString());
//							list2.Add(num3.ToString());
//							list2.Add(num4.ToString());
//							list2.Add(num5.ToString());
//							string text6 = (num + num2 + num3 + num4 + num5).ToString();
//							string wordColor8 = JSCQSCPhase.get_cl_zhdx(text6);
//							wordColor8 = this.SetWordColor(wordColor8);
//							string wordColor9 = JSCQSCPhase.get_cl_zhds(text6);
//							wordColor9 = this.SetWordColor(wordColor9);
//							string wordColor10 = JSCQSCPhase.get_cl_lh(num.ToString(), num5.ToString());
//							wordColor10 = this.SetWordColor(wordColor10);
//							string item = JSCQSCPhase.get_jscqsc_str(num + "," + num2 + "," + num3);
//							string item2 = JSCQSCPhase.get_jscqsc_str(num2 + "," + num3 + "," + num4);
//							string item3 = JSCQSCPhase.get_jscqsc_str(num3 + "," + num4 + "," + num5);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text6,
//								wordColor8,
//								wordColor9,
//								wordColor10,
//								item,
//								item2,
//								item3
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 12:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_jspk10_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row15 in dataTable.Rows)
//						{
//							string value = row15["phase"].ToString();
//							string value2 = row15["play_open_date"].ToString();
//							int num = Convert.ToInt32(row15["n1"].ToString());
//							int num2 = Convert.ToInt32(row15["n2"].ToString());
//							int num3 = Convert.ToInt32(row15["n3"].ToString());
//							int num4 = Convert.ToInt32(row15["n4"].ToString());
//							int num5 = Convert.ToInt32(row15["n5"].ToString());
//							int num6 = Convert.ToInt32(row15["n6"].ToString());
//							int num7 = Convert.ToInt32(row15["n7"].ToString());
//							int num8 = Convert.ToInt32(row15["n8"].ToString());
//							int num9 = Convert.ToInt32(row15["n9"].ToString());
//							int num10 = Convert.ToInt32(row15["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string text5 = (num + num2).ToString();
//							string wordColor = (Convert.ToInt32(text5) > 11) ? "大" : "小";
//							wordColor = this.SetWordColor(wordColor);
//							string wordColor2 = (Convert.ToInt32(text5) % 2 != 0) ? "單" : "雙";
//							wordColor2 = this.SetWordColor(wordColor2);
//							string wordColor3 = (num <= num10) ? "虎" : "龍";
//							wordColor3 = this.SetWordColor(wordColor3);
//							string wordColor4 = (num2 <= num9) ? "虎" : "龍";
//							wordColor4 = this.SetWordColor(wordColor4);
//							string wordColor5 = (num3 <= num8) ? "虎" : "龍";
//							wordColor5 = this.SetWordColor(wordColor5);
//							string wordColor6 = (num4 <= num7) ? "虎" : "龍";
//							wordColor6 = this.SetWordColor(wordColor6);
//							string wordColor7 = (num5 <= num6) ? "虎" : "龍";
//							wordColor7 = this.SetWordColor(wordColor7);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text5,
//								wordColor,
//								wordColor2,
//								wordColor3,
//								wordColor4,
//								wordColor5,
//								wordColor6,
//								wordColor7
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 14:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_jssfc_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					int num11 = 8;
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row16 in dataTable.Rows)
//						{
//							string value = row16["phase"].ToString();
//							string value2 = row16["play_open_date"].ToString();
//							for (int i = 1; i <= num11; i++)
//							{
//								string columnName = "n" + i;
//								list2.Add(row16[columnName].ToString());
//							}
//							string text7 = JSSFCPhase.get_zh(list2).ToString();
//							string wordColor8 = JSSFCPhase.get_cl_zhdx(text7);
//							wordColor8 = this.SetWordColor(wordColor8);
//							string wordColor9 = JSSFCPhase.get_cl_zhds(text7);
//							wordColor9 = this.SetWordColor(wordColor9);
//							string wordColor11 = JSSFCPhase.get_cl_zhwsdx(text7);
//							wordColor11 = this.SetWordColor(wordColor11);
//							string wordColor12 = JSSFCPhase.get_cl_lh(list2[0].ToString(), list2[num11 - 1].ToString());
//							wordColor12 = this.SetWordColor(wordColor12);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text7,
//								wordColor8,
//								wordColor9,
//								wordColor11,
//								wordColor12
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 15:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_jsft2_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row17 in dataTable.Rows)
//						{
//							string value = row17["phase"].ToString();
//							string value2 = row17["play_open_date"].ToString();
//							int num = Convert.ToInt32(row17["n1"].ToString());
//							int num2 = Convert.ToInt32(row17["n2"].ToString());
//							int num3 = Convert.ToInt32(row17["n3"].ToString());
//							int num4 = Convert.ToInt32(row17["n4"].ToString());
//							int num5 = Convert.ToInt32(row17["n5"].ToString());
//							int num6 = Convert.ToInt32(row17["n6"].ToString());
//							int num7 = Convert.ToInt32(row17["n7"].ToString());
//							int num8 = Convert.ToInt32(row17["n8"].ToString());
//							int num9 = Convert.ToInt32(row17["n9"].ToString());
//							int num10 = Convert.ToInt32(row17["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string text5 = (num + num2).ToString();
//							string wordColor = (Convert.ToInt32(text5) > 11) ? "大" : "小";
//							wordColor = this.SetWordColor(wordColor);
//							string wordColor2 = (Convert.ToInt32(text5) % 2 != 0) ? "單" : "雙";
//							wordColor2 = this.SetWordColor(wordColor2);
//							string wordColor3 = (num <= num10) ? "虎" : "龍";
//							wordColor3 = this.SetWordColor(wordColor3);
//							string wordColor4 = (num2 <= num9) ? "虎" : "龍";
//							wordColor4 = this.SetWordColor(wordColor4);
//							string wordColor5 = (num3 <= num8) ? "虎" : "龍";
//							wordColor5 = this.SetWordColor(wordColor5);
//							string wordColor6 = (num4 <= num7) ? "虎" : "龍";
//							wordColor6 = this.SetWordColor(wordColor6);
//							string wordColor7 = (num5 <= num6) ? "虎" : "龍";
//							wordColor7 = this.SetWordColor(wordColor7);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text5,
//								wordColor,
//								wordColor2,
//								wordColor3,
//								wordColor4,
//								wordColor5,
//								wordColor6,
//								wordColor7
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 16:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_car168_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row18 in dataTable.Rows)
//						{
//							string value = row18["phase"].ToString();
//							string value2 = row18["play_open_date"].ToString();
//							int num = Convert.ToInt32(row18["n1"].ToString());
//							int num2 = Convert.ToInt32(row18["n2"].ToString());
//							int num3 = Convert.ToInt32(row18["n3"].ToString());
//							int num4 = Convert.ToInt32(row18["n4"].ToString());
//							int num5 = Convert.ToInt32(row18["n5"].ToString());
//							int num6 = Convert.ToInt32(row18["n6"].ToString());
//							int num7 = Convert.ToInt32(row18["n7"].ToString());
//							int num8 = Convert.ToInt32(row18["n8"].ToString());
//							int num9 = Convert.ToInt32(row18["n9"].ToString());
//							int num10 = Convert.ToInt32(row18["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string text5 = (num + num2).ToString();
//							string wordColor = (Convert.ToInt32(text5) > 11) ? "大" : "小";
//							wordColor = this.SetWordColor(wordColor);
//							string wordColor2 = (Convert.ToInt32(text5) % 2 != 0) ? "單" : "雙";
//							wordColor2 = this.SetWordColor(wordColor2);
//							string wordColor3 = (num <= num10) ? "虎" : "龍";
//							wordColor3 = this.SetWordColor(wordColor3);
//							string wordColor4 = (num2 <= num9) ? "虎" : "龍";
//							wordColor4 = this.SetWordColor(wordColor4);
//							string wordColor5 = (num3 <= num8) ? "虎" : "龍";
//							wordColor5 = this.SetWordColor(wordColor5);
//							string wordColor6 = (num4 <= num7) ? "虎" : "龍";
//							wordColor6 = this.SetWordColor(wordColor6);
//							string wordColor7 = (num5 <= num6) ? "虎" : "龍";
//							wordColor7 = this.SetWordColor(wordColor7);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text5,
//								wordColor,
//								wordColor2,
//								wordColor3,
//								wordColor4,
//								wordColor5,
//								wordColor6,
//								wordColor7
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 17:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_ssc168_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row19 in dataTable.Rows)
//						{
//							string value = row19["phase"].ToString();
//							string value2 = row19["play_open_date"].ToString();
//							int num = Convert.ToInt32(row19["n1"].ToString());
//							int num2 = Convert.ToInt32(row19["n2"].ToString());
//							int num3 = Convert.ToInt32(row19["n3"].ToString());
//							int num4 = Convert.ToInt32(row19["n4"].ToString());
//							int num5 = Convert.ToInt32(row19["n5"].ToString());
//							list2.Add(num.ToString());
//							list2.Add(num2.ToString());
//							list2.Add(num3.ToString());
//							list2.Add(num4.ToString());
//							list2.Add(num5.ToString());
//							string text6 = (num + num2 + num3 + num4 + num5).ToString();
//							string wordColor8 = SSC168Phase.get_cl_zhdx(text6);
//							wordColor8 = this.SetWordColor(wordColor8);
//							string wordColor9 = SSC168Phase.get_cl_zhds(text6);
//							wordColor9 = this.SetWordColor(wordColor9);
//							string wordColor10 = SSC168Phase.get_cl_lh(num.ToString(), num5.ToString());
//							wordColor10 = this.SetWordColor(wordColor10);
//							string item = SSC168Phase.get_ssc168_str(num + "," + num2 + "," + num3);
//							string item2 = SSC168Phase.get_ssc168_str(num2 + "," + num3 + "," + num4);
//							string item3 = SSC168Phase.get_ssc168_str(num3 + "," + num4 + "," + num5);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text6,
//								wordColor8,
//								wordColor9,
//								wordColor10,
//								item,
//								item2,
//								item3
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 18:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_vrcar_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row20 in dataTable.Rows)
//						{
//							string value = row20["phase"].ToString();
//							string value2 = row20["play_open_date"].ToString();
//							int num = Convert.ToInt32(row20["n1"].ToString());
//							int num2 = Convert.ToInt32(row20["n2"].ToString());
//							int num3 = Convert.ToInt32(row20["n3"].ToString());
//							int num4 = Convert.ToInt32(row20["n4"].ToString());
//							int num5 = Convert.ToInt32(row20["n5"].ToString());
//							int num6 = Convert.ToInt32(row20["n6"].ToString());
//							int num7 = Convert.ToInt32(row20["n7"].ToString());
//							int num8 = Convert.ToInt32(row20["n8"].ToString());
//							int num9 = Convert.ToInt32(row20["n9"].ToString());
//							int num10 = Convert.ToInt32(row20["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string text5 = (num + num2).ToString();
//							string wordColor = (Convert.ToInt32(text5) > 11) ? "大" : "小";
//							wordColor = this.SetWordColor(wordColor);
//							string wordColor2 = (Convert.ToInt32(text5) % 2 != 0) ? "單" : "雙";
//							wordColor2 = this.SetWordColor(wordColor2);
//							string wordColor3 = (num <= num10) ? "虎" : "龍";
//							wordColor3 = this.SetWordColor(wordColor3);
//							string wordColor4 = (num2 <= num9) ? "虎" : "龍";
//							wordColor4 = this.SetWordColor(wordColor4);
//							string wordColor5 = (num3 <= num8) ? "虎" : "龍";
//							wordColor5 = this.SetWordColor(wordColor5);
//							string wordColor6 = (num4 <= num7) ? "虎" : "龍";
//							wordColor6 = this.SetWordColor(wordColor6);
//							string wordColor7 = (num5 <= num6) ? "虎" : "龍";
//							wordColor7 = this.SetWordColor(wordColor7);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text5,
//								wordColor,
//								wordColor2,
//								wordColor3,
//								wordColor4,
//								wordColor5,
//								wordColor6,
//								wordColor7
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 19:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_vrssc_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row21 in dataTable.Rows)
//						{
//							string value = row21["phase"].ToString();
//							string value2 = row21["play_open_date"].ToString();
//							int num = Convert.ToInt32(row21["n1"].ToString());
//							int num2 = Convert.ToInt32(row21["n2"].ToString());
//							int num3 = Convert.ToInt32(row21["n3"].ToString());
//							int num4 = Convert.ToInt32(row21["n4"].ToString());
//							int num5 = Convert.ToInt32(row21["n5"].ToString());
//							list2.Add(num.ToString());
//							list2.Add(num2.ToString());
//							list2.Add(num3.ToString());
//							list2.Add(num4.ToString());
//							list2.Add(num5.ToString());
//							string text6 = (num + num2 + num3 + num4 + num5).ToString();
//							string wordColor8 = VRSSCPhase.get_cl_zhdx(text6);
//							wordColor8 = this.SetWordColor(wordColor8);
//							string wordColor9 = VRSSCPhase.get_cl_zhds(text6);
//							wordColor9 = this.SetWordColor(wordColor9);
//							string wordColor10 = VRSSCPhase.get_cl_lh(num.ToString(), num5.ToString());
//							wordColor10 = this.SetWordColor(wordColor10);
//							string item = VRSSCPhase.get_vrssc_str(num + "," + num2 + "," + num3);
//							string item2 = VRSSCPhase.get_vrssc_str(num2 + "," + num3 + "," + num4);
//							string item3 = VRSSCPhase.get_vrssc_str(num3 + "," + num4 + "," + num5);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text6,
//								wordColor8,
//								wordColor9,
//								wordColor10,
//								item,
//								item2,
//								item3
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 20:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_xyftoa_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row22 in dataTable.Rows)
//						{
//							string value = row22["phase"].ToString();
//							string value2 = row22["play_open_date"].ToString();
//							int num = Convert.ToInt32(row22["n1"].ToString());
//							int num2 = Convert.ToInt32(row22["n2"].ToString());
//							int num3 = Convert.ToInt32(row22["n3"].ToString());
//							int num4 = Convert.ToInt32(row22["n4"].ToString());
//							int num5 = Convert.ToInt32(row22["n5"].ToString());
//							int num6 = Convert.ToInt32(row22["n6"].ToString());
//							int num7 = Convert.ToInt32(row22["n7"].ToString());
//							int num8 = Convert.ToInt32(row22["n8"].ToString());
//							int num9 = Convert.ToInt32(row22["n9"].ToString());
//							int num10 = Convert.ToInt32(row22["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string text5 = (num + num2).ToString();
//							string wordColor = (Convert.ToInt32(text5) > 11) ? "大" : "小";
//							wordColor = this.SetWordColor(wordColor);
//							string wordColor2 = (Convert.ToInt32(text5) % 2 != 0) ? "單" : "雙";
//							wordColor2 = this.SetWordColor(wordColor2);
//							string wordColor3 = (num <= num10) ? "虎" : "龍";
//							wordColor3 = this.SetWordColor(wordColor3);
//							string wordColor4 = (num2 <= num9) ? "虎" : "龍";
//							wordColor4 = this.SetWordColor(wordColor4);
//							string wordColor5 = (num3 <= num8) ? "虎" : "龍";
//							wordColor5 = this.SetWordColor(wordColor5);
//							string wordColor6 = (num4 <= num7) ? "虎" : "龍";
//							wordColor6 = this.SetWordColor(wordColor6);
//							string wordColor7 = (num5 <= num6) ? "虎" : "龍";
//							wordColor7 = this.SetWordColor(wordColor7);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text5,
//								wordColor,
//								wordColor2,
//								wordColor3,
//								wordColor4,
//								wordColor5,
//								wordColor6,
//								wordColor7
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 21:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_xyftsg_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row23 in dataTable.Rows)
//						{
//							string value = row23["phase"].ToString();
//							string value2 = row23["play_open_date"].ToString();
//							int num = Convert.ToInt32(row23["n1"].ToString());
//							int num2 = Convert.ToInt32(row23["n2"].ToString());
//							int num3 = Convert.ToInt32(row23["n3"].ToString());
//							int num4 = Convert.ToInt32(row23["n4"].ToString());
//							int num5 = Convert.ToInt32(row23["n5"].ToString());
//							int num6 = Convert.ToInt32(row23["n6"].ToString());
//							int num7 = Convert.ToInt32(row23["n7"].ToString());
//							int num8 = Convert.ToInt32(row23["n8"].ToString());
//							int num9 = Convert.ToInt32(row23["n9"].ToString());
//							int num10 = Convert.ToInt32(row23["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string text5 = (num + num2).ToString();
//							string wordColor = (Convert.ToInt32(text5) > 11) ? "大" : "小";
//							wordColor = this.SetWordColor(wordColor);
//							string wordColor2 = (Convert.ToInt32(text5) % 2 != 0) ? "單" : "雙";
//							wordColor2 = this.SetWordColor(wordColor2);
//							string wordColor3 = (num <= num10) ? "虎" : "龍";
//							wordColor3 = this.SetWordColor(wordColor3);
//							string wordColor4 = (num2 <= num9) ? "虎" : "龍";
//							wordColor4 = this.SetWordColor(wordColor4);
//							string wordColor5 = (num3 <= num8) ? "虎" : "龍";
//							wordColor5 = this.SetWordColor(wordColor5);
//							string wordColor6 = (num4 <= num7) ? "虎" : "龍";
//							wordColor6 = this.SetWordColor(wordColor6);
//							string wordColor7 = (num5 <= num6) ? "虎" : "龍";
//							wordColor7 = this.SetWordColor(wordColor7);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text5,
//								wordColor,
//								wordColor2,
//								wordColor3,
//								wordColor4,
//								wordColor5,
//								wordColor6,
//								wordColor7
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
//			case 22:
//			{
//				List<object> list = new List<object>();
//				List<string> list2 = new List<string>();
//				DataSet currentByPhase = CallBLL.cz_phase_happycar_bll.GetCurrentByPhase(text);
//				if (currentByPhase != null)
//				{
//					DataTable dataTable = currentByPhase.Tables[0];
//					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
//					string text3 = string.Concat(dataTable.Rows[0]["is_closed"]);
//					string text4 = string.Concat(dataTable.Rows[0]["is_opendata"]);
//					if (text3.Equals("1") && text4.Equals("1"))
//					{
//						foreach (DataRow row24 in dataTable.Rows)
//						{
//							string value = row24["phase"].ToString();
//							string value2 = row24["play_open_date"].ToString();
//							int num = Convert.ToInt32(row24["n1"].ToString());
//							int num2 = Convert.ToInt32(row24["n2"].ToString());
//							int num3 = Convert.ToInt32(row24["n3"].ToString());
//							int num4 = Convert.ToInt32(row24["n4"].ToString());
//							int num5 = Convert.ToInt32(row24["n5"].ToString());
//							int num6 = Convert.ToInt32(row24["n6"].ToString());
//							int num7 = Convert.ToInt32(row24["n7"].ToString());
//							int num8 = Convert.ToInt32(row24["n8"].ToString());
//							int num9 = Convert.ToInt32(row24["n9"].ToString());
//							int num10 = Convert.ToInt32(row24["n10"].ToString());
//							list2.Add((num < 10) ? ("0" + num) : num.ToString());
//							list2.Add((num2 < 10) ? ("0" + num2) : num2.ToString());
//							list2.Add((num3 < 10) ? ("0" + num3) : num3.ToString());
//							list2.Add((num4 < 10) ? ("0" + num4) : num4.ToString());
//							list2.Add((num5 < 10) ? ("0" + num5) : num5.ToString());
//							list2.Add((num6 < 10) ? ("0" + num6) : num6.ToString());
//							list2.Add((num7 < 10) ? ("0" + num7) : num7.ToString());
//							list2.Add((num8 < 10) ? ("0" + num8) : num8.ToString());
//							list2.Add((num9 < 10) ? ("0" + num9) : num9.ToString());
//							list2.Add((num10 < 10) ? ("0" + num10) : num10.ToString());
//							string text5 = (num + num2).ToString();
//							string wordColor = (Convert.ToInt32(text5) > 11) ? "大" : "小";
//							wordColor = this.SetWordColor(wordColor);
//							string wordColor2 = (Convert.ToInt32(text5) % 2 != 0) ? "單" : "雙";
//							wordColor2 = this.SetWordColor(wordColor2);
//							string wordColor3 = (num <= num10) ? "虎" : "龍";
//							wordColor3 = this.SetWordColor(wordColor3);
//							string wordColor4 = (num2 <= num9) ? "虎" : "龍";
//							wordColor4 = this.SetWordColor(wordColor4);
//							string wordColor5 = (num3 <= num8) ? "虎" : "龍";
//							wordColor5 = this.SetWordColor(wordColor5);
//							string wordColor6 = (num4 <= num7) ? "虎" : "龍";
//							wordColor6 = this.SetWordColor(wordColor6);
//							string wordColor7 = (num5 <= num6) ? "虎" : "龍";
//							wordColor7 = this.SetWordColor(wordColor7);
//							dictionary2.Add("phase", value);
//							dictionary2.Add("play_open_date", value2);
//							dictionary2.Add("draw_num", new List<string>(list2));
//							dictionary2.Add("total", new List<string>
//							{
//								text5,
//								wordColor,
//								wordColor2,
//								wordColor3,
//								wordColor4,
//								wordColor5,
//								wordColor6,
//								wordColor7
//							});
//							list.Add(new Dictionary<string, object>(dictionary2));
//							dictionary2.Clear();
//							list2.Clear();
//						}
//					}
//				}
//				dictionary.Add("jqkj", list);
//				break;
//			}
			}
			val.set_success(200);
			val.set_data(dictionary);
			strResult = JsonHandle.ObjectToJson(val);
		}

		public void get_gamehall(HttpContext context, ref string strResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			ReturnResult val = new ReturnResult();
			val.set_success(400);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("type", "get_gamehall");
			if (context.Session["user_name"] == null)
			{
				context.Response.End();
				return;
			}
			string text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			int num = int.Parse(LSRequest.qq("lid"));
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			DataTable lotteryList = GetLotteryList();
			string text2 = "0";
			string text3 = "";
			if (num.Equals(100))
			{
				DataTable currentPhase = CallBLL.cz_phase_six_bll.GetCurrentPhase("1");
				if (currentPhase != null || currentPhase.Rows.Count > 0)
				{
					DateTime now = DateTime.Now;
					string text4 = currentPhase.Rows[0]["sn_stop_date"].ToString();
					if (now < DateTime.Parse(text4))
					{
						text2 = "2";
						text3 = Convert.ToDateTime(text4).ToString("yyyy-MM-dd HH:mm:ss");
					}
					else
					{
						text2 = "0";
						text3 = "-";
					}
				}
			}
			else
			{
				DataTable dataTable = null;
				switch (num)
				{
				case 0:
					dataTable = CallBLL.cz_phase_kl10_bll.IsPhaseClose();
					break;
				case 1:
					dataTable = CallBLL.cz_phase_cqsc_bll.IsPhaseClose();
					break;
				case 2:
					dataTable = CallBLL.cz_phase_pk10_bll.IsPhaseClose();
					break;
				case 3:
					dataTable = CallBLL.cz_phase_xync_bll.IsPhaseClose();
					break;
				case 4:
					dataTable = CallBLL.cz_phase_jsk3_bll.IsPhaseClose();
					break;
				case 5:
					dataTable = CallBLL.cz_phase_kl8_bll.IsPhaseClose();
					break;
				case 6:
					dataTable = CallBLL.cz_phase_k8sc_bll.IsPhaseClose();
					break;
				case 7:
					dataTable = CallBLL.cz_phase_pcdd_bll.IsPhaseClose();
					break;
				case 8:
					dataTable = CallBLL.cz_phase_pkbjl_bll.IsPhaseClose();
					break;
				case 9:
					dataTable = CallBLL.cz_phase_xyft5_bll.IsPhaseClose();
					break;
				case 10:
					dataTable = CallBLL.cz_phase_jscar_bll.IsPhaseClose();
					break;
				case 11:
					dataTable = CallBLL.cz_phase_speed5_bll.IsPhaseClose();
					break;
				case 13:
					dataTable = CallBLL.cz_phase_jscqsc_bll.IsPhaseClose();
					break;
				case 12:
					dataTable = CallBLL.cz_phase_jspk10_bll.IsPhaseClose();
					break;
				case 14:
					dataTable = CallBLL.cz_phase_jssfc_bll.IsPhaseClose();
					break;
				case 15:
					dataTable = CallBLL.cz_phase_jsft2_bll.IsPhaseClose();
					break;
				case 16:
					dataTable = CallBLL.cz_phase_car168_bll.IsPhaseClose();
					break;
				case 17:
					dataTable = CallBLL.cz_phase_ssc168_bll.IsPhaseClose();
					break;
				case 18:
					dataTable = CallBLL.cz_phase_vrcar_bll.IsPhaseClose();
					break;
				case 19:
					dataTable = CallBLL.cz_phase_vrssc_bll.IsPhaseClose();
					break;
				case 20:
					dataTable = CallBLL.cz_phase_xyftoa_bll.IsPhaseClose();
					break;
				case 21:
					dataTable = CallBLL.cz_phase_xyftsg_bll.IsPhaseClose();
					break;
				case 22:
					dataTable = CallBLL.cz_phase_happycar_bll.IsPhaseClose();
					break;
				}
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					string text5 = dataTable.Rows[0]["isopen"].ToString();
					string text6 = dataTable.Rows[0]["openning"].ToString();
					dataTable.Rows[0]["opendate"].ToString();
					string value = dataTable.Rows[0]["endtime"].ToString();
					DataRow[] array = lotteryList.Select($" id={num} ");
					DateTime dateTime = Convert.ToDateTime(array[0]["begintime"].ToString());
					if (text5.Equals("0"))
					{
						text2 = "0";
						DateTime now2 = DateTime.Now;
						text3 = ((!(now2.ToString("yyyy-MM-dd") == now2.AddHours(7.0).ToString("yyyy-MM-dd"))) ? (now2.AddDays(1.0).ToString("yyyy-MM-dd ") + dateTime.ToString("HH:mm:ss")) : (now2.ToString("yyyy-MM-dd ") + dateTime.ToString("HH:mm:ss")));
					}
					else
					{
						text2 = ((!text6.Equals("n")) ? "2" : "1");
						text3 = Convert.ToDateTime(value).ToString("HH:mm:ss");
					}
				}
			}
			dictionary2.Add(num.ToString(), text2 + "," + text3 + "," + text);
			dictionary.Add("isopendata", dictionary2);
			val.set_data(dictionary);
			val.set_success(200);
			strResult = JsonHandle.ObjectToJson(val);
		}

		public void get_reportcookies(HttpContext context, ref string strResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			ReturnResult val = new ReturnResult();
			val.set_success(400);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("type", "get_reportcookies");
			if (context.Session["user_name"] == null)
			{
				context.Response.End();
				return;
			}
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
			val.set_data(dictionary);
			val.set_success(200);
			strResult = JsonHandle.ObjectToJson(val);
		}

		protected new string get_tz(string hm)
		{
			if (hm.Trim() == "49")
			{
				return "和";
			}
			if (int.Parse(hm) % 4 == 0)
			{
				return "4";
			}
			return (int.Parse(hm) % 4).ToString();
		}

		protected string get_pcdd_bs_str(string num)
		{
			string text = "03,06,09,12,15,18,21,24";
			string text2 = "02,05,08,11,17,20,23,26";
			string text3 = "01,04,07,10,16,19,22,25";
			string text4 = num;
			if (int.Parse(text4) < 10)
			{
				text4 = "0" + int.Parse(text4);
			}
			if (text.IndexOf(text4) > -1)
			{
				return "紅波";
			}
			if (text2.IndexOf(text4) > -1)
			{
				return "藍波";
			}
			if (text3.IndexOf(text4) > -1)
			{
				return "綠波";
			}
			return "-";
		}

		public override void ProcessRequest(HttpContext context)
		{
			checkLoginByHandler(0);
			string text = LSRequest.qq("action");
			string strResult = "";
			if (!string.IsNullOrEmpty(text))
			{
				switch (text)
				{
				case "get_ad":
					get_ad(context, ref strResult);
					break;
				case "get_newbetlist":
					get_newbetlist(context, ref strResult);
					break;
				case "get_openlottery":
					IsOpenLottery(context, ref strResult);
					break;
				case "get_playbylottery":
					GetPlayByLottery(context, ref strResult);
					break;
				case "get_phasebylottery":
					GetPhaseByLottery(context, ref strResult);
					break;
				case "set_skin":
					set_skin(context, ref strResult);
					break;
				case "get_six_date":
					get_six_date(context, ref strResult);
					break;
				case "get_user_rate":
					get_user_rate(context, ref strResult);
					break;
				case "get_currentphase":
					get_currentphase(context, ref strResult);
					break;
				case "get_gamehall":
					get_gamehall(context, ref strResult);
					break;
				case "get_reptcookies":
					get_reportcookies(context, ref strResult);
					break;
				}
				context.Response.ContentType = "text/json";
				context.Response.Write(strResult);
			}
		}
	}
}
