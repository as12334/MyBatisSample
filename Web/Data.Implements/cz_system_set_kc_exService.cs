using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_system_set_kc_exService : BaseService<cz_system_set_kc_ex>, Icz_system_set_kc_exService
    {
        #region 构造方法

        public cz_system_set_kc_exService() : base() { }

        public cz_system_set_kc_exService(string language) : base(language) { }

        #endregion

        public cz_system_set_kc_ex GetModel(int i)
        {
            throw new NotImplementedException();
        }
    }
}
