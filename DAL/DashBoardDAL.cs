using DAL.StoreProcedure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL
{
    public class DashBoardDAL
    {
        private DbWorker _DbWorker;

        public DashBoardDAL(string connection)
        {
            _DbWorker = new DbWorker(connection);
        }

        public DataTable GetRevenueOrderByDay(DateTime from_date, DateTime to_date, int status, string saler_id)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@FromDate", from_date);
                objParam[1] = new SqlParameter("@ToDate", to_date);
                objParam[2] = new SqlParameter("@Status", status);
                objParam[3] = new SqlParameter("@SalerId", saler_id);
                return _DbWorker.GetDataTable("SP_GetRevenueOrderByDay", objParam);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetNewClientByDay(DateTime from_date, DateTime to_date, string saler_id)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@FromDate", from_date);
                objParam[1] = new SqlParameter("@ToDate", to_date);
                objParam[2] = new SqlParameter("@SalerId", saler_id);
                return _DbWorker.GetDataTable("SP_GetNewClientByDay", objParam);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetRevenueOrderGroupBySale(DateTime from_date, DateTime to_date, int type, string saler_id, int department_id)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[5];
                objParam[0] = new SqlParameter("@FromDate", from_date);
                objParam[1] = new SqlParameter("@ToDate", to_date);
                objParam[2] = new SqlParameter("@Type", type);
                objParam[3] = new SqlParameter("@SalerId", !string.IsNullOrEmpty(saler_id) ? saler_id : (object)DBNull.Value);
                objParam[4] = new SqlParameter("@DepartmentId", department_id > 0 ? department_id : (object)DBNull.Value);
                return _DbWorker.GetDataTable("SP_GetRevenueOrderGroupBySale", objParam);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetOrderDashboard(int user_id, string saler_id)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@UserloginId", user_id);
                objParam[1] = new SqlParameter("@SalerId", saler_id);
                return _DbWorker.GetDataTable("SP_OrderDashboard", objParam);
            }
            catch
            {
                throw;
            }
        }
    }
}
