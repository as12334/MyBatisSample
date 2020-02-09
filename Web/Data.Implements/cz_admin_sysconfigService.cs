using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class cz_admin_sysconfigService : BaseService<cz_admin_sysconfig>, Icz_admin_sysconfigService
    {
        #region 构造方法

        public cz_admin_sysconfigService() : base() { }

        public cz_admin_sysconfigService(string language) : base(language) { }

        #endregion

        public DataRow GetItem()
        {
            DataSet dataSet2 = GetAllDataSet();
            if(dataSet2 != null && dataSet2.Tables[0].Rows.Count > 0)
            {
                return dataSet2.Tables[0].Rows[0];
            }
            return null;
        }
    }
}
