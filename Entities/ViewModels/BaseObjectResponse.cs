using Entities.ViewModels.ApiSever;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels
{
    public class BaseObjectResponse<T>
    {
        public int status { get; set; }
        public string msg { get; set; }
    }
    public class ObjectResponse<T>
    {
        public int status { get; set; }
        public string msg { get; set; }
        public List<ProductCategoryViewModel> data { get; set; }
    }
    public class BaseObjectResponseV2<T>
    {
        public int status { get; set; }
        public string msg { get; set; }
    } 
    public class BaseObjectQr
    {
        public int status { get; set; }
        public string barcode_image_url { get; set; }
        public string manual_entry_key { get; set; }
    }

}
