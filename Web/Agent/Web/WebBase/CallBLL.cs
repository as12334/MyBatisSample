
using LotterySystem.BLL;

namespace Agent.Web.WebBase
{
    public class CallBLL
    {
        public static cz_user_psw_err_logBLL cz_user_psw_err_log_bll;
        public static cz_usersBLL cz_users_bll;
        public static cz_users_childBLL cz_users_child_bll;
        public static cz_admin_sysconfigBLL cz_admin_sysconfig_bll;
//        public static cz_rate_sixBLL cz_rate_six_bll;
        public static cz_rate_kcBLL cz_rate_kc_bll;
        public static cz_login_logBLL cz_login_log_bll;
        public static cz_phase_kl10BLL cz_phase_kl10_bll;
        public static cz_lotteryBLL cz_lottery_bll;
        public static cz_stat_onlineBLL cz_stat_online_bll;
        public static cz_stat_top_onlineBLL cz_stat_top_online_bll;

//        public static HashSet<string> redisHelper;
        public static object redisHelper { get; set; }

        public static void Call()
        {
            if (cz_user_psw_err_log_bll == null)
            {
                cz_user_psw_err_log_bll = new cz_user_psw_err_logBLL();
            }
            if (cz_users_bll == null)
            {
                cz_users_bll = new cz_usersBLL();
            }
            
        }
    }
}