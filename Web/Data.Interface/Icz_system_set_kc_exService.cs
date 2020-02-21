using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_system_set_kc_exService : IBaseService<cz_system_set_kc_ex>
    {
        cz_system_set_kc_ex GetModel(int i);
    }
}
