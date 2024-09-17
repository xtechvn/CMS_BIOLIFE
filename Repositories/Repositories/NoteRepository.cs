using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ILogger<NoteRepository> _logger;
        private readonly NoteDAL _NoteDAL;
        
        public NoteRepository(IOptions<DataBaseConfig> dataBaseConfig, ILogger<NoteRepository> logger)
        {
            _logger = logger;
            _NoteDAL = new NoteDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<long> Delete(long Id, int userLogin)
        {
            try
            {
                var model = await _NoteDAL.FindAsync(Id);
                if (model.UserId != userLogin)
                {
                    return -1;
                }

                if ((DateTime.Now - (model.UpdateTime == null ? model.CreateDate.Value : model.UpdateTime.Value)).TotalHours > 24)
                {
                    return -2;
                }

                await _NoteDAL.DeleteAsync(Id);
                return Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete - NoteRepository: " + ex.Message);
                return 0;
            }
        }

        public async Task<List<NoteViewModel>> GetListByType(long DataId, int Type)
        {
            try
            {
                return await _NoteDAL.GetListByType(DataId, Type);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetListByType - NoteRepository: " + ex.Message);
                return null;
            }
        }

        public async Task<long> UpSert(Note model)
        {
            try
            {
                if (model.Id == 0)
                {
                    model.CreateDate = DateTime.Now;
                    model.UpdateTime = DateTime.Now;
                    return await _NoteDAL.CreateAsync(model);
                }
                else
                {
                    var modelUpdate = await _NoteDAL.FindAsync(model.Id);
                    modelUpdate.Comment = model.Comment;
                    modelUpdate.UpdateTime = DateTime.Now;
                    await _NoteDAL.UpdateAsync(modelUpdate);
                    return modelUpdate.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("UpSert - NoteRepository: " + ex.Message);
                return 0;
            }
        }
    }
}
