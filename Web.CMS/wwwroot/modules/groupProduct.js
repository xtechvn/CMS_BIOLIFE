var listGroupProduct = [], listCampaign = [], listParent = [];

var utm_medium = '', utm_source = '', utm_campaign = '', utm_campaign_alias = '';

var listChildren = [], listGrandChildren = [], listLink = [], listLinkWrong = [], listLable = [];

var fromDate, toDate, campaignId = 0, cnt = 0, utm_sourceId = 0, utm_mediumId = 0, utm_campaignId = 0;

var _parrentId, _parrentName, _childrenId, _childrenName, _grandChilrenId, _grandChilrenName;

var listUtm_Source = [], listUtm_Medium = [], listUtm_Campaign = [];

function dateToString(date) {
    let month = date.getMonth() + 1;
    let day = String(date.getDate()).padStart(2, '0');
    let year = date.getFullYear();
    return day + '/' + month + '/' + year;
}

function dateStrToString(dateStr) {
    var spilt = dateStr.split('-');
    return spilt[2] + '/' + spilt[1] + '/' + spilt[0];
}

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

$(document).ready(function () {
    $('.datepicker-input').Zebra_DatePicker({
        format: 'd/m/Y'
    }).removeAttr('readonly');

    fromDate = dateToString(new Date());
    toDate = dateToString(new Date().addDays(7));
    $('#selectedMenu').hide();
    $('#erroradd').hide();
    $('#errorupdate').hide();
    $('#showLinkWrong').hide();
    _menu.InitGroupProduct();
    _menu.GetAllCampaign();
    setTimeout(function () {
        _menu.GetAllUtmMedium();
    }, 300);
    setTimeout(function () {
        _menu.GetAllUtmSource();
    }, 500);
    setTimeout(function () {
        _menu.GetAllUtmCampaign();
    }, 700);
    $("#fromDate").Zebra_DatePicker({
        onSelect: function () {
            $(this).change();
            fromDate = dateStrToString($(this).val());
            $('#fromDate').val(fromDate)
        }
    }).removeAttr('readonly');
    $("#toDate").Zebra_DatePicker({
        onSelect: function () {
            $(this).change();
            toDate = dateStrToString($(this).val());
            $('#toDate').val(toDate)
        }
    }).removeAttr('readonly');

    $("#selectCampaign").change(function () {
        campaignId = parseInt($(this).children(":selected").attr("value"));
        var campaignName = "";
        for (var i = 0; i < listCampaign.length; i++) {
            if (listCampaign[i].id == campaignId) {
                campaignName = listCampaign[i].campaignName
                utm_campaign = listCampaign[i].campaignName
            }
        }
        utm_campaign = $('#selectCampaign').val()
    });
    $("#selectUtmSource").change(function () {
        utm_sourceId = parseInt($(this).children(":selected").attr("value"));
        for (var i = 0; i < listUtm_Source.length; i++) {
            if (utm_sourceId == listUtm_Source[i].id) {
                $('#utm_sour').html(listUtm_Source[i].description);
                utm_source = listUtm_Source[i].description
            }
        }
    });
    $("#selectUtmCampaign").change(function () {
        utm_campaignId = parseInt($(this).children(":selected").attr("value"));
        for (var i = 0; i < listUtm_Campaign.length; i++) {
            if (utm_campaignId == listUtm_Campaign[i].id) {
                $('#utm_cp').html(listUtm_Campaign[i].description);
                utm_campaign = listUtm_Campaign[i].description
            }
        }
    });
    $("#selectUtmMedium").change(function () {
        utm_mediumId = parseInt($(this).children(":selected").attr("value"));
        for (var i = 0; i < listUtm_Medium.length; i++) {
            if (utm_mediumId == listUtm_Medium[i].id) {
                $('#utm_medi').html(listUtm_Medium[i].description);
                utm_medium = listUtm_Medium[i].description
            }
        }
    });
});

$(".tab-link").click(function () {
    $(".tab-link").removeClass("active");
    if (!$(this).hasClass("active")) {
        $(this).addClass("active");
    } else {
        $(this).removeClass("active");
    }
    var tabid = $(this).data("id");
    $(".item-tab-content").css("display", "none");
    $(".item-tab-content[data-id='" + tabid + "']").fadeIn();
});

function getChildrenMenu(item) {
    listChildren = [];
    $('#selectedMenu').show();
    $("#childrenMenu").empty();
    $("#grandChildrenMenu").empty();
    _grandChilrenId = "";
    var parentId = parseInt(item.id);
    _parrentId = parentId
    //lay thong tin ban ghi cha
    var parentInfo = listGroupProduct.find(n => n.id == parentId);
    if (parentInfo != null) {
        $('#parrent').html(parentInfo.name + '<i class="fa fa-angle-right"></i>');
        $('#children').html('');
        $('#grandchildren').html('');
        _grandChilrenId = "";
    }

    item.style.color = "red"
    for (var i = 0; i < listParent.length; i++) {
        if (listParent[i].id != parentId) {
            $('#' + listParent[i].id).attr('style', 'color:black');
        }
    }
    for (var i = 0; i < listGroupProduct.length; i++) {
        if (listGroupProduct[i].parentId > -1 && listGroupProduct[i].parentId == parentId) {
            listChildren.push(listGroupProduct[i]);
        }
    }
    for (var i = 0; i < listChildren.length; i++) {
        $("#childrenMenu").append(
            "<li id='" + listChildren[i].id + "' onclick='getGrandChildrenMenu(this)'><a >" + listChildren[i].name + " <i class='fa fa-angle-right'></i></a></li>"
        );
    }
}

