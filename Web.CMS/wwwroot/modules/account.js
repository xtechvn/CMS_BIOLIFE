$(document).ready(function () {
    _login.Initialization()
})
var _login = {
    Token: '',
    DotText:'',
    OnLoggingText: '<i class="fa fa-sign-in"></i>Đang đăng nhập',
    LoginSucess: false,
    Initialization: function () {
        $('#user-name').keyup(function (e) {
            $('#validate-username').html('')
            $('#validate-general').html('')
        });
        $('#password').keyup(function (e) {
            $('#validate-password').html('')
            $('#validate-general').html('')
        });
    },

    Login: function () {
        _login.LoopDisplayLoading()
        $('.btn-login').attr('disabled', true);
        var model = {
            UserName: $('#user-name').val(),
            Password: $('#password').val(),
            RememberMe: false,
            ReturnUrl: $('#form-login').attr('data-url')
        }
        if ($('#remember-me').is(":checked")) {
            model.RememberMe=true
        }
        if (model.UserName == undefined || model.UserName.trim() == '') {
            $('#validate-username').html('Tài khoản không được để trống, vui lòng thử lại')
        }
        if (model.Password == undefined || model.Password.trim() == '') {
            $('#validate-password').html('Mật khẩu không được để trống, vui lòng thử lại')
        }
        $.ajax({
            url: "ConfirmLogin",
            type: "post",
            data: { model: model },
            success: function (result) {
                _login.LoginSucess = true
                $('.btn-login').removeAttr('disabled');
                if (result != undefined && result.status == 0) {
                    window.location.href = result.direct;
                }
                else {
                    $('#validate-general').html(result.msg);
                    $('.img_loading_summit').remove()

                }

            }
        });
    },
    LoopDisplayLoading: function () {
        setTimeout(function () {
            if (_login.DotText.trim() == '...') _login.DotText = ''
            $('.btn-login').html(_login.OnLoggingText + _login.DotText)
            _login.DotText = _login.DotText + '.'
            if (!_login.LoginSucess) _login.LoopDisplayLoading()
            else {
                $('.btn-login').html('<i class="fa fa-sign-in"></i>Đăng nhập');
            }
        }, 1000)
    },
}