using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Implements
{
    using Entity;
    using Data.Interface;
    using Data.Implements;
    using Business.Interface;
    public class cz_user_psw_err_logLottery : BaseLottery<cz_user_psw_err_log>, Icz_user_psw_err_logLottery
    {
        #region 属性注入与构造方法
		
        private Icz_user_psw_err_logService service;

        public cz_user_psw_err_logLottery()
        {
            this.service = new cz_user_psw_err_logService();
            base.BaseService = this.service;
        }

        public cz_user_psw_err_logLottery(string language)
        {
            this.service = new cz_user_psw_err_logService(language);
            base.BaseService = this.service;
        }
        
        #endregion
    }
}