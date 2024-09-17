using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.OrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task<GenericViewModel<OrderViewModel>> GetList(OrderViewSearchModel searchModel);
        Task<OrderDetailViewModel> GetOrderDetailByOrderId(long OrderId);
        Task<TotalCountSumOrder> GetTotalCountSumOrder(OrderViewSearchModel searchModel);
        Task<long> UpdateOrder(Order model);
        Task<List<ListOrderDetailViewModel>> GetListOrderDetail(long orderid);
        Task<Order> GetOrderByOrderNo(string orderNo);
        Task<List<OrderViewModel>> GetByClientId(long clientId, int payId = 0, int status = 0);
    }
}
