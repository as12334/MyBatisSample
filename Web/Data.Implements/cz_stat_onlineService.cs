using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using BuilderDALSQL;
using LotterySystem.DBUtility;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_stat_onlineService : BaseService<cz_stat_online>, Icz_stat_onlineService
    {
        #region 构造方法

        public cz_stat_onlineService() : base() { }

        public cz_stat_onlineService(string language) : base(language) { }

        
        public DataTable query_sql(string str4)
        {
            DataSet dataSet = DbHelperSQL.Query(str4);
            return dataSet.Tables[0];
//            throw new System.NotImplementedException();
        }

        public void executte_sql(string str4)
        {
            DbHelperSQL.executte_sql(str4);
        }
        #endregion

        public void executte_sql(string str, SqlParameter[] parameterArray)
        {
           DbHelperSQL.executte_sql(str,parameterArray);
        }

        public void executte_sql(List<CommandInfo> list)
        {
            DbHelperSQL.ExecuteSqlTran(list);
        }

        public DataTable query_sql(string str, SqlParameter[] parameterArray)
        {
            DataSet dataSet = DbHelperSQL.Query(str,parameterArray);
            return dataSet.Tables[0];
        }
    }
}
