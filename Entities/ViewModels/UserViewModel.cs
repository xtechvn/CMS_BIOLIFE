using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.ViewModels
{
    public class UserViewModel : User
    {
        public string RoleId { get; set; }
        public string BirthDayPicker { get; set; }
        public string CompanyType { get; set; }
        public string OldCompanyType { get; set; }
    }

    public class UserDataViewModel : User
    {
        public string DepartmentName{ get; set; }
        public string UserPositionName{ get; set; }
        public List<int> RoleIdList { get; set; }
    }

    public class UserDetailViewModel
    {
        public User Entity { get; set; }
        public List<int> RoleIdList { get; set; }
    }

    public class UserGridModel : User
    {
        public List<Role> RoleList { get; set; }
        public Entities.Models.Department UserDepartment { get; set; }
        public Entities.Models.UserPosition UserPosition { get; set; }
    }

    public class AccountModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class UserPasswordModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}
