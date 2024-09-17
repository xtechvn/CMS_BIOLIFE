using Entities.Models;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface INoteRepository
    {
        Task<List<NoteViewModel>> GetListByType(long DataId, int Type);
        Task<long> UpSert(Note model);
        Task<long> Delete(long Id, int userLogin);
    }
}
