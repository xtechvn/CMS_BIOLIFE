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
    public class PositionDAL : GenericService<Position>
    {
        public PositionDAL(string connection) : base(connection)
        {
        }

        public List<Position> GetListAllData()
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.Set<Position>().ToList();
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListAllData - PositionDAL. " + ex);
                return null;
            }
        }

        public async Task<Position> GetByPositionName(string postionName)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var detail = _DbContext.Set<Position>().FirstOrDefaultAsync(n => n.PositionName.ToLower().Equals(postionName.ToLower()));
                    if (detail != null)
                    {
                        return await detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByPositionName - PositionDAL: " + ex);
                return null;
            }
        }

    }
}
