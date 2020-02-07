using Agent.Web.WebBase;
using Entity;
using LotterySystem.Common;
using LotterySystem.Common.Redis;
using Web;

namespace Agent.Web.Handler
{
    using LotterySystem.Model;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using System.Web.SessionState;

    public class LoginHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string str = LSRequest.qq("action").Trim();
                string strResult = "";
                string str3 = str;
                if ((str3 != null) && (str3 == "user_login"))
                {
                    this.user_login(context, ref strResult);
                }
                context.Response.ContentType = "text/json";
                context.Response.Write(strResult);
                context.Response.End();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private void user_login(HttpContext context, ref string strResult)
        {
            string str = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            ReturnResult result = new ReturnResult();
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                { 
                    "type",
                    "user_login"
                }
            };
            string loginName = LSRequest.qq("loginName").Trim().ToLower();
            string loginPwd = LSRequest.qq("loginPwd").Trim();
            string ValidateCode = LSRequest.qq("ValidateCode").Trim();
            if (PageBase.is_ip_locked())
            {
                context.Session["lottery_session_img_code"] = null;
                result.set_success(400);
                result.set_tipinfo("由於輸入錯誤次數過多,您已被禁用,請稍後再試!");
                strResult = JsonHandle.ObjectToJson(result);
            }
            else if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPwd))
            {
                context.Response.End();
            }
            else
            {
                DateTime time = new DateTime();
                string retry_times;
                string str10;
                string str11;
                string str14;
                DateTime? nullable;
                int num2;
                DateTime? nullable3;
                DateTime time2;
                if (int.Parse(FileCacheHelper.get_GetLockedPasswordCount()) == 0)
                {
                    context.Session["lottery_session_img_code_display"] = 1;
                }
                if (context.Session["lottery_session_img_code_display"] == null)
                {
                    if (CallBLL.cz_user_psw_err_log_bll.IsExistUser(loginName))
                    {
//                        TODO 登录超时
//                        if (PageBase.IsErrTimesAbove(ref time, str5))
//                        {
//                            if (!PageBase.IsErrTimeout(time))
//                            {
//                                context.Session["lottery_session_img_code"] = null;
//                                result.set_success(400);
//                                result.set_tipinfo("");
//                                dictionary.Add("is_display_code", "1");
//                                result.set_data(dictionary);
//                                strResult = JsonHandle.ObjectToJson(result);
//                                context.Session["lottery_session_img_code_display"] = 1;
//                                return;
//                            }
//                            CallBLL.cz_user_psw_err_log_bll.ZeroErrTimes(str5);
//                            context.Session["lottery_session_img_code"] = null;
//                            context.Session["lottery_session_img_code_display"] = 0;
//                        }
//                        else
//                        {
//                            context.Session["lottery_session_img_code"] = null;
//                            context.Session["lottery_session_img_code_display"] = 0;
//                        }
                    }
                    else
                    {
                        context.Session["lottery_session_img_code"] = null;
                        context.Session["lottery_session_img_code_display"] = 0;
                    }
                }
                if (context.Session["lottery_session_img_code_display"].ToString() == "0")
                {
                    if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPwd))
                    {
                        context.Response.End();
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(loginPwd))
                    {
                        context.Response.End();
                        return;
                    }
                    if (string.IsNullOrEmpty(ValidateCode))
                    {
                        context.Session["lottery_session_img_code"] = null;
                        result.set_success(400);
                        result.set_tipinfo("");
                        dictionary.Add("is_display_code", "1");
                        result.set_data(dictionary);
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session["lottery_session_img_code_display"] = 1;
                        return;
                    }
                    if (context.Session["lottery_session_img_code"] == null)
                    {
                        context.Response.End();
                        return;
                    }
                    if (context.Session["lottery_session_img_code"].ToString().ToLower() != ValidateCode.ToLower())
                    {
                        context.Session["lottery_session_img_code"] = null;
                        result.set_success(400);
                        result.set_tipinfo(PageBase.GetMessageByCache("u100001", "MessageHint"));
                        strResult = JsonHandle.ObjectToJson(result);
                        return;
                    }
                }
                cz_users _users = CallBLL.CzUsersService.AgentLogin(loginName.ToLower());
                cz_users_child _child = null;
                if (_users == null)
                {
                    _child = CallBLL.CzUsersChildService.AgentLogin(loginName.ToLower());
                    if (_child != null)
                    {
                        retry_times = _child.get_retry_times().ToString();
                        if (!string.IsNullOrEmpty(retry_times) && (int.Parse(retry_times) > int.Parse(FileCacheHelper.get_GetLockedUserCount())))
                        {
                            if (!PageBase.IsLockedTimeout(loginName, "child"))
                            {
                                context.Session["lottery_session_img_code"] = null;
                                result.set_success(560);
                                result.set_tipinfo("您的帳號因密碼多次輸入錯誤被鎖死,請與管理員聯系!");
                                strResult = JsonHandle.ObjectToJson(result);
                                return;
                            }
                            PageBase.zero_retry_times_children(loginName);
                        }
                        str10 = _child.get_salt().Trim();
                        str11 = DESEncrypt.EncryptString(loginPwd, str10);
                        if (_child.get_u_psw() != str11)
                        {
                            context.Session["lottery_session_img_code"] = null;
                            PageBase.inc_retry_times_children(loginName);
                            PageBase.login_error_ip();
                            result.set_success(400);
                            result.set_tipinfo(PageBase.GetMessageByCache("u100003", "MessageHint"));
                            strResult = JsonHandle.ObjectToJson(result);
                            if (context.Session["lottery_session_img_code_display"].ToString() == "0")
                            {
                                if (CallBLL.cz_user_psw_err_log_bll.IsExistUser(loginName))
                                {
                                    CallBLL.cz_user_psw_err_log_bll.UpdateErrTimes(loginName);
                                }
                                else
                                {
                                    CallBLL.cz_user_psw_err_log_bll.AddUser(loginName);
                                }
                                if (PageBase.IsErrTimesAbove(ref time, loginName))
                                {
                                    context.Session["lottery_session_img_code"] = null;
                                    result.set_success(400);
                                    result.set_tipinfo(PageBase.GetMessageByCache("u100003", "MessageHint"));
                                    dictionary.Add("is_display_code", "1");
                                    result.set_data(dictionary);
                                    strResult = JsonHandle.ObjectToJson(result);
                                    context.Session["lottery_session_img_code_display"] = 1;
                                }
                            }
                            return;
                        }
                        str2 = _child.get_status().ToString();
                        str3 = PageBase.upper_user_status(_child.get_parent_u_name());
                        _users = CallBLL.CzUsersService.AgentLogin(_child.get_parent_u_name());
                    }
                    else
                    {
                        context.Session["lottery_session_img_code"] = null;
                        PageBase.login_error_ip();
                        result.set_success(400);
                        result.set_tipinfo(PageBase.GetMessageByCache("u100002", "MessageHint"));
                        dictionary.Add("fs_name", "loginName");
                        strResult = JsonHandle.ObjectToJson(result);
                        return;
                    }
                    PageBase.zero_retry_times_children(loginName);
                }
                else
                {
                    retry_times = _users.get_retry_times().ToString();
                    if (!string.IsNullOrEmpty(retry_times) && (int.Parse(retry_times) > int.Parse(FileCacheHelper.get_GetLockedUserCount())))
                    {
                        if (!PageBase.IsLockedTimeout(loginName, "master"))
                        {
                            context.Session["lottery_session_img_code"] = null;
                            result.set_success(560);
                            result.set_tipinfo("您的帳號因密碼多次輸入錯誤被鎖死,請與管理員聯系!");
                            strResult = JsonHandle.ObjectToJson(result);
                            return;
                        }
                        PageBase.zero_retry_times(loginName);
                    }
                    str = _users.get_a_state().ToString();
                    string str12 = _users.get_a_state().ToString();
                    str4 = PageBase.upper_user_status(_users.get_u_name());
                    if (str12.Equals("2"))
                    {
                        context.Session["lottery_session_img_code"] = null;
                        result.set_success(400);
                        result.set_tipinfo(PageBase.GetMessageByCache("u100005", "MessageHint"));
                        dictionary.Add("fs_name", "loginName");
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session.Abandon();
                        return;
                    }
                    if (str4 == "2")
                    {
                        context.Session["lottery_session_img_code"] = null;
                        result.set_success(400);
                        result.set_tipinfo("您的上級帳號已被停用,请与管理员联系!");
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session.Abandon();
                        return;
                    }
                    str = (str12 == null) ? "0" : str;
                    str10 = _users.get_salt().Trim();
                    str11 = DESEncrypt.EncryptString(loginPwd, str10);
                    if (_users.get_u_psw() != str11)
                    {
                        context.Session["lottery_session_img_code"] = null;
                        PageBase.inc_retry_times(loginName);
                        PageBase.login_error_ip();
                        result.set_success(400);
                        result.set_tipinfo(PageBase.GetMessageByCache("u100003", "MessageHint"));
                        strResult = JsonHandle.ObjectToJson(result);
                        if (context.Session["lottery_session_img_code_display"].ToString() == "0")
                        {
                            if (CallBLL.cz_user_psw_err_log_bll.IsExistUser(loginName))
                            {
                                CallBLL.cz_user_psw_err_log_bll.UpdateErrTimes(loginName);
                            }
                            else
                            {
                                CallBLL.cz_user_psw_err_log_bll.AddUser(loginName);
                            }
                            if (PageBase.IsErrTimesAbove(ref time, loginName))
                            {
                                context.Session["lottery_session_img_code"] = null;
                                result.set_success(400);
                                result.set_tipinfo(PageBase.GetMessageByCache("u100003", "MessageHint"));
                                dictionary.Add("is_display_code", "1");
                                result.set_data(dictionary);
                                strResult = JsonHandle.ObjectToJson(result);
                                context.Session["lottery_session_img_code_display"] = 1;
                            }
                        }
                        return;
                    }
                    PageBase.zero_retry_times(loginName);
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    context.Session["user_name"] = _users.get_u_name().Trim();
                    context.Session["user_type"] = _users.get_u_type().Trim();
                    context.Session["child_user_name"] = _child.get_u_name().Trim();
                    context.Session["user_state"] = str2.Trim();
                    if (str2.Equals("2"))
                    {
                        context.Session["lottery_session_img_code"] = null;
                        result.set_success(400);
                        result.set_tipinfo("您的帳號已被停用,请与管理员联系!");
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session.Abandon();
                        return;
                    }
                    if (_users.get_a_state() == 2)
                    {
                        context.Session["lottery_session_img_code"] = null;
                        result.set_success(400);
                        result.set_tipinfo("您的主帳號已被停用,请与管理员联系!");
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session.Abandon();
                        return;
                    }
                    if (str3 == "2")
                    {
                        context.Session["lottery_session_img_code"] = null;
                        result.set_success(400);
                        result.set_tipinfo("您的上級帳號已被停用,请与管理员联系!");
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session.Abandon();
                        return;
                    }
                    if (str2 == "1")
                    {
                        result.set_success(200);
                        result.set_tipinfo(PageBase.GetMessageByCache("u100004", "MessageHint"));
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session["user_state"] = str2;
                    }
                    else if (_users.get_a_state() == 1)
                    {
                        result.set_success(200);
                        result.set_tipinfo("您的主帳號已被凍結,请与管理员联系!");
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session["user_state"] = _users.get_a_state().ToString();
                    }
                    else if (str3 == "1")
                    {
                        result.set_success(200);
                        result.set_tipinfo(PageBase.GetMessageByCache("u100006", "MessageHint"));
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session["user_state"] = str3;
                    }
                    else
                    {
                        context.Session["user_state"] = "0";
                        result.set_success(200);
                        strResult = JsonHandle.ObjectToJson(result);
                    }
                }
                else
                {
                    context.Session["user_name"] = _users.get_u_name().Trim();
                    context.Session["user_type"] = _users.get_u_type().Trim();
                    context.Session["user_state"] = str.Trim();
                    if (str.Equals("1"))
                    {
                        result.set_success(200);
                        result.set_tipinfo(PageBase.GetMessageByCache("u100004", "MessageHint"));
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session["user_state"] = str;
                    }
                    else if (str4 == "1")
                    {
                        result.set_success(200);
                        result.set_tipinfo(PageBase.GetMessageByCache("u100006", "MessageHint"));
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session["user_state"] = str4;
                    }
                    else
                    {
                        context.Session["user_state"] = "0";
                        result.set_success(200);
                        strResult = JsonHandle.ObjectToJson(result);
                    }
                }
                agent_userinfo_session _session = new agent_userinfo_session();
                _session.set_u_id(_users.get_u_id());
                _session.set_u_name(_users.get_u_name().Trim());
                _session.set_u_psw(_users.get_u_psw().Trim());
                _session.set_u_nicker(_users.get_u_nicker().Trim());
                _session.set_u_skin(_users.get_u_skin().Trim());
                if (_child != null)
                {
                    if (string.IsNullOrEmpty(_child.get_u_skin()))
                    {
                        _session.set_u_skin("");
                    }
                    else
                    {
                        _session.set_u_skin(_child.get_u_skin());
                    }
                }
                _session.set_sup_name(_users.get_sup_name().Trim());
                _session.set_u_type(_users.get_u_type().Trim());
                _session.set_su_type(_users.get_su_type().Trim());
                _session.set_a_state(_users.get_a_state());
                _session.set_six_kind(_users.get_six_kind());
                _session.set_kc_kind(_users.get_kc_kind());
                _session.set_allow_sale(_users.get_allow_sale());
                _session.set_kc_allow_sale(_users.get_kc_allow_sale());
                _session.set_negative_sale(_users.get_negative_sale());
                if (!_users.get_allow_view_report().HasValue)
                {
                    _session.set_allow_view_report(0);
                }
                else
                {
                    _session.set_allow_view_report(_users.get_allow_view_report());
                }
                DataRow item = CallBLL.cz_admin_sysconfig_bll.GetItem();
                if (item == null)
                {
                    _session.set_u_skin("Blue");
                }
                else
                {
                    string str13 = item["agent_skin"].ToString();
                    if (string.IsNullOrEmpty(_session.get_u_skin()) || (str13.IndexOf(_session.get_u_skin()) < 0))
                    {
                        _session.set_u_skin(str13.Split(new char[] { '|' })[0]);
                    }
                }
                if (_child != null)
                {
                    _child.set_salt("");
                }
                _session.set_users_child_session(_child);
                cz_users zJInfo = CallBLL.CzUsersService.GetZJInfo();
                if (zJInfo != null)
                {
                    _session.set_zjname(zJInfo.get_u_name().ToString().Trim());
                }
                if (!_session.get_u_type().ToLower().Equals("zj"))
                {
                    cz_rate_kc rateKCByUserName = CallBLL.CzRateKcService.GetRateKCByUserName(_session.get_u_name());
                    _session.set_fgs_name(rateKCByUserName.get_fgs_name());
                    _session.set_gd_name(rateKCByUserName.get_gd_name());
                    _session.set_zd_name(rateKCByUserName.get_zd_name());
                    _session.set_dl_name(rateKCByUserName.get_dl_name());
                    DataTable userOpOdds = CallBLL.CzRateKcService.GetUserOpOdds(_session.get_u_name());
                    if (userOpOdds != null)
                    {
                        if ((userOpOdds.Rows[0]["six_op_odds"] != null) && (userOpOdds.Rows[0]["six_op_odds"].ToString() != ""))
                        {
                            _session.set_six_op_odds(new int?(int.Parse(userOpOdds.Rows[0]["six_op_odds"].ToString())));
                        }
                        if ((userOpOdds.Rows[0]["kc_op_odds"] != null) && (userOpOdds.Rows[0]["kc_op_odds"].ToString() != ""))
                        {
                            _session.set_kc_op_odds(new int?(int.Parse(userOpOdds.Rows[0]["kc_op_odds"].ToString())));
                        }
                    }
                }
                context.Session["child_user_name"] = null;
                if (_child != null)
                {
                    context.Session["child_user_name"] = _child.get_u_name();
                }
                context.Session["user_name"] = _users.get_u_name();
                context.Session[_users.get_u_name() + "lottery_session_user_info"] = _session;
                PageBase.SetAppcationFlag(loginName);
                if (FileCacheHelper.get_RedisStatOnline().Equals(1) || FileCacheHelper.get_RedisStatOnline().Equals(2))
                {
                    bool flag4 = false;
                    if ((_session.get_users_child_session() != null) && _session.get_users_child_session().get_is_admin().Equals(1))
                    {
                        flag4 = true;
                    }
                    if (!flag4)
                    {
//                        if (FileCacheHelper.get_RedisStatOnline().Equals(1))
//                        {
//                            new PageBase_Redis().InitUserOnlineTopToRedis(str5, _session.get_u_type());
//                        }
//                        if (FileCacheHelper.get_RedisStatOnline().Equals(2))
//                        {
//                            new PageBase_Redis().InitUserOnlineTopToRedisStack(str5, _session.get_u_type());
//                        }
                    }
                }
                else
                {
                    MemberPageBase.stat_top_online(loginName);
                    MemberPageBase.stat_online(loginName, _session.get_u_type());
                }
                if (FileCacheHelper.get_RedisStatOnline() == 0)
                {
                    PageBase.ZeroIsOutFlag(loginName);
                }
                CallBLL.cz_user_psw_err_log_bll.ZeroErrTimes(loginName);
                cz_login_log _log = new cz_login_log();
                _log.set_ip(LSRequest.GetIP());
                _log.set_login_time(new DateTime?(DateTime.Now));
                _log.set_u_name(loginName);
                PageBase base2 = new PageBase();
