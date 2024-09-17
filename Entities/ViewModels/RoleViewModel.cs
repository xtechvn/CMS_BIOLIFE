using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên nhóm quyền")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }

    public class RoleDataModel : Role
    {
        public int CountUser { get; set; }
    }

    public class RoleUserViewModel
    {
        public int RoleId { get; set; }
        public List<User> ListUser { get; set; }
    }
    public class RolePermissionViewModel
    {
        public int MenuId { get; set; }
        public int PermissionId { get; set; }
    }
}
