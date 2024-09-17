using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.GroupProductModels
{
    public class MenuGroupProduct
    {
        public GroupProduct parent_menu { get; set; }
        public List<GroupProduct> child_menu{get;set;}
    }
}
