using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly CommonDAL _CommonDAL;

        public CommonRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _CommonDAL = new CommonDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<List<Province>> GetProvinceList()
        {
            return await _CommonDAL.GetProvinceList();
        }
        public async Task<List<District>> GetDistrictList(string id)
        {
            var ProvinceDetail =await _CommonDAL.GetProvinceDetail(Convert.ToInt32(id));
            return await _CommonDAL.District(ProvinceDetail.ProvinceId);
        }

        public async Task<Supplier> GetSupplierById(int id)
        {
            return await _CommonDAL.GetSupplierById(id);
        }

        public async Task<List<District>> GetDistrictListByProvinceId(string provinceId)
        {
            return await _CommonDAL.GetDistrictListByProvinceId(provinceId);
        }

        public async Task<List<Ward>> GetWardListByDistrictId(string districtId)
        {
            return await _CommonDAL.GetWardListByDistrictId(districtId);
        }

        public async Task<List<AllCode>> GetAllCodeByType(string type)
        {
            return await _CommonDAL.GetAllCodeListByType(type);
        }

        public List<AttachFile> GetAttachFilesByDataIdAndType(long dataId, int type)
        {
            try
            {
                var dataTable = _CommonDAL.GetAttachFilesByDataIdAndType(dataId, type);
                return dataTable.ToList<AttachFile>();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<National>> GetNationalList()
        {
            return await _CommonDAL.GetNationalList();
        }
    }
}
