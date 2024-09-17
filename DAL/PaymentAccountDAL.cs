using DAL.Generic;
using Entities.ConfigModels;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DAL
{
   public class PaymentAccountDAL : GenericService<PaymentAccount>
    {
        public PaymentAccountDAL(string connection) : base(connection) { }
        public int CreatePaymentAccount(PaymentAccount model)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                  
                        var deta = _DbContext.PaymentAccounts.Add(model);
                        _DbContext.SaveChanges();
                   

                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CreatePaymentAccount - PaymentAccountDAL: " + ex);
                return 0;
            }
        }
        public  List<PaymentAccount> GetAllByClientId( long clientID)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    var deta = _DbContext.PaymentAccounts.AsNoTracking().Where(s=>s.ClientId==clientID).ToList();

                    return deta;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetAllByClientId - PaymentAccountDAL: " + ex);
                return null;
            }
        }
    
        public int Setup(PaymentAccount model)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    if (model.Id == 0)
                    {
                        var deta = _DbContext.PaymentAccounts.Add(model);
                        _DbContext.SaveChanges();
                    }else
                    {
                        var deta = _DbContext.PaymentAccounts.Update(model);
                        _DbContext.SaveChanges();
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Setup - PaymentAccountDAL: " + ex);
                return 0;
            }
        }
        public int Delete(int Id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    if (Id != 0)
                    {
                        var deleteModel =  _DbContext.PaymentAccounts.FirstOrDefault(s=>s.Id == Id);
                        _DbContext.PaymentAccounts.Remove(deleteModel);
                        _DbContext.SaveChanges();
                    }
                    
                }
                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Delete - PaymentAccountDAL: " + ex);
                return 0;
            }
        }
        public async Task<PaymentAccount> getPaymentAccountById(int Id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var data =await _DbContext.PaymentAccounts.FirstOrDefaultAsync(s => s.Id == Id);
                    return data;
                }
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("getPaymentAccountById - PaymentAccountDAL: " + ex);
                return null;
            }
        }
        
    }
}
