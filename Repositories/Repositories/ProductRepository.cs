using DAL;
using Entities.ConfigModels;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;

namespace Repositories.Repositories
{
    /// <summary>
    /// Repos này sẽ là lớp BASE của Product
    /// 1. Single responsibility principle: Mỗi đối tượng chỉ làm duy nhất 1 nhiệm vụ
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        // private readonly EsConnection es_repository;

        private readonly GroupProductDAL _GroupProductDAL;
        private readonly IOptions<DataBaseConfig> _DataBaseConfig;

        public ProductRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _DataBaseConfig = dataBaseConfig;

            _GroupProductDAL = new GroupProductDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        
    }
}
