using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using LotterySystem.DBUtility;

namespace Data.Interface
{
    using Entity;
    public interface Icz_stat_onlineService : IBaseService<cz_stat_online>
    {
        void executte_sql(string str, SqlParameter[] parameterArray);
        void executte_sql(List<CommandInfo> list);
        DataTable query_sql(string str, SqlParameter[] parameterArray);
    }
}
