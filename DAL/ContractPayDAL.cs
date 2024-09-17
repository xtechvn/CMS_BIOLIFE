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
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class ContractPayDAL : GenericService<ContractPay>
    {
        private static DbWorker _DbWorker;
        private static OrderDAL orderDAL;
        public ContractPayDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
            orderDAL = new OrderDAL(connection);
        }

        public ContractPay GetById(long contractPayId)
        {
            try
            {

                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.ContractPays.AsNoTracking().FirstOrDefault(x => x.PayId == contractPayId);
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetById - ContractPayDAL: " + ex);
                return null;
            }
        }

        public ContractPay GetByBillNo(string billNo)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.ContractPays.AsNoTracking().FirstOrDefault(x => x.BillNo == billNo);
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByBillNo - ContractPayDAL: " + ex);
                return null;
            }
        }

        //public List<ContractPay> GetByOrderId(long PayId)
        //{
        //    try
        //    {

        //        using (var _DbContext = new EntityDataContext(_connection))
        //        {
        //            var detail = _DbContext.ContractPay.Where(x => x.PayId == PayId).ToList();
        //            if (detail != null)
        //            {
        //                return detail;
        //            }
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.InsertLogTelegram("GetByOrderId - ContractPayDAL: " + ex);
        //        return null;
        //    }
        //}

        public int CancelById(long id)
        {
            try
            {

                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.ContractPays.AsNoTracking().FirstOrDefault(x => x.PayId == id);
                    if (detail != null)
                    {
                        detail.PayStatus = (int)DepositHistoryConstant.CONTRACT_PAY_STATUS.HUY;
                        _DbContext.SaveChanges();
                        return 1;
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CancelById - ContractPayDAL: " + ex);
                return -1;
            }
        }

        public List<ContractPay> GetForAddContract()
        {
            try
            {

                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return _DbContext.ContractPays.AsNoTracking().ToList(); ;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetForAddContract - ContractPayDAL: " + ex);
                return new List<ContractPay>();
            }
        }

        public List<ContractPayDetailViewModel> GetByDataIds(List<int> dataIds)
        {
            try
            {
                var listContractPayDetail = new List<ContractPayDetailViewModel>();
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var contractPayDetails = _DbContext.ContractPayDetails.AsNoTracking().Where(n => dataIds.Contains((int)n.DataId)).OrderByDescending(n => n.CreatedDate).ToList();
                    var listId = contractPayDetails.Select(n => n.PayId).ToList();
                    var contractPays = _DbContext.ContractPays.AsNoTracking().Where(n => listId.Contains((int)n.PayId)).OrderByDescending(n => n.CreatedDate).ToList();
                    foreach (var item in contractPayDetails)
                    {
                        ContractPayDetailViewModel model = new ContractPayDetailViewModel();
                        model.ContractPayDetail = item;
                        model.ContractPay = contractPays.FirstOrDefault(n => n.PayId == item.PayId);
                        listContractPayDetail.Add(model);
                    }
                }
                return listContractPayDetail;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByDataIds - ContractPayDAL: " + ex);
                return new List<ContractPayDetailViewModel>();
            }
        }

        public List<ContractPayDetail> GetByContractPayIds(List<int> contractPayIds)
        {
            try
            {

                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var contractPays = _DbContext.ContractPayDetails.AsNoTracking().Where(x => contractPayIds.Contains(x.PayId)
                    && (x.ServiceId == 0 || x.ServiceId == null)).ToList();
                    if (contractPays != null)
                    {
                        return contractPays;
                    }
                }
                return new List<ContractPayDetail>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByContractPayIds - ContractPayDAL: " + ex);
                return new List<ContractPayDetail>();
            }
        }

        public List<ContractPay> GetByContractPayIdList(List<int> contractPayIds)
        {
            try
            {

                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var contractPays = _DbContext.ContractPays.AsNoTracking().Where(x => contractPayIds.Contains(x.PayId)).ToList();
                    if (contractPays != null)
                    {
                        return contractPays;
                    }
                }
                return new List<ContractPay>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByContractPayIdList - ContractPayDAL: " + ex);
                return new List<ContractPay>();
            }
        }

        public List<ContractPayDetail> GetByContractPayId(int contractPayId)
        {
            try
            {

                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var contractPays = _DbContext.ContractPayDetails.AsNoTracking().Where(x => x.PayId == contractPayId).ToList();
                    if (contractPays != null)
                    {
                        return contractPays;
                    }
                }
                return new List<ContractPayDetail>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByContractPayId - ContractPayDAL: " + ex);
                return new List<ContractPayDetail>();
            }
        }

        public List<ContractPayDetail> GetByContractDataIds(List<long> dataIds)
        {
            try
            {

                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.ContractPayDetails.AsNoTracking().Where(x => x.DataId != null
                    && dataIds.Contains(x.DataId.Value) && (x.ServiceId == 0 || x.ServiceId == null)).ToList();
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return new List<ContractPayDetail>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByContractDataIds - ContractPayDAL: " + ex);
                return new List<ContractPayDetail>();
            }
        }

        public DataTable GetPagingList(ContractPaySearchModel searchModel, int currentPage, int pageSize, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[13];
                objParam[0] = new SqlParameter("@BillNo", searchModel.BillNo);
                objParam[1] = new SqlParameter("@Description", searchModel.Content);
                objParam[2] = new SqlParameter("@ContractPayType", searchModel.Type);
                objParam[3] = new SqlParameter("@PayType", searchModel.PayType);
                objParam[4] = new SqlParameter("@ClientID", searchModel.ClientId);
                if (searchModel.CreateByIds == null || searchModel.CreateByIds.Count == 0)
                {
                    objParam[5] = new SqlParameter("@UserCreate", DBNull.Value);
                }
                else
                {
                    objParam[5] = new SqlParameter("@UserCreate", string.Join(",", searchModel.CreateByIds));
                }
                objParam[6] = new SqlParameter("@CreateDateFrom", searchModel.FromCreateDate);
                objParam[7] = new SqlParameter("@CreateDateTo", searchModel.ToCreateDate);
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
                if (searchModel.SupplierId == 0)
                    objParam[10] = new SqlParameter("@SupplerId", DBNull.Value);
                else
                    objParam[10] = new SqlParameter("@SupplerId", searchModel.SupplierId);
                if (searchModel.EmployeeId == 0)
                    objParam[11] = new SqlParameter("@SalerId", DBNull.Value);
                else
                    objParam[11] = new SqlParameter("@SalerId", searchModel.EmployeeId);
                if (string.IsNullOrEmpty(searchModel.ServiceCode))
                    objParam[12] = new SqlParameter("@ServiceCode", DBNull.Value);
                else
                    objParam[12] = new SqlParameter("@ServiceCode", searchModel.ServiceCode);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - ContractPayDAL: " + ex);
            }
            return null;
        }

        public DataTable GetByClientId(long clientId, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ClientId", clientId);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByClientId - ContractPayDAL: " + ex);
            }
            return null;
        }

        public DataTable GetByPayId(long payId, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@PayId", payId);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByPayId - ContractPayDAL: " + ex);
            }
            return null;
        }

        public DataTable GetByOrderId(long orderId, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@OrderId", orderId);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByOrderId - ContractPayDAL: " + ex);
            }
            return null;
        }

        public DataTable GetAllOrderDebt(ContractPaySearchModel searchModel, int currentPage, int pageSize, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[7];
                if (string.IsNullOrEmpty(searchModel.OrderNo))
                    objParam[0] = new SqlParameter("@OrderNo", DBNull.Value);
                else
                    objParam[0] = new SqlParameter("@OrderNo", searchModel.OrderNo);
                if (searchModel.StatusMulti.Count == 0)
                    objParam[1] = new SqlParameter("@status", DBNull.Value);
                else
                    objParam[1] = new SqlParameter("@status", string.Join(",", searchModel.StatusMulti));
                if (searchModel.ClientId == null || searchModel.ClientId == 0)
                    objParam[2] = new SqlParameter("@ClientId", DBNull.Value);
                else
                    objParam[2] = new SqlParameter("@ClientId", searchModel.ClientId.ToString());
                if (string.IsNullOrEmpty(searchModel.LabelName))
                    objParam[3] = new SqlParameter("@LabelName", DBNull.Value);
                else
                    objParam[3] = new SqlParameter("@LabelName", searchModel.LabelName);
                if (pageSize == -1)
                {
                    objParam[4] = new SqlParameter("@PageIndex", -1);
                    objParam[5] = new SqlParameter("@PageSize", -1);
                }
                else
                {
                    objParam[4] = new SqlParameter("@PageIndex", currentPage);
                    objParam[5] = new SqlParameter("@PageSize", pageSize);
                }
                if (searchModel.DebtStatus == -1)
                    objParam[6] = new SqlParameter("@DebtStatus", DBNull.Value);
                else
                    objParam[6] = new SqlParameter("@DebtStatus", searchModel.DebtStatus);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAllOrderDebt - ContractPayDAL: " + ex);
            }
            return null;
        }

        public DataTable GetLisContractPayDebt(ContractPaySearchModel searchModel, int currentPage, int pageSize, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[5];
                if (string.IsNullOrEmpty(searchModel.BillNo))
                    objParam[0] = new SqlParameter("@BillNo", DBNull.Value);
                else
                    objParam[0] = new SqlParameter("@BillNo", searchModel.BillNo);
                if (searchModel.ClientId == null || searchModel.ClientId == 0)
                    objParam[1] = new SqlParameter("@ClientID", DBNull.Value);
                else
                    objParam[1] = new SqlParameter("@ClientID", searchModel.ClientId.ToString());
                if (pageSize == -1)
                {
                    objParam[2] = new SqlParameter("@PageIndex", -1);
                    objParam[3] = new SqlParameter("@PageSize", -1);
                }
                else
                {
                    objParam[2] = new SqlParameter("@PageIndex", currentPage);
                    objParam[3] = new SqlParameter("@PageSize", pageSize);
                }
                if (searchModel.DebtStatus == -1)
                    objParam[4] = new SqlParameter("@DebtStatus", DBNull.Value);
                else
                    objParam[4] = new SqlParameter("@DebtStatus", searchModel.DebtStatus);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetLisContractPayDebt - ContractPayDAL: " + ex);
            }
            return null;
        }

        public int CreateContractPay(ContractPayViewModel model)
        {
            int id = 0;
            List<int> detailIds = new List<int>();
            try
            {
                SqlParameter[] objParam_contractPay = new SqlParameter[16];
                objParam_contractPay[0] = new SqlParameter("@BillNo", model.BillNo);
                if (model.ClientId == null || model.ClientId == 0)
                    objParam_contractPay[1] = new SqlParameter("@ClientId", DBNull.Value);
                else
                    objParam_contractPay[1] = new SqlParameter("@ClientId", model.ClientId);
                objParam_contractPay[2] = new SqlParameter("@Note", string.IsNullOrEmpty(model.Note) ? DBNull.Value.ToString() :
                    model.Note);
                if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_NCC_HOAN_TRA
                    || model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_HOA_HONG_NCC)
                {
                    objParam_contractPay[3] = new SqlParameter("@Amount", model.ContractPayDetails.Sum(n => n.Amount));
                }
                else
                {
                    objParam_contractPay[3] = new SqlParameter("@Amount", model.Amount);
                }
                objParam_contractPay[4] = new SqlParameter("@Type", model.Type);
                objParam_contractPay[5] = new SqlParameter("@PayType", model.PayType);
                if (model.PayType == (int)DepositHistoryConstant.CONTRACT_PAYMENT_TYPE.CHUYEN_KHOAN)
                {
                    objParam_contractPay[6] = new SqlParameter("@BankingAccountId", model.BankingAccountId);
                }
                else
                {
                    objParam_contractPay[6] = new SqlParameter("@BankingAccountId", DBNull.Value);
                }
                objParam_contractPay[7] = new SqlParameter("@Description", string.IsNullOrEmpty(model.Description)
                    ? DBNull.Value.ToString() : model.Description);
                objParam_contractPay[8] = new SqlParameter("@AttatchmentFile", string.IsNullOrEmpty(model.AttatchmentFile)
                    ? DBNull.Value.ToString() : model.AttatchmentFile);
                objParam_contractPay[9] = new SqlParameter("@ExportDate", DateTime.Now);
                objParam_contractPay[10] = new SqlParameter("@PayStatus", (int)DepositHistoryConstant.CONTRACT_PAY_STATUS.KE_TOAN_DUYET);
                objParam_contractPay[11] = new SqlParameter("@CreatedBy", model.CreatedBy);
                objParam_contractPay[12] = new SqlParameter("@CreatedDate", DateTime.Now);
                objParam_contractPay[13] = new SqlParameter("@SupplierId", Convert.ToInt32(model.SupplierId));
                objParam_contractPay[14] = new SqlParameter("@ObjectType", Convert.ToInt32(model.ObjectType));
                objParam_contractPay[15] = new SqlParameter("@EmployeeId", Convert.ToInt32(model.EmployeeId));
                id = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertContractPay, objParam_contractPay);
                if (id > 0)
                {
                    foreach (var item in model.ContractPayDetails)
                    {
                        var detailId = 0;
                        SqlParameter[] objParam_contractPayDetail = new SqlParameter[8];
                        objParam_contractPayDetail[0] = new SqlParameter("@PayId", id);
                        if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_KY_QUY)
                        {
                            objParam_contractPayDetail[1] = new SqlParameter("@DataId", item.Id);
                        }
                        else
                        {
                            objParam_contractPayDetail[1] = new SqlParameter("@DataId", item.OrderId);

                        }
                        objParam_contractPayDetail[2] = new SqlParameter("@CreatedBy", model.CreatedBy);
                        objParam_contractPayDetail[3] = new SqlParameter("@Amount", item.Amount);
                        objParam_contractPayDetail[4] = new SqlParameter("@CreatedDate", DateTime.Now);
                        objParam_contractPayDetail[5] = new SqlParameter("@ServiceId", Convert.ToInt64(item.ServiceId));
                        objParam_contractPayDetail[6] = new SqlParameter("@ServiceType", Convert.ToInt32(item.ServiceType));
                        if (string.IsNullOrEmpty(item.ServiceCode))
                            objParam_contractPayDetail[7] = new SqlParameter("@ServiceCode", DBNull.Value);
                        else
                            objParam_contractPayDetail[7] = new SqlParameter("@ServiceCode", item.ServiceCode);

                        detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertContractPayDetail, objParam_contractPayDetail);
                        if (detailId > 0)
                            detailIds.Add(detailId);
                        if (detailId <= 0)
                        {
                            using (var _DbContext = new EntityDataContext(_connection))
                            {
                                var entity = _DbContext.ContractPays.Find(id);
                                _DbContext.ContractPays.Remove(entity);
                                foreach (var idDetail in detailIds)
                                {
                                    var detail = _DbContext.ContractPayDetails.Find(idDetail);
                                    _DbContext.ContractPayDetails.Remove(detail);
                                }
                                _DbContext.SaveChanges();
                            }
                            return -1;
                        }
                        //nếu thanh toán đủ thì cập nhật trạng thái của đơn hàng - Đơn hàng đã thanh toán
                        var status = (int)OrderStatus.WAITING_FOR_OPERATOR;
                        var orderInfo = orderDAL.GetDetailOrderByOrderId(item.OrderId).Result;
                        if (orderInfo != null && orderInfo.OrderStatus != (int)OrderStatus.CREATED_ORDER
                             && orderInfo.OrderStatus != (int)OrderStatus.CONFIRMED_SALE)
                        {
                            status = orderInfo.OrderStatus.Value;
                        }
                        if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG && item.Amount >= item.TotalNeedPayment)
                        {
                            SqlParameter[] objParam_updateFinishPayment = new SqlParameter[5];
                            objParam_updateFinishPayment[0] = new SqlParameter("@OrderId", item.OrderId);
                            objParam_updateFinishPayment[1] = new SqlParameter("@IsFinishPayment", true);
                            objParam_updateFinishPayment[2] = new SqlParameter("@PaymentStatus", (int)PaymentStatus.PAID);
                            objParam_updateFinishPayment[3] = new SqlParameter("@Status", status);
                            objParam_updateFinishPayment[4] = new SqlParameter("@DebtStatus", (int)DepositHistoryConstant.ORDER_DEBT_STATUS.GACH_NO_DU);
                            _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateOrderFinishPayment, objParam_updateFinishPayment);
                        }
                        if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG && item.Amount < item.TotalNeedPayment)
                        {

                            SqlParameter[] objParam_updateFinishPayment = new SqlParameter[5];
                            objParam_updateFinishPayment[0] = new SqlParameter("@OrderId", item.OrderId);
                            objParam_updateFinishPayment[1] = new SqlParameter("@IsFinishPayment", false);
                            objParam_updateFinishPayment[2] = new SqlParameter("@PaymentStatus", (int)PaymentStatus.PAID_NOT_ENOUGH);
                            objParam_updateFinishPayment[3] = new SqlParameter("@Status", status);
                            objParam_updateFinishPayment[4] = new SqlParameter("@DebtStatus", (int)DepositHistoryConstant.ORDER_DEBT_STATUS.GACH_NO_CHUA_DU);
                            _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateOrderFinishPayment, objParam_updateFinishPayment);
                        }
                        if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG)
                        {
                            orderDAL.UpdateOrderStatus(item.OrderId, status, model.CreatedBy.Value, model.CreatedBy.Value).Wait();
                        }
                        //nếu thanh toán đủ thì cập nhật trạng thái của nạp quỹ - Chờ duyệt
                        if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_KY_QUY)
                        {
                            SqlParameter[] objParam_updateFinishPayment = new SqlParameter[3];
                            objParam_updateFinishPayment[0] = new SqlParameter("@DepositHistoryId", item.Id);
                            objParam_updateFinishPayment[1] = new SqlParameter("@IsFinishPayment", true);
                            objParam_updateFinishPayment[2] = new SqlParameter("@Status", (int)DepositHistoryConstant.DEPOSIT_STATUS.CHO_DUYET);
                            _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateDepositFinishPayment, objParam_updateFinishPayment);
                        }
                    }

                    SqlParameter[] objParam_UpdateContractPayDebtStatus = new SqlParameter[3];
                    objParam_UpdateContractPayDebtStatus[0] = new SqlParameter("@PayId", id);
                    objParam_UpdateContractPayDebtStatus[1] = new SqlParameter("@DebtStatus", model.ContractPayDetails.Sum(n => n.Amount) >= model.Amount ?
                        (int)DepositHistoryConstant.CONTRACTPAY_DEBT_STATUS.DA_GACH_HET :
                         (int)DepositHistoryConstant.CONTRACTPAY_DEBT_STATUS.CHUA_GACH_HET);
                    objParam_UpdateContractPayDebtStatus[2] = new SqlParameter("@UpdatedBy", model.CreatedBy);
                    _DbWorker.ExecuteNonQuery(StoreProcedureConstant.Sp_UpdateDebtStatusByPayId, objParam_UpdateContractPayDebtStatus);
                }
                return id;
            }
            catch (Exception ex)
            {
                DeleteContractPayFail(id, detailIds);
                LogHelper.InsertLogTelegram("CreateContactPay - ContractPayDAL. " + ex);
                return -1;
            }
        }

        private void DeleteContractPayFail(int id, List<int> detailIds)
        {
            using (var _DbContext = new EntityDataContext(_connection))
            {
                var entity = _DbContext.ContractPays.Find(id);
                _DbContext.ContractPays.Remove(entity);
                foreach (var idDetail in detailIds)
                {
                    var detail = _DbContext.ContractPayDetails.Find(idDetail);
                    _DbContext.ContractPayDetails.Remove(detail);
                }
                _DbContext.SaveChanges();
            }
        }

        private void DeleteContractPayDetail(List<int> detailIds)
        {
            using (var _DbContext = new EntityDataContext(_connection))
            {
                foreach (var idDetail in detailIds)
                {
                    var detail = _DbContext.ContractPayDetails.Find(idDetail);
                    _DbContext.ContractPayDetails.Remove(detail);
                }
                _DbContext.SaveChanges();
            }
        }

        public int UpdateContractPay(ContractPayViewModel model)
        {
            try
            {
                int id = 0;
                SqlParameter[] objParam_contractPay = new SqlParameter[16];
                objParam_contractPay[0] = new SqlParameter("@BillNo", model.BillNo);
                if (model.ClientId == null || model.ClientId == 0)
                    objParam_contractPay[1] = new SqlParameter("@ClientId", Convert.ToInt32(0));
                else
                    objParam_contractPay[1] = new SqlParameter("@ClientId", model.ClientId);
                objParam_contractPay[2] = new SqlParameter("@Note", model.Note);
                objParam_contractPay[3] = new SqlParameter("@Amount", model.Amount);
                objParam_contractPay[4] = new SqlParameter("@Type", model.Type);
                objParam_contractPay[5] = new SqlParameter("@PayType", model.PayType);
                if (model.PayType == (int)DepositHistoryConstant.CONTRACT_PAYMENT_TYPE.CHUYEN_KHOAN)
                {
                    objParam_contractPay[6] = new SqlParameter("@BankingAccountId", model.BankingAccountId);
                }
                else
                {
                    objParam_contractPay[6] = new SqlParameter("@BankingAccountId", DBNull.Value);
                }
                objParam_contractPay[7] = new SqlParameter("@Description", string.IsNullOrEmpty(model.Description)
                    ? DBNull.Value.ToString() : model.Description);
                objParam_contractPay[8] = new SqlParameter("@AttatchmentFile", !string.IsNullOrEmpty(model.AttatchmentFile) ?
                    model.AttatchmentFile : DBNull.Value.ToString());
                objParam_contractPay[9] = new SqlParameter("@ExportDate", DateTime.Now);
                objParam_contractPay[10] = new SqlParameter("@PayStatus", model.PayStatus);
                objParam_contractPay[11] = new SqlParameter("@UpdatedBy", model.UpdatedBy);
                objParam_contractPay[12] = new SqlParameter("@PayId", model.PayId);
                objParam_contractPay[13] = new SqlParameter("@SupplierId", Convert.ToInt32(model.SupplierId));
                objParam_contractPay[14] = new SqlParameter("@ObjectType", Convert.ToInt32(model.ObjectType));
                objParam_contractPay[15] = new SqlParameter("@EmployeeId", Convert.ToInt32(model.EmployeeId));
                id = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateContractPay, objParam_contractPay);
                foreach (var item in model.ContractPayDetails)
                {
                    var detailId = 0;
                    if (item.PayDetailId == 0)
                    {
                        SqlParameter[] objParam_contractPayDetail = new SqlParameter[8];
                        objParam_contractPayDetail[0] = new SqlParameter("@PayId", id);
                        if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_KY_QUY)
                        {
                            objParam_contractPayDetail[1] = new SqlParameter("@DataId", item.Id);
                        }
                        else
                        {
                            objParam_contractPayDetail[1] = new SqlParameter("@DataId", item.OrderId);
                        }
                        objParam_contractPayDetail[2] = new SqlParameter("@CreatedBy", model.UpdatedBy);
                        objParam_contractPayDetail[3] = new SqlParameter("@Amount", item.Amount);
                        objParam_contractPayDetail[4] = new SqlParameter("@CreatedDate", DateTime.Now);
                        objParam_contractPayDetail[5] = new SqlParameter("@ServiceId", Convert.ToInt64(item.ServiceId));
                        objParam_contractPayDetail[6] = new SqlParameter("@ServiceType", Convert.ToInt32(item.ServiceType));
                        if (string.IsNullOrEmpty(item.ServiceCode))
                            objParam_contractPayDetail[7] = new SqlParameter("@ServiceCode", DBNull.Value);
                        else
                            objParam_contractPayDetail[7] = new SqlParameter("@ServiceCode", item.ServiceCode);
                        detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertContractPayDetail, objParam_contractPayDetail);
                        item.PayDetailId = detailId;
                        if (detailId <= 0)
                        {
                            return -1;
                        }
                        if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_KY_QUY)
                        {
                            var listContractPayDetail = GetByContractPayId(id);
                            var contractPayDetails = listContractPayDetail.Where(n => n.DataId != item.Id).ToList();
                            foreach (var contractPayDetail in contractPayDetails)
                            {
                                DeleteContractPayDetail(contractPayDetail);
                                SqlParameter[] objParam_Detail = new SqlParameter[3];
                                objParam_Detail[0] = new SqlParameter("@DepositHistoryId", contractPayDetail.DataId);
                                objParam_Detail[1] = new SqlParameter("@IsFinishPayment", false);
                                objParam_Detail[2] = new SqlParameter("@Status", (int)DepositHistoryConstant.DEPOSIT_STATUS.CHO_DUYET);
                                detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateDepositFinishPayment, objParam_Detail);
                            }
                        }
                    }
                    else
                    {
                        if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG
                            || model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_NCC_HOAN_TRA
                            || model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_HOA_HONG_NCC)
                        {
                            SqlParameter[] objParam_contractPayDetail = new SqlParameter[8];
                            objParam_contractPayDetail[0] = new SqlParameter("@Id", item.PayDetailId);
                            objParam_contractPayDetail[1] = new SqlParameter("@PayId", id);
                            if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG)
                            {
                                objParam_contractPayDetail[2] = new SqlParameter("@DataId", item.OrderId);
                            }
                            if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_KY_QUY)
                            {
                                objParam_contractPayDetail[2] = new SqlParameter("@DataId", item.Id);
                            }
                            if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG)
                            {
                                objParam_contractPayDetail[3] = new SqlParameter("@Amount", item.Amount);
                            }
                            if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_KY_QUY)
                            {
                                objParam_contractPayDetail[3] = new SqlParameter("@Amount", model.Amount);
                            }
                            objParam_contractPayDetail[4] = new SqlParameter("@UpdatedBy", model.UpdatedBy);
                            objParam_contractPayDetail[5] = new SqlParameter("@ServiceId", Convert.ToInt64(item.ServiceId));
                            objParam_contractPayDetail[6] = new SqlParameter("@ServiceType", Convert.ToInt32(item.ServiceType));
                            if (string.IsNullOrEmpty(item.ServiceCode))
                                objParam_contractPayDetail[7] = new SqlParameter("@ServiceCode", DBNull.Value);
                            else
                                objParam_contractPayDetail[7] = new SqlParameter("@ServiceCode", item.ServiceCode);
                            detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateContractPayDetail, objParam_contractPayDetail);
                            if (detailId <= 0)
                            {
                                return -1;
                            }
                        }
                    }

                    //nếu thanh toán đủ thì cập nhật trạng thái của đơn hàng - Đơn hàng đã thanh toán
                    var status = (int)OrderStatus.WAITING_FOR_OPERATOR;
                    var orderInfo = orderDAL.GetDetailOrderByOrderId(item.OrderId).Result;
                    if (orderInfo != null && orderInfo.OrderStatus != (int)OrderStatus.CREATED_ORDER
                         && orderInfo.OrderStatus != (int)OrderStatus.CONFIRMED_SALE)
                    {
                        status = orderInfo.OrderStatus.Value;
                    }
                    if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG && item.Amount >= item.TotalNeedPayment)
                    {
                        SqlParameter[] objParam_contractPayDetail = new SqlParameter[5];
                        objParam_contractPayDetail[0] = new SqlParameter("@OrderId", item.OrderId);
                        objParam_contractPayDetail[1] = new SqlParameter("@IsFinishPayment", true);
                        objParam_contractPayDetail[2] = new SqlParameter("@Status", status);
                        objParam_contractPayDetail[3] = new SqlParameter("@DebtStatus", DBNull.Value);
                        objParam_contractPayDetail[4] = new SqlParameter("@PaymentStatus", (int)PaymentStatus.PAID);

                        detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateOrderFinishPayment, objParam_contractPayDetail);
                    }
                    else
                    {

                        SqlParameter[] objParam_contractPayDetail = new SqlParameter[5];
                        objParam_contractPayDetail[0] = new SqlParameter("@OrderId", item.OrderId);
                        objParam_contractPayDetail[1] = new SqlParameter("@IsFinishPayment", false);
                        objParam_contractPayDetail[2] = new SqlParameter("@Status", status);
                        objParam_contractPayDetail[3] = new SqlParameter("@DebtStatus", DBNull.Value);
                        objParam_contractPayDetail[4] = new SqlParameter("@PaymentStatus", (int)PaymentStatus.PAID_NOT_ENOUGH);
                        detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateOrderFinishPayment, objParam_contractPayDetail);
                    }
                    if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG)
                    {
                        orderDAL.UpdateOrderStatus(item.OrderId, status, model.UpdatedBy.Value, model.UpdatedBy.Value).Wait();
                    }
                    //nếu thanh toán đủ thì cập nhật trạng thái của nạp quỹ - Chờ duyệt
                    if (model.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_KY_QUY)
                    {
                        SqlParameter[] objParam_contractPayDetail = new SqlParameter[3];
                        objParam_contractPayDetail[0] = new SqlParameter("@DepositHistoryId", item.Id);
                        objParam_contractPayDetail[1] = new SqlParameter("@IsFinishPayment", true);
                        objParam_contractPayDetail[2] = new SqlParameter("@Status", (int)DepositHistoryConstant.DEPOSIT_STATUS.CHO_DUYET);
                        detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateDepositFinishPayment, objParam_contractPayDetail);
                        if (detailId <= 0)
                        {
                            return -1;
                        }
                    }
                }

                SqlParameter[] objParam_UpdateContractPayDebtStatus = new SqlParameter[3];
                objParam_UpdateContractPayDebtStatus[0] = new SqlParameter("@PayId", model.PayId);
                objParam_UpdateContractPayDebtStatus[1] = new SqlParameter("@DebtStatus", model.ContractPayDetails.Sum(n => n.Amount) >= model.Amount ?
                    (int)DepositHistoryConstant.CONTRACTPAY_DEBT_STATUS.DA_GACH_HET :
                     (int)DepositHistoryConstant.CONTRACTPAY_DEBT_STATUS.CHUA_GACH_HET);
                objParam_UpdateContractPayDebtStatus[2] = new SqlParameter("@UpdatedBy", model.UpdatedBy);
                _DbWorker.ExecuteNonQuery(StoreProcedureConstant.Sp_UpdateDebtStatusByPayId, objParam_UpdateContractPayDebtStatus);
                var listDetai = GetByContractPayIds(new List<int>() { model.PayId });
                foreach (var item in listDetai)
                {
                    var exists = model.ContractPayDetails.FirstOrDefault(n => n.PayDetailId == item.Id);
                    if (exists == null)
                    {
                        DeleteContractPayDetail(item);
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateContactPay - ContractPayDAL. " + ex);
                return -1;
            }
        }

        public long CountContractPayInYear()
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return _DbContext.ContractPays.AsNoTracking().Where(x => ((DateTime)x.CreatedDate).Year == DateTime.Now.Year).Count();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CountContractPayInYear - ContractPayDAL: " + ex.ToString());
                return -1;
            }
        }

        public async Task<string> getContractPayByBillNo(string bill_no)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    var data = _DbContext.ContractPays.AsNoTracking().FirstOrDefault(s => s.BillNo == bill_no);
                    return data == null ? "" : data.BillNo;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getContractPayByBillNo - ContractPayDAL: " + ex);
                return "";
            }
        }

        public async Task<DataTable> GetContractPayByOrderId(long OrderId)
        {
            try
            {

                SqlParameter[] objParam_contractPay = new SqlParameter[1];
                objParam_contractPay[0] = new SqlParameter("@OrderId", OrderId);

                return _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetContractPayByOrderId, objParam_contractPay);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContractPayByOrderId - ContractPayDAL. " + ex);
                return null;
            }
        }

        public int DeleteContractPayDetail(ContractPayDetail model)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    _DbContext.ContractPayDetails.Remove(model);
                    _DbContext.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DeleteContractPayDetail - ContractPayDAL: " + ex);
                return -1;
            }
        }

        public async Task<List<ContractPay>> GetContractPayByClientId(long clientId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var data = _DbContext.ContractPays.AsNoTracking().Where(s => s.ClientId == clientId && s.IsDelete == false).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContractPayByClientId - ContractPayDAL: " + ex);
                return null;
            }
        }

        public ContractPayDetail GetContractPayDetail(long dataId, long payId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var data = _DbContext.ContractPayDetails.AsNoTracking().Where(s => s.PayId == payId &&
                    s.DataId == dataId).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContractPayDetail - ContractPayDAL: " + ex);
                return null;
            }
        }

        public int AddContractPayDetail(ContractPayViewModel model, bool isOrder = false)
        {
            List<int> detailIds = new List<int>();
            try
            {
                model.Type = (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG;
                foreach (var item in model.ContractPayDetails)
                {
                    var contractPayDetail = GetContractPayDetail(item.OrderId, item.PayId);
                    var detailId = 0;
                    if (contractPayDetail != null)
                    {
                        SqlParameter[] objParam_contractPayDetail = new SqlParameter[8];
                        objParam_contractPayDetail[0] = new SqlParameter("@Id", contractPayDetail.Id);
                        objParam_contractPayDetail[1] = new SqlParameter("@PayId", item.PayId);
                        objParam_contractPayDetail[2] = new SqlParameter("@DataId", item.OrderId);
                        objParam_contractPayDetail[3] = new SqlParameter("@Amount", item.Amount + (double)contractPayDetail.Amount);
                        objParam_contractPayDetail[4] = new SqlParameter("@UpdatedBy", model.CreatedBy);
                        objParam_contractPayDetail[5] = new SqlParameter("@ServiceId", DBNull.Value);
                        objParam_contractPayDetail[6] = new SqlParameter("@ServiceType", DBNull.Value);
                        objParam_contractPayDetail[7] = new SqlParameter("@ServiceCode", DBNull.Value);
                        detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateContractPayDetail, objParam_contractPayDetail);
                        if (detailId <= 0)
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        SqlParameter[] objParam_contractPayDetail = new SqlParameter[8];
                        objParam_contractPayDetail[0] = new SqlParameter("@PayId", item.PayId);
                        objParam_contractPayDetail[1] = new SqlParameter("@DataId", item.OrderId);
                        objParam_contractPayDetail[2] = new SqlParameter("@CreatedBy", model.CreatedBy);
                        objParam_contractPayDetail[3] = new SqlParameter("@Amount", item.Amount);
                        objParam_contractPayDetail[4] = new SqlParameter("@CreatedDate", DateTime.Now);
                        objParam_contractPayDetail[5] = new SqlParameter("@ServiceId", DBNull.Value);
                        objParam_contractPayDetail[6] = new SqlParameter("@ServiceType", DBNull.Value);
                        objParam_contractPayDetail[7] = new SqlParameter("@ServiceCode", DBNull.Value);
                        detailId = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertContractPayDetail, objParam_contractPayDetail);
                        if (detailId > 0)
                            detailIds.Add(detailId);
                        if (detailId <= 0)
                        {
                            DeleteContractPayDetail(detailIds);
                            return -1;
                        }
                    }

                    var orderInfo = orderDAL.GetDetailOrderByOrderId(item.OrderId).Result;
                    var isFinishPayment = false;
                    var debtStatus = (int)DepositHistoryConstant.ORDER_DEBT_STATUS.GACH_NO_CHUA_DU;
                    var paymentStatus = (int)PaymentStatus.PAID_NOT_ENOUGH;
                    if (item.AmountOrder <= item.Amount)
                    {
                        paymentStatus = (int)PaymentStatus.PAID;
                        isFinishPayment = true;
                    }
                    var debtStatusContractPay = (int)DepositHistoryConstant.CONTRACTPAY_DEBT_STATUS.CHUA_GACH_HET;
                    if (isOrder)
                    {
                        if (item.Amount == item.TotalNeedPayment)
                            debtStatusContractPay = (int)DepositHistoryConstant.CONTRACTPAY_DEBT_STATUS.DA_GACH_HET;
                    }
                    else
                        debtStatusContractPay = model.DebtStatus == null ?
                            (int)DepositHistoryConstant.CONTRACTPAY_DEBT_STATUS.CHUA_GACH_HET : model.DebtStatus.Value;
                    if (item.AmountOrder <= model.ContractPayDetails.Sum(n => n.Amount))
                        debtStatus = (int)DepositHistoryConstant.ORDER_DEBT_STATUS.GACH_NO_DU;
                    SqlParameter[] objParam_updateFinishPayment = new SqlParameter[5];
                    objParam_updateFinishPayment[0] = new SqlParameter("@OrderId", item.OrderId);
                    objParam_updateFinishPayment[1] = new SqlParameter("@IsFinishPayment", isFinishPayment);
                    objParam_updateFinishPayment[2] = new SqlParameter("@PaymentStatus", paymentStatus);
                    objParam_updateFinishPayment[3] = new SqlParameter("@DebtStatus", Convert.ToInt32(debtStatus));
                    objParam_updateFinishPayment[4] = new SqlParameter("@Status", orderInfo.OrderStatus == (int)OrderStatus.CREATED_ORDER ?
                        (int)OrderStatus.WAITING_FOR_OPERATOR : Convert.ToInt32(orderInfo.OrderStatus));
                    _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateOrderFinishPayment, objParam_updateFinishPayment);

                    SqlParameter[] objParam_UpdateContractPayDebtStatus = new SqlParameter[3];
                    objParam_UpdateContractPayDebtStatus[0] = new SqlParameter("@PayId", item.PayId);
                    objParam_UpdateContractPayDebtStatus[1] = new SqlParameter("@DebtStatus", debtStatusContractPay);
                    objParam_UpdateContractPayDebtStatus[2] = new SqlParameter("@UpdatedBy", model.CreatedBy);
                    _DbWorker.ExecuteNonQuery(StoreProcedureConstant.Sp_UpdateDebtStatusByPayId, objParam_UpdateContractPayDebtStatus);
                }
                return 1;
            }
            catch (Exception ex)
            {
                DeleteContractPayDetail(detailIds);
                LogHelper.InsertLogTelegram("CreateContactPay - ContractPayDAL. " + ex);
                return -1;
            }
        }

        public int DeleteContractPayDetailByIds(List<ContractPayViewModel> model)
        {
            int id = 0;
            try
            {
                foreach (var item in model)
                {
                    var contractPayDetail = GetContractPayDetail(item.OrderId, item.PayId);
                    SqlParameter[] objParam_contractPay = new SqlParameter[1];
                    objParam_contractPay[0] = new SqlParameter("@PayDetailID", Convert.ToInt32(contractPayDetail?.Id));
                    id = _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_DeleteContractPayDetailByPayDetailId, objParam_contractPay);
                }
                return id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DeleteContractPayDetailByIds - ContractPayDAL. " + ex);
                return -1;
            }
        }

        public List<ContractPay> GetByOrderId(long orderId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var contractPayDetails = _DbContext.ContractPayDetails.AsNoTracking().Where(x => orderId == x.DataId.Value).ToList();
                    var listPayIds = contractPayDetails.Select(n => n.PayId).ToList();
                    var contractPays = _DbContext.ContractPays.AsNoTracking().Where(x => listPayIds.Contains(x.PayId)).ToList();
                    return contractPays;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByContractPayIdList - ContractPayDAL: " + ex);
                return new List<ContractPay>();
            }
        }

        public int UpdateOrderFinishPayment(ContractPayViewModel model, bool isPayment = true)
        {
            try
            {
                SqlParameter[] objParam_contractPayDetail = new SqlParameter[5];
                objParam_contractPayDetail[0] = new SqlParameter("@OrderId", model.OrderId);
                objParam_contractPayDetail[1] = new SqlParameter("@IsFinishPayment", false);
              
                objParam_contractPayDetail[2] = new SqlParameter("@Status", model.OrderStatus);
                if (!isPayment)
                    objParam_contractPayDetail[3] = new SqlParameter("@DebtStatus", Convert.ToInt32(0));
                else
                    objParam_contractPayDetail[3] = new SqlParameter("@DebtStatus", (int)DepositHistoryConstant.ORDER_DEBT_STATUS.GACH_NO_CHUA_DU);
                objParam_contractPayDetail[4] = new SqlParameter("@PaymentStatus", isPayment ? (int)PaymentStatus.PAID_NOT_ENOUGH :
                    Convert.ToInt32((int)PaymentStatus.UNPAID));
                _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateOrderFinishPayment, objParam_contractPayDetail);
                //if (!isPayment && model.PermisionType != PermisionType.DUOC_CN)
                //{
                //    SqlParameter[] objParam_updateServiceStatus = new SqlParameter[2];
                //    objParam_updateServiceStatus[0] = new SqlParameter("@OrderId", model.OrderId);
                //    objParam_updateServiceStatus[1] = new SqlParameter("@Status", Convert.ToInt32((int)ServiceStatus.New));
                //    _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateAllServiceStatusByOrderId, objParam_updateServiceStatus);
                //}

                SqlParameter[] objParam_UpdateContractPayDebtStatus = new SqlParameter[3];
                objParam_UpdateContractPayDebtStatus[0] = new SqlParameter("@PayId", Convert.ToInt32(model.PayId));
                objParam_UpdateContractPayDebtStatus[1] = new SqlParameter("@DebtStatus", (int)DepositHistoryConstant.CONTRACTPAY_DEBT_STATUS.CHUA_GACH_HET);
                objParam_UpdateContractPayDebtStatus[2] = new SqlParameter("@UpdatedBy", model.UpdatedBy);
                _DbWorker.ExecuteNonQuery(StoreProcedureConstant.Sp_UpdateDebtStatusByPayId, objParam_UpdateContractPayDebtStatus);
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateOrderFinishPayment - ContractPayDAL. " + ex);
                return -1;
            }
        }

        public DataTable GetDetailContractPayById(long payId, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@PayId", payId);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetailContractPayById - ContractPayDAL: " + ex);
            }
            return null;
        }

        public int UndoContractPayByCancelService(int contractPayId, long orderId, int userId)
        {
            try
            {
                var detail = GetContractPayDetail(orderId, contractPayId);
                SqlParameter[] objParam_contractPayDetail = new SqlParameter[5];
                objParam_contractPayDetail[0] = new SqlParameter("@OrderId", orderId);
                objParam_contractPayDetail[1] = new SqlParameter("@IsFinishPayment", false);
                objParam_contractPayDetail[2] = new SqlParameter("@Status", (int)OrderStatus.CANCEL);
                objParam_contractPayDetail[3] = new SqlParameter("@DebtStatus", (int)DepositHistoryConstant.ORDER_DEBT_STATUS.GACH_NO_CHUA_DU);
                objParam_contractPayDetail[4] = new SqlParameter("@PaymentStatus", Convert.ToInt32((int)PaymentStatus.UNPAID));
                _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateOrderFinishPayment, objParam_contractPayDetail);

                var result = DeleteContractPayDetailByIds(new List<ContractPayViewModel>()
                {
                    new ContractPayViewModel(){OrderId = orderId, PayId = contractPayId}
                });
                if (result < 0)
                    return result;
                SqlParameter[] objParam_UpdateContractPayDebtStatus = new SqlParameter[3];
                objParam_UpdateContractPayDebtStatus[0] = new SqlParameter("@PayId", contractPayId);
                objParam_UpdateContractPayDebtStatus[1] = new SqlParameter("@DebtStatus", (int)DepositHistoryConstant.CONTRACTPAY_DEBT_STATUS.CHUA_GACH_HET);
                objParam_UpdateContractPayDebtStatus[2] = new SqlParameter("@UpdatedBy", userId);
                _DbWorker.ExecuteNonQuery(StoreProcedureConstant.Sp_UpdateDebtStatusByPayId, objParam_UpdateContractPayDebtStatus);
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateContractPayDetail - ContractPayDAL. " + ex);
                return -1;
            }
        }

        public DataTable GetContractPayServiceListBySupplierId(long supplierId, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@SupplierId", supplierId);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContractPayServiceListBySupplierId - ContractPayDAL: " + ex);
            }
            return null;
        }

        public DataTable GetServiceDetail(string serviceCode, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ServiceCode", serviceCode);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContractPayServiceListBySupplierId - ContractPayDAL: " + ex);
            }
            return null;
        }

        public List<PaymentRequestDetailViewModel> GetByDataIdsService(List<long> dataIds)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var paymentRequest = (from request in _DbContext.ContractPays.ToList()
                                          join detail in _DbContext.ContractPayDetails.ToList() on request.PayId equals detail.PayId
                                          where (request.IsDelete == null || request.IsDelete.Value == false)
                                          && (detail.ServiceId != null && dataIds.Contains(detail.ServiceId.Value))
                                          select new PaymentRequestDetailViewModel
                                          {
                                              OrderId = (int)detail.DataId,
                                              Amount = detail.Amount.Value,
                                              ServiceType = detail.ServiceType != null ? detail.ServiceType.Value : 0,
                                              ServiceId = detail.ServiceId != null ? detail.ServiceId.Value : 0,
                                              ContractPayId = detail.PayId,
                                              SupplierId = request.SupplierId != null ? request.SupplierId.Value : 0,
                                              Id = (int)detail.Id,
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

        public DataTable GetContractPayBySupplierId(long orderId, long serviceId, int serviceType, string proc)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@OrderId", orderId);
                objParam[1] = new SqlParameter("@ServiceId", serviceId);
                objParam[2] = new SqlParameter("@ServiceType", serviceType);
                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContractPayBySupplierId - ContractPayDAL: " + ex);
            }
            return null;
        }
        public double GetTotalAmountContractPayByServiceId(string ServiceId, long ServiceType, long ContractPayType)
        {

            try
            {


                SqlParameter[] objParam_contractPay = new SqlParameter[3];
                objParam_contractPay[0] = new SqlParameter("@ServiceId", ServiceId);
                objParam_contractPay[1] = new SqlParameter("@ServiceType", ServiceType);
                objParam_contractPay[2] = new SqlParameter("@ContractPayType", ContractPayType);

                DataTable dt = _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetTotalAmountContractPayByServiceId, objParam_contractPay);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var amount = Convert.ToDouble(dt.Rows[0]["Amount"]);
                    return amount;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DeleteContractPayDetailByIds - ContractPayDAL. " + ex);
                return 0;
            }
        }
    }
}
