var _product_function = {
    POST: function (url, model, callback) {
        $.ajax({
            url: url,
            type: "POST",
            data: model,
            success: function (result) {
                callback(result)
            },
            error: function (err) {
                console.log(err)
            }
        });
    },
    POSTSynchorus: function (url, model) {
        var data = undefined
        $.ajax({
            url: url,
            type: "POST",
            data: model,
            success: function (result) {
                data = result;
            },
            error: function (err) {
                console.log(err)
            },
            async: false
        });
        return data
    },
    POSTPromise: function (url, data) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: 'post',
                url: url,
                data: { request: data },
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
    },
    POSTFileSynchorus: function (url, model) {
        var data = undefined
        $.ajax({
            url: url,
            type: "POST",
            data: model,
            processData: false,  // Prevent jQuery from processing the data
            contentType: false,  // Prevent jQuery from setting contentType
            success: function (result) {
                data = result;
            },
            error: function (err) {
                console.log(err)
            },
            async: false
        });
        return data
    },
    Comma: function (number) { //function to add commas to textboxes
        number = ('' + number).replace(/[^0-9.,]+/g, '');
        number += '';
        number = number.replaceAll(',', '');
        x = number.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1))
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        return x1 + x2;
    },
    CorrectImage: function (image) {
        var img_src = image
        if (!img_src.includes(_product_constants.VALUES.StaticDomain)
            && !img_src.includes("data:image")
            && !img_src.includes("http")
            && !img_src.includes("base64,"))
            img_src = _product_constants.VALUES.StaticDomain + image
        else if  (!img_src.includes(_product_constants.VALUES.StaticDomain)
            && !img_src.includes("data:video")
            && !img_src.includes("http")
            && !img_src.includes("base64,"))
            img_src = _product_constants.VALUES.StaticDomain + image
        return img_src
    },
    CheckIfImageVideoIsLocal: function (data) {
        if (data != undefined && (data.includes("data:image") || data.includes("data:video") || data.includes("base64,"))) {
            return true
        }
        else {
            return false
        }
    }
    
}
var _product_constants = {
    VALUES: {
        ProductDetail_Max_Image: 9,
        ProductDetail_Max_Avt: 1,
        DefaultSpecificationValue: [
            { id: 1, name: 'Thương hiệu', type: 3 },
            { id: 5, name: 'Chất liệu', type: 3 },
            { id: 2, name: 'Độ tuổi khuyến nghị', type: 3 },
            { id: 6, name: 'Ngày sản xuất', type: 2 },
            { id: 3, name: 'Tên tổ chức chịu trách nhiệm sản xuất', type: 3 },
            { id: 7, name: 'Địa chỉ tổ chức chịu trách nghiệm sản xuất', type: 3 },
            { id: 4, name: 'Sản phẩm đặt theo yêu cầu', type: 3 },
        ],
        StaticDomain: `https://static-image.adavigo.com`,
        ImageExtension: ['jpeg', 'jpg', 'png', 'bmp'],
        VideoExtension: ['mp4'],

    },
    HTML: {
        Product: `
            <tr data-id="{id}">
                            <td style="width: 0;">
                                <label class="check-list mb-3">
                                    <input type="checkbox">
                                    <span class="checkmark"></span>
                                </label>
                            </td>
                            <td style="width: 100px;">
                                <div class="item-order">
                                    <div class="img">
                                        <img src="{avatar}" alt="">
                                    </div>
                                    <div class="info">
                                        <h3 class="name-product">
                                           {name}
                                        </h3>
                                        <div class="cat" style="display:none;">Phân loại hàng: {attribute}</div>

                                    </div>
                                </div>
                            </td>
                            <td class="text-center">{order_count}</td>
                            <td class="text-center">{amount}</td>
                            <td class="text-center">{stock}</td>
                            <td class="text-center">
                                <a href="javascript:;" class="product-edit">Cập nhật</a><br />
                                <a href="javascript:;" class="product-viewmore onclick"  >Xem thêm</a><br />
                               
                                <div class="form-down grid-slide-{id}" style="display: none;" >
                                    <div >
                                        <a href="javascript:;" class="product-copy-sp">Sao chép</a><br />
                                        <a href="javascript:;" class="product-remove-sp">Ẩn</a><br />
                                        <a href="javascript:;" class="product-open-sp">Hiển thị</a><br />
                                        <a href="javascript:;" class="product-remove-sp2">Xóa</a><br />
                                    </div>

                                </div>
                            </td>
                        </tr>
        `,
        SubProduct: `  <tr class="sub-product"data-id="{id}"data-main-id="{main_id}" style="{display}">
                            <td style="width: 0;">
                            </td>
                            <td style="width: 100px;">
                                <div class="item-order">
                                    <div class="img">
                                        <img src="{avatar}" alt="">
                                    </div>
                                    <div class="info">
                                        <h3 class="name-product"> {attribute}</h3>
                                        <div class="cat">{attribute_detail}</div>
                                        <div class="cat">{sku}</div>
                                    </div>
                                </div>
                            </td>
                           <td class="text-center">{order_count}</td>
                            <td class="text-center">{amount}</td>
                            <td class="text-center">{stock}</td>
                            <td class="text-center"></td>
                        </tr>`,
        SubProductViewMore: `<tr class="sub-product" data-main-id="{main_id}">
                                    <td colspan="6" class="text-center">
                                        <a href="javascript:;" data-main-id="{main_id}" class="xemthem">
                                            <span><nw  data-count="{count_item}">Xem thêm (còn {count} phân loại)</nw>
                                                <i class="icofont-simple-down"></i>
                                            </span>
                                        </a>
                                    </td>
                                </tr>`,
        ProductDetail_Images_AddImagesButton: `<div class="items import">
                                <label class="choose choose-wrap">
                                    <input class="image_input" type="file" name="myFile" multiple>
                                    <div class="choose-content choose-product-images">
                                        <i class="icofont-image"></i>
                                        <span>Thêm hình ảnh (<nw class="count">{0}</nw>/{max})</span>
                                    </div>
                                </label>
                            </div>`,
        ProductDetail_Images_Item: ` <div class="items magnific_popup" data-id="{id}">
                                <button type="button" class="delete"><i class="icofont-close-line"></i></button>
                                <a class="thumb_img thumb_1x1 magnific_thumb">
                                    <img src="{src}">
                                </a>
                            </div>`,
        ProductDetail_Video_Item: ` <div class="items magnific_popup" data-id="{id}">
                                <button type="button" class="delete"><i class="icofont-close-line"></i></button>
                                <a class="thumb_img thumb_1x1 magnific_thumb">
                                   <video>
                                      <source src="{src}" type="video/mp4">
                                      Your browser does not support the video tag.
                                    </video>
                                </a>
                            </div>`,
        ProductDetail_Attribute_Row_Item: ` <div class="col-md-6 lastest-attribute-value item" draggable="true">
                            <div class="box-list">
                                <div class="form-group namesp flex-input-choose">
                                    <div id="image_row_item" class="item flex flex-lg-nowrap gap10 mb-2 "style="flex-wrap: nowrap !important;position: relative !important;" >
                                        <div class="wrap_input" style="padding: 0 0 0 10px !important;" >
                                            <div class="manage-color  " >
                                                <div class="flex list align-items-center " style="flex-wrap: nowrap !important;position: relative !important; width: 90px !important;" > <div class="items import">
                                                        <label class="choose choose-wrap">
                                                            <input class="image_input" type="file" name="myFile">
                                                            <div class="choose-content choose-product-images" >
                                                                <i class="icofont-image"></i>
                                                            </div>
                                                        </label>
                                                    </div></div>

                                            </div>
                                        </div>
                                    </div>
                             
                                    <div class="relative w-100">
                                        <input type="text" class="form-control attributes-name attributes-name-{index}" data-id="{index}" placeholder="Tên thuộc tính" maxlength="50" value="">
                                        <p class="error" style="display:none;"> </p>
                                        <span class="note"><nw class="count">0</nw>/50</span>
                                    </div>
                                    <div class="right-action">
                                        <a class="icon-action attribute-item-delete" style="display:none;" href="javascript:;"><i class="icofont-trash"></i></a>
                                        <a class="icon-action attribute-item-draggable" href="javascript:;"><i class="icofont-drag"></i></a>
                                    </div>

                                </div>
                            </div>
                        </div>`,

        ProductDetail_Attribute_Row_Add_Attributes: ` <div class="item-edit item flex flex-lg-nowrap gap10 mb-2 w-100 product-attributes-add">
                <label class="label"></label>
                <div class="wrap_input pb-4">
                    <button  class="btn btn-add btn-add-attributes"><i class="icofont-plus"></i>Thêm phân loại</button>
                </div>
            </div>`,
        ProductDetail_Attribute_Row: ` <div class="item-edit item flex flex-lg-nowrap gap10 mb-2 w-100 product-attributes" >
                <label class="label">Phân loại hàng</label>
                <div class="wrap_input pb-4">
                    <span class="delete">
                        <i class="icofont-close"></i>
                    </span>
                    <div class="row ">
                        <div class="col-md-6">
                            <div class="box-list">
                                <h6>
                                    <b > Tên thuộc tính</b>
                                    <span class="mx-2" style="color: #919191; font-weight: normal;display:none; ">
                                        (Tùy
                                        chỉnh)
                                    </span>
                                    <nw class="edit-attributes-name-nw item "style="display:flex;" >
                                         <input type="text" class="form-control edit-attribute-name">
                                         <p type="error" style="display:none;"> </p>
                                        <a href="javascript:;" class="text-base edit-attributes-name-confirm"><i class="icofont-checked"></i></a>
                                        <a href="javascript:;" class="text-base edit-attributes-name-cancel" ><i class="icofont-close-squared-alt"></i></a>
                                    </nw>
                                    <a class="attribute-name-edit" style="display:none;" href="javascript:;"><i class="icofont-ui-edit"></i></a>
                                </h6>

                            </div>
                        </div>
                    </div>
                    <div class="line-bottom"></div>
                    <div class="row row-attributes-value">
                       {html}
                    </div>
                </div>
            </div>`,

        ProductDetail_Specification_Row_Item: ` <div class="col-md-6 " >
                        <div class="item flex flex-lg-nowrap gap10 mb-2 w-100">
                            <label class="label">{name}<span style="display:none;">0/10</span></label>
                            <div class="wrap_input">
                               {wrap_input}

                            </div>
                        </div>
                    </div>`,
        ProductDetail_Specification_Row_Item_DateTime: `<div class="datepicker-wrap namesp" data-type="2" data-attr-id="{id}">
                                <input  placeholder="Vui lòng chọn"
                                       class="datepicker-input form-control" type="text" value="{value}">
                            </div>`,
        ProductDetail_Specification_Row_Item_Input: ` <div class="form-group namesp"data-type="3" data-attr-id="{id}">
                <input type="text" class="form-control" placeholder="{placeholder}" value="{value}">
                <a href="" class="edit"><i class="icofont-thin-down"></i></a>
           
            </div>`,
        ProductDetail_Specification_Row_Item_SelectOptions_NewOptions: `<li style=" list-style: none; "><input class="checkbox-option" type="checkbox" name="{option-name}" value="{value}" {checked}> <span>{name}</span></li>`,
        ProductDetail_Specification_Row_Item_SelectOptions: ` <div class="form-group namesp"data-type="1" data-attr-id="{id}">
                <input type="text" class="form-control input-select-option" data-value="{dataid}" placeholder="{placeholder}" readonly value="{value}">
                <a href="" class="edit"><i class="icofont-thin-down"></i></a>
            </div>
            <div class="select-option p-2" style="width:90%;display:none;">
                <div class="them-chatlieu">
                    <div class="content_lightbox">
                        <div class="form-group w-100 ">
                            <div class="search-wrapper">
                                <input type="text" class="input_search onclick-togle border" name=""
                                       value="" placeholder="Chọn giá trị ...">
                                <span class="search-btn">
                                    <button type="submit">
                                        <svg class="icon-svg">
                                            <use xlink:href="/images/icons/icon.svg#search"></use>
                                        </svg>
                                    </button>
                                </span>
                            </div>
                        </div>
                        <ul style="min-height:200px;overflow:scroll;overflow-x: hidden;overflow-y: scroll;">
                            <img src="/images/icons/loading.gif" style=" width: 20px; height: 20px; " />
                         </ul>
                        <div class="border-top text-center pt-2">
                            <a href="javascript:;" class="text-primary add-specificaion-value">
                                <i class="icofont-plus mr-2"></i>Thêm thuộc
                                tính mới
                            </a>
                            <div class="flex flex-nowrap gap10 align-items-center add-specificaion-value-box" style="display:none;">
                                <input type="text" class="form-control" placeholder="Nhập vào">
                                <a href="javascript:;" class="text-base add-specificaion-value-add" style="font-size: 18px">
                                    <i class="icofont-checked"></i>
                                </a>
                                <a href="javascript:;" class="add-specificaion-value-cancel" style="font-size: 18px;">
                                    <i class="icofont-close-squared-alt "></i>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>`,
        ProductDetail_Attribute_Price_Tr_Td_hide: `<td class="td-attributes td-attribute-0 td-attributes-name-{data-id} {class-name}" style="display: none;" {row_span}>{name}</td>`,
        ProductDetail_Attribute_Price_Tr_Td: `<td class="td-attributes td-attribute-{i} td-attributes-name-{data-id} {class-name}" {row_span}>{name}</td>`,
        ProductDetail_Attribute_Price_TrMain: ` <tr class="tr-main" data-attribute-0="Phân loại 1" data-attribute-1="Phân loại 2-1">
                                   {td_arrtibute}
                                    <td class="td-price">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control input-price" placeholder="Giá nhập">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-profit">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control input-price" placeholder="Lợi nhuận">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-amount">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control input-price" placeholder="Giá bán">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-stock">
                                        <div class="form-group mb-0">
                                            <input type="text" class="form-control input-price" placeholder="Kho hàng">
                                        </div>
                                    </td>
                                    <td class="td-sku">
                                        <div class="form-group mb-0">
                                            <input type="text" class="form-control" placeholder="SKU phân loại">
                                        </div>
                                    </td>
                                </tr>  `,
        ProductDetail_Attribute_Price_TrSub: `<tr class="tr-sub" data-attribute-0="Phân loại 1" data-attribute-1="Phân loại 2-2">
                                   {td_arrtibute}
                                    <td class="td-price">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control input-price" placeholder="Giá nhập">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-profit">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control input-price" placeholder="Lợi nhuận">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-amount">
                                        <div class="form-group mb-0 price">
                                            <input type="text" class="form-control input-price" placeholder="Giá bán">
                                            <span class="note">đ</span>
                                        </div>
                                    </td>
                                    <td class="td-stock">
                                        <div class="form-group mb-0">
                                            <input type="text" class="form-control input-price" placeholder="Kho hàng">
                                        </div>
                                    </td>
                                    <td class="td-sku">
                                        <div class="form-group mb-0">
                                            <input type="text" class="form-control" placeholder="SKU phân loại">
                                        </div>
                                    </td>
                                </tr>`,
        ProductDetail_DiscountGroupBuy_Row: ` <tr class="discount-groupbuy-row">
                                    <td class="name">Khoảng giá 1</td>
                                    <td>
                                        <div class="flex gap10 flex-nowrap align-items-center justify-content-center">
                                            <div class="form-group mb-0 quanity-from">
                                                <input type="text" class="form-control input-price"
                                                       placeholder="Từ số lượng">
                                            </div>
                                            <i class="icofont-arrow-right"></i>
                                            <div class="form-group mb-0 quanity-to">
                                                <input type="text" class="form-control input-price"
                                                       placeholder="Đến số lượng">
                                            </div>
                                        </div>

                                    </td>
                                    <td>
                                        <div class="flex gap10 flex-nowrap align-items-center justify-content-center discount-value">
                                            <label class="radio mb-3">
                                                <input type="radio" name="{discount-type}" value="0">
                                                <span class="checkmark"></span>
                                            </label>
                                            <div class="form-group mb-0 price mr-3 discount-number">
                                                <input type="text" class="form-control input-price" placeholder="Nhập số">
                                                <span class="note">đ</span>
                                            </div>
                                            <label class="radio mb-3">
                                                <input type="radio" name="{discount-type}" value="1">
                                                <span class="checkmark"></span>

                                            </label>
                                            <div class="form-group mb-0 price discount-percent">
                                                <input type="text" class="form-control input-price" placeholder="Nhập số" maxlength="3">
                                                <span class="note">%</span>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="text-center">
                                       <a href="javascript:;" class="delete-row">
                                            <i class="icofont-trash"></i>
                                       </a>
                                    </td>

                                </tr>`,
        ProductDetail_GroupProduct_ResultDirection: `<b>{name}<i class="icofont-thin-right"></i></b>`,
        ProductDetail_GroupProduct_ResultSelected: `<b >{name}</b>`,
        ProductDetail_GroupProduct_colmd4_Li: ` <li data-id="{id}" data-name="{name}"><a href="javascript:;">{name}<i class="{icofont-thin-right}"></i></a></li>`,
        ProductDetail_GroupProduct_colmd4: `<div class="col-md-4" data-level="{level}">
                        <div class="list-toys">
                            <h6><a href="">{name}<i class="icofont-thin-right"></i></a></h6>
                            <ul>
                               {li}
                                
                            </ul>
                        </div>
                    </div>`
    }
}


