using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_rate_kcService : BaseService<cz_rate_kc>, Icz_rate_kcService
    {
        #region 构造方法

        public cz_rate_kcService() : base() { }

        public cz_rate_kcService(string language) : base(language) { }

        #endregion
    }
}
