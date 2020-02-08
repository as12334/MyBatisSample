using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_user_psw_err_logService : IBaseService<cz_user_psw_err_log>
    {
        bool IsExistUser(string loginName);
        void UpdateErrTimes(string loginName);
        void AddUser(string loginName);
        void ZeroErrTimes(string loginName);
    }
}
