using Aspose.Cells;
using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Repositories.Repositories.BaseRepos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace Repositories.IRepositories
{
    public class DepartmentRepository : BaseRepository, IDepartmentRepository
    {
        private readonly DepartmentDAL _DepartmentDAL;
        private const string DEPARTMENT_CODE_FORMAT = "DEPARTMENT.{0}.{1}";

        public DepartmentRepository(IHttpContextAccessor context, IOptions<DataBaseConfig> dataBaseConfig, IUserRepository userRepository, IConfiguration configuration) : base(context, dataBaseConfig, configuration, userRepository)
        {
            _DepartmentDAL = new DepartmentDAL(_SqlServerConnectString);
        }

        public async Task<long> Create(Department model)
        {
            try
            {
                await CheckExistName(model);
                Department parent_model = null;

                if (model.ParentId != null && model.ParentId.Value > 0) parent_model = await GetById(model.ParentId.Value);

                model.FullParent = parent_model != null ? $"{(!String.IsNullOrEmpty(parent_model.FullParent) ? $"{parent_model.FullParent}," : String.Empty)}{parent_model.Id}" : String.Empty;
                model.Status = 0;
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = _SysUserModel.Id;
                var data = await _DepartmentDAL.CreateAsync(model);
                var update = await GetById(model.Id);
                update.FullParent += ","+data;
                await _DepartmentDAL.UpdateAsync(update);
                return data;
            }
            catch
            {
                throw;
            }
        }

        public async Task<long> Update(Department model)
        {
            try
            {
                await CheckExistName(model);
                Department parent_model = null;

                if (model.ParentId != null && model.ParentId.Value > 0) parent_model = await GetById(model.ParentId.Value);

                var data = await GetById(model.Id);

                data.Description = model.Description;
                data.DepartmentName = model.DepartmentName;
                data.FullParent = parent_model != null ? ($"{(!String.IsNullOrEmpty(parent_model.FullParent) ? $"{parent_model.FullParent}," : data.FullParent)}{parent_model.Id}") : data.FullParent;
                data.DepartmentCode = model.DepartmentCode;
                data.UpdatedDate = DateTime.Now;
                data.UpdatedBy = _SysUserModel.Id;
                data.Branch = model.Branch;

                await _DepartmentDAL.UpdateAsync(data);
                return model.Id;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Department>> GetAll(string name)
        {
            try
            {
                var datas = await _DepartmentDAL.GetByConditionAsync(s => s.IsDelete == false);


                if (!String.IsNullOrEmpty(name))
                {
                    List<Department> result = datas.Where(s => s.DepartmentName.ToLower().Contains(name.ToLower())).ToList();
                    var full_parent_ids = result.Select(s => s.FullParent).Where(s => !string.IsNullOrEmpty(s))
                        .SelectMany(s => s.Split(',')).Select(s => int.Parse(s)).Distinct().ToList();

                    result.AddRange(datas.Where(s => full_parent_ids.Contains(s.Id)));

                    return result.Distinct();
                }

                return datas;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Department> GetById(int id)
        {
            return await _DepartmentDAL.FindAsync(id);
        }

        public async Task<long> Delete(int id)
        {
            try
            {
                var child_datas = await _DepartmentDAL.GetByConditionAsync(s => s.ParentId == id && s.IsDelete == false);
                if (child_datas != null && child_datas.Any())
                    throw new Exception("Phòng ban đang chứa phòng ban con đang hoạt động. Bạn không thể xóa phòng ban đã chọn.");

                var data = await GetById(id);
                data.IsDelete = true;
                await _DepartmentDAL.UpdateAsync(data);
                return id;
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> CheckExistName(Department model)
        {
            try
            {
                var datas = await _DepartmentDAL.GetByConditionAsync(s => s.DepartmentName.ToLower() == model.DepartmentName.ToLower()
                && s.ParentId == model.ParentId && s.Id != model.Id);

                if (datas != null && datas.Any())
                {
                    throw new Exception("Tên phòng ban đã tồn tại trong cùng phòng ban Cha");
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

    }
}
