var _accountAccessAPI =
{
    Init: function () {
        this.modal_element = $('#global_modal_popup');
    },

    ShowAddform: function ()
    {
        let url = '/AccountAccessAPI/Add';

        _ajax_caller.get(url,null, function (result) {
            $('body').append(result);
            $('#AddAAA').addClass('show')
        });
    },

    ShowUpdateform: function (id, id_allcode, id_accountaccessapipermission) {
        let url = '/AccountAccessAPI/Update';

        _ajax_caller.get(url, { id: id, id_AllCode: id_allcode, id_AccountAccessAPIPermission : id_accountaccessapipermission}, function (result) {
            $('body').append(result);
            $('#UpdateOrDeleteAAA').addClass('show')
        });
    },

    Add: function () {

        let CodeValue = document.getElementById("codevalue").value;
        let UserName = document.getElementById("username").value;
        let Password = document.getElementById("password").value;
        let Description = document.getElementById("description").value;
        const StatusButton = document.getElementsByName('status');
        let Status = null;
        for (const radio of StatusButton) {
            if (radio.checked) {
                Status = radio.value;
            }
        }

        if ($('#codevalue').val() == 'default')
        {
            $('#codevalue').addClass('is-invalid');
            return;
        }
        else
        {
            $('#codevalue').removeClass('is-invalid');
        }


        if (!/^[a-zA-Z0-9._]+$/.test(UserName) || UserName === null) {
            $('#username').addClass('is-invalid');
            return;
        }
        else
        {
            $('#username').removeClass('is-invalid');
        }

        if (!/^(?=.*\d)(?=.*[a-zA-Z]).{6,}$/.test(Password))
        {
            $('#password').addClass('is-invalid');
            return;
        }
        else {
            $('#password').removeClass('is-invalid');
        }



        /*let Form = $('#add-aaa-form');
        Form.validate({
            rules: {
                username: {
                    required: true,
                },
                code: "required",
                password: "required"
            },
            messages: {
                username: "Tên đăng không được bỏ trống và chỉ chứa chữ cái,chữ số ,dấu . và dấu _",
                code: "Vui lòng chọn dự án",
                password: "Mật khẩu không được bỏ trống hay chứa dấu cách"
            }
        });


        if (!Form.valid()) {
            return;
        }*/


        

        var AccountAccessAPI =
        {
            "UserName": UserName,
            "Password": Password,
            "Status": Status,
            "Description": Description
        }

        
        let urlAAA = "/AccountAccessAPI/InsertAccountAccessAPI";


        _ajax_caller.post(urlAAA, { model: AccountAccessAPI, codevalue: CodeValue }, function (result) {
            if (result.isSuccess) {
                _msgalert.success(result.message);
                _accountAccessAPI.modal_element.modal('hide');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                _msgalert.error(result.message);
            }
        });
    },

    Update: function (Id, Id_AccountAccessAPIPermission) {

        let CodeValue = document.getElementById("codevalue").value;
        /*let Id = document.getElementById("Id").value;*/
        let UserName = document.getElementById("username").value;
        let Password = document.getElementById("password").value;
        let Description = document.getElementById("description").value;
        const StatusButton = document.getElementsByName('status');
        let Status = null;
        for (const radio of StatusButton) {
            if (radio.checked) {
                Status = radio.value;
            }
        }

        if (!/^[a-zA-Z0-9._]+$/.test(UserName) || UserName === null) {
            $('#username').addClass('is-invalid');
            return;
        }
        else {
            $('#username').removeClass('is-invalid');
        }

        if (Password != '')
        {
            if (!/^(?=.*\d)(?=.*[a-zA-Z]).{6,}$/.test(Password)) {
                $('#password').addClass('is-invalid');
                return;
            }
            else {
                $('#password').removeClass('is-invalid');
            }
        }

        var AccountAccessAPI =
        {
            "Id":Id,
            "UserName": UserName,
            "Password": Password,
            "Status": Status,
            "Description": Description
        }

        let urlAAA = "/AccountAccessAPI/UpdateAccountAccessAPI";


        _ajax_caller.post(urlAAA, { model: AccountAccessAPI, codevalue: CodeValue, id_accountAccessapipermission: Id_AccountAccessAPIPermission }, function (result) {
            if (result.isSuccess) {
                _msgalert.success(result.message);
                _accountAccessAPI.modal_element.modal('hide');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                _msgalert.error(result.message);
            }
        });
    },

    Close: function (id) {

        $('#'+id).removeClass('show')
        setTimeout(function () {
            $('#'+id).remove();

        }, 300);
    },

    ResetPassword: function (id)
    {
        let urlAAA = "/AccountAccessAPI/ResetPassword";
        _ajax_caller.post(urlAAA, { id: id }, function (result) {
            if (result.isSuccess) {
                _msgalert.success(result.message);
                _accountAccessAPI.modal_element.modal('hide');
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                _msgalert.error(result.message);
            }
        });
    }

}

$(document).ready(function () {
    $('input').attr('autocomplete', 'off');
    _accountAccessAPI.Init();
});
