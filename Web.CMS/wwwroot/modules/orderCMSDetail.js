let input = $('#order_Id').val();
let type = 7;
$(document).ready(function () {
   
    _orderDetail.LoadPackages(input);
    _orderDetail.LoadContractPay(input);
    _orderDetail.LoadBillVAT(input);
    _orderDetail.LoadFile(input, type);
    _orderDetail.LoadPersonInCharge(input);
});
var _orderDetail = {
    LoadOeederDetail: function () {
        _orderDetail.LoadPackages(input);
        _orderDetail.LoadContractPay(input);
        _orderDetail.LoadBillVAT(input);
        _orderDetail.LoadFile(input, type);
        _orderDetail.LoadPersonInCharge(input);
    },

    LoadPackages: function (input) {
        $.ajax({
            url: "/Order/Packages",
            type: "Post",
            data: { orderId: input},
            success: function (result) {
                $('#imgLoading_Packages').hide();
                $('#grid_data_Packages').html(result);
            }
        });
    },
    LoadContractPay: function (input) {
        $.ajax({
            url: "/Order/ContractPay",
            type: "Post",
            data: { orderId: input },
            success: function (result) {
                $('#imgLoading_ContractPay').hide();
                $('#grid_data_ContractPay').html(result);
            }
        });
    },
    LoadBillVAT: function (input) {
        $.ajax({
            url: "/Order/BillVAT",
            type: "Post",
            data: { orderId: input },
            success: function (result) {
                $('#imgLoading_BillVAT').hide();
                $('#grid_data_BillVAT').html(result);
            }
        });
    },
    LoadListPassenger: function (input) {
        $.ajax({
            url: "/Order/ListPassenger",
            type: "Post",
            data: { orderId: input },
            success: function (result) {
                $('#imgLoading_ListPassenger').hide();
                $('#grid_data_ListPassenger').html(result);
            }
        });
    },
    LoadFile: function (input, type) {
        _global_function.RenderFileAttachment($('#grid_data_File'), input,type)
        $('#imgLoading_File').hide();
        /*
        $.ajax({
            url: "/Order/File",
            type: "Post",
            data: { orderId: input, type: type },
            success: function (result) {
                $('#imgLoading_File').hide();
                $('#grid_data_File').html(result);
            }
        });
        */
    },
    LoadPersonInCharge: function (input) {
        $.ajax({
            url: "/Order/PersonInCharge",
            type: "Post",
            data: { orderId: input },
            success: function (result) {
                $('#imgLoading_PersonInCharge').hide();
                $('#grid_data_PersonInCharge').html(result);
            }
        });
    },
    ChangeOrderSaler: function (order_id, order_no) {
       
        var title = 'Nhận xử lý đơn hàng';
        var description = 'Bạn có chắc chắn muốn nhận xử lý đơn hàng này không?';
        _msgconfirm.openDialog(title, description, function () {
            $.ajax({
                url: "/Order/ChangeOrderSaler",
                type: "Post",
                data: { order_id: order_id, saleid: 0, OrderNo: order_no},
                success: function (result) {
                    if (result.status === 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 1000);
                    }
                    else {
                        _msgalert.error(result.msg);

                    }
                }
            });
        });
    },

}
