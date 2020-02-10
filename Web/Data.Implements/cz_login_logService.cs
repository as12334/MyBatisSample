using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_login_logService : BaseService<cz_login_log>, Icz_login_logService
    {
        #region 构造方法

        public cz_login_logService() : base() { }

        public cz_login_logService(string language) : base(language) { }

        #endregion

        public bool Add(cz_login_log log)
        {
            int insert = Insert(log);
            if (insert > 0)
            {
                return true;
            }
            return false;
        }
    }
}
