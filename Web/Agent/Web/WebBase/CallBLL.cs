
using Data.Implements;
using Data.Interface;
using LotterySystem.BLL;

namespace Agent.Web.WebBase
{
    public class CallBLL
    {
        public static Icz_user_psw_err_logService CzUserPswErrLogService;
        public static Icz_usersService CzUsersService;
        public static Icz_users_childService CzUsersChildService;
        public static cz_admin_sysconfigBLL cz_admin_sysconfig_bll;
//        public static cz_rate_sixBLL cz_rate_six_bll;
        public static Icz_rate_kcService CzRateKcService;
        public static Icz_login_logService CzLoginLogService;
        public static cz_phase_kl10BLL cz_phase_kl10_bll;
        public static cz_lotteryBLL cz_lottery_bll;
        public static cz_stat_onlineBLL cz_stat_online_bll;
        public static cz_stat_top_onlineBLL cz_stat_top_online_bll;

//        public static HashSet<string> redisHelper;
        public static object redisHelper { get; set; }

        public static void Call()
        {
            if (CzUserPswErrLogService == null)
            {
                CzUserPswErrLogService = new cz_user_psw_err_logService();
            }
            if (CzUsersService == null)
            {
                CzUsersService = new cz_usersService();
            }
            
            if (CzUsersChildService == null)
            {
                CzUsersChildService = new cz_users_childService();
            }
            
            if (CzRateKcService == null)
            {
                CzRateKcService = new cz_rate_kcService();
            }
            
            if (CzLoginLogService == null)
            {
                CzLoginLogService = new cz_login_logService();
            }
            
        }
    }
}