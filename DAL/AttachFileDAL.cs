using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class AttachFileDAL : GenericService<AttachFile>
    {
        private DbWorker dbWorker;

        public AttachFileDAL(string connection) : base(connection) {
            dbWorker = new DbWorker(connection);
        }
        public async Task<List<AttachFile>> GetListByType(long DataId, int Type)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.AttachFiles.Where(s => s.DataId == DataId && s.Type == Type).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListByType - AttachFileDAL: " + ex);
                return new List<AttachFile>();
            }
        }
        public async Task<int> UpdateAttachFile(AttachFile attachFile)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var deta = _DbContext.AttachFiles.Update(attachFile);
                    _DbContext.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAttachFile - AttachFileDAL: " + ex);
                return 0;
            }
        }
        public async Task<long> CheckIfAttachFileExists(AttachFile attachFile)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var data = await _DbContext.AttachFiles.AsNoTracking().FirstOrDefaultAsync(s => s.Path == attachFile.Path && s.Type ==attachFile.Type && s.DataId==attachFile.DataId && s.Ext==attachFile.Ext);
                    if(data!=null && data.Id > 0)
                    {
                        return data.Id;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CheckIfAttachFileExists - AttachFileDAL: " + ex);
                return -1;
            }
        }
        public async Task<List<AttachFile>> GetListByDataID(long dataId, int type)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.AttachFiles.AsNoTracking().Where(s => s.DataId == dataId && s.Type == type).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CheckIfAttachFileExists - AttachFileDAL: " + ex);
                return null;
            }
        }
        public async Task<long> SaveAttachFileURL(AttachFile attachFile)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var exists= await _DbContext.AttachFiles.Where(s => s.Id==attachFile.Id && s.DataId == attachFile.DataId && s.Type == attachFile.Type && s.Path==attachFile.Path).FirstOrDefaultAsync();
                    if(exists!=null && exists.Id > 0)
                    {
                        return exists.Id;
                    }
                    else
                    {
                        var new_attach = new AttachFile()
                        {
                            Capacity=attachFile.Capacity,
                            CreateDate=DateTime.Now,
                            DataId=attachFile.DataId,
                            Ext=attachFile.Ext,
                            Path=attachFile.Path,
                            Type=attachFile.Type,
                            UserId=attachFile.UserId
                        };
                        _DbContext.AttachFiles.Add(new_attach);

                        await _DbContext.SaveChangesAsync();
                        return new_attach.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAttachFile - AttachFileDAL: " + ex);
                return -1;
            }
        }
        public async Task<int> DeleteNonExistsAttachFile(List<long> remain_ids, long data_id,  int service_type)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var exists = await _DbContext.AttachFiles.Where(s =>  s.DataId == data_id && s.Type == service_type && !remain_ids.Contains(s.Id)).ToListAsync();
                    if (exists != null && exists.Count > 0)
                    {
                        _DbContext.AttachFiles.RemoveRange(exists);
                        await _DbContext.SaveChangesAsync();
                    }
                    return 1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAttachFile - AttachFileDAL: " + ex);
                return -1;
            }
        }
        public int InsertAttachFile(AttachFile booking)
        {
            try
            {

                SqlParameter[] objParam_order = new SqlParameter[6];
                objParam_order[0] = new SqlParameter("@DataId", booking.DataId);
                objParam_order[1] = new SqlParameter("@UserId", booking.UserId);
                objParam_order[2] = new SqlParameter("@Type", booking.Type);
                objParam_order[3] = new SqlParameter("@Path", booking.Path);
                objParam_order[4] = new SqlParameter("@Ext", booking.Ext);
                objParam_order[5] = new SqlParameter("@Capacity", booking.Capacity);

                var id = dbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertAttachFile, objParam_order);
                booking.Id = id;
                return id;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CreateHotelBookingRooms - HotelBookingDAL. " + ex);
                return -1;
            }
        }
    }
}
