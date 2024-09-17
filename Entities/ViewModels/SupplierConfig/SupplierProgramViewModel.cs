using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.SupplierConfig
{
    public class SupplierProgramSearchModel
    {
        public string ProgramCode { get; set; }
        public string Description { get; set; }
        public string ProgramStatus { get; set; }
        public string ServiceType { get; set; }
        public int SupplierID { get; set; }
        public int? ClientId { get; set; }
        public string StartDateFrom { get; set; }
        public string StartDateTo { get; set; }
        public string EndDateFrom { get; set; }
        public string EndDateTo { get; set; }
        public string UserCreate { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
        public string UserVerify { get; set; }
        public DateTime? VerifyDateFrom { get; set; }
        public DateTime? VerifyDateTo { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class SupplierProgramGridViewModel
    {
        public long ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ServiceType { get; set; }
        public string ServiceTypeName { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public int SupplierId { get; set; }
        public int ProgramStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserVerifyName { get; set; }
        public int TotalRow { get; set; }
    }
}
