using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_jp_oddsService : IBaseService<cz_jp_odds>
    {
        DataTable GetTableInfo(string text2, string text);
    }
}
