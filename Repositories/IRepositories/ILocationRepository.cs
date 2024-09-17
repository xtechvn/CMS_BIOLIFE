using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IProvinceRepository
    {
        public Task<List<Province>> GetProvincesList();
        public Task<string> AddNewProvince(Province newItem);
        public Task<string> UpdateProvince(Province updatedItem);
        public int CheckProvinceExists(Province newProvince, bool for_update=false);
        public Task<Province> GetLastestProvinceWithIDAsync();
        public Task<Province> GetProvinceById(long province_id);
    }
    public interface IDistrictRepository
    {
        public Task<List<District>>  GetDistrictList();
        public Task<string> AddNewDistrict(District newItem);

        public Task<string> UpdateDistrict(District updatedItem);
        public int CheckDistrictExists(District newDistrict, bool for_update = false);
        public Task<District> GetLastestDistrictWithIDAsync();

    }
    public interface IWardRepository
    {
        public Task<List<Ward>> GetWardList();
        public Task<string> AddNewWard(Ward newItem);

        public Task<string> UpdateWard(Ward updatedItem);
        public int CheckWardExists(Ward newWard, bool for_update = false);
        public Task<Ward> GetLastestWardWithIDAsync();
    }
}
