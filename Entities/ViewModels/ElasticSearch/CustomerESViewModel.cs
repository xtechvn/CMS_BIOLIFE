using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.ElasticSearch
{
    public class CustomerESViewModel
    {
        public long _id { get; set; } // ID ElasticSearch
        public long id { get; set; } // ID customer
        public string clientname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int clienttype { get; set; }
        public int userid { get; set; }
    }
    public class ClientESViewModel
    {
      
        public long _id { get; set; } // ID customer
        public string ClientName { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string Phone { get; set; }
        public DateTime JoinDate { get; set; }
        public int ClientType { get; set; }
        public string unix_timestamp { get; set; }
        public string suggest_search { get; set; }
        public string userid { get; set; }

    }
    public class earchClientESViewModel
    {

        public string clientname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string id { get; set; }
        public string clienttype { get; set; }
        public string userid { get; set; }

    }
}
