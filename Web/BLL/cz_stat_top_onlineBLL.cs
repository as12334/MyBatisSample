using System.Data;
using BuilderDALSQL;

namespace LotterySystem.BLL
{
    public class cz_stat_top_onlineBLL
    {
        public DataTable query_sql(string str4)
        {
            DataSet dataSet = DbHelperSQL.Query(str4);
            return dataSet.Tables[0];
//            throw new System.NotImplementedException();
        }

        public void executte_sql(string str4)
        {
            DbHelperSQL.ExecuteSql(str4);
        }
    }
}