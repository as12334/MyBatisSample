using System;
using System.Collections.Generic;
using System.Text;
using BuilderDALSQL;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_user_psw_err_logService : BaseService<cz_user_psw_err_log>, Icz_user_psw_err_logService
    {
        #region 构造方法

        public cz_user_psw_err_logService() : base() { }

        public cz_user_psw_err_logService(string language) : base(language) { }

        #endregion

        public bool IsExistUser(string loginName)
        {
            IList<cz_user_psw_err_log> czUserPswErrLogs = GetListByWhere(String.Format(" u_name = '{0}'", loginName));
            if (czUserPswErrLogs.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void ZeroErrTimes(string loginName)
        {
            UpdateFields(string.Format("err_times = 0,update_date = '{0}'",DateTime.Now), String.Format("u_name = '{0}'",loginName));
        }

        public void UpdateErrTimes(string loginName)
        {
            UpdateFields(string.Format("err_times = err_times + 1,update_date = '{0}'",DateTime.Now), String.Format("u_name = '{0}'",loginName));
        }

        public void AddUser(string loginName)
        {
            cz_user_psw_err_log czUserPswErrLog = new cz_user_psw_err_log();
            czUserPswErrLog.set_u_name(loginName);
            czUserPswErrLog.set_err_times(1);
            czUserPswErrLog.set_update_date(DateTime.Now);
            Insert(czUserPswErrLog);
        }
    }
}
