using System;
using System.Collections.Generic;
using System.Data;
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

        public cz_rate_kc GetRateKCByUserName(string getUName)
        {
            throw new NotImplementedException();
        }

        public DataTable GetUserOpOdds(string getUName)
        {
            throw new NotImplementedException();
        }

        public DataTable UpperLowerLevels(string uName, string sUtype, string sUname)
        {
            throw new NotImplementedException();
        }

        public DataTable GetRateByUserID(string text)
        {
            throw new NotImplementedException();
        }
    }
}
