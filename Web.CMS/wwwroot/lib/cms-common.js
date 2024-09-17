// form validate extension method
jQuery.validator.addMethod("valueNotEquals", function (value, element, arg) {
    return arg !== value;
}, "Value must not equal arg.");

jQuery.validator.addMethod("exactlength", function (value, element, param) {
    return this.optional(element) || value.length == param;
}, $.validator.format("Please enter exactly {0} characters."));

// ----------------------

function ConvertToJSONDate(strdate) {
    if (strdate == null || strdate == "") {
        return null;
    }
    let arrdate = strdate.split("/");
    var jsdate = new Date(arrdate[2], arrdate[1] - 1, arrdate[0]);
    return jsdate.toJSON();
}

function ConvertToJSONDateTime(strdatetime) {
    if (strdatetime == null || strdatetime == "") {
        return null;
    }
    let arrdate = strdatetime.split(" ")[0].split("/");
    let arrtime = strdatetime.split(" ")[1].split(":");

    var jsdate = new Date(arrdate[2], arrdate[1] - 1, arrdate[0], parseInt(arrtime[0]), parseInt(arrtime[1]));

    return new Date(jsdate.getTime() - (jsdate.getTimezoneOffset() * 60000)).toJSON();
}

function ConvertJsDateToString(date) {
    return ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()))
        + '/' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1)))
        + '/' + date.getFullYear();
}

//----------- Input vn curency number----------
$(document).on('keyup blur', '.currency', function () {
    formatCurrency($(this));
});

$(document).on('keyup blur', '.currency-usd', function () {
    formatDecimalCurrency($(this));
});

function formatNumber(n) {
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}

function formatCurrency(input, blur) {
    var input_val = input.val();
    if (input_val === "") { return; }
    var original_len = input_val.length;
    var caret_pos = input.prop("selectionStart");

    if (input_val.indexOf(".") >= 0) {
        var decimal_pos = input_val.indexOf(".");
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos);
        left_side = formatNumber(left_side);
        right_side = formatNumber(right_side);

        if (blur === "blur") {
            right_side += "00";
        }

        right_side = right_side.substring(0, 2);
        input_val = left_side + "." + right_side;
    } else {
        input_val = formatNumber(input_val);
    }

    // send updated string to input
    input.val(input_val);
    // put caret back in the right position
    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    input[0].setSelectionRange(caret_pos, caret_pos);
}

function formatDecimalNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}

function formatDecimalCurrency(input, blur) {
    // get input value
    var input_val = input.val();

    // don't validate empty input
    if (input_val === "") { return; }

    // original length
    var original_len = input_val.length;

    // initial caret position 
    var caret_pos = input.prop("selectionStart");

    // check for decimal
    if (input_val.indexOf(".") >= 0) {

        // get position of first decimal
        // this prevents multiple decimals from
        // being entered
        var decimal_pos = input_val.indexOf(".");

        // split number by decimal point
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos);

        // add commas to left side of number
        left_side = formatDecimalNumber(left_side);

        // validate right side
        right_side = formatDecimalNumber(right_side);

        // On blur make sure 2 numbers after decimal
        if (blur === "blur") {
            right_side += "00";
        }

        // Limit decimal to only 2 digits
        right_side = right_side.substring(0, 2);

        // join number by .
        input_val = left_side + "." + right_side;

    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        input_val = formatDecimalNumber(input_val);
        input_val = input_val;

        // final formatting
        if (blur === "blur") {
            input_val += ".00";
        }
    }

    // send updated string to input
    input.val(input_val);

    // put caret back in the right position
    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    input[0].setSelectionRange(caret_pos, caret_pos);
}

function ConvertMoneyToNumber(strMoney) {
    return parseFloat(strMoney.replace(/,/g, ""));
}
//----------- End input curency number------

