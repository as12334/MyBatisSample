using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class AccountService : BaseService<Account>, IAccountService
    {
        #region ���췽��

        public AccountService() : base() { }

        public AccountService(string language) : base(language) { }

        #endregion
    }
}
