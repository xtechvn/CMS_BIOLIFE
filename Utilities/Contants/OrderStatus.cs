using System.ComponentModel;

namespace Utilities.Contants
{
    // Trạng thái đơn
    public enum OrderStatus
    {
        /// <summary>
        /// Mặc định trạng thái đơn khi được khởi tạo
        /// </summary>
        [Description("Chờ thanh toán")]
        CREATED_ORDER = 0,

        [Description("Đang xử lý")]
        CONFIRMED_SALE = 1,


        [Description("Đang giao hàng")]
        WAITING_FOR_OPERATOR = 2,

 
        [Description("Hoàn thành")]
        FINISHED = 3,
      
        [Description("Đã hủy")]
        CANCEL = 4,
       

    }

    // Trạng thái đơn
    public enum OrderĐebtStatus
    {
        /// <summary>
        /// Đã gạch nợ đủ cho đơn hàng
        /// </summary>
        [Description("Gạch nợ đủ")]
        PAID_ENOUGH = 1,
        /// <summary>
        /// Chưa đã gạch nợ đủ cho đơn hàng
        /// </summary>
        [Description("Gạch nợ chưa đủ")]
        PAID_NOT_ENOUGH = 2,

    }
}
