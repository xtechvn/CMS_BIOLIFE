using Azure.Core;
using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class OrderDAL : GenericService<Order>
    {
        private static DbWorker _DbWorker;
        public OrderDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }
        public async Task<DataTable> GetPagingList(OrderViewSearchModel searchModel, string proc)
        {
            try
            {

                SqlParameter[] objParam = new SqlParameter[24];


                objParam[0] = (CheckDate(searchModel.CreateTime) == DateTime.MinValue) ? new SqlParameter("@CreateTime", DBNull.Value) : new SqlParameter("@CreateTime", CheckDate(searchModel.CreateTime));
                objParam[1] = (CheckDate(searchModel.ToDateTime) == DateTime.MinValue) ? new SqlParameter("@ToDateTime", DBNull.Value) : new SqlParameter("@ToDateTime", CheckDate(searchModel.ToDateTime).AddDays(1));
                objParam[2] = new SqlParameter("@SysTemType", searchModel.SysTemType);
                objParam[3] = new SqlParameter("@OrderNo", searchModel.OrderNo);
                objParam[4] = new SqlParameter("@Note", searchModel.Note);
                objParam[5] = new SqlParameter("@ServiceType", searchModel.ServiceType == null ? "" : string.Join(",", searchModel.ServiceType));
                objParam[6] = new SqlParameter("@UtmSource", searchModel.UtmSource == null ? "" : searchModel.UtmSource);
                objParam[7] = new SqlParameter("@status", searchModel.Status == null ? "" : string.Join(",", searchModel.Status));
                objParam[8] = new SqlParameter("@CreateName", searchModel.CreateName);
                if (searchModel.Sale == null)
                {
                    objParam[9] = new SqlParameter("@Sale", DBNull.Value);

                }
                else
                {
                    objParam[9] = new SqlParameter("@Sale", searchModel.Sale);

                }
                objParam[10] = new SqlParameter("@SaleGroup", searchModel.SaleGroup);
                objParam[11] = new SqlParameter("@PageIndex", searchModel.PageIndex);
                objParam[12] = new SqlParameter("@PageSize", searchModel.pageSize);
                objParam[13] = new SqlParameter("@StatusTab", searchModel.StatusTab);
                objParam[14] = new SqlParameter("@ClientId", searchModel.ClientId);
                objParam[15] = new SqlParameter("@SalerPermission", searchModel.SalerPermission);
                objParam[16] = new SqlParameter("@OperatorId", searchModel.OperatorId);
                if (searchModel.StartDateFrom == null)
                {
                    objParam[17] = new SqlParameter("@StartDateFrom", DBNull.Value);

                }
                else
                {
                    objParam[17] = new SqlParameter("@StartDateFrom", searchModel.StartDateFrom);

                }
                if (searchModel.StartDateTo == null)
                {
                    objParam[18] = new SqlParameter("@StartDateTo", DBNull.Value);

                }
                else
                {
                    objParam[18] = new SqlParameter("@StartDateTo", searchModel.StartDateTo);

                }
                if (searchModel.EndDateFrom == null)
                {
                    objParam[19] = new SqlParameter("@EndDateFrom", DBNull.Value);

                }
                else
                {
                    objParam[19] = new SqlParameter("@EndDateFrom", searchModel.EndDateFrom);

                }
                if (searchModel.EndDateTo == null)
                {
                    objParam[20] = new SqlParameter("@EndDateTo", DBNull.Value);

                }
                else
                {
                    objParam[20] = new SqlParameter("@EndDateTo", searchModel.EndDateTo);

                }
                if (searchModel.PermisionType == null)
                {
                    objParam[21] = new SqlParameter("@PermisionType", DBNull.Value);

                }
                else
                {
                    objParam[21] = new SqlParameter("@PermisionType", searchModel.PermisionType);

                }
                if (searchModel.PaymentStatus == null)
                {
                    objParam[22] = new SqlParameter("@PaymentStatus", DBNull.Value);

                }
                else
                {
                    objParam[22] = new SqlParameter("@PaymentStatus", searchModel.PaymentStatus);

                }

                objParam[23] = new SqlParameter("@OrderId", searchModel.BoongKingCode);


                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - OrderDal: " + ex);
            }
            return null;
        }
        private DateTime CheckDate(string dateTime)
        {
            DateTime _date = DateTime.MinValue;
            if (!string.IsNullOrEmpty(dateTime))
            {
                _date = DateTime.ParseExact(dateTime, "d/M/yyyy", CultureInfo.InvariantCulture);
            }

            return _date != DateTime.MinValue ? _date : DateTime.MinValue;
        }
        public async Task<OrderDetailViewModel> GetDetailOrderByOrderId(long OrderId)
        {
            try
            {

                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@OrderId", OrderId);

                DataTable dt = _DbWorker.GetDataTable(ProcedureConstants.SP_GetDetailOrderByOrderId, objParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<OrderDetailViewModel>();
                    return data[0];
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetailOrderByOrderId - OrderDal: " + ex);
            }
            return null;
        }
        public DataTable GetListOrderByClientId(long clienId, string proc, int status = 0)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@ClientId", clienId);
                objParam[1] = new SqlParameter("@IsFinishPayment", DBNull.Value);
                if (status == 0)
                    objParam[2] = new SqlParameter("@OrderStatus", DBNull.Value);
                else
                    objParam[2] = new SqlParameter("@OrderStatus", status);

                return _DbWorker.GetDataTable(proc, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListOrderByClientId - OrderDal: " + ex);
            }
            return null;
        }
        public List<Order> GetByOrderIds(List<long> orderIds)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    return _DbContext.Orders.AsNoTracking().Where(s => orderIds.Contains(s.OrderId)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByOrderIds - OrderDal: " + ex);
                return new List<Order>();
            }
        }
        public async Task<int> UpdateOrderStatus(long OrderId, long Status, long UpdatedBy, long UserVerify)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@OrderId", OrderId);
                objParam[1] = new SqlParameter("@Status", Status);
                objParam[2] = new SqlParameter("@UpdatedBy", UpdatedBy);
                objParam[3] = UserVerify == 0 ? new SqlParameter("@UserVerify", DBNull.Value) : new SqlParameter("@UserVerify", UserVerify);

                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateOrderStatus, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetailOrderServiceByOrderId - OrderDal: " + ex);
            }
            return 0;
        }
        public async Task<long> UpdateOrder(Order model)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[18];
                objParam[0] = new SqlParameter("@OrderId", model.OrderId);
                objParam[1] = new SqlParameter("@ClientId", model.ClientId == 0 ? DBNull.Value : model.ClientId);
                objParam[2] = new SqlParameter("@OrderNo", model.OrderNo == null ? DBNull.Value : model.OrderNo);
                objParam[3] = new SqlParameter("@Price", model.Price == null? DBNull.Value : model.Price);
                objParam[4] = new SqlParameter("@Profit", model.Profit == null ? DBNull.Value : model.Profit);
                objParam[5] = new SqlParameter("@Discount", model.Discount == null ? DBNull.Value : model.Discount);
                objParam[6] = new SqlParameter("@Amount", model.Amount == null ? DBNull.Value : model.Amount);
                objParam[7] = new SqlParameter("@Status", model.OrderStatus == 0 ? DBNull.Value : model.OrderStatus);
                objParam[8] = new SqlParameter("@PaymentType", model.PaymentType == 0 ? DBNull.Value : model.PaymentType);
                objParam[9] = new SqlParameter("@PaymentStatus", model.PaymentStatus == 0 ? DBNull.Value : model.PaymentStatus);
                objParam[10] = new SqlParameter("@UtmSource", model.UtmSource == null ? DBNull.Value : model.UtmSource);
                objParam[11] = new SqlParameter("@UtmMedium", model.UtmMedium == null ? DBNull.Value : model.UtmMedium);
                objParam[12] = new SqlParameter("@Note", model.Note == null ? DBNull.Value : model.Note);
                objParam[13] = new SqlParameter("@VoucherId", model.VoucherId == null ? DBNull.Value : model.VoucherId);
                objParam[14] = new SqlParameter("@IsDelete", model.IsDelete == null ? DBNull.Value : model.IsDelete);
                objParam[15] = new SqlParameter("@UserId", model.UserId == 0 ? DBNull.Value : model.UserId);
                objParam[16] = new SqlParameter("@UserGroupIds", model.UserGroupIds == null ? DBNull.Value : model.UserGroupIds);
                objParam[17] = new SqlParameter("@UserUpdateId", model.UserUpdateId == null ? DBNull.Value : model.UserUpdateId);

                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.Sp_UpdateOrder, objParam);

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateOrderSaler - OrderDal: " + ex);
                return -2;
            }
        }
        public Order GetByOrderNo(string orderNo)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    return _DbContext.Orders.AsNoTracking().FirstOrDefault(s => s.OrderNo == orderNo);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByOrderNo - OrderDal: " + ex);
                return null;
            }
        }
    }
}
