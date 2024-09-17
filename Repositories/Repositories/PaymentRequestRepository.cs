using Aspose.Cells;
using DAL;
using DAL.Funding;
using DAL.StoreProcedure;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Funding;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Utilities;
using Utilities.Contants;

namespace Repositories.Repositories
{
    public class PaymentRequestRepository : IPaymentRequestRepository
    {
        private readonly UserDAL userDAL;
        private readonly PaymentRequestDAL paymentRequestDAL;
        private readonly PaymentVoucherDAL paymentVoucherDAL;

        private readonly OrderDAL orderDAL;


        public PaymentRequestRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            userDAL = new UserDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            paymentRequestDAL = new PaymentRequestDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            paymentVoucherDAL = new PaymentVoucherDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            orderDAL = new OrderDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
         
        }

        public int CreatePaymentRequest(PaymentRequestViewModel model)
        {
            var paymentRequest = paymentRequestDAL.GetByPaymentCode(model.PaymentCode);
            if (paymentRequest != null)
                return -2;
            return paymentRequestDAL.CreatePaymentRequest(model);
        }

        public PaymentRequestViewModel GetById(int paymentRequestId)
        {
            try
            {
                var requestInfos = paymentRequestDAL.GetRequestDetail(paymentRequestId,
                    ProcedureConstants.sp_GetDetailPaymentRequest).ToList<PaymentRequestViewModel>();
                var requestInfo = requestInfos.FirstOrDefault();
                requestInfo.PaymentDateStr = DateUtil.DateToString(requestInfo.PaymentDate);
                requestInfo.RelateData = new List<PaymentRequestDetailViewModel>();
                if (requestInfo.Type == (int)PAYMENT_VOUCHER_TYPE.HOAN_TRA_KHACH_HANG)
                {
                    foreach (var item in requestInfos)
                    {
                        PaymentRequestDetailViewModel model = new PaymentRequestDetailViewModel();
                        item.CopyProperties(model);
                        model.OrderId = item.OrderId;
                        model.OrderNo = item.OrderNo;
                        model.Amount = item.Amount;
                        model.OrderAmount = item.OrderAmount;
                        model.OrderAmountPay = item.OrderAmountPay;
                        model.ServiceId = (int)item.ServiceId;
                        model.UserCreateFullName = item.UserCreateFullName;
                        model.DepartmentName = item.DepartmentName;
                        model.ServiceId = (int)item.ServiceId;
                        model.BankIdName = item.BankIdName;
                        model.AccountNumber = item.AccountNumber;
                        model.ServiceId = (int)item.ServiceId;
                        var serviceInfo = paymentRequestDAL.GetDetailServiceById(item.ServiceId, item.ServiceType,
                          ProcedureConstants.Sp_GetDetailServiceById).ToList<PaymentRequestViewModel>().FirstOrDefault();
                        model.ServiceAmount = serviceInfo != null ? serviceInfo.Amount : 0;
                        model.ServicePrice = serviceInfo != null ? serviceInfo.Price : 0;
                        requestInfo.RelateData.Add(model);
                    }
                }
                if (requestInfo.Type == (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_DICH_VU || requestInfo.Type == (int)PAYMENT_VOUCHER_TYPE.CHI_PHI_MARKETING)
                {
                    var requestServiceDetails = paymentRequestDAL.GetRequestDetail(paymentRequestId,
                        ProcedureConstants.sp_GetAllServiceByRequestiD).ToList<PaymentRequestViewModel>();
                    foreach (var item in requestServiceDetails)
                    {
                        PaymentRequestDetailViewModel model = new PaymentRequestDetailViewModel();
                        item.CopyProperties(model);
                        model.OrderId = item.OrderId;
                        model.ServiceId = (int)item.ServiceId;
                        var serviceInfo = paymentRequestDAL.GetDetailServiceById(item.ServiceId, item.ServiceType,
                        ProcedureConstants.Sp_GetDetailServiceById).ToList<PaymentRequestViewModel>().FirstOrDefault();
                        model.ServiceAmount = serviceInfo != null ? serviceInfo.Amount : 0;
                        double servicePrice = 0;
                        if (requestInfo.Type == (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_DICH_VU)
                        {
                            //servicePrice = GetAmontRequestForSupplier(item.ServiceId, item.ServiceType, requestInfo.SupplierId.Value, serviceInfo.Price);
                        }
                        model.ServicePrice = servicePrice;
                        requestInfo.RelateData.Add(model);
                    }
                }
                //foreach (var item in requestInfo.RelateData)
                //{
                //    var serviceInfo = paymentRequestDAL.GetDetailServiceById(item.ServiceId, item.ServiceType,
                //        ProcedureConstants.sp_GetAllServiceByRequestiD).ToList<PaymentRequestViewModel>().FirstOrDefault();
                //    item.ServiceAmount = serviceInfo != null ? serviceInfo.Amount : 0;
                //    item.ServicePrice = serviceInfo != null ? serviceInfo.Price : 0;
                //}
                requestInfo.IsIncludeService = requestInfo.IsServiceIncluded == true ? 1 : 0;
                return requestInfo;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetById - PaymentRequestRepository: " + ex);
                return null;
            }
        }

        public PaymentRequest GetByRequestNo(string paymentRequestNo)
        {
            try
            {
                return paymentRequestDAL.GetByRequestNo(paymentRequestNo);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByRequestNo - PaymentRequestRepository: " + ex);
                return null;
            }
        }

    

        public List<CountStatus> GetCountStatus(PaymentRequestSearchModel searchModel)
        {
            try
            {
                var listPaymentRequests = paymentRequestDAL.GetCountStatus(searchModel,
                ProcedureConstants.SP_CountPaymentRequestByStatus).ToList<CountStatus>();
                return listPaymentRequests;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetCountStatus - PaymentRequestRepository: " + ex);
                return new List<CountStatus>();
            }
        }

        public int UpdatePaymentRequest(PaymentRequestViewModel model)
        {
            var entity = paymentRequestDAL.GetById(model.Id);
            var result = paymentRequestDAL.UpdatePaymentRequest(model);
            if (result > 0 && entity.Status == (int)PAYMENT_REQUEST_STATUS.TU_CHOI && model.Amount != entity.Amount && model.IsServiceIncluded.Value)
            {
                var detailRequests = paymentRequestDAL.GetByPaymentRequestId((int)model.Id);
                foreach (var item in detailRequests)
                {
                    item.Amount = model.Amount;
                    paymentRequestDAL.UpdateRequestDetail(item);
                }
            }
            if (result > 0 && model.IsAdminEdit && model.Amount != entity.Amount && model.IsServiceIncluded.Value)
            {
                var detailRequests = paymentRequestDAL.GetByPaymentRequestId((int)model.Id);
                foreach (var item in detailRequests)
                {
                    item.Amount = model.Amount;
                    paymentRequestDAL.UpdateRequestDetail(item);
                }
                //tính lại số tiền phiếu chi nếu có
                var amountChange = entity.Amount - model.Amount;
                var paymentVoucher = paymentVoucherDAL.CheckExistsPaymentRequest(new List<long>() { model.Id },
                    ProcedureConstants.SP_CheckExistsPaymentVoucherByRequestId);
                if (paymentVoucher.Count > 0)
                {
                    foreach (var item in paymentVoucher)
                    {
                        item.Amount = item.Amount - amountChange;
                        paymentVoucherDAL.Update(item);
                    }
                }
            }
            return result;
        }

        public int ApprovePaymentRequest(string paymentRequestNo, int userId, int status)
        {
            var entity = paymentRequestDAL.GetByRequestNo(paymentRequestNo);
            entity.UpdatedBy = userId;
            entity.Status = status;
            return paymentRequestDAL.ApproveRequest(entity);
        }

        public int RejectPaymentRequest(string paymentRequestNo, string noteReject, int userId)
        {
            var entity = paymentRequestDAL.GetByRequestNo(paymentRequestNo);
            entity.Status = (int)PAYMENT_REQUEST_STATUS.TU_CHOI;
            entity.DeclineReason = noteReject;
            entity.UpdatedBy = userId;
            entity.UpdatedDate = DateTime.Now;
            return paymentRequestDAL.UpdateRequest(entity);
        }

        public int UndoApprove(string paymentRequestNo, string note, int userId, int status)
        {
            var entity = paymentRequestDAL.GetByRequestNo(paymentRequestNo);
            entity.UpdatedBy = userId;
            entity.Status = status;
            entity.Note = note;
            return paymentRequestDAL.UndoApproveRequest(entity);
        }

        public List<PaymentRequestViewModel> GetServiceListBySupplierId(long supplierId, int requestId = 0, int serviceId = 0)
        {
            try
            {
                var listService = new List<PaymentRequestViewModel>();
                var listServiceOutput = paymentRequestDAL.GetServiceListBySupplierId(supplierId,
                    ProcedureConstants.SP_GetAllServiceBySupplierId).ToList<PaymentRequestViewModel>();
                var listServiceId = listServiceOutput.Select(n => Convert.ToInt64(n.ServiceId)).ToList();
                var listRequestDetail = paymentRequestDAL.GetByDataIds(listServiceId);
                if (requestId == 0)
                    listRequestDetail = listRequestDetail.Where(n => n.Status != (int)PAYMENT_REQUEST_STATUS.TU_CHOI).ToList();
                if (serviceId != 0)
                    listServiceOutput = listServiceOutput.Where(n => n.ServiceId == serviceId).ToList();
                foreach (var item in listServiceOutput)
                {
                    item.TotalAmount = item.Amount;
                    var detail = listRequestDetail.Where(n => n.OrderId == item.OrderId && n.RequestId == requestId && n.ServiceId == item.ServiceId).FirstOrDefault();
                    if (detail != null)
                    {
                        item.IsChecked = true;
                        item.Id = detail.Id;
                        item.AmountPayment = detail.Amount;
                    }
                    item.TotalDisarmed = item.AmountPay;
                    item.TotalNeedPayment = item.Amount - item.TotalDisarmed;
                    if (item.TotalNeedPayment < 0)
                        item.TotalNeedPayment = 0;
                    PaymentRequestViewModel model = new PaymentRequestViewModel();
                    item.CopyProperties(model);
                    if (requestId > 0)
                    {
                        if (listService.FirstOrDefault(n => n.ServiceId == item.ServiceId) == null)
                        {
                            if (detail != null)
                            {
                                item.IsDisabled = true;
                                item.IsChecked = true;
                            }
                            listService.Add(item);
                        }
                    }
                    else
                    {
                        if (listService.FirstOrDefault(n => n.ServiceId == item.ServiceId) == null && item.TotalNeedPayment > 0)
                            listService.Add(item);
                    }
                }
                return listService;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetServiceListBySupplierId - PaymentRequestRepository: " + ex);
            }
            return new List<PaymentRequestViewModel>();
        }

        public List<PaymentRequestViewModel> GetServiceListByClientId(long clientId, int requestId = 0)
        {
            try
            {
                var listServiceOutput = paymentRequestDAL.GetServiceListByClientId(clientId,
                    ProcedureConstants.SP_GetAllServiceByClientId).ToList<PaymentRequestViewModel>();
                var listService = new List<PaymentRequestViewModel>();
                var listRequestDetail = paymentRequestDAL.GetByDataIds(listServiceOutput.Select(n => Convert.ToInt64(n.OrderId)).ToList());
                foreach (var item in listServiceOutput)
                {
                    item.TotalAmount = item.Amount;
                    var detail = listRequestDetail.Where(n => n.OrderId == item.OrderId && n.RequestId == requestId).FirstOrDefault();
                    if (detail != null)
                    {
                        item.IsChecked = true;
                        item.Id = detail.Id;
                        item.AmountPayment = detail.Amount;
                    }
                    item.TotalDisarmed = listRequestDetail.Where(n => n.OrderId == item.OrderId).Sum(n => n.Amount);
                    item.TotalNeedPayment = item.Amount - item.TotalDisarmed;
                    if (item.TotalNeedPayment < 0)
                        item.TotalNeedPayment = 0;
                    PaymentRequestViewModel model = new PaymentRequestViewModel();
                    item.CopyProperties(model);
                    if (requestId > 0)
                    {
                        var requestServiceDetails = paymentRequestDAL.GetRequestDetail(requestId,
                       ProcedureConstants.sp_GetAllServiceByRequestiD).ToList<PaymentRequestViewModel>();
                        foreach (var service in requestServiceDetails)
                        {
                            if (listService.FirstOrDefault(n => n.ServiceId == service.ServiceId) == null)
                            {
                                service.IsChecked = true;
                                service.IsDisabled = true;
                                service.AmountPayment = service.AmountPay;
                                listService.Add(service);
                            }
                        }
                    }
                    if (listService.FirstOrDefault(n => n.ServiceId == item.ServiceId) == null && item.TotalNeedPayment > 0)
                        listService.Add(item);
                }
                return listService;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetServiceListBySupplierId - PaymentRequestRepository: " + ex);
            }
            return new List<PaymentRequestViewModel>();
        }

        public List<PaymentRequestViewModel> GetByClientId(long clientId, int paymentVoucherId = 0)
        {
            try
            {
                var listPaymentRequest = paymentRequestDAL.GetServiceListByClientId(clientId,
                    ProcedureConstants.SP_GetListPaymentRequestByClientId).ToList<PaymentRequestViewModel>().Where(n => n.Status == (int)PAYMENT_REQUEST_STATUS.CHO_CHI).ToList();
                var listPaymentRequestOutput = new List<PaymentRequestViewModel>();
                var listPaymentRequestExists = paymentRequestDAL.GetPaymentRequestExists(listPaymentRequest.Select(n => n.Id).ToList(),
                    ProcedureConstants.SP_CheckCreatePaymentVoucher).ToList<PaymentRequestViewModel>();
                var listRequetIdExists = new List<long>();
                foreach (var item in listPaymentRequestExists)
                {
                    var requestIds = item.RequestIds.Split(",");
                    foreach (var requestId in requestIds)
                    {
                        if (!string.IsNullOrEmpty(requestId))
                            listRequetIdExists.Add(int.Parse(requestId));
                    }
                }
                if (listPaymentRequestExists.Count > 0)
                {
                    foreach (var item in listPaymentRequest)
                    {
                        PaymentRequestViewModel model = new PaymentRequestViewModel();
                        var exists = listRequetIdExists.Contains(item.Id);
                        if (!exists)
                        {
                            item.CopyProperties(model);
                            listPaymentRequestOutput.Add(model);
                        }
                    }
                }
                else
                {
                    listPaymentRequestOutput = listPaymentRequest;
                }
                var paymentVoucher = paymentVoucherDAL.GetById(paymentVoucherId);
                if (paymentVoucher != null)
                {
                    var listRequest = paymentRequestDAL.GetByIds(
                   paymentVoucher.RequestId.Split(",").Select(n => Convert.ToInt64(n)).ToList());
                    if (paymentVoucherId > 0)
                    {
                        foreach (var item in listRequest)
                        {
                            var record = listPaymentRequest.FirstOrDefault(n => n.Id == item.Id);
                            if (record != null && listPaymentRequestOutput.FirstOrDefault(n => n.Id == item.Id) == null)
                            {
                                record.IsChecked = true;
                                listPaymentRequestOutput.Add(record);
                            }

                        }
                    }
                }
                return listPaymentRequestOutput;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByClientId - PaymentRequestRepository: " + ex);
            }
            return new List<PaymentRequestViewModel>();
        }

        public List<PaymentRequestViewModel> GetBySupplierId(long supplierId, int paymentVoucherId = 0, string requestType = "1,2")
        {
            try
            {
                var listPaymentRequest = paymentRequestDAL.GetServiceListBySupplierId(supplierId,
                    ProcedureConstants.SP_GetListPaymentRequestBySupplierId, requestType).ToList<PaymentRequestViewModel>();
                var listPaymentRequestOutput = new List<PaymentRequestViewModel>();
                var listPaymentRequestExists = paymentRequestDAL.GetPaymentRequestExists(listPaymentRequest.Select(n => n.Id).ToList(),
                    ProcedureConstants.SP_CheckCreatePaymentVoucher).ToList<PaymentRequestViewModel>();
                var listRequetIdExists = new List<long>();
                foreach (var item in listPaymentRequestExists)
                {
                    var requestIds = item.RequestIds.Split(",");
                    foreach (var requestId in requestIds)
                    {
                        if (!string.IsNullOrEmpty(requestId))
                            listRequetIdExists.Add(int.Parse(requestId));
                    }
                }
                if (listPaymentRequestExists.Count > 0)
                {
                    foreach (var item in listPaymentRequest)
                    {
                        PaymentRequestViewModel model = new PaymentRequestViewModel();
                        var exists = listRequetIdExists.Contains(item.Id);
                        if (!exists)
                        {
                            item.CopyProperties(model);
                            listPaymentRequestOutput.Add(model);
                        }
                    }
                }
                else
                {
                    listPaymentRequestOutput = listPaymentRequest;
                }
                var paymentVoucher = paymentVoucherDAL.GetById(paymentVoucherId);
                if (paymentVoucher != null && !string.IsNullOrEmpty(paymentVoucher.RequestId))
                {
                    var listRequest = paymentRequestDAL.GetByIds(
                   paymentVoucher.RequestId.Split(",").Select(n => Convert.ToInt64(n)).ToList());
                    if (paymentVoucherId > 0)
                    {
                        foreach (var item in listRequest)
                        {
                            PaymentRequestViewModel model = new PaymentRequestViewModel();
                            item.CopyProperties(model);
                            model.IsChecked = true;
                            listPaymentRequestOutput.Add(model);
                        }
                    }
                }

                return listPaymentRequestOutput;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetBySupplierId - PaymentRequestRepository: " + ex);
            }
            return new List<PaymentRequestViewModel>();
        }

        public List<PaymentRequestViewModel> GetByServiceId(long serviceId, int type, int requestType = 0)
        {
            try
            {
                var listServiceOutput = paymentRequestDAL.GetListPaymentRequestByServiceId(serviceId, type,
                    ProcedureConstants.sp_GetListPaymentRequestByServiceId, requestType).ToList<PaymentRequestViewModel>();
                foreach (var item in listServiceOutput)
                {
                    item.ListServiceCodeAndType = new List<CountStatus>();
                    if (!string.IsNullOrEmpty(item.PaymentVoucherCode))
                    {
                        var listPaymentVoucher = paymentVoucherDAL.GetByPaymentCodes(item.PaymentVoucherCode.Split(",").ToList());
                        foreach (var paymentVoucher in listPaymentVoucher)
                        {
                            CountStatus countStatus = new CountStatus();
                            countStatus.DataNo = paymentVoucher.PaymentCode;
                            countStatus.DataId = paymentVoucher.Id;
                            item.ListServiceCodeAndType.Add(countStatus);
                        }
                    }
                }
                return listServiceOutput;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByServiceId - PaymentRequestRepository: " + ex);
            }
            return new List<PaymentRequestViewModel>();
        }

        public List<PaymentRequestViewModel> GetRequestByClientId(long clientId, long orderid = 0)
        {
            try
            {
                var listPaymentRequest = paymentRequestDAL.GetServiceListByClientId(clientId,
                    ProcedureConstants.SP_GetListPaymentRequestByClientId).ToList<PaymentRequestViewModel>();
                foreach (var item in listPaymentRequest)
                {
                    item.ListServiceCodeAndType = new List<CountStatus>();
                    if (!string.IsNullOrEmpty(item.PaymentVoucherCode))
                    {
                        var listPaymentVoucher = paymentVoucherDAL.GetByPaymentCodes(item.PaymentVoucherCode.Split(",").ToList());
                        foreach (var paymentVoucher in listPaymentVoucher)
                        {
                            CountStatus countStatus = new CountStatus();
                            countStatus.DataNo = paymentVoucher.PaymentCode;
                            countStatus.DataId = paymentVoucher.Id;
                            item.ListServiceCodeAndType.Add(countStatus);
                        }
                    }
                }
                if (orderid > 0)
                {
                    var listTemp = new List<PaymentRequestViewModel>();
                    foreach (var item in listPaymentRequest)
                    {
                        var requestDetails = paymentRequestDAL.GetByPaymentRequestId((int)item.Id);
                        if (requestDetails.FirstOrDefault(n => n.OrderId == orderid) != null)
                            listTemp.Add(item);
                    }
                    listPaymentRequest = listTemp;
                }
                return listPaymentRequest;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByServiceId - PaymentRequestRepository: " + ex);
            }
            return new List<PaymentRequestViewModel>();
        }

        public int DeletePaymentRequest(string paymentRequestNo, int userId)
        {
            var entity = paymentRequestDAL.GetByRequestNo(paymentRequestNo);
            entity.UpdatedBy = userId;
            entity.IsDelete = true;
            entity.UpdatedDate = DateTime.Now;
            return paymentRequestDAL.UpdateRequest(entity);
        }
        public List<OrderPaymentRequest> GetListPaymentRequestByOrderId(int Orderid)
        {
            try
            {
                var dt = paymentRequestDAL.GetListPaymentRequestByOrderId(Orderid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<OrderPaymentRequest>();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListPaymentRequestByOrderId - PaymentRequestRepository: " + ex);
            }
            return null;
        }

    }
}
