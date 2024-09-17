using Entities.ViewModels;
using Entities.ViewModels.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caching.Elasticsearch
{
    public interface IProductESRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// string StrEsConfig = $"{elasticConfig.Host}:{elasticConfig.Port}";
        /// IESRepository<EsProductViewModel> _ESRepository = new ESRepository<EsProductViewModel>(StrEsConfig);
        /// var id = "0134190440_1";
        /// var Model = _ESRepository.FindById("product", id);
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        TEntity FindById(string indexName, object value, string field_name);

        int UpSert(TEntity entity, string indexName);
        Task<int> UpSertAsync(TEntity entity, string indexName);
        bool DeleteProductByCode(string indexName, string document_id);
    }
}
