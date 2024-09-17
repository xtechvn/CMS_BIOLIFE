using Aspose.Cells;
using Caching.Elasticsearch;
using DAL;
using DAL.StoreProcedure;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.CustomerManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace Repositories.Repositories
{
    public class CustomerManagerRepository : ICustomerManagerRepository
    {
        private readonly ClientDAL _ClientDAL;
        private readonly PaymentAccountDAL _PaymentAccountDAL;
        private readonly IAllCodeRepository _allCodeRepository;
        private readonly UserAgentDAL _UserAgentDAL;
        private readonly AccountClientDAL _AccountClientDAL;
        private readonly UserDAL _UserDAL;
        private ClientESRepository _clientESRepository;
        private readonly IConfiguration _configuration;
        public CustomerManagerRepository(IConfiguration configuration, IOptions<DataBaseConfig> dataBaseConfig, IAllCodeRepository allCodeRepository)
        {
            _configuration = configuration;
            _ClientDAL = new ClientDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _allCodeRepository = allCodeRepository;
            _PaymentAccountDAL = new PaymentAccountDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _UserAgentDAL = new UserAgentDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _AccountClientDAL = new AccountClientDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _UserDAL = new UserDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _clientESRepository = new ClientESRepository(_configuration["DataBaseConfig:Elastic:Host"]);
        }

        public int SetUpClient(CustomerManagerView model)
        {
            try
            {
                if (model.Id == 0)
                {
                    //tạo mới
                    var a = 1;
                    var CreateAccount = 1;
                    var CreatePaymentAccount_type = 1;
                    var Client = new Client
                    {
                        Id = model.Id,
                        AgencyType = model.AgencyType,
                        ClientType = Convert.ToInt32(model.id_ClientType),
                        PermisionType = Convert.ToInt32(model.id_nhomkhach),
                        Email = model.email,
                        ClientName = model.Client_name,
                        Status = 0,
                        Note = model.Note,
                        JoinDate = DateTime.Now,
                        Phone = model.phone,
                        UpdateTime = DateTime.Now,
                        Birthday = DateTime.Now,
                        BusinessAddress = model.DiaChi_giaodich,
                        ExportBillAddress = model.DC_hoadon,
                        TaxNo = model.Maso_Id,
                        ClientCode = model.ClientCode,
                        SaleMapId = (int?)model.UserId,
                        ParentId = -1,

                    };
                    var CreateClient = _ClientDAL.SetUpClient(Client);
                    var data2 = _ClientDAL.GetClientByEmail(model.email);
                    if (CreateClient > 0 && CreateClient != 2)
                    {
                        if (data2 != null)
                        {
                            var Account_Client = new AccountClient
                            {
                                ClientId = data2.Id,
                                ClientType = Convert.ToInt32(model.id_ClientType),
                                UserName = model.email,
                                Password = EncodeHelpers.MD5Hash("123456"),
                                PasswordBackup = EncodeHelpers.MD5Hash("123456"),
                                Status = 0,
                                GroupPermission= GroupPermissionStatus.Admin,

                            };
                          
                            var CreateAccountClient = _AccountClientDAL.CreateAccountClient(Account_Client);

                            if (Convert.ToInt32(model.id_loaikhach) != -1)
                            {
                                if (model.So_tk != "" && model.Name_tk != "" && model.Name_nh != "" && model.diachi_chinhanh != "")
                                {
                                    var PaymentAccountModel = new PaymentAccount
                                    {
                                        AccountName = model.Name_tk,
                                        AccountNumb = model.So_tk,
                                        BankName = model.Name_nh,
                                        ClientId = data2.Id,
                                        Branch = model.diachi_chinhanh,
                                    };
                                    var CreatePaymentAccount = _PaymentAccountDAL.CreatePaymentAccount(PaymentAccountModel);
                                    CreatePaymentAccount_type = CreatePaymentAccount;
                                }
                            }

                            var UserAgent = new UserAgent
                            {
                                ClientId = data2.Id,
                                UpdateLast = DateTime.Now,
                                CreateDate = DateTime.Now,
                                VerifyDate = DateTime.Now,
                                VerifyStatus = VerifyStatus.CHUA_DUYET,
                                MainFollow = (short?)MainFollow.SALER,
                                CreatedBy = (int)model.UserId,
                                UpdatedBy = (int)model.UserId,
                                UserId = (int)model.UserId,
                            };
                            var CreateUserAgent = _UserAgentDAL.CreateUserAgent(UserAgent);
                            if(CreateAccountClient > 0 && CreateUserAgent == 1)
                            {
                                return 1;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        return 0;
                    }
                    else
                    {
                        return CreateClient;
                    }

                }
                else
                {
                    var data = _UserAgentDAL.GetUserAgentClient((int)model.Id);
                    var data2 = _ClientDAL.GetClientDetail(model.Id).Result;
                    if (data != null)
                    {
                        data.UpdatedBy = Convert.ToInt32(model.UserId);
                        //var updataUserAgent = _UserAgentDAL.UpdataUserAgentClient(data);
                    }
                    var Client = new Client
                    {
                        Id = model.Id,
                        AgencyType = model.AgencyType,
                        ClientType = Convert.ToInt32(model.id_ClientType),
                        PermisionType = Convert.ToInt32(model.id_nhomkhach),
                        Email = model.email,
                        ClientName = model.Client_name,
                        JoinDate = model.JoinDate,
                        Status = 0,
                        Note = model.Note,
                        Phone = model.phone,
                        UpdateTime = DateTime.Now,
                        BusinessAddress = model.DiaChi_giaodich,
                        ExportBillAddress = model.DC_hoadon,
                        TaxNo = model.Maso_Id,
                        ClientCode = model.ClientCode,
                        ParentId = data2.ParentId,
                    };

                    var CreateClient = _ClientDAL.SetUpClient(Client);


                    return CreateClient;

                }


            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("SetUpClient - CustomerManagerRepository: " + ex);
                return 0;
            }
        }

        public async Task<CustomerManagerViewModel> GetDetailClient(long ClientId)
        {
            var model = new CustomerManagerViewModel();
            try
            {

                DataTable dt = await _ClientDAL.getClientid(ClientId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<CustomerManagerViewModel>();
                    model = data[0];
                }

                return model;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetailClient - CustomerManagerRepository: " + ex);
                return null;
            }
        }
        public async Task<GenericViewModel<CustomerManagerViewModel>> GetPagingList(CustomerManagerViewSearchModel searchModel, int currentPage, int pageSize)
        {
            var model = new GenericViewModel<CustomerManagerViewModel>();
            try
            {
                DataTable dt = await _ClientDAL.GetPagingList(searchModel, currentPage, pageSize, ProcedureConstants.GETGetAllClient_Search);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = (from row in dt.AsEnumerable()
                                select new CustomerManagerViewModel
                                {
                                    Id = !row["Id"].Equals(DBNull.Value) ? Convert.ToInt32(row["Id"]) : 0,
                                    ClientName = !row["ClientName"].Equals(DBNull.Value) ? row["ClientName"].ToString() : "",
                                    Email = !row["Email"].Equals(DBNull.Value) ? row["Email"].ToString() : "",
                                    Phone = !row["Phone"].Equals(DBNull.Value) ? row["Phone"].ToString() : "",
                                    client_type_name = !row["ClienType"].Equals(DBNull.Value) ? row["ClienType"].ToString() : "",
                                    AgencyType_name = !row["AgencyType"].Equals(DBNull.Value) ? row["AgencyType"].ToString() : "",
                                    PermisionType_name = !row["PermisionType"].Equals(DBNull.Value) ? row["PermisionType"].ToString() : "",
                                    UserId = !row["UserId"].Equals(DBNull.Value) ? Convert.ToInt32(row["UserId"]) : 0,
                                    CreateDate_UserAgent = !row["CreateDate"].Equals(DBNull.Value) ? row["CreateDate"].ToString() : "",
                                    sum_payment = !row["SumPayment"].Equals(DBNull.Value) ? Convert.ToDouble(row["SumPayment"]) : 0,
                                    UpdateLast = !row["UpdateTime"].Equals(DBNull.Value) ? row["UpdateTime"].ToString() : "",
                                    VerifyDate = !row["CreateDate"].Equals(DBNull.Value) ? row["VerifyDate"].ToString() : "",
                                    JoinDate = Convert.ToDateTime(!row["JoinDate"].Equals(DBNull.Value) ? row["JoinDate"].ToString() : ""),
                                    UserId_name = !row["FullName"].Equals(DBNull.Value) ? row["FullName"].ToString() : "",
                                    Create_name = !row["CreateName"].Equals(DBNull.Value) ? row["CreateName"].ToString() : "",
                                    ACStatus = !row["ACStatus"].Equals(DBNull.Value) ? Convert.ToInt32(row["ACStatus"]) : 0,
                                    ClientCode = !row["ClientCode"].Equals(DBNull.Value) ? row["ClientCode"].ToString() : "",


                                }).ToList();


                    model.ListData = data;
                    model.CurrentPage = currentPage;
                    model.PageSize = pageSize;
                    model.TotalRecord = Convert.ToInt32(dt.Rows[0]["TotalRow"]);
                    model.TotalPage = (int)Math.Ceiling((double)model.TotalRecord / model.PageSize);
                    return model;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - CustomerManagerRepository: " + ex);
                return null;
            }

        }
        public int ResetStatusAc(long clientId, long Status,int type)
        {
            try
            {

                var model = _AccountClientDAL.AccountClientByClientId(clientId);
                model.Status = (byte?)Status;
                if(type != 0)
                {
                    model.ClientType = type;
                }
                _AccountClientDAL.UpdataAccountClient(model);
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ResetStatusAc - CustomerManagerRepository: " + ex);
                return 0;
            }

        }
        public async Task<AmountRemainView> GetAmountRemainOfContractByClientId(long ClientId)
        {
            var model = new AmountRemainView();
            try
            {
                double sum = 0;
                DataTable dt = await _ClientDAL.GetAmountRemainOfContractByClientId(ClientId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var Data = (from row in dt.AsEnumerable()
                                select new AmountRemainView
                                {
                                    AmountRemain = row["AmountRemain"] != DBNull.Value ? Convert.ToDouble(row["AmountRemain"]) : 0,
                                }).ToList();

                    model = Data[0];
                }
                return model;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAmountRemainOfContractByClientId - CustomerManagerRepository: " + ex);
                return null;
            }
        }

        public async Task<string> ExportDeposit(CustomerManagerViewSearchModel searchModel, string FilePath, field field)
        {
            var pathResult = string.Empty;
            try
            {
                var data = new List<CustomerManagerViewModel>();
                DataTable dt = await _ClientDAL.GetPagingList(searchModel, searchModel.PageIndex, searchModel.PageSize, ProcedureConstants.GETGetAllClient_Search);
                if (dt != null && dt.Rows.Count > 0)
                {
                    data = (from row in dt.AsEnumerable()
                            select new CustomerManagerViewModel
                            {
                                Id = !row["Id"].Equals(DBNull.Value) ? Convert.ToInt32(row["Id"]) : 0,
                                ClientName = !row["ClientName"].Equals(DBNull.Value) ? row["ClientName"].ToString() : "",
                                Email = !row["Email"].Equals(DBNull.Value) ? row["Email"].ToString() : "",
                                Phone = !row["Phone"].Equals(DBNull.Value) ? row["Phone"].ToString() : "",
                                client_type_name = !row["ClienType"].Equals(DBNull.Value) ? row["ClienType"].ToString() : "",
                                AgencyType_name = !row["AgencyType"].Equals(DBNull.Value) ? row["AgencyType"].ToString() : "",
                                PermisionType_name = !row["PermisionType"].Equals(DBNull.Value) ? row["PermisionType"].ToString() : "",
                                UserId = !row["UserId"].Equals(DBNull.Value) ? Convert.ToInt32(row["UserId"]) : 0,
                                CreateDate_UserAgent = !row["CreateDate"].Equals(DBNull.Value) ? row["CreateDate"].ToString() : "",
                                sum_payment = !row["SumPayment"].Equals(DBNull.Value) ? Convert.ToDouble(row["SumPayment"]) : 0,
                                UpdateLast = !row["UpdateTime"].Equals(DBNull.Value) ? row["UpdateTime"].ToString() : "",
                                VerifyDate = !row["CreateDate"].Equals(DBNull.Value) ? row["VerifyDate"].ToString() : "",
                                JoinDate = Convert.ToDateTime(!row["JoinDate"].Equals(DBNull.Value) ? row["JoinDate"].ToString() : ""),
                                UserId_name = !row["FullName"].Equals(DBNull.Value) ? row["FullName"].ToString() : "",
                                Create_name = !row["CreateName"].Equals(DBNull.Value) ? row["CreateName"].ToString() : "",
                                ACStatus = !row["ACStatus"].Equals(DBNull.Value) ? Convert.ToInt32(row["ACStatus"]) : 0,
                                ClientCode = !row["ClientCode"].Equals(DBNull.Value) ? row["ClientCode"].ToString() : "",

                            }).ToList();

                }
                if (data != null && data.Count > 0)
                {
                    Workbook wb = new Workbook();
                    Worksheet ws = wb.Worksheets[0];
                    ws.Name = "Danh sách khách hàng";
                    Cells cell = ws.Cells;

                    var range = ws.Cells.CreateRange(0, 0, 1, 1);
                    StyleFlag st = new StyleFlag();
                    st.All = true;
                    Style style = ws.Cells["A1"].GetStyle();

                    #region Header


                    // Set column width
                    var listfield = new List<int>();
                    var listfieldtext = new List<string>();
                    if (field.MaKH) { listfieldtext.Add("Mã khách hàng"); listfield.Add(1); }
                    if (field.TenKH) { listfieldtext.Add("Tên khách hàng"); listfield.Add(2); }
                    if (field.LienHe) { listfieldtext.Add("Liên hệ"); listfield.Add(3); }
                    if (field.DoiTuong) { listfieldtext.Add("Đối tượng"); listfield.Add(4); }
                    if (field.LoaiKH) { listfieldtext.Add("Loại khách hàng"); listfield.Add(5); }
                    if (field.NhomKH) { listfieldtext.Add("Nhóm khách hàng"); listfield.Add(6); }
                    if (field.TongTT) { listfieldtext.Add("Tổng thanh toán"); listfield.Add(7); }
                    if (field.VNPhuTrach) { listfieldtext.Add("Nhân viên phụ trách"); listfield.Add(8); }
                    if (field.NgayTao) { listfieldtext.Add("Ngày tạo"); listfield.Add(9); }
                    if (field.NgayCN) { listfieldtext.Add("Ngày cập nhật"); listfield.Add(10); }

                    if (field.NguoiTao) { listfieldtext.Add("Người tạo"); listfield.Add(11); }
                    if (field.Status) { listfieldtext.Add("Trạng thái"); listfield.Add(12); }

                    cell.SetColumnWidth(0, 8);
                    for (int i = 1; i <= listfield.Count; i++)
                    {
                        cell.SetColumnWidth(i, 40);
                    }

                    range = cell.CreateRange(0, 0, 1, listfield.Count + 1);
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

                    int Index = 1;
                    // Set header value
                    ws.Cells["A1"].PutValue("STT");
                    List<string> Cell = new List<string>() { "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R" };
                    for (int I = 0; I < listfield.Count; I++)
                    {

                        ws.Cells[Cell[I] + Index].PutValue(listfieldtext[I]);


                    }



                    #endregion

                    #region Body

                    range = cell.CreateRange(1, 0, data.Count, listfield.Count + 1);
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

                    Style numberStyle = ws.Cells[Cell[listfield.Count] + "2"].GetStyle();
                    numberStyle.Number = 3;
                    numberStyle.HorizontalAlignment = TextAlignmentType.Right;
                    numberStyle.VerticalAlignment = TextAlignmentType.Center;

                    int RowIndex = 1;

                    foreach (var item in data)
                    {
                        string ttchitiet = string.Empty;
                        var listfield2 = new List<int>();
                        listfield2.AddRange(listfield);
                        RowIndex++;
                        ws.Cells["A" + RowIndex].PutValue(RowIndex - 1);
                        ws.Cells["A" + RowIndex].SetStyle(alignCenterStyle);
                        for (int I = 0; I < listfield.Count; I++)
                        {

                            for (int f = 0; f < listfield2.Count; f++)
                            {
                                if (listfield2[f] == 1)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(item.ClientCode);
                                    listfield2.Remove(listfield2[f]);
                                    f--;
                                    break;
                                }
                                if (listfield2[f] == 2)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(item.ClientName);
                                    listfield2.Remove(listfield2[f]); f--; break;
                                }
                                if (listfield2[f] == 3)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(item.Phone + "\n" + item.Email);
                                    listfield2.Remove(listfield2[f]); f--; break;
                                }
                                if (listfield2[f] == 4)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(item.AgencyType_name);
                                    listfield2.Remove(listfield2[f]); f--; break;
                                }
                                if (listfield2[f] == 5)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(item.client_type_name);
                                    listfield2.Remove(listfield2[f]); f--; break;
                                }
                                if (listfield2[f] == 6)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(item.PermisionType_name);
                                    listfield2.Remove(listfield2[f]); f--; break;
                                }
                                if (listfield2[f] == 7)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(item.sum_payment.ToString("###,###,###"));
                                    listfield2.Remove(listfield2[f]); f--; break;
                                }
                                if (listfield2[f] == 8)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(item.UserId_name);
                                    listfield2.Remove(listfield2[f]); f--; break;
                                }
                                if (listfield2[f] == 9)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(item.JoinDate.ToString("dd/MM/yyyy HH:mm:ss"));
                                    listfield2.Remove(listfield2[f]); f--; break;
                                }
                                if (listfield2[f] == 10)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(Convert.ToDateTime(item.UpdateLast).ToString("dd/MM/yyy HH:mm:ss"));
                                    listfield2.Remove(listfield2[f]); f--; break;
                                }
                                if (listfield2[f] == 11)
                                {
                                    ws.Cells[Cell[I] + RowIndex].PutValue(item.Create_name);
                                    listfield2.Remove(listfield2[f]); f--; break;
                                }
                                if (listfield2[f] == 12)
                                {
                                    if (item.ACStatus == 0)
                                    {
                                        ws.Cells[Cell[I] + RowIndex].PutValue("Đang hoạt động");
                                        listfield2.Remove(listfield2[f]); f--; break;
                                    }
                                    else
                                    {
                                        ws.Cells[Cell[I] + RowIndex].PutValue("Ngừng hoạt động");
                                        listfield2.Remove(listfield2[f]); f--; break;
                                    }
                                }

                            }


                        }


                    }

                    #endregion
                    wb.Save(FilePath);
                    pathResult = FilePath;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ExportDeposit - CustomerManagerRepository: " + ex);
            }
            return pathResult;
        }
    }
}
