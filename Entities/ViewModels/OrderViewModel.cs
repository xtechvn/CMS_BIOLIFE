using Entities.Models;
using Entities.ViewModels.Products;
using System;
using System.Collections.Generic;

namespace Entities.ViewModels
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public string OrderNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ClientName { get; set; }
        public long? ClientId { get; set; }
        public string ClientNumber { get; set; }
        public string ClientEmail { get; set; }
        public string Note { get; set; }
        public double Payment { get; set; }
        public double Amount { get; set; }
        public string UtmSource { get; set; }
        public double Profit { get; set; }
        //public List<Source> StatusDetail { get; set; } = new List<Source>();
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public int PayDetailId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreateName { get; set; }
        public string UpdateName { get; set; }
        public DateTime UpdateLast { get; set; }
        public string SalerName { get; set; }
        public string SalerUserName { get; set; }
        public string SalerEmail { get; set; }
        public string SalerGroupName { get; set; }
        public string PaymentTypeName { get; set; }
        public string PaymentStatus { get; set; }
        public double TotalDisarmed { get; set; }
        public double TotalAmount{ get; set; }
        public double TotalNeedPayment { get; set; }
        public string UsUpdateName { get; set; }
        public string CreatedName { get; set; }
        public string ServiceType { get; set; }
        public string Vouchercode { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
        public string PaymentStatusName { get; set; }
        public string PermisionTypeName { get; set; }
        public string OperatorIdName { get; set; }
        public string UtmMedium { get; set; }
        public int IsFinishPayment { get; set; }
        public string ListProductId { get; set; }
        public string OrderStatus { get; set; }
        public string OrderCode { get; set; }

        public List<ProductMongoDbModel> ListProduct { get; set; }
    }
    public class TotalValueOrder
    {
        public string TotalAmmount { get; set; }
        public string TotalProductService { get; set; }
        public string TotalDone { get; set; }
        public string TotalProfit { get; set; }

    }
    public class ProductServiceName
    {
        public string OrderId { get; set; }
        public string ServiceName { get; set; }
        public string StatusName { get; set; }      
        public string ServiceId { get; set; }      
        public string Note { get; set; }      
        public int Status { get; set; }      
        public string Type { get; set; }      
    }
    public class AllCodeData
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public List<AllCode> Data { get; set; }
    }
    public class Source
    {
        public int Key { get; set; }
        public int Value { get; set; }
    }
    public class FilterOrder
    {
        public TotalValueOrder TotalValueOrder { get; set; }
        public long Totalrecord { get; set; }
        public long TotalData { get; set; }
        public long Totalrecord1 { get; set; }
        public long Totalrecord2 { get; set; }
        public long Totalrecord3 { get; set; }
        public long Totalrecord4 { get; set; }
        public long TotalrecordErr { get; set; }
        public List<AllCode> SysTemType { get; set; }
        public List<AllCode> Source { get; set; }
        public List<AllCode> Type { get; set; }
        public List<AllCode> Customers { get; set; }
        public List<AllCode> Status { get; set; }
        public string[] SuggestOrder { get; set; }

    }
    public class OrderViewSearchModel
    {
        public int SysTemType { get; set; } = -1;
        public string PaymentStatus { get; set; }
        public string PermisionType { get; set; }
        public string[] HINHTHUCTT { get; set; }

        public string OrderNo { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
        public string Note { get; set; }
        public string UtmSource { get; set; }
        public List<int>? ServiceType { get; set; }
        public List<int>? Status { get; set; }
        public string CreateTime { get; set; }
        public string ToDateTime { get; set; }
        public string CreateName { get; set; }
        public string OperatorId { get; set; }
        public string Sale { get; set; }
        public string SaleGroup { get; set; }
        public string ClientId { get; set; }
        public string SalerPermission { get; set; }
        public string BoongKingCode { get; set; }
        public int StatusTab { get; set; } = 99;
        public int PageIndex { get; set; }
        public int pageSize { get; set; }
    }
    public class SearchOrder
    {
        public string Id { get; set; }
        public string Deposit_type { get; set; }
        public string ImageScreen { get; set; }
        public string UserVerifyId { get; set; }
        public string VerifyDate { get; set; }
        public string NoteReject { get; set; }
        public string OrderId { get; set; }
        public string OrderNo { get; set; }
        public string ServiceType { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string ClientId { get; set; }
        public string ContactClientId { get; set; }
        public string SmsContent { get; set; }
        public string PaymentType { get; set; }
        public string BankCode { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentNo { get; set; }
        public string ColorCode { get; set; }
        public string Discount { get; set; }
        public string Profit { get; set; }
        public string ExpriryDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ProductService { get; set; }
        public string Note { get; set; }
        public string UtmSource { get; set; }
        public string UpdateLast { get; set; }
        public string SalerId { get; set; }
        public string SalerGroupId { get; set; }
        public string UserUpdateId { get; set; }
        public string SystemType { get; set; }
        public string AccountClientId { get; set; }
        public string ContactClient { get; set; }
        public string Contract { get; set; }
        public object Passenger { get; set; }
    }
    public class SearchOrderModels
    {
        public string Status { get; set; }
        public string Msg { get; set; }
        public List<SearchOrder> Data { get; set; }
    }
    public class OrderDetailViewModel
    {
        public string OrderId { get; set; }
        public string OrderNo { get; set; }
        public string Label { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public double Amount { get; set; }
        public int Status { get; set; }
        public string PermisionTypeName { get; set; }
        public string FullName { get; set; }
        public string UserUpdateFullName { get; set; }
        public double AmountPay { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TaxNo { get; set; }
        public string BusinessAddress { get; set; }
        public double Id { get; set; }
        public long ClientId { get; set; }
        public long CreatedBy { get; set; }
        public int ContactClientId { get; set; }
        public int SalerId { get; set; }
        public DateTime? UpdateLast { get; set; }
        public long? UserUpdateId { get; set; }
        public long? AccountClientId { get; set; }
        public int? OrderStatus { get; set; }
        public string BankCode { get; set; }
        public string Note { get; set; }
        public short? BranchCode { get; set; }
        public int? PaymentStatus { get; set; }
        public byte? ServiceType { get; set; }
        public short? SystemType { get; set; }
        public string OrderStatusName { get; set; }
        public string PaymentStatusName { get; set; }
        public string SystemTypeName { get; set; }
        public string PaymentTypeName { get; set; }
        public DateTime PaymentDate { get; set; }
        public string SalerGroupId { get; set; }
        public string BranchCodeName { get; set; }
        public string code { get; set; }
        public long Discount { get; set; }
        public long Profit { get; set; }
        public long Refund { get; set; }
        public string Address { get; set; }
        public string WardName { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
    }
    public class OrderServiceViewModel
    {
        public string OrderId { get; set; }
        public string OrderNo { get; set; }
        public string ServiceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateTime { get; set; }
        public string Id { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public double Profit { get; set; }
        public double Refund { get; set; }
        public double Discount { get; set; }
        public double OrderAmount { get; set; }
        public string Type { get; set; }
        public string FullName { get; set; }
        public string code { get; set; }
        public string Note { get; set; }
        public string StatusName { get; set; }
        public string ServiceCode { get; set; }
        public int  Status { get; set; }

    }
    public class FieldOrder
    {
        public bool OrderNo { get; set; }
        public bool  DateOrder { get; set; }
        public bool ClientOrder { get; set; }
        public bool NoteOrder { get; set; }
        public bool PayOrder { get; set; }
        public bool UtmSource { get; set; }
        public bool ProfitOrder { get; set; }
        public bool SttOrder { get; set; }
        public bool StartDateOrder { get; set; }
        public bool CreatedName { get; set; }
        public bool UpdatedDate { get; set; }
        public bool UpdatedName { get; set; }
        public bool MainEmp { get; set; }
        public bool SubEmp { get; set; }
        public bool Voucher { get; set; }
        public bool Operator { get; set; }
        public bool HINHTHUCTT { get; set; }
        public bool KHACHPT { get; set; }
        public bool tum_medium { get; set; }
    }
    public class OrderPaymentRequest
    {
        public string OrderId { get; set; }
        public string OrderNo { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public double Amount { get; set; }
        public string ClientId { get; set; }
        public string Note { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class OrderandService
    {
        public string OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
       
    }
    public class TotalCountSumOrder
    {
        public double Amount { get; set; }
        public double Profit { get; set; }
        public double Price { get; set; }
   
    }
}
