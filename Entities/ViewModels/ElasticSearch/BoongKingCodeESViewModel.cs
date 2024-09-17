using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.ElasticSearch
{
  public class BoongKingCodeESViewModel
    {
        public int id { get; set; }
        public int orderid { get; set; }
        public int serviceid { get; set; }

        public string bookingcode { get; set; }
        public string listorderid { get; set; }
        public bool isdelete { get; set; }
  
      
    }
}