var _product_constants_2 = {
    Values: {
        GroupProduct: 32,
        GroupProductName: 'BioLife',

    },
    Attributes: {
        Input:` <div class="col-md-6 lastest-attribute-value item ui-sortable-handle attributes-detail" draggable="true">
                                                    <div class="box-list">
                                                        <div class="form-group namesp flex-input-choose">
                                                            <label class="choose choose-wrap">
                                                                <input type="file" name="myFile">
                                                                <div class="choose-content">
                                                                    <i class="icofont-image"></i>
                                                                </div>
                                                            </label>
                                                            <div class="relative w-100">
                                                                <input type="text" class="form-control" placeholder="" value="">
                                                                <span class="note"><nw class="count">0</nw>/14</span>
                                                            </div>
                                                            <div class="right-action">
                                                                <a class="icon-action delete-attribute-detail" href="javascript:;"><i class="icofont-trash"></i></a>
                                                                <a class="icon-action attribute-item-draggable" href="javascript:;"><i class="icofont-drag"></i></a>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>`
    },
    DiscountGroupBuy: {
        Tr:` <tr>
                                            <td>Khoảng giá  @(++i)</td>
                                            <td>
                                                <div class="flex gap10 flex-nowrap align-items-center justify-content-center">
                                                    <div class="form-group mb-0">
                                                        <input type="text" class="form-control input-price " placeholder="Từ sản phẩm" value="">
                                                    </div>
                                                    <i class="icofont-arrow-right"></i>
                                                    <div class="form-group mb-0">
                                                        <input type="text" class="form-control input-price" placeholder="Đến sản phẩm" value="">
                                                    </div>
                                                </div>

                                            </td>
                                            <td>
                                                <div class="flex gap10 flex-nowrap align-items-center justify-content-center">
                                                    <label class="radio mb-3">
                                                        <input type="radio" name="discount-type-@i"checked>
                                                        <span class="checkmark"></span>
                                                    </label>
                                                    <div class="form-group mb-0 price mr-3">
                                                        <input type="text" class="form-control input-price" placeholder="Nhập số"">
                                                        <span class="note">đ</span>
                                                    </div>
                                                    <label class="radio mb-3">
                                                        <input type="radio" name="discount-type-@i" >
                                                        <span class="checkmark"></span>

                                                    </label>
                                                    <div class="form-group mb-0 price">
                                                        <input type="text" class="form-control input-price" placeholder="">
                                                        <span class="note">%</span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="text-center">
                                                 <a href="javascript:;" class="delete-row">
                                            <i class="icofont-trash"></i>
                                       </a>
                                            </td>

                                        </tr>`
    }
}

