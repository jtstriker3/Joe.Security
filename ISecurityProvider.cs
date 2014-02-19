using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joe.Security
{
    public interface ISecurityProvider
    {
        Boolean IsUserInRole(params String[] roles);
        Boolean IsUserInRole(String userID, params String[] roles);
        String UserID { get; set; }
    }
}
