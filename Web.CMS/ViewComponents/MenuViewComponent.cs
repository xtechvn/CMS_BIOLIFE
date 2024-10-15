using Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities;
using WEB.Adavigo.CMS.Service;
using WEB.CMS.Customize;

namespace WEB.CMS.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuRepository _MenuRepository;
        private ManagementUser _ManagementUser;

        public MenuViewComponent(IMenuRepository menuRepository, ManagementUser managementUser)
        {
            _MenuRepository = menuRepository;
            _ManagementUser = managementUser;
           
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<int> menu_ids = new List<int>();
            var current_user = _ManagementUser.GetCurrentUser();
            if (current_user != null && current_user.Permissions != null && current_user.Permissions.Any())
            {
                menu_ids = current_user.Permissions.Where(s => s.PermissionId == (int)PermissionType.TRUY_CAP).Select(s => s.MenuId);
          
            }

            var menus = await _MenuRepository.GetMenuPermissionOfUser(menu_ids);
            menus = menus.Where(x => x.Status == 0).ToList();
            var path = (string)HttpContext.Request.Path;
            var actived = menus.FirstOrDefault(s => path.ToLower() == (s.Link ?? string.Empty).ToLower());
            var menu_id = actived != null ? actived.Id : -1;
            var parent_id = GetRootParentId(menus, menu_id);

            ViewBag.MenuId = menu_id;
            ViewBag.ParentId = parent_id;
            ViewBag.Menu = menus;
            return View();
        }

        private int GetRootParentId(IEnumerable<Menu> datas, int child_id)
        {
            var model = datas.FirstOrDefault(s => s.Id == child_id);
            if (model != null)
            {
                if (model.ParentId <= 0)
                {
                    return model.Id;
                }
                else
                {
                    return GetRootParentId(datas, model.ParentId.Value);
                }
            }
            else
            {
                return -1;
            }
        }
    }
}
