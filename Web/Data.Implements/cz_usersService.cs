using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_usersService : BaseService<cz_users>, Icz_usersService
    {
        #region 构造方法

        public cz_usersService() : base() { }

        public cz_usersService(string language) : base(language) { }

        #endregion

        public cz_users AgentLogin(string toLower)
        {
            IList<cz_users> listByWhere = GetListByWhere(String.Format("u_name = '{0}'" , toLower));
            if (listByWhere.Count > 0)
            {
                return listByWhere[0];
            }

            return null;
        }

        public cz_users GetZJInfo()
        {
            IList<cz_users> listByWhere = GetListByWhere("u_type = 'zj'");
            if (listByWhere.Count > 0)
            {
                return listByWhere[0];
            }

            return null;
        }

        public int UpUserPwd(string name, string psw, string ramSalt)
        {
            string sql = string.Format("u_psw = '{0}',salt = '{1}',is_changed = 1", psw,ramSalt);
            int updateFields = UpdateFields(sql,string.Format("u_name = '{0}'",name));
            return updateFields;
        }

        public int UpdateUserPwdStutas(string name)
        {
            int updateFields = UpdateFields("a_state = 0",string.Format("u_name = '{0}'",name));
            return updateFields;
        }

        public cz_users GetUserInfoByUName(string str)
        {
            throw new NotImplementedException();
        }

//        查询所有上级
        public IList<cz_users> upperUsers(string u_name)
        {
            string stmtId = "upperUsers";
            stmtId = String.Format("{0}.{1}", this.stmtPrefix, stmtId);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("u_name", u_name);
            IList<cz_users> resultList = DbHelper.Instance.DataMapper.QueryForList<cz_users>(stmtId, parameters);
            return resultList;
        }
    }
}