function getGrandChildrenMenu(item) {
    $("#grandChildrenMenu").empty();
    listGrandChildren = [];
    _grandChilrenId = "";
    item.style.color = "red"
    var parentId = parseInt(item.id);
    for (var i = 0; i < listChildren.length; i++) {
        if (listChildren[i].id != parentId) {
            $('#' + listChildren[i].id).attr('style', 'color:black');
        }
    }
    _childrenId = parentId;
    //lay thong tin ban ghi cha
    var parentInfo = listGroupProduct.find(n => n.id == parentId);
    if (parentInfo != null) {
        $('#children').html(parentInfo.name + '<i class="fa fa-angle-right"></i>');
        $('#grandchildren').html('');
    }

    for (var i = 0; i < listGroupProduct.length; i++) {
        if (listGroupProduct[i].parentId > -1 && listGroupProduct[i].parentId == parentId) {
            listGrandChildren.push(listGroupProduct[i]);
        }
    }
    for (var i = 0; i < listGrandChildren.length; i++) {
        $("#grandChildrenMenu").append(
            "<li id='" + listGrandChildren[i].id + "' onclick='getIdGrandChildren(this)'><a >" + listGrandChildren[i].name + "</li>"
        );
    }
}

function getIdGrandChildren(item) {
    var id = parseInt(item.id);
    item.style.color = "red"
    for (var i = 0; i < listGrandChildren.length; i++) {
        if (listGrandChildren[i].id != id) {
            $('#' + listGrandChildren[i].id).attr('style', 'color:black');
        }
    }
    _grandChilrenId = id
    var groupInfo = listGroupProduct.find(n => n.id == id);
    if (groupInfo != null)
        $('#grandchildren').html(groupInfo.name);
}

function searchMenu() {
    var value = $('#searchInput').val()
    var listParentTemp = []
    listChildren = []
    listGrandChildren = []
    if (value == "" || value == null) {
        listParent = []
        $("#parentMenu").empty();
        for (var i = 0; i < listGroupProduct.length; i++) {
            if (listGroupProduct[i].parentId == -1) {
                listParent.push(listGroupProduct[i]);
            }
        }
        for (var i = 0; i < listParent.length; i++) {
            $("#parentMenu").append(
                "<li id='" + listParent[i].id + "' onclick='getChildrenMenu(this)'><a>" + listParent[i].name + " <i class='fa fa-angle-right'></i></a></li>"
            );
        }
        return;
    }
    for (var i = 0; i < listGroupProduct.length; i++) {
        if (listGroupProduct[i].name.toLowerCase().indexOf(value) != -1) {
            //neu la menu cha
            if (listGroupProduct[i].parentId == -1) {
                var info = listParentTemp.find(n => n.id == listGroupProduct.id)
                if (info == null) {
                    listParentTemp.push(listGroupProduct[i]);
                }
            }
            else {
                //kiem tra menu con - de lay ra menu cha
                var parentInfo = listGroupProduct.find(n => n.id == listGroupProduct.parentId)
                if (parentInfo != null) {
                    //neu la menu cha cua menu con vua tim kiem
                    if (parentInfo.parentId == -1) {
                        var info = listParentTemp.find(n => n.id == listGroupProduct.id)
                        if (info == null) {
                            listParentTemp.push(listGroupProduct[i]);
                        }
                    }
                    else {
                        var parentInfo = listGroupProduct.find(n => n.id == listGroupProduct.parentId)
                        if (parentInfo != null && parentInfo.parentId == -1) {
                            var info = listParentTemp.find(n => n.id == listGroupProduct.id)
                            if (info == null) {
                                listParentTemp.push(listGroupProduct[i]);
                            }
                        }
                    }
                }
            }
        }
    }
    if (value != "" && value != null) {
        listParent = listParentTemp
        listChildren = []
        $("#parentMenu").empty();
        for (var i = 0; i < listParent.length; i++) {
            $("#parentMenu").append(
                "<li id='" + listParent[i].id + "' onclick='getChildrenMenu(this)'><a>" + listParent[i].name + " <i class='fa fa-angle-right'></i></a></li>"
            );
        }
    }
}

function deleteItem(item) {

    var id = parseInt(item.id);
    if (listLink != null && listLink.length > 0) {
        listLink.splice(id, 1);
    }
    $("#tbLink tbody").empty();
    $('#totallink').html(listLink.length);
    for (var i = 0; i < listLink.length; i++) {
        var labelInfo = listLable.find(n => listLink[i].toLowerCase().indexOf(n.domain.toLowerCase()) >= 0);
        $("#tbLink tbody").append(
            "<tr>" +
            "<td id='" + i + "'>" + listLink[i] + "</td>" +
            "<td>" + labelInfo.storeName.charAt(0).toUpperCase() + labelInfo.storeName.slice(1) + "</td>" +
            "<td> <button type='submit' id='" + i + "' onclick='deleteItem(this)' class='btn btn -default white'>Xóa</button></td>" +
            "</tr>"
        );
    }
}

function change_alias(alias) {
    var str = alias;
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, "");
    str = str.replace(/ + /g, "");
    str = str.trim();
    return str;
}

