using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class LabelDAL : GenericService<Label>
    {
        private static DbWorker _DbWorker;
        public LabelDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public async Task<Label> getLabelDetailById(int label_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Labels.AsNoTracking().FirstOrDefaultAsync(s => s.Id == label_id);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("FindByLabelId - LabelDAL: " + ex);
                return null;
            }
        }

        public async Task<List<Label>> getLabelActive()
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Labels.AsNoTracking().Where(n => n.Status == (int)Status.HOAT_DONG).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getLabelActive - LabelDAL: " + ex);
                return null;
            }
        }

    }
}
