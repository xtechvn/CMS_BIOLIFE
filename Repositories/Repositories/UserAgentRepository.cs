using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels.UserAgent;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using Utilities;

namespace Repositories.Repositories
{
    public class UserAgentRepository: IUserAgentRepository
    {
  
        private readonly UserAgentDAL _userAgentDAL;
     

        public UserAgentRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _userAgentDAL=new UserAgentDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }
        public UserAgent GetUserAgentClient(int ClientId)
        {
            try
            {
              
                return _userAgentDAL.GetUserAgentClient(ClientId);
          

            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetUserAgentClient - UserAgentRepository: " + ex);
                return null;
            }
        }
        public List<UserAgentViewModel> UserAgentByClient(int ClientId,long id)
        {
            try
            {
                return _userAgentDAL.GeListUserAgentByClient(ClientId,id);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UserAgentByClient - PaymentAccountRepository: " + ex);
                return null;
            }
        }
        public int UpdataUserAgent(int Id, int UserId, int create_id, long ClientId)
        {
            try
            {

                return _userAgentDAL.UpdataUserAgent(Id, UserId, create_id, ClientId);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdataUserAgent - PaymentAccountRepository: " + ex);
                return 0;
            }
        }
    }
}
