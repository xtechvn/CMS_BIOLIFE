using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class NationalRepository : INationalRepository
    {
        private readonly NationalDAL _nationalDAL;
        public NationalRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _nationalDAL = new NationalDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }


      
        public async Task<National> GetNationalById(long id)
        {
            try
            {
                var detail = await _nationalDAL.GetNationalById(id);
                return detail;
            }
            catch (Exception ex)
            {
                string msg = "GetNationalById - NationalRepository: " + ex;
                return null;
            }
        }
    }
    
}
