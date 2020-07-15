using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_bet_kcService : IBaseService<cz_bet_kc>
    {
        DataTable GetNewBet(string text2, string text4, string text5, string text3);
    }
}
