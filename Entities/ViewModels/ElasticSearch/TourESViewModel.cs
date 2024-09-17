using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.ElasticSearch
{
    public class TourESViewModel
    {
        public long id { get; set; }
        public string tourname { get; set; }
        public string servicecode { get; set; }
        public string organizingtypename { get; set; }
        public string tourtypename { get; set; }
        public string tourtype { get; set; }
        public string startpoint1 { get; set; }
        public string startpoint2 { get; set; }
        public string startpoint3 { get; set; }
        public string supplierid { get; set; }
        public string fullname { get; set; }
        public string datedeparture { get; set; }
        public string groupendpoint { get; set; }
        public DateTime createddate { get; set; }
    }
}
