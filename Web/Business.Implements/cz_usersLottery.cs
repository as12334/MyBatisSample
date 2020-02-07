using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Implements
{
    using Entity;
    using Data.Interface;
    using Data.Implements;
    using Business.Interface;
    public class cz_usersLottery : BaseLottery<cz_users>, Icz_usersLottery
    {
		#region 属性注入与构造方法
		
        private Icz_usersService service;

        public cz_usersLottery()
        {
            this.service = new cz_usersService();
            base.BaseService = this.service;
        }

        public cz_usersLottery(string language)
        {
            this.service = new cz_usersService(language);
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
