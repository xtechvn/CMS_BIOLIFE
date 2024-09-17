using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.ApiSever
{
   public class ProductCategoryViewModel
    {
        public int id { get; set; }
        public int code { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string image { get; set; }
    }
}
