using Entities.Models;
using Entities.ViewModels.Funding;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ViewModels
{
    public class ContractPayViewModel : ContractPay
    {
        public string ClientName { get; set; }
        public string SupplierName { get; set; }
        public string EmployeeName { get; set; }
        public string ContractPayType { get; set; }
        public string PayDetail { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public int ContractPayId { get; set; }
        public long OrderId { get; set; }
        public string TypeStr { get; set; }
        public string PayTypeStr { get; set; }
        public string TransNo { get; set; }
        public List<long> DataIds { get; set; }
        public List<string> DataNo { get; set; }
        public List<CountStatus> DataContent { get; set; }
        public List<ContractPayDetailViewModel> RelateData { get; set; }
        public List<ContractPayViewModel> ContractPayDetail { get; set; }
        public string CreatedByName { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string OrderNo { get; set; }
        public double AmountRemain { get; set; }
        public double Payment { get; set; }
        public double AmountOrder { get; set; }
        public double TotalDeposit { get; set; }
        public double TotalPayment { get; set; }
        public double TotalNeedPayment { get; set; }
        public long TotalRow { get; set; }
        public long Total { get; set; }
        public string PayDetailId { get; set; }
        public double AmountPayDetail { get; set; }
        public IFormFile imagefile { get; set; }
        public string OrderCreateName { get; set; }
        public string PayIdDetail { get; set; }
        public int DataId { get; set; }
        public double ContractPayAmount { get; set; }
        public double TotalAmountPayDetail { get; set; }
        public byte? OrderStatus { get; set; }
        public double TotalAmountRemain { get; set; }
        public double AmountPayOrder { get; set; }
        public string ServiceName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string StatusTrans { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal AmountPay { get; set; }
        public string ServiceCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ServiceType { get; set; }
        public long ServiceId { get; set; }
        public long ServiceIdParent { get; set; }
        public string SalerName { get; set; }
        public string GroupBookingId { get; set; }
        public int DepositHistoryId { get; set; }
        public List<ContractPayDetailViewModel> ContractPayDetails { get; set; }
        public int PermisionType { get; set; }
    }

    public class ContractPayViewModelBK : ContractPay
    {
        public string ClientName { get; set; }
        public string ContractPayType { get; set; }
        public string PayDetail { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string TypeStr { get; set; }
        public string PayTypeStr { get; set; }
        public string CreatedByName { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public double TotalDeposit { get; set; }
        public long TotalRow { get; set; }
        public string PayDetailId { get; set; }
        public IFormFile imagefile { get; set; }
    }
}
