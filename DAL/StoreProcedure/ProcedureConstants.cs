using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.StoreProcedure
{
    public class ProcedureConstants
    {
        public const string CLIENT_SEARCH = "sp_client_search";
        public const string GET_REVENU_DATE_RANGE = "sp_GetRevenueByDateRange";
        public const string GET_LABEL_REVENU_DATE_RANGE = "sp_GetLabelRevenueByDateRange";
        public const string GET_LABEL_QUANTITY_DATE_RANGE = "sp_GetOrderCountForEachLabelByDateRange";
        public const string ARTICLE_SEARCH = "Article_Search";
        public const string Campaign_Search = "Campaign_Search";
        public const string PriceDetail_Search_ByCampaignID = "PriceDetail_Search_ByCampaignID";
        public const string CampaignDetail_Search_ByCampaignID = "CampaignDetail_Search_ByCampaignID";     
        public const string GETALLORDER_SEARCH = "SP_GetAllOrder_search";
        //public const string GETALLORDERLIST= "SP_GetAllOrderData";
        //public const string GETALLORDERSTATUS = "SP_GetOrderStatusDetail";
        public const string GETGetAllClient_Search = "SP_GetClientData";
        public const string GET_TOTALCOUNT_ORDER = "SP_CountTotalOrderHeader";
        public const string SP_GetListContract = "SP_GetListContract";
        public const string SP_GetListContractPay = "SP_GetListContractPay";
        public const string SP_GetAllOrder_Debt = "SP_GetAllOrder_Debt";
        public const string SP_GetListContractPayDebt = "SP_GetListContractPayDebt";
        public const string SP_GetListContractPayByClientId = "SP_GetListContractPayByClientId";
        public const string SP_GetListContractPayByOrderId = "Sp_GetListContractPayDetailByOrderId";
        public const string sp_GetDetailContractPay = "sp_GetDetailContractPay";
        public const string sp_GetListOrderByPayId = "sp_GetListOrderByPayId";
        public const string sp_GetListOrderDebtByClientId = "sp_GetListOrderDebtByClientId";
        public const string SP_GetListPaymentRequest = "SP_GetListPaymentRequest";
        public const string SP_GetListDebtStatistic = "SP_GetListDebtStatistic";
        public const string SP_GetListPaymentVoucher = "SP_GetListPaymentVoucher";
        public const string SP_CountPaymentRequestByStatus = "SP_CountPaymentRequestByStatus";
        public const string SP_CountListDebtStatistic = "SP_CountListDebtStatistic";
        public const string SP_CountInvoiceRequestStatus = "SP_CountInvoiceRequestStatus";
        public const string SP_GetListPolicy = "SP_GetListPolicy";
        public const string SP_GetListOrder = "SP_GetListOrder";
        public const string SP_GetListSupplier = "SP_GetListSupplier";
        public const string SP_InsertPolicy = "SP_InsertPolicy";
        public const string SP_GetDepositHistoryByClientId = "SP_GetDepositHistoryByClientId";
        public const string SP_GetDetailOrderByClientId = "SP_GetDetailOrderByClientId";
        public const string Sp_CountTotalContractByStatus = "Sp_CountTotalContractByStatus";
        public const string SP_GetListHotelBooking = "SP_GetListHotelBooking";
        public const string SP_GetAllServiceBySupplierId = "SP_GetAllServiceBySupplierId";
        public const string SP_GetAllServiceBySupplierIdForReturn = "SP_GetAllServiceBySupplierIdForReturn";
        public const string SP_GetAllSubServiceBySupplierIdForReturn = "SP_GetAllSubServiceBySupplierIdForReturn";
        public const string SP_GetAllServiceByServiceCode = "SP_GetAllServiceByServiceCode";
        public const string SP_GetListContractPayByServiceId = "SP_GetListContractPayByServiceId";
        public const string SP_GetAllServiceByClientId = "SP_GetAllServiceByClientId";
        public const string sp_GetDetailPaymentRequest = "sp_GetDetailPaymentRequest";
        public const string SP_GetDetailDebtStatistic = "SP_GetDetailDebtStatistic";
        public const string sp_GetDetailPaymentVoucher = "sp_GetDetailPaymentVoucher";
        public const string SP_GetListPaymentRequestByClientId = "SP_GetListPaymentRequestByClientId";
        public const string SP_CheckCreatePaymentVoucher = "SP_CheckCreatePaymentVoucher";
        public const string SP_CheckExistsPaymentVoucherByRequestId = "SP_CheckExistsPaymentVoucherByRequestId";
        public const string SP_GetListPaymentRequestBySupplierId = "SP_GetListPaymentRequestBySupplierId";
        public const string sp_GetListPaymentRequestByServiceId = "sp_GetListPaymentRequestByServiceId";
        public const string sp_GetAllServiceByRequestiD = "sp_GetAllServiceByRequestiD";
        public const string Sp_GetDetailServiceById = "Sp_GetDetailServiceById";
        public const string SP_GetListInvoiceRequest = "SP_GetListInvoiceRequest";
        public const string SP_GetListInvoice = "SP_GetListInvoice";
        public const string SP_GetListUserByUserId = "SP_GetListUserByUserId";
        public const string PRODUCT_GetBoughtQuantity = "Product_GetBoughtQuantity";
        public const string Sp_GetListComments = "Sp_GetListComments";
        public const string SP_GetListAccountAccessAPI = "SP_GetListAccountAccessAPI";
        public const string SP_GetDetailOrderByOrderId = "SP_GetDetailOrderByOrderId";
        public const string SP_GetListOrderDetail = "SP_GetListOrderDetail";

    }
    
}
