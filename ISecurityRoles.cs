using System;
namespace Joe.Security
{
    public interface ISecurityRoles
    {
        string AllRoles { get; set; }
        string CreateRoles { get; set; }
        string DeleteRoles { get; set; }
        string[] GetAllRolesArray();
        string[] GetCreateRolesArray();
        string[] GetDeleteRolesArray();
        string[] GetReadRolesArray();
        string[] GetUpdateRolesArray();
        string ReadRoles { get; set; }
        string UpdateRoles { get; set; }
    }
}
