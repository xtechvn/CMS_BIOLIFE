using Caching.RedisWorker;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;
using WEB.CMS.Customize;

namespace CMS.Controllers
{
    [CustomAuthorize]
    public class LocationController : Controller
    {
        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IWardRepository _wardRepository;
        private readonly RedisConn _RedisService;
        private readonly IConfiguration _Configuration;

        public LocationController(IConfiguration configuration, IProvinceRepository provinceRepository, IDistrictRepository districtRepository, IWardRepository wardRepository, RedisConn redisService)
        {
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _RedisService = redisService;
            _Configuration = configuration;
            _RedisService.Connect();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LoadProvince()
        {
            int stt_code = -1;
            string msg = "Error On Excution";
            List<Province> data = null;
            try
            {
                data = await _provinceRepository.GetProvincesList();
                stt_code = 1;
                msg = "Success";
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("LoadProvince - LocationController: " + ex.ToString());

            }
            return Ok(new
            {
                stt_code = stt_code,
                msg = msg,
                data =  data
            });
        }
        [HttpGet]
        public async Task<IActionResult> LoadDistrict()
        {
            int stt_code = -1;
            string msg = "Error On Excution";
            List<District> data = null;
            try
            {
                data = await _districtRepository.GetDistrictList();
                stt_code = 1;
                msg = "Success";
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("LoadDistrict - LocationController: " + ex.ToString());

            }
            return Ok(new
            {
                stt_code = stt_code,
                msg = msg,
                data = data
            });
        }
        [HttpGet]
        public async Task<IActionResult> LoadWard()
        {
            int stt_code = -1;
            string msg = "Error On Excution";
            List<Ward> data = null;
            try
            {
                data = await _wardRepository.GetWardList();
                stt_code = 1;
                msg = "Success";
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("LoadWard - LocationController: " + ex.ToString());

            }
            return Ok(new
            {
                stt_code = stt_code,
                msg = msg,
                data = data
            });
        }
        //Thêm mới Location
        [HttpPost]
        public async Task<IActionResult> Add(string location_type, string location_data_json)
        {
            string msg = "";
            int stt_code = 0;
            dynamic data = null;
            try
            {
                if (location_type == null || location_type == "" || location_type.Length < 1 || location_data_json == null || location_data_json == "")
                {
                    stt_code = -1;
                    msg = "Dữ liệu gửi lên không chính xác, vui lòng kiểm tra lại";
                    return new JsonResult(new
                    {
                        stt_code = stt_code,
                        msg = msg
                    });
                }
                switch (location_type)
                {
                    case "p":
                        {
                            Province p = JsonConvert.DeserializeObject<Province>(location_data_json);
                            if (p.Name == "" || p.Name == null || p.Type == null || p.Type == "")
                            {
                                stt_code = -1;
                                msg = "Dữ liệu gửi lên không chính xác, vui lòng kiểm tra lại";
                            }
                            else
                            {
                                var exists = _provinceRepository.CheckProvinceExists(p);
                                switch (exists)
                                {
                                    case 0:
                                        {
                                            var lastest_province = await _provinceRepository.GetLastestProvinceWithIDAsync();
                                            p.ProvinceId = Convert.ToInt32(lastest_province.ProvinceId) > 0 ? (Convert.ToInt32(lastest_province.ProvinceId) + 1).ToString() : p.ProvinceId;
                                            string result = await _provinceRepository.AddNewProvince(p);
                                            if (result != null)
                                            {
                                                p.Id = Convert.ToInt32(result);
                                                msg = "Thêm mới " + p.Type + " " + p.Name + " thành công.";
                                                stt_code = 1;
                                                
                                                data = p;
                                            }
                                            else
                                            {
                                                msg = "Thêm mới " + p.Type + " " + p.Name + " thất bại.";
                                                stt_code = -1;
                                            }
                                        }
                                        break;
                                    case 1:
                                        {

                                            stt_code = -1;
                                            msg = p.Type + " " + p.Name + " đã tồn tại, không thể thêm mới.";
                                        }
                                        break;
                                    default:
                                        {
                                            stt_code = -1;
                                            msg = "Có lỗi trong quá trình xử lý, vui lòng liên hệ với bộ phận IT.";
                                        }
                                        break;
                                }

                            }
                        }
                        break;
                    case "d":
                        {
                            District d = JsonConvert.DeserializeObject<District>(location_data_json);

                            if (d.Name == "" || d.Name == null || d.Type == null || d.Type == "" || d.ProvinceId == null || d.ProvinceId == "")
                            {
                                stt_code = -1;
                                msg = "Dữ liệu gửi lên không chính xác, vui lòng kiểm tra lại";
                            }
                            else
                            {
                                var exists = _districtRepository.CheckDistrictExists(d);
                                switch (exists)
                                {
                                    case 0:
                                        {
                                            var lastest_district = await _districtRepository.GetLastestDistrictWithIDAsync();
                                            d.DistrictId = Convert.ToInt32(lastest_district.DistrictId) > 0 ? (Convert.ToInt32(lastest_district.DistrictId) + 1).ToString() : d.DistrictId;
                                            string result = await _districtRepository.AddNewDistrict(d);
                                            if (result != null)
                                            {
                                                d.Id = Convert.ToInt32(result);
                                                msg = "Thêm mới " + d.Type + " " + d.Name + " thành công.";
                                                stt_code = 1;
                                                
                                                data = d;
                                            }
                                            else
                                            {
                                                msg = "Thêm mới " + d.Type + " " + d.Name + " thất bại.";
                                                stt_code = -1;

                                            }
                                        }
                                        break;
                                    case 1:
                                        {

                                            stt_code = -1;
                                            msg = d.Type + " " + d.Name + " đã tồn tại, không thể thêm mới.";
                                        }
                                        break;
                                    default:
                                        {
                                            stt_code = -1;
                                            msg = "Có lỗi trong quá trình xử lý, vui lòng liên hệ với bộ phận IT.";
                                        }
                                        break;
                                }

                            }

                        }
                        break;
                    case "w":
                        {
                            Ward w = JsonConvert.DeserializeObject<Ward>(location_data_json);
                            if (w.Name == "" || w.Name == null || w.Type == null || w.Type == "" || w.DistrictId == null || w.DistrictId == "")
                            {
                                stt_code = -1;
                                msg = "Dữ liệu gửi lên không chính xác, vui lòng kiểm tra lại";
                            }
                            else
                            {
                                var exists = _wardRepository.CheckWardExists(w);
                                switch (exists)
                                {
                                    case 0:
                                        {
                                            var lastest_ward = await _wardRepository.GetLastestWardWithIDAsync();
                                            w.DistrictId = Convert.ToInt32(lastest_ward.WardId) > 0 ? (Convert.ToInt32(lastest_ward.WardId) + 1).ToString() : w.DistrictId;
                                            string result = await _wardRepository.AddNewWard(w);
                                            if (result != null)
                                            {
                                                w.Id = Convert.ToInt32(result);
                                                msg = "Thêm mới " + w.Type + " " + w.Name + " thành công.";
                                                stt_code = 1;
                                               
                                                data = w;
                                            }
                                            else
                                            {
                                                msg = "Thêm mới " + w.Type + " " + w.Name + " thất bại.";
                                                stt_code = -1;

                                            }
                                        }
                                        break;
                                    case 1:
                                        {

                                            stt_code = -1;
                                            msg = w.Type + " " + w.Name + " đã tồn tại, không thể thêm mới.";
                                        }
                                        break;
                                    default:
                                        {
                                            stt_code = -1;
                                            msg = "Có lỗi trong quá trình xử lý, vui lòng liên hệ với bộ phận IT.";
                                        }
                                        break;
                                }

                            }

                        }
                        break;
                    default: break;
                }
                return Ok(new
                {
                    stt_code = stt_code,
                    msg = msg,
                    data = data
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("AddNewLocation - LocationController with '"+ location_data_json + "' - location_type:" + location_type + " : \n" + ex.ToString());

                return  Ok(new
                {
                    stt_code = -2,
                    msg = "Error: On Excution"
                });

            }
        }
        //Update Location:
        [HttpPost]
        public async Task<IActionResult> Update(string location_type, string location_data_json)
        {

            string msg = "";
            int stt_code = 0;
            dynamic data = null;
            try
            {
                if (location_type == null || location_type == "" || location_type.Length < 1 || location_data_json == null || location_data_json == "")
                {
                    stt_code = -1;
                    msg = "Dữ liệu gửi lên không chính xác, vui lòng kiểm tra lại";
                    return Ok(new { stt_code = stt_code, msg = msg });
                }
                switch (location_type)
                {
                    case "p":
                        {
                            Province p = JsonConvert.DeserializeObject<Province>(location_data_json);
                            if (p.Name == "" || p.Name == null || p.Type == null || p.Type == "" || p.Id < 1)
                            {
                                stt_code = -1;
                                msg = "Dữ liệu gửi lên không chính xác, vui lòng kiểm tra lại";
                            }
                            else
                            {
                                var exists = _provinceRepository.CheckProvinceExists(p,true);
                                switch (exists)
                                {
                                    case 0:
                                        {
                                            string result = await _provinceRepository.UpdateProvince(p);
                                            if (result != null)
                                            {
                                                msg = "Province Updated: " + result;
                                                int db_index = Convert.ToInt32(_Configuration["Redis:Database:db_common"]) > -1 ? Convert.ToInt32(_Configuration["Redis:Database:db_common"]) : 0;
                                                _RedisService.clear("PROVINCE", db_index);
                                                stt_code = 1;
                                               
                                                data = p;
                                            }
                                            else
                                            {
                                                stt_code = -1;
                                                msg = "Cannot Update Province: " + p.Name;
                                            }

                                        } break;
                                    case 1:
                                        {
                                            stt_code = -1;
                                            msg = "Tỉnh / Thành phố với tên '" + p.Name + "' đã tồn tại, hoặc không có thay đổi nào trong thông tin cập nhật mới.";
                                        }
                                        break;
                                    default:
                                        {
                                            stt_code = -1;
                                            msg = "Có lỗi trong quá trình xử lý, vui lòng liên hệ với bộ phận IT.";
                                        }
                                        break;
                                }
                               
                            }
                        }
                        break;
                    case "d":
                        {
                            District d = JsonConvert.DeserializeObject<District>(location_data_json);
                            if (d.Id < 1 || d.Name == "" || d.Name == null || d.Type == null || d.Type == "" || d.ProvinceId == null || d.ProvinceId == "")
                            {
                                stt_code = -1;
                                msg = "Dữ liệu gửi lên không chính xác, vui lòng kiểm tra lại";
                            }
                            else
                            {
                                var exists = _districtRepository.CheckDistrictExists(d,true);
                                switch (exists)
                                {
                                    case 0:
                                        {
                                            string result = await _districtRepository.UpdateDistrict(d);
                                            if (result != null)
                                            {
                                                int db_index = Convert.ToInt32(_Configuration["Redis:Database:db_common"]) > -1 ? Convert.ToInt32(_Configuration["Redis:Database:db_common"]) : 0;
                                                _RedisService.clear("DISTRICT_" + d.Id, db_index);
                                                stt_code = 1;
                                                msg = "District Updated: " + result;
                                                
                                                data = d;

                                            }
                                            else
                                            {
                                                stt_code = -1;
                                                msg = "Cannot Update District: " + d.Name;
                                            }
                                        } break;
                                    case 1:
                                        {

                                            stt_code = -1;
                                            msg =d.Type+ " với tên '" + d.Name + "' đã tồn tại, hoặc không có thay đổi nào trong thông tin cập nhật mới.";
                                        }
                                        break;
                                    default:
                                        {
                                            stt_code = -1;
                                            msg = "Có lỗi trong quá trình xử lý, vui lòng liên hệ với bộ phận IT.";
                                        }
                                        break;
                                }
                                           
                            }

                        }
                        break;
                    case "w":
                        {
                            Ward w = JsonConvert.DeserializeObject<Ward>(location_data_json);
                            var exists = _wardRepository.CheckWardExists(w, true);
                            switch (exists)
                            {
                                case 0:
                                    {
                                        if (w.Id < 1 || w.Name == "" || w.Name == null || w.Type == null || w.Type == "" || w.DistrictId == null || w.DistrictId == "")
                                        {
                                            stt_code = -1;
                                            msg = "Dữ liệu gửi lên không chính xác, vui lòng kiểm tra lại";
                                        }
                                        else
                                        {
                                            string result = await _wardRepository.UpdateWard(w);
                                            if (result != null)
                                            {
                                                int db_index = Convert.ToInt32(_Configuration["Redis:Database:db_common"]) > -1 ? Convert.ToInt32(_Configuration["Redis:Database:db_common"]) : 0;
                                                _RedisService.clear("WARD_" + w.Id, db_index);
                                                stt_code = 1;
                                                msg = "Ward Updated: " + result;
                                               
                                                data = w;
                                            }
                                            else
                                            {
                                                stt_code = -1;
                                                msg = "Cannot Update Ward: " + w.Name;
                                            }
                                        }
                                    }
                                    break;
                                case 1:
                                    {

                                        stt_code = -1;
                                        msg = w.Type+ " với tên '" + w.Name + "' đã tồn tại, hoặc không có thay đổi nào trong thông tin cập nhật mới.";
                                    }
                                    break;
                                default:
                                    {
                                        stt_code = -1;
                                        msg = "Có lỗi trong quá trình xử lý, vui lòng liên hệ với bộ phận IT.";
                                    }
                                    break;
                            }
                           

                        }
                        break;
                    default: break;
                }

                return Ok(new { stt_code = stt_code, msg = msg, data = data });
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateLocation - LocationController with '" + location_data_json + "' - location_type:" + location_type + " : \n" + ex.ToString());
                return Ok(new { stt_code = -2, msg = "Error On Excution" });
            }
        }
        public IActionResult AddLocation(string id)
        {
            string[] a = id.Split("-");
            if ((a.Length == 2) && (a[0] == "a"))
            {
                ViewBag.IsIDExsist = 1;
                switch (a[1])
                {
                    case "p":
                        {
                            ViewBag.LocationTypeList = new List<string> { "Tỉnh", "Thành phố" };
                            ViewBag.Label = "Thêm mới Tỉnh";

                        }
                        break;
                    case "d":
                        {
                            ViewBag.LocationTypeList = new List<string> { "Quận", "Huyện", "Thành phố" };
                            ViewBag.Label = "Thêm mới Quận";

                        }
                        break;
                    case "w":
                        {
                            ViewBag.LocationTypeList = new List<string> { "Phường", "Xã", "Đường" };
                            ViewBag.Label = "Thêm mới Phường";

                        }
                        break;
                    default: break;
                }
            }
            else
            {
                ViewBag.IsIDExsist = 0;
                ViewBag.LocationTypeList = new List<string> { "Tỉnh", "Thành phố", "Quận", "Huyện", "Phường", "Xã", "Đường" };
            }
            return View();

        }
        public IActionResult EditLocation(string location_id, string location_json)
        {
            try
            {
                string[] a = location_id.Split("-");
                if ((a.Length == 3) && (a[0] == "e"))
                {
                    switch (a[1])
                    {
                        case "p":
                            {
                                Province p = JsonConvert.DeserializeObject<Province>(location_json);
                                ViewBag.LocationTypeList = new List<string> { "Tỉnh", "Thành phố" };
                                ViewBag.LocationName = p.Name;
                                ViewBag.LocationType = p.Type;
                                if (p.Status == null)
                                    ViewBag.Status = 0;
                                else
                                    ViewBag.Status = (int)p.Status;
                            }
                            break;
                        case "d":
                            {
                                District d = JsonConvert.DeserializeObject<District>(location_json);

                                ViewBag.LocationTypeList = new List<string> { "Quận", "Huyện", "Thành phố" };
                                ViewBag.LocationName = d.Name;
                                ViewBag.LocationType = d.Type;
                                if (d.Status == null)
                                    ViewBag.Status = 0;
                                else
                                    ViewBag.Status = (int)d.Status;
                            }
                            break;
                        case "w":
                            {
                                Ward w = JsonConvert.DeserializeObject<Ward>(location_json);

                                ViewBag.LocationTypeList = new List<string> { "Phường", "Xã", "Đường" };
                                ViewBag.LocationName = w.Name;
                                ViewBag.LocationType = w.Type;
                                if (w.Status == null)
                                    ViewBag.Status = 0;
                                else
                                    ViewBag.Status = (int)w.Status;
                            }
                            break;
                        default: break;
                    }

                }
                else
                {
                    ViewBag.LocationTypeList = new List<string> { "Tỉnh", "Thành phố", "Quận", "Huyện", "Phường", "Xã", "Đường" };
                }
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("OpenEditForm - LocationController with '" + location_json + "' - location_id:" + location_id + " : \n" + ex.ToString());
                string error = "Error: " + ex.ToString();
                return Content(error);
            }
        }
        public async Task<IActionResult> Sync()
        {
            try 
            {

                var province=await _provinceRepository.GetProvincesList();
                if(province!=null && province.Count > 0)
                {
                    _RedisService.Set(CacheName.PROVINCE, JsonConvert.SerializeObject(province), Convert.ToInt32(_Configuration["Redis:Database:db_common"]));
                }
                var districts = await _districtRepository.GetDistrictList();
                if (districts != null && districts.Count > 0)
                {
                    _RedisService.Set(CacheName.DISTRICT, JsonConvert.SerializeObject(districts), Convert.ToInt32(_Configuration["Redis:Database:db_common"]));
                }
                var ward = await _wardRepository.GetWardList();
                if (ward != null && ward.Count > 0)
                {
                    _RedisService.Set(CacheName.WARD, JsonConvert.SerializeObject(ward), Convert.ToInt32(_Configuration["Redis:Database:db_common"]));
                }
                return Ok(new
                {
                    is_success = true
                });

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Sync - LocationController : " + ex.ToString());
                string error = "Error: " + ex.ToString();
                return Ok(new
                {
                    is_success = false
                });
            }
        }

    }
}
