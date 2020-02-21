using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_jp_oddsService : BaseService<cz_jp_odds>, Icz_jp_oddsService
    {
        #region 构造方法

        public cz_jp_oddsService() : base() { }

        public cz_jp_oddsService(string language) : base(language) { }

        #endregion

        public DataTable GetTableInfo(string text2, string text)
        {
            throw new NotImplementedException();
        }
    }
}
