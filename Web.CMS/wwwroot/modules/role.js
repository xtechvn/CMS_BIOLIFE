$(document).ready(function () {
    var _searchData = {
        roleName: '',
        strUserId: '',
        currentPage: 1,
        pageSize: 30
    };
    _role.Init(_searchData);

    $('#token-input-user').tokenInput('/User/GetUserSuggestionList', {
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
            _role.OnChangeUserId($(this).val());
        },
        onDelete: function (item) {
            _role.OnChangeUserId($(this).val());
        }
    });
});

var _changeInterval = null;
$("#ip-kup-search-role").keyup(function (e) {
    if (e.which === 13) {
        _role.OnChangeRoleName(e.target.value)
    } else {
        clearInterval(_changeInterval);
        _changeInterval = setInterval(function () {
            _role.OnChangeRoleName(e.target.value);
            clearInterval(_changeInterval);
        }, 500);
    }
});

$('#grid-data').on('click', 'tr.line-item td', function () {
    let _tabActive = 1;
    let seft = $(this);
    let seftparent = seft.closest('tr.line-item');

    if (seft.hasClass('count-user')) {
        _tabActive = 2;
    }

    let id = seftparent.data('id');
    let isloadajax = JSON.parse(seftparent.data('ajaxdetail'));

    if (seftparent.hasClass('active')) {
        seftparent.removeClass('active');
        seftparent.next().addClass('mfp-hide');
    } else {
        seftparent.siblings('tr.line-item').removeClass('active');
        seftparent.addClass('active');
        if (!isloadajax) {
            _role.OnGetDetail(id, _tabActive, function (result) {
                seftparent.data('ajaxdetail', "true");
                seftparent.siblings('.info-detail').addClass('mfp-hide');
                seftparent.after(result);
            });
        } else {
            seftparent.siblings('.info-detail').addClass('mfp-hide');
            seftparent.next().find('.tab-default a[data-tab="' + _tabActive + '"]').trigger('click');
            seftparent.next().removeClass('mfp-hide');
        }
    }
});

$('#grid-data').on('click', '.tab-default a', function () {
    let seft = $(this);
    let tab = parseInt(seft.data('tab'));

    if (tab === 2) {
        let id = parseInt(seft.data('id'));
        _role.OnLoadUserData(id, function (result) {
            seft.closest('tr').find('.grid-user-role').html(result);
        });
    }

    if (tab === 3) {
        let id = parseInt(seft.data('id'));
        _role.OnLoadMenuPermission(id, function (result) {
            seft.closest('tr').find('.grid-menu-permission').html(result);
            $(".scrollbar-inner").scrollbar();
        });
    }

    seft.siblings().removeClass('active');
    seft.addClass('active');
    seft.closest('tr.info-detail').find('.tab-detail').hide();
    seft.closest('tr.info-detail').find('.tab-detail[data-id=' + tab + ']').show();
});

$('#grid-data').on('click', '.btn--toggle-permission', function () {
    if (!$(this).closest('li').hasClass("active")) {
        $(this).closest('li').addClass("active");
        $(this).closest('li').find('.lever2').slideDown('fast');
    } else {
        $(this).closest('li').removeClass("active");
        $(this).closest('li').find('.lever2').slideUp('fast');
    }
});

$('#grid-data').on('click', '.ckb-item-menu', function () {
    var seft = $(this);
    var _arrayParam = '';
    var _updateType = 0;
    var seftParent = seft.closest('li');
    var isParentActived = seftParent.hasClass("active");

    if (!isParentActived) {
        seftParent.addClass("active");
        seftParent.find('.lever2').slideDown('fast');
    }

    if (seft.is(':checked')) {
        _updateType = 1;
        seftParent.find('.ckb-role-permission').prop('checked', true);
    } else {
        _updateType = 0;
        seftParent.find('.ckb-role-permission').prop('checked', false);
    }

    seftParent.find('.ckb-role-permission').each(function () {
        _arrayParam += $(this).data('param') + ',';
    });
    _role.OnUpdateRolePermission(_arrayParam, _updateType);
});

$('#grid-data').on('click', '.ckb-role-permission', function () {
    let seft = $(this);
    var seftParent = seft.closest('li');
    var _checkParent = false;
    let _param = seft.data('param');
    let _type = seft.is(":checked") ? 1 : 0;
    if (seftParent.find('.ckb-role-permission:checked').length > 0) {
        _checkParent = true;
    }
    _role.OnUpdateRolePermission(_param, _type);
    seftParent.find('.ckb-item-menu').prop('checked', _checkParent);
});

