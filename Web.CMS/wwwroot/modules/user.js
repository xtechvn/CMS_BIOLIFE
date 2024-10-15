$(document).ready(function () {
    var _searchData = {
        userName: '',
        strRoleId: '',
        status: -1,
        currentPage: 1,
        pageSize: 20
    };
    _user.Init(_searchData);

    $('#token-input-role').tokenInput('/Role/GetRoleSuggestionList', {
        queryParam: "name",
        hintText: "Nhập từ khóa tìm kiếm",
        searchingText: "Đang tìm kiếm...",
        placeholder: 'Nhập từ khóa tìm kiếm',
        searchDelay: 500,
        preventDuplicates: true,
        minChars: 4,
        noResultsText: "Không tìm thấy kết quả",
        tokenLimit: 10,
        onAdd: function (item) {
            _user.OnChangeRoleId($(this).val());
        },
        onDelete: function (item) {
            _user.OnChangeRoleId($(this).val());
        }
    });
});

var _changeInterval = null;
$("#ip-kup-search-user").keyup(function (e) {
    if (e.which === 13) {
        _user.OnChangeUser(e.target.value);
    } else {
        clearInterval(_changeInterval);
        _changeInterval = setInterval(function () {
            _user.OnChangeUser(e.target.value);
            clearInterval(_changeInterval);
        }, 500);
    }
});

$('.checkbox-tb-column').click(function () {
    let seft = $(this);
    let id = seft.data('id');
    if (seft.is(':checked')) {
        $('td:nth-child(' + id + '),th:nth-child(' + id + ')').removeClass('mfp-hide');
    } else {
        $('td:nth-child(' + id + '),th:nth-child(' + id + ')').addClass('mfp-hide');
    }
});

$('#grid-data').on('click', 'tr.line-item', function () {
    let seft = $(this);
    let id = seft.data('id');
    let isloadajax = JSON.parse(seft.data('ajaxdetail'));
    let colSpan = $('.checkbox-tb-column:checked').length + 1;

    if (!isloadajax) {
        $.ajax({
            url: "/user/GetDetail",
            type: "post",
            data: { Id: id },
            success: function (result) {
                seft.after(result);
                seft.data('ajaxdetail', "true");
                $('.info-detail').children('td').attr('colspan', colSpan);

            }
        });
    }

    $('.info-detail').addClass('mfp-hide');
    $('.info-detail').children('td').attr('colspan', colSpan);

    if (seft.hasClass('active')) {
        seft.removeClass('active');
        seft.next().addClass('mfp-hide');
    } else {
        $('#grid-data tr.active').removeClass('active');
        seft.addClass('active');
        seft.next().removeClass('mfp-hide');
    }
});

$('#grid-data').on('click', '.btn-panel-detail', function () {
    let seft = $(this);
    let tab = seft.data('tab');
    seft.siblings().removeClass('active');
    seft.addClass('active');
    seft.closest('tr.info-detail').find('.tab-detail').hide();
    seft.closest('tr.info-detail').find('.tab-detail[data-id=' + tab + ']').show();
});

$('#grid-data').on('click', '.data-left li, .data-right li', function () {

    let seft = $(this);

    if (seft.closest('ul').hasClass('data-left')) {
        $('.data-right li').removeClass('active');
    } else {
        $('.data-left li').removeClass('active');
    }

    if (seft.hasClass('active')) {
        $(this).removeClass('active');
    } else {
        $(this).addClass('active');
    }

});

$('#grid-data').on('click', '.btn-change-user-status', function () {
    let seft = $(this);
    let userid = parseInt(seft.data('id'));
    let title = 'Xác nhận tài khoản';
    let description = 'Bạn có chắc chắn muốn đổi trạng thái hoạt động nhân viên này?';
    _msgconfirm.openDialog(title, description, function () {
        $.ajax({
            url: "/user/ChangeUserStatus",
            type: "post",
            data: { id: userid },
            success: function (result) {
                if (result.isSuccess) {
                    _msgalert.success(result.message);
                    if (result.status === 0) {
                        seft.children('span').html('Khóa tài khoản');
                        seft.closest('.tab-detail').find('.text-status-user').html('Đang hoạt động');
                    } else {
                        seft.children('span').html('Mở tài khoản');
                        seft.closest('.tab-detail').find('.text-status-user').html('Tạm ngừng');
                    }
                    $.magnificPopup.close();
                } else {
                    _msgalert.error(result.message);
                }
            }
        });
    });
});

