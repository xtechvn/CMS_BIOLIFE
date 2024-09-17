using DAL;
using Entities.ConfigModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using Repositories.Repositories.BaseRepos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Repositories.Repositories
{
    public class DashboardRepository : BaseRepository, IDashboardRepository
    {
        private readonly DashBoardDAL _DashBoardDAL;
        public DashboardRepository(IHttpContextAccessor context, IOptions<DataBaseConfig> dataBaseConfig, IUserRepository userRepository, IConfiguration configuration) : base(context, dataBaseConfig, configuration, userRepository)
        {
            _DashBoardDAL = new DashBoardDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public DataTable GetNewClientByDay(DateTime from_date, DateTime to_date)
        {
            try
            {
                return _DashBoardDAL.GetNewClientByDay(from_date, to_date, _SysUserModel.UserUnderList);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetOrderDashboard()
        {
            try
            {
                return _DashBoardDAL.GetOrderDashboard(_SysUserModel.Id, _SysUserModel.UserUnderList);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetRevenueOrderByDay(DateTime from_date, DateTime to_date, int status)
        {
            try
            {
                return _DashBoardDAL.GetRevenueOrderByDay(from_date, to_date, status, _SysUserModel.UserUnderList);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetRevenueOrderGroupBySale(DateTime from_date, DateTime to_date, int type)
        {
            try
            {
                // _SysUserModel.UserUnderList
                return _DashBoardDAL.GetRevenueOrderGroupBySale(from_date, to_date, type, string.Empty, _SysUserModel.DepartmentId);
            }
            catch
            {
                throw;
            }
        }
    }
}
