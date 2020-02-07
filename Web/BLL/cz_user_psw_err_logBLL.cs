using System.Data;
using Data.Implements;
using Entity;

namespace LotterySystem.BLL
{
    public class cz_user_psw_err_logBLL 
    {
        
        public bool IsExistUser(string str5)
        {
            var czUsers = new cz_users();
            czUsers.set_u_id("dafsdfa");
            
            var czUsersService = new cz_usersService();
            czUsersService.Insert(czUsers);
            czUsersService.GetRowCount();
//            var executeQuery = ExecuteQuery($"SELECT * FROM cz_user_psw_err_log WHERE u_name = '{str5}'");
//            if (executeQuery.Tables[0].Rows.Count == 0)
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }
            return false;
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