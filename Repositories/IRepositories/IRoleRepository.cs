using Entities.Models;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAll();
        GenericViewModel<RoleDataModel> GetPagingList(string roleName, string strUserId, int currentPage, int pageSize);
        Task<int> Upsert(RoleViewModel entity);
        Task<Role> GetById(int Id);
        Task<int> UpdateUserRole(int roleId, int userId, int type);
        Task<List<User>> GetListUserOfRole(int Id);

        /// <summary>
        /// Add Or Delete Role Permission
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type">
        /// 1:Add
        /// 0:Remove
        /// </param>
        /// <returns></returns>
        Task<bool> AddOrDeleteRolePermission(string data, int type);

        Task<List<RolePermission>> GetRolePermissionById(int roleId);

        Task<int> DeleteRole(int roleId);

        Task<List<Role>> GetRoleListByUserId(int userId);
    }
}
