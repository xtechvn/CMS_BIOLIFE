using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels
{
    public class CategoryViewModel
    {
        public string name { get; set; }
        public int cate_id { get; set; }
        public int parent_id { get; set; }
        public string path { get; set; }
    }
}
