using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WEB.CMS.Models;

namespace WEB.CMS.ViewComponents
{
    public class TopBarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var _UserName = string.Empty;
            var _UserId = string.Empty;
            try
            {

                if (HttpContext.User.FindFirst(ClaimTypes.Name) != null)
                {
                    _UserName = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                    _UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                }

            }
            catch
            {

            }

            ViewBag.UserId = _UserId;
            ViewBag.UserName = _UserName;
            return View();
        }
    }
}
