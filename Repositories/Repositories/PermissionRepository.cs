using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private PermissionDAL _PermissionDAL;

        public PermissionRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _PermissionDAL = new PermissionDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<int> Create(PermissionModel model)
        {
            try
            {
                var entity = new Permission()
                {
                    SortOrder = model.SortOrder,
                    CreatedOn = DateTime.Now,
                    Name = model.Name,
                    Status = model.Status,
                };
                var listAllPermission = _PermissionDAL.GetAll();
                if (listAllPermission.Where(n => n.Name.Equals(model.Name)).FirstOrDefault() != null)
                {
                    return 2;
                }

                var rs = (int)await _PermissionDAL.CreateAsync(entity);
                return rs;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Create - PermissionRepository:" + ex);
            }
            return 0;
        }

        public async Task<List<Permission>> GetAll()
        {
            return await _PermissionDAL.GetAllAsync();
        }

        public Task<Permission> GetById(int Id)
        {
            return _PermissionDAL.FindAsync(Id);
        }

        public GenericViewModel<PermissionModel> GetPagingList(string permissionName, int permissionId, int currentPage, int pageSize)
        {
            var model = new GenericViewModel<PermissionModel>();
            int totalRecord = 0;
            try
            {
                model.ListData = _PermissionDAL.GetListPaging(permissionName, permissionId, out totalRecord, currentPage, pageSize);
                model.PageSize = pageSize;
                model.CurrentPage = currentPage;
                model.TotalRecord = totalRecord;
                model.TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - PermissionRepository:" + ex);
            }
            return model;
        }

        public async Task<int> Update(PermissionModel model)
        {
            try
            {
                var entity = await _PermissionDAL.FindAsync(model.Id);
                entity.Name = model.Name;
                entity.SortOrder = model.SortOrder;
                entity.ModifiedOn = model.ModifiedOn;
                var listAllPermission = _PermissionDAL.GetAll();
                if (listAllPermission.Where(n => n.Name.Equals(model.Name)).FirstOrDefault() != null)
                {
                    return 2;
                }
                await _PermissionDAL.UpdateAsync(entity);
                return model.Id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Update - PermissionRepository:" + ex);
                return -1;
            }
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
