$(document).ready(function () {

    $('body').on('click', '.onclick', function () {
        if (!$(this).hasClass("onclick-active")) {
            $(this).addClass("onclick-active");
            $(this).removeClass("onclick");
            $(this).next('.form-down').slideDown();
            $(this).next('.form-down-filter').slideDown();
            $(this).next('.list-noti').slideDown();
            $('.form-down input').focus();


        }
    });
    $('body').on('click', '.onclick-active', function () {
        if (!$(this).hasClass("onclick")) {
            $(this).removeClass("onclick-active");
            $(this).addClass("onclick");
            $(this).next('.form-down').slideUp();
            $(this).next('.list-noti').slideUp();
            $(this).next('.form-down-filter').slideUp();
        }

    });
    $(document).click(function (event) {
        if ($(event.target).closest('.wrap-notifi').length == 0) {
            $(".list-noti").slideUp();
            $(".wrap-notifi .notifi").removeClass('active');
        }
    });
    $('body').on('click', '.onclick-filter', function () {
        if (!$(this).hasClass("onclick-active-filter")) {
            $(this).addClass("onclick-active-filter");
            $(this).removeClass("onclick-filter");
            $(this).next('.form-down-filter').slideDown();
        }
    });
    $('body').on('click', '.onclick-active-filter', function () {
        if (!$(this).hasClass("onclick-filter")) {
            $(this).removeClass("onclick-active-filter");
            $(this).addClass("onclick-filter");
            $(this).next('.form-down-filter').slideUp();
        }

    });
});
var _global_function = {
    AddLoading: function () {
        $("#loading").remove()
        $('body').append(`<div class="loading" id="loading"><img id="loading-image" src="/images/icons/loading.gif" alt="Loading..." style="margin-left: 10%;" /></div>`)
    },
    RemoveLoading: function () {
        $("#loading").remove()
    },
    Comma: function (number) { //function to add commas to textboxes
        number = ('' + number).replace(/[^0-9.,]+/g, '');
        number += '';
        number = number.replaceAll(',', '');
        x = number.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1))
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        return x1 + x2;
    },
    DateTimeDotNetToString: function (date_string, has_time = false) {
        //"2024-08-28T09:15:09.43Z"
        var date = new Date(date_string)
        var text = ("0" + date.getDate()).slice(-2) + '/' + ("0" + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();
        if (has_time == true) {
            var time_text = + ' ' + (date.getHours()) + ':' + (("0" + date.getMinutes()).slice(-2))
            return text + ' ' + time_text
        }
        return text
    },
    ParseJSDate: function (text) {
        var parse_value = text.split(' ')[0].split('/')
        if (parse_value != undefined && parse_value.length > 2) {
            return new Date(parse_value[2] + '-' + parse_value[1] + '-' + parse_value[0] );
        }
        return undefined;
    },
    ParseDateTostring: function (text) {
        var parse_value = text.split(' ')[0].split('/')
        if (parse_value != undefined && parse_value.length > 2) {
            return new String(parse_value[2] + '-' + parse_value[1] + '-' + parse_value[0]);
        }
        return undefined;
    },
    GetDayText: function (date, donetdate = false) {
        var text = ("0" + date.getDate()).slice(-2) + '/' + ("0" + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear() + ' ' + ("0" + date.getHours()).slice(-2) + ':' + ("0" + date.getMinutes()).slice(-2);
        if (donetdate) {
            text = ("0" + (date.getMonth() + 1)).slice(-2) + '/' + ("0" + date.getDate()).slice(-2) + '/' + date.getFullYear() + ' ' + ("0" + date.getHours()).slice(-2) + ':' + ("0" + date.getMinutes()).slice(-2);
        }
        return text;
    },
    GetDayTextDateRangePicker: function (date) {
        var text = ("0" + date.getDate()).slice(-2) + '/' + ("0" + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();

        return text;
    },
    delay_callback: function (callback, ms) {
        var timer = 0;
        return function () {
            var context = this, args = arguments;
            clearTimeout(timer);
            timer = setTimeout(function () {
                callback.apply(context, args);
            }, ms || 0);
        };
    },
    GetDateTimeFromVNDateTimeSlash(date) {
        var date_timesplit = date.split(' ');
        if (date_timesplit.length < 2) return undefined;

        var time_split = date_timesplit[1].split(':');
        var date_split = date_timesplit[0].split('/');
        return new Date(parseInt(date_split[2]), parseInt(date_split[1]), parseInt(date_split[0]) - 1, parseInt(time_split[0]), parseInt(time_split[1]), 0)
    },
    GetDateFromVNDateTimeSlash(date) {
        var time_list = date.split('/');
        if (time_list.length < 3) return undefined;
        return new Date(parseInt(time_list[2]), parseInt(time_list[1]) - 1, parseInt(time_list[0]), 0, 0, 0)
    },
    GetDateFromVNDateTimeSlashDateRange(date, is_end_date = false) {
        var time_list = date.split('-');
        if (time_list.length < 2) return undefined;
        if (!is_end_date) {
            return _global_function.GetDateFromVNDateTimeSlash(time_list[0])
        }
        else {
            return _global_function.GetDateFromVNDateTimeSlash(time_list[1])

        }
    },
    RemoveUnicode: function (str) {
        if (str == null || str == undefined || str.trim() == '') {
            return str
        }
        // remove accents
        var from = "àáãảạăằắẳẵặâầấẩẫậèéẻẽẹêềếểễệđùúủũụưừứửữựòóỏõọôồốổỗộơờớởỡợìíỉĩịäëïîöüûñçýỳỹỵỷ",
            to = "aaaaaaaaaaaaaaaaaeeeeeeeeeeeduuuuuuuuuuuoooooooooooooooooiiiiiaeiiouuncyyyyy";
        for (var i = 0, l = from.length; i < l; i++) {
            str = str.replaceAll(from[i], to[i]);
        }

        str = str.toLowerCase()
            .trim()
            .replace(/[^a-z0-9\-]/g, '')
            .replace(/-+/g, '');

        return str;
    },
    CheckIfSpecialCharracter: function (text) {
        if (text == null || text == undefined || text.trim() == '') {
            return false
        }
        return text.match(/[^a-zA-Z0-9àáãảạăằắẳẵặâầấẩẫậèéẻẽẹêềếểễệđùúủũụưừứửữựòóỏõọôồốổỗộơờớởỡợìíỉĩịäëïîöüûñçýỳỹỵỷ ]/g) ? true : false;
    },
    RemoveSpecialCharacter: function (text) {
        return text.replaceAll(/[^a-zA-Z0-9àáãảạăằắẳẵặâầấẩẫậèéẻẽẹêềếểễệđùúủũụưừứửữựòóỏõọôồốổỗộơờớởỡợìíỉĩịäëïîöüûñçýỳỹỵỷ ]/g, '');
    },
    RemoveSpecialCharacter_ky_tu_DB: function (text) {
        return text.replaceAll(/[^a-zA-Z0-9]/g, "_");
    },
    CheckIfStringIsDate: function (text) {
        if (text == null || text == undefined || text.trim() == '')
            return true
        let noSpace = text.replace(/\s/g, '')
        if (noSpace.length < 3) {
            return false
        }
        var split = noSpace.split('/')
        var date = new Date(split[2], split[1], split[0])
        return date > 0
    },
    GetAmountFromCurrencyInput: function (element) {
        return (isNaN(parseFloat(element.val().replaceAll(',', ''))) ? 0 : parseFloat(element.val().replaceAll(',', '')))
    },
    GetAmountFromHTMLElement: function (element) {
        if (element.html() == undefined || element.html().trim() == '' || isNaN(parseFloat(element.html().replaceAll(',', '')))) {
            return 0;
        }

        return parseFloat(element.html().replaceAll(',', ''))
    },
    RenderFileAttachment: function (element, data_id, type, allow_edit = true, allow_preview = false, separate_confirm = false) {
        var data = {
            id: element.attr('id'),
            DataId: data_id,
            Type: type,
            option: {
                allow_edit: allow_edit,
                allow_preview: allow_preview,
                separate_confirm: separate_confirm
            }
        }
        $.ajax({
            url: "/AttachFile/Widget",
            data: data,
            type: "POST",
            success: function (result) {
                element.html(result)

            }
        });
    },
    ConfirmFileUpload: function (element, data_id) {
        if (data_id != null && data_id != undefined) {
            element.find('.attachment-widget').attr('data-dataid', data_id)
        }
        element.find('.confirm-file').trigger('click')
    },
    GetAttachmentFiles: function (element) {
        var list = []
        element.find('.file').each(function (index, item) {
            var element = $(item)
            var path_split = element.attr('data-path') != undefined && element.attr('data-path') != null ? element.attr('data-path').split('.') : [];

            list.push({
                id: element.attr('data-id'),
                path: element.attr('data-path'),
                ext: path_split != undefined && path_split.length > 0 ? path_split[path_split.length - 1] : '',
            });
        })
        return list
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
    eraseCookie: function (name) {
        document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    },
    delay_callback: function (callback, ms) {
        var timer = 0;
        return function () {
            var context = this, args = arguments;
            clearTimeout(timer);
            timer = setTimeout(function () {
                callback.apply(context, args);
            }, ms || 0);
        };
    },
    GetFirstDayInPreviousMonth: function (yourDate) {
        return new Date(yourDate.getFullYear(), yourDate.getMonth() - 1, 1);
    },
    GetLastDayOfPreviousMonth: function (year, month) {
        return new Date(year, month, 0);
    },
    DateDotNETToDatePicker: function (date) {
        return ("0" + date.getDate()).slice(-2) + '/' + ("0" + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear() ;
    },
    RenderBreadcumb: function (breadcumb_list) {
        var html = `<div class="header-right">
             <div class="container">
                <ol class="breadcrumb">
                  {breadcrumb}
                </ol>
            </div>
            </div>`
        var html_breadcumb = '<li class="breadcrumb-item {active}" {aria} ><a href="{url}">{name}</a></li>'
        var html_sub = ''
        $(breadcumb_list).each(function (index, item) {
            html_sub += html_breadcumb.replaceAll('{url}', item.url).replaceAll('{name}', item.name)
                .replaceAll('{active}', item.activated ? 'active' : '')
                .replaceAll('{aria}', item.activated ? `aria-current="page"` : '')
        });

        $('#topbar .flex').prepend(html.replace('{breadcrumb}', html_sub))
    }
}