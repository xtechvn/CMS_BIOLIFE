
var _department = {
    Init: function () {
        this.modal_element = $('#global_modal_popup');
        this.OnSearch();
    },

    GetSearchParam: function () {
        let objSearch = {
            name: $('#ip_search_fullname').val()
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
        _ajax_caller.post('/department/search', input, function (result) {
            $('#grid_data').html(result);
            let search_name = input.name;
            if (search_name && search_name != null && search_name != null) {
                $('#grid_data .department_name').each(function () {
                    var seft = $(this);
                    var text = seft.text();

                    if (text.toLowerCase().includes(search_name.toLowerCase())) {
                        seft.html(`<strong>${text}</strong>`)
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
        let title = `${id > 0 ? "Cập nhật" : "Thêm mới"} phòng ban`;
        let url = '/Department/AddOrUpdate';
        _department.modal_element.find('.modal-title').html(title);
        _department.modal_element.find('.modal-dialog').css('max-width', '680px');
        _ajax_caller.get(url, { id: id, parent_id: parent_id }, function (result) {
            _department.modal_element.find('.modal-body').html(result);
            _department.modal_element.modal('show');
            $('#branch-code').select2({
                minimumResultsForSearch: Infinity
            });
        });
    },

    OnSave: function () {
        let Form = $('#form_department');
        Form.validate({
            rules: {
                //DepartmentCode: "required",
                DepartmentName: "required"
            },
            messages: {
                // DepartmentCode: "Vui lòng nhập mã phòng ban",
                DepartmentName: "Vui lòng nhập tên phòng ban"
            }
        });

        if (!Form.valid()) { return; }


        let formData = this.GetFormData(Form);
        console.log(formData);
        formData['Branch'] = $('#branch-code').find(':selected').val();

        let url = "/Department/AddOrUpdate";
        _ajax_caller.post(url, { model: formData }, function (result) {
            if (result.isSuccess) {
                _msgalert.success(result.message);
                _department.modal_element.modal('hide');
                _department.ReLoad();
            } else {
                _msgalert.error(result.message);
            }
        });
    },

    OnDelete: function (id) {
        let title = 'Xác nhận xóa phòng ban';
        let description = 'Bạn xác nhận muốn xóa phòng ban này?';
        _msgconfirm.openDialog(title, description, function () {
            let url = "/Department/Delete";
            _ajax_caller.get(url, { Id: id }, function (result) {
                if (result.isSuccess) {
                    _msgalert.success(result.message);
                    _department.ReLoad();
                } else {
                    _msgalert.error(result.message);
                }
            });
        });
    }
}

var _changeInterval = null;
$("#ip_search_fullname").keyup(function (e) {
    if (e.which === 13) {
        _department.OnChangeFullNameSearch(e.target.value);
    } else {
        clearInterval(_changeInterval);
        _changeInterval = setInterval(function () {
            _department.OnChangeFullNameSearch(e.target.value);
            clearInterval(_changeInterval);
        }, 1000);
    }
});

$(document).ready(function () {
    $('input').attr('autocomplete', 'off');
    _department.Init();
});
