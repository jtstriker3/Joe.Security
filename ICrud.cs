using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joe.Security
{
    public interface ICrud
    {
        Boolean CanCreate { get; set; }
        Boolean CanRead { get; set; }
        Boolean CanUpdate { get; set; }
        Boolean CanDelete { get; set; }
    }
}