function LoadingComponent() {
    var html = '<div class="wrap_bg mb20">'
        + '<div class="placeholder mb10" style="height: 50px;"></div>'
        + '<div class="placeholder mb10" style="height: 50px; width: 200px;"></div>'
        + '<div class="box-create-api" style="padding:10px;">'
        + '<div class="placeholder mb10" style="height: 80px;"></div>'
        + '<div class="placeholder mb10" style="height: 80px;"></div>'
        + '<div class="placeholder" style="height: 80px;"></div>'
        + '</div>'
        + '</div>';
    return html;
}

$(document).ready(function () {
    /*
    $('.datepicker-input').Zebra_DatePicker({
        format: 'd/m/Y',
        onSelect: function () {
            $(this).change();
        }
    }).removeAttr('readonly');


    $('.datetimepicker-input').Zebra_DatePicker({
        format: 'd/m/Y H:i',
        onSelect: function () {
            $(this).change();
        }
    }).removeAttr('readonly');
    */
    $('.datetimepicker-input').daterangepicker({
        singleDatePicker: true,
        timePicker: true,
        minYear: 1901,
        startDate: moment().startOf('hour'),
        endDate: moment().startOf('hour').add(32, 'hour'),
        locale: {
            format: 'M/DD hh:mm A'
        }
    }, function (start, end, label) {
        $(this).val(start.format('MM/DD/YYYY HH:mm:ss'));
        $(this).change();

    });

    $('.datepicker-input').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        minYear: 1901,
        maxYear: parseInt(moment().format('YYYY'), 10),
        locale: {
            format: 'DD/M/YYYY'
        }
    }, function (start, end, label) {
        $(this).val(start.format('MM/DD/YYYY'));
        $(this).change();

    });

    $('.select2').select2();

    //var path = window.location.pathname;
    //var a_filter = ' li > a[href$="' + path + '"]';
    //$(a_filter).css('background-color', 'var(--color-main2)');
});

var _common = {
    tinyMce: function (element_id, height = 600) {
        var useDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
        tinymce.init({
            selector: element_id,
            plugins: 'print preview paste importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons',
            imagetools_cors_hosts: ['picsum.photos'],
            menubar: 'file edit view insert format tools table help',
            toolbar: 'undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | ltr rtl',
            toolbar_sticky: true,
            autosave_ask_before_unload: false,
            autosave_interval: '30s',
            autosave_prefix: '{path}{query}-{id}-',
            autosave_restore_when_empty: false,
            autosave_retention: '2m',
            image_advtab: true,
            link_list: [
                { title: 'My page 1', value: 'https://www.tiny.cloud' },
                { title: 'My page 2', value: 'http://www.moxiecode.com' }
            ],
            image_list: [
                { title: 'My page 1', value: 'https://www.tiny.cloud' },
                { title: 'My page 2', value: 'http://www.moxiecode.com' }
            ],
            image_class_list: [
                { title: 'None', value: '' },
                { title: 'Some class', value: 'class-name' }
            ],
            importcss_append: true,
            file_picker_callback: function (callback, value, meta) {
                /* Provide file and text for the link dialog */
                if (meta.filetype === 'file') {
                    callback('https://www.google.com/logos/google.jpg', { text: 'My text' });
                }

                /* Provide image and alt text for the image dialog */
                if (meta.filetype === 'image') {
                    callback('https://www.google.com/logos/google.jpg', { alt: 'My alt text' });
                }

                /* Provide alternative source and posted for the media dialog */
                if (meta.filetype === 'media') {
                    callback('movie.mp4', { source2: 'alt.ogg', poster: 'https://www.google.com/logos/google.jpg' });
                }
            },
            templates: [
                { title: 'New Table', description: 'creates a new table', content: '<div class="mceTmpl"><table width="98%%"  border="0" cellspacing="0" cellpadding="0"><tr><th scope="col"> </th><th scope="col"> </th></tr><tr><td> </td><td> </td></tr></table></div>' },
                { title: 'Starting my story', description: 'A cure for writers block', content: 'Once upon a time...' },
                { title: 'New list with dates', description: 'New List with dates', content: '<div class="mceTmpl"><span class="cdate">cdate</span><br /><span class="mdate">mdate</span><h2>My List</h2><ul><li></li><li></li></ul></div>' }
            ],
            template_cdate_format: '[Date Created (CDATE): %m/%d/%Y : %H:%M:%S]',
            template_mdate_format: '[Date Modified (MDATE): %m/%d/%Y : %H:%M:%S]',
            height: height,
            image_caption: true,
            quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
            noneditable_noneditable_class: 'mceNonEditable',
            toolbar_mode: 'sliding',
            contextmenu: 'link image imagetools table',
            skin: useDarkMode ? 'oxide-dark' : 'oxide',
            content_css: useDarkMode ? 'dark' : 'default',
            content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }'
        });
    }
};

