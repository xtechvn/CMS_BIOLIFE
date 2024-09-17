using DAL.Generic;
using DAL.StoreProcedure;
using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.OrderDetail;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace DAL.OrderDetail
{
    public class OrderDetailDAL : GenericService<Entities.Models.OrderDetail>
    {
        private static DbWorker _DbWorker;
        public OrderDetailDAL(string connection) : base(connection)
        {
            _DbWorker = new DbWorker(connection);
        }
        public async Task<List<ListOrderDetailViewModel>> GetListOrderDetail(long OrderId)
        {
            try
            {

                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@OrderId", OrderId);

                DataTable dt = _DbWorker.GetDataTable(ProcedureConstants.SP_GetListOrderDetail, objParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var data = dt.ToList<ListOrderDetailViewModel>();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListOrderDetail - OrderDetailDAL: " + ex);
            }
            return null;
        }
     
    }
}
