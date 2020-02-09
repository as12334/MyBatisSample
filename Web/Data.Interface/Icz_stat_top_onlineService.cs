using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_stat_top_onlineService : IBaseService<cz_stat_top_online>
    {
        DataTable query_sql(string str4);
        void executte_sql(string str4);
    }
}
