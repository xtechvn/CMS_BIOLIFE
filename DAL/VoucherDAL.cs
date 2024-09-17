using DAL.Generic;
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
    public class VoucherDAL : GenericService<Voucher>
    {
        public VoucherDAL(string connection) : base(connection)
        {
        }

        public async Task<Voucher> FindByVoucherCode(string voucherCode)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Vouchers.FirstOrDefaultAsync(s => s.Code.ToUpper() == voucherCode.ToUpper());
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("FindByVoucherCode - VoucherDAL: " + ex);
                return null;
            }
        }

        public async Task<Voucher> FindByVoucherCode(string voucherCode, bool is_public = false)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Vouchers.FirstOrDefaultAsync(s => s.Code.ToUpper() == voucherCode.ToUpper() && s.IsPublic == is_public);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("FindByVoucherCode - VoucherDAL: " + ex);
                return null;
            }
        }

        public async Task<List<Voucher>> getVoucherPublic(bool is_public)
        {
            try
            {
                var current_date = DateTime.Now;
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Vouchers.AsNoTracking().Where(s => s.IsPublic == is_public && s.Cdate <= current_date && s.EDate >= current_date).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("getVoucherPublic - VoucherDAL: " + ex);
                return null;
            }
        }
        public async Task<string> FindByVoucherid(int voucherId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {

                    var Voucher = await _DbContext.Vouchers.FirstOrDefaultAsync(s => s.Id == voucherId);
                    return Voucher.Code;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("FindByVoucherCode - VoucherDAL: " + ex.ToString());
                return null;
            }
        }
    }
}
