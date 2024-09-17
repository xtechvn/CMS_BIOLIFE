using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Policy
{
    public class PolicyViewModel
    {
        public string Create_Name { get; set; }
        public string PermissionType_Name { get; set; }
        public int PolicyId { get; set; }
        public string PolicyName { get; set; }
        public string EffectiveDate { get; set; }
        public int PermissionType { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string PolicyCode { get; set; }

    }
    public class PolicySearchViewModel
    {
        public string Policy_Id { get; set; }
        public string PolicyName { get; set; }
        public DateTime? EffectiveDateFrom { get; set; }//Ngày hiệu lực từ--
        public DateTime? EffectiveDateTo { get; set; }//Ngày hiệu lực đến--
        public DateTime? CreateDateFrom { get; set; }//Ngày tạo từ--
        public DateTime? CreateDateTo { get; set; }//Ngày tạo đến--
        public string PermissionType { get; set; }//Ngày tạo đến--
        public string UserCreate { get; set; }//Ngày tạo đến--
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
     
    }
}
