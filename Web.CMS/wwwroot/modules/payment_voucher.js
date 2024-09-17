let isPickerCreate_payment = false;
let listUserCreate = []
var listServiceType = []
var listPaymentType = []
$(document).ready(function () {
    $('input').attr('autocomplete', 'off');
    $('input').keyup(function (e) {
        if (e.which === 13) {
            _payment_voucher_service.OnPaging(1);
        }
    });
    //end multi select
    var SearchParam = _payment_voucher_service.GetParam()
    _payment_voucher_service.Init(SearchParam);
    $(".input").on('keyup', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            _payment_voucher_service.OnPaging(1)
        }
    });
    setTimeout(function () {
        $('#token-input-client').css('height', 30)
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

    //multi select
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
            _payment_voucher_service.SearchParam.typeMulti = listServiceType

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
            _payment_voucher_service.SearchParam.paymentTypeMulti = listPaymentType

            if (checked_list && checked_list.length > 0) {
                btnText.innerText = `${checked_list.length} Selected`;
            } else {
                btnText.innerText = "Tất cả hình thức";
            }
        });
    })
});
var _payment_voucher_service = {
    Init: function (objSearch) {
        $('#divClient').show()
        $('#divSupplier').show()
        this.SearchParam = objSearch;
        this.Search(objSearch);
    },
    GetParam: function () {
        var objSearch = {
            paymentCode: $('#billNo').val(),
            content: $('#content').val(),
            type: $('#payment_voucher_type').val(),
            paymentType: $('#payType').val(),
            bankingAccountSource: $('#bankingAccountSource').val(),
            typeMulti: listServiceType,
            paymentTypeMulti: listPaymentType,
            createByIds: $('#createdBy').select2("val"),
            clientId: $('#token-input-client').val() != null && $('#token-input-client').val() !== undefined &&
                ($('#token-input-client').val()).length > 0 ? ($('#token-input-client').val())[0] : 0,
            supplierId: $('#token-input-supplier').val() != null && $('#token-input-supplier').val() !== undefined &&
                ($('#token-input-supplier').val()).length > 0 ? ($('#token-input-supplier').val())[0] : 0,
            currentPage: 1,
            pageSize: 20
        }
        if ($('#filter_date_create_daterangepicker').data('daterangepicker') !== undefined &&
            $('#filter_date_create_daterangepicker').data('daterangepicker') != null && isPickerCreate_payment) {
            objSearch.createdDateFromStr = $('#filter_date_create_daterangepicker').data('daterangepicker').startDate._d.toLocaleDateString("en-GB");
            objSearch.createdDateToStr = $('#filter_date_create_daterangepicker').data('daterangepicker').endDate._d.toLocaleDateString("en-GB");
        } else {
            objSearch.createdDateFromStr = null
            objSearch.createdDateToStr = null
        }
        return objSearch
    },
    BackToList: function () {
        window.location.href = '/PaymentVoucher/Index'
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
            url: "/PaymentVoucher/ExportExcel",
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
    Search: function (input) {
        window.scrollTo(0, 0);
        //$('#imgLoading').show();
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentVoucher/Search",
            type: "Post",
            data: input,
            success: function (result) {
                _global_function.RemoveLoading()
                //$('#imgLoading').hide();
                $('#grid_data').html(result);
            }
        });
    },
    OnChangeRequestType: function () {
        var type = $('#payment_voucher_type').val()
        if (type === '1' || type === '2') {
            $('#divClient').hide()
            $('#divSupplier').show()
        }
        if (type === '3') {
            $('#divClient').show()
            $('#divSupplier').hide()
        }
    },
    OnPaging: function (value) {
        var objSearch = this.GetParam()
        objSearch.currentPage = value;
        this.SearchParam = objSearch
        this.Search(objSearch);
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
    AddPaymentVoucher: function () {
        let title = 'Thêm phiếu chi';
        let url = '/PaymentVoucher/AddNew';
        var param = {
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    EditPaymentVoucher: function (id) {
        let title = 'Chỉnh sửa phiếu chi';
        let url = '/PaymentVoucher/Edit?paymentVoucherId=' + id;
        var param = {
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    FileAttachment: function (data_id, type, readonly = false, allowPreview = true) {
        _global_function.RenderFileAttachment($('.attachment_file'), data_id, type, readonly, allowPreview)
    },
}