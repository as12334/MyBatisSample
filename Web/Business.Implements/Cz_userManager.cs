using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Implements
{
    using Entity;
    using Data.Interface;
    using Data.Implements;
    using Business.Interface;
    public class Cz_userManager : BaseManager<Cz_user>, ICz_userManager
    {
		#region 属性注入与构造方法
		
        private ICz_userService service;

        public Cz_userManager()
        {
            this.service = new Cz_userService();
            base.BaseService = this.service;
        }

        public Cz_userManager(string language)
        {
            this.service = new Cz_userService(language);
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
