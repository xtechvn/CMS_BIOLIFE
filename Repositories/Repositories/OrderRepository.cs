using DAL;
using DAL.OrderDetail;
using DAL.StoreProcedure;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.OrderDetail;
using Microsoft.Extensions.Options;
using Nest;
using PdfSharp;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace Repositories.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAL _OrderDal;
        private readonly ClientDAL _clientDAL;
        private readonly AllCodeDAL allCodeDAL;
        private readonly UserDAL userDAL;
        private readonly OrderDetailDAL _orderDetailDAL;
        private readonly ContractPayDAL contractPayDAL;



        public OrderRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _OrderDal = new OrderDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            allCodeDAL = new AllCodeDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            userDAL = new UserDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _clientDAL = new ClientDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _orderDetailDAL = new OrderDetailDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            contractPayDAL = new ContractPayDAL(dataBaseConfig.Value.SqlServer.ConnectionString);


        }
        public async Task<GenericViewModel<OrderViewModel>> GetList(OrderViewSearchModel searchModel)
        {
            var model = new GenericViewModel<OrderViewModel>();

            try
            {
                DataTable dt = await _OrderDal.GetPagingList(searchModel, ProcedureConstants.GETALLORDER_SEARCH);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<OrderViewModel>();
                    model.ListData = data;
                    model.CurrentPage = searchModel.PageIndex;
                    model.PageSize = searchModel.pageSize;
                    model.TotalRecord = Convert.ToInt32(dt.Rows[0]["TotalRow"]);
                    model.TotalPage = (int)Math.Ceiling((double)model.TotalRecord / model.PageSize);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetList - OrderRepository: " + ex);
            }
            return model;
        }
        public async Task<OrderDetailViewModel> GetOrderDetailByOrderId(long OrderId)
        {
            try
            {
                return await _OrderDal.GetDetailOrderByOrderId(OrderId);
                
               
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("GetOrderDetailByOrderId - OrderRepository: " + ex);
            }
            return null;
        }
        public async Task<TotalCountSumOrder> GetTotalCountSumOrder(OrderViewSearchModel searchModel)
        {
            var model = new TotalCountSumOrder();
            try
            {
                searchModel.PageIndex = -1;
                DataTable dt = await _OrderDal.GetPagingList(searchModel, ProcedureConstants.GET_TOTALCOUNT_ORDER);
                if (dt != null && dt.Rows.Count > 0)
                {


                    model.Profit = dt.Rows[0]["Profit"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dt.Rows[0]["Profit"]);
                    model.Amount = dt.Rows[0]["Amount"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dt.Rows[0]["Amount"]);
                    model.Price = dt.Rows[0]["Price"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dt.Rows[0]["Price"]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetTotalCountSumOrder in OrderRepository: " + ex);
            }
            return model;
        }
        public async Task<long> UpdateOrder(Order model)
        {
            try
            {
                return await _OrderDal.UpdateOrder(model);
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("GetTotalCountSumOrder in OrderRepository: " + ex);
            }
            return -1;
        }
        public async Task<List<ListOrderDetailViewModel>> GetListOrderDetail(long orderid)
        {
            try
            {
                return await _orderDetailDAL.GetListOrderDetail(orderid);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetTotalCountSumOrder in OrderRepository: " + ex);
            }
            return null;
        }
        public async Task<Order> GetOrderByOrderNo(string orderNo)
        {
            try
            {
                return _OrderDal.GetByOrderNo(orderNo);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetOrderByOrderNo - OrderRepository: " + ex);
            }
            return null;
        }
        public async Task<List<OrderViewModel>>  GetByClientId(long clientId, int payId = 0, int status = 0)
        {
            try
            {
                var listOrder = new List<OrderViewModel>();
                var listOrderOutput = new List<OrderViewModel>();
                var dt = _OrderDal.GetListOrderByClientId(clientId, ProcedureConstants.SP_GetDetailOrderByClientId, status);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listOrder = dt.ToList<OrderViewModel>();
                    var listContractPayDetail = contractPayDAL.GetByContractDataIds(listOrder.Select(n => Convert.ToInt64(n.OrderId)).ToList());
                    foreach (var item in listOrder)
                    {
                        OrderViewModel orderViewModel = new OrderViewModel();
                        var detail = listContractPayDetail.Where(n => n.DataId != null
                                && n.DataId.Value == Convert.ToInt64(item.OrderId) && n.PayId == payId).FirstOrDefault();
                        var TotalDisarmed = listContractPayDetail.Where(n => n.DataId != null
                                && n.DataId.Value == Convert.ToInt64(item.OrderId)).ToList().Sum(n => n.Amount);
                        item.TotalDisarmed = (double)TotalDisarmed;
                        item.TotalAmount = item.Amount;
                        item.TotalNeedPayment = item.Amount - item.TotalDisarmed;
                        item.CopyProperties(orderViewModel);
                        if (detail != null)
                        {
                            orderViewModel.PayDetailId = detail.Id;
                            orderViewModel.IsChecked = true;
                            orderViewModel.Amount = (double)detail?.Amount;
                            orderViewModel.Payment = (double)detail?.Amount;
                        }

                        if (item.TotalNeedPayment > 0 || (item.Amount == 0 && item.IsFinishPayment == 0))
                        {
                            if (payId <= 0)
                                orderViewModel.Amount = item.TotalNeedPayment;
                            listOrderOutput.Add(orderViewModel);
                        }

                    }
                    if (payId != 0)
                    {
                        var allCode_ORDER_STATUS = allCodeDAL.GetListByType(AllCodeType.ORDER_STATUS);
                        var listOrderId = listOrderOutput.Select(n => Convert.ToInt64(n.OrderId)).ToList();
                        listContractPayDetail = contractPayDAL.GetByContractPayIds(new List<int>() { payId });
                        var listOrderDisable = listContractPayDetail.Where(n => !listOrderId.Contains(n.DataId.Value)).ToList();
                        foreach (var item in listOrderDisable)
                        {
                            OrderViewModel orderViewModel = new OrderViewModel();
                            var order = listOrder.FirstOrDefault(n => Convert.ToInt64(n.OrderId) == item.DataId);
                            if (order != null)
                            {
                                order.CopyProperties(orderViewModel);
                                orderViewModel.Amount = (double)item?.Amount;
                                orderViewModel.Payment = (double)item?.Amount;
                                orderViewModel.TotalDisarmed = order.Amount;
                                orderViewModel.TotalAmount = order.Amount;
                            }
                            else
                            {
                                var orderInfo =await _OrderDal.GetDetailOrderByOrderId(item.DataId.Value);
                                if (orderInfo != null)
                                {
                                    orderViewModel.OrderId = orderInfo.OrderId.ToString();
                                    orderViewModel.OrderNo = orderInfo.OrderNo;
                                    orderViewModel.StartDate = orderInfo.StartDate != null ?
                                        orderInfo.StartDate.ToString("dd:MM:yyyy") : string.Empty;
                                    orderViewModel.EndDate = orderInfo.EndDate != null ?
                                        orderInfo.EndDate.ToString("dd:MM:yyyy") : string.Empty;
                                    orderViewModel.Status = allCode_ORDER_STATUS.FirstOrDefault(n => n.CodeValue == orderInfo.OrderStatus)?.Description;
                                    orderViewModel.SalerName = userDAL.GetById(orderInfo.SalerId != null ? orderInfo.SalerId : 0).Result?.FullName;
                                    orderViewModel.Amount = (double)item?.Amount;
                                    orderViewModel.Payment = (double)item?.Amount;
                                    orderViewModel.TotalDisarmed = orderInfo.Amount;
                                    orderViewModel.TotalAmount = orderInfo.Amount;
                                }
                            }
                            orderViewModel.PayDetailId = item.Id;
                            orderViewModel.IsChecked = true;

                            orderViewModel.IsDisabled = true;
                            listOrderOutput.Add(orderViewModel);
                        }
                    }
                }
                return listOrderOutput;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByClientId - OrderRepository: " + ex);
            }
            return new List<OrderViewModel>();
        }

    }
}
