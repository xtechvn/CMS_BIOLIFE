var totalCustomer = 0;
var totalOrder = 0;
$(document).ready(function () {
    setTimeout(function () {
        menu.GetTotalCustomerInday();
    }, 200)
    setTimeout(function () {
        menu.GetRevenuToday();
    }, 500)
});
var menu = {
    GetTotalCustomerInday: function () {
        $.ajax({
            url: "/DashBoard/GetCustomerInDay",
            type: "post",
            success: function (result) {
                $('#customerInDay').html(result.data + " khách hàng mới");
                totalCustomer = result.data
            }
        });
    },
    GetRevenuToday: function () {
        $.ajax({
            url: "/DashBoard/GetRevenuToday",
            type: "post",
            success: function (result) {
                $('#totalOrder').html(result.data.totalOrder + " đơn hàng");
                totalOrder = result.data.totalOrder
                $('#totalMoney').html(result.data.revenueStr);
            }
        });
    },
    ViewCustomer: function () {
        if (totalCustomer <= 0) {
            _msgalert.error('Hiện chưa có khách hàng nào mới');
            return;
        }
        var todayTime = new Date();
        var month = (todayTime.getMonth() + 1);
        var day = (todayTime.getDate());
        var year = (todayTime.getFullYear());
        window.location.href = "/Client?date=" + day + "-" + month + "-" + year;
    },
    ViewOrder: function () {
        if (totalOrder <= 0) {
            _msgalert.error('Hiện chưa có đơn hàng nào mới');
            return;
        }
        var todayTime = new Date();
        var month = (todayTime.getMonth() + 1);
        var day = (todayTime.getDate());
        var year = (todayTime.getFullYear());
        window.location.href = "/Order?date=" + day + "-" + month + "-" + year;
    },
};










