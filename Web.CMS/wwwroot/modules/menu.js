
var _menu = {

    Init: function () {
        this.modal_element = $('#global_modal_popup');
        this.OnSearch();
		
    },

    GetSearchParam: function () {
        let objSearch = {
            name: $('#ip_search_name').val(),
            link: $('#ip_search_link').val()
        }
        return objSearch;
    },

    GetFormData: function ($form) {
        var unindexed_array = $form.serializeArray();
        var indexed_array = {};
        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });
        return indexed_array;
    },

    Search: function (input) {
        window.scrollTo(0, 0);
        _ajax_caller.post('/menu/search', input, function (result) {
            $('#grid_data').html(result);
            let search_name = input.name;
            let search_link = input.link;
            if ((search_name && search_name != null && search_name != null) ||
                (search_link && search_link != null && search_link != null)) {
                $('#grid_data .menu_name').each(function () {
                    let seft = $(this);
                    let name = seft.text();
                    let seft_link = seft.closest('tr').find('.menu_link');
                    let link = seft_link.text();

                    if (name.toLowerCase().includes(search_name.toLowerCase())) {
                        seft.html(`<strong>${name}</strong>`)
                    }

                    if (link != null && link != "") {
                        if (link.toLowerCase().includes(search_name.toLowerCase())) {
                            seft_link.html(`<strong>${link}</strong>`)
                        }
                    }
                });
            }
        });
    },

    OnSearch: function () {
        let objSearch = this.GetSearchParam();
        this.SearchParam = objSearch;
        this.Search(objSearch);
    },

    ReLoad: function () {
        this.Search(this.SearchParam);
    },

    OnChangeFullNameSearch: function (value) {
        var searchobj = this.SearchParam;
        searchobj.name = value;
        this.SearchParam = searchobj;
        this.Search(searchobj);
    },

    ShowAddOrUpdate: function (id, parent_id = 0) {
        _global_function.AddLoading()

        let title = `${id > 0 ? "Cập nhật" : "Thêm mới"} menu`;
        let url = '/menu/AddOrUpdate';
        //_menu.modal_element.find('.modal-title').html(title);
        //_menu.modal_element.find('.modal-dialog').css('max-width', '680px');
        //let param = { id: id, parent_id: parent_id }
        //_magnific.OpenSmallPopup(title, url, param);
        //_ajax_caller.get(url, { id: id, parent_id: parent_id }, function (result) {

        //    _menu.modal_element.find('.modal-body').html(result);
        //    _menu.modal_element.show();
        //});
        _ajax_caller.get(url, { id: id, parent_id: parent_id }, function (result) {
            $('body').append(result);
            _global_function.RemoveLoading()
            $('#menu-add-or-update').addClass('show')

        });
    },

    ShowPermision: function (id) {
        let title = `Cập nhật các quyền thuộc menu`;
        let url = '/menu/Permission';
        _global_function.AddLoading()

        //let param = { id: id }
        //_magnific.OpenSmallPopup(title, url, param);
        $('#flybooking-service').addClass('show')

        //_menu.modal_element.find('.modal-title').html(title);
        //_menu.modal_element.find('.modal-dialog').css('max-width', '680px');
        //_ajax_caller.get(url, { id: id }, function (result) {
        //    _menu.modal_element.find('.modal-body').html(result);
        //    _menu.modal_element.show();
        //});
        _ajax_caller.get(url, { id: id }, function (result) {
            $('body').append(result);
            _global_function.RemoveLoading()
            $('#menu-add-or-update').addClass('show')

        });
    },

    OnSave: function () {
        let Form = $('#form_menu');
        Form.validate({
            rules: {
                DepartmentCode: "required",
                Name: "required"
            },
            messages: {
                 DepartmentCode: "Vui lòng nhập mã phòng ban",
                Name: "Vui lòng nhập tên menu"
            }
        });

        if (!Form.valid()) { return; }


        let formData = this.GetFormData(Form);

        let url = "/menu/Summit";
        _ajax_caller.post(url, { request: formData }, function (result) {
            if (result.isSuccess) {
                _msgalert.success(result.message);
                _menu.modal_element.modal('hide');
                _menu.ReLoad();
            } else {
                _msgalert.error(result.message);
            }
        });
    },

    OnSavePermission: function () {

        let menu_id = $('#ip_menu_id').val();
        let permissions = [];
        $('#block_permission_options .ckb__permission').each(function () {
            let seft = $(this);
            if (seft.is(':checked')) {
                permissions.push(seft.val());
            }
        });

        var obj = {
            menu_id: menu_id,
            permission_ids: permissions
        }

        console.log(obj);

        let url = "/menu/SavePermission";
        _ajax_caller.post(url, { model: obj }, function (result) {
            if (result.isSuccess) {
                _msgalert.success(result.message);
                _menu.modal_element.modal('hide');
                _menu.ReLoad();
            } else {
                _msgalert.error(result.message);z
            }
        });
    },

    OnChangeStatus: function (id, status) {
        let title = `Xác nhận ${status == 0 ? "hiển thị" : "ẩn"} menu`;
        let description = `${status == 1 ? "Khi ẩn Menu cha, các menu con cũng sẽ bị ẩn. " : ""}Bạn xác nhận muốn ${status == 0 ? "hiển thị" : "ẩn"} menu này?`;

        _msgconfirm.openDialog(title, description, function () {
            let url = "/menu/ChangeStatus";
            _ajax_caller.get(url, { Id: id, Status: status }, function (result) {
                if (result.isSuccess) {
                    _msgalert.success(result.message);
                    _menu.ReLoad();
                } else {
                    _msgalert.error(result.message);
                }
            });
        });
    },
    Close: function () {
        _global_function.AddLoading()

        $('#menu-add-or-update').removeClass('show')
        setTimeout(function () {
            $('#menu-add-or-update').remove();
            _global_function.RemoveLoading()

        }, 300);
    }
}

var _changeInterval = null;

$("#ip_search_name").keyup(function (e) {
    if (e.which === 13) {
        _menu.OnChangeFullNameSearch(e.target.value);
    } else {
        clearInterval(_changeInterval);
        _changeInterval = setInterval(function () {
            _menu.OnChangeFullNameSearch(e.target.value);
            clearInterval(_changeInterval);
        }, 1000);
    }
});

$(document).ready(function () {
    $('input').attr('autocomplete', 'off');
    _menu.Init();
});

