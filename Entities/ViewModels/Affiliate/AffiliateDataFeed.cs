using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.ViewModels.Affiliate
{
    [System.Serializable]
    public class AccesstradeDataFeed
    {
        public string sku { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public int price { get; set; }
        public int retail_Price { get; set; }
        public string url { get; set; }
        public string image_url { get; set; }
        public string category_id { get; set; }
        public string category_name { get; set; }

    }
    [System.Serializable]
    public class AdpiaDataFeed
    {
        public string product_name { get; set; }
        public string product_id { get; set; }
        public string category { get; set; }
        public double price { get; set; }
        public double discount { get; set; }
        public string link { get; set; }
        public string image { get; set; }
        public string url { get; set; }

    }
}
