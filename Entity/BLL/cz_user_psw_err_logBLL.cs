using System.Data;
using Application.DBUtility;
using Npgsql;

namespace LotterySystem.BLL
{
    public class cz_user_psw_err_logBLL : NpSqlBase
    {
        
        public bool IsExistUser(string str5)
        {
            var executeQuery = ExecuteQuery($"SELECT * FROM cz_user_psw_err_log WHERE u_name = '{str5}'");
            if (executeQuery.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void UpdateErrTimes(string str5)
        {
            throw new System.NotImplementedException();
        }

        public void AddUser(string str5)
        {
            throw new System.NotImplementedException();
        }

        public void ZeroErrTimes(string str5)
        {
            throw new System.NotImplementedException();
        }
    }
}