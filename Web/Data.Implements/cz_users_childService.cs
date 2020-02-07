using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_users_childService : BaseService<cz_users_child>, Icz_users_childService
    {
        #region 构造方法

        public cz_users_childService() : base() { }

        public cz_users_childService(string language) : base(language) { }

        #endregion
    }
}
