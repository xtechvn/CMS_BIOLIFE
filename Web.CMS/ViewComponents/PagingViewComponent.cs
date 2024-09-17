using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB.CMS.ViewComponents
{
    public class PagingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Paging pageModel)
        {
            return View(pageModel);
        }
    }
}
