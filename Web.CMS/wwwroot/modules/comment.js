

var obj =
{
    "clientID": null,
    "createDateFrom": null,
    "createDateTo": null,
    "pageIndex" : 1,
    "pageSize" : 10
}

var _comment =
{
    LoadComment: function () {
        let url = '/Comment/ListComment';
        _ajax_caller.post(url, { request: obj }, function (result) {
            $('#Comment-content').html(``);
            $('#Comment-content').append(result);
            window.scrollTo(0, 0);
        });
    },
    getDateTimeRanges: function (type) {
        const now = moment();
        const startOfDay = now.clone().startOf('day');
        const endOfDay = now.clone().endOf('day');

        switch (type) {
            case 'today':
                return { start: startOfDay.format('YYYY-MM-DD'), end: endOfDay.format('YYYY-MM-DD') };
            case 'yesterday':
                return { start: startOfDay.clone().subtract(1, 'day').format('YYYY-MM-DD'), end: endOfDay.clone().subtract(1, 'day').format('YYYY-MM-DD') };
            case 'thisWeek':
                return { start: startOfDay.clone().startOf('week').format('YYYY-MM-DD'), end: startOfDay.clone().endOf('week').format('YYYY-MM-DD') };
            case 'lastWeek':
                return { start: startOfDay.clone().subtract(1, 'week').startOf('week').format('YYYY-MM-DD'), end: startOfDay.clone().subtract(1, 'week').endOf('week').format('YYYY-MM-DD') };
            case 'thisMonth':
                return { start: startOfDay.clone().startOf('month').format('YYYY-MM-DD'), end: startOfDay.clone().endOf('month').format('YYYY-MM-DD') };
            case 'lastMonth':
                return { start: startOfDay.clone().subtract(1, 'month').startOf('month').format('YYYY-MM-DD'), end: startOfDay.clone().subtract(1, 'month').endOf('month').format('YYYY-MM-DD') };
            case 'thisQuarter':
                return { start: startOfDay.clone().startOf('quarter').format('YYYY-MM-DD'), end: startOfDay.clone().endOf('quarter').format('YYYY-MM-DD') };
            case 'lastQuarter':
                return { start: startOfDay.clone().subtract(1, 'quarter').startOf('quarter').format('YYYY-MM-DD'), end: startOfDay.clone().subtract(1, 'quarter').endOf('quarter').format('YYYY-MM-DD') };
            default:
                return { start: moment('2024-01-01', 'YYYY-MM-DD').format('YYYY-MM-DD'), end: endOfDay.format('YYYY-MM-DD') };
        }
    },

    OnChangePageSize: function ()
    {
        obj.pageSize = $("#selectPaggingOptions").find(':selected').val();
        sessionStorage.setItem("SelectedPageSize", obj.pageSize);
        obj.pageIndex = 1;
        _comment.LoadComment();
    },

    OnChangeClient: function ()
    {
        obj.clientID = $("#clientInput").find(':selected').val();
        obj.pageIndex = 1;
        _comment.LoadComment();
    },

    OnChanggeDate: function ()
    {
        var DateSelected = this.getDateTimeRanges($("#DateInput").find(':selected').val());
        obj.createDateFrom = DateSelected.start;
        obj.createDateTo = DateSelected.end;
        obj.pageIndex = 1;
        _comment.LoadComment();
    },

    OnPanging: function (value)
    {
        obj.pageIndex = value;
        _comment.LoadComment();
    },
}

$(document).ready(function () {
    $("#clientInput").select2({
        //theme: 'bootstrap4',
        placeholder: "Tìm kiếm theo tên, số điện thoại, email khách hàng",
        ajax: {
            url: "/Comment/Client",
            type: "post",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                var query = {
                    phoneOrName: params.term,
                }
                return query;
            },
            processResults: function (response) {
                $("#clientInput").empty();
                return {
                    results: $.map(response.data, function (item) {
                        return {
                            text: item.clientname + " - " + item.phone + " - " + item.email,
                            id: item.id,
                        }
                    })
                };
            },
        }
    }).on('select2:opening', function (e) {
        //Bỏ chọn option sẽ load lại với các giá trị mặc định
        obj.pageSize = $("#selectPaggingOptions").find(':selected').val();
        obj.clientID = null;
        var DateSelected = _comment.getDateTimeRanges($("#DateInput").find(':selected').val());
        obj.createDateFrom = DateSelected.start;
        obj.createDateTo = DateSelected.end;
        obj.pageIndex = 1;
        _comment.LoadComment();
    });


    var InputClientElement = $('.select2-selection__arrow')
    InputClientElement[0].classList.add('ClientInput_Arrow'); 
    _comment.LoadComment();
});