var _imageError = function (element) {
    element.onerror = null;
    element.src = '/images/icons/noimage.png';
};

var _msgalert = {
    error: (content, title) => {
        toastr.error(content, title);
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "1000",
            "hideDuration": "1000",
            "timeOut": "3000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        return false;
    },

    success: (content, title) => {
        toastr.success(content, title);
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "1000",
            "hideDuration": "1000",
            "timeOut": "3000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        return true;
    },

    notify_tooltip: (content, title) => {
        toastr.success(content, title);
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "1000",
            "hideDuration": "1000",
            "timeOut": "10000",
            "extendedTimeOut": "2000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        return true;
    }
};

var _msgconfirm = {
    openDialog: function (title, description, callback) {
        Swal.fire({
            title: title,
            text: description,
            // width: 600,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#ED5C6A',
            cancelButtonColor: '#A6A4A4',
            confirmButtonText: '<i class="fa fa-check"></i> Đồng ý',
            cancelButtonText: '<i class="fa fa-minus-circle"></i> Bỏ qua'
        }).then((result) => {
            if (result.value) {
                callback();
            }
        })
    },

    alertSuccess: function (title, description) {
        Swal.fire({
            title: title,
            text: description,
            icon: 'success',
            confirmButtonColor: '#ED5C6A',
            confirmButtonText: '<i class="fa fa-check"></i> Đồng ý',
        });
    },

    alertError: function (title, description) {
        Swal.fire({
            title: title,
            text: description,
            icon: 'error',
            confirmButtonColor: '#ED5C6A',
            confirmButtonText: '<i class="fa fa-check"></i> Đồng ý',
        });
    },
};

