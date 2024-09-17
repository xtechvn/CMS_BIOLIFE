var _account_2fa_setup = {
    Initialization: function () {

    },
    OnMFATestGG: function () {
        var model = {
            otp: $('#otp-test').val(),
        };
        $.ajax({
            url: "/Account/OTPTest",
            method: "POST",
            data: model,
            success: function (result) {
                if (result != undefined && result.status == 0) {
                    _msgalert.success(result.msg)
                }
                else {
                    _msgalert.error(result.msg)

                }
            },
        });
    },
    Confirm: function () {
        $.ajax({
            url: "/Account/Confirm2FA",
            method: "POST",
            data: {
            },
            success: function (result) {
                if (result != undefined && result.status == 0) {
                    _msgalert.success(result.msg)
                    setTimeout(function () {
                        window.location.href = '/';
                    }, 1000);
                }
                else {
                    _msgalert.error(result.msg)

                }
            },
        });
    },
};