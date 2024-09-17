using Entities.Models;
using Entities.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IAccountClientRepository
    {
        long GetMainAccountClientByClientId(long client_id);
        AccountClient AccountClientByClientId(long client_id);
    }
}
