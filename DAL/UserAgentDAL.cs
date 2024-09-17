using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels.UserAgent;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class UserAgentDAL : GenericService<UserAgent>
    {
        private static DbWorker _DbWorker;
        public UserAgentDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }
        public int CreateUserAgent(UserAgent model)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    var deta = _DbContext.UserAgents.Add(model);
                    _DbContext.SaveChanges();


                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CreateUserAgent - UserAgentDAL: " + ex);
                return 0;
            }
        }
        public int UpdataUserAgent(int Id, int UserId, int create_id, long ClientId)
        {
            try
            {
                if (Id == 0)
                {
                    SqlParameter[] objParam = new SqlParameter[8];
                    objParam[0] = new SqlParameter("@UserId", UserId);
                    objParam[1] = new SqlParameter("@ClientId", ClientId);
                    objParam[2] = new SqlParameter("@MainFollow", DBNull.Value);
                    objParam[3] = new SqlParameter("@CreateDate", DateTime.Now);
                    objParam[4] = new SqlParameter("@UpdateLast", DateTime.Now);
                    objParam[5] = new SqlParameter("@VerifyDate", DateTime.Now);
                    objParam[6] = new SqlParameter("@VerifyStatus", VerifyStatus.DA_DUYET);
                    objParam[7] = new SqlParameter("@CreatedBy", create_id);

                    return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertUserAgent, objParam);
                }
                else
                {
                    SqlParameter[] objParam = new SqlParameter[6];
                    objParam[0] = new SqlParameter("@Id", Id);
                    objParam[1] = new SqlParameter("@UserId", UserId);
                    objParam[2] = new SqlParameter("@MainFollow", DBNull.Value);
                    objParam[3] = new SqlParameter("@VerifyDate", DateTime.Now);
                    objParam[4] = new SqlParameter("@VerifyStatus", VerifyStatus.DA_DUYET);
                    objParam[5] = new SqlParameter("@UpdateBy", create_id);

                    return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_UpdateUserAgent, objParam);
                }


                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdataUserAgent - UserAgentDAL: " + ex);
                return 0;
            }
        }

        public List<UserAgentViewModel> GeListUserAgentByClient(int ClientId,long id)
        {
            try
            {

                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@ClientId", ClientId > 0 ? ClientId:DBNull.Value);
                objParam[1] = new SqlParameter("@Id", id > 0 ? id : DBNull.Value);
              

                DataTable dt= _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetUserAgentByClientId, objParam);
                if(dt!=null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<UserAgentViewModel>();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UserAgentByClient - UserAgentDAL: " + ex);
                
            }
            return null;
        }
        public UserAgent GetUserAgentClient(int ClientId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var model = _DbContext.UserAgents.FirstOrDefault(s => s.ClientId == ClientId);
                    return model;
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserAgentClient - UserAgentDAL: " + ex);
                return null;
            }
        }
        public int UpdataUserAgentClient(UserAgent model)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var deta = _DbContext.UserAgents.Update(model);
                    _DbContext.SaveChanges();
                    return 1;
                }

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdataUserAgentClient - UserAgentDAL: " + ex);
                return 0;
            }
        }
    }
}