var _user = {

    Init: function (data) {
        this.SearchParam = data;
        this.Search(data);
        this.modal_element = $('#global_modal_popup');
    },

    Search: function (input) {
        $.ajax({
            url: "/user/search",
            type: "post",
            data: input,
            success: function (result) {
                $('#grid-data').html(result);
                $('.checkbox-tb-column').each(function () {
                    let seft = $(this);
                    let id = seft.data('id');
                    if (seft.is(':checked')) {
                        $('td:nth-child(' + id + '),th:nth-child(' + id + ')').removeClass('mfp-hide');
                    } else {
                        $('td:nth-child(' + id + '),th:nth-child(' + id + ')').addClass('mfp-hide');
                    }
                });
            }
        });
    },

    ReLoad: function () {
        this.Search(this.SearchParam);
    },

    OnChangeUser: function (value) {
        var searchobj = this.SearchParam;
        searchobj.userName = value;
        searchobj.currentPage = 1;
        this.SearchParam = searchobj;
        this.Search(searchobj);
    },

    OnChangeStatus: function (value) {
        var searchobj = this.SearchParam;
        searchobj.status = value;
        searchobj.currentPage = 1;
        this.SearchParam = searchobj;
        this.Search(searchobj);
    },

    OnChangeRoleId: function (value) {
        var searchobj = this.SearchParam;
        searchobj.strRoleId = value;
        searchobj.currentPage = 1;
        this.SearchParam = searchobj;
        this.Search(searchobj);
    },

    OnPaging: function (value) {
        var searchobj = this.SearchParam;
        searchobj.currentPage = value;
        this.SearchParam = searchobj;
        this.Search(searchobj);
    },

    OnOpenCreateForm: function () {
        let title = 'Thêm người dùng';
        let url = '/user/AddOrUpdate';
        _user.modal_element.find('.modal-title').html(title);
        _user.modal_element.find('.modal-dialog').css('max-width', '900px');
        _ajax_caller.get(url, { Id: 0 }, function (result) {
            _user.modal_element.find('.modal-body').html(result);
            $('.select2_modal').select2();
            $('.DepartmentId').select2({
                placeholder: "Chọn phòng ban"
            });
            $('.UserPositionId').select2({
                placeholder: "Chọn chức vụ"
            });
            $('.select2_modal_multiple').select2({
                placeholder: "Chọn vai trò"
            });
            $('.select2_modal_multiple').select2({
                placeholder : "Chọn vai trò"
            });
            $('.datepicker-input').daterangepicker({
                singleDatePicker: true,
                showDropdowns: true,
                minYear: 1901,
                maxYear: parseInt(moment().format('YYYY'), 10),
                locale: {
                    format: 'DD/MM/YYYY'
                }
            }, function (start, end, label) {
                $(this).val(start.format('MM/DD/YYYY'));
                $(this).change();
            });
            _user.modal_element.modal('show');
            $('.modal-backdrop').css('z-index','6')
        });

        //let param = { Id: 0 };
        //_magnific.OpenLargerPopup(title, url, param);
        
      
    },

    OnOpenEditForm: function (id) {
        let title = 'Cập nhật người dùng';
        let url = '/user/AddOrUpdate';

        _user.modal_element.find('.modal-title').html(title);
        _user.modal_element.find('.modal-dialog').css('max-width', '900px');
        _ajax_caller.get(url, { Id: id }, function (result) {
            _user.modal_element.find('.modal-body').html(result);
            $('.select2_modal').select2();
            $('.select2_modal_multiple').select2({
                placeholder: "Chọn vai trò"
            });
            $('.DepartmentId').select2({
                placeholder: "Chọn phòng ban"
            });
            $('.UserPositionId').select2({
                placeholder: "Chọn chức vụ"
            });
            $('.select2_modal_multiple').select2({
                placeholder: "Chọn vai trò"
            });
            $('.datepicker-input').daterangepicker({
                singleDatePicker: true,
                showDropdowns: true,
                minYear: 1901,
                maxYear: parseInt(moment().format('YYYY'), 10),
                locale: {
                    format: 'DD/MM/YYYY'
                }
            }, function (start, end, label) {
                $(this).val(start.format('MM/DD/YYYY'));
                $(this).change();
            });
            _user.modal_element.modal('show');
            $('.modal-backdrop').css('z-index', '6')

        });


        //let param = { Id: id };
        //_magnific.OpenLargerPopup(title, url, param);

        
    },
    OnOpenGenQrFrom: function (id) {
        let url = '/User/ViewConfirm';
        let param = {
            id: id
        };
        _magnific.OpenSmallPopup('', url, param);
       
    },
    ConfirmQRCodeUser: function (id) {
        _global_function.AddLoading()
        $.ajax({
            url: '/user/ConfirmPassQr',
            type: 'POST',
            data: {
                id: id,
                pass: $('#ConfirmUserPass').val()
            },
            success: function (result) {
              
                if (result.isSuccess) {
                    _global_function.RemoveLoading()
                    _user.GenQRCodeUser(id)

                } else {
                    _global_function.RemoveLoading()
                    _msgalert.error(result.message);
                   
                }
            },
        });
    },
    GenQRCodeUser: function (id) {
        let url = '/User/QrCodeUser';
        let param = {
            id: id
        };
        _magnific.OpenSmallPopup('', url, param);
        setTimeout(function () {
            $('#QrCode').removeClass('placeholder placeholderqr')
        }, 10)
    },
    OnOpenCopyForm: function (id) {
        let title = 'Thêm người dùng';
        let url = '/user/AddOrUpdate';

        _user.modal_element.find('.modal-title').html(title);
        _user.modal_element.find('.modal-dialog').css('max-width', '900px');
        _ajax_caller.get(url, { Id: id, IsClone: true }, function (result) {
            _user.modal_element.find('.modal-body').html(result);
            $('.DepartmentId').select2({
                placeholder: "Chọn phòng ban"
            });
            $('.UserPositionId').select2({
                placeholder: "Chọn chức vụ"
            });
            $('.select2_modal_multiple').select2({
                placeholder: "Chọn vai trò"
            });
            $('.select2_modal').select2();

            $('.datepicker-input').daterangepicker({
                singleDatePicker: true,
                showDropdowns: true,
                minYear: 1901,
                maxYear: parseInt(moment().format('YYYY'), 10),
                locale: {
                    format: 'DD/MM/YYYY'
                }
            }, function (start, end, label) {
                $(this).val(start.format('MM/DD/YYYY'));
                $(this).change();
            });
            _user.modal_element.modal('show');
            $('.modal-backdrop').css('z-index', '6')

        });
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

    OnUpdateUser: function () {
        let FromCreate = $('#form-create-user');
        FromCreate.validate({
            rules: {
                UserName: "required",
                Email: {
                    required: true,
                    email: true
                },
                Password: {
                    required: true,
                    minlength: 6
                },
                RePassword: {
                    required: true,
                    equalTo: "#Password"
                }
            },
            messages: {
                UserName: "Vui lòng nhập tên đăng nhập",
                Email: {
                    required: 'Vui lòng nhập email',
                    email: 'Email không đúng định dạng'
                },
                Password: {
                    required: 'Vui lòng nhập mật khẩu',
                    minlength: 'Độ dài mật khẩu phải lớn hơn 6 kí tự'
                },
                RePassword: {
                    required: 'Vui lòng nhập lại mật khẩu',
                    equalTo: 'Mật khẩu không chính xác'
                },
                RoleId: {
                    required: 'Vui lòng chọn vai trò cho người dùng',
                },
                DepartmentId: {
                    required: 'Vui lòng chọn phòng ban cho người dùng',
                },
                Level: {
                    required: 'Vui lòng chọn chức vụ của người dùng',
                }
            }
        });

        if (FromCreate.valid()) {
            let form = document.getElementById('form-create-user');
            var formData = new FormData(form);
            let roles = $('#RoleId').val();
            let UserPositionId = $('#UserPositionId').val();
            let Rank = $('#UserPositionId').find(':selected').attr('data-lvl');
            formData.set("RoleId", roles != null ? roles.join(',') : "");
            formData.set("UserPositionId", UserPositionId != null ? UserPositionId : 0);
            formData.set("Rank", Rank != null ? Rank : 0);
            var company_type = ''
            $('.company-type:checked').each(function (index, item) {
                var element = $(this)
                if (company_type.trim() == '') {
                    company_type = '' + element.val()
                }
                else {
                    company_type += ',' + element.val()
                }
            })
            formData.set("CompanyType", company_type);
            formData.set("UserName", $('#UserName').val());
            formData.set("BirthDay", _global_function.GetDayText($('#datepicker').data('daterangepicker').startDate._d, true));
            formData.set("OldCompanyType", $('#form-create-user').attr('data-companytype'))

            /*
            var model = {
                RoleId: roles != null ? roles.join(',') : "",
                UserPositionId: UserPositionId != null ? UserPositionId : 0,
                Rank: Rank != null ? Rank : 0,
                Phone: $('#Phone').val() != undefined ? $('#Phone').val() : '',
                Id: $('#Id').val() != undefined ? $('#Id').val() :0,
                Address: $('#Address').val() != undefined ? $('#Address').val() : '',
                UserName: $('#UserName').val() != undefined ? $('#UserName').val() : '',
                BirthDay: _global_function.GetDayText($('#datepicker').data('daterangepicker').endDate._d,true).split(' ')[0],
                Email: $('#Email').val() != undefined ? $('#Email').val() : '',
                Address: $('#Address').val() != undefined ? $('#Address').val() : '',
                Status: $('#Status').find(':selected').val(),
                DepartmentId: $('#DepartmentId').find(':selected').val(),
                Note: $('#Note').val(),
            }
            
            var list_file=[]
            $($('input[name="imagefile"]')[0].files).each(function (index, item) {
                list_file.push(item)
            });
            */
            $.ajax({
                url: '/user/upsert',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (result) {
                    if (result.isSuccess) {
                        _msgalert.success(result.message);
                        _user.ReLoad();
                        _user.modal_element.modal('hide');
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

    OnUpdateRole: function (userid, type) {
        let _arrRole = [];
        let _arrRoleData = [];

        if (type === 0) {
            $('.data-left[data-id=' + userid + '] li.active').each(function () {
                let _roleid = $(this).data('id');
                let _rolename = $(this).html();
                _arrRole.push(parseInt(_roleid));
                _arrRoleData.push({ roleid: _roleid, rolename: _rolename })
            });
            $('.data-right[data-id=' + userid + ']').each(function () {
                let _roleid = $(this).data('id');
                let _rolename = $(this).html();
                _arrRole.push(parseInt(_roleid));
                _arrRoleData.push({ roleid: _roleid, rolename: _rolename })
            });
        } else {
            $('.data-right[data-id=' + userid + '] li.active').each(function () {
                let _roleid = $(this).data('id');
                let _rolename = $(this).html();
                _arrRole.push(parseInt(_roleid));
                _arrRoleData.push({ roleid: _roleid, rolename: _rolename })
            });
        }

        if (_arrRole.length > 0) {
            let input = {
                userId: userid,
                arrRole: _arrRole,
                type: type
            }
            $.ajax({
                url: "/user/UpdateUserRole",
                type: "post",
                data: input,
                success: function (result) {
                    if (result.isSuccess) {
                        let strHtml = "";
                        _arrRoleData.map((obj) => strHtml += '<li data-id="' + obj.roleid + '">' + obj.rolename + '</li>');
                        if (type === 0) {
                            $('.data-left[data-id=' + userid + '] li.active').remove();
                            $('.data-right[data-id=' + userid + ']').append(strHtml);
                        } else {
                            $('.data-right[data-id=' + userid + '] li.active').remove();
                            $('.data-left[data-id=' + userid + ']').append(strHtml);
                        }
                    }
                }
            });
        }
       
    },

    OnResetPassword: function (id) {
        let title = 'Reset mật khẩu người dùng';
        let description = 'Bạn có chắc chắn muốn reset mật khẩu của nhân viên này?';
        _msgconfirm.openDialog(title, description, function () {
            $.ajax({
                url: "/user/ResetPasswordByUserId",
                type: "post",
                data: { userId: id },
                success: function (data) {
                    if (data.isSuccess) {
                        _msgalert.success(data.message);
                        _magnific.OpenResetPasswordPopup(data.result);
                    } else {
                        _msgalert.error(data.message);
                    }
                }
            });
        });
    },

    OnResetMFA: function (id) {
        let title = 'Reset bảo mật 2 lớp';
        let description = 'Bạn có chắc chắn muốn reset MFA của nhân viên này?';
        _msgconfirm.openDialog(title, description, function () {
            $.ajax({
                url: "/user/ResetMFA",
                type: "post",
                data: { id: id },
                success: function (data) {
                    if (data.isSuccess) {
                        _msgalert.success(data.message);
                    } else {
                        _msgalert.error(data.message);
                    }
                }
            });
        });
    },
    
};