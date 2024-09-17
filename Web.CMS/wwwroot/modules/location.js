//-- Thông tin các tỉnh thành phố, quận huyện, phường xã lấy từ sever:
var list_province_global;
var list_district_global;
var list_ward_global;
//-- Chứa ID - thông tin button người dùng chọn
var selected_value;
//-- Chứa thông tin tỉnh/thành phố được chọn.
var selected_p;
//-- Chứa thông tin quận / huyện được chọn.
var selected_d;
//-- Chứa thông tin phường/xã/đường được chọn.
var selected_w;

$(document).ready(function () {
    InitData();
    _location_load.LoadProvinces();
    _location_load.LoadDistricts();
    _location_load.LoadWards();
    BindEvent();

});

var AppendLocationRecord = function (location_type_lower_case, location_keyword, location_name, location_pos_in_list, location_level) {
    var element = '<li><a id="s-' + location_keyword + '-' + location_pos_in_list + '" href="#" class="' + location_type_lower_case + '-select-trigger">' + location_name + '</a><a id="e-' + location_keyword + '-' + location_pos_in_list + '" class="edit blue medium open-popup edit-trigger" href="#edit" onclick="_location.OpenFormEdit(\'e-' + location_keyword + '-' + location_pos_in_list + '\')">Sửa</a></li>';
    switch (location_level) {
        case 0:
            $("#province_scroll_list").append(element);
            break;
        case 1:
            $("#district_scroll_list").append(element);
            break;
        case 2:
            $("#ward_scroll_list").append(element);
            break;
        default:
            break;
    }

};

var InitData = function () {
    list_province_global = null;
    list_district_global = null;
    list_ward_global = null;
    selected_value = null;
    selected_p = null;
    selected_d = null;
    selected_w = null;
    $("#imgLoading_p").css({ 'display': '' });
    $("#imgLoading_d").css({ 'display': 'none' });
    $("#imgLoading_w").css({ 'display': 'none' });
    $("#province_scroll_list").css({ 'display': 'none' });
    $("#district_scroll_list").css({ 'display': 'none' });
    $("#ward_scroll_list").css({ 'display': 'none' });
    $("#sb_district").css({ 'display': 'none' });
    $("#sb_ward").css({ 'display': 'none' });

};


//Render thông tin tỉnh/ thành phố
var DisplayProvince = function () {
    $("#imgLoading_p").css({ 'display': '' });
    $("#province_scroll_list").css({ 'display': 'none' });
    $("#province_scroll_list").empty();

    list_province_global.forEach(display_list_p);
    function display_list_p(item, index) {
        AppendLocationRecord('province', 'p', item.name, index, 0);
    }
    $("#imgLoading_p").css({ 'display': 'none' });
    $("#province_scroll_list").css({ 'display': '' });
};

//Render thông tin quận/huyện
var DisplayDistrict = function () {
    $("#imgLoading_d").css({ 'display': '' });
    $("#district_scroll_list").css({ 'display': 'none' });
    $("#district_scroll_list").empty();

    list_district_global.forEach(display_list_d_by_pid);
    function display_list_d_by_pid(item, index) {
        if (item.provinceId == selected_p.provinceId) {
            AppendLocationRecord('district', 'd', item.name, index, 1);
        }
        $("#imgLoading_d").css({ 'display': 'none' });
        $("#district_scroll_list").css({ 'display': '' });
        $("#sb_district").css({ 'display': '' });
    }

};

//Render thông tin phường / xã / đường
var DisplayWard = function () {
    $("#imgLoading_w").css({ 'display': '' });
    $("#ward_scroll_list").css({ 'display': 'none' });
    $("#ward_scroll_list").empty();
    list_ward_global.forEach(display_list_w_by_did);
    function display_list_w_by_did(item, index) {
        if (item.districtId == selected_d.districtId) {
            AppendLocationRecord('ward', 'w', item.name, index, 2);
        }
        $("#imgLoading_w").css({ 'display': 'none' });
        $("#ward_scroll_list").css({ 'display': '' });
        $("#sb_ward").css({ 'display': '' });
    }
};

