using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_usersService : BaseService<cz_users>, Icz_usersService
    {
        #region 构造方法

        public cz_usersService() : base() { }

        public cz_usersService(string language) : base(language) { }

        #endregion
    }
}
