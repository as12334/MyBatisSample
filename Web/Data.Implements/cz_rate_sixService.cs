using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_rate_sixService : BaseService<cz_rate_six>, Icz_rate_sixService
    {
        #region ���췽��

        public cz_rate_sixService() : base() { }

        public cz_rate_sixService(string language) : base(language) { }

        #endregion
    }
}
