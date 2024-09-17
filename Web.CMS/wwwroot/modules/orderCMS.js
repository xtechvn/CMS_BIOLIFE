let orderFields = {
    OrderNo: true,
    DateOrder: true,
    ClientOrder: true,
    NoteOrder: true,
    PayOrder: true,
    UtmSource: false,
    ProfitOrder: true,
    SttOrder: true,
    StartDateOrder: true,
    CreatedName: false,
    UpdatedDate: false,
    UpdatedName: false,
    MainEmp: false,
    SubEmp: false,
    Voucher: false,
    Operator: false,
    HINHTHUCTT: true,
    KHACHPT: false,
    tum_medium: true,
    HINHTHUCTTb: true,
    YcHoaDon: true,


}
let cookieName = 'orderFields_transactionsms';
let cookieFilterName = 'orderFields_filter';
let cookieSaleName = 'cookieSaleName_filter';
let cookieOperatorName = 'cookieOperatorName_filter';
let cookieOrderNo = 'cookieOrderNo_filter';
let cookieBoongkingCode = 'cookieBoongkingcode_filter';
let cookieClient = 'cookieClient_filter';
let cookiename;
var timer;
let isPicker = false;
let listServiceType = [];
let listService = [];
let listStatus = [];
let listHINHTHUCTT = [];
let tabActive = 99;
let PageIndex = 1;

