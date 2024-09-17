var _order_detail_common = {
    FileAttachment: function (data_id, type) {
        _global_function.RenderFileAttachment($('.attachment-file-block'), data_id, type)

        /*
        $.ajax({
            url: "/AttachFile/Widget",
            type: "Post",
            data: { data_id: data_id, type: type, element_name: _order_detail_html.element_name },
            success: function (result) {
                $('.attachment-file-block').html(result);
            }
        });
        */
    },
    UserSuggesstion: function (element,service_type=0) {
        var selected = element.find(':selected').val()
        element.select2();
        debugger
        $.ajax({
            url: "UserSuggestion",
            type: "post",
            data: { txt_search: "", service_type:service_type },
            success: function (result) {
                if (result != undefined && result.data != undefined && result.data.length > 0) {
                    result.data.forEach(function (item) {
                        element.append(_order_detail_html.html_user_option.replaceAll('{user_id}', item.id).replace('{user_email}', item.fullname).replace('{user_name}', item.username).replace('{user_phone}', item.phone == undefined ? "" : ' - ' + item.phone))

                    });
                    element.val(selected).trigger('change');
                }
                else {
                    element.trigger('change');

                }

            }
        });
    },
    ExistsTourSuggesstion: function (element) {
        var selected = element.find(':selected').val()
        element.select2({
            ajax: {
                url: "ExistsTourSuggesstion",
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
                                text: item.tourName + (item.organizingTypeName != undefined || item.tourTypeName == undefined ? ' (' : '') + (item.organizingTypeName == undefined ? '' : item.organizingTypeName) + (item.tourTypeName == undefined ? '' : (item.organizingTypeName != undefined ? ' - ' : '') + item.tourTypeName) + (item.organizingTypeName != undefined || item.tourTypeName == undefined ? ')' : ''),
                                id: item.id,
                            }
                        })
                    };
                },
                cache: true
            }
        });
        element.val(selected).trigger('change');
    },
    Select2FixedOptionWithAddNew: function (element) {
        element.select2({
            tags: true,

            createTag: function (params) {
                var term = $.trim(params.term);

                if (term === '') {
                    return null;
                }

                return {
                    id: term,
                    text: term,
                }
            }
        });
    },
    Select2WithFixedOptionAndSearch: function (element) {
        element.select2({
           
        });
    },
    Select2WithFixedOptionAndNoSearch: function (element) {
        element.select2({
            minimumResultsForSearch: Infinity
        });
    },
    SingleDateTimePicker: function (element) {

        var today = new Date();
        var yyyy = today.getFullYear();
        var yyyy_max = yyyy + 10;
        var current_date = element.val();
        var min_range = '01/01/2000 00:00';
        var max_range = '31/12/' + yyyy_max + ' 23:59';
        var mm = today.getMonth() + 1; // Months start at 0!
        var dd = today.getDate();
        var hh = today.getHours();
        var minutes = today.getMinutes()
        if (current_date == undefined || current_date == null || current_date.trim() == '') {
            current_date = dd + '/' + mm + '/' + yyyy + ' ' + hh + ':' + minutes
        }

        element.daterangepicker({
            singleDatePicker: true,
            autoApply: true,
            showDropdowns: true,
            drops: 'down',
            minDate: min_range,
            maxDate: max_range,
            timePicker: true,
            timePicker24Hour: true,
            locale: {
                format: 'DD/MM/YYYY HH:mm'
            }
        }, function (start, end, label) {


        });
        element.data('daterangepicker').setStartDate(current_date);
    },
    SingleDatePicker: function (element, dropdown_position = 'down') {
        var current_date = element.val();
        var today = new Date();
        var yyyy = today.getFullYear();
        var yyyy_max = yyyy + 10;

        var min_range = '01/01/2000';
        var max_range = '31/12/' + yyyy_max;

        element.daterangepicker({
            singleDatePicker: true,
            autoApply: true,
            showDropdowns: true,
            drops: dropdown_position,
            minDate: min_range,
            maxDate: max_range,
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {


        });
        element.data('daterangepicker').setStartDate(current_date);

    },
    SingleDatePickerWithMinDateIsElementvalue: function (element, dropdown_position = 'down') {
        var current_date = element.val();
        var today = new Date();
        var yyyy = today.getFullYear();
        var yyyy_max = yyyy + 10;

        var min_range = element.val();
        var max_range = '31/12/' + yyyy_max;

        element.daterangepicker({
            singleDatePicker: true,
            autoApply: true,
            showDropdowns: true,
            drops: dropdown_position,
            minDate: min_range,
            maxDate: max_range,
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {


        });
        element.data('daterangepicker').setStartDate(current_date);

    },
    SingleDateTimePickerWithMinDateIsElementvalue: function (element, dropdown_position = 'down') {
        var current_date = element.val();
        var today = new Date();
        var yyyy = today.getFullYear();
        var yyyy_max = yyyy + 10;

        var min_range = element.val();
        var max_range = '31/12/' + yyyy_max;

        element.daterangepicker({
            singleDatePicker: true,
            autoApply: true,
            showDropdowns: true,
            drops: dropdown_position,
            minDate: min_range,
            maxDate: max_range,
            timePicker: true,
            timePicker24Hour: true,
            locale: {
                format: 'DD/MM/YYYY HH:mm'
            }
        }, function (start, end, label) {


        });
        element.data('daterangepicker').setStartDate(current_date);

    },
    SingleDateTimePickerFromNow: function (element, dropdown_position = 'down') {

        var today = new Date();
        var yyyy = today.getFullYear();
        var mm = today.getMonth() + 1; // Months start at 0!
        var dd = today.getDate();
        var yyyy_max = yyyy + 10;
        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        var time_now = dd + '/' + mm + '/' + yyyy + ' ' + ("0" + today.getHours()).slice(-2) + ':' + ("0" + today.getMinutes()).slice(-2);
        var max_range = '31/12/' + yyyy_max+' 23:59';

        element.daterangepicker({
            singleDatePicker: true,
            autoApply: true,
            showDropdowns: true,
            drops: dropdown_position,
            minDate: time_now,
            maxDate: max_range,
            timePicker: true,
            timePicker24Hour: true,
            locale: {
                format: 'DD/MM/YYYY HH:mm'
            }
        }, function (start, end, label) {


        });
    },
    SingleDatePickerFromNow: function (element, dropdown_position = 'down') {

        var today = new Date();
        var yyyy = today.getFullYear();
        var mm = today.getMonth() + 1; // Months start at 0!
        var dd = today.getDate();
        var yyyy_max = yyyy + 10;
        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        var time_now = dd + '/' + mm + '/' + yyyy;
        var max_range = '31/12/' + yyyy_max;

        element.daterangepicker({
            singleDatePicker: true,
            autoApply: true,

            showDropdowns: true,
            drops: dropdown_position,
            minDate: time_now,
            maxDate: max_range,
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {


        });
    },
    SingleDateTimePickerFromNowNoUpdateInput: function (element, dropdown_position = 'down') {

        var today = new Date();
        var yyyy = today.getFullYear();
        var mm = today.getMonth() + 1; // Months start at 0!
        var dd = today.getDate();
        var yyyy_max = yyyy + 10;
        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        var time_now = dd + '/' + mm + '/' + yyyy;
        var max_range = '31/12/' + yyyy_max;
        var element_value = element.val()
        var element_date = _global_function.GetDateTimeFromVNDateTimeSlash(element_value)
        element.daterangepicker({
            singleDatePicker: true,
            timePicker: true,
            timePicker24Hour:true,
            autoApply: true,
            showDropdowns: true,
            drops: dropdown_position,
            minDate: time_now,
            maxDate: max_range,
            locale: {
                format: 'DD/MM/YYYY HH:mm'
            }
        }, function (start, end, label) {


        });
        if (element_date > today) {
            element.data('daterangepicker').setStartDate(element_value);
        }
    },
    SingleDatePickerFromNowNoUpdateInput: function (element, dropdown_position = 'down') {

        var today = new Date();
        var yyyy = today.getFullYear();
        var mm = today.getMonth() + 1; // Months start at 0!
        var dd = today.getDate();
        var yyyy_max = yyyy + 10;
        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        var time_now = dd + '/' + mm + '/' + yyyy;
        var max_range = '31/12/' + yyyy_max;
        var element_value = element.val()
        var element_date = undefined
        if (element_value != undefined && element_value.trim()!='') {
            element_date = _global_function.GetDateFromVNDateTimeSlash(element_value)
        }
        element.daterangepicker({
            singleDatePicker: true,
            autoApply: true,
            showDropdowns: true,
            drops: dropdown_position,
            minDate: time_now,
            maxDate: max_range,
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {


        });
        if (element_date != undefined && element_date > today) {
            element.data('daterangepicker').setStartDate(element_value);
        }
    },
    SingleDateRangePickerFromNowNoUpdateInput: function (element, dropdown_position = 'down') {

        var today = new Date();
        var yyyy = today.getFullYear();
        var mm = today.getMonth() + 1; // Months start at 0!
        var dd = today.getDate();
        var yyyy_max = yyyy + 10;
        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        var time_now = dd + '/' + mm + '/' + yyyy;
        var max_range = '31/12/' + yyyy_max;
        var element_date = _global_function.GetDateFromVNDateTimeSlash(element.val())
        element.daterangepicker({
            autoApply: true,
            showDropdowns: true,
            drops: dropdown_position,
            minDate: time_now,
            maxDate: max_range,
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {


        });
        if (element_date > today) {
            element.data('daterangepicker').setStartDate(element.val());
        }
    },
    SingleDatePickerBirthDay: function (element, dropdown_position='down') {

        var current_date = element.val();

        var today = new Date();
        var yyyy = today.getFullYear();
        var mm = today.getMonth() + 1; // Months start at 0!
        var dd = today.getDate();
        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        var min_range = '01/01/1890';
        var max_range = dd + '/' + mm + '/' + yyyy;

        element.daterangepicker({
            singleDatePicker: true,
            autoApply: true,
            showDropdowns: true,
            drops: dropdown_position,
            minDate: min_range,
            maxDate: max_range,
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {


        });

    },
    OnApplyStartDateOfBookingRange: function (start_date_element,end_date_element) {

      //  end_date_element.data('daterangepicker').minDate = start_date_element.data('daterangepicker').startDate._d
        var today = new Date();
        var yyyy = today.getFullYear();
        var yyyy_max = yyyy + 10;
        var max_range = '31/12/' + yyyy_max;

        var element_date = _global_function.GetDateFromVNDateTimeSlash(start_date_element.val())
        var min_time = _global_function.GetDayTextDateRangePicker(element_date)
        end_date_element.daterangepicker({
            autoApply: true,
            singleDatePicker:true,
            showDropdowns: true,
            drops: 'down',
            minDate: min_time,
            maxDate: max_range,
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {


        });
        if (element_date > today) {
            end_date_element.data('daterangepicker').setStartDate(end_date_element.val());
        }
    },
    OnApplyStartDateOfBookingRangeDatetime: function (start_date_element, end_date_element) {

        //end_date_element.data('daterangepicker').minDate = start_date_element.data('daterangepicker').startDate._d
        
        var today = new Date();
        var yyyy = today.getFullYear();
        var yyyy_max = yyyy + 10;
        var max_range = '31/12/' + yyyy_max+' 23:59';
        //var element_date = _global_function.GetDateTimeFromVNDateTimeSlash(start_date_element.val())
        var min_time = _global_function.GetDayText(start_date_element.data('daterangepicker').startDate._d)
        end_date_element.daterangepicker({
            singleDatePicker: true,
            showDropdowns: true,
            drops: 'down',
            autoApply: true,
            minDate: min_time,
            maxDate: max_range,
            timePicker: true,
            timePicker24Hour: true,
            locale: {
                format: 'DD/MM/YYYY HH:mm'
            }
        }, function (start, end, label) {


        });
    },
    OnApplyPackageDateDateRange: function (apply_element,start_date_element, end_date_element) {

        //end_date_element.data('daterangepicker').minDate = start_date_element.data('daterangepicker').startDate._d

        
        var min_date = _global_function.GetDateFromVNDateTimeSlash(start_date_element.val())
        var min_time = _global_function.GetDayText(min_date)
        var max_date = _global_function.GetDateFromVNDateTimeSlash(end_date_element.val())
        var max_time = _global_function.GetDayText(max_date)
        apply_element.daterangepicker({
            showDropdowns: true,
            drops: 'down',
            autoApply: true,
            minDate: min_time,
            maxDate: max_time,
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {


        });
    },
    GetDateFromNoUpdateDateRangeElement: function (element) {
        var date = _global_function.GetDateFromVNDateTimeSlash(element.val())
        var date_from_picker = element.data('daterangepicker').startDate._d
        if (date_from_picker > date)
            return date
        else
            return date_from_picker
    },
    GetDateFromNoUpdateDateRangeElementDateRange: function (element, is_end_date = false) {
        var value = element.val()
        var from = _global_function.GetDateFromVNDateTimeSlashDateRange(value, false)
        var to = _global_function.GetDateFromVNDateTimeSlashDateRange(value, true)
        var date_from_picker = element.data('daterangepicker').startDate._d.setHours(0,0,0,0)
        var date_to_picker = element.data('daterangepicker').endDate._d.setHours(0, 0, 0, 0)
        
        if (date_from_picker <= date_to_picker)
        {
            return is_end_date ? to : from
        }
        else
        {
            return is_end_date ? date_to_picker : date_from_picker

        }
    }
}

