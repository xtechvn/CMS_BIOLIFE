using Entities.ConfigModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels
{
    public class UserRoleCacheModel
    {
        public IEnumerable<PermissionData> Permission { get; set; }
        public string UserUnderList { get; set; }
        public string Role { get; set; }
    }
}
