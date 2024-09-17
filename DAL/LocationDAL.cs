using DAL.Generic;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class ProvinceDAL : GenericService<Province>
    {
        public ProvinceDAL(string connection) : base(connection)
        { 
        }
        public async Task<List<Province>> GetProvincesList()
        {
            try
            {
                return await GetAllAsync();
            } catch(Exception e)
            {
                string msg = "GetProvincesList - ProvinceDAL: " + e;
                return null;
            }
        }
        public async Task<string> AddNewProvince(Province newProvince)
        {
            try
            {
                await CreateAsync(newProvince);
                return newProvince.Id.ToString();
            }
            catch (Exception e)
            {
                string msg = "AddNewProvince - ProvinceDAL: " + e;
                return null;
            }
        }
        public int CheckProvinceExists(Province newProvince, bool for_update = false)
        {
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                var exists = true;
                if (for_update)
                {
                     exists = _DbContext.Provinces.AsNoTracking().Any(x => x.Name.Trim() == newProvince.Name.Trim() && x.Status==newProvince.Status);
                }
                else
                {
                     exists = _DbContext.Provinces.AsNoTracking().Any(x => x.Name.Trim() == newProvince.Name.Trim());
                }
                if (exists)
                {
                    return 1;
                }
                else return 0;
            }
            catch (Exception e)
            {
                string msg = "CheckProvinceExists - ProvinceDAL: " + e;
                return -1;
            }
        }
        
        public async Task<string> UpdateProvince(Province updatedProvince)
        {
            try
            {
                await UpdateAsync(updatedProvince);
                string msg = "Updated-" + updatedProvince.ProvinceId;
                return msg;
            }
            catch (Exception e)
            {
                string msg = "UpdateProvince - ProvinceDAL: " + e;
                return msg;
            }
        }
        public async Task<Province> GetLastestProvince()
        {
            Province lastest_item=null;
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                lastest_item = _DbContext.Provinces.AsNoTracking().OrderByDescending(x=>x.ProvinceId).FirstOrDefault();
            }
            catch (Exception e)
            {
                string msg = "GetLastestProvince - ProvinceDAL: " + e;
            }
            return lastest_item;
        }
        public async Task<Province> GetProvinceById(long province_id)
        {
            Province lastest_item = null;
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                lastest_item = await _DbContext.Provinces.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==province_id);
            }
            catch (Exception e)
            {
                string msg = "GetProvinceById - ProvinceDAL: " + e;
            }
            return lastest_item;
        }
    }
    public class DistrictDAL : GenericService<District>
    {
        public DistrictDAL(string connection) : base(connection)
        {
        }
        public async Task<List<District>>  GetDistrictsList()
        {
            try
            {
                return await GetAllAsync();
            }
            catch (Exception e)
            {
                string msg = "GetDistrictsList - DistrictDAL: " + e;
                return null;
            }
        }
        public async Task<List<District>> GetDistrictsListByProvinceID(string provinceID)
        {
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                var list = await _DbContext.Districts.AsNoTracking().Where(x=>x.ProvinceId== provinceID).ToListAsync();
                return list;
            }
            catch (Exception e)
            {
                string msg = "GetDistrictsListByProvinceID - DistrictDAL: " + e;
                return null;
            }
        }
        public async Task<string> AddNewDistrict(District newDistrict)
        {
            try
            {
                await CreateAsync(newDistrict);
                return newDistrict.Id.ToString();
            }
            catch (Exception e)
            {
                string msg = "AddNewDistrict - DistrictDAL: " + e;
                return null;
            }
        }
        public async Task<string> UpdateDistrict(District updatedDistrict)
        {
            try
            {
                await UpdateAsync(updatedDistrict);
                string msg = "Updated-" + updatedDistrict.DistrictId;
                return msg;
            }
            catch (Exception e)
            {
                string msg = "UpdateDistrict - DistrictDAL: " + e;
                return msg;
            }
        }
        public int CheckDistrictExists(District newDistrict, bool for_update = false)
        {
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                var exists = true;
                if (for_update)
                {
                     exists = _DbContext.Districts.AsNoTracking().Any(x => x.Name.Trim() == newDistrict.Name.Trim() && x.Type.Trim() == newDistrict.Type.Trim() && x.ProvinceId.Trim() == newDistrict.ProvinceId.Trim() && x.Status == newDistrict.Status);

                }
                else
                {
                    exists = _DbContext.Districts.AsNoTracking().Any(x => x.Name.Trim() == newDistrict.Name.Trim() && x.Type.Trim() == newDistrict.Type.Trim() && x.ProvinceId.Trim() == newDistrict.ProvinceId.Trim());

                }
                if (exists) return 1;
                else return 0;
            }
            catch (Exception e)
            {
                string msg = "CheckDistrictExists - DistrictDAL: " + e;
                return -1;
            }
        }
        public async Task<District> GetLastestDistrict()
        {
            District lastest_item = null;
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                lastest_item = _DbContext.Districts.AsNoTracking().OrderByDescending(x => x.DistrictId).FirstOrDefault();
            }
            catch (Exception e)
            {
                string msg = "GetLastestProvince - DistrictDAL: " + e;
            }
            return lastest_item;
        }

    }
    public class WardDAL : GenericService<Ward>
    {
        public WardDAL(string connection) : base(connection)
        {
        }
        public async Task<List<Ward>>  GetWardsList()
        {
            try
            {
                return  await GetAllAsync();
            }
            catch (Exception e)
            {
                string msg = "GetDistrictsList - WardDAL: " + e;
                return null;
            }
        }
        public async Task<List<Ward>> GetWardListByDistrictID(string districtID)
        {
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                var list = await _DbContext.Wards.AsNoTracking().Where(x => x.DistrictId == districtID).ToListAsync();
                return list;
            }
            catch (Exception e)
            {
                string msg = "GetDistrictsListByProvinceID - WardDAL: " + e;
                return null;
            }
        }
        public async Task<string> AddNewWard(Ward newWard)
        {
            try
            {
                await CreateAsync(newWard);
                return newWard.Id.ToString();
            }
            catch (Exception e)
            {
                string msg = "AddNewProvince - WardDAL: " + e;
                return null;
            }
        }
        public async Task<string> UpdateWard(Ward updatedWard)
        {
            try
            {
                await UpdateAsync(updatedWard);
                string msg = "Updated-" + updatedWard.WardId;
                return msg;
            }
            catch (Exception e)
            {
                string msg = "UpdateWard - WardDAL: " + e;
                return msg;
            }
        }
        public int CheckWardExists(Ward newWard, bool for_update = false)
        {
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                var exists = true;
                if (for_update)
                {
                    exists = _DbContext.Wards.AsNoTracking().Any(x => x.Name.Trim() == newWard.Name.Trim() && x.Type.Trim() == newWard.Type.Trim() && x.DistrictId.Trim() == newWard.DistrictId.Trim() && x.Status==newWard.Status);

                }
                else
                {
                    exists = _DbContext.Wards.AsNoTracking().Any(x => x.Name.Trim() == newWard.Name.Trim() && x.Type.Trim() == newWard.Type.Trim() && x.DistrictId.Trim() == newWard.DistrictId.Trim());
                }
                if (exists) return 1;
                else return 0;
            }
            catch (Exception e)
            {
                string msg = "CheckWardExists - WardDAL: " + e;
                return -1;
            }
        }
        public async Task<Ward> GetLastestWard()
        {
            Ward lastest_item = null;
            try
            {
                var _DbContext = new EntityDataContext(_connection);
                lastest_item = _DbContext.Wards.AsNoTracking().OrderByDescending(x => x.WardId).FirstOrDefault();
            }
            catch (Exception e)
            {
                string msg = "GetLastestProvince - WardDAL: " + e;
            }
            return lastest_item;
        }
    }
}