//                _log.set_browser_type(Utils.GetBrowserInfo(HttpContext.Current));
                bool flag5 = CallBLL.CzLoginLogService.Add(_log);
                if (_child == null)
                {
                    str14 = _users.get_is_changed().ToString();
                    if (string.IsNullOrEmpty(str14))
                    {
                        result.set_success(550);
                        result.set_tipinfo("新密碼首次登錄,需重置密碼!");
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session["modifypassword"] = "【首次登錄，重置密碼】";
                    }
                    else if (str14 == "0")
                    {
                        result.set_success(550);
                        result.set_tipinfo("新密碼首次登錄,需重置密碼!");
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session["modifypassword"] = "【重置上級修改的密碼】";
                    }
                    else
                    {
                        nullable = _users.get_last_changedate();
                        num2 = PageBase.PasswordExpire();
                        nullable3 = nullable;
                        time2 = DateTime.Now.AddDays((double) -num2);
                        if (nullable3.HasValue ? (nullable3.GetValueOrDefault() < time2) : false)
                        {
                            result.set_success(550);
                            result.set_tipinfo("密碼過期,需重置密碼!");
                            strResult = JsonHandle.ObjectToJson(result);
                            context.Session["modifypassword"] = "【密碼過期，重置密碼】";
                        }
                    }
                }
                else
                {
                    str14 = _child.get_is_changed().ToString();
                    if (string.IsNullOrEmpty(str14))
                    {
                        result.set_success(550);
                        result.set_tipinfo("新密碼首次登錄,需重置密碼!");
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session["modifypassword"] = "【首次登錄，重置密碼】";
                    }
                    else if (str14 == "0")
                    {
                        result.set_success(550);
                        result.set_tipinfo("新密碼首次登錄,需重置密碼!");
                        strResult = JsonHandle.ObjectToJson(result);
                        context.Session["modifypassword"] = "【重置上級修改的密碼】";
                    }
                    else
                    {
                        nullable = _child.get_last_changedate();
                        num2 = PageBase.PasswordExpire();
                        if (nullable.HasValue && ((nullable3 = nullable).HasValue ? (nullable3.GetValueOrDefault() < (time2 = DateTime.Now.AddDays((double) -num2))) : false))
                        {
                            result.set_success(550);
                            result.set_tipinfo("密碼過期,需重置密碼!");
                            strResult = JsonHandle.ObjectToJson(result);
                            context.Session["modifypassword"] = "【密碼過期，重置密碼】";
                        }
                    }
                }
            }
        }

        private string UserStatus(string status)
        {
            ReturnResult result = new ReturnResult();
            string str = "";
            if (status != "0")
            {
                if (status == "1")
                {
                    result.set_success(400);
                    result.set_tipinfo(PageBase.GetMessageByCache("u100004", "MessageHint"));
                    str = JsonHandle.ObjectToJson(result);
                }
                if (status == "2")
                {
                    result.set_success(400);
                    result.set_tipinfo(PageBase.GetMessageByCache("u100005", "MessageHint"));
                    str = JsonHandle.ObjectToJson(result);
                }
            }
            return str;
        }

        public bool IsReusable =>
            false;
    }
}

