using Entities.Models;
using System;

namespace Entities.ViewModels.Invoice
{
    public class InvoiceRequestDetailViewModel : InvoiceRequestDetail
    {
        public double? TotalPrice { get; set; }
        public double? TotalPriceVAT { get; set; }
        public double? VatAmount { get; set; }
        public string InvoiceRequestNo { get; set; }
        public string UserCreateName { get; set; }
        public string UserCreateRequestName { get; set; }
        public DateTime? PlanDate { get; set; }
        public string PlanDateViewStr
        {
            get
            {
                if (PlanDate != null)
                    return PlanDate.Value.ToString("dd/MM/yyyy");
                return string.Empty;
            }
        }
    }
}
