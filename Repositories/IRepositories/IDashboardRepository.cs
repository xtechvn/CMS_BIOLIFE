using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Repositories.IRepositories
{
    public interface IDashboardRepository
    {
        DataTable GetRevenueOrderByDay(DateTime from_date, DateTime to_date, int status);
        DataTable GetNewClientByDay(DateTime from_date, DateTime to_date);
        DataTable GetRevenueOrderGroupBySale(DateTime from_date, DateTime to_date, int type);

        DataTable GetOrderDashboard();
    }
}
