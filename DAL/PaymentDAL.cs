using DAL.Generic;
using Entities.Models;
using Entities.ViewModels;
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
    public class PaymentDAL : GenericService<Payment>
    {
        public PaymentDAL(string connection) : base(connection)
        {
        }

        public async Task<List<PaymentViewModel>> GetListByOrderId(long OrderId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var data = await (from _payment in _DbContext.Payments.AsNoTracking()
                                      join a in _DbContext.AllCodes.Where(s => s.Type == AllCodeType.PAYMENT_TYPE) on _payment.PaymentType equals a.CodeValue into af
                                      from _paymentType in af.DefaultIfEmpty()
                                      where _payment.OrderId == OrderId
                                      select new PaymentViewModel
                                      {
                                          Id = _payment.Id,
                                          Amount = _payment.Amount,
                                          PaymentDate = _payment.PaymentDate,
                                          PaymentType = _payment.PaymentType,
                                          OrderId = _payment.OrderId,
                                          PaymentTypeName = _paymentType.Description,
                                          //UserId = _payment.UserId,
                                          Note = _payment.Note,
                                          CreatedOn = _payment.CreatedOn,
                                          ModifiedOn = _payment.ModifiedOn
                                          //ProductId = _payment.ProductId,
                                          //ProductCode = _product.ProductCode,
                                      }).ToListAsync();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListByOrderId - PaymentDAL: " + ex);
                return null;
            }
        }

        public async Task<double> GetOrderPaymentAmount(long orderId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Payments.Where(s => s.OrderId == orderId).SumAsync(s => s.Amount);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetOrderPaymentAmount - PaymentDAL: " + ex);
                return 0;
            }
        }

        public async Task<Payment> GetFirstPaymentOrder(long orderId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Payments.Where(s => s.OrderId == orderId).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetFirstPaymentOrder - PaymentDAL: " + ex);
                return null;
            }
        }

        public async Task<List<Payment>> GetByDepositHistoryIds(List<int> depositHistoryIds)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Payments.Where(s => depositHistoryIds.Contains((int)s.OrderId) && s.DepositPaymentType == (int)DepositHistoryConstant.DEPOSIT_PAYMENT_TYPE.NAP_QUY).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetByDepositHistoryIds - PaymentDAL: " + ex);
                return new List<Payment>();
            }
        }
        public List<Payment> GetPaymentClientId(long cilentId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var data = _DbContext.Payments.Where(s => s.ClientId == cilentId).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPaymentClientId - PaymentDAL: " + ex);
                return null;
            }
        }
        public Payment GetPaymentDateClientId(long cilentId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var data = _DbContext.Payments.OrderByDescending(x => x.CreatedOn).ToList();
                    return data[0];
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPaymentDateClientId - PaymentDAL: " + ex);
                return null;
            }
        }
    }
}
