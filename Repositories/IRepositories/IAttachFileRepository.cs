using Entities.Models;
using Entities.ViewModels.Attachment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IAttachFileRepository
    {
        Task<List<AttachFile>> GetListByType(long DataId, int type);
        Task<long> Delete(long Id, int userLogin);
        Task<List<object>> CreateMultiple(List<AttachFile> models);
        Task<int> UpdateAttachFile(AttachFile attachFile);
        Task<int> SaveAttachFileURL(List<AttachfileViewModel> attachFile, long data_id, int user_summit, int service_type);
        long DeleteAttachFilesByDataId(long Id, int type);
        Task<long> AddAttachFile(AttachFile attachFile);
        Task<List<AttachFile>> GetListByDataID(long dataId, int type);
        Task<int> DeleteNonExistsAttachFile(List<long> remain_ids, long data_id, int service_type);
    }
}
