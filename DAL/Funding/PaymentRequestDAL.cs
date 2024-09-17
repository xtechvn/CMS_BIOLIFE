using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Funding;
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
    public class PaymentRequestDAL : GenericService<PaymentRequest>
    {
        private static DbWorker _DbWorker;
        public PaymentRequestDAL(string connection) : base(connection)
        {
            _connection = connection;
            _DbWorker = new DbWorker(connection);
        }

        public PaymentRequest GetById(long paymentRequestId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.PaymentRequests.AsNoTracking().FirstOrDefault(x => x.Id == paymentRequestId);
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetById - PaymentRequestDAL: " + ex);
                return null;
            }
        }

        public PaymentRequest GetByRequestNo(string paymentRequestNo)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.PaymentRequests.AsNoTracking().FirstOrDefault(x => x.PaymentCode == paymentRequestNo);
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetById - PaymentRequestDAL: " + ex);
                return null;
            }
        }

        public PaymentRequest GetByPaymentCode(string paymentCode)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.PaymentRequests.AsNoTracking().FirstOrDefault(x => x.PaymentCode == paymentCode);
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByPaymentCode - PaymentRequestDAL: " + ex);
                return null;
            }
        }

        public List<PaymentRequest> GetByPaymentCodes(List<string> paymentCodes)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var requests = _DbContext.PaymentRequests.AsNoTracking().Where(x => paymentCodes.Contains(x.PaymentCode)).ToList();
                    if (requests != null)
                    {
                        return requests;
                    }
                }
                return new List<PaymentRequest>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByPaymentCode - PaymentRequestDAL: " + ex);
                return null;
            }
        }

        public DataTable GetPagingList(PaymentRequestSearchModel searchModel, int currentPage, int pageSize, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[21];
                objParam[0] = new SqlParameter("@PaymentCode", searchModel.PaymentCode);
                objParam[1] = new SqlParameter("@Description", searchModel.Content);
                if (searchModel.StatusMulti == null || searchModel.StatusMulti.Count == 0)
                    objParam[2] = new SqlParameter("@PaymentStatus", -1);
                else
                    objParam[2] = new SqlParameter("@PaymentStatus", string.Join(",", searchModel.StatusMulti));
                if (searchModel.TypeMulti == null || searchModel.TypeMulti.Count == 0)
                    objParam[3] = new SqlParameter("@PaymentRequestType", -1);
                else
                    objParam[3] = new SqlParameter("@PaymentRequestType", string.Join(",", searchModel.TypeMulti));
                if (searchModel.PaymentTypeMulti == null || searchModel.PaymentTypeMulti.Count == 0)
                    objParam[4] = new SqlParameter("@PayType", -1);
                else
                    objParam[4] = new SqlParameter("@PayType", string.Join(",", searchModel.PaymentTypeMulti));
                if (searchModel.SupplierId == 0)
                    objParam[5] = new SqlParameter("@SupplierID", DBNull.Value);
                else
                    objParam[5] = new SqlParameter("@SupplierID", searchModel.SupplierId);
                if (searchModel.ClientId == 0)
                    objParam[16] = new SqlParameter("@ClientId", DBNull.Value);
                else
                    objParam[16] = new SqlParameter("@ClientId", searchModel.ClientId);
                objParam[6] = new SqlParameter("@PaymentDateFrom", searchModel.PaymentDateFrom);
                objParam[7] = new SqlParameter("@PaymentDateTo", searchModel.PaymentDateTo);
                if (searchModel.CreateByIds == null || searchModel.CreateByIds.Count == 0)
                {
                    objParam[8] = new SqlParameter("@UserCreate", DBNull.Value);
                }
                else
                {
                    objParam[8] = new SqlParameter("@UserCreate", string.Join(",", searchModel.CreateByIds));
                }
                objParam[9] = new SqlParameter("@CreateDateFrom", searchModel.FromCreateDate);
                objParam[10] = new SqlParameter("@CreateDateTo", searchModel.ToCreateDate);
                if (searchModel.VerifyByIds == null || searchModel.VerifyByIds.Count == 0)
                {
                    objParam[11] = new SqlParameter("@UserVerify", DBNull.Value);
                }
                else
                {
                    objParam[11] = new SqlParameter("@UserVerify", string.Join(",", searchModel.VerifyByIds));
                }
                objParam[12] = new SqlParameter("@VerifyDateFrom", searchModel.VerifyDateFrom);
                objParam[13] = new SqlParameter("@VerifyDateTo", searchModel.VerifyDateTo);
                if (pageSize == -1)
                {
                    objParam[14] = new SqlParameter("@PageIndex", -1);
                    objParam[15] = new SqlParameter("@PageSize", DBNull.Value);
                }
                else
                {
                    objParam[14] = new SqlParameter("@PageIndex", currentPage);
                    objParam[15] = new SqlParameter("@PageSize", pageSize);
                }
                if (searchModel.IsSupplierDebt == null)
                    objParam[17] = new SqlParameter("@IsSupplierDebt", DBNull.Value);
                else
                    objParam[17] = new SqlParameter("@IsSupplierDebt", searchModel.IsSupplierDebt);
                objParam[18] = new SqlParameter("@ServiceCode", searchModel.ServiceCode);
                objParam[19] = new SqlParameter("@OrderNo", searchModel.OrderNo);
                if (searchModel.IsPaymentBefore == null)
                    objParam[20] = new SqlParameter("@IsPaymentBefore", DBNull.Value);
                else
                    objParam[20] = new SqlParameter("@IsPaymentBefore", searchModel.IsPaymentBefore);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - PaymentRequestDAL: " + ex);
            }
            return null;
        }

        public DataTable GetCountStatus(PaymentRequestSearchModel searchModel, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[19];
                objParam[0] = new SqlParameter("@PaymentCode", searchModel.PaymentCode);
                objParam[1] = new SqlParameter("@Description", searchModel.Content);
                if (searchModel.StatusMulti == null || searchModel.StatusMulti.Count == 0)
                    objParam[2] = new SqlParameter("@PaymentStatus", -1);
                else
                    objParam[2] = new SqlParameter("@PaymentStatus", string.Join(",", searchModel.StatusMulti));
                if (searchModel.TypeMulti == null || searchModel.TypeMulti.Count == 0)
                    objParam[3] = new SqlParameter("@PaymentRequestType", -1);
                else
                    objParam[3] = new SqlParameter("@PaymentRequestType", string.Join(",", searchModel.TypeMulti));
                if (searchModel.PaymentTypeMulti == null || searchModel.PaymentTypeMulti.Count == 0)
                    objParam[4] = new SqlParameter("@PayType", -1);
                else
                    objParam[4] = new SqlParameter("@PayType", string.Join(",", searchModel.PaymentTypeMulti));
                if (searchModel.SupplierId == 0)
                    objParam[5] = new SqlParameter("@SupplierID", DBNull.Value);
                else
                    objParam[5] = new SqlParameter("@SupplierID", searchModel.SupplierId);
                objParam[6] = new SqlParameter("@PaymentDateFrom", searchModel.PaymentDateFrom);
                objParam[7] = new SqlParameter("@PaymentDateTo", searchModel.PaymentDateTo);
                if (searchModel.CreateByIds == null || searchModel.CreateByIds.Count == 0)
                {
                    objParam[8] = new SqlParameter("@UserCreate", DBNull.Value);
                }
                else
                {
                    objParam[8] = new SqlParameter("@UserCreate", string.Join(",", searchModel.CreateByIds));
                }
                objParam[9] = new SqlParameter("@CreateDateFrom", searchModel.FromCreateDate);
                objParam[10] = new SqlParameter("@CreateDateTo", searchModel.ToCreateDate);
                if (searchModel.VerifyByIds == null || searchModel.VerifyByIds.Count == 0)
                {
                    objParam[11] = new SqlParameter("@UserVerify", DBNull.Value);
                }
                else
                {
                    objParam[11] = new SqlParameter("@UserVerify", string.Join(",", searchModel.VerifyByIds));
                }
                objParam[12] = new SqlParameter("@VerifyDateFrom", searchModel.VerifyDateFrom);
                objParam[13] = new SqlParameter("@VerifyDateTo", searchModel.VerifyDateTo);
                if (searchModel.ClientId == 0)
                    objParam[14] = new SqlParameter("@ClientId", DBNull.Value);
                else
                    objParam[14] = new SqlParameter("@ClientId", searchModel.ClientId);
                if (searchModel.IsSupplierDebt == null)
                    objParam[15] = new SqlParameter("@IsSupplierDebt", DBNull.Value);
                else
                    objParam[15] = new SqlParameter("@IsSupplierDebt", searchModel.IsSupplierDebt);
                objParam[16] = new SqlParameter("@ServiceCode", searchModel.ServiceCode);
                objParam[17] = new SqlParameter("@OrderNo", searchModel.OrderNo);
                if (searchModel.IsPaymentBefore == null)
                    objParam[18] = new SqlParameter("@IsPaymentBefore", DBNull.Value);
                else
                    objParam[18] = new SqlParameter("@IsPaymentBefore", searchModel.IsPaymentBefore);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetCountStatus - PaymentRequestDAL: " + ex);
            }
            return null;
        }

        public DataTable GetServiceListBySupplierId(long supplierId,  string proc, string requestType = "1,2")
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@SupplierId", supplierId);
                objParam[1] = new SqlParameter("@Status", DBNull.Value);
                objParam[2] = new SqlParameter("@RequestType", requestType);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetServiceListBySupplierId - PaymentRequestDAL: " + ex);
            }
            return null;
        }

        public DataTable GetListPaymentRequestByServiceId(long serviceId, int type, string proc, int requestType = 0)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@ServiceId", serviceId);
                objParam[1] = new SqlParameter("@Type", type);
                if (requestType != 0)
                {
                    objParam[2] = new SqlParameter("@RequestType", requestType);
                }
                else
                {
                    objParam[2] = new SqlParameter("@RequestType", (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_DICH_VU + "," + (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_KHAC);
                }
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetServiceListBySupplierId - PaymentRequestDAL: " + ex);
            }
            return null;
        }

        public DataTable GetServiceListByClientId(long clientId, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ClientId", clientId);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetServiceListByClientId - PaymentRequestDAL: " + ex);
            }
            return null;
        }

        public DataTable GetPaymentRequestExists(List<long> requestIds, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@RequestId", string.Join(",", requestIds));
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPaymentRequestExists - PaymentRequestDAL: " + ex);
            }
            return null;
        }

        public DataTable GetRequestDetail(long requestId, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@RequestId", requestId);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetRequestDetail - PaymentRequestDAL: " + ex);
            }
            return null;
        }

        public DataTable GetDetailServiceById(long serviceId, int serviceType, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@ServiceId", serviceId);
                objParam[1] = new SqlParameter("@Type", serviceType);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetRequestDetail - PaymentRequestDAL: " + ex);
            }
            return null;
        }

        public int CreatePaymentRequest(PaymentRequestViewModel model)
        {
            int id = 0;
            List<int> detailIds = new List<int>();
            try
            {
                SqlParameter[] objParam_paymentRequest = new SqlParameter[18];
                objParam_paymentRequest[0] = new SqlParameter("@PaymentCode", model.PaymentCode);
                objParam_paymentRequest[1] = new SqlParameter("@Type", model.Type);
                objParam_paymentRequest[2] = new SqlParameter("@PaymentType", model.PaymentType);
                if (model.Type == (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_DICH_VU || model.Type == (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_KHAC
                     || model.Type == (int)PAYMENT_VOUCHER_TYPE.CHI_PHI_MARKETING)
                {
                    objParam_paymentRequest[3] = new SqlParameter("@SupplierId", model.SupplierId);
                    objParam_paymentRequest[4] = new SqlParameter("@ClientId", Convert.ToInt32(0));
                }
                else
                {
                    objParam_paymentRequest[3] = new SqlParameter("@SupplierId", Convert.ToInt32(0));
                    objParam_paymentRequest[4] = new SqlParameter("@ClientId", model.ClientId);
                }
                objParam_paymentRequest[5] = new SqlParameter("@Amount", model.Amount);
                if (model.PaymentType == (int)DepositHistoryConstant.CONTRACT_PAYMENT_TYPE.CHUYEN_KHOAN)
                {
                    objParam_paymentRequest[6] = new SqlParameter("@BankingAccountId", Convert.ToInt32(model.BankingAccountId));
                }
                else
                {
                    objParam_paymentRequest[6] = new SqlParameter("@BankingAccountId", Convert.ToInt32(0));
                }
                objParam_paymentRequest[7] = new SqlParameter("@Description", string.IsNullOrEmpty(model.Description)
                    ? DBNull.Value.ToString() : model.Description);
                objParam_paymentRequest[8] = new SqlParameter("@Note", string.IsNullOrEmpty(model.Note)
                    ? DBNull.Value.ToString() : model.Note);
                if (model.IsServiceIncluded == null)
                    objParam_paymentRequest[9] = new SqlParameter("@IsServiceIncluded", DBNull.Value);
                else
                    objParam_paymentRequest[9] = new SqlParameter("@IsServiceIncluded", model.IsServiceIncluded);
                if (model.IsSend == 1)
                    objParam_paymentRequest[10] = new SqlParameter("@Status", Convert.ToInt32((int)PAYMENT_REQUEST_STATUS.CHO_TBP_DUYET));
                else
                    objParam_paymentRequest[10] = new SqlParameter("@Status", Convert.ToInt32((int)PAYMENT_REQUEST_STATUS.LUU_NHAP));
                objParam_paymentRequest[11] = new SqlParameter("@PaymentDate", model.PaymentDate);
                objParam_paymentRequest[12] = new SqlParameter("@CreatedBy", model.CreatedBy);
                objParam_paymentRequest[13] = new SqlParameter("@CreatedDate", DateTime.Now);
                if (model.IsSupplierDebt == null)
                    objParam_paymentRequest[14] = new SqlParameter("@IsSupplierDebt", DBNull.Value);
                else
                    objParam_paymentRequest[14] = new SqlParameter("@IsSupplierDebt", model.IsSupplierDebt);
                objParam_paymentRequest[15] = new SqlParameter("@BankName", string.IsNullOrEmpty(model.BankName)
                   ? DBNull.Value.ToString() : model.BankName);
                objParam_paymentRequest[16] = new SqlParameter("@BankAccount", string.IsNullOrEmpty(model.BankAccount)
                   ? DBNull.Value.ToString() : model.BankAccount);
                if (model.IsPaymentBefore == null)
                    objParam_paymentRequest[17] = new SqlParameter("@IsPaymentBefore", false);
                else
                    objParam_paymentRequest[17] = new SqlParameter("@IsPaymentBefore", model.IsPaymentBefore);
                id = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertPaymentRequest, objParam_paymentRequest);
                if (id > 0 && model.PaymentRequestDetails != null)
                {
                    foreach (var item in model.PaymentRequestDetails)
                    {
                        var detailId = 0;
                        SqlParameter[] objParam_requestDetail = new SqlParameter[8];
                        objParam_requestDetail[0] = new SqlParameter("@RequestId", id);
                        objParam_requestDetail[1] = new SqlParameter("@OrderId", item.OrderId);
                        objParam_requestDetail[2] = new SqlParameter("@CreatedBy", model.CreatedBy);
                        objParam_requestDetail[3] = new SqlParameter("@Amount", item.AmountPayment);
                        objParam_requestDetail[4] = new SqlParameter("@CreatedDate", DateTime.Now);
                        objParam_requestDetail[5] = new SqlParameter("@ServiceId", Convert.ToInt64(item.ServiceId));
                        objParam_requestDetail[6] = new SqlParameter("@Type", Convert.ToInt32(item.ServiceType));
                        objParam_requestDetail[7] = new SqlParameter("@ServiceCode", string.IsNullOrEmpty(item.ServiceCode) ? DBNull.Value.ToString() : item.ServiceCode);
                        detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertPaymentRequestDetail, objParam_requestDetail);
                        if (detailId > 0)
                            detailIds.Add(detailId);
                        if (detailId <= 0)
                        {
                            DeleteRequest(id, detailIds);
                            return -1;
                        }
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CreatePaymentRequest - PaymentRequestDAL. " + ex);
                DeleteRequest(id, detailIds);
                return -1;
            }
        }

        private void DeleteRequest(long id, List<int> detailIds)
        {
            using (var _DbContext = new EntityDataContext(_connection))
            {
                var entity = _DbContext.PaymentRequests.Find(id);
                _DbContext.PaymentRequests.Remove(entity);
                foreach (var idDetail in detailIds)
                {
                    var detail = _DbContext.PaymentRequestDetails.Find(idDetail);
                    _DbContext.PaymentRequestDetails.Remove(detail);
                }
                _DbContext.SaveChanges();
            }
        }

        public int UpdatePaymentRequest(PaymentRequestViewModel model)
        {
            int id = 0;
            List<int> detailIds = new List<int>();
            try
            {
                SqlParameter[] objParam_paymentRequest = new SqlParameter[22];
                objParam_paymentRequest[0] = new SqlParameter("@Id", model.Id);
                objParam_paymentRequest[1] = new SqlParameter("@PaymentCode", model.PaymentCode);
                objParam_paymentRequest[2] = new SqlParameter("@Type", model.Type);
                objParam_paymentRequest[3] = new SqlParameter("@PaymentType", model.PaymentType);
                if (model.Type == (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_DICH_VU || model.Type == (int)PAYMENT_VOUCHER_TYPE.THANH_TOAN_KHAC
                      || model.Type == (int)PAYMENT_VOUCHER_TYPE.CHI_PHI_MARKETING)
                {
                    objParam_paymentRequest[4] = new SqlParameter("@SupplierId", model.SupplierId);
                    objParam_paymentRequest[5] = new SqlParameter("@ClientId", DBNull.Value);
                }
                else
                {
                    objParam_paymentRequest[4] = new SqlParameter("@SupplierId", DBNull.Value);
                    objParam_paymentRequest[5] = new SqlParameter("@ClientId", model.ClientId);
                }
                objParam_paymentRequest[6] = new SqlParameter("@Amount", model.Amount);
                if (model.PaymentType == (int)DepositHistoryConstant.CONTRACT_PAYMENT_TYPE.CHUYEN_KHOAN)
                {
                    objParam_paymentRequest[7] = new SqlParameter("@BankingAccountId", model.BankingAccountId);
                }
                else
                {
                    objParam_paymentRequest[7] = new SqlParameter("@BankingAccountId", DBNull.Value);
                }
                objParam_paymentRequest[8] = new SqlParameter("@Description", string.IsNullOrEmpty(model.Description)
                    ? DBNull.Value.ToString() : model.Description);
                objParam_paymentRequest[9] = new SqlParameter("@Note", string.IsNullOrEmpty(model.Note)
                    ? DBNull.Value.ToString() : model.Note);
                if (model.IsServiceIncluded == null)
                    objParam_paymentRequest[10] = new SqlParameter("@IsServiceIncluded", DBNull.Value);
                else
                    objParam_paymentRequest[10] = new SqlParameter("@IsServiceIncluded", model.IsServiceIncluded);
                if (model.IsSend == 1)
                    objParam_paymentRequest[11] = new SqlParameter("@Status", Convert.ToInt32((int)PAYMENT_REQUEST_STATUS.CHO_TBP_DUYET));
                else
                    objParam_paymentRequest[11] = new SqlParameter("@Status", Convert.ToInt32(model.Status));
                objParam_paymentRequest[12] = new SqlParameter("@PaymentDate", model.PaymentDate);
                objParam_paymentRequest[13] = new SqlParameter("@UserVerify", DBNull.Value);
                objParam_paymentRequest[14] = new SqlParameter("@VerifyDate ", DBNull.Value);
                objParam_paymentRequest[15] = new SqlParameter("@DeclineReason ", DBNull.Value);
                objParam_paymentRequest[16] = new SqlParameter("@UpdatedBy", model.UpdatedBy);
                objParam_paymentRequest[17] = new SqlParameter("@UpdatedDate", DateTime.Now);
                if (model.IsSupplierDebt == null)
                    objParam_paymentRequest[18] = new SqlParameter("@IsSupplierDebt", DBNull.Value);
                else
                    objParam_paymentRequest[18] = new SqlParameter("@IsSupplierDebt", model.IsSupplierDebt);
                objParam_paymentRequest[19] = new SqlParameter("@BankName", string.IsNullOrEmpty(model.BankName)
                    ? DBNull.Value.ToString() : model.BankName);
                objParam_paymentRequest[20] = new SqlParameter("@BankAccount", string.IsNullOrEmpty(model.BankAccount)
                   ? DBNull.Value.ToString() : model.BankAccount);
                if (model.IsPaymentBefore == null)
                    objParam_paymentRequest[21] = new SqlParameter("@IsPaymentBefore", false);
                else
                    objParam_paymentRequest[21] = new SqlParameter("@IsPaymentBefore", model.IsPaymentBefore);
                id = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdatePaymentRequest, objParam_paymentRequest);
                if (id > 0 && model.PaymentRequestDetails != null)
                {
                    foreach (var item in model.PaymentRequestDetails)
                    {
                        var detailId = 0;
                        if (item.Id == 0)
                        {
                            SqlParameter[] objParam_requestDetail = new SqlParameter[8];
                            objParam_requestDetail[0] = new SqlParameter("@RequestId", id);
                            objParam_requestDetail[1] = new SqlParameter("@OrderId", item.OrderId);
                            objParam_requestDetail[5] = new SqlParameter("@ServiceId", Convert.ToInt64(item.ServiceId));
                            objParam_requestDetail[6] = new SqlParameter("@Type", Convert.ToInt32(item.ServiceType));
                            objParam_requestDetail[2] = new SqlParameter("@CreatedBy", model.UpdatedBy);
                            objParam_requestDetail[3] = new SqlParameter("@Amount", item.AmountPayment);
                            objParam_requestDetail[4] = new SqlParameter("@CreatedDate", DateTime.Now);
                            objParam_requestDetail[7] = new SqlParameter("@ServiceCode", string.IsNullOrEmpty(item.ServiceCode) ? DBNull.Value.ToString() : item.ServiceCode);
                            detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertPaymentRequestDetail, objParam_requestDetail);
                            if (detailId > 0)
                                detailIds.Add(detailId);
                        }
                        else
                        {
                            SqlParameter[] objParam_requestDetail = new SqlParameter[8];
                            objParam_requestDetail[0] = new SqlParameter("@Id", item.Id);
                            objParam_requestDetail[1] = new SqlParameter("@RequestId", id);
                            objParam_requestDetail[5] = new SqlParameter("@ServiceId", Convert.ToInt64(item.ServiceId));
                            objParam_requestDetail[6] = new SqlParameter("@Type", Convert.ToInt32(item.ServiceType));
                            objParam_requestDetail[2] = new SqlParameter("@OrderId", item.OrderId);
                            objParam_requestDetail[3] = new SqlParameter("@UpdatedBy", model.UpdatedBy);
                            objParam_requestDetail[4] = new SqlParameter("@Amount", item.AmountPayment);
                            objParam_requestDetail[7] = new SqlParameter("@ServiceCode", string.IsNullOrEmpty(item.ServiceCode) ? DBNull.Value.ToString() : item.ServiceCode);
                            detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdatePaymentRequestDetail, objParam_requestDetail);
                            if (detailId > 0)
                                detailIds.Add(detailId);
                        }
                    }
                }
                var request = GetById(id);
                if (request.IsServiceIncluded != true)
                {
                    var listDetail = GetByRequestId(model.Id);
                    foreach (var item in listDetail)
                    {
                        var exists = detailIds.FirstOrDefault(n => n == item.Id);
                        if (exists == 0)
                        {
                            DeletePaymentRequestDetail(item);
                        }
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdatePaymentRequest - PaymentRequestDAL. " + ex);
                return -1;
            }
        }

        public List<PaymentRequestDetail> GetByRequestId(long requestId)
        {
            try
            {

                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var contractPays = _DbContext.PaymentRequestDetails.Where(x => x.RequestId == requestId).ToList();
                    if (contractPays != null)
                    {
                        return contractPays;
                    }
                }
                return new List<PaymentRequestDetail>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByRequestId - PaymentRequestDAL: " + ex);
                return new List<PaymentRequestDetail>();
            }
        }

        public List<PaymentRequest> GetByIds(List<long> ids)
        {
            try
            {

                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var requests = _DbContext.PaymentRequests.Where(x => ids.Contains(x.Id)).ToList();
                    return requests;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByIds - PaymentRequestDAL: " + ex);
                return new List<PaymentRequest>();
            }
        }

        public int DeletePaymentRequestDetail(PaymentRequestDetail model)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    _DbContext.PaymentRequestDetails.Remove(model);
                    _DbContext.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DeletePaymentRequestDetail - PaymentRequestDAL: " + ex);
                return -1;
            }
        }

        public int UpdateRequest(PaymentRequest model)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    _DbContext.PaymentRequests.Update(model);
                    _DbContext.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ApprovePaymentRequest - PaymentRequestDAL. " + ex);
                return -1;
            }
        }

        public int UpdateRequestDetail(PaymentRequestDetail model)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    _DbContext.PaymentRequestDetails.Update(model);
                    _DbContext.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ApprovePaymentRequest - PaymentRequestDAL. " + ex);
                return -1;
            }
        }

        public int ApproveRequest(PaymentRequest model)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[5];
                objParam[0] = new SqlParameter("@Id", model.Id);
                objParam[1] = new SqlParameter("@Status", model.Status);
                objParam[2] = new SqlParameter("@OrderStatus", (int)OrderStatus.FINISHED);
                objParam[3] = new SqlParameter("@UpdatedBy", model.UpdatedBy);
                objParam[4] = new SqlParameter("@UpdatedDate", DateTime.Now);
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdatePaymentRequestStatus, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ApproveRequest - PaymentRequestDAL. " + ex);
                return -1;
            }
        }

        public int UndoApproveRequest(PaymentRequest model)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@RequestId", model.Id);
                objParam[1] = new SqlParameter("@Status", model.Status);
                objParam[2] = new SqlParameter("@Note", model.Note);
                objParam[3] = new SqlParameter("@UpdateBy", model.UpdatedBy);
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UnVerifyPaymentRequest, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UndoApproveRequest - PaymentRequestDAL. " + ex);
                return -1;
            }
        }

        public List<PaymentRequestDetailViewModel> GetByDataIds(List<long> dataIds)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    //var requestDetails = _DbContext.PaymentRequestDetail.Where(x => dataIds.Contains(x.OrderId)).ToList();
                    //if (requestDetails != null)
                    //{
                    //    return requestDetails;
                    //}
                    var paymentRequest = (from request in _DbContext.PaymentRequests.ToList()
                                          join detail in _DbContext.PaymentRequestDetails.ToList() on request.Id equals detail.RequestId
                                          where (request.IsDelete == null || request.IsDelete.Value == false)
                                          && (detail.ServiceId != null && dataIds.Contains(detail.ServiceId.Value))
                                          select new PaymentRequestDetailViewModel
                                          {
                                              OrderId = (int)detail.OrderId,
                                              Amount = detail.Amount,
                                              Type = detail.Type,
                                              ServiceId = detail.ServiceId != null ? detail.ServiceId.Value : 0,
                                              RequestId = detail.RequestId,
                                              Id = (int)detail.Id,
                                              Status = request.Status,
                                          }).ToList();
                    return paymentRequest;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByRequestId - PaymentRequestDAL: " + ex);
                return new List<PaymentRequestDetailViewModel>();
            }
        }

        public List<PaymentRequestDetail> GetByPaymentRequestId(int paymentRequestId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var requestDetails = _DbContext.PaymentRequestDetails.Where(x => x.RequestId == paymentRequestId).ToList();
                    return requestDetails;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByPaymentRequestId - PaymentRequestDAL: " + ex);
                return new List<PaymentRequestDetail>();
            }
        }

        public DataTable GetListPaymentRequestByOrderId(int orderId)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@OrderId", orderId);
                return _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetListPaymentRequestByOrderId, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListPaymentRequestByOrderId - PaymentRequestDAL: " + ex);
            }
            return null;
        }

        public List<PaymentRequestDetail> GetByRequestIds(List<long> requestIds)
        {
            try
            {

                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var contractPays = _DbContext.PaymentRequestDetails.Where(x => requestIds.Contains(x.RequestId)).ToList();
                    if (contractPays != null)
                    {
                        return contractPays;
                    }
                }
                return new List<PaymentRequestDetail>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByRequestId - PaymentRequestDAL: " + ex);
                return new List<PaymentRequestDetail>();
            }
        }
    }
}
