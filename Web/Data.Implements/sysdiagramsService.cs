using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Implements
{
    using Entity;
    using Data.Interface;
    public class sysdiagramsService : BaseService<sysdiagrams>, IsysdiagramsService
    {
        #region 构造方法

        public sysdiagramsService() : base() { }

        public sysdiagramsService(string language) : base(language) { }

        #endregion
    }
}
