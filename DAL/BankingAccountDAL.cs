using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Utilities;
using Utilities.Contants;

namespace DAL
{
    public class BankingAccountDAL : GenericService<BankingAccount>
    {
        private static DbWorker _DbWorker;
        public BankingAccountDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public List<BankingAccount> GetAllBankingAccount()
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return _DbContext.BankingAccounts.AsNoTracking().ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAllBankingAccount - BankingAccountDAL: " + ex);
                return null;
            }
        }
        public BankOnePay GetBankOnePayByBankName(string bank_name)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return _DbContext.BankOnePays.FirstOrDefault(s => s.BankName == bank_name);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAllBankingAccount - BankingAccountDAL: " + ex);
                return null;
            }
        }

        public BankingAccount GetById(int bankAccountId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return _DbContext.BankingAccounts.AsNoTracking().FirstOrDefault(n => n.Id == bankAccountId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetById - BankingAccountDAL: " + ex);
                return null;
            }
        }

        public int InsertBankingAccount(BankingAccount model)
        {
            try
            {
                SqlParameter[] objParam_contractPay = new SqlParameter[]
                {
                    new SqlParameter("@BankId", model.BankId ?? (object)DBNull.Value),
                    new SqlParameter("@AccountNumber", model.AccountNumber ?? (object)DBNull.Value),
                    new SqlParameter("@AccountName", model.AccountName ?? (object)DBNull.Value),
                    new SqlParameter("@Branch", model.Branch ?? (object)DBNull.Value),
                    new SqlParameter("@SupplierId",model.SupplierId==null? (object)DBNull.Value : model.SupplierId),
                    new SqlParameter("@ClientId",model.ClientId==null? (object)DBNull.Value: model.ClientId),
                    new SqlParameter("@CreatedBy", model.CreatedBy),
                    new SqlParameter("@CreatedDate",DateTime.Now)

                };
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertBankingAccount, objParam_contractPay);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateSupplier - SupplierDAL. " + ex);
                return -1;
            }
        }

        public int UpdateBankingAccount(BankingAccount model)
        {
            try
            {
                SqlParameter[] objParam_contractPay = new SqlParameter[]
                {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@BankId", model.BankId ?? (object)DBNull.Value),
                    new SqlParameter("@AccountNumber", model.AccountNumber ?? (object)DBNull.Value),
                    new SqlParameter("@AccountName", model.AccountName ?? (object)DBNull.Value),
                    new SqlParameter("@Branch", model.Branch ?? (object)DBNull.Value),
                    new SqlParameter("@SupplierId",model.SupplierId==null? (object)DBNull.Value : model.SupplierId),
                    new SqlParameter("@ClientId",model.ClientId==null? (object)DBNull.Value: model.ClientId),
                    new SqlParameter("@UpdatedBy", model.UpdatedBy)
                };
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdateBankingAccount, objParam_contractPay);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateSupplier - SupplierDAL. " + ex);
                return -1;
            }
        }

        public DataTable GetBankAccountDataTableBySupplierId(int supplier_id)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[]
                {
                    new SqlParameter("@SupplierId", supplier_id)
                };

                return _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetListBankingAccountBySupplierId, objParam);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetBankAccountByClientId(int clientId)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[]
                {
                    new SqlParameter("@ClientId", clientId)
                };
                return _DbWorker.GetDataTable(StoreProcedureConstant.SP_GetListBankingAccountByClientId, objParam);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("BankingAccountDAL - GetBankAccountByClientId: " + ex);
                return null;
            }
        }
    }
}
