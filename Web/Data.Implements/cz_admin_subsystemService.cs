using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_admin_subsystemService : BaseService<cz_admin_subsystem>, Icz_admin_subsystemService
    {
        #region 构造方法

        public cz_admin_subsystemService() : base() { }

        public cz_admin_subsystemService(string language) : base(language) { }

        #endregion

        public cz_admin_subsystem GetModel()
        {
            throw new NotImplementedException();
        }
    }
}
