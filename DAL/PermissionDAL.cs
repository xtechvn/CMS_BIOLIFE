using DAL.Generic;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DAL
{
    public class PermissionDAL : GenericService<Permission>
    {
        public PermissionDAL(string connection) : base(connection)
        {
        }

        public List<PermissionModel> GetListPaging(string permissionName, int permissionId, out int totalRecord, int currentPage = 1, int pageSize = 10)
        {
            totalRecord = 0;
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var datalist = _DbContext.Permissions.AsQueryable();
                    if (!string.IsNullOrEmpty(permissionName))
                    {
                        datalist = datalist.Where(s => s.Name.Contains(permissionName));
                    }
                    if (permissionId > 0)
                    {
                        datalist = datalist.Where(s => s.Id == permissionId);
                    }
                    totalRecord = datalist.Count();

                    return (from p in datalist.AsEnumerable()
                            select new PermissionModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                SortOrder = p.SortOrder,
                                Status = p.Status,
                            }).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListPaging - PermissionDAL: " + ex);
                return null;
            }
        }
    }
}
