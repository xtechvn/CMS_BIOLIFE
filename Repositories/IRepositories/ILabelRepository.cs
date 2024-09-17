using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Repositories.IRepositories
{
    public interface ILabelRepository
    {
        List<Label> GetAll();
        List<Label> GetListAll();
        Task<Label> GetById(int Id);
        Task<List<Label>> GetLabelActive();
        Task<int> Create(Label model);
        Task<int> Update(Label model);
        Task<int> Delete(int id);
    }
}