var _role = {
    Init: function (data) {
        this.SearchParam = data;
        this.Search(data);
    },

    Search: function (input) {
        $.ajax({
            url: "/role/search",
            type: "post",
            data: input,
            success: function (result) {
                $('#grid-data').html(result);
            }
        });
    },

    ReLoad: function () {
        this.Search(this.SearchParam);
    },

    OnChangeRoleName: function (value) {
        var searchobj = this.SearchParam;
        searchobj.roleName = value;
        searchobj.currentPage = 1;
        this.SearchParam = searchobj;
        this.Search(searchobj);
    },

    OnChangeUserId: function (value) {
        var searchobj = this.SearchParam;
        searchobj.strUserId = value;
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
        let title = 'Thêm nhóm quyền';
        let url = '/Role/AddOrUpdate';
        let param = { Id: 0 };
        _magnific.OpenSmallPopup(title, url, param);
    },

    OnOpenEditForm: function (id) {
        let title = 'Cập nhật nhóm quyền';
        let url = '/Role/AddOrUpdate';
        let param = { Id: id };
        _magnific.OpenSmallPopup(title, url, param);
    },

    OnUpsert() {
        let FromCreate = $('#form-role');
        FromCreate.validate({
            rules: {
                Name: "required",
            },
            messages: {
                Name: "Vui lòng nhập tên nhóm quyền",
            }
        });

        if (FromCreate.valid()) {
            let form = document.getElementById('form-role');
            var formData = new FormData(form);
            $.ajax({
                url: '/role/upsert',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (data) {
                    if (data.isSuccess) {
                        _msgalert.success(data.message);
                        _role.ReLoad();
                        $.magnificPopup.close();
                    } else {
                        _msgalert.error(data.message);
                    }
                },
                error: function (jqXHR) {
                },
                complete: function (jqXHR, status) {
                }
            });
        }
    },

    OnLoadUserData: function (id, callback) {
        $.ajax({
            url: "/role/RoleListUser",
            type: "post",
            data: { Id: id },
            success: function (result) {
                callback(result);
            }
        });
    },

    OnLoadMenuPermission: function (id, callback) {
        $.ajax({
            url: "/role/RolePermission",
            type: "post",
            data: { Id: id },
            success: function (result) {
                callback(result);
            }
        });
    },

    OnDeleteUser(roleid, userid) {
        let objParam = {
            roleId: parseInt(roleid),
            userId: parseInt(userid),
            type: 0
        };
        let title = 'Thông báo xác nhận';
        let description = 'Bạn có chắc chắn muốn gỡ người dùng ra khỏi nhóm quyền này?';
        _msgconfirm.openDialog(title, description, function () {
            $.ajax({
                url: "/role/UpdateUserRole",
                type: "post",
                data: objParam,
                success: function (result) {
                    if (result.isSuccess) {
                        _msgalert.success(result.message);
                        $.magnificPopup.close();
                        _role.OnLoadUserData(roleid, function (result) {
                            $('.grid-user-role[data-roleid=' + roleid + ']').html(result);
                        });
                    } else {
                        _msgalert.error(result.message);
                    }
                }
            });
        });
    },

    OnUpdateUserRole: function (roleid, userid, type) {
        let obj = {
            roleId: parseInt(roleid),
            userId: userid,
            type: type
        };
        console.log(obj);
        $.ajax({
            url: "/role/UpdateUserRole",
            type: "post",
            data: obj,
            success: function (result) {
            }
        });
    },

    OnDelete: function (id) {
        let title = 'Thông báo xác nhận';
        let description = 'Bạn có chắc chắn muốn xóa nhóm quyền này?';
        _msgconfirm.openDialog(title, description, function () {
            $.ajax({
                url: "/role/delete",
                type: "post",
                data: { roleId: id },
                success: function (result) {
                    if (result.isSuccess) {
                        _msgalert.success(result.message);
                        _role.ReLoad();
                        $.magnificPopup.close();
                    } else {
                        _msgalert.error(result.message);
                    }
                }
            });
        });
    },

    OnGetDetail: function (id, tabactive, callback) {
        $.ajax({
            url: "/role/GetDetail",
            type: "post",
            data: { Id: id, tabActive: tabactive },
            success: function (result) {
                callback(result);
            }
        });
    },

    OnUpdateRolePermission: function (paramData, updateType) {
        $.ajax({
            url: "/role/UpdateRolePermission",
            type: "post",
            data: { data: paramData, type: updateType },
            success: function (result) {
            }
        });
    }
};