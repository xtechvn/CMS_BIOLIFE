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
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly ProvinceDAL _provinceDAL;
        public ProvinceRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _provinceDAL = new ProvinceDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }


        public async Task<List<Province>> GetProvincesList()
        {
            try
            {
                return await _provinceDAL.GetProvincesList();
            } catch(Exception ex)
            {
                string msg = "GetProvincesList - ProvinceRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }
        public async Task<string> AddNewProvince(Province newItem)
        {
            try
            {
                string msg= await _provinceDAL.AddNewProvince(newItem);
                return msg;
            }
            catch (Exception ex)
            {
                string msg = "AddNewProvince - ProvinceRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }
        public async Task<string> UpdateProvince(Province updatedItem)
        {
            try
            {
                string msg = await _provinceDAL.UpdateProvince(updatedItem);
                return msg;
            }
            catch (Exception ex)
            {
                string msg = "UpdateProvince - ProvinceRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }
        public int CheckProvinceExists(Province newProvince, bool for_update = false)
        {
            try
            {
                var province = _provinceDAL.CheckProvinceExists(newProvince, for_update);
                return province;
            }
            catch (Exception ex)
            {
                string msg = "CheckProvinceExists - ProvinceRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return -2;
            }
        }
        public async Task<Province> GetLastestProvinceWithIDAsync()
        {
            try
            {
                var province = await _provinceDAL.GetLastestProvince();
                return province;
            }
            catch (Exception ex)
            {
                string msg = "GetLastestProvinceWithIDAsync - ProvinceRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }
        public async Task<Province> GetProvinceById(long province_id)
        {
            try
            {
                var province = await _provinceDAL.GetProvinceById(province_id);
                return province;
            }
            catch (Exception ex)
            {
                string msg = "GetProvinceById - ProvinceRepository: " + ex;
                return null;
            }
        }
    }
    public class DistrictRepository : IDistrictRepository
    {
        private readonly DistrictDAL _districtDAL;
        public DistrictRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _districtDAL = new DistrictDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<string> AddNewDistrict(District newItem)
        {
            try
            {
                string msg = await _districtDAL.AddNewDistrict(newItem);
                return msg;
            }
            catch (Exception ex)
            {
                string msg = "AddNewDistrict - DistrictRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }

        public int CheckDistrictExists(District newDistrict, bool for_update = false)
        {
            try
            {
                var district = _districtDAL.CheckDistrictExists(newDistrict, for_update);
                return district;
            }
            catch (Exception ex)
            {
                string msg = "CheckDistrictExists - DistrictRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return -2;
            }
        }

        public async Task<List<District>> GetDistrictList()
        {
            try
            {
                return await _districtDAL.GetDistrictsList();
            }
            catch (Exception ex)
            {
                string msg = "GetDistrictList - DistrictRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }

        public async Task<string> UpdateDistrict(District updatedItem)
        {
            try
            {
                string msg = await _districtDAL.UpdateDistrict(updatedItem);
                return msg;
            }
            catch (Exception ex)
            {
                string msg = "UpdateDistrict - DistrictRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }
        public async Task<District> GetLastestDistrictWithIDAsync()
        {
            try
            {
                var province = await _districtDAL.GetLastestDistrict();
                return province;
            }
            catch (Exception ex)
            {
                string msg = "GetLastestDistrictWithIDAsync - ProvinceRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }
    }
    public class WardRepository : IWardRepository
    {
        private readonly WardDAL _wardDAL;
        public WardRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _wardDAL = new WardDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<string> AddNewWard(Ward newItem)
        {
            try
            {
                string msg = await _wardDAL.AddNewWard(newItem);
                return msg;
            }
            catch (Exception ex)
            {
                string msg = "AddNewWard - WardRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }

        public int CheckWardExists(Ward newWard, bool for_update = false)
        {
            try
            {
                var ward = _wardDAL.CheckWardExists(newWard, for_update);
                return ward;
            }
            catch (Exception ex)
            {
                string msg = "CheckWardExists - WardRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return -2;
            }
        }

        public async Task<List<Ward>> GetWardList()
        {
            try
            {
                return await _wardDAL.GetWardsList();
            }
            catch (Exception ex)
            {
                string msg = "GetWardList - WardRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }

        public async Task<string> UpdateWard(Ward updatedItem)
        {
            try
            {
                string msg = await _wardDAL.UpdateWard(updatedItem);
                return msg;
            }
            catch (Exception ex)
            {
                string msg = "UpdateWard - WardRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }
        public async Task<Ward> GetLastestWardWithIDAsync()
        {
            try
            {
                var ward = await _wardDAL.GetLastestWard();
                return ward;
            }
            catch (Exception ex)
            {
                string msg = "GetLastestWardWithIDAsync - ProvinceRepository: " + ex;
                //LogHelper.InsertLogTelegram(msg);
                return null;
            }
        }
    }
}
