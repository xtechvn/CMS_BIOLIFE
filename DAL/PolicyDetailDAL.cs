using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels.Policy;
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
    public class PolicyDetailDAL : GenericService<PolicyDetail>
    {
        private static DbWorker _DbWorker;
        public PolicyDetailDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }
        public PolicyDetail GetPolicyDetailByType(int ClientType, int PermisionType)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var Policy = _DbContext.Policies.Where(n => (DateTime)n.EffectiveDate <= DateTime.Now && n.PermissionType == PermisionType && n.IsPrivate==false && n.IsDelete == false).OrderByDescending(s=>s.CreatedDate).FirstOrDefault();
                    var Policy_Detail = _DbContext.PolicyDetails.FirstOrDefault(n => n.PolicyId == Policy.PolicyId && n.ClientType == ClientType);

                    if (Policy_Detail != null)
                    {
                        return Policy_Detail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPolicyDetailByType - PolicyDetailDAL. " + ex);
                return null;
            }
        }

        public async Task<int> InsertPolicyDetail(AddPolicyDtailViewModel model,long PolicyId)
        {
            try
            {
                if(model.extra_policy!=null)
                foreach(var item in model.extra_policy)
                {
                        if (item.DebtType == null) { item.DebtType = "0"; }
                    SqlParameter[] objParam = new SqlParameter[17];
                    objParam[0] = new SqlParameter("@PolicyId", PolicyId);
                    objParam[1] = new SqlParameter("@ClientType", item.ClientType);
                    objParam[2] = new SqlParameter("@DebtType", item.DebtType);
                    objParam[3] = new SqlParameter("@ProductFlyTicketDebtAmount", item.ProductFlyTicketDebtAmount);
                    objParam[4] = new SqlParameter("@HotelDebtAmout", item.HotelDebtAmout);
                    objParam[5] = new SqlParameter("@ProductFlyTicketDepositAmount", item.ProductFlyTicketDepositAmount);
                    objParam[6] = new SqlParameter("@HotelDepositAmout", item.HotelDepositAmout);
                    objParam[7] = new SqlParameter("@CreatedBy", model.CreatedBy);
                    objParam[8] = new SqlParameter("@CreatedDate", "");
                    objParam[9] = new SqlParameter("@UpdatedBy", "");
                    objParam[10] = new SqlParameter("@UpdatedDate", "");
                    objParam[11] = new SqlParameter("@VinWonderDebtAmount", item.VinWonderDebtAmount);
                    objParam[12] = new SqlParameter("@TourDebtAmount", item.TourDebtAmount);
                    objParam[13] = new SqlParameter("@TouringCarDebtAmount", item.TouringCarDebtAmount);
                    objParam[14] = new SqlParameter("@VinWonderDepositAmount", item.VinWonderDepositAmount);
                    objParam[15] = new SqlParameter("@TourDepositAmount", item.TourDepositAmount);
                    objParam[16] = new SqlParameter("@TouringCarDepositAmount", item.TouringCarDepositAmount);   
                        _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_InsertPolicyDetail, objParam);
                }
                
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CreatePolicy - PolicyDal: " + ex);
            }
            return 0;
        }
        public async Task<int> UpdatePolicyDetail(PolicyDetail model)
        {
            try
            {
                if (model.ProductFlyTicketDebtAmount == null) { model.ProductFlyTicketDebtAmount = 0; }
                if (model.HotelDebtAmout == null) { model.HotelDebtAmout = 0; }
                if (model.ProductFlyTicketDepositAmount == null) { model.ProductFlyTicketDepositAmount = 0; }
                if (model.HotelDepositAmout == null) { model.HotelDepositAmout = 0; }

                if (model.VinWonderDebtAmount == null) { model.VinWonderDebtAmount = 0; }
                if (model.TourDebtAmount == null) { model.TourDebtAmount = 0; }
                if (model.TouringCarDebtAmount == null) { model.TouringCarDebtAmount = 0; }
                if (model.VinWonderDepositAmount == null) { model.VinWonderDepositAmount = 0; }
                if (model.TourDepositAmount == null) { model.TourDepositAmount = 0; }
                if (model.TouringCarDepositAmount == null) { model.TouringCarDepositAmount = 0; }
                
                        SqlParameter[] objParam = new SqlParameter[16];
                        objParam[0] = new SqlParameter("@Id", model.Id);
                        objParam[1] = new SqlParameter("@PolicyId", model.PolicyId);
                        objParam[2] = new SqlParameter("@ClientType", model.ClientType);
                        objParam[3] = new SqlParameter("@DebtType", model.DebtType);
                        objParam[4] = new SqlParameter("@ProductFlyTicketDebtAmount", model.ProductFlyTicketDebtAmount);
                        objParam[5] = new SqlParameter("@HotelDebtAmout", model.HotelDebtAmout);
                        objParam[6] = new SqlParameter("@ProductFlyTicketDepositAmount", model.ProductFlyTicketDepositAmount);
                        objParam[7] = new SqlParameter("@HotelDepositAmout", model.HotelDepositAmout);
                        objParam[8] = new SqlParameter("@UpdatedBy", model.UpdatedBy);
                        objParam[9] = new SqlParameter("@UpdatedDate", "");
                        objParam[10] = new SqlParameter("@VinWonderDebtAmount", model.VinWonderDebtAmount);
                        objParam[11] = new SqlParameter("@TourDebtAmount", model.TourDebtAmount);
                        objParam[12] = new SqlParameter("@TouringCarDebtAmount", model.TouringCarDebtAmount);
                        objParam[13] = new SqlParameter("@VinWonderDepositAmount", model.VinWonderDepositAmount);
                        objParam[14] = new SqlParameter("@TourDepositAmount", model.TourDepositAmount);
                        objParam[15] = new SqlParameter("@TouringCarDepositAmount", model.TouringCarDepositAmount);
                return _DbWorker.ExecuteNonQuery(StoreProcedureConstant.SP_UpdatePolicyDetail, objParam); 
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdatePolicyDetail - PolicyDal: " + ex);
            }
            return 0;
        }
    }
}
