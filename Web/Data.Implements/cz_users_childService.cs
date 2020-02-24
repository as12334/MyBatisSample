using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;

    public class cz_users_childService : BaseService<cz_users_child>, Icz_users_childService
    {
        #region 构造方法

        public cz_users_childService() : base()
        {
        }

        public cz_users_childService(string language) : base(language)
        {
        }

        #endregion

        public cz_users_child AgentLogin(string toLower)
        {
            IList<cz_users_child> listByWhere = GetListByWhere(String.Format("u_name = '{0}'",toLower));
            if (listByWhere.Count > 0)
            {
                return listByWhere[0];
            }

            return null;
        }

        public int UpUserPwd(string name, string psw, string ramSalt)
        {
            string sql = string.Format("u_psw = '{0}',salt = '{1}'", psw,ramSalt);
            int updateFields = UpdateFields(sql,string.Format("u_name = '{0}'",name));
            return updateFields;
        }

        public int UpdateUserPwdStutas(string name)
        {
            int updateFields = UpdateFields("status = 0",string.Format("u_name = '{0}'",name));
            return updateFields;
        }

        public string GetPermissionsName(string toString)
        {
            
            throw new NotImplementedException();
        }

        public int UpdateSkin(string text2, string text)
        {
            throw new NotImplementedException();
        }
    }
}