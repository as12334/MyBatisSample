using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_rate_sixService : IBaseService<cz_rate_six>
    {
        DataTable GetRateByUserID(string text);
    }
}
