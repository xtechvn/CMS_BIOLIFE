
let isPickerCreated = false;
let isPickerPayment = false;
let isPickerApproved = false;
let listStatusType = [];
let listServiceType = [];
let listPaymentType = [];
let isResetTab = true;
let listUserCreate = []
let fields = {
    paymentCode: null,
    content: null,
    serviceCode: null,
    orderNo: null,
    type: null,
    status: -1,
    statusMulti: [],
    typeMulti: [],
    paymentTypeMulti: [],
    paymentType: null,
    createByIds: null,
    verifyByIds: null,
    clientId: null,
    supplierId: null,
    currentPage: 1,
    pageSize: 20,
    fromCreateDateStr: null,
    toCreateDateStr: null,
    paymentDateFromStr: null,
    paymentDateToStr: null,
    verifyDateFromStr: null,
    verifyDateToStr: null,
    isSupplierDebt: null,
    isPaymentBefore: null,
}
let cookieFilterPaymentRequestName = 'payment_request_search_cache'
let cookiePaymentRequest_filter_Client = 'cookiePaymentRequest_filter_Client';
let cookiePaymentRequest_filter_Supplier = 'cookiePaymentRequest_filter_Supplier';
let cookiePaymentRequest_filter_CreateBy = 'cookiePaymentRequest_filter_CreateBy';
let cookiePaymentRequest_filter_VerifyBy = 'cookiePaymentRequest_filter_VerifyBy';
$(document).ready(function () {
    if (_global_function.getCookie(cookieFilterPaymentRequestName) != null && window.location.href.indexOf("PaymentRequest/Index") != -1) {
        let cookie = JSON.parse(_global_function.getCookie(cookieFilterPaymentRequestName))
        fields = cookie
        _payment_request_service.GetCacheFilter()
        _global_function.eraseCookie(cookieFilterPaymentRequestName)
    }
    $('input').attr('autocomplete', 'off');
    $('input').keyup(function (e) {
        if (e.which === 13) {
            _payment_request_service.OnPaging(1);
        }
    });
    $('input').attr('autocomplete', 'off');
    //multi select
    const selectBtnStatusType = document.querySelector(".select-btn-status-type")
    const itemsStatusType = document.querySelectorAll(".item-status-type");
    const selectBtnServiceType = document.querySelector(".select-btn-service-type")
    const itemsServiceType = document.querySelectorAll(".item-service-type");
    const selectBtnPaymentType = document.querySelector(".select-btn-payment-type")
    const itemsPaymentType = document.querySelectorAll(".item-payment-type");
    $(document).click(function (event) {
        var $target = $(event.target);
        if (!$target.closest('#select-btn-status-type').length) {//checkbox_trans_type_
            if ($('#list-item-status').is(":visible") && !$target[0].id.includes('status_type_text') && !$target[0].id.includes('status_type')
                && !$target[0].id.includes('list-item-status') && !$target[0].id.includes('checkbox_status_type')) {
                selectBtnStatusType.classList.toggle("open");
            }
        }
        if (!$target.closest('#select-btn-service-type').length) {
            if ($('#list-item-service').is(":visible") && !$target[0].id.includes('service_type_text') && !$target[0].id.includes('service_type')
                && !$target[0].id.includes('list-item-service') && !$target[0].id.includes('checkbox_service_type')) {
                selectBtnServiceType.classList.toggle("open");
            }
        }
        if (!$target.closest('#select-btn-payment-type').length) {
            if ($('#list-item-payment').is(":visible") && !$target[0].id.includes('payment_type_text') && !$target[0].id.includes('payment_type')
                && !$target[0].id.includes('list-item-payment') && !$target[0].id.includes('checkbox_payment_type')) {
                selectBtnPaymentType.classList.toggle("open");
            }
        }
    });
    if (selectBtnStatusType !== null && selectBtnStatusType !== undefined)
        selectBtnStatusType.addEventListener("click", (e) => {
            e.preventDefault();
            selectBtnStatusType.classList.toggle("open");
        });
    if (selectBtnServiceType !== null && selectBtnServiceType !== undefined)
        selectBtnServiceType.addEventListener("click", (e) => {
            e.preventDefault();
            selectBtnServiceType.classList.toggle("open");
        });
    if (selectBtnPaymentType !== null && selectBtnPaymentType !== undefined)
        selectBtnPaymentType.addEventListener("click", (e) => {
            e.preventDefault();
            selectBtnPaymentType.classList.toggle("open");
        });

    itemsStatusType.forEach(item => {
        item.addEventListener("click", () => {
            item.classList.toggle("checked");
            let checked = document.querySelectorAll(".checked"),
                btnText = document.querySelector(".btn-text-status-type");
            listStatusType = []
            let checked_list = []

            for (var i = 0; i < checked.length; i++) {
                id = checked[i].getAttribute('id')
                if (id.includes('status_type_')) {
                    checked_list.push(checked[i]);
                    listStatusType.push(parseInt(id.replace('status_type_', '')))
                }
            }
            _payment_request_service.SearchParam.statusMulti = listStatusType

            if (checked_list && checked_list.length > 0) {
                btnText.innerText = `${checked_list.length} Selected`;
            } else {
                btnText.innerText = "Tất cả trạng thái";
            }
        });
    })
    itemsServiceType.forEach(item => {
        item.addEventListener("click", () => {
            item.classList.toggle("checked");

            let checked = document.querySelectorAll(".checked"),
                btnText = document.querySelector(".btn-text-service-type");
            let checked_list = []
            listServiceType = []
            for (var i = 0; i < checked.length; i++) {
                id = checked[i].getAttribute('id')
                if (id.includes('service_type_')) {
                    checked_list.push(checked[i]);
                    listServiceType.push(parseInt(id.replace('service_type_', '')))
                }
            }
            _payment_request_service.SearchParam.typeMulti = listServiceType

            if (checked_list && checked_list.length > 0) {
                btnText.innerText = `${checked_list.length} Selected`;
            } else {
                btnText.innerText = "Tất cả loại nghiệp vụ";
            }
        });
    })
    itemsPaymentType.forEach(item => {
        item.addEventListener("click", () => {
            item.classList.toggle("checked");

            let checked = document.querySelectorAll(".checked"),
                btnText = document.querySelector(".btn-text-payment-type");
            let checked_list = []
            listPaymentType = []
            for (var i = 0; i < checked.length; i++) {
                id = checked[i].getAttribute('id')
                if (id.includes('payment_type_')) {
                    checked_list.push(checked[i]);
                    listPaymentType.push(parseInt(id.replace('payment_type_', '')))
                }
            }
            _payment_request_service.SearchParam.paymentTypeMulti = listPaymentType

            if (checked_list && checked_list.length > 0) {
                btnText.innerText = `${checked_list.length} Selected`;
            } else {
                btnText.innerText = "Tất cả hình thức";
            }
        });
    })
    //end multi select
    if (window.location.href.indexOf("PaymentRequest/Index") != -1) {
        var SearchParam = _payment_request_service.GetParam()
        _payment_request_service.Init(SearchParam);
    }

    $(".input").on('keyup', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            _payment_request_service.OnPaging(1)
        }
    });
    setTimeout(function () {
        $('#token-input-client').css('height', 30)
        $('#token-input-supplier').css('height', 30)
    }, 800)

    $("#token-input-client").select2({
        theme: 'bootstrap4',
        placeholder: "Tên KH, Điện Thoại, Email",
        maximumSelectionLength: 1,
        ajax: {
            url: "/Contract/ClientSuggestion",
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
    $("#token-input-supplier").select2({
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
        placeholder: "Người tạo",
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
    $("#approveBy").select2({
        theme: 'bootstrap4',
        placeholder: "Người duyệt",
        ajax: {
            url: "/User/GetUserSuggestionList",
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

});
var _payment_request_service = {
    Init: function (objSearch) {
        $('#divClient').show()
        $('#divSupplier').show()
        if (window.location.href.indexOf("PaymentRequest/Index") != -1) {
            this.SearchParam = objSearch;
            this.Search(objSearch);
        }
    },
    ActionSearch: function (typeSearch) {
        isResetTab = true
        this.OnPaging(1, true)
    },
    GetParam: function () {
        var select_client = $('#token-input-client').select2("val");
        var select_supplier = $('#token-input-supplier').select2("val");
        var createdBy = $('#createdBy').select2("val");
        var approveBy = $('#approveBy').select2("val");
        if (select_client != null) {
            let cookiename = {
                id: select_client[0],
                nameselect: $('#token-input-client').select2('data')[0].text
            }
            _global_function.eraseCookie(cookiePaymentRequest_filter_Client)
            window.localStorage.setItem(cookiePaymentRequest_filter_Client, JSON.stringify(cookiename))
        } else {
            window.localStorage.removeItem(cookiePaymentRequest_filter_Client)
        }
        if (select_supplier != null) {
            let cookiename = {
                id: select_supplier[0],
                nameselect: $('#token-input-supplier').select2('data')[0].text
            }
            _global_function.eraseCookie(cookiePaymentRequest_filter_Supplier)
            window.localStorage.setItem(cookiePaymentRequest_filter_Supplier, JSON.stringify(cookiename))
        } else {
            window.localStorage.removeItem(cookiePaymentRequest_filter_Supplier)
        }
        
        if (createdBy != null) {
            let cookiename = {
                id: createdBy[0],
                nameselect: $('#createdBy').select2('data')[0].text
            }
            _global_function.eraseCookie(cookiePaymentRequest_filter_CreateBy)
            window.localStorage.setItem(cookiePaymentRequest_filter_CreateBy, JSON.stringify(cookiename))
        } else {
            window.localStorage.removeItem(cookiePaymentRequest_filter_CreateBy)
        }
        if (approveBy != null) {
            let cookiename = {
                id: approveBy[0],
                nameselect: $('#approveBy').select2('data')[0].text
            }
            _global_function.eraseCookie(cookiePaymentRequest_filter_VerifyBy)
            window.localStorage.setItem(cookiePaymentRequest_filter_VerifyBy, JSON.stringify(cookiename))
        } else {
            window.localStorage.removeItem(cookiePaymentRequest_filter_VerifyBy)
        }
        var objSearch = {
            paymentCode: $('#billNo').val(),
            content: $('#content').val(),
            serviceCode: $('#serviceCode').val(),
            orderNo: $('#orderNo').val(),
            type: $('#request_type').val(),
            status: -1,
            statusMulti: listStatusType,
            typeMulti: listServiceType,
            paymentTypeMulti: listPaymentType,
            paymentType: $('#payType').val(),
            createByIds: $('#createdBy').select2("val"),
            verifyByIds: $('#approveBy').select2("val"),
            clientId: $('#token-input-client').val(),
            supplierId: $('#token-input-supplier').val(),
            currentPage: 1,
            pageSize: 20
        }
        if ($('#filter_date_create_daterangepicker').data('daterangepicker') !== undefined &&
            $('#filter_date_create_daterangepicker').data('daterangepicker') != null && isPickerCreated) {
            objSearch.fromCreateDateStr = $('#filter_date_create_daterangepicker').data('daterangepicker').startDate._d.toLocaleDateString("en-GB");
            objSearch.toCreateDateStr = $('#filter_date_create_daterangepicker').data('daterangepicker').endDate._d.toLocaleDateString("en-GB");
        } else {
            objSearch.fromCreateDateStr = null
            objSearch.toCreateDateStr = null
        }
        if ($('#filter_date_payment').data('daterangepicker') !== undefined &&
            $('#filter_date_payment').data('daterangepicker') != null && isPickerPayment) {
            objSearch.paymentDateFromStr = $('#filter_date_payment').data('daterangepicker').startDate._d.toLocaleDateString("en-GB");
            objSearch.paymentDateToStr = $('#filter_date_payment').data('daterangepicker').endDate._d.toLocaleDateString("en-GB");
        } else {
            objSearch.paymentDateFromStr = null
            objSearch.paymentDateToStr = null
        }
        if ($('#filter_date_approve').data('daterangepicker') !== undefined &&
            $('#filter_date_approve').data('daterangepicker') != null && isPickerApproved) {
            objSearch.verifyDateFromStr = $('#filter_date_approve').data('daterangepicker').startDate._d.toLocaleDateString("en-GB");
            objSearch.verifyDateToStr = $('#filter_date_approve').data('daterangepicker').endDate._d.toLocaleDateString("en-GB");
        } else {
            objSearch.verifyDateFromStr = null
            objSearch.verifyDateToStr = null
        }
        var isSupplierDebt = $('#isSupplierDebt').val()
        if (parseInt(isSupplierDebt) == -1)
            objSearch.isSupplierDebt = null
        if (parseInt(isSupplierDebt) == 0)
            objSearch.isSupplierDebt = false
        if (parseInt(isSupplierDebt) == 1)
            objSearch.isSupplierDebt = true
        var isPaymentBefore = $('#isPaymentBefore').val()
        if (parseInt(isPaymentBefore) == -1)
            objSearch.isPaymentBefore = null
        if (parseInt(isPaymentBefore) == 0)
            objSearch.isPaymentBefore = false
        if (parseInt(isPaymentBefore) == 1)
            objSearch.isPaymentBefore = true
        return objSearch
    },
    SetCacheFilter: function (objSearch) {
        fields.paymentCode = objSearch.paymentCode
        fields.content = objSearch.content
        fields.serviceCode = objSearch.serviceCode
        fields.orderNo = objSearch.orderNo
        fields.type = objSearch.type
        fields.status = objSearch.status
        fields.paymentType = objSearch.paymentType
        fields.createByIds = objSearch.createByIds
        fields.verifyByIds = objSearch.verifyByIds
        fields.statusMulti = objSearch.statusMulti
        fields.paymentTypeMulti = objSearch.paymentTypeMulti
        fields.typeMulti = objSearch.typeMulti
        fields.clientId = objSearch.clientId
        fields.supplierId = objSearch.supplierId
        fields.currentPage = objSearch.currentPage
        fields.fromCreateDateStr = objSearch.fromCreateDateStr
        fields.toCreateDateStr = objSearch.toCreateDateStr
        fields.paymentDateFromStr = objSearch.paymentDateFromStr
        fields.paymentDateToStr = objSearch.paymentDateToStr
        fields.verifyDateFromStr = objSearch.verifyDateFromStr
        fields.verifyDateToStr = objSearch.verifyDateToStr
        fields.isSupplierDebt = objSearch.isSupplierDebt
        fields.isPaymentBefore = objSearch.isPaymentBefore
        _global_function.setCookie(cookieFilterPaymentRequestName, JSON.stringify(fields), 100)
    },
    GetCacheFilter: function () {
        let cookie = _global_function.getCookie(cookieFilterPaymentRequestName)
        fields = JSON.parse(cookie)
        $('#billNo').val(fields.paymentCode)
        $('#content').val(fields.content)
        $('#serviceCode').val(fields.serviceCode)
        $('#orderNo').val(fields.orderNo)
        if (window.localStorage.getItem(cookiePaymentRequest_filter_Client) != null) {
            var cookie1 = window.localStorage.getItem(cookiePaymentRequest_filter_Client)
            var client = JSON.parse(cookie1)
            $('#token-input-client').html('<option selected value = ' + client.id + '> ' + client.nameselect + '</option>')
            _global_function.eraseCookie(cookiePaymentRequest_filter_Client)
        }
        if (window.localStorage.getItem(cookiePaymentRequest_filter_Supplier) != null) {
            var cookie1 = window.localStorage.getItem(cookiePaymentRequest_filter_Supplier)
            var supplier = JSON.parse(cookie1)
            $('#token-input-supplier').html('<option selected value = ' + supplier.id + '> ' + supplier.nameselect + '</option>')
            _global_function.eraseCookie(cookiePaymentRequest_filter_Supplier)
        }
        if (window.localStorage.getItem(cookiePaymentRequest_filter_CreateBy) != null) {
            var cookie1 = window.localStorage.getItem(cookiePaymentRequest_filter_CreateBy)
            var createBy = JSON.parse(cookie1)
            $('#createdBy').html('<option selected value = ' + createBy.id + '> ' + createBy.nameselect + '</option>')
        }
        if (window.localStorage.getItem(cookiePaymentRequest_filter_VerifyBy) != null) {
            var cookie1 = window.localStorage.getItem(cookiePaymentRequest_filter_VerifyBy)
            var approveBy = JSON.parse(cookie1)
            $('#approveBy').html('<option selected value = ' + approveBy.id + '> ' + approveBy.nameselect + '</option>')
            _global_function.eraseCookie(cookiePaymentRequest_filter_VerifyBy)
        }
        if (fields.paymentTypeMulti != null && fields.paymentTypeMulti !== undefined && fields.paymentTypeMulti.length > 0) {
            var btnTextPaymentType = document.querySelector(".btn-text-payment-type");
            btnTextPaymentType.innerText = `${fields.paymentTypeMulti.length} Selected`;
            for (var i = 0; i < fields.paymentTypeMulti.length; i++) {
                $('#payment_type_' + fields.paymentTypeMulti[i] + '').addClass('checked')
            }
            listPaymentType = fields.paymentTypeMulti
        }
        if (fields.typeMulti != null && fields.typeMulti !== undefined && fields.typeMulti.length > 0) {
            var btnTextService = document.querySelector(".btn-text-service-type");
            btnTextService.innerText = `${fields.typeMulti.length} Selected`;
            for (var i = 0; i < fields.typeMulti.length; i++) {
                $('#service_type_' + fields.typeMulti[i] + '').addClass('checked')
            }
            listServiceType = fields.typeMulti
        }
        if (fields.statusMulti != null && fields.statusMulti !== undefined && fields.statusMulti.length > 0) {
            var btnTextStatus = document.querySelector(".btn-text-status-type");
            btnTextStatus.innerText = `${fields.statusMulti.length} Selected`;
            for (var i = 0; i < fields.statusMulti.length; i++) {
                $('#status_type_' + fields.statusMulti[i] + '').addClass('checked')
            }
            listStatusType = fields.statusMulti
        }
        if (fields.fromCreateDateStr != null && fields.fromCreateDateStr != undefined && fields.fromCreateDateStr !== '') {
            $('input[name="datetimeCreate"]').daterangepicker({
                autoUpdateInput: true,
                autoApply: true,
                showDropdowns: true,
                drops: 'down',
                locale: {
                    format: 'DD/MM/YYYY'
                }
            });
            $('input[name="datetimeCreate"]').data('daterangepicker').setStartDate(fields.fromCreateDateStr);
            $('input[name="datetimeCreate"]').data('daterangepicker').setEndDate(fields.toCreateDateStr);
        }
        if (fields.paymentDateFromStr != null && fields.paymentDateFromStr != undefined && fields.paymentDateFromStr !== '') {
            $('input[name="datetimePayment"]').daterangepicker({
                autoUpdateInput: true,
                autoApply: true,
                showDropdowns: true,
                drops: 'down',
                locale: {
                    format: 'DD/MM/YYYY'
                }
            });
            $('input[name="datetimePayment"]').data('daterangepicker').setStartDate(fields.paymentDateFromStr);
            $('input[name="datetimePayment"]').data('daterangepicker').setEndDate(fields.paymentDateToStr);
        }
        if (fields.verifyDateFromStr != null && fields.verifyDateFromStr != undefined && fields.verifyDateFromStr !== '') {
            $('input[name="datetimeApprove"]').daterangepicker({
                autoUpdateInput: true,
                autoApply: true,
                showDropdowns: true,
                drops: 'down',
                locale: {
                    format: 'DD/MM/YYYY'
                }
            });
            $('input[name="datetimeApprove"]').data('daterangepicker').setStartDate(fields.verifyDateFromStr);
            $('input[name="datetimeApprove"]').data('daterangepicker').setEndDate(fields.verifyDateToStr);
        }

        if (fields.isSupplierDebt) {
            $("#isSupplierDebt").val(0).prop('selected', true);
            $("#select2-isSupplierDebt-container").html(' Có công nợ với NCC ')
        }
        if (fields.isSupplierDebt === false) {
            $("#isSupplierDebt").val(0).prop('selected', true);
            $("#select2-isSupplierDebt-container").html(' Không công nợ với NCC ')
        }
        if (fields.isPaymentBefore) {
            $("#isPaymentBefore").val(0).prop('selected', true);
            $("#select2-isPaymentBefore-container").html(' Đã thanh toán trước ')
        }
        if (fields.isPaymentBefore === false) {
            $("#isPaymentBefore").val(0).prop('selected', true);
            $("#select2-isPaymentBefore-container").html(' Chưa thanh toán trước ')
        }
        //_global_function.eraseCookie(cookieFilterPaymentRequestName)
    },
    BackToList: function () {
        window.location.href = '/PaymentRequest/Index'
    },
    Export: function () {
        $('#btnExport').prop('disabled', true);
        $('#icon-export').removeClass('fa-file-excel-o');
        $('#icon-export').addClass('fa-spinner fa-pulse');
        var objSearch = this.GetParam()
        objSearch.currentPage = 1;
        this.SearchParam = objSearch
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentRequest/ExportExcel",
            type: "Post",
            data: this.SearchParam,
            success: function (result) {
                _global_function.RemoveLoading()
                $('#btnExport').prop('disabled', false);
                if (result.isSuccess) {
                    _msgalert.success(result.message);
                    window.location.href = result.path;
                } else {
                    _msgalert.error(result.message);
                }
                $('#icon-export').removeClass('fa-spinner fa-pulse');
                $('#icon-export').addClass('fa-file-excel-o');
            }
        });
    },
    Search: function (input, is_count_status = true, isClickSearch = false) {
        window.scrollTo(0, 0);
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentRequest/Search",
            type: "Post",
            data: input,
            success: function (result) {
                _global_function.RemoveLoading()
                $('#grid_data').html(result);
                if (isClickSearch) {
                    var modelCache = _payment_request_service.GetParam()
                    _payment_request_service.SetCacheFilter(modelCache)
                } else {

                }

            }
        });
        if (is_count_status) {
            this.OnCountStatus()
            this.SetActive(-1)
        }
    },
    OnPaging: function (value, isClickSearch) {
        var objSearch = this.GetParam()
        objSearch.currentPage = value;
        this.SearchParam = objSearch
        this.Search(objSearch, true, isClickSearch);
    },
    OnChangeAccountClientId: function (value) {
        listUserCreate = []
        var searchobj = this.SearchParam;
        var listId = value.split(',')
        searchobj.createByIds = []
        for (var i = 0; i < listId.length; i++) {
            searchobj.createByIds.push(parseInt(listId[i]))
        }
        searchobj.currentPage = 1;
        listUserCreate = searchobj.createByIds
        this.SearchParam = searchobj;
        this.Search(searchobj);
    },
    OnChangeClient: function (value) {
        var searchobj = this.SearchParam;
        var listId = value.split(',')
        searchobj.clientId = null
        for (var i = 0; i < listId.length; i++) {
            searchobj.createByIds.push(parseInt(listId[i]))
        }
        searchobj.currentPage = 1;
        this.SearchParam = searchobj;
        this.Search(searchobj);
    },
    Add: function (serviceId, serviceType, supplierId, amount, orderId, serviceCode, clientId, amount_supplier_refund) {
        let title = 'Thêm phiếu yêu cầu chi';
        let url = '/PaymentRequest/AddNew';
        var param = {
            'serviceId': serviceId,
            'serviceType': serviceType,
            'serviceCode': serviceCode,
            'supplierId': supplierId,
            'amount_supplier_refund': amount_supplier_refund,
            'amount': amount,
            'orderId': orderId,
            'clientId': clientId,
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    Edit: function (paymentRequestId, serviceId, serviceType, supplierId, amount, orderId, clientId, amount_supplier_refund) {
        let title = 'Chỉnh sửa phiếu yêu cầu chi';
        let url = '/PaymentRequest/Edit?paymentRequestId=' + paymentRequestId + "&service_type=";
        var param = {
            'serviceId': serviceId,
            'serviceType': serviceType,
            'amount_supplier_refund': amount_supplier_refund,
            'supplierId': supplierId,
            'amount': amount,
            'orderId': orderId,
            'clientId': clientId,
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    EditAdmin: function (paymentRequestId, serviceId, serviceType, supplierId, amount, orderId, clientId, amount_supplier_refund) {
        let title = 'Admin chỉnh sửa phiếu yêu cầu chi';
        let url = '/PaymentRequest/EditAdmin?paymentRequestId=' + paymentRequestId + "&service_type=";
        var param = {
            'serviceId': serviceId,
            'serviceType': serviceType,
            'supplierId': supplierId,
            'amount_supplier_refund': amount_supplier_refund,
            'amount': amount,
            'orderId': orderId,
            'clientId': clientId,
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    OnCountStatus: function () {
        var objSearch = this.GetParam();
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentRequest/CountStatus",
            type: "Post",
            data: objSearch,
            success: function (result) {
                _global_function.RemoveLoading()
                $('#countSttDraft').text('Lưu nháp (0)')
                $('#countSttWaitDepartmentLeadApprove').text('Chờ TBP duyệt (0)')
                $('#countSttWaitDirectorApprove').text('Chờ KTT duyệt (0)')
                $('#countSttReject').text('Từ chối (0)')
                $('#countSttApprove').text('Chờ chi (0)')
                $('#countSttPayment').text('Đã chi (0)')
                $('#countSttAll').text('Tất cả (0)')
                if (result.data.length > 0) {
                    for (var i = 0; i < result.data.length; i++) {
                        if (result.data[i].status == -1) {
                            $('#countSttAll').text('Tất cả (' + result.data[i].total + ')')
                        }
                        if (result.data[i].status == 0) {
                            $('#countSttDraft').text('Lưu nháp (' + result.data[i].total + ')')
                        }
                        if (result.data[i].status == 1) {
                            $('#countSttReject').text('Từ chối (' + result.data[i].total + ')')
                        }
                        if (result.data[i].status == 2) {
                            $('#countSttWaitDepartmentLeadApprove').text('Chờ TBP duyệt (' + result.data[i].total + ')')
                        }
                        if (result.data[i].status == 3) {
                            $('#countSttWaitDirectorApprove').text('Chờ KTT duyệt (' + result.data[i].total + ')')
                        }
                        if (result.data[i].status == 4) {
                            $('#countSttApprove').text('Chờ chi (' + result.data[i].total + ')')
                        }
                        if (result.data[i].status == 5) {
                            $('#countSttPayment').text('Đã chi (' + result.data[i].total + ')')
                        }
                    }
                }
            }
        });
    },
    OnSearchStatus: function (status) {
        $('#status').val(-1)
        isResetTab = false
        var objSearch = this.SearchParam;
        objSearch.currentPage = 1;
        objSearch.status = status;
        this.Search(objSearch, false);
        this.SetActive(status)
    },
    OnChangeRequestType: function () {
        var type = $('#request_type').val()
        if (type === '1' || type === '2') {
            $('#divClient').hide()
            $('#divSupplier').show()
        }
        if (type === '3') {
            $('#divClient').show()
            $('#divSupplier').hide()
        }
    },
    SetActive: function (status) {
        var objSearch = this.SearchParam
        if (!isResetTab && objSearch.currentPage > 1)
            return
        let status_choose = $('#status').val()
        $('#countSttAll').removeClass('active')
        $('#countSttDraft').removeClass('active')
        $('#countSttWaitDepartmentLeadApprove').removeClass('active')
        $('#countSttWaitDirectorApprove').removeClass('active')
        $('#countSttApprove').removeClass('active')
        $('#countSttReject').removeClass('active')
        $('#countSttPayment').removeClass('active')
        if (status == -1) {
            $('#countSttAll').addClass('active')
        }
        if (status == 0) {
            $('#countSttDraft').addClass('active')
            if (status_choose !== undefined && status_choose !== null && status_choose !== '' && status_choose !== '1')
                $('#countSttDraft').addClass('disabled-a-tag')
            else
                $('#countSttDraft').removeClass('disabled-a-tag')
        }
        if (status == 2) {
            $('#countSttWaitDepartmentLeadApprove').addClass('active')
            if (status_choose !== undefined && status_choose !== null && status_choose !== '' && status_choose !== '2')
                $('#countSttWaitDepartmentLeadApprove').addClass('disabled-a-tag')
            else
                $('#countSttWaitDepartmentLeadApprove').removeClass('disabled-a-tag')
        }
        if (status == 3) {
            $('#countSttWaitDirectorApprove').addClass('active')
            if (status_choose !== undefined && status_choose !== null && status_choose !== '' && status_choose !== '3')
                $('#countSttWaitDirectorApprove').addClass('disabled-a-tag')
            else
                $('#countSttWaitDirectorApprove').removeClass('disabled-a-tag')
        }
        if (status == 1) {
            $('#countSttReject').addClass('active')
            if (status_choose !== undefined && status_choose !== null && status_choose !== '' && status_choose !== '1')
                $('#countSttReject').addClass('disabled-a-tag')
            else
                $('#countSttReject').removeClass('disabled-a-tag')
        }
        if (status == 4) {
            $('#countSttApprove').addClass('active')
            if (status_choose !== undefined && status_choose !== null && status_choose !== '' && status_choose !== '4')
                $('#countSttApprove').addClass('disabled-a-tag')
            else
                $('#countSttApprove').removeClass('disabled-a-tag')
        }
        if (status == 5) {
            $('#countSttPayment').addClass('active')
            if (status_choose !== undefined && status_choose !== null && status_choose !== '' && status_choose !== '4')
                $('#countSttPayment').addClass('disabled-a-tag')
            else
                $('#countSttPayment').removeClass('disabled-a-tag')
        }
    },
    Approve: function (status) {
        _msgconfirm.openDialog("Phê duyệt yêu cầu chi", "Bạn có chắc muốn phê duyệt mã phiếu yêu cầu chi " + $('#paymentRequestNo').val() + " không?", function () {
            $.ajax({
                url: "/PaymentRequest/Approve",
                type: "Post",
                data: { 'paymentRequestNo': $('#paymentRequestNo').val(), 'status': status },
                success: function (result) {
                    if (result.isSuccess === true) {
                        _msgalert.success(result.message);
                        $.magnificPopup.close();
                        setTimeout(function () {
                            window.location.reload()
                        }, 1000)
                    } else {
                        _msgalert.error(result.message);
                    }
                }
            });
        });
    },
    Reject: function () {
        let title = 'Từ chối duyệt yêu cầu';
        let url = '/PaymentRequest/Reject';
        var param = {
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    RejectDeposit: function () {
        if ($('#note').val() === null || $('#note').val() === undefined || $('#note').val() === '') {
            _msgalert.error('Vui lòng nhập lý do từ chối');
            return
        }
        var note = $('#note').val()
        if (note.length > 300) {
            _msgalert.error('Nội dung từ chối không được quá 300 kí tự');
            return
        }
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentRequest/RejectRequest",
            type: "Post",
            data: { 'paymentRequestNo': $('#paymentRequestNo').val(), 'note': $('#note').val() },
            success: function (result) {
                if (result.isSuccess === true) {
                    _msgalert.success(result.message);
                    $.magnificPopup.close();
                    setTimeout(function () {
                        window.location.reload()
                    }, 1000)
                } else {
                    _msgalert.error(result.message);
                }
            }
        });
    },
    Delete: function (paymentRequestNo, isDetail = true, serviceType = 0, clientIdService = 0) {

        if (paymentRequestNo !== undefined && paymentRequestNo !== null && paymentRequestNo !== '')
            $('#paymentRequestNo').val(paymentRequestNo)
        else
            paymentRequestNo = $('#paymentRequestNo').val()
        _msgconfirm.openDialog("Xác nhận xóa yêu cầu chi", "Bạn có chắc muốn xóa phiếu yêu cầu chi " + paymentRequestNo + " không?", function () {
            $.ajax({
                url: "/PaymentRequest/Delete",
                type: "Post",
                data: { 'paymentRequestNo': paymentRequestNo },
                success: function (result) {
                    if (result.isSuccess === true) {
                        _msgalert.success(result.message);
                        $.magnificPopup.close();
                        if (isDetail) {
                            setTimeout(function () {
                                window.location.href = "/PaymentRequest/Index"
                            }, 1000)
                            return
                        }

                        if (serviceType !== 0) {
                            setTimeout(function () {
                                if (serviceType == 3) { // vé máy bay
                                    _set_service_fly_detail.ShowPaymentTab()
                                }
                                else if (serviceType == 1) { // Khách sạn
                                    _SetService_Detail.loadTourDetail()
                                }
                                else if (serviceType == 5) { // Tour
                                    _SetService_Tour_Detail.ShowTourPaymentTab();
                                }
                                else if (serviceType == 9) { // Tour
                                    _set_service_other_detail.ShowPaymentTab();
                                }
                                else {
                                    window.location.reload()
                                }
                            }, 500)
                        }
                        if (clientIdService != 0) {
                            if (serviceType == 3) { // vé máy bay
                                _set_service_fly_detail.ShowRefundTab()
                            }
                            else if (serviceType == 1) { // Khách sạn
                                _SetService_Detail.loadListHotelBookingRefund()
                            }
                            else if (serviceType == 5) { // Tour
                                _SetService_Tour_Detail.TourServiceRefund(1);
                            }
                            else if (serviceType == 9) { // Tour
                                _set_service_ws_detail.ShowPaymentTab();
                            }
                            else {
                                window.location.reload()
                            }
                        }
                    } else {
                        _msgalert.error(result.message);
                    }
                }
            });
        });
    },
    UndoApprove: function (status) {
        let title = 'Bỏ duyệt yêu cầu';
        let url = '/PaymentRequest/UndoApprove';
        var param = {
            'status': status
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    UndoApproveRequest: function (status) {
        if ($('#note').val() === null || $('#note').val() === undefined || $('#note').val() === '') {
            _msgalert.error('Vui lòng nhập lý do bỏ duyệt');
            return
        }
        var note = $('#note').val()
        if (note.length > 300) {
            _msgalert.error('Nội dung bỏ duyệt không được quá 300 kí tự');
            return
        }
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentRequest/UndoApproveRequest",
            type: "Post",
            data: {
                'paymentRequestNo': $('#paymentRequestNo').val(),
                'note': $('#note').val(),
                'status': $('#status_undo_approve').val(),
            },
            success: function (result) {
                _global_function.RemoveLoading()
                if (result.isSuccess === true) {
                    _msgalert.success(result.message);
                    $.magnificPopup.close();
                    setTimeout(function () {
                        window.location.reload()
                    }, 1000)
                } else {
                    _msgalert.error(result.message);
                }
            }
        });
    },
}