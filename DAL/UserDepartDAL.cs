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
    public class UserDepartDAL : GenericService<UserDepart>
    {
        public UserDepartDAL(string connection) : base(connection)
        {
        }

        public async Task<List<UserDepart>> GetByIds(List<int?> ids)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.UserDeparts.Where(s => ids.Contains(s.DepartmentId)).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByIds - UserDepartDAL: " + ex);
                return null;
            }
        }
    }
}