$(document).ready(function () {
    $("body").on('click', ".reset-order-no", function (ev, picker) {
        $('#OrderNo').val(null).trigger('change')
        $('.reset-order-no').hide()

    })
    $('#OrderNo').on("select2:select", function (e) {
        $('.reset-order-no').show()

    });
    $('input[name="datetimeOrder"]').daterangepicker({
        //timePicker: true,
        //startDate: '',
        //endDate: '',
        maxDate: new Date(),
        //locale: {
        //    format: 'DD-MM-YYYY'
        //},
        //autoUpdateInput: true,
        autoUpdateInput: false,
        locale: {
            cancelLabel: 'Clear'
        }
    });
    $('input[name="datetimeOrder"]').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
        isPicker = true;
    });
    $('input[name="datetimeOrder"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        isPicker = false;
    });
    _ordersCMS.Init();

    $("#OrderNo").select2({
        theme: 'bootstrap4',
        placeholder: "Mã đơn hàng",
        /* tags: true,*/
        ajax: {
            url: "/OrderManual/OrderNoSuggestion",
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
                            text: item.orderno,
                            id: item.orderno.toUpperCase(),
                        }
                    })
                };
            },
            cache: true
        }
    });


    $("#CreateName").select2({
        theme: 'bootstrap4',
        placeholder: "Người tạo",
        maximumSelectionLength: 1,
        ajax: {
            url: "/OrderManual/UserSuggestion",
            type: "post",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                var query = {
                    txt_search: params.term,
                }

                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (response) {
                return {
                    results: $.map(response.data, function (item) {
                        return {
                            text: item.fullname + ' - ' + item.email,
                            id: item.id,
                        }
                    })
                };
            },
            cache: true
        }
    });

    $("#ClientId").select2({
        theme: 'bootstrap4',
        placeholder: "Thông tin khách hàng",
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

                // Query parameters will be ?search=[term]&type=public
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
    $("#OperatorId").select2({
        theme: 'bootstrap4',
        placeholder: "Điều hành viên",
        maximumSelectionLength: 1,
        ajax: {
            url: "/OrderManual/UserSuggestion",
            type: "post",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                var query = {
                    txt_search: params.term,
                }

                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (response) {
                return {
                    results: $.map(response.data, function (item) {
                        return {
                            text: item.fullname + ' - ' + item.email,
                            id: item.id,
                        }
                    })
                };
            },
            cache: true
        }
    });
    $("#BoongKingCode").select2({
        theme: 'bootstrap4',
        placeholder: "Mã code dịch vụ",
        maximumSelectionLength: 1,
        ajax: {
            url: "/Order/BoongKingCodeSuggestion",
            type: "post",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                var query = {
                    txt_search: params.term,
                }

                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (response) {
                return {
                    results: $.map(response.data, function (item) {
                        return {
                            text: item.bookingcode,
                            id: item.listorderid,
                        }
                    })
                };
            },
            cache: true
        }
    });
    if (_ordersCMS.getCookie('orderFields_transactionsms') != null) {
        let cookie = _ordersCMS.getCookie(cookieName)
        orderFields = JSON.parse(cookie)
    } else {
        _ordersCMS.setCookie(cookieName, JSON.stringify(orderFields), 10)
    }
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
    _ordersCMS.checkShowHide();
    _ordersCMS.setValueFilter();
    //const selectBtnServiceType = document.querySelector(".select-btn-service-type");
    //const itemsServiceType = document.querySelectorAll(".item-service-type");

    const selectBtnService = document.querySelector(".select-service-type");
    const itemsService = document.querySelectorAll(".item-services");

    const selectBtnStatus = document.querySelector(".select-btn-status");
    const itemsStatus = document.querySelectorAll(".item-status");

    const selectBtnHINHTHUCTT = document.querySelector(".select-btn-HINHTHUCTT");
    const itemsHINHTHUCTT = document.querySelectorAll(".item-HINHTHUCTT");

    $(document).click(function (event) {
        var $target = $(event.target);
        //if (!$target.closest('#utmSourde').length) {
        //    if ($('#list-item-service').is(":visible") && !$target[0].id.includes('service_type') && !$target[0].id.includes('service_type_text')
        //        && !$target[0].id.includes('list-item-service') && !$target[0].id.includes('checkbox_service_item')) {
        //        selectBtnServiceType.classList.toggle("open");
        //    }
        //}
        if (!$target.closest('#ServiceType').length) {
            if ($('#list-item-service-type').is(":visible") && !$target[0].id.includes('service_data') && !$target[0].id.includes('service_type')
                && !$target[0].id.includes('list-item-service-type') && !$target[0].id.includes('checkbox_service_type')) {
                selectBtnService.classList.toggle("open");
            }
        }
        if (!$target.closest('#Status').length) {
            if ($('#list-item-status').is(":visible") && !$target[0].id.includes('status_data') && !$target[0].id.includes('checkbox_status')
                && !$target[0].id.includes('list-item-status') && !$target[0].id.includes('status_text')) {
                selectBtnStatus.classList.toggle("open");
            }
            //if ($('#list-item-status').is(":visible") && !$target.id.includes('status_data')) {
            //    selectBtnStatus.classList.toggle("open");
            //}
        }
        if (!$target.closest('#HINHTHUCTT').length) {
            if ($('#list-item-HINHTHUCTT').is(":visible") && !$target[0].id.includes('HINHTHUCTT_data') && !$target[0].id.includes('checkbox_HINHTHUCTT')
                && !$target[0].id.includes('list-item-HINHTHUCTT') && !$target[0].id.includes('HINHTHUCTT_text')) {
                selectBtnHINHTHUCTT.classList.toggle("open");
            }

        }

    });
    //selectBtnServiceType.addEventListener("click", (e) => {
    //    e.preventDefault();
    //    selectBtnServiceType.classList.toggle("open");
    //});
    selectBtnService.addEventListener("click", (e) => {
        e.preventDefault();
        selectBtnService.classList.toggle("open");
    });
    selectBtnStatus.addEventListener("click", (e) => {
        e.preventDefault();
        selectBtnStatus.classList.toggle("open");
    });
    selectBtnHINHTHUCTT.addEventListener("click", (e) => {
        e.preventDefault();
        selectBtnHINHTHUCTT.classList.toggle("open");
    });
    //itemsServiceType.forEach(item => {
    //    item.addEventListener("click", () => {
    //        item.classList.toggle("checked");

    //        let checked = document.querySelectorAll("#list-item-service .checked"),
    //            btnText = document.querySelector(".btn-text-service-type");
    //        let checked_list = []
    //        listServiceType = []
    //        for (var i = 0; i < checked.length; i++) {
    //            id = checked[i].getAttribute('id')
    //            if (id.includes('service_type_')) {
    //                checked_list.push(checked[i]);
    //            }
    //            listServiceType.push(parseInt(id.replace('service_type_', '')))
    //        }

    //        if (listServiceType && listServiceType.length > 0) {
    //            btnText.innerText = `${listServiceType.length} Selected`;
    //        } else {
    //            btnText.innerText = "Tất cả nguồn";
    //        }
    //    });
    //})
    itemsService.forEach(item => {
        item.addEventListener("click", () => {
            item.classList.toggle("checked");

            let checked = document.querySelectorAll("#list-item-service-type .checked"),
                btnText = document.querySelector(".btn-text-service");
            let checked_list = []
            listService = []
            for (var i = 0; i < checked.length; i++) {
                id = checked[i].getAttribute('id')
                if (listService && id.includes('service_data_')) {
                    checked_list.push(checked[i]);
                }
                listService.push(parseInt(id.replace('service_data_', '')))

            }
            if (listService.length > 0) {
                btnText.innerText = `${listService.length} Selected`;
            } else {
                btnText.innerText = "Tất cả dịch vụ";
            }


        });
    })

    itemsStatus.forEach(item => {
        item.addEventListener("click", () => {
            item.classList.toggle("checked");

            let checked = document.querySelectorAll("#list-item-status .checked"),
                btnText = document.querySelector(".btn-text-status");
            let checked_list = []
            listStatus = []
            for (var i = 0; i < checked.length; i++) {
                id = checked[i].getAttribute('id')
                if (id.includes('status_data_')) {
                    checked_list.push(checked[i]);
                }
                listStatus.push(parseInt(id.replace('status_data_', '')))
            }
            if (listStatus && listStatus.length > 0) {
                btnText.innerText = `${listStatus.length} Selected`;
            } else {
                btnText.innerText = "Tất cả trạng thái";
            }
        })
    })
    itemsHINHTHUCTT.forEach(item => {
        item.addEventListener("click", () => {
            item.classList.toggle("checked");

            let checked = document.querySelectorAll("#list-item-HINHTHUCTT .checked"),
                btnTextHINHTHUCTT = document.querySelector(".btn-text-HINHTHUCTT");
            let checked_list = []
            listHINHTHUCTT = []
            for (var i = 0; i < checked.length; i++) {
                id = checked[i].getAttribute('id')
                if (id.includes('HINHTHUCTT_data_')) {
                    checked_list.push(checked[i]);
                }

                listHINHTHUCTT.push((id.replace('HINHTHUCTT_data_', '')))
            }
            if (listHINHTHUCTT && listHINHTHUCTT.length > 0) {
                btnTextHINHTHUCTT.innerText = `${listHINHTHUCTT.length} Selected`;
            } else {
                btnTextHINHTHUCTT.innerText = "Tất cả hình thức thanh toán";
            }
        })
    })
});

