using Entities.Models;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAll();
        GenericViewModel<PermissionModel> GetPagingList(string permissionName, int permissionId, int currentPage, int pageSize);
        Task<Permission> GetById(int Id);
        Task<int> Create(PermissionModel model);
        Task<int> Update(PermissionModel model);
        Task<int> Delete(int id);
    }
}
