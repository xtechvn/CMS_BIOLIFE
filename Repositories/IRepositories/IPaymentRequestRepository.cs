using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Funding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.IRepositories
{
    public interface IPaymentRequestRepository
    {

        int CreatePaymentRequest(PaymentRequestViewModel model);
        PaymentRequestViewModel GetById(int paymentRequestId);
        PaymentRequest GetByRequestNo(string paymentRequestNo);
        List<CountStatus> GetCountStatus(PaymentRequestSearchModel searchModel);
        int UpdatePaymentRequest(PaymentRequestViewModel model);
        int ApprovePaymentRequest(string paymentRequestNo, int userId, int status);
        int RejectPaymentRequest(string paymentRequestNo, string noteReject, int userId);
        int UndoApprove(string paymentRequestNo, string note, int userId, int status);
        List<PaymentRequestViewModel> GetServiceListBySupplierId(long supplierId, int requestId = 0, int serviceId = 0);
        List<PaymentRequestViewModel> GetServiceListByClientId(long clientId, int requestId = 0);
        List<PaymentRequestViewModel> GetByClientId(long clientId, int paymentVoucherId = 0);
        List<PaymentRequestViewModel> GetBySupplierId(long supplierId, int paymentVoucherId = 0, string requestType = "1,2");
        List<PaymentRequestViewModel> GetByServiceId(long serviceId, int type, int requestType = 0);
        List<PaymentRequestViewModel> GetRequestByClientId(long clientId, long orderid = 0);
        int DeletePaymentRequest(string paymentRequestNo, int userId);
        List<OrderPaymentRequest> GetListPaymentRequestByOrderId(int Orderid);
    }
}
