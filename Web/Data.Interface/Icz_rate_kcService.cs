using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_rate_kcService : IBaseService<cz_rate_kc>
    {
        cz_rate_kc GetRateKCByUserName(string getUName);
        DataTable GetUserOpOdds(string getUName);
        DataTable UpperLowerLevels(string uName, string sUtype, string sUname);
    }
}
