using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
   public  interface IMFARepository
    {
        public Task<Mfauser> get_MFA_DetailByUserID(long client_id);
        public Task<long> CreateAsync(Mfauser mfa_record);
        public Task<bool> UpdateAsync(Mfauser mfa_record);
    }
}