var BindEvent = function () {
    $('body').on('keyup', '.input_search', function (event) {
        var id = event.target.id;
        var a = id.split("-");
        if (a[0] == "search") {
            var value = $(this).val().toLowerCase();
            switch (a[1]) {
                case "p": {
                    $('#province_scroll_list > li').each(function () {
                        var currentLiText = $(this).text(),
                            showCurrentLi = currentLiText.toLowerCase().indexOf(value.toLowerCase()) !== -1;
                        $(this).toggle(showCurrentLi);
                    });
                } break;
                case "d": {
                    $('#district_scroll_list > li').each(function () {
                        var currentLiText = $(this).text(),
                            showCurrentLi = currentLiText.toLowerCase().indexOf(value.toLowerCase()) !== -1;
                        $(this).toggle(showCurrentLi);
                    });
                } break;
                case "w": {
                    $("#ward_scroll_list> li").each(function () {
                        var currentLiText = $(this).text(),
                            showCurrentLi = currentLiText.toLowerCase().indexOf(value.toLowerCase()) !== -1;
                        $(this).toggle(showCurrentLi);
                    });
                } break;
                default: break;
            }
        }
    });
    $('body').on('click', 'a.province-select-trigger', function (event) {
        var id = event.target.id.split("-");
        var position = id[id.length - 1];
        
        selected_p = list_province_global[parseInt(position)];
        selected_d = null;
        selected_w = null;
        $('#e_p_notselect').addClass("mfp-hide");
        $('#e_d_notselect').addClass("mfp-hide");
        $(".div_error").css({ 'display': 'none' });
        $('.province-select-trigger').removeClass("font-weight-bold");
        $('.province-select-trigger').removeClass("p-activated");
        $("#district_scroll_list").empty();
        $("#ward_scroll_list").empty();
        $("#sb_ward").css({ 'display': 'none' });

        DisplayDistrict();

        $(this).addClass("font-weight-bold");
        $(this).addClass("p-activated");
    });
    $('body').on('click', 'a.district-select-trigger', function (event) {
        var id = event.target.id.split("-");
        var position = id[id.length - 1];

        selected_d = list_district_global[parseInt(position)];
        selected_w = null;
        $('.district-select-trigger').removeClass("font-weight-bold");
        $('.district-select-trigger').removeClass("d-activated");
        $('#e_p_notselect').addClass("mfp-hide");
        $('#e_d_notselect').addClass("mfp-hide");
        $(".div_error").css({ 'display': 'none' });

        $(this).addClass("font-weight-bold");
        $(this).addClass("d-activated");

        DisplayWard();
    });
    $('body').on('click', 'a.ward-select-trigger', function (event) {
        var id = event.target.id.split("-");
        var position = id[id.length - 1];
        $('#e_p_notselect').addClass("mfp-hide");
        $('#e_d_notselect').addClass("mfp-hide");
        $(".div_error").css({ 'display': 'none' });

        selected_w = list_ward_global[position];
        $('.ward-select-trigger').removeClass("font-weight-bold");
        $('.ward-select-trigger').removeClass("w-activated");
        $(this).addClass("font-weight-bold");
        $(this).addClass("w-activated");

    });
};
var _location_load = {
     LoadProvinces : function () {
        $.ajax({
            url: 'location/LoadProvince',
            type: 'GET',
            success: function (result) {
                if (result.stt_code == 1) {
                    list_province_global = result.data;
                    $("#imgLoading_p").css({ 'display': 'none' });
                    DisplayProvince();
                }
                else {
                    $("#imgLoading_p").css({ 'display': 'none' });
                    $('#e_p_notselect').removeClass("mfp-hide");
                    $('#e_p_notselect').html('Không tìm thấy bất kỳ thông tin địa điểm nào, vui lòng liên hệ bộ phận kỹ thuật.');
                }
            },
        });
    },
    LoadDistricts: function () {
        $.ajax({
            url: 'location/LoadDistrict',
            type: 'GET',
            success: function (result) {
                if (result.stt_code == 1) {
                    list_district_global = result.data;
                    $("#imgLoading_d").css({ 'display': 'none' });
                }
                else {
                    $("#imgLoading_d").css({ 'display': 'none' });
                    $('#e_p_notselect').removeClass("mfp-hide");
                    $('#e_p_notselect').html('Không tìm thấy bất kỳ thông tin địa điểm nào, vui lòng liên hệ bộ phận kỹ thuật.');
                }
            },
        });
    },
    LoadWards: function () {
        $.ajax({
            url: 'location/LoadWard',
            type: 'GET',
            success: function (result) {
                if (result.stt_code == 1) {
                    list_ward_global = result.data;
                    $("#imgLoading_w").css({ 'display': 'none' });

                }
                else {
                    $("#imgLoading_w").css({ 'display': 'none' });
                    $('#e_w_notselect').removeClass("mfp-hide");
                    $('#e_w_notselect').html('Không tìm thấy bất kỳ thông tin địa điểm nào, vui lòng liên hệ bộ phận kỹ thuật.');
                }
            },
        });
    },
};
var _location = {
    OpenFormAdd: function (id) {
        $('#e_p_notselect').addClass("mfp-hide");
        $('#e_d_notselect').addClass("mfp-hide");
        var name = '';
        var new_location_type = ''
        if (id.split("-")[1] != null)
            selected_value = id.split("-")[1];
        else {
            id = "a-w";
            selected_value = null;
            selected_p = null;
            selected_d = null;
            selected_w = null;
        }
        switch (selected_value) {
            case "p": {
                new_location_type = "Tỉnh / Thành phố";
            } break;
            case "d": {
                if (selected_p == null) {
                    $(".div_error").css({ 'display': '' });
                    $('#e_p_notselect').removeClass("mfp-hide");
                    return;
                }
                else {
                    name = ' cho ' + selected_p.type + ' ' + selected_p.name;
                    new_location_type = 'Quận / Huyện';
                }
            } break;
            case "w": {
                if (selected_p == null) {
                    $(".div_error").css({ 'display': '' });
                    $('#e_p_notselect').removeClass("mfp-hide");
                    return;

                }
                else if (selected_d == null) {
                    $(".div_error").css({ 'display': '' });
                    $('#e_d_notselect').removeClass("mfp-hide");
                    return;
                }
                else {
                    name = ' cho ' + selected_d.type + ' ' + selected_d.name;
                    new_location_type = 'Xã / Phường / Đường';

                };
            } break;
            default: {

            } break;
        }
        let title = 'Thêm ' + new_location_type + ' mới' + name;
        let url = '/location/AddLocation';
        let param = { id: id };
        _magnific.OpenSmallPopupWithHeader(title, url, param);
    },
    //Mở magnific popup thay đổi địa điểm
    OpenFormEdit: function (location_id) {
        var arr = location_id.split("-");
        selected_value = location_id;
        var edit_name = 'Địa điểm';
        $('#btnEditLocation').removeAttr("disabled");
        if (arr[0] != "e" || arr == null || arr.length != 3 || parseInt(arr[2]) == null)
            return;
        else {
            var location_json = "";
            switch (arr[1]) {
                case "p": {
                    location_json = JSON.stringify(list_province_global[parseInt(arr[2])]);
                    selected_p = list_province_global[parseInt(arr[2])];
                    edit_name = selected_p.type + ' ' + selected_p.name;
                } break;
                case "d": {
                    location_json = JSON.stringify(list_district_global[parseInt(arr[2])]);
                    selected_d = list_district_global[parseInt(arr[2])];
                    edit_name = selected_d.type + ' ' + selected_d.name;
                } break;
                case "w": {
                    location_json = JSON.stringify(list_ward_global[parseInt(arr[2])]);
                    selected_w = list_ward_global[parseInt(arr[2])];
                    edit_name = selected_w.type + ' ' + selected_w.name;
                } break;
                default: break;
            }
            let title = 'Chỉnh sửa ' + edit_name;
            let url = '/location/EditLocation';
            let param = {
                location_id: location_id,
                location_json: location_json
            };
            _magnific.OpenSmallPopupWithHeader(title, url, param);

        }


    },
    //Trigger khi thay đổi loại địa điểm trong form thêm địa chỉ.
    AddPopupChangeLocationType: function () {
        $('#l_lbl').html($('#selectLocationType :selected').text());
        $('#e_fa').html("Bạn chưa nhập " + $('#selectLocationType :selected').text());
        $('#i_a_name').attr('placeholder', "Nhập tên " + $('#selectLocationType :selected').text() + " mới.");
        $('#e_fa').addClass("mfp-hide");
    },
    //Trigger khi thay đổi loại địa điểm trong form sửa địa chỉ.
    EditPopupChangeLocationType: function () {
        $('#l_lbl').html($('#selectLocationType :selected').text());
        $('#e_fe').html("Bạn chưa nhập " + $('#selectLocationType :selected').text());
        $('#i_e_name').attr('placeholder', "Nhập tên " + $('#selectLocationType :selected').text() + " mới.");
        $('#e_fe').addClass("mfp-hide");
    },
    //Confirm thêm địa chỉ mới
    OnAddLocation: function () {
        $('#onapply_location').css('display', '');
        var type = $('#selectLocationType :selected').text();
        var name = $('#i_a_name').val();
        var status = $('.form-check-input:checked').val();
        if (parseInt(status) <= -1 || parseInt(status) >= 2)
            status = 0;
        else status = parseInt($('.form-check-input:checked').val());
        switch (selected_value) {
            case "p": {
                if (type.length === 0) {
                    $('#e_fa').html("Vui lòng chọn loại địa chỉ hợp lệ.");
                    $('#e_formadd').removeClass("mfp-hide");
                    return;
                }
                else if (name.length === 0) {
                    $('#e_fa').html("Tên địa chỉ mới không được bỏ trống.");
                    $('#e_fa').removeClass("mfp-hide");
                    return;
                }
                else {
                    var provinceid = parseInt(list_province_global[list_province_global.length - 1].provinceId) + 1;
                    var location_data_json = {
                        "ProvinceId": provinceid,
                        "Name": name,
                        "Type": type,
                        "Status": status
                    };
                    var json_str = JSON.stringify(location_data_json);
                    $('#btnAddLocation').attr("disabled", true);

                    $.ajax({
                        url: 'location/Add',
                        type: 'POST',
                        data: {
                            location_type: 'p',
                            location_data_json: json_str,
                        },
                        success: function (result) {
                            if (result.stt_code == 1) {
                                list_province_global.push(result.data);
                                $.magnificPopup.close();
                                DisplayProvince();
                                _msgalert.success(result.msg);

                            }
                            else {
                                $("#e_fa").html(result.msg);
                                $("#e_fa").removeClass("mfp-hide");
                                $('btnAddLocation').attr("disabled", true);
                                $('#btnAddLocation').removeAttr("disabled");

                            }
                        },
                        error: function (jqXHR) {
                        },
                        complete: function (jqXHR, status) {
                        }
                    });
                }
            } break;
            case "d": {

                if (type.length === 0) {
                    $('#e_fa').html("Vui lòng chọn loại địa chỉ hợp lệ.");
                    $('#e_fa').removeClass("mfp-hide");
                    return;
                }
                else if (name.length === 0) {
                    $('#e_fa').html("Tên địa chỉ mới không được bỏ trống.");
                    $('#e_fa').removeClass("mfp-hide");
                    return;
                }
                var districtID = parseInt(list_district_global[list_district_global.length - 1].districtId) + 1;
                var location_data_json = {
                    "DistrictId": districtID,
                    "Name": name,
                    "Type": type,
                    "Location": "",
                    "ProvinceId": selected_p.provinceId,
                    "Status": status
                };
                var json_str = JSON.stringify(location_data_json);
                $('#btnAddLocation').attr("disabled", true);
                var reqObject = {
                    location_type: 'd',
                    location_data_json: json_str,
                };
                $.ajax({
                    url: 'location/Add',
                    type: 'POST',
                    data: reqObject,
                    success: function (result) {
                        if (result.stt_code == 1) {
                            list_district_global.push(result.data);
                            $.magnificPopup.close();
                            DisplayDistrict();
                            _msgalert.success(result.msg);

                        }
                        else {
                            $("#e_fa").html(result.msg);
                            $("#e_fa").removeClass("mfp-hide");
                            $('#btnAddLocation').removeAttr("disabled");

                        }
                    }
                });
            } break;
            case "w": {
                if (type.length === 0) {
                    $('#e_fa').html("Vui lòng chọn loại địa chỉ hợp lệ.");
                    $('#e_fa').removeClass("mfp-hide");
                    return;
                }
                else if (name.length === 0) {
                    $('#e_fa').html("Tên địa chỉ mới không được bỏ trống.");
                    $('#e_fa').removeClass("mfp-hide");
                    return;
                }
                var wardID = parseInt(list_ward_global[list_ward_global.length - 1].wardId) + 1;
                var location_data_json = {
                    "WardId": wardID,
                    "Name": name,
                    "Type": type,
                    "Location": "",
                    "DistrictId": selected_ddistrictId,
                    "Status": status
                };
                var json_str = JSON.stringify(location_data_json);
                $('#btnAddLocation').attr("disabled", true);

                $.ajax({
                    url: 'location/Add',
                    type: 'POST',
                    data: {
                        location_type: 'w',
                        location_data_json: json_str,
                    },
                    success: function (result) {
                        if (result.stt_code == 1) {
                            list_ward_global.push(result.data);
                            $.magnificPopup.close();
                            DisplayWard();
                            _msgalert.success(result.msg);

                        }
                        else {
                            $("#e_fa").html(result.msg);
                            $("#e_fa").removeClass("mfp-hide");
                            $('#btnAddLocation').removeAttr("disabled");

                        }
                    }
                });
            } break;
            default: break;
        }
        $('#onapply_location').css('display', 'none');
    },
    //Confirm thay đổi địa chỉ
    OnEditLocation: function () {
        $('#onapply_location').css('display', '');
        var a = selected_value.split("-");
        var l_type = a[1];
        var l_pos = a[2];
        var type = $('#selectLocationType :selected').text();
        var name = $('#i_e_name').val();
        var status = $('.form-check-input:checked').val();
        if (parseInt(status) <= -1 || parseInt(status) >= 2)
            status = 0;
        else status = parseInt($('.form-check-input:checked').val());
        switch (l_type) {
            case "p": {
                var provinceid = list_province_global[l_pos].provinceId;
                var id = list_province_global[l_pos].id;
                var location_data_json = {
                    "Id": id,
                    "ProvinceId": provinceid,
                    "Name": name,
                    "Type": type,
                    "Status": status
                };
                var json_str = JSON.stringify(location_data_json);
                $('#btnEditLocation').attr("disabled", true);
                $.ajax({
                    url: 'location/Update',
                    type: 'POST',
                    data: {
                        location_type: 'p',
                        location_data_json: json_str,
                    },
                    success: function (result) {
                        if (result.stt_code == 1) {
                            list_province_global[l_pos] = result.data;
                            $.magnificPopup.close();
                            DisplayProvince();
                            _msgalert.success(result.msg);

                        }
                        else {
                            $("#e_fe").html(result.msg);
                            $("#e_fe").removeClass("mfp-hide");
                            $('#btnEditLocation').removeAttr("disabled");
                        }
                    }
                });

            } break;
            case "d": {
                var districtId = list_district_global[l_pos].districtId;
                var id = list_district_global[l_pos].id;

                var location_data_json = {
                    "Id": id,
                    "DistrictId": districtId,
                    "Name": name,
                    "Type": type,
                    "Location": "",
                    "ProvinceId": selected_p.provinceId,
                    "Status": status
                };
                var json_str = JSON.stringify(location_data_json);
                $('#btnEditLocation').attr("disabled", true);
                $.ajax({
                    url: 'location/Update',
                    type: 'POST',
                    data: {
                        location_type: 'd',
                        location_data_json: json_str,
                    },
                    success: function (result) {
                        if (result.stt_code == 1) {
                            list_district_global[l_pos] = result.data;
                            $.magnificPopup.close();
                            DisplayDistrict();
                            _msgalert.success(result.msg);

                        }
                        else {
                            $("#e_fe").html(result.msg);
                            $("#e_fe").removeClass("mfp-hide");
                            $('#btnEditLocation').removeAttr("disabled");
                        }
                    }
                });

            } break;
            case "w": {
                var id = list_ward_global[l_pos].id;

                var wardID = list_ward_global[l_pos].wardId;
                var location_data_json = {
                    "Id": id,
                    "WardId": wardID,
                    "Name": name,
                    "Type": type,
                    "Location": "",
                    "DistrictId": selected_d.districtId,
                    "Status": status
                };
                var json_str = JSON.stringify(location_data_json);
                $('#btnEditLocation').attr("disabled", true);
                $.ajax({
                    url: 'location/Update',
                    type: 'POST',
                    data: {
                        location_type: 'w',
                        location_data_json: json_str,
                    },
                    success: function (result) {
                        if (result.stt_code == 1) {
                            list_ward_global[l_pos] = result.data;
                            $.magnificPopup.close();
                            DisplayWard();
                            _msgalert.success(result.msg);

                        }
                        else {
                            $("#e_fe").html(result.msg);
                            $("#e_fe").removeClass("mfp-hide");
                            $('#btnEditLocation').removeAttr("disabled");
                        }
                    }
                });

            } break;
            default: break;
        };
        $('#onapply_location').css('display', 'none');
    },
    Sync: function () {
        $.ajax({
            url: 'location/Sync',
            type: 'POST',
            data: {
               
            },
            success: function (result) {
                if (result.is_success) {
                    _msgalert.success('Success');

                }
                else {
                    _msgalert.error('Failed');

                }
            }
        });
    }
};



