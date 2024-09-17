using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.ElasticSearch
{
    public class UserESViewModel
    {
        public long _id { get; set; } // ID ElasticSearch
        public long id { get; set; } // ID customer
        public string username { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
