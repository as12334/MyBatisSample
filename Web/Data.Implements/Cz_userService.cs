using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class Cz_userService : BaseService<Cz_user>, ICz_userService
    {
        #region 构造方法

        public Cz_userService() : base() { }

        public Cz_userService(string language) : base(language) { }

        #endregion
    }
}
