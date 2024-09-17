using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels
{
    public class MenuViewModel
    {
        public Menu Parent { get; set; }
        public List<Menu> ChildList { get; set; }
    }

    public class MenuUpsertViewModel : Menu
    {
    }

    public class MenuPermissionModel
    {
        public int menu_id { get; set; }
        public IEnumerable<int> permission_ids { get; set; }
    }
}
