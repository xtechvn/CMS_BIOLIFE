using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels.AccountAccessApiPermission;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class AccountAccessApiPermissionDAL : GenericService<AccountAccessApiPermission>
    {
        private static DbWorker _DbWorker;
        public AccountAccessApiPermissionDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public async Task<int> InsertAccountAccessApiPermission(AAAPSubmitModel model ) 
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@AccountAccessApiId",model.AccountAccessApiId != null ? model.AccountAccessApiId : DBNull.Value),
                    new SqlParameter("@ProjectType",model.ProjectType != null ? model.ProjectType : DBNull.Value)

                };
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_InsertAccountAccessAPIPermission, sqlParameters);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("InsertAccountAccessAPI - AccountAccessApiDAL: " + ex);
                return -1;
            }
        }

        public async Task<int> UpdateAccountAccessApiPermission(AAAPSubmitModel model)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@Id",model.Id != null ? model.Id : DBNull.Value),
                    new SqlParameter("@AccountAccessApiId",model.AccountAccessApiId != null ? model.AccountAccessApiId : DBNull.Value),
                    new SqlParameter("@ProjectType",model.ProjectType != null ? model.ProjectType : DBNull.Value)

                };
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.sp_UpdateAccountAccessAPIPermission, sqlParameters);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("InsertAccountAccessAPI - AccountAccessApiDAL: " + ex);
                return -1;
            }
        }
    }
}
