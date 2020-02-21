using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_usersService : IBaseService<cz_users>
    {
        IList<cz_users> upperUsers(string getParentUName);
        cz_users AgentLogin(string toLower);
        cz_users GetZJInfo();
        int UpUserPwd(string toString, string str8, string ramSalt);
        int UpdateUserPwdStutas(string toString);
        cz_users GetUserInfoByUName(string str);
        cz_users GetUserInfoByUID(string text);
    }
}
