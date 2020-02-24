using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_users_childService : IBaseService<cz_users_child>
    {
        cz_users_child AgentLogin(string toLower);
        int UpUserPwd(string toString, string psw, string ramSalt);
        int UpdateUserPwdStutas(string status);
        string GetPermissionsName(string toString);
        int UpdateSkin(string text2, string text);
    }
}
