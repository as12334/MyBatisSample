using System.Collections.Generic;
using Application.DBUtility;
using LotterySystem.Model;

namespace LotterySystem.BLL
{
    public class cz_users_childBLL : NpSqlBase
    {
        public cz_users_child AgentLogin(string userName)
        {
            var executeQuery = ExecuteQuery($"SELECT * FROM cz_users WHERE u_name = '{userName}'");
            List<cz_users_child> tableToEntity = TableToEntity<cz_users_child>(executeQuery.Tables[0]);
            return tableToEntity[0];
        }


    }
}