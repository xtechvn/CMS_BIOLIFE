using DAL.Funding;
using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels.Funding;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;
using Utilities.Contants;
using System.Linq;
using System.Threading.Tasks;
using Entities.ViewModels;
using Aspose.Cells;
using System.Drawing;
using DAL.StoreProcedure;
using System.Data;
using System.Globalization;

namespace Repositories.Repositories
{
    public class DepositHistoryRepository : IDepositHistoryRepository
    {
        private readonly DepositHistoryDAL depositHistoryDAL;
        private readonly AllCodeDAL allCodeDAL;
        private readonly ClientDAL clientDAL;
        private readonly PaymentDAL paymentDAL;
        private readonly UserDAL userDAL;
        private readonly ContractPayDAL contractPayDAL;
        private readonly BankingAccountDAL bankingAccountDAL;

        public DepositHistoryRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {

            depositHistoryDAL = new DepositHistoryDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            allCodeDAL = new AllCodeDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            clientDAL = new ClientDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            paymentDAL = new PaymentDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            userDAL = new UserDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            bankingAccountDAL = new BankingAccountDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            contractPayDAL = new ContractPayDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public DepositFunding GetById(int depositHistoryId)
        {
            DepositFunding depositFunding = new DepositFunding();
            var depositHistory = depositHistoryDAL.GetById(depositHistoryId);
            depositHistory.CopyProperties(depositFunding);
            var listPayment = paymentDAL.GetByDepositHistoryIds(new List<int>() { depositHistory.Id }).Result;
            depositFunding.Payments = listPayment;
            depositFunding.TotalDeposit = listPayment.Sum(n => n.Amount);
            var listContractPay = contractPayDAL.GetByDataIds(new List<int>() { depositHistory.Id });
            listContractPay = listContractPay.Where(n => n.ContractPay != null).ToList();
            depositFunding.TotalDeposit += listContractPay.Where(n => n.ContractPay != null).Sum(n => n.ContractPay.Amount);
            var listUserIds = new List<long>();
            foreach (var item in listContractPay)
            {
                if (item.ContractPay != null)
                    listUserIds.Add((long)item.ContractPay.CreatedBy.Value);
            }
            var users = userDAL.GetByIds(listUserIds).Result;
            var bankingAccounts = bankingAccountDAL.GetAllBankingAccount();
            var allCode_CONTRACT_PAY_TYPE = allCodeDAL.GetListByType(AllCodeType.PAY_TYPE);
            foreach (var item in listContractPay)
            {
                var user = users.FirstOrDefault(n => n.Id == item.ContractPay.CreatedBy);
                item.CreatedByName = user?.FullName;
                var bankingAccount = bankingAccounts.FirstOrDefault(n => n.Id == item.ContractPay.BankingAccountId);
                item.BankAccount = bankingAccount?.AccountNumber;
                item.BankName = bankingAccount?.BankId;
                item.PaymentTypeStr = allCode_CONTRACT_PAY_TYPE.FirstOrDefault(n => n.CodeValue == item.ContractPay.PayType)?.Description;
            }
            depositFunding.ContractPays = listContractPay;
            var listAccountClientId = new List<long>();
            var listUserId = new List<long>();
            if (depositHistory.UserId != null)
                listAccountClientId.Add(depositHistory.UserId.Value);
            if (depositHistory.UserVerifyId != null)
                listUserId.Add(depositHistory.UserVerifyId.Value);
            var listClient = clientDAL.GetClientInfo(listAccountClientId).Result;
            var clientCreate = listClient.FirstOrDefault(n => n.ClientMapId == depositHistory.UserId);
            depositFunding.CreatedBy = clientCreate?.ClientName;
            depositFunding.Email = clientCreate?.Email;
            depositFunding.ClientName = clientCreate?.ClientName;
            depositFunding.ClientEmail = clientCreate?.Email;
            depositFunding.Mobile = clientCreate?.Phone;

            var clientVerify = userDAL.GetByIds(listUserId).Result.FirstOrDefault();
            depositFunding.Approver = clientVerify?.FullName;
            depositFunding.EmailVerify = clientVerify?.Email;

            var allCode_SERVICE_TYPE = allCodeDAL.GetListByType(AllCodeType.SERVICE_TYPE);
            var allCode_PAYMENT_TYPE = allCodeDAL.GetListByType(AllCodeType.PAYMENT_TYPE);
            var allCode_DEPOSITHISTORY_TYPE = allCodeDAL.GetListByType(AllCodeType.DEPOSITHISTORY_TYPE);
            var allCode_DEPOSIT_STATUS = allCodeDAL.GetListByType(AllCodeType.DEPOSIT_STATUS);
            depositFunding.ServiceTypeStr = allCode_SERVICE_TYPE.FirstOrDefault(n => n.CodeValue == depositHistory.ServiceType)?.Description;
            depositFunding.TransTypeStr = allCode_DEPOSITHISTORY_TYPE.FirstOrDefault(n => n.CodeValue == depositHistory.TransType)?.Description;
            depositFunding.StatusStr = allCode_DEPOSIT_STATUS.FirstOrDefault(n => n.CodeValue == depositHistory.Status)?.Description;
            depositFunding.PaymentTypeStr = allCode_PAYMENT_TYPE.FirstOrDefault(n => n.CodeValue == depositHistory.PaymentType)?.Description;
            return depositFunding;
        }

        public DepositFunding GetByNo(string depositHistoryNo)
        {
            DepositFunding depositFunding = new DepositFunding();
            var depositHistory = depositHistoryDAL.GetByNo(depositHistoryNo);
            depositHistory.CopyProperties(depositFunding);
            var listPayment = paymentDAL.GetByDepositHistoryIds(new List<int>() { depositHistory.Id }).Result;
            depositFunding.Payments = listPayment;
            depositFunding.TotalDeposit = listPayment.Sum(n => n.Amount);
            var listContractPay = contractPayDAL.GetByDataIds(new List<int>() { depositHistory.Id });
            depositFunding.TotalDeposit += listContractPay.Sum(n => n.ContractPay.Amount);
            var listAccountClientId = new List<long>();
            var listUserId = new List<long>();
            if (depositHistory.UserId != null)
                listAccountClientId.Add(depositHistory.UserId.Value);
            if (depositHistory.UserVerifyId != null)
                listUserId.Add(depositHistory.UserVerifyId.Value);
            var listClient = clientDAL.GetClientInfo(listAccountClientId).Result;
            var clientCreate = listClient.FirstOrDefault(n => n.ClientMapId == depositHistory.UserId);
            depositFunding.CreatedBy = clientCreate?.ClientName;
            depositFunding.Email = clientCreate?.Email;
            depositFunding.ClientName = clientCreate?.ClientName;
            depositFunding.ClientEmail = clientCreate?.Email;
            depositFunding.Mobile = clientCreate?.Phone;

            var clientVerify = userDAL.GetByIds(listUserId).Result.FirstOrDefault();
            depositFunding.Approver = clientVerify?.FullName;
            depositFunding.EmailVerify = clientVerify?.Email;

            var allCode_SERVICE_TYPE = allCodeDAL.GetListByType(AllCodeType.SERVICE_TYPE);
            var allCode_PAYMENT_TYPE = allCodeDAL.GetListByType(AllCodeType.PAYMENT_TYPE);
            var allCode_DEPOSITHISTORY_TYPE = allCodeDAL.GetListByType(AllCodeType.DEPOSITHISTORY_TYPE);
            var allCode_DEPOSIT_STATUS = allCodeDAL.GetListByType(AllCodeType.DEPOSIT_STATUS);
            depositFunding.ServiceTypeStr = allCode_SERVICE_TYPE.FirstOrDefault(n => n.CodeValue == depositHistory.ServiceType)?.Description;
            depositFunding.TransTypeStr = allCode_DEPOSITHISTORY_TYPE.FirstOrDefault(n => n.CodeValue == depositHistory.TransType)?.Description;
            depositFunding.StatusStr = allCode_DEPOSIT_STATUS.FirstOrDefault(n => n.CodeValue == depositHistory.Status)?.Description;
            depositFunding.PaymentTypeStr = allCode_PAYMENT_TYPE.FirstOrDefault(n => n.CodeValue == depositHistory.PaymentType)?.Description;
            return depositFunding;
        }

        public List<DepositFunding> GetDepositHistories(FundingSearch searchModel, out long total, out List<CountStatus> countStatus,
            int currentPage = 1, int pageSize = 20)
        {
            var listDepositHistory = new List<DepositFunding>();
            try
            {
                //get config 
                var allCode_SERVICE_TYPE = allCodeDAL.GetListByType(AllCodeType.SERVICE_TYPE);
                var allCode_PAYMENT_TYPE = allCodeDAL.GetListByType(AllCodeType.PAYMENT_TYPE);
                var allCode_DEPOSITHISTORY_TYPE = allCodeDAL.GetListByType(AllCodeType.DEPOSITHISTORY_TYPE);
                var allCode_DEPOSIT_STATUS = allCodeDAL.GetListByType(AllCodeType.DEPOSIT_STATUS);

                var listDeposit = depositHistoryDAL.GetDepositHistories(searchModel, out total, out countStatus, currentPage, pageSize);

                //get client info
                var listUserCreate = listDeposit.Where(n => n.UserId != null).Select(n => n.UserId.Value).ToList();
                var listClient = clientDAL.GetClientInfo(listUserCreate).Result;

                //get user info
                var listUserVerify = listDeposit.Where(n => n.UserVerifyId != null).Select(n => n.UserVerifyId.Value).ToList();
                var listUserInfo = userDAL.GetByIds(listUserVerify).Result;
                var listPayment = paymentDAL.GetByDepositHistoryIds(listDeposit.Select(n => n.Id).ToList()).Result;
                var listContractPay = contractPayDAL.GetByDataIds(listDeposit.Select(n => n.Id).ToList());
                foreach (var item in listDeposit)
                {
                    DepositFunding depositFunding = new DepositFunding();
                    item.CopyProperties(depositFunding);
                    depositFunding.ServiceTypeStr = allCode_SERVICE_TYPE.FirstOrDefault(n => n.CodeValue == item.ServiceType)?.Description;
                    depositFunding.TransTypeStr = allCode_DEPOSITHISTORY_TYPE.FirstOrDefault(n => n.CodeValue == item.TransType)?.Description;
                    depositFunding.PaymentTypeStr = allCode_PAYMENT_TYPE.FirstOrDefault(n => n.CodeValue == item.PaymentType)?.Description;
                    depositFunding.countStatus = countStatus;
                    var client = listClient.FirstOrDefault(n => n.ClientMapId == item.UserId);
                    depositFunding.CreatedBy = client?.ClientName;
                    depositFunding.Email = client?.Email;
                    var userVefiry = listUserInfo.FirstOrDefault(n => n.Id == item.UserVerifyId);
                    depositFunding.Approver = userVefiry?.FullName;
                    depositFunding.EmailVerify = userVefiry?.Email;
                    var totalAmount = listPayment.Where(n => n.OrderId == item.Id).Sum(n => n.Amount);
                    totalAmount += listContractPay.Where(n => n.ContractPayDetail.DataId == item.Id).Sum(n => (double)n.ContractPayDetail.Amount);
                    depositFunding.TotalDeposit = totalAmount;
                    depositFunding.StatusStr = allCode_DEPOSIT_STATUS.FirstOrDefault(n => n.CodeValue == depositFunding.Status)?.Description;
                    listDepositHistory.Add(depositFunding);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDepositHistories - DepositHistoryRepository: " + ex);
                total = 0;
                countStatus = new List<CountStatus>();
            }
            return listDepositHistory;
        }

        public GenericViewModel<DepositHistoryViewMdel> getDepositHistoryByUserId(long userId, int currentPage = 1, int pageSize = 20)
        {

            var model = new GenericViewModel<DepositHistoryViewMdel>();
            try
            {
                var AccountClient = clientDAL.GetAccountClientIdDetai(userId).Result;
                if (AccountClient != null)
                {
                    var data = depositHistoryDAL.getDepositHistoryByUserId((long)AccountClient.Id, currentPage, pageSize, out int totalRecord);
                    model.ListData = data;
                    model.PageSize = pageSize;
                    model.CurrentPage = currentPage;
                    model.TotalRecord = totalRecord;
                    model.TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
                }


                return model;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getDepositHistoryByUserId - DepositHistoryRepository: " + ex);
                return null;
            }
        }

        public List<ContractPay> GetContractPays()
        {
            try
            {
                return contractPayDAL.GetForAddContract();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetContractPays - DepositHistoryRepository: " + ex);
                return new List<ContractPay>();
            }
        }

        //public List<ContractPay> GetContractPayByOrderId(long orderId)
        //{
        //    try
        //    {
        //        return contractPayDAL.GetByOrderId(orderId);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.InsertLogTelegram("GetContractPayByOrderId - DepositHistoryRepository: " + ex);
        //        return new List<ContractPay>();
        //    }
        //}

        public int DeleteContractPay(long contractPayId)
        {
            return contractPayDAL.CancelById(contractPayId);
        }

        public string ExportDeposit(FundingSearch searchModel, string FilePath)
        {
            var pathResult = string.Empty;
            try
            {
                var depositHistories = GetDepositHistories(searchModel, out long total, out List<CountStatus> countStatus, 1, -1);

                if (depositHistories != null && depositHistories.Count > 0)
                {
                    Workbook wb = new Workbook();
                    Worksheet ws = wb.Worksheets[0];
                    ws.Name = "Danh sách nạp quỹ";
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
                    cell.SetColumnWidth(6, 30);
                    cell.SetColumnWidth(7, 25);
                    cell.SetColumnWidth(8, 25);
                    cell.SetColumnWidth(9, 25);
                    cell.SetColumnWidth(10, 25);
                    cell.SetColumnWidth(11, 25);
                    cell.SetColumnWidth(12, 25);

                    // Set header value
                    ws.Cells["A1"].PutValue("STT");
                    ws.Cells["B1"].PutValue("Mã giao dịch");
                    ws.Cells["C1"].PutValue("Tiêu đề");
                    ws.Cells["D1"].PutValue("Loại quỹ");
                    ws.Cells["E1"].PutValue("Số tiền nạp");
                    ws.Cells["F1"].PutValue("Loại giao dịch");
                    ws.Cells["G1"].PutValue("Hình thức thanh toán");
                    ws.Cells["H1"].PutValue("Ngày giao dịch");
                    ws.Cells["I1"].PutValue("Người tạo");
                    ws.Cells["J1"].PutValue("Ngày duyệt");
                    ws.Cells["K1"].PutValue("Người  duyệt");
                    ws.Cells["L1"].PutValue("Trạng thái");
                    #endregion

                    #region Body

                    range = cell.CreateRange(1, 0, depositHistories.Count, 12);
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

                    foreach (var item in depositHistories)
                    {
                        RowIndex++;
                        ws.Cells["A" + RowIndex].PutValue(RowIndex - 1);
                        ws.Cells["A" + RowIndex].SetStyle(alignCenterStyle);
                        ws.Cells["B" + RowIndex].PutValue(item.TransNo);
                        ws.Cells["C" + RowIndex].PutValue(item.Title);
                        ws.Cells["D" + RowIndex].PutValue(item.ServiceTypeStr);
                        ws.Cells["E" + RowIndex].PutValue(item.TotalDeposit.ToString("N0") + "/" + item.Price.Value.ToString("N0"));
                        ws.Cells["E" + RowIndex].SetStyle(numberStyle);
                        ws.Cells["F" + RowIndex].PutValue(item.TransTypeStr);
                        ws.Cells["G" + RowIndex].PutValue(item.PaymentTypeStr);
                        ws.Cells["H" + RowIndex].PutValue(item.CreateDate != null ? item.CreateDate.Value.ToString("dd-MM-yyyy HH:mm") : "");
                        ws.Cells["I" + RowIndex].PutValue(item.CreatedBy);
                        ws.Cells["J" + RowIndex].PutValue(item.VerifyDate != null ? item.VerifyDate.Value.ToString("dd-MM-yyyy HH:mm") : "");
                        ws.Cells["K" + RowIndex].PutValue(item.Approver);
                        ws.Cells["L" + RowIndex].PutValue(item.StatusStr);
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

        public List<DepositFunding> GetByClientId(long clientId, int payId = 0)
        {
            try
            {
                var listDepositFunding = new List<DepositFunding>();
                var listDepositFundingOutput = new List<DepositFunding>();
                var dt = depositHistoryDAL.GetListOrderByClientId(clientId, ProcedureConstants.SP_GetDepositHistoryByClientId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listDepositFunding = (from row in dt.AsEnumerable()
                                          select new DepositFunding
                                          {
                                              Id = Convert.ToInt32(row["Id"].ToString()),
                                              TransNo = row["TransNo"].ToString(),
                                              ServiceName = row["ServiceName"].ToString(),
                                              StatusStr = row["Status"].ToString(),
                                              UserName = row["UserName"].ToString(),
                                              Price = !row["Amount"].Equals(DBNull.Value) ? Convert.ToDouble(row["Amount"].ToString()) : 0,
                                          }).ToList();
                    if (payId > 0)
                    {
                        var allCode_SERVICE_TYPE = allCodeDAL.GetListByType(AllCodeType.SERVICE_TYPE);
                        var allCode_DEPOSIT_STATUS = allCodeDAL.GetListByType(AllCodeType.DEPOSIT_STATUS);
                        var contractPayDetail = contractPayDAL.GetByContractPayIds(new List<int>() { payId });
                        foreach (var item in contractPayDetail)
                        {
                            var depositHisInfo = depositHistoryDAL.GetById((int)item.DataId.Value);
                            DepositFunding deposit = new DepositFunding();
                            depositHisInfo.CopyProperties(deposit);
                            deposit.ServiceName = allCode_SERVICE_TYPE.FirstOrDefault(n => n.CodeValue == deposit.ServiceType)?.Description;
                            deposit.StatusStr = allCode_DEPOSIT_STATUS.FirstOrDefault(n => n.CodeValue == deposit.Status)?.Description;
                            var accountClient = clientDAL.GetAccountClientByID(depositHisInfo.UserId.Value).Result;
                            deposit.UserName = accountClient?.UserName;
                            if (listDepositFunding.FirstOrDefault(n => n.Id == depositHisInfo.Id) == null)
                                listDepositFunding.Add(deposit);
                        }
                    }
                    var listContractPayDetail = contractPayDAL.GetByContractDataIds(listDepositFunding.Select(n => Convert.ToInt64(n.Id)).ToList());
                    foreach (var item in listDepositFunding)
                    {
                        DepositFunding depositFunding = new DepositFunding();
                        var details = listContractPayDetail.Where(n => n.DataId != null && n.DataId.Value == Convert.ToInt64(item.Id)).ToList();
                        item.TotalDisarmed = details.Sum(n => (double)n.Amount);
                        item.TotalNeedPayment = item.Price.Value - item.TotalDisarmed;
                        item.CopyProperties(depositFunding);
                        var detail = listContractPayDetail.Where(n => n.DataId != null
                             && n.DataId.Value == Convert.ToInt64(item.Id) && n.PayId == payId).FirstOrDefault();
                        if (detail != null)
                        {
                            depositFunding.PayDetailId = detail.Id;
                            depositFunding.IsChecked = true;
                            depositFunding.Payment = (double)detail?.Amount;
                        }
                        if (item.TotalNeedPayment > 0)
                            listDepositFundingOutput.Add(depositFunding);
                    }
                }
                return listDepositFundingOutput;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByClientId - OrderRepository" + ex);
            }
            return new List<DepositFunding>();
        }
    }
}

