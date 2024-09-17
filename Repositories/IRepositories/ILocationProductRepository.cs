using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface ILocationProductRepository
    {
        public Task<LocationProduct> GetById(int id);
        public Task<List<LocationProduct>> GetByProductCode(string product_code);
        public Task<LocationProduct> SearchIfExists(string product_code, int group_id, int order_no);
        public Task Update(LocationProduct model);
        public Task<long> Addnew(LocationProduct model);
        public Task<long> DeleteAsync(LocationProduct model);
    }
}
