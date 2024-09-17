using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB.CMS.Models.Location
{
    public class LocationViewModel
    {
    }
    public class RegionModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string typename { get; set; }
        public string parentid { get; set; }
        public int type { get; set; }
    }
}
