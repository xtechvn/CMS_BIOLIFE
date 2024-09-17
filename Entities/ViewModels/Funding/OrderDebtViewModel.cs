using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels.Funding
{
    public class OrderDebtViewModel
    {
        public long ContractPayId { get; set; }
        public long OrderId { get; set; }
        public long ClientId { get; set; }
        public string OrderNo { get; set; }
        public string BillNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ClientName { get; set; }
        public string PayId { get; set; }
        public string PayBillNo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string DebtNote { get; set; }
        public decimal? Payment { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public string DebtStatusName { get; set; }
        public int StatusCode { get; set; }
        public int DebtStatus { get; set; }
        public DateTime? CreateTime { get; set; }
        public int OrderStatus { get; set; }
        public string CreateName { get; set; }
        public string UpdateName { get; set; }
        public DateTime? UpdateLast { get; set; }
        public string SalerName { get; set; }
        public string SalerUserName { get; set; }
        public string SalerEmail { get; set; }
        public long TotalRow { get; set; }
        public List<CountStatus> ContractPays { get; set; }
        public double AmountRemain { get; set; }
        public double TotalNeedPayment { get; set; }
        public double AmountOrder { get; set; }
        public int CreatedBy { get; set; }
        public List<OrderDebtViewModel> orderDebtViewModels { get; set; }
    }
}
