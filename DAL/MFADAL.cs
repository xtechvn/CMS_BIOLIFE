using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DAL
{
    public class MFADAL :GenericService<Mfauser>
    {
        private static DbWorker _DbWorker;
        public MFADAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }
        public async Task<Mfauser> get_MFA_DetailByUserID(long user_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    // _DbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.ProductMapId == id);
                    var detail = await  _DbContext.Mfausers.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == user_id);
                    if (detail != null)
                    {
                        return detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("get_MFA_DetailByClientID - MFADAL: " + ex);
                return null;
            }
        }
    }
}
