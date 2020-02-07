using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Implements
{
    using Entity;
    using Data.Interface;
    using Data.Implements;
    using Business.Interface;
    public class cz_rate_kcLottery : BaseLottery<cz_rate_kc>, Icz_rate_kcLottery
    {
		#region 属性注入与构造方法
		
        private Icz_rate_kcService service;

        public cz_rate_kcLottery()
        {
            this.service = new cz_rate_kcService();
            base.BaseService = this.service;
        }

        public cz_rate_kcLottery(string language)
        {
            this.service = new cz_rate_kcService(language);
            base.BaseService = this.service;
        }
        
        #endregion
    }
}
