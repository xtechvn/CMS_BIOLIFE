using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
   public class AccountClientDAL : GenericService<AccountClient>
    {
        private static DbWorker _DbWorker;
        public AccountClientDAL(string connection) : base(connection) {
            _DbWorker = new DbWorker(connection);
        }
        public int CreateAccountClient(AccountClient model)
        {
            try
            {
                    SqlParameter[] objParam = new SqlParameter[9];
                    objParam[0] = new SqlParameter("@ClientId ", model.ClientId);
                    objParam[1] = new SqlParameter("@ClientType", model.ClientType);
                    objParam[2] = new SqlParameter("@UserName", model.UserName);
                    objParam[3] = new SqlParameter("@Password", model.Password);
                    objParam[4] = new SqlParameter("@PasswordBackup", model.PasswordBackup);
                    objParam[5] = new SqlParameter("@ForgotPasswordToken", model.ForgotPasswordToken);
                    objParam[6] = new SqlParameter("@Status", model.Status);
                    objParam[7] = new SqlParameter("@GroupPermission", model.GroupPermission);
                    objParam[8] = new SqlParameter("@GoogleToken", model.GoogleToken);

                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_InsertAccountClient, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CreateAccountClient - AccountClientDAL: " + ex);
                return 0;
            }
        }
        public long GetMainAccountClientByClientId(long client_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    var main_account =  _DbContext.AccountClients.FirstOrDefault(x => x.ClientId == client_id);
                    if (main_account != null)
                    {
                        return main_account.Id;
                    }


                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetMainAccountClientByClientId - AccountClientDAL: " + ex.ToString());

            }
            return -1;

        }
        public AccountClient AccountClientByClientId(long client_id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    var main_account = _DbContext.AccountClients.FirstOrDefault(x => x.ClientId == client_id);
                    if (main_account != null)
                    {
                        return main_account;
                    }


                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("AccountClientByClientId - AccountClientDAL: " + ex.ToString());

            }
            return null;

        }
        public async Task<int> UpdataAccountClient(AccountClient model)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[10];
                objParam[0] = new SqlParameter("@Id ", model.Id);
                objParam[1] = new SqlParameter("@ClientId ", model.ClientId);
                objParam[2] = new SqlParameter("@ClientType", model.ClientType);
                objParam[3] = new SqlParameter("@UserName", model.UserName);
                objParam[4] = new SqlParameter("@Password", model.Password);
                objParam[5] = new SqlParameter("@PasswordBackup", model.PasswordBackup);
                objParam[6] = new SqlParameter("@ForgotPasswordToken", model.ForgotPasswordToken);
                objParam[7] = new SqlParameter("@Status", model.Status);
                objParam[8] = new SqlParameter("@GroupPermission", model.GroupPermission);
                objParam[9] = new SqlParameter("@GoogleToken", model.GoogleToken);

                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_UpdateAccountClient, objParam);    
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdataAccountClient - AccountClientDAL: " + ex.ToString());

            }
            return 0;

        }
        
    }
}
