using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joe.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class SecurityAttribute : Attribute, Joe.Security.ISecurityRoles
    {
        public String CreateRoles { get; set; }
        public String ReadRoles { get; set; }
        public String UpdateRoles { get; set; }
        public String DeleteRoles { get; set; }
        public String AllRoles { get; set; }

        public String[] GetCreateRolesArray()
        {
            return (CreateRoles ?? String.Empty).Split(',');
        }
        public String[] GetReadRolesArray()
        {
            return (ReadRoles ?? String.Empty).Split(',');
        }
        public String[] GetUpdateRolesArray()
        {
            return (UpdateRoles ?? String.Empty).Split(',');
        }
        public String[] GetDeleteRolesArray()
        {
            return (DeleteRoles ?? String.Empty).Split(',');
        }
        public String[] GetAllRolesArray()
        {
            return (AllRoles ?? String.Empty).Split(',');
        }
    }
}
