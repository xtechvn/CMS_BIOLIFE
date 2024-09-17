using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels.Attachment;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class AttachFileRepository : IAttachFileRepository
    {
        private readonly ILogger<AttachFileRepository> _logger;
        private readonly AttachFileDAL _AttachFileDAL;
        private readonly CommonDAL _CommonDAL;

        public AttachFileRepository(IOptions<DataBaseConfig> dataBaseConfig,ILogger<AttachFileRepository> logger)
        {
            _logger = logger;
            _AttachFileDAL = new AttachFileDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _CommonDAL = new CommonDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<long> Delete(long Id, int userLogin)
        {
            try
            {
                var model = await _AttachFileDAL.FindAsync(Id);
                if (model.UserId != userLogin)
                {
                    return -1;
                }

                await _AttachFileDAL.DeleteAsync(Id);
                return Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete - AttachFileRepository : " + ex);
                return 0;
            }
        }
        public long DeleteAttachFilesByDataId(long Id, int type)
        {
            try
            {
                _CommonDAL.DeleteAttachFilesByDataId(Id, type);
                return Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteAttachFilesByDataId - AttachFileRepository : " + ex);
                return 0;
            }
        }

        public async Task<List<AttachFile>> GetListByType(long DataId, int Type)
        {
            try
            {
                return await _AttachFileDAL.GetListByType(DataId, Type);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetListByType - AttachFileRepository : " + ex);
                return new List<AttachFile>();
            }
        }

        public async Task<List<object>> CreateMultiple(List<AttachFile> models)
        {
            try
            {
                var _ListRs = new List<object>();
                foreach (var item in models)
                {
                    item.CreateDate = DateTime.Now;
                    var _AttachId = await _AttachFileDAL.CreateAsync(item);
                    _ListRs.Add(new
                    {
                        Id = _AttachId,
                        FilePath = item.Path,
                        Ext=item.Ext
                    });
                }
                return _ListRs;
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateMultiple - AttachFileRepository: " + ex);
                return null;
            }
        }
        public async Task<int> UpdateAttachFile(AttachFile attachFile)
        {
            try
            {

                return await _AttachFileDAL.UpdateAttachFile(attachFile);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAttachFile - AttachFileRepository: " + ex);
                return 0;
            }
        }
        public async Task<long> AddAttachFile(AttachFile attachFile)
        {
            try
            {

                var exists_id= await _AttachFileDAL.CheckIfAttachFileExists(attachFile);
                if(exists_id > 0)
                {
                    return exists_id;
                }
                else
                {
                    var id =  _AttachFileDAL.InsertAttachFile(attachFile);
                    return attachFile.Id;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAttachFile - AttachFileRepository: " + ex);
                return 0;
            }
        }
        public async Task<int> SaveAttachFileURL(List<AttachfileViewModel> attachFile,long data_id,int user_summit,int service_type)
        {
            try
            {
                List<long> remain_ids = new List<long>();

                if (attachFile!=null && attachFile.Count > 0)
                {
                   foreach(var att in attachFile)
                   {
                        var attach_file = new AttachFile()
                        {
                            CreateDate = DateTime.Now,
                            Capacity = 0,
                            DataId = data_id,
                            Ext = att.ext,
                            Id=att.id,
                            Path=att.path,
                            UserId= user_summit,
                            Type= service_type
                        };
                        var id= await _AttachFileDAL.SaveAttachFileURL(attach_file);
                        att.id = id;
                   }
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAttachFile - AttachFileRepository: " + ex);
                return 0;
            }
        }
        public async Task<int> DeleteNonExistsAttachFile(List<long> remain_ids, long data_id, int service_type)
        {
            try
            {
               
                var success = await _AttachFileDAL.DeleteNonExistsAttachFile(remain_ids, data_id, service_type);
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAttachFile - AttachFileRepository: " + ex);
                return 0;
            }
        }
        public async Task<List<AttachFile>> GetListByDataID(long dataId, int type)
        {
            try
            {
                return await _AttachFileDAL.GetListByDataID(dataId, type);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListByDataID - AttachFileRepository: " + ex);
                return null;
            }
        }
    }
}
