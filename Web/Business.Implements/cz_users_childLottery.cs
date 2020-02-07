using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Implements
{
    using Entity;
    using Data.Interface;
    using Data.Implements;
    using Business.Interface;
    public class cz_users_childLottery : BaseLottery<cz_users_child>, Icz_users_childLottery
    {
		#region 属性注入与构造方法
		
        private Icz_users_childService service;

        public cz_users_childLottery()
        {
            this.service = new cz_users_childService();
            base.BaseService = this.service;
        }

        public cz_users_childLottery(string language)
        {
            this.service = new cz_users_childService(language);
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
