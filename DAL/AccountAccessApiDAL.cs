using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels.AccountAccessAPI;
using Entities.ViewModels.Comment;
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
    public class AccountAccessApiDAL : GenericService<AccountAccessApi>
    {
        private static DbWorker _DbWorker;
        public AccountAccessApiDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public async Task<List<AccountAccessApiViewModel>> GetAllAccountAccessAPI() 
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@PageIndex",-1),
                    new SqlParameter("@PageSize",-1)
                };
            var lstObj = _DbWorker.GetDataTable(ProcedureConstants.SP_GetListAccountAccessAPI, sqlParameters);
            List<AccountAccessApiViewModel> Result = lstObj.Rows.Cast<DataRow>().Select(row => new AccountAccessApiViewModel
            {
                Id = row.Field<int>("Id"),
                Id_AllCode = row.Field<int>("Id_AllCode"),
                Id_AccountAccessAPIPermission = row.Field<int>("Id_AccountAccessAPIPermission"),
                UserName = row.Field<string>("UserName"),
                CodeName = row.Field<string>("CodeName"),
                Status = row.Field<short>("Status"),
                CreateDate = row.Field<DateTime>("CreateDate"),
                UpdateLast = row.Field<DateTime>("UpdateLast"),
                Description = row.Field<string>("Description"),
            }).OrderBy(x => x.Status).ToList();
            return Result;
        }

        public async Task<int> InsertAccountAccessAPI(AccountAccessApiSubmitModel model) 
        {
            try 
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@UserName",!string.IsNullOrEmpty(model.UserName) ? model.UserName : DBNull.Value),
                    new SqlParameter("@Password",!string.IsNullOrEmpty(model.Password) ? PresentationUtils.Encrypt(model.Password) : DBNull.Value),
                    new SqlParameter("@Status",model.Status != null ? model.Status : DBNull.Value),
                    new SqlParameter("@CreateDate",DateTime.Now),
                    new SqlParameter("@UpdateLast",DateTime.Now),
                    new SqlParameter("@Description",!string.IsNullOrEmpty(model.Description) ? model.Description : DBNull.Value)
                };
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_InsertAccountAccessAPI,sqlParameters);
            }
            catch (Exception ex) 
            {
                LogHelper.InsertLogTelegram("InsertAccountAccessAPI - AccountAccessApiDAL: " + ex);
                return -1;
            }
        }

        public async Task<int> UpdateAccountAccessAPI(AccountAccessApiSubmitModel model)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@UserName", !string.IsNullOrEmpty(model.UserName) ? model.UserName : DBNull.Value),
                    new SqlParameter("@Password", !string.IsNullOrEmpty(model.Password)? model.Password : DBNull.Value ),
                    new SqlParameter("@Status", model.Status != null ? model.Status : DBNull.Value),
                    new SqlParameter("@CreateDate",DateTime.Now),
                    new SqlParameter("@UpdateLast",DateTime.Now),
                    new SqlParameter("@Description",!string.IsNullOrEmpty(model.Description) ? model.Description : DBNull.Value)
                };
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_UpdateAccountAccessAPI, sqlParameters);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAccountAccessAPI - AccountAccessApiDAL: " + ex);
                return -1;
            }
        }

        public async Task<int> ResetPassword(int id)
        {
            try
            {
                string password = PresentationUtils.Encrypt("abc123");

                SqlParameter[] sqlParameters = new SqlParameter[]
                 {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@UserName", DBNull.Value),
                    new SqlParameter("@Password",password),
                    new SqlParameter("@Status",DBNull.Value),
                    new SqlParameter("@CreateDate",DBNull.Value),
                    new SqlParameter("@UpdateLast",DateTime.Now),
                    new SqlParameter("@Description",DBNull.Value)
                 };
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_UpdateAccountAccessAPI, sqlParameters);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("ResetPassword - AccountAccessApiDAL: " + ex);
                return -1;
            }
        }
    }
}
