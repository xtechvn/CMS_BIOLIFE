using Aspose.Cells;
using DAL;
using DAL.Funding;
using DAL.StoreProcedure;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Funding;
using Microsoft.Extensions.Options;
using Nest;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;
using static Utilities.DepositHistoryConstant;

namespace Repositories.Repositories
{
    public class ContractPayRepository : IContractPayRepository
    {
        private readonly ContractPayDAL _contractPayDAL;
        private readonly AllCodeDAL allCodeDAL;
        private readonly OrderDAL orderDAL;
        private readonly DepositHistoryDAL depositHistoryDAL;
        private readonly BankingAccountDAL bankingAccountDAL;
        private readonly ClientDAL clientDAL;
        private readonly UserDAL userDAL;

        public ContractPayRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            allCodeDAL = new AllCodeDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _contractPayDAL = new ContractPayDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            clientDAL = new ClientDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            orderDAL = new OrderDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            depositHistoryDAL = new DepositHistoryDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            bankingAccountDAL = new BankingAccountDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            userDAL = new UserDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public string ExportDeposit(ContractPaySearchModel searchModel, string FilePath)
        {
            var pathResult = string.Empty;
            try
            {
                var contractPays = GetListContractPay(searchModel, out long total, 1, -1);

                if (contractPays != null && contractPays.Count > 0)
                {
                    Workbook wb = new Workbook();
                    Worksheet ws = wb.Worksheets[0];
                    ws.Name = "Danh sách phiếu thu";
                    Cells cell = ws.Cells;

                    var range = ws.Cells.CreateRange(0, 0, 1, 1);
                    StyleFlag st = new StyleFlag();
                    st.All = true;
                    Style style = ws.Cells["A1"].GetStyle();

                    #region Header
                    range = cell.CreateRange(0, 0, 1, 12);
                    style = ws.Cells["A1"].GetStyle();
                    style.Font.IsBold = true;
                    style.IsTextWrapped = true;
                    style.ForegroundColor = Color.FromArgb(33, 88, 103);
                    style.BackgroundColor = Color.FromArgb(33, 88, 103);
                    style.Pattern = BackgroundType.Solid;
                    style.Font.Color = Color.White;
                    style.VerticalAlignment = TextAlignmentType.Center;
                    style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.TopBorder].Color = Color.Black;
                    style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.BottomBorder].Color = Color.Black;
                    style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.LeftBorder].Color = Color.Black;
                    style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.RightBorder].Color = Color.Black;
                    range.ApplyStyle(style, st);

                    // Set column width
                    cell.SetColumnWidth(0, 8);
                    cell.SetColumnWidth(1, 20);
                    cell.SetColumnWidth(2, 40);
                    cell.SetColumnWidth(3, 20);
                    cell.SetColumnWidth(4, 20);
                    cell.SetColumnWidth(5, 30);
                    cell.SetColumnWidth(6, 40);
                    cell.SetColumnWidth(7, 25);
                    cell.SetColumnWidth(8, 25);
                    cell.SetColumnWidth(9, 25);

                    // Set header value
                    ws.Cells["A1"].PutValue("STT");
                    ws.Cells["B1"].PutValue("Mã phiếu");
                    ws.Cells["C1"].PutValue("Loại nghiệp vụ");
                    ws.Cells["D1"].PutValue("Hình thức");
                    ws.Cells["E1"].PutValue("Khách hàng");
                    ws.Cells["F1"].PutValue("Số tiền");
                    ws.Cells["G1"].PutValue("Nội dung");
                    ws.Cells["H1"].PutValue("Đơn hàng/Nạp quỹ liên quan");
                    ws.Cells["I1"].PutValue("Ngày tạo");
                    ws.Cells["J1"].PutValue("Người tạo");
                    #endregion

                    #region Body

