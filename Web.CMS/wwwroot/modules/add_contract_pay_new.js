var listContractPayDetail = []
var listServiceRefundDetail = []
var listDetail = []
var amountEdit = 0
var isSetInput = false
var isEdit = false
let isPickerCreateAddContract = false;
let contractpay_type_order = 1
let contractpay_type_deposit = 2
let contractpay_type_supplier_refund = 3
let contractpay_type_supplier_commision = 4
let contractpay_type_other = 5
let object_type = 1
var bankingAccountId = 0
var listBankAccount = []
var totalNeedPaymentOrder = 0
var is_admin = false
var _contract_pay_create_new = {
    Initialization: function () {
        $('#divSupplier').hide()
        $('#divCustomer').show()
        $('#divEmployee').hide()
        $('#partner_choose_type').val(0)
        $('#partner_choose_type').attr('disabled', true)
        $('#partner_choose_type').addClass('background-disabled')
        $('#lblBankAccountRequired').show()
        $('#lblBankAccount').hide()
        $('input').attr('autocomplete', 'off');
        if ($('#is_admin').val() == '1')
            is_admin = true
        $("#client-select").select2({
            theme: 'bootstrap4',
            placeholder: "Tên KH, Điện Thoại, Email",
            hintText: "Nhập từ khóa tìm kiếm",
            searchingText: "Đang tìm kiếm...",
            maximumSelectionLength: 1,
            ajax: {
                url: "/CustomerManager/ClientSuggestion",
                type: "post",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        txt_search: params.term,
                    }
                    return query;
                },
                processResults: function (response) {
                    return {
                        results: $.map(response.data, function (item) {
                            return {
                                text: item.clientname + ' - ' + item.email + ' - ' + item.phone,
                                id: item.id,
                            }
                        })
                    };
                },
                cache: true
            }
        });

        $("#supplier-select").select2({
            theme: 'bootstrap4',
            placeholder: "Tên NCC, Điện Thoại, Email",
            hintText: "Nhập từ khóa tìm kiếm",
            searchingText: "Đang tìm kiếm...",
            maximumSelectionLength: 1,
            ajax: {
                url: "/PaymentRequest/GetSuppliersSuggest",
                type: "post",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        name: params.term,
                    }
                    return query;
                },
                processResults: function (response) {
                    return {
                        results: $.map(response, function (item) {
                            return {
                                text: item.name,
                                id: item.id,
                            }
                        })
                    };
                },
                cache: true
            }
        });

        $("#createdBy").select2({
            theme: 'bootstrap4',
            placeholder: "Tên nhân viên",
            hintText: "Nhập từ khóa tìm kiếm",
            searchingText: "Đang tìm kiếm...",
            maximumSelectionLength: 1,
            ajax: {
                url: "/PaymentRequest/GetUserSuggestionList",
                type: "post",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        name: params.term,
                    }
                    // Query parameters will be ?search=[term]&type=public
                    return query;
                },
                processResults: function (response) {
                    return {
                        results: $.map(response, function (item) {
                            return {
                                text: item.fullname,
                                id: item.id,
                            }
                        })
                    };
                },
                cache: true
            }
        });

        $("#orderCodeFilter").select2({
            theme: 'bootstrap4',
            placeholder: "Mã phiếu",
            hintText: "Nhập từ khóa tìm kiếm",
            searchingText: "Đang tìm kiếm...",
            closeOnSelect: false,
            ajax: {
                url: "/Receipt/GetListFilter",
                type: "post",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        jsonData: JSON.stringify(listContractPayDetail),
                        text: params.term,
                    }
                    return query;
                },
                processResults: function (response) {
                    return {
                        results: $.map(response.data, function (item) {
                            return {
                                text: item.orderCode,
                                id: item.orderId,
                            }
                        })
                    };
                },
                cache: true
            }
        });

        $("#serviceCodeFilter").select2({
            theme: 'bootstrap4',
            placeholder: "Tìm kiếm và chọn dịch vụ",
            hintText: "Nhập từ khóa tìm kiếm",
            searchingText: "Đang tìm kiếm...",
            closeOnSelect: false,
            ajax: {
                url: "/Receipt/GetListServiceFilter",
                type: "post",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        jsonData: JSON.stringify(listServiceRefundDetail),
                        text: params.term,
                    }
                    return query;
                },
                processResults: function (response) {
                    return {
                        results: $.map(response.data, function (item) {
                            return {
                                text: item.serviceCode,
                                id: item.serviceId,
                            }
                        })
                    };
                },
                cache: true
            }
        });

        $("#serviceCodeCommissionFilter").select2({
            theme: 'bootstrap4',
            placeholder: "Tìm kiếm và chọn dịch vụ",
            hintText: "Nhập từ khóa tìm kiếm",
            searchingText: "Đang tìm kiếm...",
            closeOnSelect: false,
            ajax: {
                url: "/Receipt/GetListServiceFilter",
                type: "post",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        jsonData: JSON.stringify(listServiceRefundDetail),
                        text: params.term,
                    }
                    return query;
                },
                processResults: function (response) {
                    return {
                        results: $.map(response.data, function (item) {
                            return {
                                text: item.serviceCode,
                                id: item.serviceId,
                            }
                        })
                    };
                },
                cache: true
            }
        });
    },
    FormatNumber: function () {
        var amount = $('#amount').val()
        $('#amount').val(amount.replaceAll(',', '.').replaceAll(',', ''))
        var n = parseFloat($('#amount').val().replace(/\D/g, ''), 10);
        $('#amount').val(isNaN(n) === true ? '' : n.toLocaleString().replaceAll('.', ','));
    },
    FormatNumberStr: function (amount) {
        var n = parseFloat(amount, 10);
        return isNaN(n) === true ? '' : n.toLocaleString().replaceAll('.', ',')
    },
    FormatNumberOrder: function (i) {
        var amount = $('#amount_order_' + i).val()
        $('#amount_order_' + i).val(amount.replaceAll(',', '.').replaceAll(',', ''))
        var n = parseFloat($('#amount_order_' + i).val().replace(/\D/g, ''), 10);
        $('#amount_order_' + i).val(isNaN(n) === true ? '' : n.toLocaleString().replaceAll('.', ','));
    },
    FormatNumberService: function (value) {
        var id = value.id
        var serviceCode = id.replace('amount_service_', '')
        var amount = $('#amount_service_' + serviceCode).val()
        $('#amount_service_' + serviceCode).val(amount.replaceAll(',', '.').replaceAll(',', ''))
        var n = parseFloat($('#amount_service_' + serviceCode).val().replace(/\D/g, ''), 10);
        $('#amount_service_' + serviceCode).val(isNaN(n) === true ? '' : n.toLocaleString().replaceAll('.', ','));
    },
    UpdateAmount: function (index, orderId) {
        var totalAmount = 0
        for (var i = 0; i < listContractPayDetail.length; i++) {
            if (listContractPayDetail[i].orderId == orderId) {
                var amount_input = parseFloat($('#amount_order_' + listContractPayDetail[i].orderCode).val().replaceAll('.', '').replaceAll(',', ''))
                listContractPayDetail[i].amount = amount_input
                if (amount_input > listContractPayDetail[i].totalNeedPayment)
                    listContractPayDetail[i].amount = listContractPayDetail[i].totalNeedPayment
            }
            if (listContractPayDetail[i].amount !== undefined && listContractPayDetail[i].amount !== null
                && listContractPayDetail[i].amount) {
                var checked = $('#order_ckb_' + listContractPayDetail[i].orderCode).is(":checked")
                if (checked) {
                    totalAmount += listContractPayDetail[i].amount
                }
                if (i == index) {
                    $('#amount_order_' + listContractPayDetail[i].orderCode).val(_contract_pay_create_new.FormatNumberStr(listContractPayDetail[i].amount))
                }
            }
        }
        $('#total_amount_need_pay').html(_contract_pay_create_new.FormatNumberStr(totalAmount))
        //var amount = $('#amount').val()
        if (!is_admin) {
            if (totalAmount > parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))) {
                _msgalert.error(' Tổng tiền cần giải trừ không được lớn hơn số tiền của phiếu thu');
            }
        }
    },
    UpdateAmountService: function (serviceId) {
        var totalAmount = 0
        for (var i = 0; i < listServiceRefundDetail.length; i++) {
            var checked = $('#service_ckb_' + listServiceRefundDetail[i].serviceCode).is(":checked")
            if (checked) {
                listServiceRefundDetail[i].amount = parseFloat($('#amount_service_' + listServiceRefundDetail[i].serviceCode).val().replaceAll('.', '').replaceAll(',', ''))
                totalAmount += listServiceRefundDetail[i].amount
            }
        }
        $('#total_amount_need_pay').html(_contract_pay_create_new.FormatNumberStr(totalAmount))
        $('#amount').val(_contract_pay_create_new.FormatNumberStr(totalAmount))
    },
    GetDataByClientId: function (clientId, isEdit = false) {
        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_other) return
        if (clientId == null || clientId == undefined || clientId == '') {
            var clientId = $('#client-select').val()[0]
            //if (client !== null && client !== undefined && client !== '') {
            //    clientId = client[0]
            //}
        }
        /*_contract_pay_create_new.GetListBankAccountAdavigo()*/
        if (contract_type === '1') {
            _contract_pay_create_new.GetOrderListByClientId(clientId, isEdit);
        }
        if (contract_type === '2') {
            _contract_pay_create_new.GetDepositListByClientId(clientId);
        }
    },
    GetDataBySupplierId: function (supplierId, isEdit = false) {
        var contract_type = $('#contract-type').val()
        if (supplierId == null || supplierId == undefined || supplierId == '') {
            var supplier = $('#supplier-select').val()
            if (supplier !== null && supplier !== undefined && supplier !== '') {
                supplierId = supplier[0]
            }
        }
        _contract_pay_create_new.GetListBankAccountAdavigo(supplierId)
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_other) return
        if (parseInt(contract_type) === 3) {
            _contract_pay_create_new.GetServiceListBySupplierId(supplierId, isEdit);
        }
        if (parseInt(contract_type) === 4) {
            _contract_pay_create_new.GetServiceListBySupplierId(supplierId, isEdit, true);
        }
    },
    GetServiceListBySupplierId: function (supplierId, isEdit = false, isCommission = false) {
        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_other) return
        _global_function.AddLoading()
        $.ajax({
            url: "/Receipt/GetServiceListBySupplierId",
            type: "Post",
            data: {
                'supplierId': supplierId, 'contractPayId': $('#payId').val()
            },
            success: function (result) {
                _global_function.RemoveLoading()
                listServiceRefundDetail = result.data
                if (isCommission) {
                    _contract_pay_create_new.GenderSupplierCommissionTable(result, isEdit, true)
                } else {
                    _contract_pay_create_new.GenderSupplierRefundTable(result, isEdit)
                }
            }
        });
    },
    GenderSupplierRefundTable: function (result, isEdit) {
        var totalAmount = 0
        var totalDisarmed = 0
        var totalNeedPayment = 0
        var totalPayment = 0
        $("#body_supplier_refund_list").empty();
        for (var i = 0; i < result.data.length; i++) {
            var urlService = ''
            if (result.data[i].serviceType == 1) { //Khách sạn
                urlService = "/SetService/VerifyHotelServiceDetai/" + result.data[i].serviceId
            }
            if (result.data[i].serviceType == 3) { // vé máy bay
                urlService = "/SetService/fly/detail/" + result.data[i].groupBookingId
            }
            if (result.data[i].serviceType == 5) { //tour
                urlService = "/SetService/Tour/Detail/" + result.data[i].serviceId
            }
            if (result.data[i].serviceType == 9) { //other
                urlService = "/SetService/Others/Detail/" + result.data[i].serviceId
            }
            $('#supplier-refund-relate-table').find('tbody').append(
                "<tr id='service_" + i + "'>" +
                "<td>" +
                "<label class='check-list number'>" +
                " <input type='checkbox' id='service_ckb_" + result.data[i].serviceCode + "' name='service_ckb' onclick='_contract_pay_create_new.OnCheckBoxService(" + i + ")'>" +
                " <span class='checkmark'></span>" + (i + 1) +
                "  </label>" +
                "<td>" +
                " <a class='blue' href='" + urlService + "'> " + result.data[i].serviceCode + " </a>"
                + "</td>" +
                "<td>" + result.data[i].startDateStr + " - " + result.data[i].endDateStr + "</td>" +
                "<td>" +
                " <a class='blue' href='/Order/Orderdetails?id=" + result.data[i].orderId + "'> " + result.data[i].orderNo + " </a>"
                + "</td>" +
                "<td  >" + result.data[i].salerName + "</td>" +
                "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(result.data[i].totalAmount) + "</td>" +
                "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(result.data[i].totalDisarmed) + "</td>" +
                "<td class='text-right'>" + _contract_pay_create_new.FormatNumberStr(result.data[i].totalNeedPayment) + "</td>" +
                "<td class='text-right' >" + "<input type='text' id='amount_service_" + result.data[i].serviceCode + "' class='background-disabled text-right' maxlength='15'  autocomplete='off' style='min-width: 100px;' disabled  onkeyup='_contract_pay_create_new.FormatNumberService(this);' onchange='_contract_pay_create_new.UpdateAmountService(" + result.data[i].serviceId + ")' >" + "</td>" +
                "</tr>"
            );
            totalAmount += result.data[i].totalAmount
            totalDisarmed += result.data[i].totalDisarmed
            totalNeedPayment += result.data[i].totalNeedPayment
            if (isEdit) {
                totalPayment += result.data[i].payment
                let index = result.data[i].serviceCode
                let payment = result.data[i].payment
                setTimeout(function () {
                    $('#amount_service_' + index).val(_contract_pay_create_new.FormatNumberStr(payment))
                    $('#amount_service_' + index).attr('disabled', false)
                    $('#amount_service_' + index).removeClass('background-disabled')
                }, 1000)
            }
            if (result.data[i].isChecked) {
                let index = result.data[i].serviceCode
                setTimeout(function () {
                    $('#service_ckb_' + index).prop('checked', true)
                }, 1000)
            }
            if (result.data[i].isDisabled) {
                let index = result.data[i].serviceCode
                setTimeout(function () {
                    $('#service_ckb_' + index).attr('disabled', true)
                    $('#amount_service_' + index).attr('disabled', true)
                    $('#amount_service_' + index).addClass('background-disabled')
                    $('#service_' + index).addClass('background-disabled')
                }, 1000)
            }
        }
        $('#supplier-refund-relate-table').find('tbody').append(
            "<tr style='font-weight:bold !important;'>" +
            "<td class='text-right' colspan='5'> Tổng </td>" +
            "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(totalAmount) + "</td>" +
            "<td class='text-right'>" + _contract_pay_create_new.FormatNumberStr(totalDisarmed) + "</td>" +
            "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(totalNeedPayment) + "</td>" +
            "<td class='text-right' id='total_amount_need_pay'>0</td>" +
            "</tr>"
        );
        if (isEdit) {
            setTimeout(function () {
                $('#total_amount_need_pay').html(_contract_pay_create_new.FormatNumberStr(totalPayment))
                _contract_pay_create_new.OnCheckBoxService();
            }, 1000)
        }
    },
    GenderSupplierCommissionTable: function (result, isEdit) {
        var totalAmount = 0
        var totalDisarmed = 0
        var totalNeedPayment = 0
        var totalPayment = 0
        $("#body_supplier_commision_list").empty();
        for (var i = 0; i < result.data.length; i++) {
            var urlService = ''
            if (result.data[i].serviceType == 1) { //Khách sạn
                urlService = "/SetService/VerifyHotelServiceDetai/" + result.data[i].serviceId
            }
            if (result.data[i].serviceType == 3) { // vé máy bay
                urlService = "/SetService/fly/detail/" + result.data[i].groupBookingId
            }
            if (result.data[i].serviceType == 5) { //tour
                urlService = "/SetService/Tour/Detail/" + result.data[i].serviceId
            }
            if (result.data[i].serviceType == 9) { //other
                urlService = "/SetService/Others/Detail/" + result.data[i].serviceId
            }
            $('#supplier-commision-relate-table').find('tbody').append(
                "<tr id='service_" + i + "'>" +
                "<td>" +
                "<label class='check-list number'>" +
                " <input type='checkbox' id='service_ckb_" + result.data[i].serviceCode + "' name='service_ckb' onclick='_contract_pay_create_new.OnCheckBoxService(" + i + ");'>" +
                " <span class='checkmark'></span>" + (i + 1) +
                "  </label>" +
                "<td>" +
                " <a class='blue' href='" + urlService + "'> " + result.data[i].serviceCode + " </a>"
                + "</td>" +
                "<td>" + result.data[i].startDateStr + " - " + result.data[i].endDateStr + "</td>" +
                "<td>" +
                " <a class='blue' href='/Order/Orderdetails?id=" + result.data[i].orderId + "'> " + result.data[i].orderNo + " </a>"
                + "</td>" +
                "<td  >" + result.data[i].salerName + "</td>" +
                "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(result.data[i].totalAmount) + "</td>" +
                "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(result.data[i].totalDisarmed) + "</td>" +
                "<td class='text-right'>" + _contract_pay_create_new.FormatNumberStr(result.data[i].totalNeedPayment) + "</td>" +
                "<td class='text-right' >" + "<input type='text' id='amount_service_" + result.data[i].serviceCode + "' class='background-disabled text-right' maxlength='15'  autocomplete='off' style='min-width: 100px;' disabled  onkeyup='_contract_pay_create_new.FormatNumberService(this);' onchange='_contract_pay_create_new.UpdateAmountService(" + result.data[i].serviceId + ")'>" + "</td>" +
                "</tr>"
            );
            totalAmount += result.data[i].totalAmount
            totalDisarmed += result.data[i].totalDisarmed
            totalNeedPayment += result.data[i].totalNeedPayment
            if (isEdit) {
                totalPayment += result.data[i].payment
                let index = result.data[i].serviceCode
                let payment = result.data[i].payment
                setTimeout(function () {
                    $('#amount_service_' + index).val(_contract_pay_create_new.FormatNumberStr(payment))
                    $('#amount_service_' + index).attr('disabled', false)
                    $('#amount_service_' + index).removeClass('background-disabled')
                }, 1000)
            }
            if (result.data[i].isChecked) {
                let index = result.data[i].serviceCode
                setTimeout(function () {
                    $('#service_ckb_' + index).prop('checked', true)
                }, 1000)
            }
            if (result.data[i].isDisabled) {
                let index = result.data[i].serviceCode
                setTimeout(function () {
                    $('#service_ckb_' + index).attr('disabled', true)
                    $('#amount_service_' + index).attr('disabled', true)
                    $('#amount_service_' + index).addClass('background-disabled')
                    $('#service_' + index).addClass('background-disabled')
                }, 1000)
            }
        }
        $('#supplier-commision-relate-table').find('tbody').append(
            "<tr style='font-weight:bold !important;'>" +
            "<td class='text-right' colspan='5'> Tổng </td>" +
            "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(totalAmount) + "</td>" +
            "<td class='text-right'>" + _contract_pay_create_new.FormatNumberStr(totalDisarmed) + "</td>" +
            "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(totalNeedPayment) + "</td>" +
            "<td class='text-right' id='total_amount_need_pay'>0</td>" +
            "</tr>"
        );
        if (isEdit) {
            setTimeout(function () {
                $('#total_amount_need_pay').html(_contract_pay_create_new.FormatNumberStr(totalPayment))
                _contract_pay_create_new.OnCheckBoxService();
            }, 1000)
        }
    },
    GetOrderListByClientId: function (clientId, isEdit = false) {
        _global_function.AddLoading()
        $.ajax({
            url: "/Receipt/GetOrderListByClientId",
            type: "Post",
            data: {
                'clientId': clientId, 'payId': $('#payId').val()
            },
            success: function (result) {
                _global_function.RemoveLoading()
                listContractPayDetail = result.data
                var totalAmount = 0
                var totalDisarmed = 0
                var totalNeedPayment = 0
                var totalPayment = 0
                $("#body_order_list").empty();
                for (var i = 0; i < result.data.length; i++) {
                    $('#order-relate-table').find('tbody').append(
                        "<tr id='order_" + i + "'>" +
                        "<td>" +
                        "<label class='check-list number'>" +
                        " <input type='checkbox'  id='order_ckb_" + result.data[i].orderCode + "' name='order_ckb' onclick='_contract_pay_create_new.OnCheckBox(" + i + ");_contract_pay_create_new.AddToListDetail(" + i + ")'>" +
                        " <span class='checkmark'></span>" + (i + 1) +
                        "  </label>" +
                        "<td>" +
                        " <a class='blue' href='/Order/Orderdetails?id=" + result.data[i].orderId + "'> " + result.data[i].orderCode + " </a>"
                        + "</td>" +
           
                        "<td style='color:#FF9900;'>" + result.data[i].orderStatus + "</td>" +
                        "<td  >" + result.data[i].salerName + "</td>" +
                        "<td >" + _global_function.DateTimeDotNetToString( result.data[i].createdDate)+ "</td>" +
                        "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(result.data[i].totalAmount) + "</td>" +
                        "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(result.data[i].totalDisarmed) + "</td>" +
                        "<td class='text-right'>" + _contract_pay_create_new.FormatNumberStr(result.data[i].totalNeedPayment) + "</td>" +
                        "<td class='text-right' >" + "<input type='text' class='form-control' id='amount_order_" + result.data[i].orderCode + "' class='background-disabled text-right' maxlength='15'  autocomplete='off' style='min-width: 100px;' disabled  onkeyup='_contract_pay_create_new.FormatNumberOrder(" + result.data[i].orderCode + ");' onchange='_contract_pay_create_new.UpdateAmount(" + i + "," + result.data[i].orderId + ")' >" + "</td>" +
                        "</tr>"
                    );
                    totalAmount += result.data[i].totalAmount
                    totalDisarmed += result.data[i].totalDisarmed
                    totalNeedPayment += result.data[i].totalNeedPayment
                    if (isEdit) {
                        totalPayment += result.data[i].payment
                        let index = result.data[i].orderCode
                        let payment = result.data[i].payment
                        setTimeout(function () {
                            $('#amount_order_' + index).val(_contract_pay_create_new.FormatNumberStr(payment))
                        }, 1000)
                    }
                    if (result.data[i].isChecked) {
                        let index = result.data[i].orderCode
                        setTimeout(function () {
                            $('#order_ckb_' + index).prop('checked', true)
                        }, 1000)
                    }
                    if (result.data[i].isDisabled) {
                        let index = result.data[i].orderCode
                        setTimeout(function () {
                            $('#order_ckb_' + index).attr('disabled', true)
                            $('#amount_order_' + index).attr('disabled', true)
                            $('#amount_order_' + index).addClass('background-disabled')
                            $('#order_' + index).addClass('background-disabled')
                        }, 1000)
                    }
                }
                $('#order-relate-table').find('tbody').append(
                    "<tr style='font-weight:bold !important;'>" +
                    "<td class='text-right' colspan='5'> Tổng </td>" +
                    "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(totalAmount) + "</td>" +
                    "<td class='text-right'>" + _contract_pay_create_new.FormatNumberStr(totalDisarmed) + "</td>" +
                    "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(totalNeedPayment) + "</td>" +
                    "<td class='text-right' id='total_amount_need_pay'>0</td>" +
                    "</tr>"
                );
                if (isEdit) {
                    setTimeout(function () {
                        $('#total_amount_need_pay').html(_contract_pay_create_new.FormatNumberStr(totalPayment))
                        _contract_pay_create_new.OnCheckBox();
                    }, 1000)
                }
                _contract_pay_create_new.SetValueToOrderCodeInput()
            }
        });
    },
    SetValueToOrderCodeInput: function () {
        var orderCodeInput = $('#orderCodeInput').val()
        if (orderCodeInput != null && orderCodeInput !== undefined && orderCodeInput !== '') {
            $('#order_ckb_' + orderCodeInput).prop('checked', true)
            $('#amount_order_' + orderCodeInput).attr('disabled', false)
            $('#amount_order_' + orderCodeInput).removeClass('background-disabled')
            for (var i = 0; i < listContractPayDetail.length; i++) {
                if (listContractPayDetail[i].orderCode === orderCodeInput) {
                    $('#amount_order_' + orderCodeInput).val(_contract_pay_create_new.FormatNumberStr(listContractPayDetail[i].totalNeedPayment))
                    $('#amount').val(_contract_pay_create_new.FormatNumberStr(listContractPayDetail[i].totalNeedPayment))
                    $('#total_amount_need_pay').html(_contract_pay_create_new.FormatNumberStr(listContractPayDetail[i].totalNeedPayment))
                    break
                }
            }
        }
    },
    GetDepositListByClientId: function (clientId) {
        _global_function.AddLoading()
        $.ajax({
            url: "/Receipt/GetDepositListByClientId",
            type: "Post",
            data: { 'clientId': clientId, 'payId': $('#payId').val() },
            success: function (result) {
                _global_function.RemoveLoading()
                listContractPayDetail = result.data
                var totalAmount = 0
                $("#body_deposit_history_list").empty();
                for (var i = 0; i < result.data.length; i++) {
                    $('#deposit-relate-table').find('tbody').append(
                        "<tr>" +
                        "<td>" +
                        " <label class='radio mr-2'>" +
                        "  <input type='radio' name='optradio' id='order_radio_" + i + "'>" +
                        " <span class='checkmark'></span> " + (i + 1) +
                        " </label>"
                        + "</td>" +
                        "<td>" +
                        " <a class='blue' href='/Funding/Detail?depositHistotyId=" + result.data[i].id + "'> " + result.data[i].transNo + " </a>"
                        + "</td>" +
                        "<td>" + result.data[i].serviceName + "</td>" +
                        "<td style='color:#FF9900;'>" + result.data[i].statusStr + "</td>" +
                        "<td>" + result.data[i].userName + "</td>" +
                        "<td class='text-right'>" + _contract_pay_create_new.FormatNumberStr(result.data[i].price) + "</td>" +
                        "</tr>"
                    );
                    totalAmount += result.data[i].price
                    if (result.data[i].isChecked) {
                        let index = i
                        setTimeout(function () {
                            $('#order_radio_' + index).prop('checked', true)
                        }, 800)
                    }
                }
                $('#deposit-relate-table').find('tbody').append(
                    "<tr style='font-weight:bold !important;'>" +
                    "<td class='text-right' colspan='5'> Tổng </td>" +
                    "<td>" + _contract_pay_create_new.FormatNumberStr(totalAmount) + "</td>" +
                    "</tr>"
                );

            }
        });
    },
    Validate: function () {
        if ($('#contract-type').val() == undefined || $('#contract-type').val() == null || $('#contract-type').val() == '') {
            _msgalert.error('Vui lòng chọn loại nghiệp vụ');
            return false;
        }
        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && (parseInt(contract_type) == contractpay_type_order
            || parseInt(contract_type) == contractpay_type_deposit)) {
            if (($('#client-select').val() == undefined || $('#client-select').val() == null || $('#client-select').val() == '') && !is_admin) {
                _msgalert.error('Vui lòng chọn khách hàng');
                return false;
            }
        }

        if (contract_type !== null && contract_type !== '' && (parseInt(contract_type) == contractpay_type_supplier_commision
            || parseInt(contract_type) == contractpay_type_supplier_refund)) {
            if (($('#supplier-select').val() == undefined || $('#supplier-select').val() == null || $('#supplier-select').val() == '') && !is_admin) {
                _msgalert.error('Vui lòng chọn nhà cung cấp');
                return false;
            }
        }
        if ($('#contract-pay-type').val() == undefined || $('#contract-pay-type').val() == null || $('#contract-pay-type').val() == '') {
            _msgalert.error('Vui lòng chọn hình thức');
            return false;
        }
        if (parseInt($('#contract-pay-type').val()) == 2 && ($('#bankingAccount').val() == undefined || $('#bankingAccount').val() == null || $('#bankingAccount').val() == '')) {
            _msgalert.error('Vui lòng chọn tài khoản ngân hàng nhận');
            return false;
        }
        if (($('#amount').val() == undefined || $('#amount').val() == null || $('#amount').val() == '') && totalNeedPaymentOrder > 0) {
            _msgalert.error('Vui lòng nhập số tiền');
            return false;
        }
        if ($('#content').val() == undefined || $('#content').val() == null || $('#content').val() == '') {
            _msgalert.error('Vui lòng nhập nội dung');
            return false;
        }
        var file = document.querySelector('input[name=imagefile]').files[0]
        if (file !== undefined && file !== null) {
            var fileName = file.name
            if (!(fileName.includes('.jpg') || fileName.includes('.png') || fileName.includes('.jpeg') || fileName.includes('.gif '))) {
                _msgalert.error('File đính kèm không đúng định dạng ảnh .png, .jpg, .jpeg, gif. Vui lòng kiểm tra lại');
                return false;
            }
        }
        var flag = false
        if (contract_type == contractpay_type_other) return true
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_order) { // thu tiền đơn hàng
            var totalInputAmount = 0
            for (var i = 0; i < listContractPayDetail.length; i++) {
                var checked = $('#order_ckb_' + listContractPayDetail[i].orderCode).is(":checked")
                if (checked) {
                    totalInputAmount += listContractPayDetail[i].amount
                    if (listContractPayDetail[i].amount === 0 && listContractPayDetail[i].totalNeedPayment > 0 && !is_admin) {
                        _msgalert.error('Vui lòng nhập số tiền giải trừ lớn hơn 0 cho đơn hàng: ' + listContractPayDetail[i].orderCode);
                        return false;
                    }
                    flag = true
                }
            }
            if (!flag && !is_admin) {
                _msgalert.error('Vui lòng tích chọn đơn hàng cần giải trừ');
                return false;
            }
            if ((totalInputAmount === 0 && totalNeedPaymentOrder > 0) && !is_admin) {
                _msgalert.error('Vui lòng nhập số tiền giải trừ lớn hơn 0');
                return false;
            }
            var amountContract = parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))

            if (totalInputAmount > amountContract && !is_admin) {
                _msgalert.error(' Tổng tiền cần giải trừ không được lớn hơn số tiền của phiếu thu');
                return false;
            }
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_deposit) { // thu tiền nạp quỹ
            for (var i = 0; i < listContractPayDetail.length; i++) {
                var checked = $('#order_radio_' + i).is(":checked")
                if (checked) {
                    flag = true
                }
            }
            if (!flag) {
                _msgalert.error('Vui lòng tích chọn nạp quỹ cần giải trừ');
                return false;
            }
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_supplier_refund) {
            var totalInputAmount = 0
            for (var i = 0; i < listServiceRefundDetail.length; i++) {
                var checked = $('#service_ckb_' + listServiceRefundDetail[i].serviceCode).is(":checked")
                if (checked) {
                    totalInputAmount += listServiceRefundDetail[i].amount
                    if (listServiceRefundDetail[i].amount === 0) {
                        _msgalert.error('Vui lòng nhập số tiền hoàn trả lớn hơn 0');
                        return false;
                    }
                    flag = true
                }
            }
            if (!flag) {
                _msgalert.error('Vui lòng tích chọn dịch vụ cần hoàn trả');
                return false;
            }
            if (totalInputAmount === 0) {
                _msgalert.error('Vui lòng nhập số tiền hoàn trả lớn hơn 0');
                return false;
            }
            var amountContract = parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))

            if (totalInputAmount > amountContract) {
                _msgalert.error(' Tổng tiền cần hoàn trả không được lớn hơn số tiền của phiếu thu');
                return false;
            }
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_supplier_commision) {
            var totalInputAmount = 0
            for (var i = 0; i < listServiceRefundDetail.length; i++) {
                var checked = $('#service_ckb_' + listServiceRefundDetail[i].serviceCode).is(":checked")
                if (checked) {
                    totalInputAmount += listServiceRefundDetail[i].amount
                    if (listServiceRefundDetail[i].amount === 0) {
                        _msgalert.error('Vui lòng nhập số tiền trả hoa hồng lớn hơn 0');
                        return false;
                    }
                    flag = true
                }
            }
            if (!flag) {
                _msgalert.error('Vui lòng tích chọn dịch vụ cần trả hoa hồng');
                return false;
            }
            if (totalInputAmount === 0) {
                _msgalert.error('Vui lòng nhập số tiền trả hoa hồng lớn hơn 0');
                return false;
            }
            var amountContract = parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))

            if (totalInputAmount > amountContract) {
                _msgalert.error(' Tổng tiền cần trả hoa hồng không được lớn hơn số tiền của phiếu thu');
                return false;
            }
        }

        return true
    },
    AddNewContractPay: function () {
        let validate = _contract_pay_create_new.Validate()
        if (!validate)
            return;
        let contractPayDetails = listContractPayDetail
        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_order) { // thu tiền đơn hàng
            contractPayDetails = []
            for (var i = 0; i < listContractPayDetail.length; i++) {
                var checked = $('#order_ckb_' + listContractPayDetail[i].orderCode).is(":checked")

                if (checked) {
                    if (listContractPayDetail[i].amount == null || isNaN(listContractPayDetail[i].amount))
                        listContractPayDetail[i].amount = 0
                    contractPayDetails.push(listContractPayDetail[i])
                }
            }
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_deposit) { // thu tiền ký quỹ
            contractPayDetails = []
            for (var i = 0; i < listContractPayDetail.length; i++) {
                var checked = $('#order_radio_' + i).is(":checked")
                if (checked) {
                    listContractPayDetail[i].amount = parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))
                    contractPayDetails.push(listContractPayDetail[i])
                }
            }
        }
        if (contract_type !== null && contract_type !== '' && (parseInt(contract_type) == contractpay_type_supplier_commision
            || parseInt(contract_type) == contractpay_type_supplier_refund)) { // thu tiền NCC hoàn trả hoặc hoa hồng NCC
            contractPayDetails = []
            for (var i = 0; i < listServiceRefundDetail.length; i++) {
                var checked = $('#service_ckb_' + listServiceRefundDetail[i].serviceCode).is(":checked")
                if (checked) {
                    contractPayDetails.push(listServiceRefundDetail[i])
                }
            }
        }
        var formData = new FormData();
        const file = document.querySelector('input[name=imagefile]').files[0];
        if ($('#amount').val() == undefined || $('#amount').val() == null || $('#amount').val() == '')
            $('#amount').val(0)
        let obj = {
            'type': parseInt($('#contract-type').val()),
            'payType': parseInt($('#contract-pay-type').val()),
            'bankingAccountId': parseInt($('#bankingAccount').val()),
            'description': $('#description').val(),
            'note': $('#content').val(),
            'clientId': parseInt(($('#client-select').val())),
            'supplierId': parseInt(($('#supplier-select').val())),
            'objectType': parseInt(($('#partner_choose_type').val())),
            'employeeId': parseInt(($('#createdBy').val())),
            'amount': parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', '')),
            'contractPayDetails': contractPayDetails,
        }
        formData.append('imagefile', file);
        formData.append('jsonData', JSON.stringify(obj))
        _global_function.AddLoading()
        $.ajax({
            url: "/Receipt/AddNewJson",
            type: "Post",
            contentType: false,
            data: formData,
            processData: false,
            success: function (result) {
                _global_function.RemoveLoading()
                if (result.isSuccess) {
                    _msgalert.success(result.message);
                    $.magnificPopup.close();
                    setTimeout(function () { window.location.reload() }, 500)
                } else {
                    _msgalert.error(result.message);
                }
            }
        });
    },
    OnChooseContractPayType: function () {
        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_order) { // thu tiền đơn hàng
            $('#divSupplier').hide()
            $('#divCustomer').show()
            $('#divEmployee').hide()
            $('#supplier-select').empty()
            $('#client-select').empty()
            $('#createdBy').empty()
            $('#body_order_list').empty()
            $('#partner_choose_type').val(1)
            $('#partner_choose_type').attr('disabled', true)
            $('#partner_choose_type').addClass('background-disabled')
            $('#order-relate').show()
            $('#deposit-relate').hide()
            $('#supplier-refund-relate').hide()
            $('#supplier-commision-relate').hide()
            $('input[name="datetimeCreateFilter"]').daterangepicker({
                autoUpdateInput: false,
                locale: {
                    cancelLabel: 'Clear'
                }
            });
            $('input[name="datetimeCreateFilter"]').on('cancel.daterangepicker', function (ev, picker) {
                $(this).val('');
                isPickerCreateAddContract = false;
            });
            $('input[name="datetimeCreateFilter"]').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
                isPickerCreateAddContract = true;
            });
            //var newOption = new Option('Thuy TT74', 201, true, true);
            //$('#client-select').append(newOption).trigger('change');
            //_contract_pay_create_new.GetOrderListByClientId(201);
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_deposit) {//thu tiền ký quỹ
            $('#divSupplier').hide()
            $('#divCustomer').show()
            $('#divEmployee').hide()
            $('#supplier-select').empty()
            $('#client-select').empty()
            $('#createdBy').empty()
            $('#body_deposit_history_list').empty()
            $('#partner_choose_type').val(1)
            $('#partner_choose_type').attr('disabled', true)
            $('#partner_choose_type').addClass('background-disabled')
            $('#order-relate').hide()
            $('#deposit-relate').show()
            $('#supplier-refund-relate').hide()
            $('#supplier-commision-relate').hide()
            //var newOption = new Option('Cường', 182, true, true);
            //$('#client-select').append(newOption).trigger('change');
            //_contract_pay_create_new.GetDepositListByClientId(182);
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_supplier_refund) {//thu tiền NCC hoàn trả
            $('#divSupplier').show()
            $('#divCustomer').hide()
            $('#divEmployee').hide()
            $('#body_supplier_refund_list').empty()
            $('#amount').val()
            $('#supplier-select').empty()
            $('#client-select').empty()
            $('#createdBy').empty()
            $('#partner_choose_type').val(2)
            $('#partner_choose_type').attr('disabled', true)
            $('#partner_choose_type').addClass('background-disabled')
            $('#order-relate').hide()
            $('#deposit-relate').hide()
            $('#supplier-refund-relate').show()
            $('#supplier-commision-relate').hide()
            //var supplier_id = $('#supplier-select').val()
            //if (supplier_id !== null && supplier_id !== undefined && supplier_id !== '') {
            //    //this.GetDataBySupplierId(supplier_id[0])
            //    _contract_pay_create_new.GetServiceListBySupplierId(supplier_id, isEdit);
            //}
            $('#amount').val()
            $('#amount').attr('disabled', true)
            $('#amount').addClass('background-disabled')
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_supplier_commision) {//thu tiền hoa hồng NCC
            $('#divSupplier').show()
            $('#divCustomer').hide()
            $('#amount').val()
            $('#divEmployee').hide()
            $('#supplier-select').empty()
            $('#client-select').empty()
            $('#createdBy').empty()
            $('#body_supplier_commision_list').empty()
            $('#partner_choose_type').val(2)
            $('#partner_choose_type').attr('disabled', true)
            $('#partner_choose_type').addClass('background-disabled')
            $('#order-relate').hide()
            $('#deposit-relate').hide()
            $('#supplier-refund-relate').hide()
            $('#supplier-commision-relate').show()
            //var supplier_id = $('#supplier-select').val()
            //if (supplier_id !== null && supplier_id !== undefined && supplier_id !== '') {
            //    _contract_pay_create_new.GetServiceListBySupplierId(supplier_id, isEdit, true);
            //}
            $('#amount').val()
            $('#amount').attr('disabled', true)
            $('#amount').addClass('background-disabled')
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_other) {//thu tiền khác
            $('#divSupplier').hide()
            $('#divEmployee').hide()
            $('#amount').val()
            $('#order-relate').hide()
            $('#deposit-relate').hide()
            $('#supplier-select').empty()
            $('#client-select').empty()
            $('#createdBy').empty()
            $('#supplier-refund-relate').hide()
            $('#supplier-commision-relate').hide()
            $('#partner_choose_type').val(1)
            $('#partner_choose_type').attr('disabled', false)
            $('#partner_choose_type').removeClass('background-disabled')
            $('#lblCustomer').show()
            $('#divCustomer').show()
        }
    },
    OnChoosePartnerType: function () {
        var partner_choose_type = $('#partner_choose_type').val()
        $('#divSupplier').hide()
        $('#divCustomer').hide()
        $('#divEmployee').hide()
        $('#supplier-select').empty()
        $('#client-select').empty()
        $('#createdBy').empty()
        if (partner_choose_type === '1') {//khách hàng
            $('#divCustomer').show()
        }
        if (partner_choose_type === '2') {//nhà cung cấp
            $('#divSupplier').show()
        }
        if (partner_choose_type === '3') {//nhân viên
            $('#divEmployee').show()
        }
    },
    OnChooseContractPayTypeEdit: function (client_id, supplierId, employeeId, objectType) {
        if ((client_id == undefined || client_id == null || client_id == 0 || client_id == '')
            && (supplierId == undefined || supplierId == null || supplierId == 0 || supplierId == '')
            && (employeeId == undefined || employeeId == null || employeeId == 0 || employeeId == '')) {
            return
        }
        object_type = objectType
        amountEdit = parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))
        isEdit = true

        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_order) { // thu tiền đơn hàng
            setTimeout(function () {
                var newOption = new Option($('#client_name_hide').val(), client_id, true, true);
                $('#client-select').append(newOption).trigger('change');
            }, 500)
            $('#divSupplier').hide()
            $('#divCustomer').show()
            $('#divEmployee').hide()
            $('#supplier-select').empty()
            $('#client-select').empty()
            $('#createdBy').empty()
            $('#body_order_list').empty()
            $('#partner_choose_type').val(1)
            $('#partner_choose_type').attr('disabled', true)
            $('#partner_choose_type').addClass('background-disabled')
            $('#order-relate').show()
            $('#deposit-relate').hide()
            $('#supplier-refund-relate').hide()
            $('#supplier-commision-relate').hide()
            $('#amount').attr('disabled', false)
            $('#amount').removeClass('background-disabled')
            if (client_id !== null && client_id !== undefined && client_id !== '') {
                _contract_pay_create_new.GetDataByClientId(client_id, true)
            }
            _contract_pay_create_new.GetListBankAccountAdavigo()
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_deposit) {//thu tiền ký quỹ
            setTimeout(function () {
                var newOption = new Option($('#client_name_hide').val(), client_id, true, true);
                $('#client-select').append(newOption).trigger('change');
            }, 500)
            $('#divSupplier').hide()
            $('#divCustomer').show()
            $('#divEmployee').hide()
            $('#supplier-select').empty()
            $('#client-select').empty()
            $('#createdBy').empty()
            $('#body_deposit_history_list').empty()
            $('#partner_choose_type').val(1)
            $('#partner_choose_type').attr('disabled', true)
            $('#partner_choose_type').addClass('background-disabled')
            $('#order-relate').hide()
            $('#deposit-relate').show()
            $('#supplier-refund-relate').hide()
            $('#supplier-commision-relate').hide()
            if (client_id !== null && client_id !== undefined && client_id !== '') {
                _contract_pay_create_new.GetDataByClientId(client_id, true)
            }
            _contract_pay_create_new.GetListBankAccountAdavigo()
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_supplier_refund) {//thu tiền NCC hoàn trả
            $('#divSupplier').show()
            $('#divCustomer').hide()
            $('#divEmployee').hide()
            $('#body_supplier_refund_list').empty()
            $('#supplier-select').empty()
            $('#client-select').empty()
            $('#createdBy').empty()
            $('#partner_choose_type').val(2)
            $('#partner_choose_type').attr('disabled', true)
            $('#partner_choose_type').addClass('background-disabled')
            $('#order-relate').hide()
            $('#deposit-relate').hide()
            $('#supplier-refund-relate').show()
            $('#supplier-commision-relate').hide()
            setTimeout(function () {
                var newOption = new Option($('#supplier_name_hide').val(), supplierId, true, true);
                $('#supplier-select').append(newOption).trigger('change');
            }, 500)
            _contract_pay_create_new.GetListBankAccountAdavigo(supplierId)
            if (supplierId !== null && supplierId !== undefined && supplierId !== '') {
                _contract_pay_create_new.GetServiceListBySupplierId(supplierId, true)
            }
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_supplier_commision) {//thu tiền hoa hồng NCC
            $('#divSupplier').show()
            $('#divCustomer').hide()
            $('#divEmployee').hide()
            $('#supplier-select').empty()
            $('#client-select').empty()
            $('#createdBy').empty()
            $('#body_supplier_commision_list').empty()
            $('#partner_choose_type').val(2)
            $('#partner_choose_type').attr('disabled', true)
            $('#partner_choose_type').addClass('background-disabled')
            $('#order-relate').hide()
            $('#deposit-relate').hide()
            $('#supplier-refund-relate').hide()
            $('#supplier-commision-relate').show()
            setTimeout(function () {
                var newOption = new Option($('#supplier_name_hide').val(), supplierId, true, true);
                $('#supplier-select').append(newOption).trigger('change');
            }, 500)
            _contract_pay_create_new.GetListBankAccountAdavigo(supplierId)
            if (supplierId !== null && supplierId !== undefined && supplierId !== '') {
                _contract_pay_create_new.GetServiceListBySupplierId(supplierId, true, true)
            }
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_other) {//thu tiền khác
            $('#order-relate').hide()
            $('#deposit-relate').show()
            $('#supplier-refund-relate').hide()
            $('#supplier-commision-relate').hide()
            $('#divSupplier').hide()
            $('#divCustomer').hide()
            $('#divEmployee').hide()
            $('#order-relate').hide()
            $('#deposit-relate').hide()
            $('#supplier-select').empty()
            $('#client-select').empty()
            $('#createdBy').empty()
            $('#supplier-refund-relate').hide()
            $('#supplier-commision-relate').hide()
            $('#partner_choose_type').attr('disabled', false)
            $('#partner_choose_type').removeClass('background-disabled')
            $('#amount').attr('disabled', false)
            $('#amount').removeClass('background-disabled')
            $('#partner_choose_type').val(objectType)
            if (objectType == 1) {
                $('#divCustomer').show()
                $('#client-select').attr('disabled', false)
                $('#client-select').removeClass('background-disabled')
                setTimeout(function () {
                    var newOption = new Option($('#client_name_hide').val(), client_id, true, true);
                    $('#client-select').append(newOption).trigger('change');
                }, 500)
                _contract_pay_create_new.GetListBankAccountAdavigo(client_id)
            }
            if (objectType == 2) {
                $('#divSupplier').show()
                $('#supplier-select').attr('disabled', false)
                $('#supplier-select').removeClass('background-disabled')
                setTimeout(function () {
                    var newOption = new Option($('#supplier_name_hide').val(), supplierId, true, true);
                    $('#supplier-select').append(newOption).trigger('change');
                }, 500)
                _contract_pay_create_new.GetListBankAccountAdavigo(supplierId)
            }
            if (objectType == 3) {
                $('#divEmployee').show()
                $('#createdBy').attr('disabled', false)
                $('#createdBy').removeClass('background-disabled')
                setTimeout(function () {
                    var newOption = new Option($('#employee_name_hide').val(), employeeId, true, true);
                    $('#createdBy').append(newOption).trigger('change');
                }, 500)
                _contract_pay_create_new.GetListBankAccountAdavigo()
            }

        }
        _contract_pay_create_new.OnChoosePaymentType()
    },
    Close: function () {
        $('#create_contract_pay').removeClass('show')
        setTimeout(function () {
            $('#create_contract_pay').remove();
        }, 300);
    },
    EditContractPay: function () {
        let validate = _contract_pay_create_new.Validate()
        if (!validate)
            return;
        let contractPayDetails = listContractPayDetail
        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_order) { // thu tiền đơn hàng
            contractPayDetails = []
            for (var i = 0; i < listContractPayDetail.length; i++) {
                var checked = $('#order_ckb_' + listContractPayDetail[i].orderCode).is(":checked")
                if (checked) {
                    if (listContractPayDetail[i].amount == null || isNaN(listContractPayDetail[i].amount))
                        listContractPayDetail[i].amount = 0
                    contractPayDetails.push(listContractPayDetail[i])
                }
            }
        }
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_deposit) { // thu tiền ký quỹ
            contractPayDetails = []
            for (var i = 0; i < listContractPayDetail.length; i++) {
                var checked = $('#order_radio_' + i).is(":checked")
                if (checked) {
                    listContractPayDetail[i].amount = parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))
                    contractPayDetails.push(listContractPayDetail[i])
                }
            }
        }
        if (contract_type !== null && contract_type !== '' && (parseInt(contract_type) == contractpay_type_supplier_commision
            || parseInt(contract_type) == contractpay_type_supplier_refund)) { // thu tiền NCC hoàn trả hoặc hoa hồng NCC
            contractPayDetails = []
            for (var i = 0; i < listServiceRefundDetail.length; i++) {
                var checked = $('#service_ckb_' + listServiceRefundDetail[i].serviceCode).is(":checked")
                if (checked) {
                    contractPayDetails.push(listServiceRefundDetail[i])
                }
            }
        }
        var formData = new FormData();
        const file = document.querySelector('input[name=imagefile]').files[0];
        if ($('#amount').val() == undefined || $('#amount').val() == null || $('#amount').val() == '')
            $('#amount').val(0)
        let obj = {
            'payId': parseInt($('#payId').val()),
            'billNo': $('#billNo').val(),
            'attatchmentFile': $('#attatchmentFile').val(),
            'payStatus': $('#payStatus').val(),
            'type': parseInt($('#contract-type').val()),
            'payType': parseInt($('#contract-pay-type').val()),
            'bankingAccountId': parseInt($('#bankingAccount').val()),
            'description': $('#description').val(),
            'note': $('#content').val(),
            'clientId': parseInt(($('#client-select').val())),
            'supplierId': parseInt(($('#supplier-select').val())),
            'objectType': parseInt(($('#partner_choose_type').val())),
            'employeeId': parseInt(($('#createdBy').val())),
            'amount': parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', '')),
            'contractPayDetails': contractPayDetails,
        }
        formData.append('imagefile', file);
        formData.append('jsonData', JSON.stringify(obj))

        _global_function.AddLoading()
        $.ajax({
            url: "/Receipt/Update",
            type: "Post",
            contentType: false,
            data: formData,
            processData: false,
            success: function (result) {
                _global_function.RemoveLoading()
                if (result.isSuccess) {
                    _msgalert.success(result.message);
                    $.magnificPopup.close();
                    setTimeout(function () { window.location.reload() }, 500)
                } else {
                    _msgalert.error(result.message);
                }
            }
        });
    },
    OnChoosePaymentType: function () {
        var pay_type = $('#contract-pay-type').val()
        if (parseInt(pay_type) != 2) { //thanh toán tiền mặt
            $('#bankingAccount').attr('disabled', true)
            $('#lblBankAccountRequired').hide()
            $('#lblBankAccount').show()
            $('#bankingAccount').val(-1)
        } else {
            $('#bankingAccount').attr('disabled', false)
            $('#lblBankAccountRequired').show()
            $('#lblBankAccount').hide()
            var contract_type = $('#contract-type').val()
            if (contract_type !== null && contract_type !== '' && (parseInt(contract_type) == contractpay_type_order
                || parseInt(contract_type) == contractpay_type_deposit)) {
                _contract_pay_create_new.GetListBankAccountAdavigo()
            }
            //if (contract_type !== null && contract_type !== '' && (parseInt(contract_type) == contractpay_type_supplier_commision
            //    || parseInt(contract_type) == contractpay_type_supplier_refund)) {
            //    var supplier = $('#supplier-select').val()[0]
            //    if (supplier !== null && supplier !== undefined && supplier !== '') {
            //        supplierId = supplier[0]
            //    }
            //    _contract_pay_create_new.GetListBankAccountBySupplierID(supplier)
            //}
        }
    },
    OnChangeImage: function () {
        var file = document.querySelector('input[name=imagefile]').files[0]
        if (file !== undefined && file !== null) {
            var fileName = file.name
            if (!(fileName.includes('.jpg') || fileName.includes('.png') || fileName.includes('.jpeg') || fileName.includes('.gif '))) {
                _msgalert.error('File đính kèm không đúng định dạng ảnh .png, .jpg, .jpeg, gif. Vui lòng kiểm tra lại');
                document.getElementById("imagefile").value = "";
            }
        }
    },
    OnCheckBox: function (index) {
        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_order) { // thu tiền đơn hàng
            //check số tiền chưa giải trừ, nếu >0 thì cần check số tiền, nếu <=0 thì cho phép tạo phiếu thu 0đ
            totalNeedPaymentOrder = 0
            for (var i = 0; i < listContractPayDetail.length; i++) {
                if (listContractPayDetail[i].isDisabled)
                    continue
                var checked = $('#order_ckb_' + listContractPayDetail[i].orderCode).is(":checked")
                if (checked) {
                    totalNeedPaymentOrder += listContractPayDetail[i].totalNeedPayment
                }
            }

            //calculate automatic amount
            if (totalNeedPaymentOrder > 0) {
                var amount_contract = parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))
                if (amount_contract === undefined || amount_contract === '' || amount_contract === null
                    || parseFloat(amount_contract) === 0 || isNaN(amount_contract)) {
                    if (!is_admin) {
                        _msgalert.error('Vui lòng nhập số tiền');
                        $("#orderCodeFilter").empty()
                        for (var i = 0; i < listContractPayDetail.length; i++) {
                            $('#order_ckb_' + listContractPayDetail[i].orderCode).attr("checked", false)
                            $('#amount_order_' + listContractPayDetail[i].orderCode).attr('disabled', true)
                            $('#amount_order_' + listContractPayDetail[i].orderCode).addClass('background-disabled')
                            $('#amount_order_' + listContractPayDetail[i].orderCode).val('')
                        }
                        return
                    }
                }
            }
            var totalAmount = 0
            for (var i = 0; i < listContractPayDetail.length; i++) {
                if (listContractPayDetail[i].amount !== undefined && listContractPayDetail[i].amount !== null
                    && listContractPayDetail[i].amount) {
                    var checked = $('#order_ckb_' + listContractPayDetail[i].orderCode).is(":checked")
                    if (checked && $('#amount_order_' + listContractPayDetail[i].orderCode).val() !== undefined && $('#amount_order_' + listContractPayDetail[i].orderCode).val() !== null
                        && $('#amount_order_' + listContractPayDetail[i].orderCode).val() !== '') {
                        totalAmount += parseFloat($('#amount_order_' + listContractPayDetail[i].orderCode).val().replaceAll('.', '').replaceAll(',', ''))
                    }
                }
            }
            if (isNaN(totalAmount)) totalAmount = 0

            for (var i = 0; i < listContractPayDetail.length; i++) {
                if (listContractPayDetail[i].isDisabled)
                    continue
                var checked = $('#order_ckb_' + listContractPayDetail[i].orderCode).is(":checked")
                if (checked) {
                    let isSuccess = true
                    $('#amount_order_' + listContractPayDetail[i].orderCode).attr('disabled', false)
                    $('#amount_order_' + listContractPayDetail[i].orderCode).removeClass('background-disabled')
                    var amount = $('#amount_order_' + listContractPayDetail[i].orderCode).val()
                    if (amount === null || amount === undefined || amount === '' || parseInt(amount) === 0) {
                        var totalNeedPayment = Math.min((amount_contract - totalAmount), listContractPayDetail[i].totalNeedPayment)
                        if (totalNeedPayment <= 0) {
                            $('#order_ckb_' + listContractPayDetail[i].orderCode).attr("checked", false)
                            $('#amount_order_' + listContractPayDetail[i].orderCode).attr('disabled', true)
                            $('#amount_order_' + listContractPayDetail[i].orderCode).addClass('background-disabled')
                            $('#amount_order_' + listContractPayDetail[i].orderCode).val('')
                            if (!is_admin) {
                                _msgalert.error('Tổng tiền cần giải trừ đã bằng số tiền phiếu thu');
                            }
                            isSuccess = false
                        } else {
                            $('#amount_order_' + listContractPayDetail[i].orderCode).val(_contract_pay_create_new.FormatNumberStr(totalNeedPayment))
                            _contract_pay_create_new.UpdateAmount(i, listContractPayDetail[i].orderId)
                        }
                    }
                    if (isSuccess)
                        listContractPayDetail[i].isChecked = true
                } else {
                    $('#amount_order_' + listContractPayDetail[i].orderCode).attr('disabled', true)
                    $('#amount_order_' + listContractPayDetail[i].orderCode).addClass('background-disabled')
                    $('#amount_order_' + listContractPayDetail[i].orderCode).val('')
                }
            }
        }
        _contract_pay_create_new.GetTotalAmount(index)
        setTimeout(_contract_pay_create_new.AddItemToInput(), 500)
        //setTimeout(_contract_pay_create_new.RenderItemChecked(), 500)
    },
    OnCheckBoxService: function () {
        var contract_type = $('#contract-type').val()
        var totalAmountServiceInput = 0
        if (contract_type !== null && contract_type !== '' && (parseInt(contract_type) == contractpay_type_supplier_refund
            || parseInt(contract_type) == contractpay_type_supplier_commision)) {
            for (var i = 0; i < listServiceRefundDetail.length; i++) {
                if (listServiceRefundDetail[i].isDisabled)
                    continue
                var checked = $('#service_ckb_' + listServiceRefundDetail[i].serviceCode).is(":checked")
                if (checked) {
                    $('#amount_service_' + listServiceRefundDetail[i].serviceCode).attr('disabled', false)
                    $('#amount_service_' + listServiceRefundDetail[i].serviceCode).removeClass('background-disabled')
                    var amount = $('#amount_service_' + listServiceRefundDetail[i].serviceCode).val().replaceAll('.', '').replaceAll(',', '')
                    if (amount === null || amount === undefined || amount === '' || parseFloat(amount) === 0) {
                        $('#amount_service_' + listServiceRefundDetail[i].serviceCode).val(_contract_pay_create_new.FormatNumberStr(listServiceRefundDetail[i].totalNeedPayment))
                        totalAmountServiceInput += listServiceRefundDetail[i].totalNeedPayment
                        listServiceRefundDetail[i].amount = listServiceRefundDetail[i].totalNeedPayment
                    } else {
                        totalAmountServiceInput += parseFloat(amount)
                        listServiceRefundDetail[i].amount = parseFloat(amount)
                    }
                } else {
                    $('#amount_service_' + listServiceRefundDetail[i].serviceCode).attr('disabled', true)
                    $('#amount_service_' + listServiceRefundDetail[i].serviceCode).addClass('background-disabled')
                    $('#amount_service_' + listServiceRefundDetail[i].serviceCode).val('')
                }
            }
        }
        $('#amount').val(_contract_pay_create_new.FormatNumberStr(totalAmountServiceInput))
        $('#total_amount_need_pay').html(_contract_pay_create_new.FormatNumberStr(totalAmountServiceInput))
    },
    OnRadioButton: function (index) {
        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_deposit) { // thu tiền ký quỹ
            for (var i = 0; i < listContractPayDetail.length; i++) {
                var checked = $('#order_radio_' + i).is(":checked")
                if (checked) {
                    $('#amount_order_' + listContractPayDetail[i].orderCode).attr('disabled', false)
                    $('#amount_order_' + listContractPayDetail[i].orderCode).removeClass('background-disabled')
                    var amount = $('#amount_order_' + listContractPayDetail[i].orderCode).val()
                    if (amount === null || amount === undefined || amount === '' || parseInt(amount) === 0) {
                        $('#amount_order_' + listContractPayDetail[i].orderCode).val(_contract_pay_create_new.FormatNumberStr(listContractPayDetail[i].totalNeedPayment))
                    }
                } else {
                    $('#amount_order_' + listContractPayDetail[i].orderCode).attr('disabled', true)
                    $('#amount_order_' + listContractPayDetail[i].orderCode).addClass('background-disabled')
                    $('#amount_order_' + listContractPayDetail[i].orderCode).val('')
                }
            }
        }
        _contract_pay_create_new.GetTotalAmount(index)
    },
    GetTotalAmount: function (index) {
        var totalAmount = 0
        for (var i = 0; i < listContractPayDetail.length; i++) {
            if (listContractPayDetail[i].amount !== undefined && listContractPayDetail[i].amount !== null
                && listContractPayDetail[i].amount) {
                var checked = $('#order_ckb_' + listContractPayDetail[i].orderCode).is(":checked")
                if (checked) {
                    totalAmount += parseFloat($('#amount_order_' + listContractPayDetail[i].orderCode).val().replaceAll('.', '').replaceAll(',', ''))
                }
            }
        }
        $('#total_amount_need_pay').html(_contract_pay_create_new.FormatNumberStr(totalAmount))
        if (!is_admin) {
            if (totalAmount > parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))) {
                _msgalert.error(' Tổng tiền cần giải trừ không được lớn hơn số tiền của phiếu thu');
            }
        }
    },
    FormatDate: function (date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [day, month, year].join('/');
    },
    OnCheckedOrder: function (isClearDate = false) {
        if (listContractPayDetail.length == 0) return
        if (isClearDate) {
            $('#filter_date_create_daterangepicker').data = null
        }
        var requestChoose = $('#orderCodeFilter').val()
        let fromCreateDateStr = null
        let toCreateDateStr = null
        if ($('#filter_date_create_daterangepicker').data('daterangepicker') !== undefined &&
            $('#filter_date_create_daterangepicker').data('daterangepicker') != null && !isClearDate) {
            fromCreateDateStr = $('#filter_date_create_daterangepicker').data('daterangepicker').startDate._d.toLocaleDateString("en-GB");
            toCreateDateStr = $('#filter_date_create_daterangepicker').data('daterangepicker').endDate._d.toLocaleDateString("en-GB");
        } else {
            fromCreateDateStr = null
            toCreateDateStr = null
        }
        var listDate = []
        if (fromCreateDateStr != null && toCreateDateStr != null) {
            const [fromday, frommonth, fromyear] = fromCreateDateStr.split('/');
            const [today, tomonth, toyear] = toCreateDateStr.split('/');
            const fromDate = new Date(+fromyear, frommonth - 1, +fromday, 0, 0);
            const toDate = new Date(+toyear, tomonth - 1, +today, 0, 0);
            for (var d = fromDate; d <= toDate; d.setDate(d.getDate() + 1)) {
                listDate.push(_contract_pay_create_new.FormatDate(d));
            }
        }
        for (var i = 0; i < listContractPayDetail.length; i++) {
            if ((requestChoose !== undefined && requestChoose !== null && requestChoose !== '' && requestChoose.includes(listContractPayDetail[i].orderId + ''))
                || (listDate.includes(listContractPayDetail[i].createDate))) {
                listContractPayDetail[i].isChecked = true
                $('#order_ckb_' + listContractPayDetail[i].orderCode).prop('checked', true)
            } else {
                listContractPayDetail[i].isChecked = false
                $('#order_ckb_' + listContractPayDetail[i].orderCode).prop('checked', false)
            }
        }
        setTimeout(_contract_pay_create_new.OnCheckBox(), 300)
    },
    RenderItemChecked: function () {
        //if (isEditView && isRender) return
        var totalAmount = 0
        var totalDisarmed = 0
        var totalNeedPayment = 0
        $("#body_order_list").empty();
        var listChecked = listContractPayDetail.filter(n => n.isChecked)
        var listUnChecked = listContractPayDetail.filter(n => !n.isChecked)
        var requestChoose = $('#orderCodeFilter').val()
        var index = 1
        for (var i = 0; i < listChecked.length; i++) {
            $('#order-relate-table').find('tbody').append(
                "<tr id='order_" + index + "'>" +
                "<td>" +
                "<label class='check-list number'>" +
                " <input type='checkbox' id='order_ckb_" + listChecked[i].orderCode + "' name='order_ckb' onclick='_contract_pay_create_new.OnCheckBox(" + i + ");_contract_pay_create_new.AddToListDetail(" + i + ")'>" +
                " <span class='checkmark'></span>" + (index) +
                "  </label>" +
                "<td>" +
                " <a class='blue' href='/Order/Orderdetails?id=" + listChecked[i].orderId + "'> " + listChecked[i].orderCode + " </a>"
                + "</td>" +
                "<td>" + listChecked[i].startDate + " - " + listChecked[i].endDate + "</td>" +
                "<td style='color:#FF9900;'>" + listChecked[i].status + "</td>" +
                "<td  >" + listChecked[i].salerName + "</td>" +
                "<td >" + listChecked[i].createDate + "</td>" +
                "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(listChecked[i].totalAmount) + "</td>" +
                "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(listChecked[i].totalDisarmed) + "</td>" +
                "<td class='text-right'>" + _contract_pay_create_new.FormatNumberStr(listChecked[i].totalNeedPayment) + "</td>" +
                "<td class='text-right' >" + "<input type='text' id='amount_order_" + listChecked[i].orderCode + "' class='background-disabled text-right' maxlength='15'  autocomplete='off' style='min-width: 100px;' disabled  onkeyup='_contract_pay_create_new.FormatNumberOrder(" + listChecked[i].orderCode + ");' onchange='_contract_pay_create_new.UpdateAmount(" + i + "," + listChecked[i].orderId + ")' >" + "</td>" +
                "</tr>"
            );
            totalAmount += listChecked[i].totalAmount
            totalDisarmed += listChecked[i].totalDisarmed
            totalNeedPayment += listChecked[i].totalNeedPayment
            if (listChecked[i].isChecked) {
                let code = listChecked[i].orderCode
                setTimeout(function () {
                    $('#order_ckb_' + code).prop('checked', true)
                }, 300)
            }
            index++
        }
        for (var i = 0; i < listUnChecked.length; i++) {
            $('#order-relate-table').find('tbody').append(
                "<tr id='order_" + index + "'>" +
                "<td>" +
                "<label class='check-list number'>" +
                " <input type='checkbox' id='order_ckb_" + listUnChecked[i].orderCode + "' name='order_ckb' onclick='_contract_pay_create_new.OnCheckBox(" + i + ");_contract_pay_create_new.AddToListDetail(" + i + ")'>" +
                " <span class='checkmark'></span>" + (index) +
                "  </label>" +
                "<td>" +
                " <a class='blue' href='/Order/Orderdetails?id=" + listUnChecked[i].orderId + "'> " + listUnChecked[i].orderCode + " </a>"
                + "</td>" +
                "<td>" + listUnChecked[i].startDate + " - " + listUnChecked[i].endDate + "</td>" +
                "<td style='color:#FF9900;'>" + listUnChecked[i].status + "</td>" +
                "<td  >" + listUnChecked[i].salerName + "</td>" +
                "<td >" + listUnChecked[i].createDate + "</td>" +
                "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(listUnChecked[i].totalAmount) + "</td>" +
                "<td class='text-right' >" + _contract_pay_create_new.FormatNumberStr(listUnChecked[i].totalDisarmed) + "</td>" +
                "<td class='text-right'>" + _contract_pay_create_new.FormatNumberStr(listUnChecked[i].totalNeedPayment) + "</td>" +
                "<td class='text-right' >" + "<input type='text' id='amount_order_" + listUnChecked[i].orderCode + "' class='background-disabled text-right' maxlength='15'  autocomplete='off' style='min-width: 100px;' disabled  onkeyup='_contract_pay_create_new.FormatNumberOrder(" + listUnChecked[i].orderCode + ");' onchange='_contract_pay_create_new.UpdateAmount(" + i + "," + listUnChecked[i].orderId + ")' >" + "</td>" +
                "</tr>"
            );
            index++
        }
        $('#order-relate-table').find('tbody').append(
            "<tr style='font-weight:bold !important;'>" +
            "<td class='text-right' colspan='4'> Tổng </td>" +
            "<td>" + _contract_pay_create_new.FormatNumberStr(totalAmount) + "</td>" +
            "</tr>"
        );
        _contract_pay_create_new.AddItemToInput()
    },
    AddItemToInput: function () {
        $("#orderCodeFilter").empty()
        for (var i = 0; i < listContractPayDetail.length; i++) {
            if (listContractPayDetail[i].isChecked) {
                var newOption = new Option(listContractPayDetail[i].orderCode, listContractPayDetail[i].orderId, true, true);
                $('#orderCodeFilter').append(newOption)
            }
        }
        //$('#orderCodeFilter').trigger('change');
    },
    GetOrderDetail: function () {
        var timer = 0;
        clearTimeout(timer);
        timer = setTimeout(function () {
            _global_function.AddLoading()
            $.ajax({
                url: "/Receipt/GetOrderDetail",
                type: "Post",
                data: {
                    'orderCode': $('#orderCodeInput').val()
                },
                success: function (result) {
                    _global_function.RemoveLoading()
                    $('#client-select').empty()
                    if (result.isSuccess) {
                        var orderDetail = result.data
                        var newOption = new Option(orderDetail.clientName, orderDetail.clientId, true, true);
                        $('#client-select').append(newOption).trigger('change');
                    } else {
                        _msgalert.error(result.message);
                        $('#orderCodeInput').val('')
                    }
                }
            });
        }, 1000);
    },
    GetServiceDetail: function () {
        var timer = 0;
        clearTimeout(timer);
        timer = setTimeout(function () {
            _global_function.AddLoading()
            $.ajax({
                url: "/Receipt/GetServiceDetail",
                type: "Post",
                data: {
                    'serviceCode': $('#serviceCodeInput').val()
                },
                success: function (result) {
                    _global_function.RemoveLoading()
                    $('#supplier-select').empty()
                    if (result.isSuccess) {
                        var serviceDetail = result.data
                        var newOption = new Option(serviceDetail.supplierName, orderDetail.supplierId, true, true);
                        $('#supplier-select').append(newOption).trigger('change');
                    } else {
                        _msgalert.error(result.message);
                        $('#serviceCodeInput').val('')
                    }
                }
            });
        }, 1000);
    },
    OnChangeAmount: function () {
        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_order) { // thu tiền đơn hàng
            var currentAmomunt = parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))
            if (currentAmomunt < amountEdit && !is_admin) {
                _msgalert.error('Vui lòng nhập số tiền lớn hơn hoặc bằng số tiền ban đầu');
                $('#amount').val(_contract_pay_create_new.FormatNumberStr(amountEdit))
                return false;
            }
        }
    },
    AddToListDetail: function (index) {
        var contract_type = $('#contract-type').val()
        if (contract_type !== null && contract_type !== '' && parseInt(contract_type) == contractpay_type_order) { // thu tiền đơn hàng
            for (var i = 0; i < listContractPayDetail.length; i++) {
                if (i === index) {
                    listDetail.push(listContractPayDetail[i])
                }
            }
        }
    },
    GetListBankAccountBySupplierID: function (supplierId) {
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentRequest/GetListBankAccountBySupplierId",
            type: "Post",
            data: {
                'supplierId': supplierId,
            },
            success: function (result) {
                _global_function.RemoveLoading()
                $("#bankingAccount").empty();
                $('#bankingAccount').append(
                    "<option value='0'>Chọn</option>"
                );
                listBankAccount = result.data
                for (var i = 0; i < result.data.length; i++) {
                    $('#bankingAccount').append(
                        "<option value='" + result.data[i].id + "'> " + result.data[i].bankId + " - " + result.data[i].accountNumber + "</option>"
                    );
                }
                if (bankingAccountId !== undefined && bankingAccountId !== null && bankingAccountId !== 0) {
                    $("#bankingAccount").val(bankingAccountId);
                    for (var i = 0; i < listBankAccount.length; i++) {
                        if (listBankAccount[i].id === parseInt(bankingAccountId))
                            $('#bankName').val(listBankAccount[i].accountName)
                    }
                }
            }
        });
    },
    GetListBankAccountAdavigo: function (supplierId) {
        _global_function.AddLoading()
        $.ajax({
            url: "/Receipt/GetListBankAccountAdavigo",
            type: "Post",
            data: {
                'supplierId': supplierId,
            },
            success: function (result) {
                _global_function.RemoveLoading()
                $("#bankingAccount").empty();
                $('#bankingAccount').append(
                    "<option value='0'>Chọn</option>"
                );
                listBankAccount = result.data
                for (var i = 0; i < result.data.length; i++) {
                    $('#bankingAccount').append(
                        "<option value='" + result.data[i].id + "'> " + result.data[i].bankId + " - " + result.data[i].accountNumber + "</option>"
                    );
                }
                if (bankingAccountId !== undefined && bankingAccountId !== null && bankingAccountId !== 0) {
                    $("#bankingAccount").val(bankingAccountId);
                    for (var i = 0; i < listBankAccount.length; i++) {
                        if (listBankAccount[i].id === parseInt(bankingAccountId))
                            $('#bankName').val(listBankAccount[i].accountName)
                    }
                }
            }
        });
    },
    GetListBankAccountByClientID: function (clientId) {
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentRequest/GetListBankAccountByClientID",
            type: "Post",
            data: {
                'clientId': clientId,
            },
            success: function (result) {
                _global_function.RemoveLoading()
                $("#bankingAccount").empty();
                $('#bankingAccount').append(
                    "<option value='0'>Chọn</option>"
                );
                listBankAccount = result.data
                for (var i = 0; i < result.data.length; i++) {
                    $('#bankingAccount').append(
                        "<option value='" + result.data[i].id + "'> " + result.data[i].bankId + " - " + result.data[i].accountNumber + "</option>"
                    );
                }
                if (bankingAccountId !== undefined && bankingAccountId !== null && bankingAccountId !== 0) {
                    $("#bankingAccount").val(bankingAccountId);
                    for (var i = 0; i < listBankAccount.length; i++) {
                        if (listBankAccount[i].id === parseInt(bankingAccountId))
                            $('#bankName').val(listBankAccount[i].accountName)
                    }
                }
            }
        });
    },

}