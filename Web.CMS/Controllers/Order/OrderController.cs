using Aspose.Cells;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Mongo;
using Microsoft.AspNetCore.Mvc;
using Nest;
using NuGet.Packaging.Signing;
using Repositories.IRepositories;
using System.Security.Claims;
using Utilities;
using Utilities.Contants;
using WEB.Adavigo.CMS.Service;
using WEB.CMS.Customize;
using WEB.CMS.Models.Product;

namespace WEB.CMS.Controllers
{

    public class OrderController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAllCodeRepository _allCodeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IContractPayRepository _contractPayRepository;
        private readonly IPaymentRequestRepository _paymentRequestRepository;
        private readonly ProductDetailMongoAccess _productV2DetailMongoAccess;

        public OrderController(IConfiguration configuration, IAllCodeRepository allCodeRepository, IOrderRepository orderRepository, IClientRepository clientRepository, 
            IUserRepository userRepository, IContractPayRepository contractPayRepository, IPaymentRequestRepository paymentRequestRepository)
        {
            _configuration = configuration;
            _allCodeRepository = allCodeRepository;
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _contractPayRepository = contractPayRepository;
            _paymentRequestRepository = paymentRequestRepository;
            _productV2DetailMongoAccess = new ProductDetailMongoAccess(configuration);
        }
        public IActionResult Index()
        {
            try
            {

                var serviceType = _allCodeRepository.GetListByType("SERVICE_TYPE");
                var systemtype = _allCodeRepository.GetListByType("SYSTEM_TYPE");
                var utmSource = _allCodeRepository.GetListByType("UTM_SOURCE");
                var orderStatus = _allCodeRepository.GetListByType("ORDER_STATUS");
                var PAYMENT_STATUS = _allCodeRepository.GetListByType("PAYMENT_STATUS");
                var PERMISION_TYPE = _allCodeRepository.GetListByType("PERMISION_TYPE");

                ViewBag.Order_Status = orderStatus;
                ViewBag.PAYMENT_STATUS = PAYMENT_STATUS;
                ViewBag.PERMISION_TYPE = PERMISION_TYPE;
                ViewBag.FilterOrder = new FilterOrder()
                {
                    SysTemType = systemtype,
                    Source = utmSource,
                    Type = serviceType,
                    Status = orderStatus,
                };
            }
            catch (System.Exception ex)
            {
                LogHelper.InsertLogTelegram("Index - OrderController: " + ex.ToString());
                return Content("");
            }

            return View();
        }
        public async Task<IActionResult> Search(OrderViewSearchModel searchModel, long currentPage, long pageSize)
        {

            try
            {
                ViewBag.domainImg = _configuration["DomainConfig:ImageStatic"];
                searchModel.pageSize = (int)pageSize;
                searchModel.PageIndex = (int)currentPage;
                var model = new GenericViewModel<OrderViewModel>();
                var model2 = new TotalCountSumOrder();
                model = await _orderRepository.GetList(searchModel);
                if(model != null && model.ListData != null && model.ListData.Count>0)
                {
                    foreach(var item in model.ListData)
                    {
                        item.ListProduct= await _productV2DetailMongoAccess.GetListByIds(item.ListProductId);
                    }
                }
                model2 = await _orderRepository.GetTotalCountSumOrder(searchModel);
                ViewBag.TotalValueOrder = new TotalValueOrder()
                {
                    //theo All
                    TotalAmmount = model2.Amount.ToString("N0"),
                    TotalDone = model?.ListData?.Sum(x => x.Amount).ToString("N0"),
                    TotalProductService = model2.Price.ToString("N0"),
                    TotalProfit = model2.Profit.ToString("N0")

                };
                return PartialView(model);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Search - OrderController: " + ex);
            }

            return PartialView();
        }
        public async Task<IActionResult> OrderDetail(long orderId)
        {
            try
            {
       
                if (orderId != 0)
                {
                    ViewBag.orderId = orderId;
                    var dataOrder = await _orderRepository.GetOrderDetailByOrderId(orderId);
                    if (dataOrder != null)
                    {
                     
                        if (dataOrder.CreatedDate != null)
                            ViewBag.UserCreateTime = ((DateTime)dataOrder.CreatedDate).ToString("dd/MM/yyyy HH:mm:ss");
                        if (dataOrder.UpdateLast != null)
                            ViewBag.UserUpdateTime = ((DateTime)dataOrder.UpdateLast).ToString("dd/MM/yyyy HH:mm:ss");
                        if (dataOrder.CreatedBy != null && dataOrder.CreatedBy != 0)
                        {
                            var UserCreateclient = await _userRepository.FindById((int)dataOrder.CreatedBy);
                            if (UserCreateclient != null)
                                ViewBag.UserCreateClientName = UserCreateclient.FullName;

                        }
                        if (dataOrder.UserUpdateId != null && dataOrder.UserUpdateId != 0)
                        {
                            var UserUpdateclient = await _userRepository.FindById((int)dataOrder.UserUpdateId);
                            if (UserUpdateclient != null)
                                ViewBag.UserUpdateClientName = UserUpdateclient.FullName;
                        }
                       
                        if (dataOrder.StartDate != null)
                            ViewBag.createTime = Convert.ToDateTime(dataOrder.StartDate).ToString("dd/MM/yyyy HH:mm:ss");
                        if (dataOrder.EndDate != null)
                            ViewBag.ExpriryDate = Convert.ToDateTime(dataOrder.EndDate).ToString("dd/MM/yyyy HH:mm:ss");
                        var Address = dataOrder.Address + "," + dataOrder.WardName + "," + dataOrder.DistrictName + "," + dataOrder.ProvinceName;
                        ViewBag.Address = Address.TrimStart(',').TrimEnd(',');
                        if (dataOrder.ClientId != null)
                        {
                            var UserCreateclient = await _clientRepository.GetClientDetailByClientId((long)dataOrder.ClientId);
                            if (UserCreateclient != null)
                            {
                                ViewBag.client = UserCreateclient;
                            }
                        }
                       
                    return View(dataOrder);
                    }
                 
                }
             
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Index - OrderController: " + ex.ToString());
            }

            return View();
        }
        public async Task<IActionResult> PersonInCharge(int orderId)
        {
            try
            {
                if (orderId != 0)
                {
                    var data = await _orderRepository.GetOrderDetailByOrderId(orderId);
                    if (data.SalerId != null)
                    {
                        var SalerGroup =await _userRepository.GetClientDetailAsync(data.SalerId);
                        ViewBag.Saler = SalerGroup;
                    }
                    List<User> List_SalerGroup = new List<User>();
                    if (data.SalerGroupId != null && data.SalerGroupId != "")
                    {
                        var list_SalerGroupId = Array.ConvertAll(data.SalerGroupId.ToString().Split(','), s => (s).ToString());

                        foreach (var item in list_SalerGroupId)
                        {
                            long id = Convert.ToInt32(item);
                            var SalerGroup = await _userRepository.GetClientDetailAsync(id);
                            if (SalerGroup != null)
                            {
                                var ClientName = SalerGroup.FullName.ToString();
                                List_SalerGroup.Add(SalerGroup);
                            }
                            ViewBag.SalerGroup = List_SalerGroup;
                        }
                        
                    }
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("PersonInCharge-OrderController" + ex.ToString());
                return PartialView();
            }
        }
        public async Task<IActionResult> Packages(int orderId)
        {

            try
            {
                ViewBag.domainImg = _configuration["DomainConfig:ImageStatic"];
                var list_OrderDetail =await _orderRepository.GetListOrderDetail(orderId);
                var ids= list_OrderDetail.Select(s=>s.ProductId).ToList();
                var List_product = await _productV2DetailMongoAccess.GetListByIds(string.Join(",", ids));
                ViewBag.data = List_product;
                var dataOrder = await _orderRepository.GetOrderDetailByOrderId(orderId);
                ViewBag.dataOrder = dataOrder;
                var data2 = await _contractPayRepository.GetContractPayByOrderId(orderId);
                if (data2 != null)
                {
                    ViewBag.paymentAmount = data2.Sum(s => s.AmountPay);
                }
                return PartialView(list_OrderDetail);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Packages-OrderController" + ex.ToString());
                return PartialView();
            }
        }
        public async Task<IActionResult> ContractPay(int orderId)
        {
            try
            {
                if (orderId != 0)
                {

                    var dataOrder = await _orderRepository.GetOrderDetailByOrderId(orderId);
                    if (dataOrder != null)
                    {
                        var data = await _contractPayRepository.GetContractPayByOrderId(Convert.ToInt32(dataOrder.OrderId));
                        if (data != null)
                        {
                            ViewBag.listPayment = data;
                            ViewBag.paymentAmount = data.Sum(s => s.AmountPay);
                            return PartialView(data);
                        }
                    }
                }

                return PartialView();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ContractPay - OrderController" + ex.ToString());
                return PartialView();
            }

        }
        public async Task<IActionResult> BillVAT(int orderId)
        {
            try
            {
                if (orderId != 0)
                {

                    var dataOrder = await _orderRepository.GetOrderDetailByOrderId(orderId);
                    if (dataOrder != null)
                    {
                        var data =  _paymentRequestRepository.GetListPaymentRequestByOrderId(Convert.ToInt32(dataOrder.OrderId));
                        if (data != null)
                        {
                            ViewBag.listPayment = data;
                            ViewBag.paymentAmount = data.Sum(s => s.Amount);
                            return PartialView(data);
                        }
                    }
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("BillVAT-OrderController" + ex.ToString());
                return PartialView();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ChangeOrderSaler(long? order_id, int saleid, string OrderNo)
        {

            try
            {
                var model = new LogActionModel();
                model.Type = (int)AttachmentType.OrderDetail;
                model.LogId = (long)order_id;
                if (order_id == null || order_id <= 0)
                {
                    return Ok(new
                    {
                        status = (int)ResponseType.FAILED,
                        msg = "Dữ liệu gửi lên không chính xác, vui lòng kiểm tra lại"
                    });
                }
                int _UserId = 0;
                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                if (saleid != 0) _UserId = _UserId = saleid;
                var order = new Entities.Models.Order();
                order.OrderId = (long)order_id;
                order.UserId = _UserId;
                order.UserUpdateId = _UserId;
                var success = await _orderRepository.UpdateOrder(order);
              
                return Ok(new
                {
                    status = (int)ResponseType.SUCCESS,
                    msg = "Đổi điều hành viên thành công"
                });

            }
            catch
            {
                return Ok(new
                {
                    status = (int)ResponseType.ERROR,
                    msg = "Đổi điều hành viên thất bại, vui lòng liên hệ IT"
                });
            }
        }

    }
}
