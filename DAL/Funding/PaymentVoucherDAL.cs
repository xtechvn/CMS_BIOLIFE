using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Funding;
using Entities.ViewModels.TransferSms;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Utilities;
using Utilities.Contants;

namespace DAL.Funding
{
    public class PaymentVoucherDAL : GenericService<PaymentVoucher>
    {
        private static DbWorker _DbWorker;
        private static PaymentRequestDAL paymentRequestDAL;

        public PaymentVoucherDAL(string connection) : base(connection)
        {
            _connection = connection;
            _DbWorker = new DbWorker(connection);
            paymentRequestDAL = new PaymentRequestDAL(connection);
        }

        public PaymentVoucher GetById(long PaymentVoucherId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.PaymentVouchers.AsNoTracking().FirstOrDefault(x => x.Id == PaymentVoucherId);
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetById - PaymentVoucherDAL: " + ex);
                return null;
            }
        }

        public PaymentVoucher GetByPaymentCode(string paymentCode)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.PaymentVouchers.AsNoTracking().FirstOrDefault(x => x.PaymentCode == paymentCode);
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByPaymentCode - PaymentVoucherDAL: " + ex);
                return null;
            }
        }

        public List<PaymentVoucher> GetByPaymentCodes(List<string> paymentCodes)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.PaymentVouchers.AsNoTracking().Where(x => paymentCodes.Contains(x.PaymentCode)).ToList();
                    return detail;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByPaymentCode - PaymentVoucherDAL: " + ex);
                return new List<PaymentVoucher>();
            }
        }

        public DataTable GetPagingList(PaymentVoucherViewModel searchModel, int currentPage, int pageSize, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[11];
                objParam[0] = new SqlParameter("@PaymentCode", searchModel.PaymentCode);
                objParam[1] = new SqlParameter("@Description", searchModel.Content);
                if (searchModel.TypeMulti == null || searchModel.TypeMulti.Count == 0)
                    objParam[2] = new SqlParameter("@PaymentVoucherType", -1);
                else
                    objParam[2] = new SqlParameter("@PaymentVoucherType", string.Join(",", searchModel.TypeMulti));
                if (searchModel.PaymentTypeMulti == null || searchModel.PaymentTypeMulti.Count == 0)
                    objParam[3] = new SqlParameter("@PayType", -1);
                else
                    objParam[3] = new SqlParameter("@PayType", string.Join(",", searchModel.PaymentTypeMulti));
                if (searchModel.SupplierId == 0)
                    objParam[4] = new SqlParameter("@SupplierID", DBNull.Value);
                else
                    objParam[4] = new SqlParameter("@SupplierID", searchModel.SupplierId);
                if (searchModel.ClientId == 0)
                    objParam[10] = new SqlParameter("@ClientId", DBNull.Value);
                else
                    objParam[10] = new SqlParameter("@ClientId", searchModel.ClientId);
                if (searchModel.CreateByIds == null || searchModel.CreateByIds.Count == 0)
                    objParam[5] = new SqlParameter("@UserCreate", DBNull.Value);
                else
                    objParam[5] = new SqlParameter("@UserCreate", string.Join(",", searchModel.CreateByIds));
                objParam[6] = new SqlParameter("@CreateDateFrom", searchModel.CreatedDateFrom);
                objParam[7] = new SqlParameter("@CreateDateTo", searchModel.CreatedDateTo);
                if (pageSize == -1)
                {
                    objParam[8] = new SqlParameter("@PageIndex", -1);
                    objParam[9] = new SqlParameter("@PageSize", DBNull.Value);
                }
                else
                {
                    objParam[8] = new SqlParameter("@PageIndex", currentPage);
                    objParam[9] = new SqlParameter("@PageSize", pageSize);
                }
                //if (searchModel.BankingAccountSource == 0)
                //    objParam[10] = new SqlParameter("@SourceAccount",DBNull.Value);
                //else
                //    objParam[10] = new SqlParameter("@SourceAccount", searchModel.BankingAccountSource);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - PaymentVoucherDAL: " + ex);
            }
            return null;
        }

        public DataTable GetDetail(long paymentVoucherId, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@PaymentVoucherId", paymentVoucherId);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetRequestDetail - PaymentVoucherDAL: " + ex);
            }
            return null;
        }

        public int CreatePaymentVoucher(PaymentVoucherViewModel model)
        {
            int id = 0;
            try
            {
                SqlParameter[] objParam_PaymentVoucher = new SqlParameter[16];
                objParam_PaymentVoucher[0] = new SqlParameter("@PaymentCode", model.PaymentCode);
                objParam_PaymentVoucher[1] = new SqlParameter("@Type", model.Type);
                objParam_PaymentVoucher[2] = new SqlParameter("@RequestId",
                    string.Join(",", model.PaymentRequestDetails.Select(n => n.Id).ToList()));
                objParam_PaymentVoucher[3] = new SqlParameter("@PaymentType", model.PaymentType);
                if (model.Type == (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_DICH_VU ||
                    model.Type == (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_KHAC ||
                    model.Type == (int)PAYMENT_VOUCHER_TYPE.CHI_PHI_MARKETING)
                {
                    objParam_PaymentVoucher[4] = new SqlParameter("@SupplierId", model.SupplierId);
                    objParam_PaymentVoucher[5] = new SqlParameter("@ClientId", Convert.ToInt32(0));
                }
                else
                {
                    objParam_PaymentVoucher[4] = new SqlParameter("@SupplierId", Convert.ToInt32(0));
                    objParam_PaymentVoucher[5] = new SqlParameter("@ClientId", model.ClientId);
                }
                objParam_PaymentVoucher[6] = new SqlParameter("@Amount", model.Amount);
                if (model.PaymentType == (int)DepositHistoryConstant.CONTRACT_PAYMENT_TYPE.CHUYEN_KHOAN)
                    objParam_PaymentVoucher[7] = new SqlParameter("@BankingAccountId", Convert.ToInt32(model.BankingAccountId));
                else
                    objParam_PaymentVoucher[7] = new SqlParameter("@BankingAccountId", Convert.ToInt32(0));
                objParam_PaymentVoucher[8] = new SqlParameter("@Description", string.IsNullOrEmpty(model.Description)
                    ? DBNull.Value.ToString() : model.Description);
                objParam_PaymentVoucher[9] = new SqlParameter("@Note", string.IsNullOrEmpty(model.Note)
                    ? DBNull.Value.ToString() : model.Note);
                objParam_PaymentVoucher[10] = new SqlParameter("@CreatedBy", model.CreatedBy);
                objParam_PaymentVoucher[11] = new SqlParameter("@CreatedDate", DateTime.Now);
                objParam_PaymentVoucher[12] = new SqlParameter("@AttachFiles", string.IsNullOrEmpty(model.AttachFiles) ? DBNull.Value.ToString() : model.AttachFiles);
                objParam_PaymentVoucher[13] = new SqlParameter("@BankName", string.IsNullOrEmpty(model.BankName)
                  ? DBNull.Value.ToString() : model.BankName);
                objParam_PaymentVoucher[14] = new SqlParameter("@BankAccount", string.IsNullOrEmpty(model.BankAccount)
                   ? DBNull.Value.ToString() : model.BankAccount);
                if (model.SourceAccount == null || model.SourceAccount == 0)
                    objParam_PaymentVoucher[15] = new SqlParameter("@SourceAccount", DBNull.Value);
                else
                    objParam_PaymentVoucher[15] = new SqlParameter("@SourceAccount", Convert.ToInt32(model.SourceAccount));
                id = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertPaymentVoucher, objParam_PaymentVoucher);
                UpdatePaymentRequestStatus(model.PaymentRequestDetails.Select(n => n.Id).ToList());
                if (model.Type == (int)PAYMENT_VOUCHER_TYPE.HOAN_TRA_KHACH_HANG)
                {
                    UpdateOrderRefund(model.PaymentRequestDetails.Select(n => n.Id).ToList());
                }
                return id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CreatePaymentVoucher - PaymentVoucherDAL. " + ex);
                return -1;
            }
        }

        public int UpdatePaymentVoucher(PaymentVoucherViewModel model)
        {
            int id = 0;
            try
            {
                var paymentVoucher = GetById(model.Id);
                var listRequestId = model.PaymentRequestDetails.Select(n => n.Id).ToList();
                SqlParameter[] objParam_PaymentVoucher = new SqlParameter[17];
                objParam_PaymentVoucher[0] = new SqlParameter("@Id", model.Id);
                objParam_PaymentVoucher[1] = new SqlParameter("@PaymentCode", model.PaymentCode);
                objParam_PaymentVoucher[2] = new SqlParameter("@Type", model.Type);
                objParam_PaymentVoucher[3] = new SqlParameter("@PaymentType", model.PaymentType);
                objParam_PaymentVoucher[4] = new SqlParameter("@RequestId",
                    string.Join(",", listRequestId));
                if (model.Type == (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_DICH_VU ||
                    model.Type == (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_KHAC ||
                    model.Type == (int)PAYMENT_VOUCHER_TYPE.CHI_PHI_MARKETING)
                {
                    objParam_PaymentVoucher[5] = new SqlParameter("@SupplierId", model.SupplierId);
                    objParam_PaymentVoucher[6] = new SqlParameter("@ClientId", Convert.ToInt32(0));
                }
                else
                {
                    objParam_PaymentVoucher[5] = new SqlParameter("@SupplierId", Convert.ToInt32(0));
                    objParam_PaymentVoucher[6] = new SqlParameter("@ClientId", model.ClientId);
                }
                objParam_PaymentVoucher[7] = new SqlParameter("@Amount", model.Amount);
                if (model.PaymentType == (int)DepositHistoryConstant.CONTRACT_PAYMENT_TYPE.CHUYEN_KHOAN)
                {
                    objParam_PaymentVoucher[8] = new SqlParameter("@BankingAccountId", Convert.ToInt32(model.BankingAccountId));
                }
                else
                {
                    objParam_PaymentVoucher[8] = new SqlParameter("@BankingAccountId", Convert.ToInt32(0));
                }
                objParam_PaymentVoucher[9] = new SqlParameter("@Description", string.IsNullOrEmpty(model.Description)
                    ? DBNull.Value.ToString() : model.Description);
                objParam_PaymentVoucher[10] = new SqlParameter("@Note", string.IsNullOrEmpty(model.Note)
                    ? DBNull.Value.ToString() : model.Note);
                objParam_PaymentVoucher[11] = new SqlParameter("@UpdatedBy", model.UpdatedBy);
                objParam_PaymentVoucher[12] = new SqlParameter("@UpdatedDate", DateTime.Now);
                objParam_PaymentVoucher[13] = new SqlParameter("@AttachFiles", string.IsNullOrEmpty(model.AttachFiles) ? DBNull.Value.ToString() : model.AttachFiles);
                objParam_PaymentVoucher[14] = new SqlParameter("@BankName", string.IsNullOrEmpty(model.BankName)
                ? DBNull.Value.ToString() : model.BankName);
                objParam_PaymentVoucher[15] = new SqlParameter("@BankAccount", string.IsNullOrEmpty(model.BankAccount)
                   ? DBNull.Value.ToString() : model.BankAccount);
                if (model.SourceAccount == null || model.SourceAccount == 0)
                    objParam_PaymentVoucher[16] = new SqlParameter("@SourceAccount", DBNull.Value);
                else
                    objParam_PaymentVoucher[16] = new SqlParameter("@SourceAccount", Convert.ToInt32(model.SourceAccount));
                id = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdatePaymentVoucher, objParam_PaymentVoucher);
                UpdatePaymentRequestStatus(listRequestId);
                var listOrigin = paymentVoucher.RequestId.Split(",").Select(n => long.Parse(n)).ToList();
                var listUpdateApproveStatus = new List<long>();
                foreach (var item in listOrigin)
                {
                    var exists = listRequestId.FirstOrDefault(n => n == item);
                    if (exists == 0) listUpdateApproveStatus.Add(item);
                }
                var listRequestNew = new List<long>();
                foreach (var item in listRequestId)
                {
                    var exists = listOrigin.FirstOrDefault(n => n == item);
                    if (exists == 0) listRequestNew.Add(item);
                }
                if (model.Type == (int)PAYMENT_VOUCHER_TYPE.HOAN_TRA_KHACH_HANG)
                {
                    UpdateOrderRefund(listRequestNew);
                    UpdateOrderRefund(listUpdateApproveStatus, false);
                }
                UpdatePaymentRequestStatus(listUpdateApproveStatus, (int)PAYMENT_REQUEST_STATUS.CHO_CHI);
                return id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdatePaymentVoucher - PaymentVoucherDAL. " + ex);
                return -1;
            }
        }

        private void UpdatePaymentRequestStatus(List<long> requestIds, int status = (int)PAYMENT_REQUEST_STATUS.DA_CHI)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var entities = _DbContext.PaymentRequests.Where(n => requestIds.Contains(n.Id)).ToList();
                    foreach (var item in entities)
                    {
                        item.Status = status;
                        _DbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdatePaymentRequestStatus - PaymentVoucherDAL. " + ex);
            }
        }

        public List<PaymentVoucher> CheckExistsPaymentRequest(List<long> paymentIds, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@RequestId", string.Join(',', paymentIds));
                return _DbWorker.GetDataTable(proc, objParam).ToList<PaymentVoucher>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CheckExistsPaymentRequest - PaymentVoucherDAL: " + ex);
            }
            return new List<PaymentVoucher>();
        }

        public int UpdateOrderRefund(List<long> requestIds, bool isAddRefund = true)
        {
            try
            {
                var listRequestInfo = paymentRequestDAL.GetByIds(requestIds);
                var listRequestDetails = paymentRequestDAL.GetByRequestIds(requestIds);
                foreach (var item in listRequestInfo)
                {
                    if(item.Type == (int)PAYMENT_VOUCHER_TYPE.HOAN_TRA_KHACH_HANG)
                    {
                        var listDetail = listRequestDetails.Where(n => n.RequestId == item.Id).ToList();
                        foreach (var detail in listDetail)
                        {
                            SqlParameter[] objParam_orderRefund = new SqlParameter[2];
                            objParam_orderRefund[0] = new SqlParameter("@OrderID", detail.OrderId);
                            if (isAddRefund)
                                objParam_orderRefund[1] = new SqlParameter("@Refund", detail.Amount);
                            else
                                objParam_orderRefund[1] = new SqlParameter("@Refund", -detail.Amount);
                            _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateOrderRefund, objParam_orderRefund);
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateOrderRefund - PaymentVoucherDAL: " + ex);
                return -1;
            }
        }
        public List<TransferSmsTotalModel> GetTotalAmountPaymentVoucherByDate(TransferSmsSearchModel searchModel)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];

                if (!string.IsNullOrEmpty(searchModel.FromDateStr))
                {
                    objParam[0] = new SqlParameter("@FromDate ", searchModel.FromDate);
                }
                else
                {
                    objParam[0] = new SqlParameter("@FromDate ", DBNull.Value);

                }
                if (!string.IsNullOrEmpty(searchModel.ToDateStr))
                {
                    objParam[1] = new SqlParameter("@ToDate ", searchModel.ToDate);
                }
                else
                {
                    objParam[0] = new SqlParameter("@FromDate ", DBNull.Value);

                }
                var dt = _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetTotalAmountPaymentVoucherByDate, objParam);
                if(dt!=null && dt.Rows.Count > 0)
                {
                    return dt.ToList<TransferSmsTotalModel>();
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetTotalAmountPaymentVoucherByDate - PaymentVoucherDAL: " + ex);
            }
            return null;
        }
     
      
    }
}
