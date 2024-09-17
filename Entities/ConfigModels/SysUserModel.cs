using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ConfigModels
{
    public class SysUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string UserUnderList { get; set; }
        public IEnumerable<PermissionData> Permissions { get; set; }
    }

    public class PermissionData
    {
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public int PermissionId { get; set; }
    }
}
