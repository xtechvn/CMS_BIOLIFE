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
using Utilities.Contants;

namespace DAL
{
    public class UserRoleDAL : GenericService<UserRole>
    {
        private static DbWorker _DbWorker;

        public UserRoleDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public async Task<List<Role>> GetUserActiveRoleList(int user_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var list_role_id = await _DbContext.UserRoles.Where(s => s.UserId == user_id).ToListAsync();
                    if (list_role_id != null && list_role_id.Count > 0)
                    {
                        return await _DbContext.Roles.Where(s => list_role_id.Select(x => x.RoleId).Contains(s.Id)).ToListAsync();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetById - UserPositionDAL: " + ex);
                return null;
            }
        }
        public async Task<List<RolePermissionViewModel>> GetListRolePermissionByUserAndRole(long UserId, List<long> RoleIds)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@UserId", UserId);
                objParam[1] = new SqlParameter("@RoleId", string.Join(',', RoleIds));
                DataTable dt = _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetListRolePermissionByUserAndRole, objParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<RolePermissionViewModel>();
                    return data;
                }

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListRolePermissionByUserAndRole - UserRoleDAL. " + ex);
                return null;
            }
        }
        public async Task<int> GetManagerByUserId(long UserId)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@UserId", UserId);

                DataTable dt = _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetManagerByUserId, objParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<RolePermissionViewModel>();
                    var id = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    return id;
                }

                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListRolePermissionByUserAndRole - UserRoleDAL. " + ex);
                return 0;
            }
        }
        public async Task<int> GetLeaderByUserId(long UserId)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@UserId", UserId);

                DataTable dt = _DbWorker.GetDataTable(StoreProcedureConstant.Sp_GetLeaderByUserId, objParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<RolePermissionViewModel>();
                    var id = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    return id;
                }

                return 0;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListRolePermissionByUserAndRole - UserRoleDAL. " + ex);
                return 0;
            }
        }

    }
}