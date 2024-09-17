using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
    public class StoreProcedureConstant
    {
        public static string GetClientByID = "SP_GetClientByID";
        public static string GetClientByAccountClientID = "SP_GetClientByAccountClientID";
        public static string GetContactClientByID = "SP_GetContactClientByID";
        public static string GetContractByID = "SP_GetContractByID";
        public static string sp_GetListOrderByPayId = "sp_GetListOrderByPayId";
        public static string sp_GetDetailContractPay = "sp_GetDetailContractPay";
        public static string sp_GetListDepositHistoryByPayId = "sp_GetListDepositHistoryByPayId";
        public static string SP_GetListServiceByPayId = "SP_GetListServiceByPayId";
        public static string SP_GetListSubServiceByPayId = "SP_GetListSubServiceByPayId";
        public static string GetFlyBookingDetailByOrderID = "SP_GetFlyBookingDetailByOrderID";
        public static string GetOrderByID = "SP_GetOrderByID";
        public static string GetPassengerByContactClientID = "SP_GetPassengerByContactClientID";
        public static string CreateContactClients = "SP_CreateContactClients";
        public static string CreateFlyBookingDetail = "SP_CreateFlyBookingDetail";
        public static string CreateOrder = "SP_CreateOrder";
        public static string CreatePassengers = "SP_CreatePassengers";
        public static string CreateBaggage = "SP_CreateBaggage";
        public static string CheckIfNewOrderValid = "SP_CheckIfNewOrderValid";
        public static string SP_CheckExistsDebtStatisticByOrderId = "SP_CheckExistsDebtStatisticByOrderId";
        public static string CreateFlySegment = "SP_CreateFlySegment";
        public static string CreateHotelBooking = "SP_CreateHotelBooking";
        public static string CreateHotelBookingRoomRates = "SP_CreateHotelBookingRoomRates";
        public static string CreateHotelBookingRooms = "SP_CreateHotelBookingRooms";
        public static string CreateHotelGuest = "SP_CreateHotelGuest";
        public static string InsertHotelBookingRoomExtraPackages = "SP_InsertHotelBookingRoomExtraPackages";
        public static string DeleteHotelBooking = "SP_DeleteHotelBooking";
        public static string InsertContract = "SP_InsertContract";
        public static string SP_InsertContractPay = "SP_InsertContractPay";
        public static string SP_UpdateContractPay = "SP_UpdateContractPay";
        public static string SP_DeleteContractPayDetailByPayDetailId = "SP_DeleteContractPayDetailByPayDetailId";
        public static string SP_InsertContractPayDetail = "SP_InsertContractPayDetail";
        public static string SP_UpdateContractPayDetail = "SP_UpdateContractPayDetail";
        public static string SP_UpdateOrderFinishPayment = "SP_UpdateOrderFinishPayment";
        public static string SP_UpdateAllServiceStatusByOrderId = "SP_UpdateAllServiceStatusByOrderId";
        public static string SP_UpdateServiceStatusByOrderId = "SP_UpdateServiceStatusByOrderId";
        public static string SP_UpdateOrderDebtStatus = "SP_UpdateOrderDebtStatus";
        public static string SP_UpdateDepositFinishPayment = "SP_UpdateDepositFinishPayment";
        public static string Sp_UpdateDebtStatusByPayId = "Sp_UpdateDebtStatusByPayId";
        public static string Sp_UpdateContractPayDetail = "Sp_UpdateContractPayDetail";
        public static string sp_GetDetailContract = "sp_GetDetailContract";
        public static string SP_UpdateContractStatus = "SP_UpdateContractStatus";
        public static string SP_InsertContractHistory = "SP_InsertContractHistory";
        public static string sp_GetListContractByClientId = "sp_GetListContractByClientId";
        public static string SP_GetDetailOrderServiceByOrderId = "SP_GetDetailOrderServiceByOrderId";
        public static string sp_GetDetailContractHistory = "sp_GetDetailContractHistory";
        public static string SP_DeleteContract = "SP_DeleteContract";
        public static string SP_UpdateContract = "SP_UpdateContract";
        public static string sp_get_client_by_id = "sp_get_client_by_id";
        public static string sp_GetDetailPolicy = "sp_GetDetailPolicy";
        public static string SP_GetContractPayByOrderId = "SP_GetContractPayByOrderId";
        public static string SP_CheckCreateContractPayByClientId = "SP_CheckCreateContractPayByClientId";
        public static string SP_InsertPolicy = "SP_InsertPolicy";
        public static string SP_InsertPolicyDetail = "SP_InsertPolicyDetail";
        public static string SP_UpdatePolicyDetail = "SP_UpdatePolicyDetail";
        public static string SP_UpdatePolicy = "SP_UpdatePolicy";
        //public static string SP_InsertSupplier = "SP_InsertSupplier";
        //public static string SP_GetSupplierById = "SP_GetSupplierById";
        //public static string SP_UpdateSupplier = "SP_UpdateSupplier";
        //public static string SP_GetListSupplier = "SP_GetListSupplier";
        public static string SP_DeletePolicy = "SP_DeletePolicy";
        public static string SP_GetSuggestSupplier = "SP_GetSuggestSupplier";
        public static string SP_GetSuggestSupplierOfHotel = "SP_GetSuggestSupplierOfHotel";
        public static string InsertTour = "SP_InsertTour";
        public static string InsertTourPackages = "SP_InsertTourPackages";
        public static string UpdateTourPackages = "SP_UpdateTourPackages";
        public static string UpdateTour = "SP_UpdateTour";
        public static string InsertTourGuests = "SP_InsertTourGuests";
        public static string UpdateTourGuests = "SP_UpdateTourGuests";
        public static string Sp_GetTourByOrderId = "Sp_GetTourByOrderId";
        public static string InsertFlyBookingDetail = "SP_CreateFlyBookingDetail";
        public static string UpdateFlyBookingDetail = "SP_UpdateFlyBookingDetail";
        public static string InsertFlyBookingExtraPackages = "SP_InsertFlyBookingExtraPackages";
        public static string UpdateFlyBookingExtraPackages = "SP_UpdateFlyBookingExtraPackages";
        public static string SP_InsertPaymentRequest = "SP_InsertPaymentRequest";
        public static string sp_InsertDebtStatistic = "sp_InsertDebtStatistic";
        public static string SP_UpdatePaymentRequestStatus = "SP_UpdatePaymentRequestStatus";
        public static string sp_UpdateDebtStatistic = "sp_UpdateDebtStatistic";
        public static string SP_UnVerifyPaymentRequest = "SP_UnVerifyPaymentRequest";
        public static string SP_InsertPaymentRequestDetail = "SP_InsertPaymentRequestDetail";
        public static string SP_UpdateOrderRefund = "SP_UpdateOrderRefund";
        public static string SP_UpdatePaymentRequest = "SP_UpdatePaymentRequest";
        public static string SP_UpdatePaymentRequestDetail = "SP_UpdatePaymentRequestDetail";
        public static string SP_GetDetailHotelBookingByOrderID = "SP_GetDetailHotelBookingByOrderID";
        public static string SP_GetDetailOrderByOrderId = "SP_GetDetailOrderByOrderId";
        public static string SP_GetHotelBookingById = "SP_GetHotelBookingById";
        public static string SP_GetHotelBookingRateByHotelBookingRoomID = "SP_GetHotelBookingRateByHotelBookingRoomID";
        public static string SP_GetHotelBookingRoomByHotelBookingID = "SP_GetHotelBookingRoomByHotelBookingID";
        public static string SP_GetHotelBookingByOrderID = "SP_GetHotelBookingByOrderID";
        public static string SP_GetHotelGuestByOrderId = "SP_GetHotelGuestByOrderId";
        public static string sp_gethotelbookingroomextrapackagebyhotelbookingid = "sp_gethotelbookingroomextrapackagebyhotelbookingid";
        public static string SP_UpdateOrderStatus = "SP_UpdateOrderStatus";
        public static string SP_UpdateHotelBookingStatus = "SP_UpdateHotelBookingStatus";
        public static string SP_UpdateHotelBookingRooms = "SP_UpdateHotelBookingRooms";
        public static string SP_GetAllServiceByOrderId = "SP_GetAllServiceByOrderId";
        public static string SP_GetDetailTourByID = "SP_GetDetailTourByID";
        public static string SP_GetDetailHotelBookingByID = "SP_GetDetailHotelBookingByID";
        public static string SP_GetDetailFlyBookingDetailById = "SP_GetDetailFlyBookingDetailById";
        public static string GetListFlyBooking = "SP_GetListFlyBooking";
        public static string SP_UpdateHotelBookingRoomRate = "SP_UpdateHotelBookingRoomRate";
        public static string sp_GetDetailBookingCodeByHotelBookingId = "sp_GetDetailBookingCodeByHotelBookingId";
        public static string Sp_InsertHotelBookingCode = "Sp_InsertHotelBookingCode";
        public static string Sp_UpdateHotelBookingCode = "Sp_UpdateHotelBookingCode";
        public static string SP_GetDetailBookingCodeById = "SP_GetDetailBookingCodeById";
        public static string SP_InsertPaymentVoucher = "SP_InsertPaymentVoucher";
        public static string SP_UpdatePaymentVoucher = "SP_UpdatePaymentVoucher";
        public static string SP_CountHotelBookingByStatus = "SP_CountHotelBookingByStatus";
        public static string SP_GetListTour = "SP_GetListTour";
        public static string SP_GetListTourPackagesByTourId = "SP_GetListTourPackagesByTourId";
        public static string SP_GetListTourGuestsByTourId = "SP_GetListTourGuestsByTourId";
        public static string SP_UpdateTourStatus = "SP_UpdateTourStatus";
        public static string SP_UpdateHotelBookingRoomExtraPackages = "SP_UpdateHotelBookingRoomExtraPackages";
        public static string SP_CountTourByStatus = "SP_CountTourByStatus";
        public static string SP_UpdateHotelBooking = "SP_UpdateHotelBooking";
        //public static string SP_GetListTourProduct = "SP_GetListTourProduct";
        public static string Sp_InsertServiceDeclines = "Sp_InsertServiceDeclines";
        public static string Sp_GetServiceDeclinesByOrderId = "Sp_GetServiceDeclinesByOrderId";
        public static string SP_CheckClientDebt = "SP_CheckClientDebt";
        public static string SP_GetListTourPackagesOptionalByTourId = "SP_GetListTourPackagesOptionalByTourId";
        public static string SP_InsertTourPackagesOptional = "SP_InsertTourPackagesOptional";
        public static string SP_UpdateTourPackagesOptional = "SP_UpdateTourPackagesOptional";
        //public static string SP_InsertTourProduct = "SP_InsertTourProduct";
        //public static string SP_UpdateTourProduct = "SP_UpdateTourProduct";
        public static string SP_InsertTourDestination = "SP_InsertTourDestination";
        public static string SP_UpdateTourDestination = "SP_UpdateTourDestination";
        public static string SP_DeleteTourDestination = "SP_DeleteTourDestination";
        public static string SP_UpdateOperatorByOrderid = "SP_UpdateOperatorByOrderid";
        public static string SP_GetGroupClassAirlines = "SP_GetGroupClassAirlines";
        public static string SP_GetListPlayGroundDetail = "SP_GetListPlayGroundDetail";
        public static string SP_InsertPlaygroundDetail = "SP_InsertPlaygroundDetail";
        public static string SP_UpdatePlaygroundDetail = "SP_UpdatePlaygroundDetail";
        public static string Sp_GetDetailPlayground = "Sp_GetDetailPlayground";
        public static string SP_GetListRolePermissionByUserAndRole = "SP_GetListRolePermissionByUserAndRole";
        public static string SP_GetListPaymentRequestByOrderId = "SP_GetListPaymentRequestByOrderId";


        public static string SP_InsertHotelRoom = "SP_InsertHotelRoom";
        public static string SP_UpdateHotelRoom = "SP_UpdateHotelRoom";
        public static string SP_DeleteHotelRoom = "SP_DeleteHotelRoom";
        public static string SP_CheckExistHotelRoomNameUsing = "SP_CheckExistHotelRoomNameUsing";
        public static string SP_CheckExistHotelRoomUsing = "SP_CheckExistHotelRoomUsing";
        public static string SP_CheckExistHotelRoomName = "SP_CheckExistHotelRoomName";
        public static string SP_GetHotelRoomBySupplier = "SP_GetHotelRoomBySupplier";
        public static string SP_GetDetailHotelRoom = "SP_GetDetailHotelRoom";
        public static string SP_GetHotelBySupplierId = "SP_GetHotelBySupplierId";

        public static string sp_GetDetailProgramBySupplierId = "sp_GetDetailProgramBySupplierId";
       /* public static string SP_UpdateServiceStatusByOrderId = "SP_UpdateServiceStatusByOrderId";*/
        public static string SP_GetListHotelBookingCodeByOrderId = "SP_GetListHotelBookingCodeByOrderId";
        public static string sp_InsertInvoiceRequest = "sp_InsertInvoiceRequest";
        public static string sp_InsertInvoice = "sp_InsertInvoice";
        public static string sp_InsertInvoiceFormNo = "sp_InsertInvoiceFormNo";
        public static string sp_UpdateInvoiceFormNo = "sp_UpdateInvoiceFormNo";
        public static string sp_InsertInvoiceSign = "sp_InsertInvoiceSign";
        public static string sp_UpdateInvoiceSign = "sp_UpdateInvoiceSign";
        public static string sp_InsertInvoiceRequestDetail = "sp_InsertInvoiceRequestDetail";
        public static string sp_InsertInvoiceDetail = "sp_InsertInvoiceDetail";
        public static string sp_UpdateInvoiceRequest = "sp_UpdateInvoiceRequest";
        public static string sp_UpdateInvoice = "sp_UpdateInvoice";
        public static string sp_UpdateInvoiceRequestDetail = "sp_UpdateInvoiceRequestDetail";
        public static string sp_UpdateInvoiceDetail = "sp_UpdateInvoiceDetail";
        public static string SP_VerifyInvoice = "SP_VerifyInvoice";
        public static string SP_VerifyInvoiceRequest = "SP_VerifyInvoiceRequest";
        public static string sp_GetDetailInvoiceRequest = "sp_GetDetailInvoiceRequest";
        public static string sp_GetListInvoiceRequestByClientId = "sp_GetListInvoiceRequestByClientId";
        public static string sp_GetListInvoiceRequestByInvoiceId = "sp_GetListInvoiceRequestByInvoiceId";
        public static string sp_GetListInvoiceRequestByOrderId = "sp_GetListInvoiceRequestByOrderId";
        public static string sp_GetDetailInvoice = "sp_GetDetailInvoice";
        public static string InsertFlyBookingPackagesOptional = "SP_InsertFlyBookingPackagesOptional";
        public static string UpdateFlyBookingPackagesOptional = "SP_UpdateFlyBookingPackagesOptional";
        public static string SP_GetManagerByUserId = "SP_GetManagerByUserId";
        public static string Sp_GetLeaderByUserId = "Sp_GetLeaderByUserId";

        public static string Sp_GetAmountRemainOfContractPayByClientId = "SP_GetAmountRemainOfContractByClientId";

        public static string SP_UpdateServiceStatusFromDecline = "SP_UpdateServiceStatusFromDecline";
        public static string InsertOtherBookingPackagesOptional = "SP_InsertOtherBookingPackagesOptional";
        public static string UpdateOtherBookingPackagesOptional = "SP_UpdateOtherBookingPackagesOptional";
        public static string GetListOtherBooking = "SP_GetListOtherBooking";
        public static string SP_GetListOtherBookingPackagesOptionalByBookingId = "SP_GetListOtherBookingPackagesOptionalByBookingId";
        public static string SP_GetDetailOtherBookingById = "SP_GetDetailOtherBookingById";
        
        public static string SP_InsertHotel = "SP_InsertHotel";
        public static string SP_UpdateHotel = "SP_UpdateHotel";

        public static string SP_fe_GetHotelRoomByHotelId = "SP_fe_GetHotelRoomByHotelId";
        public static string SP_GetHotelPricePolicyActiveByHotelID = "SP_GetHotelPricePolicyActiveByHotelID";
        public static string SP_GetListProgramsPackageByRoomId = "SP_GetListProgramsPackageByRoomId";
        public static string SP_GetListProgramsPackageDailyByRoomId = "SP_GetListProgramsPackageDailyByRoomId";

        public static string GetVinWonderBookingTicketByBookingID = "SP_GetVinWonderBookingTicketByBookingID"; 
        public static string GetVinWonderBookingCustomerByBookingId = "SP_GetVinWonderBookingCustomerByBookingId"; 
        public static string InsertVinWonderBooking = "SP_InsertVinWonderBooking"; 
        public static string UpdateVinWonderBooking = "sp_updateVinWonderBooking";
        public static string InsertVinWonderBookingTicketCustomer = "sp_InsertVinWonderBookingTicketCustomer";
        public static string UpdateVinWonderBookingTicketCustomer = "sp_UpdateVinWonderBookingTicketCustomer";
        public static string InsertVinWonderBookingTicket = "sp_InsertVinWonderBookingTicket";
        public static string UpdateVinWonderBookingTicket = "sp_UpdateVinWonderBookingTicket";
        public static string sp_GetDetailVinwonder = "sp_GetDetailVinwonder";

        public static string SP_InsertAttachFile = "SP_InsertAttachFile";
        public static string SP_UpdateAttachFile = "SP_UpdateAttachFile";
        public static string SP_DeleteAttachFile = "SP_DeleteAttachFile";
        public static string SP_GetAttachFileByDataIdAndType = "SP_GetAttachFileByDataIdAndType";

        public static string GetListVinWonderBooking = "SP_GetListVinWonderBooking";
        public static string InsertHotelBookingRoomsOptional = "sp_InsertHotelBookingRoomsOptional";
        public static string UpdateHotelBookingRoomsOptional = "sp_UpdateHotelBookingRoomsOptional";   
        public static string InsertHotelBookingRoomRatesOptional = "sp_InsertHotelBookingRoomRatesOptional";
        public static string UpdateHotelBookingRoomRatesOptional = "sp_UpdateHotelBookingRoomRatesOptional";
        public static string GetListHotelBookingRoomsOptionalByBookingId = "SP_GetListHotelBookingRoomsOptionalByBookingId";
        public static string sp_GetReportRevenueHotel = "Sp_GetReportRevenueHotel";
        public static string SP_GetListFlyBookingOptionalByBookingId = "SP_GetListFlyBookingOptionalByBookingId";
        public static string SP_GetListHotelBookingRoomsExtraPackageByBookingId = "SP_GetListHotelBookingRoomsExtraPackageByBookingId";
        public static string GetListHotelBookingRoomRatesOptionalByBookingId = "SP_GetListHotelBookingRoomRatesOptionalByBookingId";
        
        public static string SP_Report_TotalRevenueByDepartment = "SP_Report_TotalRevenueByDepartment";
        public static string SP_Report_TotalRevenueBySaler = "SP_Report_TotalRevenueBySaler";
        public static string SP_Report_TotalRevenueBySupplier = "SP_Report_TotalRevenueBySupplier";
        public static string SP_Report_TotalRevenueByClient = "SP_Report_TotalRevenueByClient";
        public static string SP_Report_GetListOrder = "SP_Report_GetListOrder";
        public static string SP_Report_DetailRevenueByDepartment = "SP_Report_DetailRevenueByDepartment";
        public static string SP_Report_DetailRevenueBySaler = "SP_Report_DetailRevenueBySaler";
        public static string SP_Report_DetailRevenueByClient = "SP_Report_DetailRevenueByClient";
        public static string SP_Report_DetailRevenueBySupplier = "SP_Report_DetailRevenueBySupplier";


        public static string SP_GetListPrograms = "SP_GetListPrograms";
        public static string SP_InsertPrograms = "SP_InsertPrograms";
        public static string SP_UpdatePrograms = "SP_UpdatePrograms";
        public static string sp_GetDetailProgram = "sp_GetDetailProgram";
       
        public static string SP_GetListProgramsPackage = "SP_GetListProgramsPackage";
        public static string sp_UpdateProgramPackage = "sp_UpdateProgramPackage";
        public static string sp_InsertProgramPackage = "sp_InsertProgramPackage";
        public static string Sp_DeleteProgramPackagesByProgramId = "Sp_DeleteProgramPackagesByProgramId";
        public static string SP_CheckExistsProgramsPackageByDate = "SP_CheckExistsProgramsPackageByDate";
        public static string SP_GetListProgramsPackageGroupByRoom = "SP_GetListProgramsPackageGroupByRoom";
        public static string SP_GetListProgramsPackageDailyGroupByRoomId = "SP_GetListProgramsPackageDailyGroupByRoomId";


        public static string sp_GetlistHotelBySupplierId = "sp_GetlistHotelBySupplierId";
        public static string SP_GetListProgramsBySupplierId = "SP_GetListProgramsBySupplierId";
        public static string SP_GetListSupplierByHotel = "SP_GetListSupplierByHotel";

        public static string SP_Report_TotalDebtRevenueBySupplier = "SP_Report_TotalDebtRevenueBySupplier";
        public static string SP_Report_DetailDebtRevenueBySupplier = "SP_Report_DetailDebtRevenueBySupplier";


        public static string SP_Report_TotalDebtRevenueByClient = "SP_Report_TotalDebtRevenueByClient";
        public static string SP_Report_DetailDebtRevenueByClientId = "SP_Report_DetailDebtRevenueByClientId";


        public static string GetListInvoiceCodebyOrderId = "SP_GetListInvoiceCodebyOrderId";

        public static string InsertWaterSportBooking = "SP_InsertWaterSportBooking";
        public static string UpdateWaterSportBooking = "SP_UpdateWaterSportBooking";


        public static string GetHotelPricePolicyByPrograms = "SP_GetHotelPricePolicyByPrograms";
        public static string GetHotelPricePolicyByCampaignID = "SP_GetHotelPricePolicyByCampaignID";
        public static string GetHotelPricePolicyDailyByPrograms = "SP_GetHotelPricePolicyDailyByPrograms";
        public static string GetHotelPricePolicyDailyByCampaignID = "SP_GetHotelPricePolicyDailyByCampaignID";

        public static string SP_Report_TotalRevenueOrderBySale = "SP_Report_TotalRevenueOrderBySale"; 
        public static string SP_Report_TotalRevenueOrderByClient = "SP_Report_TotalRevenueOrderByClient"; 

        public static string sp_InsertProgramPackageDaily = "sp_InsertProgramPackageDaily"; 
        public static string sp_UpdateProgramPackageDaily = "sp_UpdateProgramPackageDaily"; 
        public static string SP_DeleteProgramPackagesDailyByProgramId = "SP_DeleteProgramPackagesDailyByProgramId"; 
        public static string SP_GetListProgramsPackageDaily = "SP_GetListProgramsPackageDaily"; 
        public static string SP_CheckExistsProgramsPackageDailyByDate = "SP_CheckExistsProgramsPackageDailyByDate"; 
        public static string SP_GetTotalAmountContractPayByServiceId = "SP_GetTotalAmountContractPayByServiceId"; 

        public static string SP_GetTotalAmountPaymentVoucherByDate = "SP_GetTotalAmountPaymentVoucherByDate";


        public static string InsertTourProgramPackages = "sp_InsertTourProgramPackages";
        public static string UpdateTourProgramPackages = "[sp_UpdateTourProgramPackages]";

        public static string sp_InsertAccountAccessAPI = "sp_InsertAccountAccessAPI";
        public static string sp_UpdateAccountAccessAPI = "sp_UpdateAccountAccessAPI";

        public static string sp_InsertAccountAccessAPIPermission = "sp_InsertAccountAccessAPIPermission";
        public static string sp_UpdateAccountAccessAPIPermission = "sp_UpdateAccountAccessAPIPermission";
        public static string Sp_GetListAllCodeByTypeAndIds = "Sp_GetListAllCodeByTypeAndIds";

        public static string SP_GetListTag = "SP_GetListTag";
        public static string SP_GetListClient = "SP_GetListClient";
        public static string SP_InsertClient = "SP_InsertClient";
        public static string sp_UpdateClient = "sp_UpdateClient";
        public static string SP_GetDetailClientByClientId = "SP_GetDetailClientByClientId";
        public static string sp_InsertAccountClient = "sp_InsertAccountClient";
        public static string sp_UpdateAccountClient = "sp_UpdateAccountClient";
        public static string sp_UpdateUserAgent = "sp_UpdateUserAgent";
        public static string SP_InsertUserAgent = "SP_InsertUserAgent";
        public static string SP_GetUserAgentByClientId = "SP_GetUserAgentByClientId";
        public static string Sp_UpdateOrder = "Sp_UpdateOrder";


        #region vin wonder
        public static string sp_InsertCampaign = "sp_InsertCampaign";
        public static string sp_UpdateCampaign = "sp_UpdateCampaign";

        public static string sp_InsertVinWonderPricePolicy = "sp_InsertVinWonderPricePolicy";
        public static string sp_UpdateVinWonderPricePolicy = "sp_UpdateVinWonderPricePolicy";

        public static string SP_GetVinWonderPricePolicyByCampaignId = "SP_GetVinWonderPricePolicyByCampaignId";
        public static string sp_UpdateAllCode = "sp_UpdateAllCode";

        public static string SP_GetVinWonderPricePolicyByServiceId = "SP_GetVinWonderPricePolicyByServiceId";
        public static string SP_GetVinWonderBookingEmailByOrderID = "SP_GetVinWonderBookingEmailByOrderID";
        public static string SP_GetVinWonderBookingByOrderID = "SP_GetVinWonderBookingByOrderID";
        public static string SP_GetVinWonderBookingTicketByBookingID = "SP_GetVinWonderBookingTicketByBookingID";
        public static string SP_GetVinWonderBookingCustomerByBookingId = "SP_GetVinWonderBookingCustomerByBookingId";

        public static string Report_TotalRevenueByOrder = "SP_Report_TotalRevenueByOrder";
        public static string Report_SumTotalRevenueByOrder = "SP_Report_SumTotalRevenueByOrder";
        #endregion

        #region Tour product
        public static string SP_GetDetailTourProductByID = "SP_GetDetailTourProductByID";
        public static string SP_GetListTourProduct = "SP_GetListTourProduct";
        public static string SP_InsertTourProduct = "SP_InsertTourProduct";
        public static string SP_UpdateTourProduct = "SP_UpdateTourProduct";
        public static string SP_DeleteTourProduct = "SP_DeleteTourProduct";
        #endregion

        #region supplier
        public static string SP_InsertSupplier = "SP_InsertSupplier";
        public static string SP_UpdateSupplier = "SP_UpdateSupplier";
        public static string SP_GetSupplierById = "SP_GetSupplierById";
        public static string SP_GetListSupplier = "SP_GetListSupplier";

        public static string SP_GetDetailSupplierContact = "SP_GetDetailSupplierContact";
        public static string SP_GetDetailSupplierContactBySupplierId = "SP_GetDetailSupplierContactBySupplierId";
        public static string sp_InsertSupplierContact = "sp_InsertSupplierContact";
        public static string sp_UpdateSupplierContact = "sp_UpdateSupplierContact";
        
        public static string SP_GetListBankingAccountBySupplierId = "SP_GetListBankingAccountBySupplierId";
        public static string SP_GetListBankingAccountByClientId = "SP_GetListBankingAccountByClientId";
        public static string SP_UpdateBankingAccount = "SP_UpdateBankingAccount";
        public static string SP_InsertBankingAccount = "SP_InsertBankingAccount";

        public static string SP_GetAllServiceBySupplierId = "SP_GetAllServiceBySupplierId";
        public static string sp_InsertHotelSupplier = "sp_InsertHotelSupplier";
        public static string sp_DeleteHotelSupplierBySupplierId = "sp_DeleteHotelSupplierBySupplierId";
        public static string SP_GetListHotelBySupplierId = "SP_GetListHotelBySupplierId";
        
        public static string SP_GetListPaymentVoucherBySupplierId = "SP_GetListPaymentVoucherBySupplierId";
        public static string SP_GetListTourProgramPackagesByTourProductId = "SP_GetListTourProgramPackagesByTourProductId";

        #endregion

        #region hotel
        public static string SP_GetHotelDetailById = "SP_GetHotelDetailById";
        public static string SP_GetListHotel = "SP_GetListHotel";
        public static string SP_GetSuggestHotelByName = "SP_GetSuggestHotelByName";

        public static string SP_GetListHotelBankingAccountByHotelId = "SP_GetListHotelBankingAccountByHotelId";
        public static string SP_InsertHotelBankingAccount = "SP_InsertHotelBankingAccount";
        public static string SP_UpdateHotelBankingAccount = "SP_UpdateHotelBankingAccount";

        public static string SP_GetListHotelContactByHotelId = "SP_GetListHotelContactByHotelId";
        public static string sp_InsertHotelContact = "sp_InsertHotelContact";
        public static string sp_UpdateHotelContact = "sp_UpdateHotelContact";

        public static string SP_GetListHotelSurchargeByHotelId = "SP_GetListHotelSurchargeByHotelId";
        public static string sp_InsertHotelSurcharge = "sp_InsertHotelSurcharge";
        public static string sp_UpdateHotelSurcharge = "sp_UpdateHotelSurcharge";

        public static string SP_GetHotelRoomByHotelId = "SP_GetHotelRoomByHotelId";
        
        #endregion
    }
}
