using Entities.Models;
using Entities.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IAllCodeRepository
    {
        List<AllCode> GetAll();
        Task<List<AllCode>> GetAllCodeAsync();
        List<AllCode> GetListAllAsync();
        Task<AllCode> GetById(int Id);
        Task<long> Create(AllCode model);
        Task<long> Update(AllCode model);
        Task<long> Delete(int id);
        List<AllCode> GetListByType(string type);
        Task<List<AllCode>> GetListByCodeValueAsync(int codevalue);
        List<BankingAccount> GetBankingAccounts();
        List<BankingAccount> GetBankingAccountsBySupplierId(int supplierId);
        AllCode GetByType(string type);
        Task<short> GetLastestCodeValueByType(string type);
        Task<short> GetLastestOrderNoByType(string type);
        Task<AllCode> GetIDIfValueExists(string type, string description);
        Task<List<AllCode>> GetListSortByName(string type_name);
        Task<T> GetAllCodeValueByType<T>(string apiPrefix, string keyToken, string key, string type);
        Task<T> Sendata<T>(string apiPrefix, string keyToken, Dictionary<string,string> keyValuePairs);
        BankOnePay GetBankOnePayByBankName(string BankName);
        Task<List<AllCode>> GetAllSortByIDAndType(int id , string type);
    }
}
