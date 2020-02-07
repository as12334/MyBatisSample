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
    }
}
