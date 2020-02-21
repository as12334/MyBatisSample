using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_bet_kcService : BaseService<cz_bet_kc>, Icz_bet_kcService
    {
        #region 构造方法

        public cz_bet_kcService() : base() { }

        public cz_bet_kcService(string language) : base(language) { }

        #endregion

        public DataTable GetNewBet(string text2, string text4, string text5, string text3)
        {
            throw new NotImplementedException();
        }
    }
}
