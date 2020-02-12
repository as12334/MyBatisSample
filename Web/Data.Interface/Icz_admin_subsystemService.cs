using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_admin_subsystemService : IBaseService<cz_admin_subsystem>
    {
        cz_admin_subsystem GetModel();
    }
}
