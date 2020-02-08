using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_rate_kcBLL : BaseBLL<cz_rate_kc>, Icz_rate_kcBLL
    {
        #region 构造方法

        public cz_rate_kcBLL() : base() { }

        public cz_rate_kcBLL(string language) : base(language) { }

        #endregion
    }
}
