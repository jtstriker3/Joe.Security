using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joe.Security
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class SecurityAttribute : Attribute, Joe.Security.ISecurityRoles
    {
        public virtual String CreateRoles { get; set; }
        public virtual String ReadRoles { get; set; }
        public virtual String UpdateRoles { get; set; }
        public virtual String DeleteRoles { get; set; }
        public virtual String AllRoles { get; set; }

        public String[] GetCreateRolesArray()
        {
            return (CreateRoles ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public String[] GetReadRolesArray()
        {
            return (ReadRoles ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public String[] GetUpdateRolesArray()
        {
            return (UpdateRoles ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public String[] GetDeleteRolesArray()
        {
            return (DeleteRoles ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public String[] GetAllRolesArray()
        {
            return (AllRoles ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    /// <summary>
    /// Use this When Specifying and Application Configuration Area Grab Groups From
    /// **Must have SecurityConfiguration Group Set up in (web/app).config**
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class AreaSecurityAtrribute : Attribute, Joe.Security.ISecurityRoles
    {
        public IEnumerable<String> AreaNames { get; private set; }

        public AreaSecurityAtrribute(params String[] areas)
        {
            AreaNames = areas;

            this.SetAreas(areas);
        }


        private void SetAreas(IEnumerable<String> areas)
        {
            foreach (var area in areas)
            {
                var securityArea = SecurityConfiguration.GetInstance().ApplicationAreas.Cast<ApplicationArea>().SingleOrDefault(appArea => appArea.Name == area);
                if (securityArea != null)
                {
                    if (String.IsNullOrWhiteSpace(AllRoles))
                        AllRoles = securityArea.AllRoles;
                    else
                        AllRoles += "," + securityArea.AllRoles;

                    if (String.IsNullOrWhiteSpace(CreateRoles))
                        CreateRoles = securityArea.CreateRoles;
                    else
                        CreateRoles += "," + securityArea.CreateRoles;

                    if (String.IsNullOrWhiteSpace(ReadRoles))
                        ReadRoles = securityArea.ReadRoles;
                    else
                        ReadRoles += "," + securityArea.ReadRoles;

                    if (String.IsNullOrWhiteSpace(UpdateRoles))
                        UpdateRoles = securityArea.UpdateRoles;
                    else
                        UpdateRoles += "," + securityArea.UpdateRoles;

                    if (String.IsNullOrWhiteSpace(DeleteRoles))
                        DeleteRoles = securityArea.DeleteRoles;
                    else
                        DeleteRoles += "," + securityArea.DeleteRoles;
                }
            }
        }

        public string AllRoles { get; set; }

        public string CreateRoles { get; set; }

        public string ReadRoles { get; set; }

        public string UpdateRoles { get; set; }

        public string DeleteRoles { get; set; }

        public String[] GetCreateRolesArray()
        {
            return (CreateRoles ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public String[] GetReadRolesArray()
        {
            return (ReadRoles ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public String[] GetUpdateRolesArray()
        {
            return (UpdateRoles ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public String[] GetDeleteRolesArray()
        {
            return (DeleteRoles ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public String[] GetAllRolesArray()
        {
            return (AllRoles ?? String.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
