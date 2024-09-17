using DAL.StoreProcedure;
using Entities.ConfigModels;
using Entities.ViewModels.DashBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Data;
using System.Linq;
using System.Security.Claims;
using Utilities;
using WEB.CMS.Customize;

namespace WEB.CMS.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardRepository _DashboardRepository;

        public HomeController(ILogger<HomeController> logger, IDashboardRepository dashboardRepository)
        {
            _logger = logger;
            _DashboardRepository = dashboardRepository;
        }

        public IActionResult Index()
        {
            try
            {
                var _UserId = "";
                if (HttpContext.User.FindFirst(ClaimTypes.Name) == null)
                {
                    _UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                }

                var historyDataTable = _DashboardRepository.GetOrderDashboard();

                var histories = historyDataTable.AsEnumerable().Select(s => new HistoryViewModel
                {
                    UserVerify = s["UserVerify"].ToString(),
                    OrderNo = s["OrderNo"].ToString(),
                    OrderId = long.Parse(s["OrderId"].ToString()),
                    OrderStatus = s["OrderStatus"].ToString(),
                    UserCreated = s["UserCreated"].ToString(),
                    UpdateLast = string.IsNullOrEmpty(s["UpdateLast"].ToString()) ? (DateTime?)null : DateTime.Parse(s["UpdateLast"].ToString()),
                    CreatedDate = string.IsNullOrEmpty(s["CreatedDate"].ToString()) ? (DateTime?)null : DateTime.Parse(s["CreatedDate"].ToString())
                }).AsEnumerable();

                ViewBag.UserId = _UserId;
                ViewBag.Histories = histories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                LogHelper.InsertLogTelegram("Index - HomeController: " + ex);
            }
            return View();
        }

        public IActionResult DataMonitor()
        {
            return RedirectToAction("Index", "Error");
        }

        public IActionResult ExecuteQuery(string dataQuery)
        {

            return RedirectToAction("Index", "Error");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Error()
        {
            ViewBag.UserName = "";
            return View();
        }
    }
    
}
