using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_system_set_sixService : IBaseService<cz_system_set_six>
    {
        cz_system_set_six GetModel(int i);
    }
}
