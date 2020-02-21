using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_system_set_sixService : BaseService<cz_system_set_six>, Icz_system_set_sixService
    {
        #region 构造方法

        public cz_system_set_sixService() : base() { }

        public cz_system_set_sixService(string language) : base(language) { }

        #endregion

        public cz_system_set_six GetModel(int i)
        {
            throw new NotImplementedException();
        }
    }
}
