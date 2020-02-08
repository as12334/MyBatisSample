using System;
using System.Collections.Generic;
using System.Text;

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
            //todo 未完成
            return false;
        }

        public void ZeroErrTimes(string loginName)
        {
            throw new NotImplementedException();
        }

        public void UpdateErrTimes(string loginName)
        {
            throw new NotImplementedException();
        }

        public void AddUser(string loginName)
        {
            throw new NotImplementedException();
        }
    }
}
