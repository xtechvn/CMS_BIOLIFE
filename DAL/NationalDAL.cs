using DAL.Generic;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class NationalDAL : GenericService<National>
    {
        public NationalDAL(string connection) : base(connection)
        {
        }

        public async Task<National> GetNationalById(long id)
        {
            National lastest_item = null;
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                lastest_item = await _DbContext.Nationals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception e)
            {
                string msg = "GetNationalById - NationalDAL: " + e;
            }
            return lastest_item;
        }
    }
}
