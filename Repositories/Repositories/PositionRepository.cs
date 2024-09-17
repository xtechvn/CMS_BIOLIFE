using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
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
    public class PositionRepository : IPositionRepository
    {
        private readonly ILogger<PositionRepository> _logger;
        private readonly PositionDAL _PositionDAL;
        public PositionRepository(IOptions<DataBaseConfig> dataBaseConfig, ILogger<PositionRepository> logger)
        {
            _logger = logger;
            _PositionDAL = new PositionDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }
        public async Task<int> Create(Position model)
        {
            try
            {
                var entity = new Position();
                entity.Height = model.Height;
                entity.Width = model.Width;
                entity.PositionName = model.PositionName;
                await _PositionDAL.CreateAsync(entity);
                return model.Id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Create - PositionRepository" + ex.Message);
                return -1;
            }
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Position>> GetAll()
        {
            return await _PositionDAL.GetAllAsync();
        }

        public async Task<Position> GetById(int Id)
        {
            return await _PositionDAL.FindAsync(Id);
        }

        public async Task<Position> GetByPositionName(string positionName)
        {
            return await _PositionDAL.GetByPositionName(positionName);
        }

        public List<Position> GetListAll()
        {
            return _PositionDAL.GetListAllData();
        }

        public async Task<int> Update(Position model)
        {
            try
            {
                var entity = await _PositionDAL.FindAsync(model.Id);
                entity.Height = model.Height;
                entity.Width = model.Width;
                entity.PositionName = model.PositionName;
                await _PositionDAL.UpdateAsync(entity);
                return model.Id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Update - PositionRepository" + ex.Message);
                return -1;
            }
        }
    }
}
