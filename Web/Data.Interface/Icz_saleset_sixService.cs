using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_saleset_sixService : IBaseService<cz_saleset_six>
    {
        DataTable GetSaleSetUser();
    }
}