var _ordersCMS = {
    FirstLoading: false,
    Init: function () {

        var today = new Date();
        var yyyy = today.getFullYear();
        var mm = today.getMonth() + 1; // Months start at 0!
        var dd = today.getDate();
        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        var min_range = '01/01/2020';
        var max_range = dd + '/' + mm + '/' + (yyyy + 5);

        $('.date-range-filter').each(function (index, item) {
            var element = $(item)
            element.daterangepicker({
                autoApply: true,
                autoUpdateInput: false,
                showDropdowns: true,
                drops: 'down',
                minDate: min_range,
                maxDate: max_range,
                locale: {
                    format: 'DD/MM/YYYY'
                }
            });
            element.data('daterangepicker').setStartDate(min_range);
            element.data('daterangepicker').setEndDate(max_range);
        })
        $("body").on('apply.daterangepicker', ".date-range-filter", function (ev, picker) {
            var element = $(this)
            element.val(_global_function.GetDayText(element.data('daterangepicker').startDate._d).split(' ')[0] + ' - ' + _global_function.GetDayText(element.data('daterangepicker').endDate._d).split(' ')[0])

        });

        let _searchModel = {
            OrderNo: null,
            StartDateFrom: null,
            StartDateTo: null,
            EndDateFrom: null,
            EndDateTo: null,
            Note: null,
            UtmSource: listServiceType,
            ServiceType: listService,
            Status: listStatus,
            CreateTime: null,
            CreateName: null,
            HINHTHUCTT: listHINHTHUCTT,
            Sale: null,
            BoongKingCode: null,
            sysTemType: -1,
            StatusTab: 99,
            PageIndex: 1,
            pageSize: 20,
        };

        let objSearch = {
            searchModel: _searchModel,
            currentPage: 1,
            pageSize: 50
        };
        if (!_ordersCMS.FirstLoading) {
            _ordersCMS.FirstLoading = true
            /*            objSearch.searchModel.Status=[0,1,2,3,4,5,6,7]*/

        }

        this.SearchParam = objSearch;
        this.Search(objSearch);
    },

    Search: function (input) {

        if (_ordersCMS.getCookie(cookieFilterName) != null) {
            let cookie = _ordersCMS.getCookie(cookieFilterName)
            input = JSON.parse(cookie)
            this.SearchParam = input
            if (input.searchModel.Status.length > 0) {
                var btnTextstatus = document.querySelector(".btn-text-status");
                btnTextstatus.innerText = `${input.searchModel.Status.length} Selected`;
                for (var i = 0; i < input.searchModel.Status.length; i++) {
                    $('#status_data_' + input.searchModel.Status[i] + '').addClass('checked')
                }
            }
            if (input.searchModel.ServiceType.length > 0) {
                var btnTextservice = document.querySelector(".btn-text-service");
                btnTextservice.innerText = `${input.searchModel.ServiceType.length} Selected`;
                for (var i = 0; i < input.searchModel.ServiceType.length; i++) {
                    $('#service_data_' + input.searchModel.ServiceType[i] + '').addClass('checked')
                }
            }
            if (input.searchModel.HINHTHUCTT.length > 0) {
                var btnTextHINHTHUCTT = document.querySelector(".btn-text-HINHTHUCTT");
                btnTextHINHTHUCTT.innerText = `${input.searchModel.HINHTHUCTT.length} Selected`;
                for (var i = 0; i < input.searchModel.HINHTHUCTT.length; i++) {
                    $('#HINHTHUCTT_data_' + input.searchModel.HINHTHUCTT[i] + '').addClass('checked')
                }
            }
            if (window.localStorage.getItem(cookieSaleName) != null) {
                var cookie1 = window.localStorage.getItem(cookieSaleName)
                var SaleName = JSON.parse(cookie1)
                $('#CreateName').html('<option selected value = ' + SaleName.id + '> ' + SaleName.nameselect + '</option>')
            }
            if (window.localStorage.getItem(cookieOperatorName) != null) {
                var cookie2 = window.localStorage.getItem(cookieOperatorName)
                var SaleName = JSON.parse(cookie2)
                $('#OperatorId').html('<option selected value = ' + SaleName.id + '> ' + SaleName.nameselect + '</option>')

            }
            if (window.localStorage.getItem(cookieOrderNo) != null) {
                var cookie3 = window.localStorage.getItem(cookieOrderNo)
                var SaleName = JSON.parse(cookie3)
                $('#OrderNo').html('<option selected value = ' + SaleName.id + '> ' + SaleName.nameselect + '</option>')
                $('.reset-order-no').show()

            }
            if (window.localStorage.getItem(cookieClient) != null) {
                var cookie4 = window.localStorage.getItem(cookieClient)
                var SaleName = JSON.parse(cookie4)
                $('#ClientId').html('<option selected value = ' + SaleName.id + '> ' + SaleName.nameselect + '</option>')

            }
            if (window.localStorage.getItem(cookieBoongkingCode) != null) {
                var cookie4 = window.localStorage.getItem(cookieBoongkingCode)
                var SaleName = JSON.parse(cookie4)
                $('#ClientId').html('<option selected value = ' + SaleName.id + '> ' + SaleName.nameselect + '</option>')

            }
        }

        $('#imgLoading').show();
        $.ajax({
            url: "/order/search",
            type: "post",
            data: input,
            success: function (result) {
                $('#imgLoading').hide();
                $('#grid-data').html(result);
                var total = $('#data-total-record').val();
                $('#total-article-filter').text(total);
                $('.checkbox-tb-column').each(function () {
                    let seft = $(this);
                    let id = seft.data('id') + 1;
                    if (seft.is(':checked')) {
                        $('td:nth-child(' + id + '),th:nth-child(' + id + ')').removeClass('mfp-hide');
                    } else {
                        $('td:nth-child(' + id + '),th:nth-child(' + id + ')').addClass('mfp-hide');
                    }
                });
                _ordersCMS.checkCheckBox();
                _ordersCMS.SetActive(tabActive);
                $('#select2-selectPaggingOptions-container').html(input.searchModel.pageSize + " kết quả/trang");
                $('#select2-selectPaggingOptions-container').prop('title', input.searchModel.pageSize + " kết quả/trang");
                $('#selectPaggingOptions option[value=' + input.searchModel.pageSize+']').prop("selected", true);
               
                if (window.location.href.indexOf("Index") != -1) {
                    _ordersCMS.eraseCookie(cookieFilterName)
                }
                _ordersCMS.showHideColumn();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log("Status: " + textStatus);
            }
        });
    },
    OnPaging: function (value) {
        pageSize = 1;
        if (value > 0) {
            PageIndex = value;
            this.SearchData();
        }
    },
    SearchData: function () {
        var searchModel = this.getModel();
        this.Search(searchModel);
        this.changeSetting(15);
        $(document).load().scrollTop(0);

    },
    getModel: function () {
        var CreateName_data = $('#CreateName').select2("val");
        var OperatorId_data = $('#OperatorId').select2("val");
        var OrderNo_data = $('#OrderNo').select2("val");
        var ClientId_data = $('#ClientId').select2("val");
        var BoongKing_Code = $('#BoongKingCode').select2("val");

        var objSearch = this.SearchParam;
        objSearch.searchModel.sysTemType = $('input[name="SysTemType"]:checked').val()
        objSearch.searchModel.HINHTHUCTT = listHINHTHUCTT;
        objSearch.searchModel.OrderNo = $('#OrderNo').find(':selected').val() == undefined || $('#OrderNo').find(':selected').val().trim() == '' ? '' : $('#OrderNo').find(':selected').val().trim(),
            objSearch.searchModel.ClientId = null;
        objSearch.searchModel.BoongKingCode = null;
        var today = new Date();
        var yyyy = today.getFullYear();
        var mm = today.getMonth() + 1; // Months start at 0!
        var dd = today.getDate();
        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        var min_range = '01/01/2020 00:00';
        var max_range = dd + '/' + mm + '/' + (yyyy + 5) + ' 00:00';

        var start_date_from = _global_function.GetDayText($('#fromDate').data('daterangepicker').startDate._d);
        var start_date_to = _global_function.GetDayText($('#fromDate').data('daterangepicker').endDate._d);
        if (start_date_from.trim().toLowerCase() == min_range.trim().toLowerCase() && start_date_to.trim().toLowerCase() == max_range.trim().toLowerCase()) {
            objSearch.searchModel.StartDateFrom = null;
            objSearch.searchModel.StartDateTo = null;
        }
        else {
            objSearch.searchModel.StartDateFrom = _global_function.GetDayText($('#fromDate').data('daterangepicker').startDate._d, true);
            objSearch.searchModel.StartDateTo = _global_function.GetDayText($('#fromDate').data('daterangepicker').endDate._d, true);
        }

        var end_date_from = _global_function.GetDayText($('#toDate').data('daterangepicker').startDate._d);
        var end_date_to = _global_function.GetDayText($('#toDate').data('daterangepicker').endDate._d);
        if (end_date_from.trim().toLowerCase() == min_range.trim().toLowerCase() && end_date_to.trim().toLowerCase() == max_range.trim().toLowerCase()) {
            objSearch.searchModel.EndDateFrom = null;
            objSearch.searchModel.EndDateTo = null;
        }
        else {
            objSearch.searchModel.EndDateFrom = _global_function.GetDayText($('#toDate').data('daterangepicker').startDate._d, true);
            objSearch.searchModel.EndDateTo = _global_function.GetDayText($('#toDate').data('daterangepicker').endDate._d, true);
        }



        objSearch.searchModel.Note = $('#Note').val().trim();
        objSearch.searchModel.UtmSource = listServiceType;
        objSearch.searchModel.ServiceType = listService;
        objSearch.searchModel.Status = listStatus;
        objSearch.searchModel.CreateName = null;
        //objSearch.searchModel.Sale = $('#Sale').val().trim();
        objSearch.searchModel.StatusTab = tabActive;
        objSearch.searchModel.OperatorId = null;
        if (CreateName_data != null) {
            objSearch.searchModel.CreateName = CreateName_data[0]
            cookiename = {
                id: CreateName_data[0],
                nameselect: $('#CreateName').select2('data')[0].text
            }
            _ordersCMS.eraseCookie(cookieSaleName)

            window.localStorage.setItem(cookieSaleName, JSON.stringify(cookiename))
        } else {
            window.localStorage.removeItem(cookieSaleName)
        }
        if (OperatorId_data != null) {
            objSearch.searchModel.OperatorId = OperatorId_data[0]
            cookiename = {
                id: OperatorId_data[0],
                nameselect: $('#OperatorId').select2('data')[0].text
            }
            _ordersCMS.eraseCookie(cookieOperatorName)

            window.localStorage.setItem(cookieOperatorName, JSON.stringify(cookiename))
        }
        else {
            window.localStorage.removeItem(cookieOperatorName)
        }
        if (OrderNo_data != null) {
            objSearch.searchModel.OrderNo = OrderNo_data
            cookiename = {
                id: OrderNo_data,
                nameselect: $('#OrderNo').select2('data')[0].text
            }
            _ordersCMS.eraseCookie(cookieOrderNo)

            window.localStorage.setItem(cookieOrderNo, JSON.stringify(cookiename))
        }
        else {
            window.localStorage.removeItem(cookieOrderNo)

        }
        if (ClientId_data != null) {
            objSearch.searchModel.ClientId = ClientId_data[0]
            cookiename = {
                id: ClientId_data[0],
                nameselect: $('#ClientId').select2('data')[0].text
            }
            _ordersCMS.eraseCookie(cookieClient)

            window.localStorage.setItem(cookieClient, JSON.stringify(cookiename))
        } else {
            window.localStorage.removeItem(cookieClient)

        }
        if (BoongKing_Code != null) {
            objSearch.searchModel.BoongKingCode = BoongKing_Code
            cookiename = {
                id: BoongKing_Code,
                nameselect: $('#BoongKingCode').select2('data')[0].text
            }
            _ordersCMS.eraseCookie(cookieBoongkingCode)

            window.localStorage.setItem(cookieBoongkingCode, JSON.stringify(cookiename))
        }
        else {
            window.localStorage.removeItem(cookieBoongkingCode)

        }
        if (isPicker) {
            objSearch.searchModel.CreateTime = $('#filter_date_daterangepicker').data('daterangepicker').startDate._d.toLocaleDateString("en-GB");
            objSearch.searchModel.ToDateTime = $('#filter_date_daterangepicker').data('daterangepicker').endDate._d.toLocaleDateString("en-GB");

        }
        else {
            objSearch.searchModel.CreateTime = null;
            objSearch.searchModel.ToDateTime = null;

        }

        objSearch.searchModel.PageIndex = PageIndex;
        objSearch.searchModel.pageSize = $("#selectPaggingOptions").find(':selected').val()
        _ordersCMS.eraseCookie(cookieFilterName)



        return objSearch;
    },
    setValueFilter: function () {
        var objSearch = this.SearchParam;
        $('input[name="SysTemType"]:checked').val(objSearch.searchModel.sysTemType)
        $('#fromDate').val(objSearch.searchModel.startDate);
        $('#OrderNo').val(objSearch.searchModel.OrderNo);
        $('#ClientId').val(objSearch.searchModel.ClientId);
        $('#toDate').val(objSearch.searchModel.endDate);
        $('#Note').val(objSearch.searchModel.Note);
        listServiceType = objSearch.searchModel.UtmSource;
        listService = objSearch.searchModel.ServiceType;
        listStatus = objSearch.searchModel.Status;
        listHINHTHUCTT = objSearch.searchModel.HINHTHUCTT;
        $('#Status').val(objSearch.searchModel.Status ?? -1).change();
        $('#CreateName').val(objSearch.searchModel.CreateName);
        $('#Sale').val(objSearch.searchModel.Sale);
        $("#selectPaggingOptions").find(':selected').val(objSearch.pageSize)
    },
    saveCookieFilter: function () {
        var modelorder = this.getModel();
        this.setCookie(cookieFilterName, JSON.stringify(this.getModel()), 1)
    },

    checkCheckBox: function () {
        var len = $(".grid-slide input[type='checkbox']:checked").length;

        if (len > 9) {
            $('.table-responsive').removeClass('table-gray');
            $('.table-responsive').addClass('table-scroll');
        } else {
            $('.table-responsive').removeClass('table-scroll');
            $('.table-responsive').addClass('table-gray');
        }
    },
    changeSetting: function (position) {
        this.showHideColumn();
        this.checkCheckBox();
        switch (position) {
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:
                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
            case 10:
                if ($('#CreatedNameCheck').is(":checked")) {
                    orderFields.CreatedName = true
                } else {
                    orderFields.CreatedName = false
                }
                this.eraseCookie(cookieName);
                this.setCookie(cookieName, JSON.stringify(orderFields), 10);
                break;
            case 11:
                if ($('#UpdatedDateCheck').is(":checked")) {
                    orderFields.UpdatedDate = true
                } else {
                    orderFields.UpdatedDate = false
                }
                this.eraseCookie(cookieName);
                this.setCookie(cookieName, JSON.stringify(orderFields), 10);
                break;
            case 12:
                if ($('#UpdatedNameCheck').is(":checked")) {
                    orderFields.UpdatedName = true
                } else {
                    orderFields.UpdatedName = false
                }
                this.eraseCookie(cookieName);
                this.setCookie(cookieName, JSON.stringify(orderFields), 10);
                break;
            case 13:
                if ($('#MainEmployee').is(":checked")) {
                    orderFields.MainEmp = true
                } else {
                    orderFields.MainEmp = false
                }
                this.eraseCookie(cookieName);
                this.setCookie(cookieName, JSON.stringify(orderFields), 10);
                break;
            case 15:
                if ($('#SubEmployee').is(":checked")) {
                    orderFields.SubEmp = true
                } else {
                    orderFields.SubEmp = false
                }
            case 16:
                if ($('#Voucher').is(":checked")) {
                    orderFields.Voucher = true
                } else {
                    orderFields.Voucher = false
                }
                this.eraseCookie(cookieName);
                this.setCookie(cookieName, JSON.stringify(orderFields), 10);
                break;
            case 17:
                if ($('#Operator').is(":checked")) {
                    orderFields.Operator = true
                } else {
                    orderFields.Operator = false
                }
                this.eraseCookie(cookieName);
                this.setCookie(cookieName, JSON.stringify(orderFields), 10);
                break;

            case 22:
                if ($('#HINHTHUCTTb').is(":checked")) {
                    orderFields.HINHTHUCTTb = true
                } else {
                    orderFields.HINHTHUCTTb = false
                }
                this.eraseCookie(cookieName);
                this.setCookie(cookieName, JSON.stringify(orderFields), 10);
                break;
            case 23:
                if ($('#KHACHPT').is(":checked")) {
                    orderFields.KHACHPT = true
                } else {
                    orderFields.KHACHPT = false
                }
                this.eraseCookie(cookieName);
                this.setCookie(cookieName, JSON.stringify(orderFields), 10);
                break;
            case 24:
                if ($('#YcHoaDon').is(":checked")) {
                    orderFields.YcHoaDon = true
                } else {
                    orderFields.YcHoaDon = false
                }
                this.eraseCookie(cookieName);
                this.setCookie(cookieName, JSON.stringify(orderFields), 10);
                break;
            case 25:
                if ($('#tum_medium').is(":checked")) {
                    orderFields.MaHoaDon = true
                } else {
                    orderFields.MaHoaDon = false
                }
                this.eraseCookie(cookieName);
                this.setCookie(cookieName, JSON.stringify(orderFields), 10);
                break;
            default:
        }
    },
    showHideColumn: function () {
        $('#totalProfit').removeClass('hide-element');
        $('#sttChecklabel').removeClass('mfp-hide');
        orderFields.ProfitOrder == true;
        orderFields.SttOrder == true;
        $('#profitCheck').prop('checked', true);
        $('#sttCheck').prop('checked', true);
        $('#dateCheck').prop('checked', true);
        $('#clientCheck').prop('checked', true);
        $('#noteCheck').prop('checked', true);
        $('#payCheck').prop('checked', true);
        $('.action-btn').addClass('mfp-hide');


        if (tabActive == 1) {
            orderFields.ProfitOrder == false;
            orderFields.SttOrder == false;
            $('#profitCheck').prop('checked', false);
            $('#sttCheck').prop('checked', false);
            $('#sttChecklabel').addClass('mfp-hide');
            $('#totalProfit').addClass('hide-element');

        }
        if (tabActive == 4) {
            $('#profitCheck').prop('checked', false);
            $('#sttCheck').prop('checked', false);
            $('#dateCheck').prop('checked', false);
            $('#clientCheck').prop('checked', false);
            $('#noteCheck').prop('checked', false);
            $('#noteCheck').prop('checked', false);
            $('#payCheck').prop('checked', false);
            $('.action-btn').removeClass('mfp-hide');
            $('.href-btn').removeAttr("href");
            $('.blue').removeClass('blue');

        }
        if (tabActive == 0) {
            $('#MainEmployee').prop('checked', false);
            $('#SubEmployee').prop('checked', false);
            $('#Voucher').prop('checked', false);
            $('.action-tc').removeClass('mfp-hide');

        }
        $('.checkbox-tb-column').each(function () {
            let seft = $(this);
            let id = seft.data('id') + 1;
            if (seft.is(':checked')) {
                $('td:nth-child(' + id + '),th:nth-child(' + id + ')').removeClass('mfp-hide');
            } else {
                $('td:nth-child(' + id + '),th:nth-child(' + id + ')').addClass('mfp-hide');
            }
            if (tabActive == 4) {
                $('td:nth-child(15),th:nth-child(15)').addClass('mfp-hide');
            }

        });
    },
    checkShowHide: function () {
        if (orderFields.utmSourde === true) {
            $('#utmSourceCheck').prop('checked', true);
        } else {
            $('#utmSourceCheck').prop('checked', false);
        }

        if (orderFields.CreatedName === true) {
            $('#CreatedNameCheck').prop('checked', true);
        } else {
            $('#CreatedNameCheck').prop('checked', false);
        }
        if (orderFields.UpdatedDate === true) {
            $('#UpdatedDateCheck').prop('checked', true);
        } else {
            $('#UpdatedDateCheck').prop('checked', false);
        }
        if (orderFields.UpdatedName === true) {
            $('#UpdatedNameCheck').prop('checked', true);
        } else {
            $('#UpdatedNameCheck').prop('checked', false);
        }
        if (orderFields.MainEmp === true) {
            $('#MainEmployee').prop('checked', true);
        } else {
            $('#MainEmployee').prop('checked', false);
        }
        if (orderFields.SubEmp === true) {
            $('#SubEmployee').prop('checked', true);
        } else {
            $('#SubEmployee').prop('checked', false);
        }
        if (orderFields.Voucher === true) {
            $('#Voucher').prop('checked', true);
        } else {
            $('#Voucher').prop('checked', false);
        }
        if (orderFields.Operator === true) {
            $('#Operator').prop('checked', true);
        } else {
            $('#Operator').prop('checked', false);
        }
        if (orderFields.HINHTHUCTT === true) {
            $('#HINHTHUCTTb').prop('checked', true);
        } else {
            $('#HINHTHUCTTb').prop('checked', false);
        }

        if (orderFields.KHACHPT === true) {
            $('#KHACHPT').prop('checked', true);
        } else {
            $('#KHACHPT').prop('checked', false);
        }
        if (orderFields.tum_medium === true) {
            $('#tum_medium').prop('checked', true);
        } else {
            $('#tum_medium').prop('checked', false);
        }
        if (orderFields.YcHoaDon === true) {
            $('#YcHoaDon').prop('checked', true);
        } else {
            $('#YcHoaDon').prop('checked', false);
        }


    },
    eraseCookie: function (name) {
        document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    },
    getCookie: function (name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    },
    setCookie: function (name, value, days) {
        var expires = "";
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + (value || "") + expires + "; path=/";
    },
    OnChangeUserId: function (value, type) {
        if (type == 1) {
            $('#OrderNo').val(value.name)
        }
        else {
            $('#OrderNo').val('')

        }
    },
    OnChangeUserCreate: function (value, type) {
        if (type == 1) {
            $('#CreateName').val(value.name)
        }
        else {
            $('#CreateName').val('')

        }
    },
    onSelectPageSize: function () {
        this.SearchData();
    },
    OnSearchStatus: function (status) {
        tabActive = status;

        PageIndex = 1;
        //pageSize = 1;
        this.SearchData();

    },
    SetActive: function (status) {
        $('#countSttAll').removeClass('active')
        $('#countSttCheck').removeClass('active')
        $('#countSttNotDone').removeClass('active')
        $('#countSttDone').removeClass('active')
        $('#countSttErr').removeClass('active')
        $('#countSttErr').removeClass('active')
        $('#countSttNews').removeClass('active')
        if (status == 99)
            $('#countSttAll').addClass('active')
        if (status == 1)
            $('#countSttCheck').addClass('active')
        if (status == 2)
            $('#countSttNotDone').addClass('active')
        if (status == 3)
            $('#countSttDone').addClass('active')
        if (status == 4)
            $('#countSttErr').addClass('active')
        if (status == 0)
            $('#countSttNews').addClass('active')
    },

    ReCreateOrder: function (id) {
        _global_function.AddLoading()
        $.ajax({
            url: "/order/ReCreateOrder",
            type: "post",
            data: { orderId: id },
            success: function (data) {
                if (data.status == 0) {
                    _global_function.RemoveLoading()
                    _msgalert.success(data.message);
                } else {
                    _global_function.RemoveLoading()
                    _msgalert.error(data.message);
                }
            }
        });
    },
    Edit: function (id) {
        var order_Id = $('#order_Id').val()
        let title = 'Thêm mới/Cập nhật thành viên chính';
        let url = '/Order/ContactClientdetails';
        let param = {
            orderId: order_Id,
        };
        if (id.trim() != "") {
            param = {
                id: id,
            };
        }
        _magnific.OpenSmallPopup(title, url, param);
    },
    AddInvoiceRequest: function () {
        var order_Id = $('#order_Id').val()
        let title = 'Thêm yêu cầu xuất hóa đơn';
        let url = '/InvoiceRequest/Add';
        let param = {
            orderId: order_Id,
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    SetupContactClient: function () {
        let FromCreate = $('#ContactClient_Detail');
        FromCreate.validate({
            rules: {

                "Name": "required",
                "Mobile": {
                    required: true,
                    number: true,
                },
                "Email": {
                    required: true,
                    email: true,
                },


            },
            messages: {
                "Name": "Tên khách hàng không được bỏ trống",

                "Mobile": {
                    required: "Số điện thoại không được bỏ trống",
                    number: "Nhập đúng định dạng số",
                },
                "Email": {
                    required: "Email không được bỏ trống",
                    email: "Nhạp đúng định dạnh email",
                },
            }
        });
        if (FromCreate.valid()) {

            var model = {
                id: $('#Id').val(),
                client_id: $('#ClientId').val(),
                order_id: $('#OrderId').val(),
                name: $('#Name').val(),
                phone: $('#Mobile').val(),
                email: $('#Email').val(),
            }
            $.ajax({
                url: '/Order/UpdateContactClient',
                type: "post",
                data: { model },
                success: function (result) {
                    if (result.status == 0) {
                        _msgalert.success(result.msg);
                        $.magnificPopup.close();
                        location.reload();
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                }
            });
        }

    },
    OpenPopupAddSale: function (id) {
        let title = 'Chọn nhân viên phụ trách';
        let url = '/Order/PopupAddSale';
        let param = {
            id: id
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    AddSale: function () {
        let FromCreate = $('#Add-Sale');
        FromCreate.validate({
            rules: {

                "UserId_new": "required",
            },
            messages: {
                "UserId_new": "Nhân viên phụ trách không được bỏ trống",
            }
        });
        if (FromCreate.valid()) {
            var UserId_new = $('#UserId_new').select2("val");
            var UserId = UserId_new[0];

            var order_id = $('#order_id').val();
            $.ajax({
                url: '/Order/ChangeOrderSaler',
                type: "post",
                data: { order_id: order_id, saleid: UserId },
                success: function (result) {
                    if (result.status == 0) {
                        _msgalert.success(result.msg);
                        $.magnificPopup.close();
                        _ordersCMS.OnSearchStatus(0)
                        _ordersCMS.SetActive(0);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                }
            });
        }

    },
    Export: function () {

        $('#btnExport').prop('disabled', true);
        $('#icon-export').removeClass('fa-file-excel-o');
        $('#icon-export').addClass('fa-spinner fa-pulse');
        var objSearch = this.getModel();
        objSearch.searchModel.PageIndex = 1;
        objSearch.searchModel.pageSize = $('#countOrder').val();
        this.searchModel = objSearch
        _global_function.AddLoading()
        $.ajax({
            url: "/Order/ExportExcel",
            type: "Post",
            data: { searchModel: objSearch.searchModel, field: orderFields },
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
};

$('#OrderNo, #Note, #CreateName').keypress(function (event) {
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        _ordersCMS.SearchData();
    }
});
var _order_manual = {
    CreateOrderManual: function () {
        if ($('#create_order_manual').length) {
            $('#create_order_manual').removeClass('show')
            setTimeout(function () {
                $('#create_order_manual').remove();
            }, 300);

        }
        $.ajax({
            url: "/OrderManual/CreateOrderManual",
            type: "post",
            data: {},
            success: function (result) {
                $('body').append(result);
                setTimeout(function () {
                    $('#create_order_manual').addClass('show')
                }, 300);

            }
        });
    },
    Close: function () {
        $('#create_order_manual').removeClass('show')
        setTimeout(function () {
            $('#create_order_manual').remove();
        }, 300);
    },

}