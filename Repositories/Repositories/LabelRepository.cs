using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Microsoft.Extensions.Logging;
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
    public class LabelRepository : ILabelRepository
    {
        private readonly ILogger<LabelRepository> _logger;
        private readonly LabelDAL _LabelDAL;
        public LabelRepository(IOptions<DataBaseConfig> dataBaseConfig, ILogger<LabelRepository> logger)
        {
            _logger = logger;
            _LabelDAL = new LabelDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }
        public async Task<int> Create(Label model)
        {
            try
            {
                var entity = new Label();
                entity.Icon = model.Icon;
                entity.CreateTime = DateTime.Now;
                entity.Domain = model.Domain;
                entity.PrefixOrderCode = model.PrefixOrderCode;
                entity.StoreName = model.StoreName;
                await _LabelDAL.CreateAsync(entity);
                return model.Id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Create - LabelRepository" + ex.Message);
                return -1;
            }
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Label> GetAll()
        {
            return _LabelDAL.GetAll();
        }

        public Task<Label> GetById(int Id)
        {
            return _LabelDAL.FindAsync(Id);
        }

        public async Task<int> Update(Label model)
        {
            try
            {
                var entity = await _LabelDAL.FindAsync(model.Id);
                entity.Icon = model.Icon;
                entity.Domain = model.Domain;
                entity.PrefixOrderCode = model.PrefixOrderCode;
                entity.StoreName = model.StoreName;
                entity.UpdateTime = DateTime.Now;
                await _LabelDAL.UpdateAsync(entity);
                return model.Id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Update - LabelRepository" + ex.Message);
                return -1;
            }
        }

        public List<Label> GetListAll()
        {
            return _LabelDAL.GetAll();
        }

        public Task<List<Label>> GetLabelActive()
        {
            return _LabelDAL.getLabelActive();
        }
    }
}
