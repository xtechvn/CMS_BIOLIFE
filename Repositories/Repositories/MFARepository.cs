using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class MFARepository : IMFARepository
    {
        private readonly MFADAL _mFADAL;
        public MFARepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _mFADAL = new MFADAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<long> CreateAsync(Mfauser mfa_record)
        {
            try
            {
                return await _mFADAL.CreateAsync(mfa_record);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("CreateAsync - MFARepository: " + ex);
                return -1;
            }
        }
        public async Task<Mfauser> get_MFA_DetailByUserID(long client_id)
        {
            try
            {
                return await _mFADAL.get_MFA_DetailByUserID(client_id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("get_MFA_DetailByUserID - MFARepository: " + ex);
                return null;
            }
        }
        public async Task<bool> UpdateAsync(Mfauser mfa_record)
        {
            try
            {
                await _mFADAL.UpdateAsync(mfa_record);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateAsync - MFARepository: " + ex);
            }
            return false;

        }
    }
}
