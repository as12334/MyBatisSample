using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BuilderDALSQL;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_stat_top_onlineService : BaseService<cz_stat_top_online>, Icz_stat_top_onlineService
    {
        #region 构造方法

        public cz_stat_top_onlineService() : base() { }

        public cz_stat_top_onlineService(string language) : base(language) { }

        #endregion

        public DataTable query_sql(string str4)
        {
            DataSet dataSet = DbHelperSQL.Query(str4);
            return dataSet.Tables[0];
        }

        public void executte_sql(string str4)
        {
            DbHelperSQL.executte_sql(str4);
        }
    }
}