var _magnific = {

    OpenLargerPopup: function (title, url, param) {
        let elPopup = $('#magnific-popup-large');
        let elTitle = elPopup.find('.magnific-title');
        let elBody = elPopup.find('.magnific-body');
        elTitle.html(title);

        $.ajax({
            url: url,
            type: "post",
            data: param,
            success: function (result) {
                elBody.html(result);
            }
        });

        jQuery.magnificPopup.open({
            items: {
                src: elPopup
            },
            type: 'inline',
            midClick: true,
            mainClass: 'mfp-with-zoom',
            fixedContentPos: false,
            fixedBgPos: true,
            overflowY: 'auto',
            closeBtnInside: true,
            closeOnBgClick: false,
            preloader: false,
            removalDelay: 300
        });
    },

    OpenSmallPopup: function (title, url, param) {
        let elPopup = $('#magnific-popup-small');
        let elTitle = elPopup.find('.magnific-title');
        let elBody = elPopup.find('.magnific-body');
        elTitle.html(title);
        $.ajax({
            url: url,
            type: "post",
            data: param,
            success: function (result) {
                elBody.html(result);
            }
        });

        jQuery.magnificPopup.open({
            items: {
                src: elPopup
            },
            type: 'inline',
            midClick: true,
            mainClass: 'mfp-with-zoom',
            fixedContentPos: false,
            fixedBgPos: true,
            overflowY: 'auto',
            closeBtnInside: true,
            closeOnBgClick: false,
            preloader: false,
            removalDelay: 300
        });
    },
    OpenSmallPopupWithHeader: function (title, url, param, callback = null) {
        let elPopup = $('#magnific-popup-small-header');
        let elTitle = elPopup.find('.magnific-title');
        let elBody = elPopup.find('.magnific-body');
        elTitle.html(title);
        $.ajax({
            url: url,
            type: "post",
            data: param,
            success: function (result) {
                elBody.html(result);
                if (callback != null) {
                    callback();
                }
            }
        });

        jQuery.magnificPopup.open({
            items: {
                src: elPopup
            },
            type: 'inline',
            midClick: true,
            mainClass: 'mfp-with-zoom',
            fixedContentPos: false,
            fixedBgPos: true,
            overflowY: 'auto',
            closeBtnInside: true,
            closeOnBgClick: false,
            preloader: false,
            removalDelay: 300
        });
    },

    OpenConfirmPopup: function (title, textbody, callback) {
        this.FncCallBack = callback;
        let elPopup = $('#magnific-popup-confirm');
        let elTitle = elPopup.find('.magnific-title');
        let elBody = elPopup.find('.magnific-body');
        elTitle.html(title);
        elBody.html(textbody);
        jQuery.magnificPopup.open({
            items: {
                src: elPopup
            },
            type: 'inline',
            midClick: true,
            mainClass: 'mfp-with-zoom',
            fixedContentPos: false,
            fixedBgPos: true,
            overflowY: 'auto',
            closeOnBgClick: false,
            closeBtnInside: true,
            preloader: false,
            removalDelay: 300
        });
    },

    OnConfirm: function () {
        this.FncCallBack();
    },

    OpenResetPasswordPopup: function (result) {
        let elPopup = $('#magnific-popup-password');
        let elBody = elPopup.find('.magnific-body');
        elBody.html(result);
        jQuery.magnificPopup.open({
            items: {
                src: elPopup
            },
            type: 'inline',
            midClick: true,
            mainClass: 'mfp-with-zoom',
            fixedContentPos: false,
            fixedBgPos: true,
            overflowY: 'auto',
            closeBtnInside: true,
            preloader: false,
            removalDelay: 300
        });
    }
};

var _ajax_caller = {
    post: function (url, param = null, callback = null) {
        $.ajax({
            url: url,
            type: "POST",
            data: param,
            success: function (result) {
                callback(result);
            }
        });
    },

    postJson: function (url, param = null, callback = null) {
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(param),
            success: function (result) {
                callback(result);
            }
        });
    },

    get: function (url, param = null, callback = null) {
        $.ajax({
            url: url,
            type: "GET",
            data: param,
            success: function (result) {
                callback(result);
            }
        });
    }
};

