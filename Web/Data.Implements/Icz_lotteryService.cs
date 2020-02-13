using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_lotteryService : IBaseService<cz_lottery>
    {
        DataSet GetList();
    }
}
