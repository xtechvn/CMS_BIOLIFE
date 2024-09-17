using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels
{
    public class GroupProductViewModel
    {
        public int id { get; set; } // khoa chinh
        public string name { get; set; } //ten nhom
        public int parentId { get; set; } // id cha
        public DateTime create_date { get; set; }
        public int order_no { get; set; } // vi tri sap xep
    }

    public class GroupProductDetailModel : GroupProduct
    {
    }

    public class GroupProductUpsertModel : GroupProduct
    {
        public string imageSize { get; set; }
        public string ImageBase64 { get; set; }
    }
}
