using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_saleset_sixService : BaseService<cz_saleset_six>, Icz_saleset_sixService
    {
        #region 构造方法

        public cz_saleset_sixService() : base() { }

        public cz_saleset_sixService(string language) : base(language) { }

        #endregion

        public DataTable GetSaleSetUser()
        {
            throw new NotImplementedException();
        }
    }
}
