using System;
using System.Collections.Generic;
using Entities.Models;

namespace Entities.ViewModels.Invoice
{
    public class InvoiceViewModel : Models.Invoice
    {
        public List<string> OtherImages { get; set; }
        public List<string> AttachFiles { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? VATAmount { get; set; }
        public decimal TotalPriceVAT { get; set; }
        public double? PriceExtraExport { get; set; }
        public double? PriceExtra { get; set; }
        public string ClientName { get; set; }
        public string PayTypeStr { get; set; }
        public string AccountNumber { get; set; }
        public string BankId { get; set; }
        public string UserName { get; set; }
        public string UserCreateName { get; set; }
        public string UserVerifyName { get; set; }
        public string UserUpdateName { get; set; }
        public string UserCreateRequestName { get; set; }
        public string InvoiceRequestNo { get; set; }
        public string OrderNo { get; set; }
        public int InvoiceRequestId { get; set; }
        public string InvoiceRequestStatus { get; set; }
        public List<InvoiceRequest> RelateRequest { get; set; }
        public string ExportDateStr { get; set; }
        public string TaxNo { get; set; }
        public DateTime? PlanDate { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string ExportDateViewStr
        {
            get
            {
                if (ExportDate != null)
                    return ExportDate.Value.ToString("dd/MM/yyyy");
                return string.Empty;
            }
        }
        public int TotalRow { get; set; }
        public int VAT { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public double? PriceVat { get; set; }
        public List<InvoiceRequestDetailViewModel> InvoiceRequests { get; set; }
        public List<InvoiceDetailViewModel> InvoiceDetails { get; set; }
    }
}
