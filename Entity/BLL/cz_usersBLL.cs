

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Application.DBUtility;
using Data.Implements;
using Web;

namespace LotterySystem.BLL
{
    using LotterySystem.Model;
    public class cz_usersBLL : NpSqlBase
    {
        public cz_users AgentLogin(string userName)
        {
            var czUserService = new Cz_userService();
            czUserService.GetAllList();
            BaseDA.Get<cz_users>("SelectById", 1);
            var executeQuery = ExecuteQuery($"SELECT * FROM cz_users WHERE u_name = '{userName}'");
            List<cz_users> tableToEntity = TableToEntity<cz_users>(executeQuery.Tables[0]);
            return tableToEntity[0];
        }

        public DataTable GetZJInfo()
        {
            throw new System.NotImplementedException();
        }
        
    }
}