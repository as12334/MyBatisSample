using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Interface
{
    using Entity;
    public interface Icz_admin_sysconfigService : IBaseService<cz_admin_sysconfig>
    {
        DataRow GetItem();
    }
}
