using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Funding
{
    public class SupplierSearchModel
    {
        public string FullName { get; set; }
        public string ServiceType { get; set; }
        public string ProvinceId { get; set; }
        public string RatingStar { get; set; }
        public string ChainBrands { get; set; }
        public int? SalerId { get; set; }
        public int DepartmentId { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public List<int> CreateByIds { get; set; }
    }
}
