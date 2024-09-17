using Aspose.Cells;
using Entities.ViewModels.TransferSms;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Utilities;


namespace WEB.Adavigo.CMS.Service
{
    public class TransferSmsService
    {
        public List<TransactionSMSViewModel> SearchTransactionSMs(TransferSmsSearchModel searchModel, out long total, int pageIndex = 1, int pageSize = 10)
        {
            var listTransaction = new List<TransactionSMSViewModel>();
            try
            {
                var db = MongodbService.GetDatabase();

                total = 0;
                var collection = db.GetCollection<TransactionSMSViewModel>("BankTransferSMS");
                var filter = Builders<TransactionSMSViewModel>.Filter.Empty;
                filter &= Builders<TransactionSMSViewModel>.Filter.Where(n => n.BankTransferType != 3);
                if (searchModel.Amount != -1)
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Eq(n => n.Amount, searchModel.Amount);
                }
                if (!string.IsNullOrEmpty(searchModel.BankName) && !string.IsNullOrEmpty(searchModel.BankName.Trim()))
                {
                    //filter &= Builders<TransactionSMS>.Filter.Eq("BankName", searchModel.BankName.Trim().ToUpper());
                    filter &= Builders<TransactionSMSViewModel>.Filter.Where(s => s.BankName.ToUpper().Contains(searchModel.BankName.Trim().ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.OrderNo) && !string.IsNullOrEmpty(searchModel.OrderNo.Trim()))
                {
                    //filter &= Builders<TransactionSMS>.Filter.Eq("BookingCode", searchModel.BookingCode.Trim().ToUpper());
                    filter &= Builders<TransactionSMSViewModel>.Filter.Regex("OrderNo", new BsonRegularExpression(".*" + searchModel.OrderNo.Trim().ToUpper() + ".*"));
                }
                if (!string.IsNullOrEmpty(searchModel.FromDateStr))
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Gte("ReceiveTime", searchModel.FromDate);
                }
                if (!string.IsNullOrEmpty(searchModel.ToDateStr))
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Lte("ReceiveTime", searchModel.ToDate);
                }
                if (searchModel.StatusSuccess && searchModel.StatusFail)
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Where(n => n.StatusPush == true || n.StatusPush == false);
                }
                else
                {
                    if (searchModel.StatusFail)
                    {
                        filter &= Builders<TransactionSMSViewModel>.Filter.Eq(n => n.StatusPush, false);
                    }
                    if (searchModel.StatusSuccess)
                    {
                        filter &= Builders<TransactionSMSViewModel>.Filter.Eq(n => n.StatusPush, true);
                    }
                }
                if (searchModel.AmountSuccess && searchModel.AmountFail)
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Where(n => n.Amount > 0 || n.Amount < 0);
                }
                else
                {
                    if (searchModel.AmountSuccess)
                    {
                        filter &= Builders<TransactionSMSViewModel>.Filter.Where(n => n.Amount > 0);
                    }
                    if (searchModel.AmountFail)
                    {
                        filter &= Builders<TransactionSMSViewModel>.Filter.Where(n => n.Amount < 0);
                    }
                }

                var S = Builders<TransactionSMSViewModel>.Sort.Descending("_id");
                total = collection.Find(filter).Count();
                if (pageSize > 0)
                {
                    listTransaction = collection.Find(filter).Sort(S)
                        .Skip((pageIndex - 1) * pageSize).Limit(pageSize)
                        .ToList();
                }
                else
                {
                    listTransaction = collection.Find(filter).Sort(S).ToList();
                }
            }
            catch (Exception ex)
            {
                total = 0;
                LogHelper.InsertLogTelegram("SearchTransactionSMs - TransferSmsService. " + JsonConvert.SerializeObject(ex));
            }
            return listTransaction;
        }
        public totalAmountTransactionSMs SumAmountTransactionSMs(TransferSmsSearchModel searchModel)
        {
            var listTransaction = new List<TransactionSMSViewModel>();
            var totalAmount = new totalAmountTransactionSMs();
            double sumAmount = 0;
            try
            {
                var db = MongodbService.GetDatabase();

                var collection = db.GetCollection<TransactionSMSViewModel>("BankTransferSMS");
                var filter = Builders<TransactionSMSViewModel>.Filter.Empty;

                if (!string.IsNullOrEmpty(searchModel.FromDateStr))
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Gte("ReceiveTime", searchModel.FromDate);
                }
                if (!string.IsNullOrEmpty(searchModel.ToDateStr))
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Lte("ReceiveTime", searchModel.ToDate);
                }
                filter &= Builders<TransactionSMSViewModel>.Filter.Where(n => n.BankTransferType != 3);
                var S = Builders<TransactionSMSViewModel>.Sort.Descending("_id");

                listTransaction = collection.Find(filter).Sort(S).ToList();
                if (searchModel.type == 1)
                {
                    sumAmount = listTransaction.Where(s => s.Amount > 0).Sum(s => s.Amount);
                    totalAmount.total = listTransaction.Where(s => s.Amount > 0).ToList().Count();
                }
                else
                {
                    sumAmount = listTransaction.Where(s => s.Amount < 0).Sum(s => s.Amount);
                    totalAmount.total = listTransaction.Where(s => s.Amount < 0).ToList().Count();
                }
                totalAmount.Amount = sumAmount;


            }
            catch (Exception ex)
            {

                LogHelper.InsertLogTelegram("SearchTransactionSMs - TransferSmsService. " + JsonConvert.SerializeObject(ex));
            }
            return totalAmount;
        }
       
        public List<TransactionSMSViewModel> ListTransactionSMs(TransferSmsSearchModel searchModel)
        {
            var listTransaction = new List<TransactionSMSViewModel>();

            try
            {
                var db = MongodbService.GetDatabase();

                var collection = db.GetCollection<TransactionSMSViewModel>("BankTransferSMS");
                var filter = Builders<TransactionSMSViewModel>.Filter.Empty;
                if (!string.IsNullOrEmpty(searchModel.AccountNumber))
                {
                    //filter &= Builders<TransactionSMSViewModel>.Filter.Regex("AccountNumber", searchModel.AccountNumber);
                    filter &= (Builders<TransactionSMSViewModel>.Filter.Regex("AccountNumber", searchModel.AccountNumber) |
                        (Builders<TransactionSMSViewModel>.Filter.Where(s => s.AccountNumber.Trim().ToUpper().StartsWith( searchModel.AccountNumber.Trim().Substring(0, 2).ToUpper())) &
                   Builders<TransactionSMSViewModel>.Filter.Where(s => s.AccountNumber.Trim().ToUpper().EndsWith( searchModel.AccountNumber.Trim().Substring(searchModel.AccountNumber.Length - 2).ToUpper()))));

                }
                if (!string.IsNullOrEmpty(searchModel.BankName) && !string.IsNullOrEmpty(searchModel.BankName.Trim()))
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Where(s => s.BankName.ToUpper().Contains(searchModel.BankName.Trim().ToUpper()));
                }
                if (!string.IsNullOrEmpty(searchModel.FromDateStr))
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Gte("ReceiveTime", searchModel.FromDate);
                }
                if (!string.IsNullOrEmpty(searchModel.ToDateStr))
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Lte("ReceiveTime", searchModel.ToDate);
                }
                filter &= Builders<TransactionSMSViewModel>.Filter.Where(n => n.BankTransferType != 3);
                var S = Builders<TransactionSMSViewModel>.Sort.Descending("_id");


                listTransaction = collection.Find(filter).Sort(S).ToList();


            }
            catch (Exception ex)
            {

                LogHelper.InsertLogTelegram("SearchTransactionSMs - TransferSmsService. " + JsonConvert.SerializeObject(ex));
            }
            return listTransaction;
        }
        
        public double SumTotalAmountTransactionSMs(string AccountNumber, string BankName, DateTime? ToDate)
        {
            var listTransaction = new List<TransactionSMSViewModel>();

            double totalAmount = 0;
            try
            {
                var db = MongodbService.GetDatabase();

                var collection = db.GetCollection<TransactionSMSViewModel>("BankTransferSMS");
                var filter = Builders<TransactionSMSViewModel>.Filter.Empty;
                if (!string.IsNullOrEmpty(AccountNumber))
                {
                    //filter &= Builders<TransactionSMSViewModel>.Filter.Regex("AccountNumber", AccountNumber);\
                    filter &= Builders<TransactionSMSViewModel>.Filter.Regex("AccountNumber", AccountNumber) |
                                       (Builders<TransactionSMSViewModel>.Filter.Where(s => s.AccountNumber.Trim().ToUpper().StartsWith(AccountNumber.Trim().Substring(0, 2).ToUpper())) &
                                  Builders<TransactionSMSViewModel>.Filter.Where(s => s.AccountNumber.Trim().ToUpper().EndsWith(AccountNumber.Trim().Substring(AccountNumber.Length - 2).ToUpper())));

                }
                if (!string.IsNullOrEmpty(BankName) && !string.IsNullOrEmpty(BankName.Trim()))
                {
                    filter &= Builders<TransactionSMSViewModel>.Filter.Where(s => s.BankName.ToUpper().Contains(BankName.Trim().ToUpper()));
                }
                filter &= Builders<TransactionSMSViewModel>.Filter.Lte("ReceiveTime", ToDate);
                filter &= Builders<TransactionSMSViewModel>.Filter.Where(n => n.BankTransferType != 3);
                var S = Builders<TransactionSMSViewModel>.Sort.Descending("_id");

                listTransaction = collection.Find(filter).Sort(S).ToList();
                totalAmount = listTransaction.Sum(s => s.Amount);
            }
            catch (Exception ex)
            {

                LogHelper.InsertLogTelegram("SumTotalAmountTransactionSMs - TransferSmsService. " + JsonConvert.SerializeObject(ex));
            }
            return totalAmount;
        }
        public async Task<string> ExportDeposit(TransferSmsSearchModel searchModel, string FilePath)
        {
            var pathResult = string.Empty;
            try
            {

                var data = new List<TransactionSMSViewModel>();
                try
                {
                    var db = MongodbService.GetDatabase();


                    var collection = db.GetCollection<TransactionSMSViewModel>("BankTransferSMS");
                    var filter = Builders<TransactionSMSViewModel>.Filter.Empty;
                    if (searchModel.Amount != -1)
                    {
                        filter &= Builders<TransactionSMSViewModel>.Filter.Eq(n => n.Amount, searchModel.Amount);
                    }
                    if (!string.IsNullOrEmpty(searchModel.BankName) && !string.IsNullOrEmpty(searchModel.BankName.Trim()))
                    {
                        //filter &= Builders<TransactionSMS>.Filter.Eq("BankName", searchModel.BankName.Trim().ToUpper());
                        filter &= Builders<TransactionSMSViewModel>.Filter.Where(s => s.BankName.ToUpper().Contains(searchModel.BankName.Trim().ToUpper()));
                    }
                    if (!string.IsNullOrEmpty(searchModel.OrderNo) && !string.IsNullOrEmpty(searchModel.OrderNo.Trim()))
                    {
                        //filter &= Builders<TransactionSMS>.Filter.Eq("BookingCode", searchModel.BookingCode.Trim().ToUpper());
                        filter &= Builders<TransactionSMSViewModel>.Filter.Regex("OrderNo", new BsonRegularExpression(".*" + searchModel.OrderNo.Trim().ToUpper() + ".*"));
                    }
                    if (!string.IsNullOrEmpty(searchModel.FromDateStr))
                    {
                        filter &= Builders<TransactionSMSViewModel>.Filter.Gte("ReceiveTime", searchModel.FromDate);
                    }
                    if (!string.IsNullOrEmpty(searchModel.ToDateStr))
                    {
                        filter &= Builders<TransactionSMSViewModel>.Filter.Lte("ReceiveTime", searchModel.ToDate);
                    }
                    if (searchModel.StatusSuccess && searchModel.StatusFail)
                    {
                        filter &= Builders<TransactionSMSViewModel>.Filter.Where(n => n.StatusPush == true || n.StatusPush == false);
                    }
                    else
                    {
                        if (searchModel.StatusFail)
                        {
                            filter &= Builders<TransactionSMSViewModel>.Filter.Eq(n => n.StatusPush, false);
                        }
                        if (searchModel.StatusSuccess)
                        {
                            filter &= Builders<TransactionSMSViewModel>.Filter.Eq(n => n.StatusPush, true);
                        }
                    }
                    filter &= Builders<TransactionSMSViewModel>.Filter.Where(n => n.BankTransferType != 3);
                    var S = Builders<TransactionSMSViewModel>.Sort.Descending("_id");


                    data = collection.Find(filter).Sort(S).ToList();

                }
                catch (Exception ex)
                {

                    LogHelper.InsertLogTelegram("SearchTransactionSMs - TransferSmsService. " + JsonConvert.SerializeObject(ex));
                }

                if (data != null && data.Count > 0)
                {
                    Workbook wb = new Workbook();
                    Worksheet ws = wb.Worksheets[0];
                    ws.Name = "Danh sách đơn hàng";
                    Cells cell = ws.Cells;

                    var range = ws.Cells.CreateRange(0, 0, 1, 1);
                    StyleFlag st = new StyleFlag();
                    st.All = true;
                    Style style = ws.Cells["A1"].GetStyle();
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

                    #region Header
                    cell.SetColumnWidth(0, 8);
                    cell.SetColumnWidth(1, 60);
                    cell.SetColumnWidth(2, 40);
                    cell.SetColumnWidth(3, 50);
                    cell.SetColumnWidth(4, 50);
                    cell.SetColumnWidth(5, 40);
                    cell.SetColumnWidth(6, 40);

                    ws.Cells["A1"].PutValue("STT");
                    ws.Cells["B1"].PutValue("Nội dung CK");
                    ws.Cells["C1"].PutValue("Tài khoản");
                    ws.Cells["D1"].PutValue("Số tiền chuyển");
                    ws.Cells["E1"].PutValue("Mã đơn hàng");
                    ws.Cells["F1"].PutValue("Ngày nhận CK");
                    ws.Cells["G1"].PutValue("Ngày update đơn hàng");

                    #endregion

                    #region Body

                    range = cell.CreateRange(1, 0, data.Count, 7);
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

                    Style numberStyle = ws.Cells["A2"].GetStyle();
                    numberStyle.Number = 3;
                    numberStyle.HorizontalAlignment = TextAlignmentType.Right;
                    numberStyle.VerticalAlignment = TextAlignmentType.Center;

                    int RowIndex = 1;

                    foreach (var item in data)
                    {
                        string ttchitiet = string.Empty;

                        RowIndex++;
                        ws.Cells["A" + RowIndex].PutValue(RowIndex - 1);
                        ws.Cells["A" + RowIndex].SetStyle(alignCenterStyle);
                        ws.Cells["B" + RowIndex].PutValue(item.MessageContent);
                        ws.Cells["C" + RowIndex].PutValue(item.BankName + " - " + item.AccountNumber);
                        ws.Cells["D" + RowIndex].PutValue(item.Amount);
                        ws.Cells["D" + RowIndex].SetStyle(numberStyle);
                        ws.Cells["E" + RowIndex].PutValue(item.OrderNo);
                        ws.Cells["F" + RowIndex].PutValue(item.ReceiveTimeStr);
                        ws.Cells["G" + RowIndex].PutValue(item.CreatedTimeStr);

                    }

                    #endregion
                    wb.Save(FilePath);
                    pathResult = FilePath;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ExportDeposit - TransferSmsService: " + ex);
            }
            return pathResult;
        }
    }
}
