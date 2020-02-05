using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Implements
{
    using Entity;
    using Data.Interface;
    using Data.Implements;
    using Business.Interface;
    public class AccountManager : BaseManager<Account>, IAccountManager
    {
		#region 属性注入与构造方法
		
        private IAccountService service;

        public AccountManager()
        {
            this.service = new AccountService();
            base.BaseService = this.service;
        }

        public AccountManager(string language)
        {
            this.service = new AccountService(language);
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
