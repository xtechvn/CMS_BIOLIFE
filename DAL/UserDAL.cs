using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DAL
{
    public class UserDAL : GenericService<User>
    {
        private static DbWorker _DbWorker;
        public UserDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public List<UserGridModel> GetUserPagingList(string userName, string strRoleId, int status, int currentPage, int pageSize, out int totalRecord)
        {
            totalRecord = 0;
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var ListUser = _DbContext.Users.AsQueryable();

                    if (!string.IsNullOrEmpty(userName))
                    {
                        ListUser = ListUser.Where(s => s.UserName.Contains(userName) || s.FullName.Contains(userName) || s.Email.Contains(userName));
                    }

                    if (status != -1)
                    {
                        ListUser = ListUser.Where(s => s.Status == status);
                    }

                    if (!string.IsNullOrEmpty(strRoleId))
                    {
                        var ListUserId = GetListUserIdByRole(strRoleId);
                        ListUser = ListUser.Where(s => ListUserId.Contains(s.Id));
                    }

                    totalRecord = ListUser.Count();
                    ListUser = ListUser.OrderByDescending(s => s.CreatedOn).Skip((currentPage - 1) * pageSize).Take(pageSize);


                    var data = ListUser.Select(s => new UserGridModel
                    {
                        Id = s.Id,
                        Avata = s.Avata,
                        UserName = s.UserName,
                        FullName = s.FullName,
                        Phone = s.Phone,
                        Email = s.Email,
                        Note = s.Note,
                        BirthDay = s.BirthDay,
                        Address = s.Address,
                        Status = s.Status,
                        CreatedOn = s.CreatedOn,
                        RoleList = (from a in _DbContext.UserRoles.Where(a => a.UserId == s.Id)
                                    join b in _DbContext.Roles on a.RoleId equals b.Id into bs
                                    from b in bs.DefaultIfEmpty()
                                    select new Role
                                    {
                                        Id = b.Id,
                                        Name = b.Name,
                                        Status = b.Status
                                    }).ToList(),
                        UserDepartment = (from a in _DbContext.Departments.Where(a => a.Id == s.DepartmentId)
                                          select new Department
                                          {
                                              Id = a.Id,
                                              DepartmentName = a.DepartmentName,
                                          }).FirstOrDefault(),
                        UserPosition = (from a in _DbContext.UserPositions.Where(a => a.Id == s.UserPositionId)
                                        select new UserPosition
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                        }).FirstOrDefault(),

                    }).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserPagingList - UserDAL: " + ex);
            }
            return null;
        }

        /// <summary>
        /// UpdateUserRole
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="type">
        /// 0 : add roles
        /// 1 : remove roles</param>
        /// <returns></returns>
        public async Task UpdateUserRole(int userId, int[] arrayRole, int type = 0)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    if (type == 0)
                    {
                        foreach (var roleId in arrayRole)
                        {
                            var model = await _DbContext.UserRoles.Where(s => s.UserId == userId && s.RoleId == roleId).FirstOrDefaultAsync();
                            if (model == null || model.Id <= 0)
                            {
                                model = new UserRole
                                {
                                    UserId = userId,
                                    RoleId = roleId
                                };

                                await _DbContext.UserRoles.AddAsync(model);
                                await _DbContext.SaveChangesAsync();
                            }

                        }
                        var list = await _DbContext.UserRoles.Where(s => s.UserId == userId && !arrayRole.Contains(s.RoleId)).ToListAsync();
                        if (list != null && list.Count > 0)
                        {
                            _DbContext.UserRoles.RemoveRange(list);
                            await _DbContext.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        foreach (var roleId in arrayRole)
                        {
                            var model = await _DbContext.UserRoles.Where(s => s.UserId == userId && s.RoleId == roleId).FirstOrDefaultAsync();
                            if (model != null)
                            {
                                _DbContext.UserRoles.Remove(model);
                                await _DbContext.SaveChangesAsync();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateUserRole - UserDAL: " + ex);
            }

        }

        public async Task<List<int>> GetUserRoleId(int userId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.UserRoles.Where(x => x.UserId == userId).Select(s => s.RoleId).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserRoleId - UserDAL: " + ex);
                return new List<int>();
            }
        }

        public async Task<User> GetByUserName(string input)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Users.FirstOrDefaultAsync(s => s.UserName.Equals(input));
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByUserName - UserDAL: " + ex);
                return null;
            }
        }

        public async Task<List<User>> GetByIds(List<long> userIds)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Users.Where(s => userIds.Contains(s.Id)).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByIds - UserDAL: " + ex);
                return new List<User>();
            }
        }

        public async Task<User> GetById(long userIds)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Users.FirstOrDefaultAsync(s => userIds == s.Id);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByIds - UserDAL: " + ex);
                return null;
            }
        }

        public async Task<UserDataViewModel> GetUserInfoById(long user)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await (from u in _DbContext.Users
                                  join department in _DbContext.Departments on u.DepartmentId equals department.Id into dep
                                  from subdep in dep.DefaultIfEmpty()
                                  where u.Id == user
                                  select new UserDataViewModel
                                  {
                                      Id = u.Id,
                                      UserName = u.UserName,
                                      FullName = u.FullName,
                                      Email = u.Email,
                                      Phone = u.Phone,
                                      Address = u.Address,
                                      Avata = u.Avata,
                                      BirthDay = u.BirthDay,
                                      DepartmentId = u.DepartmentId,
                                      DepartmentName = subdep.DepartmentName,
                                      Status = u.Status,
                                      Gender = u.Gender,
                                      UserPositionId = u.UserPositionId,
                                      Level = u.Level
                                  }).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByIds - UserDAL: " + ex);
                return null;
            }
        }

        public async Task<User> GetByEmail(string input)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Users.FirstOrDefaultAsync(s => s.Email.Equals(input));
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByUserName - UserDAL: " + ex);
                return null;
            }
        }

        public List<int> GetListUserIdByRole(string strRoleId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var arrRoleId = strRoleId.Split(',').Select(s => int.Parse(s)).ToArray();
                    return _DbContext.UserRoles.Where(s => arrRoleId.Contains(s.RoleId)).Select(s => s.UserId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListUserIdByRole - UserDAL: " + ex);
                return new List<int>();
            }

        }
        public User GetUserIdById(long Id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    return _DbContext.Users.FirstOrDefault(s => s.Id == Id);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserIdById - UserDAL: " + ex);
                return null;
            }

        }
        public List<User> GetAll()
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    return _DbContext.Users.AsNoTracking().ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAll - UserDAL: " + ex);
                return null;
            }
        }
        public async Task<List<User>> GetUserSuggesstion(string txt_search)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Users.AsNoTracking().Where(s => s.UserName.ToLower().Contains(txt_search.ToLower())).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByUserName - UserDAL: " + ex);
                return null;
            }
        }

        public async Task<IEnumerable<RolePermission>> GetUserPermissionById(int user_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await (from a in _DbContext.RolePermissions
                                  join b in _DbContext.UserRoles on a.RoleId equals b.RoleId
                                  where b.UserId == user_id
                                  select new RolePermission
                                  {
                                      MenuId = a.MenuId,
                                      RoleId = a.RoleId,
                                      PermissionId = a.PermissionId
                                  }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetMenuPermissionByUserId - UserDAL: " + ex);
                return null;
            }
        }

        public async Task LogDepartmentOfUser(User model, int current_user_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var user_department_model = await _DbContext.UserDeparts.Where(s => s.UserId == model.Id)
                        .OrderByDescending(s => s.Id)
                        .FirstOrDefaultAsync();

                    var dataJoinTime = model.CreatedOn;

                    if (user_department_model != null)
                    {
                        dataJoinTime = user_department_model.LeaveDate;
                    }

                    _DbContext.UserDeparts.Add(new UserDepart
                    {
                        UserId = model.Id,
                        DepartmentId = model.DepartmentId,
                        JoinDate = dataJoinTime,
                        LeaveDate = DateTime.Now,
                        CreatedBy = current_user_id,
                        CreatedDate = DateTime.Now
                    });

                    await _DbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("LogDepartmentOfUser - UserDAL: " + ex);
                throw;
            }
        }

        public string GetListUserByUserId(int user_id)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@UserId", user_id);
                DataTable dataTable = _DbWorker.GetDataTable(ProcedureConstants.SP_GetListUserByUserId, objParam);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["ListUserId"].ToString();
                }
                else
                {
                    return user_id.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - PolicyDal: " + ex);
                throw;
            }
        }
        public async Task<User> GetChiefofDepartmentByRoleID(int role_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var user_role = await _DbContext.UserRoles.Where(s => s.RoleId == role_id).FirstOrDefaultAsync();
                    if (user_role != null && user_role.Id > 0)
                    {
                        return await _DbContext.Users.Where(s => s.Id == user_role.UserId && s.Status == 0).FirstOrDefaultAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetChiefofDepartmentByServiceType - PolicyDal: " + ex);
            }
            return null;
        }

        public async Task<List<User>> GetListChiefofDepartmentByRoleID(int role_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var user_role = _DbContext.UserRoles.Where(s => s.RoleId == role_id).ToList();
                    var listUserId = user_role.Select(n => n.UserId).ToList();
                    if (listUserId.Count > 0)
                    {
                        return await _DbContext.Users.Where(s => listUserId.Contains(s.Id) && s.Status == 0).ToListAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetChiefofDepartmentByServiceType - PolicyDal: " + ex);
            }
            return new List<User>();
        }

        public async Task<List<User>> GetListChiefofDepartmentByRoleID(List<int> role_ids)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var user_role = _DbContext.UserRoles.Where(s => role_ids.Contains(s.RoleId)).ToList();
                    var listUserId = user_role.Select(n => n.UserId).ToList();
                    if (listUserId.Count > 0)
                    {
                        return await _DbContext.Users.Where(s => listUserId.Contains(s.Id) && s.Status == 0).ToListAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetChiefofDepartmentByServiceType - PolicyDal: " + ex);
            }
            return new List<User>();
        }

        public List<User> GetListUserByRole(int role_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var user_role = _DbContext.UserRoles.Where(s => s.RoleId == role_id).ToList();
                    var userRoleIds = user_role.Select(n => n.UserId).ToList();
                    if (userRoleIds.Count > 0)
                    {
                        return _DbContext.Users.Where(s => userRoleIds.Contains(s.Id) && s.Status == 0).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetChiefofDepartmentByServiceType - PolicyDal: " + ex);
            }
            return new List<User>();
        }
        public async Task<List<User>> GetUserSuggesstion(string txt_search,List<int> ids)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Users.AsNoTracking().Where(s => s.UserName.ToLower().Contains(txt_search.ToLower()) && ids.Contains(s.Id)).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByUserName - UserDAL: " + ex);
                return null;
            }
        }
        public async Task<int> CountUser()
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Users.CountAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CountUser - UserDAL: " + ex);
                return -1;
            }
        }

    }
}