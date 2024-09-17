using Entities.Models;
using System;
using System.Collections.Generic;
using Utilities.Contants;

namespace Entities.ViewModels.Invoice
{
    public class InvoiceRequestViewModel : InvoiceRequest
    {
        public string UserName { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceCode { get; set; }
        public long InvoiceId { get; set; }
        public long InvoiceRequestId { get; set; }
        public long InvoiceDetailId { get; set; }
        public string InvoiceRequestStatus { get; set; }
        public string ProductName { get; set; }
        public string UserVerifyName { get; set; }
        public string OrderNo { get; set; }
        public string ClientName { get; set; }
        public string SalerName { get; set; }
        public string UserCreateName { get; set; }
        public string UserUpdateName { get; set; }
        public decimal? TotalPrice { get; set; }
        public double? Price { get; set; }
        public double? PriceExtra { get; set; }
        public double? PriceExtraExport { get; set; }
        public decimal TotalPriceVAT { get; set; }
        public decimal OrderAmount { get; set; }
        public double? VAT { get; set; }
        public decimal? VATAmount { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public bool IsChecked { get; set; }
        public DateTime? ExportDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateStr
        {
            get
            {
                if (StartDate != null) return StartDate.Value.ToString("dd/MM/yyyy");
                return string.Empty;
            }
        }
        public DateTime? EndDate { get; set; }
        public string EndDateStr
        {
            get
            {
                if (EndDate != null) return EndDate.Value.ToString("dd/MM/yyyy");
                return string.Empty;
            }
        }
        public int TotalRow { get; set; }
        public string PlanDateStr { get; set; }
        public string PlanDateViewStr
        {
            get
            {
                if (PlanDate != null) return PlanDate.Value.ToString("dd/MM/yyyy");
                return string.Empty;
            }
        }
       
        public int isSend { get; set; }
        public long InvoiceRequestDetailId { get; set; }
        public List<AttachFile> AttachFiles { get; set; }
        public List<InvoiceRequestDetailViewModel> InvoiceRequestDetails { get; set; }
        public IEnumerable<string> OtherImages { get; set; }

    }
}
