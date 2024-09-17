using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WEB.CMS.ViewComponents
{
    public class RevenuTodayViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            var data = 0;
            //var data = _OrderRepository.SummaryRevenuToday();
            return View(data);
        }
    }
}