var _account = {
    OnGetDatailUser: function () {
        let title = 'Thông tin cá nhân';
        let url = '/user/userprofile';
        _magnific.OpenLargerPopup(title, url);
    },

    OnChangeImage: function () {
        const preview = document.querySelector('.img-preview');
        const file = document.querySelector('input[name=imagefile]').files[0];
        const reader = new FileReader();
        reader.addEventListener("load", function () {
            preview.src = reader.result;
        }, false);
        if (file) {
            reader.readAsDataURL(file);
        }
    },

    OnGetChangePassUser: function () {
        let title = 'Thay đổi mật khẩu cá nhân';
        let url = '/user/UserChangePass';
        _magnific.OpenLargerPopup(title, url);
    },

    OnUpdate: function () {
        let FromValid = $('#form-user-profile');
        FromValid.validate({
            rules: {
                UserName: "required",
                Email: {
                    required: true,
                    email: true
                }
            },
            messages: {
                UserName: "Vui lòng nhập tên đăng nhập",
                Email: {
                    required: 'Vui lòng nhập email',
                    email: 'Email không đúng định dạng'
                }
            }
        });

        if (FromValid.valid()) {
            let form = document.getElementById('form-user-profile');
            var formData = new FormData(form);
            $.ajax({
                url: '/user/upsert',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (result) {
                    if (result.isSuccess) {
                        _msgalert.success(result.message);
                        $.magnificPopup.close();
                    } else {
                        _msgalert.error(result.message);
                    }
                },
                error: function (jqXHR) {
                },
                complete: function (jqXHR, status) {
                }
            });
        }
    },

    OnChangePassword: function () {
        let FormValid = $('#form-user-change-password');
        FormValid.validate({
            rules: {
                Password: {
                    required: true,
                    minlength: 6
                },
                NewPassword: {
                    required: true,
                    minlength: 6
                },
                ReNewPassword: {
                    required: true,
                    equalTo: "#NewPassword"
                }
            },
            messages: {
                Password: {
                    required: 'Vui lòng nhập mật khẩu hiện tại của bạn',
                    minlength: 'Độ dài mật khẩu phải lớn hơn 6 kí tự'
                },
                NewPassword: {
                    required: 'Vui lòng nhập mật khẩu mới',
                    minlength: 'Độ dài mật khẩu phải lớn hơn 6 kí tự'
                },
                ReNewPassword: {
                    required: 'Vui lòng nhập lại mật khẩu mới',
                    equalTo: 'Mật khẩu gõ lại không khớp với mật khẩu mới'
                }
            }
        });

        if (FormValid.valid()) {
            let form = document.getElementById('form-user-change-password');
            var formData = new FormData(form);
            $.ajax({
                url: '/user/changepassword',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (result) {
                    if (result.isSuccess) {
                        var title = "Thông báo đổi mật khẩu";
                        var content = "Mật khẩu của bạn đã được đổi thành công";
                        _msgalert.success(content, title);
                        FormValid.trigger("reset");
                        $.magnificPopup.close();
                    } else {
                        _msgalert.error(result.message);
                    }
                },
                error: function (jqXHR) {
                },
                complete: function (jqXHR, status) {
                }
            });
        }
    },
    logOut: function () {
        localStorage.removeItem('url_redirect');
    },
};

var _chart = {
    LoadChartRevenu: function (listChartRevenu, firstTime, id) {
        var listLabel = []
        var listData = []
        for (var i = 0; i < listChartRevenu.length; i++) {
            listLabel.push(listChartRevenu[i].dateStr);
            listData.push(listChartRevenu[i].totalRevenu);
        }
        var options = {
            series: [{
                name: "Doanh thu",
                data: listData
            }],
            chart: {
                id: 'chartRevenu',
                type: 'bar',
                height: 350
            },
            plotOptions: {
                bar: {
                    horizontal: false,
                    columnWidth: '30%',
                }
            },
            colors: ['#49b291'],
            dataLabels: {
                enabled: false,
                style: {
                    fontSize: '14px',
                    fontFamily: 'Helvetica, Arial, sans-serif',
                    fontWeight: 'bold',
                    colors: undefined
                },
                formatter: function (val, opts) {
                    return val
                },
            },
            xaxis: {
                categories: listLabel,
            }
        };

        var chart = new ApexCharts(document.querySelector("#" + id), options);
        if (!firstTime) {
            $('#' + id).html()
        }
        chart.render();
    },
    LoadChartLabelRevenu: function (listChartLabelRevenu, firstTimeLabel, id) {
        var listLabel = []
        var listData = []
        for (var i = 0; i < listChartLabelRevenu.length; i++) {
            listLabel.push(listChartLabelRevenu[i].label);
            listData.push(listChartLabelRevenu[i].totalRevenu);
        }
        var options = {
            series: [{
                name: "Doanh thu",
                data: listData
            }],
            chart: {
                id: 'chartRevenu',
                type: 'bar',
                height: 350
            },
            plotOptions: {
                bar: {
                    horizontal: false,
                    columnWidth: '30%',
                }
            },
            colors: ['#49b291'],
            dataLabels: {
                enabled: false,
                style: {
                    fontSize: '14px',
                    fontFamily: 'Helvetica, Arial, sans-serif',
                    fontWeight: 'bold',
                    colors: undefined
                },
                formatter: function (val, opts) {
                    return val
                },
            },
            xaxis: {
                categories: listLabel,
            }
        };

        var chart = new ApexCharts(document.querySelector("#" + id), options);
        if (!firstTimeLabel) {
            $('#' + id).html()
        }
        chart.render();
    }
};
