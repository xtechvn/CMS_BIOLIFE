using Entities.Models;
using Entities.ViewModels.UserAgent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.IRepositories
{
    public interface IUserAgentRepository
    {
        UserAgent GetUserAgentClient(int ClientId);
        List<UserAgentViewModel> UserAgentByClient(int ClientId,long id);
        int UpdataUserAgent(int Id, int UserId, int create_id,long ClientId);
    }
}
