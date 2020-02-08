using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_usersBLL : BaseBLL<cz_users>, Icz_usersBLL
    {
        #region 构造方法

        public cz_usersBLL() : base() { }

        public cz_usersBLL(string language) : base(language) { }

        #endregion
    }
}
