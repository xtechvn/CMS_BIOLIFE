using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface INationalRepository
    {
        Task<National> GetNationalById(long id);
    }
   
}
