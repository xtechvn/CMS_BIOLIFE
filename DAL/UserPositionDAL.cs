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
    public class UserPositionDAL : GenericService<UserPosition>
    {
        public UserPositionDAL(string connection) : base(connection)
        {
        }

        public async Task<UserPosition> GetById(int id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.UserPositions.FirstOrDefaultAsync(s => s.Id == id);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetById - UserPositionDAL: " + ex);
                return null;
            }
        }
    }
}