using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_lotteryService : BaseService<cz_lottery>, Icz_lotteryService
    {
        #region 构造方法

        public cz_lotteryService() : base() { }

        public cz_lotteryService(string language) : base(language) { }

        #endregion

        public DataSet GetList()
        {
           return GetAllDataSet(); 
        }
    }
}
