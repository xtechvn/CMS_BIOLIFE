using Entities.Models;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IPositionRepository
    {
        Task<List<Position>> GetAll();
        List<Position> GetListAll();
        Task<Position> GetById(int Id);
        Task<Position> GetByPositionName(string positionName);
        Task<int> Create(Position model);
        Task<int> Update(Position model);
        Task<int> Delete(int id);
    }
}
