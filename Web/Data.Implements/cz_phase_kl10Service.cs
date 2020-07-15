using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_phase_kl10Service : BaseService<cz_phase_kl10>, Icz_phase_kl10Service
    {
        #region 构造方法

        public cz_phase_kl10Service() : base() { }

        public cz_phase_kl10Service(string language) : base(language) { }

        #endregion
    }
}
