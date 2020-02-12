using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class zk_subsysService : BaseService<zk_subsys>, Izk_subsysService
    {
        #region 构造方法

        public zk_subsysService() : base() { }

        public zk_subsysService(string language) : base(language) { }

        #endregion
    }
}