                    range = cell.CreateRange(1, 0, contractPays.Count, 12);
                    style = ws.Cells["A2"].GetStyle();
                    style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.TopBorder].Color = Color.Black;
                    style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.BottomBorder].Color = Color.Black;
                    style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.LeftBorder].Color = Color.Black;
                    style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.RightBorder].Color = Color.Black;
                    style.VerticalAlignment = TextAlignmentType.Center;
                    range.ApplyStyle(style, st);

                    Style alignCenterStyle = ws.Cells["A2"].GetStyle();
                    alignCenterStyle.HorizontalAlignment = TextAlignmentType.Center;

                    Style numberStyle = ws.Cells["J2"].GetStyle();
                    numberStyle.Number = 3;
                    numberStyle.HorizontalAlignment = TextAlignmentType.Right;
                    numberStyle.VerticalAlignment = TextAlignmentType.Center;

                    int RowIndex = 1;

                    foreach (var item in contractPays)
                    {
                        if (item.DataContent == null) item.DataContent = new List<CountStatus>();
                        RowIndex++;
                        ws.Cells["A" + RowIndex].PutValue(RowIndex - 1);
                        ws.Cells["A" + RowIndex].SetStyle(alignCenterStyle);
                        ws.Cells["B" + RowIndex].PutValue(item.BillNo);
                        ws.Cells["C" + RowIndex].PutValue(item.ContractPayType);
                        ws.Cells["D" + RowIndex].PutValue(item.PayTypeStr);
                        ws.Cells["E" + RowIndex].PutValue(item.ClientName);
                        ws.Cells["F" + RowIndex].PutValue(item.TotalDeposit.ToString("N0") + "/" + item.Amount.ToString("N0"));
                        ws.Cells["F" + RowIndex].SetStyle(numberStyle);
                        ws.Cells["G" + RowIndex].PutValue(item.Note);
                        ws.Cells["H" + RowIndex].PutValue(string.Join(",", item.DataContent.Select(n => n.DataNo).ToList()));
                        ws.Cells["I" + RowIndex].PutValue(item.CreatedDate != null ? item.CreatedDate.Value.ToString("dd-MM-yyyy HH:mm") : "");
                        ws.Cells["J" + RowIndex].PutValue(item.UserName);
                    }

                    #endregion
                    wb.Save(FilePath);
                    pathResult = FilePath;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ExportPaymentRequest - DepositHistoryRepository: " + ex);
            }
            return pathResult;
        }

        public ContractPay GetById(long contractPayId)
        {
            return _contractPayDAL.GetById(contractPayId);
        }

        //public List<ContractPay> GetByOrderId(long OrderId)
        //{
        //    return _contractPayDAL.GetByOrderId(OrderId);
        //}

        public List<ContractPayViewModel> GetListContractPay(ContractPaySearchModel searchModel, out long total, int currentPage = 1, int pageSize = 20)
        {
            total = 0;
            //var listContractPays = _contractPayDAL.GetContractPays(searchModel, out total, currentPage, pageSize);
            //var dt = _contractPayDAL.GetPagingList(searchModel, currentPage, pageSize, ProcedureConstants.SP_GetListContractPay);
            var listContractPays = _contractPayDAL.GetPagingList(searchModel, currentPage, pageSize,
                ProcedureConstants.SP_GetListContractPay).ToList<ContractPayViewModel>();
            if (listContractPays.Count > 0)
                total = listContractPays.FirstOrDefault().TotalRow;
            var listContractPayView = new List<ContractPayViewModel>();
            var listContractPayDetail = _contractPayDAL.GetByContractPayIds(listContractPays.Select(n => n.PayId).ToList());
            var orderList = orderDAL.GetByOrderIds(listContractPayDetail.Where(n => n.DataId != null).Select(n => n.DataId.Value).ToList());
            var depositHistoryList = depositHistoryDAL.GetByIds(listContractPayDetail.Where(n => n.DataId != null).Select(n => (int)n.DataId.Value).ToList());
            var userList = userDAL.GetByIds(listContractPays.Select(n => (long)n.CreatedBy).ToList()).Result;
            foreach (var item in listContractPays)
            {
                if (string.IsNullOrEmpty(item.UserName))
                {
                    var user = userList.FirstOrDefault(n => n.Id == item.CreatedBy);
                    item.UserName = user?.FullName;
                }
                ContractPayViewModel contractPayViewModel = new ContractPayViewModel();
                item.CopyProperties(contractPayViewModel);
                var contractPayDetail = listContractPayDetail.Where(n => n.PayId == item.PayId).ToList();
                var dataIds = contractPayDetail.Select(n => n.DataId).ToList();
                contractPayViewModel.TotalDeposit = item.TotalDeposit = (double)contractPayDetail.Where(n => n.Amount != null).Sum(n => n.Amount.Value);
                contractPayViewModel.DataContent = new List<CountStatus>();
                contractPayViewModel.DataIds = new List<long>();
                if (!string.IsNullOrEmpty(item.PayDetailId))
                {
                    item.DataIds = item.PayDetailId.Split(",").Select(n => Convert.ToInt64(n)).ToList();
                }
                if (!string.IsNullOrEmpty(item.PayDetail))
                {
                    item.DataNo = item.PayDetail.Split(",").Select(n => n).ToList();
                }

                if (item.Type == (int)CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG)
                {
                    var orders = orderList.Where(n => dataIds.Contains(n.OrderId)).ToList();
                    contractPayViewModel.DataIds = orders.Select(n => n.OrderId).ToList();
                    foreach (var order in orders)
                    {
                        CountStatus countStatus = new CountStatus();
                        countStatus.DataId = order.OrderId;
                        countStatus.DataNo = order.OrderNo;
                        contractPayViewModel.DataContent.Add(countStatus);
                    }
                }
                if (item.Type == (int)CONTRACT_PAY_TYPE.THU_TIEN_KY_QUY)
                {
                    var depositHistories = depositHistoryList.Where(n => dataIds.Contains(n.Id)).ToList();
                    foreach (var depositHistory in depositHistories)
                    {
                        contractPayViewModel.DataIds = depositHistories.Select(n => (long)n.Id).ToList();
                        CountStatus countStatus = new CountStatus();
                        countStatus.DataId = depositHistory.Id;
                        countStatus.DataNo = depositHistory.TransNo;
                        contractPayViewModel.DataContent.Add(countStatus);
                    }
                }
                if (item.Type == (int)CONTRACT_PAY_TYPE.THU_TIEN_NCC_HOAN_TRA || item.Type == (int)CONTRACT_PAY_TYPE.THU_TIEN_HOA_HONG_NCC)
                {
                    if (item.DataNo != null && item.DataNo.Count > 0)
                    {
                        foreach (var serviceCode in item.DataNo)
                        {
                            var serviceInfo = GetServiceDetail(serviceCode);
                            if (serviceInfo != null)
                            {
                                CountStatus countStatus = new CountStatus();
                                countStatus.DataId = serviceInfo.ServiceId;
                                countStatus.DataIdFly = serviceInfo.GroupBookingId;
                                countStatus.DataNo = serviceCode;
                                countStatus.ServiceType = serviceInfo.ServiceType;
                                contractPayViewModel.DataContent.Add(countStatus);
                            }
                        }
                    }
                }
                listContractPayView.Add(contractPayViewModel);
            }
            return listContractPayView;
        }

        public ContractPayViewModel GetByContractPayId(int contractPayId)
        {
            ContractPayViewModel contractPayViewModel = new ContractPayViewModel();
            var contractPay = _contractPayDAL.GetById(contractPayId);
            contractPay.CopyProperties(contractPayViewModel);
            var listContractPayView = new List<ContractPayViewModel>();
            var allCode_PAY_TYPE = allCodeDAL.GetListByType(AllCodeType.PAY_TYPE);
            var allCode_CONTRACT_PAY_TYPE = allCodeDAL.GetListByType(AllCodeType.CONTRACT_PAY_TYPE);
            var allCode_DEPOSITHISTORY_TYPE = allCodeDAL.GetListByType(AllCodeType.DEPOSITHISTORY_TYPE);
            var allCode_DEPOSIT_STATUS = allCodeDAL.GetListByType(AllCodeType.DEPOSIT_STATUS);
            var allCode_SERVICE_TYPE = allCodeDAL.GetListByType(AllCodeType.SERVICE_TYPE);
            var allCode_ORDER_STATUS = allCodeDAL.GetListByType(AllCodeType.ORDER_STATUS);
            var listAccountClient = new List<Client>();
            if (contractPay.CreatedBy != null)
                listAccountClient = clientDAL.GetClientInfo(new List<long>() { (long)contractPay.CreatedBy }).Result;

            var listClient = clientDAL.GetClientByIds(new List<long>() { (long)(contractPay.ClientId != null ? contractPay.ClientId.Value : 0) });
            var listContractPayDetail = _contractPayDAL.GetByContractPayIds(new List<int>() { contractPay.PayId });
            var orderList = orderDAL.GetByOrderIds(listContractPayDetail.Where(n => n.DataId != null).Select(n => n.DataId.Value).ToList());
            var depositHistoryList = depositHistoryDAL.GetByIds(listContractPayDetail.Where(n => n.DataId != null).Select(n => (int)n.DataId.Value).ToList());
            contractPayViewModel.TypeStr = allCode_CONTRACT_PAY_TYPE.FirstOrDefault(n => n.CodeValue == contractPay.Type)?.Description;
            contractPayViewModel.PayTypeStr = allCode_PAY_TYPE.FirstOrDefault(n => n.CodeValue == contractPay.PayType)?.Description;
            var accountClient = listAccountClient.FirstOrDefault(n => n.ClientMapId == contractPay.CreatedBy);
            contractPayViewModel.CreatedByName = accountClient?.ClientName;
            if (string.IsNullOrEmpty(contractPayViewModel.CreatedByName))
            {
                var user = userDAL.GetById((int)contractPay.CreatedBy).Result;
                contractPayViewModel.CreatedByName = user?.FullName;
            }
            var client = listClient.FirstOrDefault(n => n.Id == contractPay.ClientId);
            if (client != null)
                contractPayViewModel.ClientName = client?.ClientName + " - " + client?.Email + " - " + client?.Phone;
            var bankingAccount = bankingAccountDAL.GetById(contractPay.BankingAccountId != null ? contractPay.BankingAccountId.Value : 0);
            contractPayViewModel.BankAccount = bankingAccount?.AccountNumber;
            contractPayViewModel.BankName = bankingAccount?.BankId;
            var contractPayDetail = listContractPayDetail.Where(n => n.PayId == contractPay.PayId).ToList();
            contractPayViewModel.TotalDeposit = (double)contractPayDetail.Where(n => n.Amount != null).Sum(n => n.Amount.Value);
            contractPayViewModel.RelateData = new List<ContractPayDetailViewModel>();
            foreach (var item in contractPayDetail)
            {
                ContractPayDetailViewModel model = new ContractPayDetailViewModel();
                model.ContractPay = contractPay;
                model.ContractPayDetail = item;
                if (contractPay.Type == (int)CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG)
                {
                    //var contractPayDetails = _contractPayDAL.GetByContractDataIds(new List<long>() { (long)item.DataId });
                    //model.ContractPayDetail.Amount = contractPayDetails.Sum(n => n.Amount);
                    model.Order = new OrderViewModel();
                    
                    var order = orderDAL.GetDetailOrderByOrderId(item.DataId != null ? item.DataId.Value : 0).Result;
                    if (order != null)
                    {
                        OrderViewModel orderViewModel = new OrderViewModel();
                        orderViewModel.OrderId = order.OrderId.ToString();
                        orderViewModel.OrderNo = order.OrderNo;
                        if (order.StartDate != null)
                            orderViewModel.StartDate = order.StartDate.ToString("dd/MM/yyyy");
                        if (order.EndDate != null)
                            orderViewModel.EndDate = order.EndDate.ToString("dd/MM/yyyy");
                        orderViewModel.Status = allCode_ORDER_STATUS.FirstOrDefault(n => n.CodeValue == order.OrderStatus)?.Description;
                        orderViewModel.Amount = order.Amount != null ? order.Amount : 0;
                        if (order.SalerId != null)
                        {
                            var accountClientOrder = userDAL.GetById((long)order.SalerId).Result;
                            orderViewModel.CreateName = accountClientOrder?.FullName;
                        }
                        model.Order = orderViewModel;
                    }
                }
                if (contractPay.Type == (int)CONTRACT_PAY_TYPE.THU_TIEN_KY_QUY)
                {
                    var deposit = depositHistoryDAL.GetById(item.DataId != null ? (int)item.DataId.Value : 0);
                    if (deposit != null)
                    {
                        var listAccountClientDetail = clientDAL.GetClientInfo(new List<long>() { (long)deposit?.UserId }).Result;
                        var accountClientDetail = listAccountClientDetail.FirstOrDefault(n => n.ClientMapId == deposit.UserId);
                        DepositFunding depositFunding = new DepositFunding();
                        deposit.CopyProperties(depositFunding);
                        depositFunding.ServiceTypeStr = allCode_SERVICE_TYPE.FirstOrDefault(n => n.CodeValue == deposit?.ServiceType)?.Description;
                        depositFunding.StatusStr = allCode_DEPOSIT_STATUS.FirstOrDefault(n => n.CodeValue == deposit?.Status)?.Description;
                        depositFunding.CreatedBy = accountClientDetail?.ClientName;
                        depositFunding.Email = accountClientDetail?.Email;
                        depositFunding.TransNo = deposit?.TransNo;
                        model.DepositHistory = depositFunding;
                    }
                }
                contractPayViewModel.RelateData.Add(model);
            }
            return contractPayViewModel;
        }

        public ContractPayViewModel GetByPayId(int contractPayId)
        {
            var contractPayViewModel = _contractPayDAL.GetByPayId(contractPayId, StoreProcedureConstant.sp_GetDetailContractPay).ToList<ContractPayViewModel>().FirstOrDefault();
            if (contractPayViewModel.Type == (int)DepositHistoryConstant.CONTRACT_PAY_TYPE.THU_TIEN_DON_HANG)
            {
                var listOrderByPayId = _contractPayDAL.GetDetailContractPayById(contractPayId,
                    StoreProcedureConstant.sp_GetListOrderByPayId).ToList<ContractPayViewModel>();
                contractPayViewModel.ContractPayDetail = listOrderByPayId;
            }

            
            return contractPayViewModel;
        }

        public int CreateContractPay(ContractPayViewModel model)
        {
            var entity = _contractPayDAL.GetByBillNo(model.BillNo);
            if (entity != null)
                return -2;
            return _contractPayDAL.CreateContractPay(model);
        }

        public int UpdateContractPay(ContractPayViewModel model)
        {
            return _contractPayDAL.UpdateContractPay(model);
        }

        public async Task<List<ContractPayDetaiByOrderIdlViewModel>> GetContractPayByOrderId(long OrderId)
        {
            try
            {

                DataTable data = await _contractPayDAL.GetContractPayByOrderId(OrderId);
                var listData = data.ToList<ContractPayDetaiByOrderIdlViewModel>();
                if (listData.Count > 0)
                {
                    return listData;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContractPayByOrderId - ContractPayDAL. " + ex);
            }
            return null;
        }

        public async Task<List<ContractPay>> GetContractPayByClientId(long client)
        {
            try
            {
                var data = await _contractPayDAL.GetContractPayByClientId(client);
                return data;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContractPayByOrderId - ContractPayDAL. " + ex);
            }
            return null;
        }

        public List<OrderDebtViewModel> GetListOrderDebt(ContractPaySearchModel searchModel, out long total, int currentPage = 1, int pageSize = 20)
        {
            List<OrderDebtViewModel> data;
            total = 0;
            try
            {
                data = _contractPayDAL.GetAllOrderDebt(searchModel, currentPage, pageSize,
                 ProcedureConstants.SP_GetAllOrder_Debt).ToList<OrderDebtViewModel>();
                if (data.Count > 0)
                    total = data.FirstOrDefault().TotalRow;
                foreach (var item in data)
                {
                    if (item.Payment == null) item.Payment = 0;
                    if (!string.IsNullOrEmpty(item.PayId))
                    {
                        var contractPays = item.PayId.Split(",");
                        var listContractPay = _contractPayDAL.GetByContractPayIdList(contractPays.Select(n => int.Parse(n)).ToList());
                        item.ContractPays = new List<CountStatus>();
                        foreach (var contractPay in listContractPay)
                        {
                            CountStatus countStatus = new CountStatus();
                            countStatus.DataId = contractPay.PayId;
                            countStatus.DataNo = contractPay.BillNo;
                            item.ContractPays.Add(countStatus);
                        }
                        item.PayBillNo = string.Join(",", listContractPay.Select(n => n.BillNo));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListOrderDebt - ContractPayRepository: " + ex);
                total = 0;
                data = new List<OrderDebtViewModel>();
            }
            return data;
        }

        public List<ContractPayDebtViewModel> GetListContractPayDebt(ContractPaySearchModel searchModel, out long total, int currentPage = 1, int pageSize = 20)
        {
            List<ContractPayDebtViewModel> data;
            total = 0;
            try
            {
                data = _contractPayDAL.GetLisContractPayDebt(searchModel, currentPage, pageSize,
                 ProcedureConstants.SP_GetListContractPayDebt).ToList<ContractPayDebtViewModel>();
                if (data.Count > 0)
                    total = data.FirstOrDefault().TotalRow;
                foreach (var item in data)
                {
                    item.Payment = item.AmountPay;
                    if (!string.IsNullOrEmpty(item.PayDetailId))
                    {
                        var orderIds = item.PayDetailId.Split(",");
                        var listOrders = orderDAL.GetByOrderIds(orderIds.Select(n => long.Parse(n)).ToList());
                        item.ListOrders = new List<CountStatus>();
                        foreach (var contractPay in listOrders)
                        {
                            CountStatus countStatus = new CountStatus();
                            countStatus.DataId = contractPay.OrderId;
                            countStatus.DataNo = contractPay.OrderNo;
                            item.ListOrders.Add(countStatus);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListContractPayDebt - ContractPayRepository: " + ex);
                total = 0;
                data = new List<ContractPayDebtViewModel>();
            }
            return data;
        }

        public List<ContractPayViewModel> GetListContractPayByClientId(long clientId)
        {
            List<ContractPayViewModel> data;
            try
            {
                data = _contractPayDAL.GetByClientId(clientId,
                      ProcedureConstants.SP_GetListContractPayByClientId).ToList<ContractPayViewModel>();
                data = data.Where(n => n.AmountRemain > 0).ToList();
                foreach (var item in data)
                {
                    item.TotalNeedPayment = item.AmountRemain;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByClientId - ContractPayRepository: " + ex);
                data = new List<ContractPayViewModel>();
            }
            return data;
        }

        public int AddContractPayDetail(ContractPayViewModel model, bool isOrder = false)
        {
            return _contractPayDAL.AddContractPayDetail(model, isOrder);
        }

        public string ExportOrderDebt(ContractPaySearchModel searchModel, string FilePath)
        {
            var pathResult = string.Empty;
            try
            {
                var orderDebtList = GetListOrderDebt(searchModel, out long total, 1, -1);

                if (orderDebtList != null && orderDebtList.Count > 0)
                {
                    Workbook wb = new Workbook();
                    Worksheet ws = wb.Worksheets[0];
                    ws.Name = "Danh sách đơn hàng gạch nợ";
                    Cells cell = ws.Cells;

                    var range = ws.Cells.CreateRange(0, 0, 1, 1);
                    StyleFlag st = new StyleFlag();
                    st.All = true;
                    Style style = ws.Cells["A1"].GetStyle();

                    #region Header
                    range = cell.CreateRange(0, 0, 1, 12);
                    style = ws.Cells["A1"].GetStyle();
                    style.Font.IsBold = true;
                    style.IsTextWrapped = true;
                    style.ForegroundColor = Color.FromArgb(33, 88, 103);
                    style.BackgroundColor = Color.FromArgb(33, 88, 103);
                    style.Pattern = BackgroundType.Solid;
                    style.Font.Color = Color.White;
                    style.VerticalAlignment = TextAlignmentType.Center;
                    style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.TopBorder].Color = Color.Black;
                    style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.BottomBorder].Color = Color.Black;
                    style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.LeftBorder].Color = Color.Black;
                    style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.RightBorder].Color = Color.Black;
                    range.ApplyStyle(style, st);

                    // Set column width
                    cell.SetColumnWidth(0, 8);
                    cell.SetColumnWidth(1, 20);
                    cell.SetColumnWidth(2, 40);
                    cell.SetColumnWidth(3, 20);
                    cell.SetColumnWidth(4, 20);
                    cell.SetColumnWidth(5, 30);
                    cell.SetColumnWidth(6, 40);
                    cell.SetColumnWidth(7, 25);
                    cell.SetColumnWidth(8, 25);
                    cell.SetColumnWidth(9, 25);

                    // Set header value
                    ws.Cells["A1"].PutValue("STT");
                    ws.Cells["B1"].PutValue("Mã đơn");
                    ws.Cells["C1"].PutValue("Khách hàng");
                    ws.Cells["D1"].PutValue("Thanh toán");
                    ws.Cells["E1"].PutValue("Trạng thái đơn hàng");
                    ws.Cells["F1"].PutValue("Trạng thái gạch nợ");
                    ws.Cells["G1"].PutValue("Ghi chú");
                    ws.Cells["H1"].PutValue("Phiếu thu");
                    ws.Cells["I1"].PutValue("Ngày tạo");
                    ws.Cells["J1"].PutValue("Người tạo");
                    ws.Cells["K1"].PutValue("Nhân viên chính");
                    #endregion

                    #region Body

                    range = cell.CreateRange(1, 0, orderDebtList.Count, 12);
                    style = ws.Cells["A2"].GetStyle();
                    style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.TopBorder].Color = Color.Black;
                    style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.BottomBorder].Color = Color.Black;
                    style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.LeftBorder].Color = Color.Black;
                    style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.RightBorder].Color = Color.Black;
                    style.VerticalAlignment = TextAlignmentType.Center;
                    range.ApplyStyle(style, st);

                    Style alignCenterStyle = ws.Cells["A2"].GetStyle();
                    alignCenterStyle.HorizontalAlignment = TextAlignmentType.Center;

                    Style numberStyle = ws.Cells["J2"].GetStyle();
                    numberStyle.Number = 3;
                    numberStyle.HorizontalAlignment = TextAlignmentType.Right;
                    numberStyle.VerticalAlignment = TextAlignmentType.Center;

                    int RowIndex = 1;

                    foreach (var item in orderDebtList)
                    {
                        RowIndex++;
                        ws.Cells["A" + RowIndex].PutValue(RowIndex - 1);
                        ws.Cells["A" + RowIndex].SetStyle(alignCenterStyle);
                        ws.Cells["B" + RowIndex].PutValue(item.OrderNo);
                        ws.Cells["C" + RowIndex].PutValue(item.ClientName);
                        if (item.Payment == null || item.Payment == 0)
                            ws.Cells["D" + RowIndex].PutValue("0/" + item.Amount.ToString("N0"));
                        else
                            ws.Cells["D" + RowIndex].PutValue(item.Payment.Value.ToString("N0") + "/" + item.Amount.ToString("N0"));
                        ws.Cells["D" + RowIndex].SetStyle(numberStyle);
                        ws.Cells["E" + RowIndex].PutValue(item.Status);
                        ws.Cells["F" + RowIndex].PutValue(item.DebtStatusName);
                        ws.Cells["G" + RowIndex].PutValue(item.DebtNote);
                        ws.Cells["H" + RowIndex].PutValue(item.PayBillNo);
                        ws.Cells["I" + RowIndex].PutValue(item.CreateTime.Value.ToString("dd-MM-yyyy HH:mm"));
                        ws.Cells["J" + RowIndex].PutValue(item.CreateName);
                        ws.Cells["K" + RowIndex].PutValue(item.SalerName);
                    }

                    #endregion
                    wb.Save(FilePath);
                    pathResult = FilePath;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ExportPaymentRequest - DepositHistoryRepository: " + ex);
            }
            return pathResult;
        }

        public string ExportContractPayDebt(ContractPaySearchModel searchModel, string FilePath)
        {
            var pathResult = string.Empty;
            try
            {
                var orderDebtList = GetListContractPayDebt(searchModel, out long total, 1, -1);

                if (orderDebtList != null && orderDebtList.Count > 0)
                {
                    Workbook wb = new Workbook();
                    Worksheet ws = wb.Worksheets[0];
                    ws.Name = "Danh sách phiếu thu gạch nợ";
                    Cells cell = ws.Cells;

                    var range = ws.Cells.CreateRange(0, 0, 1, 1);
                    StyleFlag st = new StyleFlag();
                    st.All = true;
                    Style style = ws.Cells["A1"].GetStyle();

                    #region Header
                    range = cell.CreateRange(0, 0, 1, 9);
                    style = ws.Cells["A1"].GetStyle();
                    style.Font.IsBold = true;
                    style.IsTextWrapped = true;
                    style.ForegroundColor = Color.FromArgb(33, 88, 103);
                    style.BackgroundColor = Color.FromArgb(33, 88, 103);
                    style.Pattern = BackgroundType.Solid;
                    style.Font.Color = Color.White;
                    style.VerticalAlignment = TextAlignmentType.Center;
                    style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.TopBorder].Color = Color.Black;
                    style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.BottomBorder].Color = Color.Black;
                    style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.LeftBorder].Color = Color.Black;
                    style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.RightBorder].Color = Color.Black;
                    range.ApplyStyle(style, st);

                    // Set column width
                    cell.SetColumnWidth(0, 8);
                    cell.SetColumnWidth(1, 20);
                    cell.SetColumnWidth(2, 40);
                    cell.SetColumnWidth(3, 20);
                    cell.SetColumnWidth(4, 20);
                    cell.SetColumnWidth(5, 30);
                    cell.SetColumnWidth(6, 40);
                    cell.SetColumnWidth(7, 25);
                    cell.SetColumnWidth(8, 25);

                    // Set header value
                    ws.Cells["A1"].PutValue("STT");
                    ws.Cells["B1"].PutValue("Mã phiếu");
                    ws.Cells["C1"].PutValue("Khách hàng");
                    ws.Cells["D1"].PutValue("Số tiền");
                    ws.Cells["E1"].PutValue("Nội dung");
                    ws.Cells["F1"].PutValue("Trạng thái gạch nợ");
                    ws.Cells["G1"].PutValue("Đơn hàng liên quan");
                    ws.Cells["H1"].PutValue("Ngày tạo");
                    ws.Cells["I1"].PutValue("Người tạo");
                    #endregion

                    #region Body

                    range = cell.CreateRange(1, 0, orderDebtList.Count, 12);
                    style = ws.Cells["A2"].GetStyle();
                    style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.TopBorder].Color = Color.Black;
                    style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.BottomBorder].Color = Color.Black;
                    style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.LeftBorder].Color = Color.Black;
                    style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.RightBorder].Color = Color.Black;
                    style.VerticalAlignment = TextAlignmentType.Center;
                    range.ApplyStyle(style, st);

                    Style alignCenterStyle = ws.Cells["A2"].GetStyle();
                    alignCenterStyle.HorizontalAlignment = TextAlignmentType.Center;

                    Style numberStyle = ws.Cells["J2"].GetStyle();
                    numberStyle.Number = 3;
                    numberStyle.HorizontalAlignment = TextAlignmentType.Right;
                    numberStyle.VerticalAlignment = TextAlignmentType.Center;

                    int RowIndex = 1;

                    foreach (var item in orderDebtList)
                    {
                        RowIndex++;
                        ws.Cells["A" + RowIndex].PutValue(RowIndex - 1);
                        ws.Cells["A" + RowIndex].SetStyle(alignCenterStyle);
                        ws.Cells["B" + RowIndex].PutValue(item.BIllNo);
                        ws.Cells["C" + RowIndex].PutValue(item.ClientName);
                        ws.Cells["D" + RowIndex].PutValue(item.Payment.ToString("N0") + "/" + item.Amount.ToString("N0"));
                        ws.Cells["D" + RowIndex].SetStyle(numberStyle);
                        ws.Cells["E" + RowIndex].PutValue(item.Note);
                        ws.Cells["F" + RowIndex].PutValue(item.DebtStatusName);
                        ws.Cells["G" + RowIndex].PutValue(item.PayDetail);
                        ws.Cells["H" + RowIndex].PutValue(item.CreatedDate.Value.ToString("dd-MM-yyyy HH:mm"));
                        ws.Cells["I" + RowIndex].PutValue(item.UserName);
                    }

                    #endregion
                    wb.Save(FilePath);
                    pathResult = FilePath;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ExportPaymentRequest - DepositHistoryRepository: " + ex);
            }
            return pathResult;
        }

        public List<OrderDebtViewModel> GetListOrderDebtByClientId(long clientId)
        {
            List<OrderDebtViewModel> data;
            try
            {
                data = _contractPayDAL.GetByClientId(clientId,
                      ProcedureConstants.sp_GetListOrderDebtByClientId).ToList<OrderDebtViewModel>();
                data = data.Where(n => n.AmountRemain > 0).ToList();
                foreach (var item in data)
                {
                    item.TotalNeedPayment = item.AmountRemain;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListOrderDebtByClientId - ContractPayRepository: " + ex);
                data = new List<OrderDebtViewModel>();
            }
            return data;
        }

        public List<ContractPayViewModel> GetListContractPayByOrderId(long orderId)
        {
            List<ContractPayViewModel> data;
            try
            {
                data = _contractPayDAL.GetByOrderId(orderId, ProcedureConstants.SP_GetListContractPayByOrderId).ToList<ContractPayViewModel>();
                //foreach (var item in data)
                //{
                //    item.TotalNeedPayment = item.AmountRemain;
                //}
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListContractPayByOrderId - ContractPayRepository: " + ex);
                data = new List<ContractPayViewModel>();
            }
            return data;
        }

        public List<ContractPayViewModel> GetListContractPayByPayId(long contractPayId)
        {
            List<ContractPayViewModel> data;
            try
            {
                data = _contractPayDAL.GetByPayId(contractPayId,
                      ProcedureConstants.sp_GetListOrderByPayId).ToList<ContractPayViewModel>();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListByContractPayId - ContractPayRepository: " + ex);
                data = new List<ContractPayViewModel>();
            }
            return data;
        }

        public int DeleteContractPayDetail(List<ContractPayViewModel> model)
        {
            var result = 1;
            try
            {
                result = _contractPayDAL.DeleteContractPayDetailByIds(model);
                if (result >= 0)
                {
                    var listOrder = orderDAL.GetByOrderIds(model.Select(n => n.OrderId).ToList());
                    //check số phiếu thu còn lại của hóa đơn
                    foreach (var item in model)
                    {
                        var listContractPayByOrderIdS = _contractPayDAL.GetByOrderId(item.OrderId);
                        var orderInfo = listOrder.FirstOrDefault(n => n.OrderId == item.OrderId);
                        var client = clientDAL.GetClientDetail((long)orderInfo.ClientId);
                        if (client != null)
                        {
                            item.PermisionType = (int)client.Result.PermisionType;
                        }
                        item.OrderStatus = (byte?)orderInfo.OrderStatus;
                        //nếu đã hết phiếu thu giải trừ cho đơn thì đổi trạng thái đơn, đổi trạng thái dịch vụ theo đơn
                        if (listContractPayByOrderIdS.Count == 0)
                        {
                            _contractPayDAL.UpdateOrderFinishPayment(item, false);
                        }
                        else //nếu còn phiếu thu của đơn thì update status nợ sang chưa đủ
                        {
                            _contractPayDAL.UpdateOrderFinishPayment(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DeleteContractPayDetail - ContractPayRepository: " + ex);
                result = -1;
            }
            return result;
        }

        public ContractPayViewModel GetDetailContractPay(long payId)
        {
            ContractPayViewModel data;
            try
            {
                data = _contractPayDAL.GetByClientId(payId,
                      ProcedureConstants.sp_GetDetailContractPay).ToList<ContractPayViewModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByClientId - ContractPayRepository: " + ex);
                data = new ContractPayViewModel();
            }
            return data;
        }

        public int UndoContractPayByCancelService(int contractPayIds, long orderId, int userId)
        {
            var result = 1;
            try
            {
                result = _contractPayDAL.UndoContractPayByCancelService(contractPayIds, orderId, userId);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UndoContractPayByCancelService - ContractPayRepository: " + ex);
                result = -1;
            }
            return result;
        }

        public List<PaymentRequestViewModel> GetContractPayServiceListBySupplierId(long supplierId, int contractPayId = 0, int serviceId = 0)
        {
            try
            {
                var listService = new List<PaymentRequestViewModel>();
                if (supplierId == 0) return listService;
                //var listServiceOutput = _contractPayDAL.GetContractPayServiceListBySupplierId(supplierId,
                //    ProcedureConstants.SP_GetAllServiceBySupplierIdForReturn).ToList<PaymentRequestViewModel>();
                var listServiceOutput = _contractPayDAL.GetContractPayServiceListBySupplierId(supplierId,
                   ProcedureConstants.SP_GetAllSubServiceBySupplierIdForReturn).ToList<PaymentRequestViewModel>();
                //foreach (var item in listSubServiceOutput)
                //{
                //    listServiceOutput.Add(item);
                //}
                var listServiceId = listServiceOutput.Select(n => Convert.ToInt64(n.ServiceId)).ToList();
                var listRequestDetail = _contractPayDAL.GetByDataIdsService(listServiceId);
                if (serviceId != 0)
                    listServiceOutput = listServiceOutput.Where(n => n.ServiceId == serviceId).ToList();
                foreach (var item in listServiceOutput)
                {
                    item.TotalAmount = item.Amount;
                    var detail = listRequestDetail.Where(n => n.OrderId == item.OrderId && n.ContractPayId == contractPayId
                    && n.ServiceId == item.ServiceId).FirstOrDefault();
                    if (detail != null)
                    {
                        item.IsChecked = true;
                        item.Id = detail.Id;
                        item.AmountPayment = detail.Amount;
                        item.Payment = detail.Amount;
                        item.TotalDisarmed = item.AmountReturn - detail.Amount;
                    }
                    else
                    {
                        item.TotalDisarmed = item.AmountReturn;
                    }
                    item.TotalNeedPayment = item.Amount - item.TotalDisarmed;
                    if (item.TotalNeedPayment < 0)
                        item.TotalNeedPayment = 0;
                    PaymentRequestViewModel model = new PaymentRequestViewModel();
                    item.CopyProperties(model);
                    if (contractPayId > 0)
                    {
                        if (listService.FirstOrDefault(n => n.ServiceId == item.ServiceId) == null)
                        {
                            if (detail != null)
                            {
                                //item.IsDisabled = true;
                                item.IsChecked = true;
                            }
                            listService.Add(item);
                        }
                    }
                    else
                    {
                        if (listService.FirstOrDefault(n => n.ServiceId == item.ServiceId) == null)
                            listService.Add(item);
                    }
                }
                return listService;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContractPayServiceListBySupplierId - ContractPayRepository: " + ex);
            }
            return new List<PaymentRequestViewModel>();
        }

        public PaymentRequestViewModel GetServiceDetail(string serviceCode)
        {
            try
            {
                var listServiceOutput = _contractPayDAL.GetServiceDetail(serviceCode,
                    ProcedureConstants.SP_GetAllServiceByServiceCode).ToList<PaymentRequestViewModel>();
                return listServiceOutput.FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetServiceDetail - ContractPayRepository: " + ex);
            }
            return null;
        }

        public List<ContractPayViewModel> GetContractPayBySupplierId(long orderId, long serviceId, int serviceType)
        {
            try
            {
                var listServiceOutput = _contractPayDAL.GetContractPayBySupplierId(orderId, serviceId, serviceType,
                    ProcedureConstants.SP_GetListContractPayByServiceId).ToList<ContractPayViewModel>();
                return listServiceOutput.ToList();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetServiceDetail - ContractPayRepository: " + ex);
            }
            return new List<ContractPayViewModel>();
        }

        public double GetTotalAmountContractPayByServiceId(string ServiceId, long ServiceType, long ContractPayType)
        {
            try
            {

                return _contractPayDAL.GetTotalAmountContractPayByServiceId(ServiceId, ServiceType, ContractPayType);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetServiceDetail - ContractPayRepository: " + ex);
            }
            return 0;
        }
    }
}
