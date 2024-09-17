using Caching.RedisWorker;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Utilities;
using Utilities.Contants;

namespace WEB.CMS.Customize
{
    public class ManagementUser
    {
        private IHttpContextAccessor _HttpContext;
        private IConfiguration _configuration;
        private readonly IUserRepository _UserRepository;
        private RedisConn _redisConn;

        public ManagementUser(IHttpContextAccessor context, IConfiguration configuration, IUserRepository UserRepository)
        {
            _HttpContext = context;
            _redisConn = new RedisConn(configuration);
            _redisConn.Connect();
            _configuration = configuration;
            _UserRepository = UserRepository;
        }

        public SysUserModel GetCurrentUser()
        {
            try
            {
                Claim ClaimDepartmentId = _HttpContext.HttpContext.User.FindFirst("DepartmentId");
                IEnumerable<PermissionData> permissions = null;
                int user_id = int.Parse(_HttpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                string role = _HttpContext.HttpContext.User.FindFirst(ClaimTypes.Role) != null ? _HttpContext.HttpContext.User.FindFirst(ClaimTypes.Role).Value : "";
                string ClaimUserUnderList = "";

                //-- Get From Cache
                permissions = Enumerable.Empty<PermissionData>();
                UserRoleCacheModel user_role_cache = new UserRoleCacheModel();
                string data_json = _redisConn.Get(CacheName.USER_ROLE + user_id + "_" + _configuration["CompanyType"], Convert.ToInt32(_configuration["Redis:Database:db_common"]));
                if(data_json!=null && data_json.Trim() != "")
                {
                    JArray objParr = null;
                    if (CommonHelper.GetParamWithKey(data_json, out objParr, _configuration["DataBaseConfig:key_api:api_manual"]))
                    {
                        user_role_cache = JsonConvert.DeserializeObject<UserRoleCacheModel>(objParr[0].ToString());
                        ClaimUserUnderList = user_role_cache.UserUnderList;
                        permissions = user_role_cache.Permission;
                    }
                    //user_role_cache = JsonConvert.DeserializeObject<UserRoleCacheModel>(data_json);
                    //ClaimUserUnderList = user_role_cache.UserUnderList;
                    //permissions = user_role_cache.Permission;
                }
                else
                {
                   user_role_cache = new UserRoleCacheModel();
                   var permission =  _UserRepository.GetUserPermissionById(user_id).Result;
                    if (permission != null && permission.Any())
                    {
                        user_role_cache.Permission = permission.Select(s => new PermissionData
                        {
                            MenuId = s.MenuId,
                            RoleId = s.RoleId,
                            PermissionId = s.PermissionId
                        });
                    }
                    else
                    {
                        user_role_cache.Permission = Enumerable.Empty<PermissionData>();
                    }
                    user_role_cache.UserUnderList = _UserRepository.GetListUserByUserId(user_id);
                    var data_encode = JsonConvert.SerializeObject(user_role_cache);
                    string token = CommonHelper.Encode(data_encode, _configuration["DataBaseConfig:key_api:api_manual"]);
                    //string token = data_encode;
                    _redisConn.Set(CacheName.USER_ROLE + user_id + "_" + _configuration["CompanyType"], token,Convert.ToInt32(_configuration["Redis:Database:db_common"]));
                }
                //-- Return model
                return new SysUserModel
                {
                    Id = int.Parse(_HttpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    Name = _HttpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value,
                    Email = _HttpContext.HttpContext.User.FindFirst(ClaimTypes.Email).Value,
                    Role = role,
                    DepartmentId = ClaimDepartmentId != null ? int.Parse(ClaimDepartmentId.Value) : 0,
                    Permissions = permissions,
                    UserUnderList = ClaimUserUnderList != null ? ClaimUserUnderList : String.Empty
                };
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetCurrentUser - ManagementUser: " + ex.ToString());
                return null;
            }
        }
    }
}
