using System;
using System.Collections.Generic;
using System.Text;

namespace ENTITIES.ViewModels.Articles
{
    public class ArticleGroupViewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string image_path { get; set; }
        public string url_path { get; set; }
        public int order_no { get; set; }

    }
    public class ProductGroupViewModel
    {
        public int id { get; set; }
        public int code { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string image { get; set; }

    }
}
