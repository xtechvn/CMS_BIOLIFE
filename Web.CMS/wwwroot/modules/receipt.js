let isPickerCreate = false;
let listUserCreate = []
$(document).ready(function () {
    $('input').attr('autocomplete', 'off');
    $('input').keyup(function (e) {
        if (e.which === 13) {
            _receipt_service.OnPaging(1);
        }
    });
    //end multi select
    var SearchParam = _receipt_service.GetParam()
    _receipt_service.Init(SearchParam);
    $(".input").on('keyup', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            _receipt_service.OnPaging(1)
        }
    });
    setTimeout(function () {
        $('#token-input-user-create').css('min-width', 200)
        $('#token-input-user-create').css('height', 30)
        $('#token-input-user-create').css('min-width', 200)
        $('#token-input-client').css('height', 30)
    }, 800)

    $('#token-input-user-create').tokenInput('/Funding/GetUserCreateSuggest', {
        queryParam: "name",
        hintText: "Nhập từ khóa tìm kiếm",
        searchingText: "Đang tìm kiếm...",
        placeholder: 'Người tạo',
        searchDelay: 500,
        preventDuplicates: true,
        minChars: 4,
        noResultsText: "Không tìm thấy kết quả",
        tokenLimit: 10,
        onAdd: function (item) {
            _receipt_service.OnChangeAccountClientId($(this).val());
        },
        onDelete: function (item) {
            _receipt_service.OnChangeAccountClientId($(this).val());
        }
    });

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
    $("#createdBy_search").select2({
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
    $("#employee_search").select2({
        theme: 'bootstrap4',
        placeholder: "Nhân viên",
        maximumSelectionLength: 1,
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
var _receipt_service = {
    Init: function (objSearch) {
        this.SearchParam = objSearch;
        this.Search(objSearch);
    },
    GetParam: function () {
        var objSearch = {
            billNo: $('#billNo').val(),
            content: $('#content').val(),
            serviceCode: $('#serviceCode').val(),
            type: $('#type').val(),
            payType: $('#payType').val(),
            createByName: $('#createdBy_search').val(),
            createByIds: $('#createdBy_search').select2("val"),
            clientId: $('#token-input-client').val(),
            supplierId: $('#token-input-supplier').val(),
            employeeId: $('#employee_search').val(),
            currentPage: 1,
            pageSize: 20
        }
        if ($('#filter_date_create_daterangepicker').data('daterangepicker') !== undefined &&
            $('#filter_date_create_daterangepicker').data('daterangepicker') != null && isPickerCreate) {
            objSearch.fromCreateDateStr = $('#filter_date_create_daterangepicker').data('daterangepicker').startDate._d.toLocaleDateString("en-GB");
            objSearch.toCreateDateStr = $('#filter_date_create_daterangepicker').data('daterangepicker').endDate._d.toLocaleDateString("en-GB");
        } else {
            objSearch.fromCreateDateStr = null
            objSearch.toCreateDateStr = null
        }
        return objSearch
    },
    BackToList: function () {
        window.location.href = '/Receipt/Index'
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
            url: "/Receipt/ExportExcel",
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
            url: "/Receipt/Search",
            type: "Post",
            data: input,
            success: function (result) {
                _global_function.RemoveLoading()
                //$('#imgLoading').hide();
                $('#grid_data').html(result);
            }
        });
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
    AddContractPay: function () {
        let title = 'Thêm phiếu thu';
        let url = '/Receipt/AddNew';
        var param = {
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    CreateContractPayJson: function (depositId) {
        _global_function.AddLoading()
        $.ajax({
            url: "/Receipt/AddContractPayJson",
            type: "Post",
            data: {
                'depositId': depositId, 'contractPayId': $('#contractPay_id').val(), 'totalAmount': $('#amount_add_contract_get').val()
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
    EditContractPay: function (payId) {
        let title = 'Chỉnh sửa phiếu thu';
        let url = '/Receipt/Edit?payId=' + payId;
        var param = {
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    EditContractPayAdmin: function (payId) {
        let title = 'Chỉnh sửa phiếu thu';
        let url = '/Receipt/EditAdmin?payId=' + payId;
        var param = {
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    EditContractPayJson: function (depositId) {
        _global_function.AddLoading()
        $.ajax({
            url: "/Receipt/AddContractPayJson",
            type: "Post",
            data: {
                'depositId': depositId, 'contractPayId': $('#contractPay_id').val(), 'totalAmount': $('#amount_add_contract_get').val()
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
    CreateOrderManual: function () {
        if ($('#create_contract_pay').length) {
            $('#create_contract_pay').removeClass('show')
            setTimeout(function () {
                $('#create_contract_pay').remove();
            }, 300);

        }
        //$.ajax({
        //    url: "CreateOrderManual",
        //    type: "post",
        //    data: {},
        //    success: function (result) {
        //        $('body').append(result);
        //        setTimeout(function () {
        //            $('#create_order_manual').addClass('show')
        //        }, 300);

        //    }
        //});
    },
    OnDelete: function (payId, billNo) {
        _msgconfirm.openDialog("Xóa phiếu thu", "Bạn có chắc muốn xóa phiếu thu " + billNo + " không", function () {
            $.ajax({
                url: "/Receipt/Delete",
                type: "Post",
                data: { 'payId': payId },
                success: function (result) {
                    if (result.isSuccess === true) {
                        _msgalert.success(result.message);
                        $.magnificPopup.close();
                        setTimeout(function () {
                            window.location.href = "/Receipt/Index"
                        }, 1000)
                    } else {
                        _msgalert.error(result.message);
                    }
                }
            });
        });
    }
}