using Microsoft.AspNetCore.Mvc;

namespace WEB.CMS.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AjaxAuthenticate()
        {
            return View();
        }
    }
}
