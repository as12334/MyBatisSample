using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Implements
{
    using Entity;
    using Data.Interface;
    using Data.Implements;
    using Business.Interface;
    public class cz_login_logLottery : BaseLottery<cz_login_log>, Icz_login_logLottery
    {
		#region 属性注入与构造方法
		
        private Icz_login_logService service;

        public cz_login_logLottery()
        {
            this.service = new cz_login_logService();
            base.BaseService = this.service;
        }

        public cz_login_logLottery(string language)
        {
            this.service = new cz_login_logService(language);
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
