using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using Repositories.Repositories.BaseRepos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly UserDAL _UserDAL;
        private readonly UserPositionDAL _userPositionDAL;
        private readonly MailConfig _MailConfig;
        private readonly MFADAL _MFADAL;
        private readonly IHttpContextAccessor _HttpContext;
        private readonly UserRoleDAL _userRoleDAL;
        private readonly UserDepartDAL _userDepart;

        public UserRepository(IHttpContextAccessor context, IOptions<DataBaseConfig> dataBaseConfig,
            IOptions<MailConfig> mailConfig, ILogger<UserRepository> logger)
        {
            _HttpContext = context;
            _logger = logger;
            _MailConfig = mailConfig.Value;
            _UserDAL = new UserDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _MFADAL = new MFADAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _userPositionDAL = new UserPositionDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _userRoleDAL = new UserRoleDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _userDepart = new UserDepartDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<UserDetailViewModel> CheckExistAccount(AccountModel entity)
        {
            try
            {
                var _encryptPassword = EncodeHelpers.MD5Hash(entity.Password);
                var _model = await _UserDAL.GetByUserName(entity.UserName);
                if (_model != null)
                {
                    if (_encryptPassword == _model.Password || _encryptPassword == _model.ResetPassword)
                    {
                        if (_model.Password != _model.ResetPassword)
                        {
                            if (_encryptPassword == _model.Password)
                            {
                                _model.ResetPassword = _encryptPassword;
                            }
                            else
                            {
                                _model.Password = _encryptPassword;
                            }

                            await _UserDAL.UpdateAsync(_model);
                        }

                        return await GetDetailUser(_model.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CheckExistAccount - UserRepository: " + ex);
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public async Task<bool> ResetPassword(string input)
        {
            try
            {
                User _model;
                if (StringHelpers.IsValidEmail(input))
                {
                    _model = await _UserDAL.GetByEmail(input);
                }
                else
                {
                    _model = await _UserDAL.GetByUserName(input);
                }

                if (_model != null)
                {
                    var _Password = StringHelpers.CreateRandomPassword();
                    _model.ResetPassword = EncodeHelpers.MD5Hash(_Password);
                    await _UserDAL.UpdateAsync(_model);

                    var _Subject = "Tin nhắn từ hệ thống";
                    var _Body = "Mật khẩu đăng nhập của bạn vừa được thay đổi: <b>" + _Password + "</b>";
                    // await EmailHelper.SendMailAsync(_model.Email, _Subject, _Body, string.Empty, string.Empty);

                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ResetPassword - UserRepository: " + ex);
                _logger.LogError("ResetPassword: " + ex.Message);
            }
            return false;
        }



        public async Task<IEnumerable<RolePermission>> GetUserPermissionById(int Id)
        {
            try
            {
                return await _UserDAL.GetUserPermissionById(Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserDetailViewModel> GetDetailUser(int Id)
        {
            var model = new UserDetailViewModel();
            try
            {
                model.Entity = await _UserDAL.FindAsync(Id);
                model.RoleIdList = await _UserDAL.GetUserRoleId(Id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetailUser - UserRepository: " + ex);
                _logger.LogError(ex.Message);
            }
            return model;
        }

        public async Task<UserDataViewModel> GetUser(int Id)
        {
            var model = new UserDataViewModel();
            try
            {
                model = await _UserDAL.GetUserInfoById(Id);
                model.RoleIdList = await _UserDAL.GetUserRoleId(Id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetailUser - UserRepository: " + ex);
                _logger.LogError(ex.Message);
            }
            return model;
        }

        public GenericViewModel<UserGridModel> GetPagingList(string userName, string strRoleId, int status, int currentPage, int pageSize)
        {
            var model = new GenericViewModel<UserGridModel>();
            try
            {
                model.ListData = _UserDAL.GetUserPagingList(userName, strRoleId, status, currentPage, pageSize, out int totalRecord);
                model.PageSize = pageSize;
                model.CurrentPage = currentPage;
                model.TotalRecord = totalRecord;
                model.TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - UserRepository: " + ex);
            }
            return model;
        }

        public async Task<int> Create(UserViewModel model)
        {
            try
            {
                var user_claim_id = _HttpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                var user_id = 0;
                if (user_claim_id != null) int.TryParse(user_claim_id.Value, out user_id);

                var entity = new User()
                {
                    UserName = StringHelpers.ConvertStringToNoSymbol(model.UserName.ToLower()).Replace(" ", ""),
                    FullName = model.FullName,
                    Password = EncodeHelpers.MD5Hash(model.Password),
                    ResetPassword = EncodeHelpers.MD5Hash(model.Password),
                    Phone = model.Phone ?? "",
                    BirthDay = !string.IsNullOrEmpty(model.BirthDayPicker) ?
                                DateTime.ParseExact(model.BirthDayPicker, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                              : model.BirthDay,
                    Gender = model.Gender,
                    Email = model.Email,
                    Avata = model.Avata,
                    Address = model.Address ?? "",
                    Status = model.Status,
                    DepartmentId = model.DepartmentId,
                    Note = model.Note ?? "",
                    CreatedBy = user_id,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = user_id,
                    UserPositionId = model.UserPositionId,
                    Level = model.Level,
                    Id = model.Id,
                    Manager = model.Manager,
                    UserMapId = model.UserMapId,
                    //UserRole = model.UserRole
                };

                // Check exist User Name or Email
                var userList = await _UserDAL.GetAllAsync();
                var exmodel = userList.Where(s => s.Status == 0 && (s.UserName == model.UserName/* || s.Email == model.Email*/));
                if (exmodel != null && exmodel.Count() > 0)
                {
                    return -1;
                }

                var userId = (int)await _UserDAL.CreateAsync(entity);

                if (!string.IsNullOrEmpty(model.RoleId))
                {
                    var role_list = model.RoleId.Split(',').Select(s => int.Parse(s)).ToArray();
                    await _UserDAL.UpdateUserRole(userId, role_list, 0);
                }
                return userId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                LogHelper.InsertLogTelegram("Create - UserRepository: " + ex);
            }

            return 0;
        }

        public async Task<int> UpdateUserRole(int userId, int[] arrayRole, int type)
        {
            try
            {
                await _UserDAL.UpdateUserRole(userId, arrayRole, type);
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateUserRole - UserRepository: " + ex);
                _logger.LogError(ex.Message);
            }
            return 0;
        }

        public async Task<int> ChangeUserStatus(int userId)
        {
            try
            {
                var model = await _UserDAL.FindAsync(userId);
                if (model.Status == 0)
                {
                    model.Status = 1;
                }
                else
                {
                    model.Status = 0;
                }
                await _UserDAL.UpdateAsync(model);
                return model.Status;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ChangeUserStatus - UserRepository: " + ex);
                _logger.LogError(ex.Message);
            }
            return -1;
        }

        public async Task<User> FindById(int id)
        {
            var model = new User();
            try
            {
                model = await _UserDAL.FindAsync(id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("FindById - UserRepository: " + ex);
                _logger.LogError(ex.Message);
            }
            return model;
        }

        public async Task<int> Update(UserViewModel model)
        {
            try
            {
                var entity = await _UserDAL.FindAsync(model.Id);

                // Check exist User Name or Email
                var userList = await _UserDAL.GetAllAsync();

                var exmodel = userList.Where(s => s.Id != entity.Id && s.Status == 0 && (s.UserName == entity.UserName /*|| s.Email == entity.Email*/)).ToList();
                if (exmodel != null && exmodel.Count > 0)
                {
                    return -1;
                }

                if (entity.DepartmentId.HasValue && entity.DepartmentId.Value > 0
                    && model.DepartmentId.HasValue && model.DepartmentId.Value > 0
                    && entity.DepartmentId.Value != model.DepartmentId.Value)
                {
                    var user_claim_id = _HttpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                    var user_id = 0;
                    if (user_claim_id != null) int.TryParse(user_claim_id.Value, out user_id);
                    await _UserDAL.LogDepartmentOfUser(entity, user_id);
                }

                entity.FullName = model.FullName;
                entity.Phone = model.Phone ?? "";
                entity.BirthDay = !string.IsNullOrEmpty(model.BirthDayPicker) ?
                                  DateTime.ParseExact(model.BirthDayPicker, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                                 : model.BirthDay;
                entity.Gender = model.Gender;
                entity.Email = model.Email;
                if (model.Avata != null)
                {
                    entity.Avata = model.Avata;
                }
                entity.Address = model.Address ?? "";
                entity.Status = model.Status;
                entity.Note = model.Note ?? "";
                entity.ModifiedBy = 1;
                entity.DepartmentId = model.DepartmentId;
                entity.ModifiedOn = DateTime.Now;
                entity.UserPositionId = model.UserPositionId;
                entity.Level = model.Level;


                await _UserDAL.UpdateAsync(entity);
                if (!string.IsNullOrEmpty(model.RoleId))
                {
                    var role_list = model.RoleId.Split(',').Select(s => int.Parse(s)).ToArray();
                    await _UserDAL.UpdateUserRole(entity.Id, role_list, 0);
                }
                return model.Id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Update - UserRepository: " + ex);
                _logger.LogError(ex.Message);
            }
            return 0;
        }

        public async Task<List<User>> GetUserSuggestionList(string name)
        {
            List<User> data = new List<User>();
            try
            {
                data = await _UserDAL.GetAllAsync();
                if (!string.IsNullOrEmpty(name))
                {
                    data = data.Where(s => s.UserName.Trim().ToLower().Contains(StringHelpers.ConvertStringToNoSymbol(name.Trim().ToLower()))).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserSuggestionList - UserRepository: " + ex);
                _logger.LogError(ex.Message);
            }
            return data;
        }

        public async Task<string> ResetPasswordByUserId(int userId)
        {
            var rs = string.Empty;
            try
            {
                var _model = await _UserDAL.FindAsync(userId);
                var _newPassword = StringHelpers.CreateRandomPassword();
                _model.ResetPassword = EncodeHelpers.MD5Hash(_newPassword);
                _model.Password = EncodeHelpers.MD5Hash(_newPassword);
                await _UserDAL.UpdateAsync(_model);
                rs = _newPassword;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ResetPasswordByUserId - UserRepository: " + ex);
                _logger.LogError(ex.Message);
            }
            return rs;
        }

        public async Task<int> ChangePassword(UserPasswordModel model)
        {
            var rs = 0;
            try
            {
                var _model = await _UserDAL.FindAsync(model.Id);
                if (_model.Password == EncodeHelpers.MD5Hash(model.Password))
                {
                    _model.ResetPassword = EncodeHelpers.MD5Hash(model.NewPassword);
                    _model.Password = EncodeHelpers.MD5Hash(model.NewPassword);
                    await _UserDAL.UpdateAsync(_model);
                    rs = model.Id;
                }
                else
                {
                    rs = -1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ChangePassword - UserRepository: " + ex);
                _logger.LogError(ex.Message);
            }
            return rs;
        }
        public List<User> GetAll()
        {
            try
            {
                return _UserDAL.GetAll();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<User>> GetUserSuggesstion(string txt_search)
        {
            try
            {
                if (txt_search == null) txt_search = "";
                return await _UserDAL.GetUserSuggesstion(txt_search);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserSuggesstion - ClientDAL: " + ex);
                return null;
            }
        } 
        public async Task<List<User>> GetUserSuggesstion(string txt_search, List<int> ids)
        {
            try
            {
                if (txt_search == null) txt_search = "";
                return await _UserDAL.GetUserSuggesstion(txt_search, ids);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserSuggesstion - ClientDAL: " + ex);
                return null;
            }
        }
        public async Task<User> GetClientDetailAsync(long clientId)
        {
            try
            {

                return _UserDAL.GetUserIdById(clientId);

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetail - ClientDAL: " + ex);
                return null;
            }
        }
        public async Task<User> GetById(long userIds)
        {
            try
            {

                return await _UserDAL.GetById(userIds);

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetail - ClientDAL: " + ex);
                return null;
            }
        }
        public List<UserPosition> GetUserPositions()
        {
            try
            {
                return _userPositionDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserPositions - ClientDAL: " + ex);
                return null;
            }
        }
        public async Task<UserPosition> GetUserPositionsByID(int id)
        {
            try
            {
                return await _userPositionDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserPositionsByID - ClientDAL: " + ex);
                return null;
            }
        }
        public async Task<List<Role>> GetUserActiveRoleList(int user_id)
        {
            try
            {
                return await _userRoleDAL.GetUserActiveRoleList(user_id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserActiveRoleList - ClientDAL: " + ex);
                return null;
            }
        }

        public string GetListUserByUserId(int user_id)
        {
            try
            {
                return _UserDAL.GetListUserByUserId(user_id);
            }
            catch
            {
                throw;
            }
        }

        //các chức năng của 1 quyền mà user đó dc phép  dùng
        public async Task<bool> CheckRolePermissionByUserAndRole(int UserId, int RoleId, int PermissionId, int MenuId)
        {
            try
            {
                var data = await _userRoleDAL.GetListRolePermissionByUserAndRole(UserId, new List<long>() { RoleId });
                if (data != null)
                {
                    var listdata = data.Where(s => s.PermissionId == PermissionId && s.MenuId == MenuId).ToList();
                    if (listdata.Count > 0)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListRolePermissionByUserAndRole - UserRepository: " + ex);
                return false;
            }
        }

        public async Task<bool> CheckRolePermissionByUserAndRole(int UserId, List<long> RoleIds, int PermissionId, int MenuId)
        {
            try
            {
                var data = await _userRoleDAL.GetListRolePermissionByUserAndRole(UserId, RoleIds);
                if (data != null)
                {
                    var listdata = data.Where(s => s.PermissionId == PermissionId && s.MenuId == MenuId).ToList();
                    return listdata.Count > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListRolePermissionByUserAndRole - UserRepository: " + ex);
                return false;
            }
        }

        public async Task<int> GetManagerByUserId(long UserId)
        {
            try
            {
                var data = await _userRoleDAL.GetManagerByUserId(UserId);

                return data;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetManagerByUserId - UserRepository: " + ex);
                return 0;
            }
        }
        public async Task<int> GetLeaderByUserId(long UserId)
        {
            try
            {
                var data = await _userRoleDAL.GetLeaderByUserId(UserId);

                return data;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetLeaderByUserId - UserRepository: " + ex);
                return 0;
            }
        }


        public async Task<User> GetChiefofDepartmentByServiceType(int service_type)
        {
            try
            {
                switch (service_type)
                {
                    case (int)ServiceType.BOOK_HOTEL_ROOM_VIN:
                        {
                            return await _UserDAL.GetChiefofDepartmentByRoleID((int)RoleType.TPDHKS);
                        }
                    case (int)ServiceType.PRODUCT_FLY_TICKET:
                        {
                            return await _UserDAL.GetChiefofDepartmentByRoleID((int)RoleType.TPDHVe);

                        }
                    case (int)ServiceType.Tour:
                        {
                            return await _UserDAL.GetChiefofDepartmentByRoleID((int)RoleType.TPDHTour);

                        }
                    default:
                        {
                            return await _UserDAL.GetChiefofDepartmentByRoleID((int)RoleType.TPDHKS);
                        }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetChiefofDepartmentByServiceType - UserRepository: " + ex);
            }
            return null;
        }

        public async Task<List<User>> GetChiefofDepartmentByServiceTypeNew(int service_type)
        {
            try
            {
                switch (service_type)
                {
                    case (int)ServiceType.BOOK_HOTEL_ROOM_VIN:
                        {
                            return await _UserDAL.GetListChiefofDepartmentByRoleID((int)RoleType.TPDHKS);
                        }
                    case (int)ServiceType.PRODUCT_FLY_TICKET:
                        {
                            return await _UserDAL.GetListChiefofDepartmentByRoleID((int)RoleType.TPDHVe);
                        }
                    case (int)ServiceType.Tour:
                        {
                            return await _UserDAL.GetListChiefofDepartmentByRoleID((int)RoleType.TPDHTour);
                        }
                    case (int)ServiceType.Other:
                        {
                            return await _UserDAL.GetListChiefofDepartmentByRoleID(new List<int>() {
                            (int)RoleType.TPDHKS,
                            (int)RoleType.TPDHTour,
                            (int)RoleType.TPDHVe,
                            (int)RoleType.TPDHKS});
                        }
                    default:
                        {
                            return await _UserDAL.GetListChiefofDepartmentByRoleID((int)RoleType.TPDHKS);
                        }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetChiefofDepartmentByServiceType - UserRepository: " + ex);
            }
            return new List<User>();
        }

        public List<User> GetAdminUser()
        {
            try
            {
                return _UserDAL.GetListUserByRole((int)RoleType.Admin);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAdminUser - UserRepository: " + ex);
            }
            return new List<User>();
        }

        public List<User> GetHeadOfAccountantUser()
        {
            try
            {
                return _UserDAL.GetListUserByRole((int)RoleType.KeToanTruong);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetHeadOfAccountantUser - UserRepository: " + ex);
            }
            return new List<User>();
        }

        public bool IsAdmin(long userId)
        {
            try
            {
                var listUserAdmin = _UserDAL.GetListUserByRole((int)RoleType.Admin);
                return listUserAdmin.FirstOrDefault(n => n.Id == userId) != null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("isAdmin - UserRepository: " + ex);
            }
            return false;
        }

        public bool IsHeadOfAccountant(long userId)
        {
            try
            {
                var listHeadOfAccountant = _UserDAL.GetListUserByRole((int)RoleType.KeToanTruong);
                return listHeadOfAccountant.FirstOrDefault(n => n.Id == userId) != null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("isHeadOfAccountant - UserRepository: " + ex);
            }
            return false;
        }

        public bool IsAccountant(long userId)
        {
            try
            {
                var listIsAccountant = _UserDAL.GetListUserByRole((int)RoleType.KT);
                return listIsAccountant.FirstOrDefault(n => n.Id == userId) != null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("isHeadOfAccountant - UserRepository: " + ex);
            }
            return false;
        }
        public List<User> GetHeadOfAccountantUser2()
        {
            try
            {
                return _UserDAL.GetListUserByRole((int)RoleType.TPKS);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetHeadOfAccountantUser2 - UserRepository: " + ex);
            }
            return new List<User>();
        }
        public async Task<List<User>> GetByIds(List<long> userIds)
        {
            try
            {

                return await _UserDAL.GetByIds(userIds);

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetail - ClientDAL: " + ex);
                return null;
            }
        }
        public async Task<List<UserDepart>> GetListUserDepartById(List<int?> ids)
        {
            try
            {

                return await _userDepart.GetByIds(ids);

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetDetail - ClientDAL: " + ex);
                return null;
            }
        }
        public async Task<int> CountUser()
        {
            try
            {

                return await _UserDAL.CountUser();

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CountUser - ClientDAL: " + ex);
                return -1;
            }
        }
    }
}
