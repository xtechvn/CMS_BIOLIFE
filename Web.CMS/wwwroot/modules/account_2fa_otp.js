var _account_2fa_otp = {
    DotText: '',
    OnLoggingText: 'Đang xác minh',
    Initialization: function (token) {
        $('#send-otp').click(function (e) {
            _account_2fa_otp.SendOTP()
        });
        $('#otp').keyup(function (e) {
            $('#validate-general').html('')
        });
    },
    SendOTP: function () {
        _account_2fa_otp.LoopDisplayLoading()
        $('#send-otp').attr('disabled', true);
        var model = {
            otp: $('#otp').val()
        }
        $.ajax({
            url: "/Account/SendOTP",
            method: "POST",
            data: model,
            success: function (result) {
                _account_2fa_otp.LoginSucess=true
                if (result != undefined && result.status == 0) {
                    setTimeout(function () {
                        window.location.href = result.direct;
                    }, 1000);
                }
                else {
                    $('#validate-general').html(result.msg)
                    $('#send-otp').attr('disabled', false);
                    $('#send-otp').removeAttr('disabled');
                    

                }
            },
        });
    },
    LoopDisplayLoading: function () {
        setTimeout(function () {
            _account_2fa_otp.DotText = _account_2fa_otp.DotText + '.'
            $('#send-otp').html(_account_2fa_otp.OnLoggingText + _account_2fa_otp.DotText)
            if (_account_2fa_otp.DotText.trim() == '....') _account_2fa_otp.DotText = ''
            if (!_account_2fa_otp.LoginSucess) _account_2fa_otp.LoopDisplayLoading()
            else {
                $('#send-otp').html('Gửi');
            }
        }, 1000)
    },
};