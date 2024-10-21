let i_attributes = 0;
let index_attributes = 0;
$(document).ready(function () {
    if (window.history && window.history.pushState) {
        $(window).on('popstate', function () {
            window.location.reload()
        });

    }
    product_detail.Initialization()
})
var product_detail = {
    Initialization: function () {
        var model = [{ url: '/', name: 'Trang chủ' }, { url: '/product', name: 'Quản lý sản phẩm' }, { url: 'javascript:;', name: 'Chi tiết sản phẩm', activated: true }]
        _global_function.RenderBreadcumb(model)
        $('.add-product').addClass('placeholder')
        $('.add-product').addClass('box-placeholder')
        $('#specifications').hide()
        product_detail.HideProductTab()
        product_detail.Detail()
        product_detail.DynamicBind()

        $('.select-group-product').magnificPopup({
            type: 'inline',
            midClick: true,
            closeOnBgClick: false, // tránh đóng khi người dùng nhấn vào vùng ngoài
            mainClass: 'mfp-with-zoom',
            fixedContentPos: false,
            fixedBgPos: true,
            overflowY: 'auto',
            closeBtnInside: true,
            preloader: false,
            removalDelay: 300,
        });
    },
    DynamicBind: function () {
        $('body').on('click', '.change-tab', function () {
            var element = $(this)
            switch (element.attr('data-id')) {
                case '1': {
                    $("#images").get(0).scrollIntoView({ behavior: 'smooth' });

                } break
                case '2': {
                    $("#selling-information").get(0).scrollIntoView({ behavior: 'smooth' });

                } break
                case '3': {
                    $("#other-information").get(0).scrollIntoView({ behavior: 'smooth' });

                } break
                case '4': {
                    $("#other-information").get(0).scrollIntoView({ behavior: 'smooth' });

                } break
            }
        });
        $('body').on('click', '.magnific_popup .delete', function () {
            var element = $(this)
            element.closest('.items').remove()

        });
        $('body').on('click', '.choose-product-images', function () {
            var element = $(this)
            if (product_detail.GetImagesCount() >= _product_constants.VALUES.ProductDetail_Max_Image) {
                _msgalert.error('Số lượng ảnh sản phẩm không được vượt quá ' + _product_function.Comma(_product_constants.VALUES.ProductDetail_Max_Image) + ' ảnh')
            }
            else {
                element.closest('div').find('input').trigger('click')
            }
        });
        $('body').on('change', '.image_input', function () {
            var element = $(this)
            product_detail.AddProductImages(element)


        });
        $('body').on('click', '.select-group-product', function () {
            var element = $(this)


        });
        $('body').on('keyup', '#description textarea, #product-name input, .product-attributes input', function () {
            var element = $(this)
            element.closest('.item').find('.count').html(_product_function.Comma(element.val().length))

        });
        $('body').on('click', '#product-detail-cancel', function () {
            var element = $(this)
            /* product_detail.Cancel()*/
            var title = 'Xác nhận hủy sản phẩm';
            var description = 'Bạn có chắc chắn muốn hủy chỉnh sửa/ thêm mới sản phẩm này?';

            _msgconfirm.openDialog(title, description, function () {
                window.location.href = '/Product'

            });

        });
        $('body').on('click', '.btn-hide', function () {
            var element = $(this)
            product_detail.Hide()

        });

        $('body').on('click', '.item-edit .delete', function () {
            var element = $(this)
           /* element.closest('.product-attributes').next().remove()*/
            element.closest('.product-attributes').remove()
            if ($('.product-attributes').length <= 0) {
                $('#single-product-amount').show()
                $('#product-attributes-price').hide()
                $('#product-attributes-all-price').closest('.item-edit').hide()

            }
            if ($('.product-attributes').length < 2) {
                $('.btn-add-attributes').show()

            } else {
                $('.btn-add-attributes').hide()
            }
            product_detail.RenderRowAttributeTablePrice()
        });
        $('body').on('click', '.btn-add-attributes', function () {
            var element = $(this)
            product_detail.AddProductAttributes()
            $('#single-product-amount').hide()
            if ($('#product-attributes-box').is(':hidden')) {
                $('#product-attributes-2').hide()

            }
            $('#product-attributes-box').show()
            $('#product-attributes-price').show()
            $('#product-attributes-all-price').closest('.item-edit').show()
            $('#product-attributes-price').closest('.item-edit').show()
            if ($('.product-attributes').length < 2) {
                $('.btn-add-attributes').show()

            } else {
                $('.btn-add-attributes').hide()
            }
        });

        $('body').on('keyup', '.lastest-attribute-value .attributes-name, .lastest-attribute-value .attributes-name-add', function () {
            var element = $(this)
            element.closest('.relative').find('.error').hide()


            $('.attributes-name').each(function (index, item) {
                if (i_attributes == 0) {
                    i_attributes = index + 1;
                }
                else {
                    i_attributes++;
                }
            })

            element.closest('.lastest-attribute-value').removeClass('lastest-attribute-value')
            if (element.val() != undefined && element.val().trim() != '') {
                if ($('.attributes-name-' + i_attributes).length > 0)
                    i_attributes++;
                element.closest('.row-attributes-value').append(_product_constants.HTML.ProductDetail_Attribute_Row_Item.replaceAll("{index}", i_attributes))
                $('.row-attributes-value .col-md-6 .attribute-item-delete').show()
                if (element.closest('.row-attributes-value').find('.col-md-6').length < 2) {
                    element.closest('.row-attributes-value').find('.attribute-item-delete').hide()
                }
            }
        });

        $('body').on('click', '.attribute-item-delete', function () {
            var element = $(this)
            var id_attributes = element.closest('.col-md-6').find('.attributes-name').attr('data-id')
            var text = $('.attributes-name-' + id_attributes).val();
            var id = -1;
            $('.product-attributes').each(function (index, item) {
                var element_parent = $(this)
                //var product_attribute_by_id = {
                //    id: index,
                //    data_values: [],
                //}
                //element.find('.attributes-name').each(function (index, item) {
                //    var element_input = $(this)

                //    if (element_input != undefined && element_input.val() != undefined && element_input.val().trim() != '' && text.trim() == element_input.val().trim()) {
                //        product_attribute_by_id.data_values.push(element_input.val())
                //    }

                //})
                //if (product_attribute_by_id.data_values.length > 0)
                //    id = product_attribute_by_id.id
                if (element.closest('.product-attributes')[0] === element_parent[0]) {
                    id = index
                    return false
                }
            })
            if (element.closest('.row-attributes-value').find('.col-md-6').length <= 2) {
                element.closest('.row-attributes-value').find('.attribute-item-delete').hide()
            }
            element.closest('.col-md-6').remove()
            if (id > -1) {
                product_detail.DeleteRowAttributeTablePrice(text, id)
            }
        });
        $('body').on('click', '.attribute-item-draggable', function () {
            var element = $(this)
            element.closest('.row-attributes-value').sortable({
                group: "row-attributes-value",
                animation: 150,
                ghostClass: "row-attributes-value",
            });

            //const column = document.querySelector('.row-attributes-value');

            //new Sortable(column, {
            //    animation: 150,
            //    ghostClass: 'blue-background-class'
            //});
            //const columns = document.querySelectorAll(".row-attributes-value");

            //columns.forEach((column) => {
            //    new Sortable(column, {
            //        group: "shared",
            //        animation: 150,
            //        ghostClass: "blue-background-class",
            //    });

            //});
        });
        $('body').on('click', '.attribute-item-add', function () {
            var element = $(this)

            $('.attributes-name').each(function (index, item) {
                i_attributes++;
                if ($('.attributes-name-' + i_attributes).length > 0)
                    i_attributes++;
            })
            element.closest('.row-attributes-value').append(_product_constants.HTML.ProductDetail_Attribute_Row_Item.replaceAll("{index}", i_attributes))
            element.closest('.row-attributes-value').find('.attribute-item-delete').show()
            //element.closest('.row-attributes-value').find('.attributes-name').addClass('attributes-name-add')
            //element.closest('.row-attributes-value').find('.attributes-name').removeClass('attributes-name')

        });
  
        $('body').on('click', '.attribute-name-edit', function () {
            var element = $(this)
            element.hide()
            element.closest('h6').find('b').attr('data-value', element.closest('h6').find('b').html())
            element.closest('h6').find('b').html('Nhập tên thuộc tính mới')
            element.closest('h6').find('b').hide()
            element.closest('h6').find('span').hide()
            element.closest('h6').find('.edit-attributes-name-nw').show()
            element.hide()

        });
        $('body').on('keyup', '.edit-attributes-name-nw input', function () {
            var element = $(this)
            var value = element.closest('h6').find('.edit-attribute-name').val()
            element.closest('h6').find('b').html(value)

        });
        $('body').on('click', '.edit-attributes-name-confirm', function () {
            var element = $(this)
            var value = element.closest('h6').find('.edit-attribute-name').val()
            element.closest('h6').find('b').html(value)
            element.closest('h6').find('b').show()

            element.closest('h6').find('.edit-attributes-name-nw').hide()
            element.closest('h6').find('.attribute-name-edit').show()
            element.closest('h6').find('span').show()
        });
        $('body').on('click', '.edit-attributes-name-cancel', function () {
            var element = $(this)
            element.closest('h6').find('b').html(element.closest('h6').find('b').attr('data-value'))
            element.closest('h6').find('.edit-attributes-name-nw').hide()
            element.closest('h6').find('b').show()
            element.closest('h6').find('span').show()
            element.closest('h6').find('.attribute-name-edit').show()

        });
        $('body').on('keyup', '#specifications .them-chatlieu .input_search', function () {
            var element = $(this)
            setTimeout(function () {
                product_detail.RenderSpecificationLi(element)
            }, 1000);
        });
        $('body').on('click', '.specifications-box .col-md-6 .namesp input', function () {
            var element = $(this)
            var parent = element.closest('.col-md-6').find('.select-option')
            $('.specifications-box .col-md-6 .select-option').each(function (index, item) {
                var compare = $(this)
                if (compare.is(parent)) {
                    if (parent.is(':hidden')) {
                        parent.fadeIn()
                        product_detail.RenderSpecificationLi(compare)

                    } else {
                        parent.fadeOut()
                    }
                } else {
                    compare.fadeOut()
                }


            })
            //$('.specifications-box .col-md-6 .select-option').fadeOut()

            //if (element.closest('.col-md-6').find('.select-option').length > 0 && element.closest('.col-md-6').find('.select-option').is('hidden')) {
            //    element.closest('.col-md-6').find('.select-option').fadeIn()
            //}

        });
        //-- Click outside div global function:
        $('body').on('click', function (e) {
            var location = $(e.target)
            if (location.closest('.specifications-box').length === 0) {
                $('.specifications-box .col-md-6 .select-option').fadeOut()
            }
            else if (location.closest('.col-md-6').find('.select-option').length === 0) {
                $('.specifications-box .col-md-6 .select-option').fadeOut()
            }



        });
        $('body').on('click', '.specifications-box .col-md-6 .add-specificaion-value', function (e) {
            var element = $(this)
            element.closest('.border-top').find('.add-specificaion-value-box').show()
            element.hide()

        });
        $('body').on('click', '.specifications-box .col-md-6 .add-specificaion-value-add , .specifications-box .col-md-6 .add-specificaion-value-cancel ', function (e) {
            var element = $(this)
            element.closest('.border-top').find('.add-specificaion-value-box').hide()
            element.closest('.border-top').find('.add-specificaion-value').show()

        });
        $('body').on('click', '.specifications-box .col-md-6 .add-specificaion-value-add ', function (e) {
            var element = $(this)
            var parent = element.closest('.them-chatlieu').find('ul')
            var name = element.closest('.add-specificaion-value-box').find('input').val()
            parent.prepend(_product_constants.HTML.ProductDetail_Specification_Row_Item_SelectOptions_NewOptions
                .replaceAll('{checked}', 'checked')
                .replaceAll('{value}', 'undefined')
                .replaceAll('{name}', name)
            )
            element.closest('.add-specificaion-value-box').find('input').val('')
            product_detail.RenderSpecificationSelectOption(element.closest('.them-chatlieu').find('ul').find('li'))
            var type = element.closest('.col-md-6').find('.namesp').attr('data-attr-id')

            _product_function.POST('/Product/AddProductSpecification', { type: type, name: name }, function (result) {

            });
        });

        $('body').on('click', '.btn-add-attributes, .edit-attributes-name-confirm, .attribute-item-add, .attribute-item-delete, #product-attributes-box .delete', function () {
            /* product_detail.RenderRowAttributeTablePrice()*/

        });
        $('body').on('focusout', '.attributes-name', function () {

            var element = $(this)

            var id_attributes = element.attr('data-id')
            var name = element.attr('data-name')
            var type = 0;
            var text = $('.attributes-name-' + id_attributes).val();
            $('.attributes-name').each(function (index, item) {
                var element = $(this)
                if (text != undefined && text != "" && text.toUpperCase().trim() == element.val().toUpperCase().trim() && parseFloat(id_attributes) != parseFloat(element.attr('data-id'))) {
                    _msgalert.error("Tên phân loại " + text + "  đã có ")
                    type = 1;
                }
            })
            name == undefined ? name = text : name;
            if (name != "" && $('.' + _global_function.RemoveSpecialCharacter_ky_tu_DB(name.trim()).replaceAll(' ', '-')).length > 0) {
                if (type == 0 && name.trim() != text.trim() && text != "") {
                    $('.tr-main').each(function (index_td, item_td) {
                        var element = $(this)

                        var attr_value = element.attr('data-attribute-0')
                        var attr_value2 = element.attr('data-attribute-1')
                        if (attr_value != undefined && attr_value.trim() == name.trim()) {
                            element.attr('data-attribute-0', text)
                        }
                        if (attr_value2 != undefined && attr_value2.trim() == name.trim()) {
                            element.attr('data-attribute-1', text)
                        }
                    })
                    $('.tr-sub').each(function (index_td, item_td) {
                        var element = $(this)

                        var attr_value = element.attr('data-attribute-0')
                        var attr_value2 = element.attr('data-attribute-1')
                        if (attr_value != undefined && attr_value.trim() == name.trim()) {
                            element.attr('data-attribute-0', text)
                        }
                        if (attr_value2 != undefined && attr_value2.trim() == name.trim()) {
                            element.attr('data-attribute-1', text)
                        }
                    })
                    $('.' + _global_function.RemoveSpecialCharacter_ky_tu_DB(name.trim()).replaceAll(' ', '-')).html(text);
                    $('.' + _global_function.RemoveSpecialCharacter_ky_tu_DB(name.trim()).replaceAll(' ', '-')).addClass(_global_function.RemoveSpecialCharacter_ky_tu_DB(text.trim()).replaceAll(' ', '-'));
                    $('.' + _global_function.RemoveSpecialCharacter_ky_tu_DB(name.trim()).replaceAll(' ', '-')).removeClass(_global_function.RemoveSpecialCharacter_ky_tu_DB(name.trim()).replaceAll(' ', '-'));
                    element.attr('data-name', text)
                }
                else {
                    if (type != 1) {
                        var id = -1;
                        $('.product-attributes').each(function (index, item) {
                            var element_parent = $(this)


                            element_parent.find('.attributes-name').each(function (index2, item2) {
                                var element_input = $(this)
                                if (element_input.attr('data-name') != "" && element_input.attr('data-name') != undefined && element_input.attr('data-name').trim() == name.trim()) {
                                    id = index

                                }

                            })
                        })
                        if (id != -1)
                            product_detail.DeleteRowAttributeTablePrice(name, id)
                    }
                  
                }
             
            } else {
                if (text != "" && type == 0) {
                    element.attr('data-name', text)
                    product_detail.AddRowAttributeTablePrice(id_attributes)
                }
                
            }


        });
        $('body').on('click', '.btn-all', function () {
            product_detail.ApplyAllPriceToTable()


        });
        $('body').on('click', '.btn-add-discount-groupbuy', function () {
            $('#discount-groupbuy').show()
            $('.btn-add-discount-groupbuy').closest('.row').hide()
            var id = $('.discount-groupbuy-row').length + 1
            var html = _product_constants.HTML.ProductDetail_DiscountGroupBuy_Row
                .replaceAll('Khoảng giá 1', 'Khoảng giá ' + id)
                .replaceAll('{discount-type}', 'discount-type-' + id)

            $(html).insertBefore('#discount-groupbuy .summary')
        });
        $('body').on('click', '#discount-groupbuy .delete-row', function () {
            var element = $(this)
            element.closest('.discount-groupbuy-row').remove()
            if ($('.discount-groupbuy-row').length <= 0) {
                $('#discount-groupbuy').hide()
                $('.btn-add-discount-groupbuy').closest('.row').show()
            }

        });
        $('body').on('keyup', '.input-price', function () {
            var element = $(this)
            var value = element.val()
            element.val(Comma(value))
            product_detail.UpdateRowData(element.closest('tr'))
        });
        $('body').on('click', '#discount-groupbuy .summary .btn-add', function () {

            var id = $('.discount-groupbuy-row').length + 1
            var html = _product_constants.HTML.ProductDetail_DiscountGroupBuy_Row
                .replaceAll('Khoảng giá 1', 'Khoảng giá ' + id)
                .replaceAll('{discount-type}', 'discount-type-' + id)

            $(html).insertBefore('#discount-groupbuy .summary')

        });
        $('body').on('click', '#them-nganhhang li', function () {
            var element = $(this)
            element.closest('.col-md-4').find('li').removeClass('active')
            element.addClass('active')
            product_detail.RenderOnSelectGroupProduct(element)

        });
        $('body').on('click', '.action .btn-outline-base', function () {
            $.magnificPopup.close()

        });
        $('body').on('click', '.action .btn-round', function () {
            product_detail.RenderSelectedGroupProduct()
            $.magnificPopup.close()
        });
        $('body').on('click', '#product-detail-cancel', function () {
            let title = 'Xác nhận hủy';
            let description = 'Dữ liệu đã chỉnh sửa sẽ không được lưu, bạn có chắc chắn không?';
            _msgconfirm.openDialog(title, description, function () {
                window.location.href='/product'

            });
        });
        $('body').on('click', '#product-detail-hide', function () {
            let title = 'Xác nhận ẩn sản phẩm';
            let description = 'Sản phẩm sẽ không còn được hiển thị ngoài trang sản phẩm, bạn có chắc chắn không?';
            _msgconfirm.openDialog(title, description, function () {
                _product_function.POST('/Product/CancelProduct', { product_id: $('#product_detail').val() }, function (result) {
                    if (result.is_success && result.data) {
                        _msgalert.success('Ẩn sản phẩm thành công')
                        setTimeout(function () {
                            window.location.href('/product/detail')
                        }, 2000);
                    }
                    else {
                    }
                });

            });
        });
        $('body').on('click', '#product-detail-confirm', function () {
            product_detail.Summit()
        });
        $('body').on('click', '.them-chatlieu .checkbox-option', function () {
            var element = $(this)

            product_detail.RenderSpecificationSelectOption(element)

        });
        $('body').on('keyup', '.col-md-6 .input-select-option', function (e) {
            e.preventDefault()
        });

        $('body').on('keyup', '#single-product-amount input', function (e) {
            var element = $(this)
            var value = element.val()
            product_detail.RenderSingleProductAmount()
            if (element.closest('.price').length > 0) {
                element.val(_product_function.Comma(element.val()))
            }
        });
        $('body').on('keyup', '.discount-percent input', function (e) {
            $('#discount-groupbuy tbody .discount-groupbuy-row').each(function (index, item) {
                var element = $(this)
                var checkbox_value = element.find('input[name="discount-type-' + (index + 1) + '"]:checked').val()
                var discount = 0
                switch (checkbox_value) {

                    case '1': {
                        discount = parseFloat(element.find('.discount-percent').find('input').val().replaceAll(',', ''))
                        if (discount > 100) {
                            _msgalert.error("Chiết khấu tối đa 100%")
                                element.find('.discount-percent').find('input').val(100)
                        }
                    } break
                }
            })


        });
        $('body').on('click', '#discount-groupbuy input[type="radio"]', function () {
            var element = $(this)
            $('#discount-groupbuy tbody .discount-groupbuy-row').each(function (index, item) {
                var element = $(this)
                var checkbox_value = element.find('input[name="discount-type-' + (index + 1) + '"]:checked').val()
                var discount = 0
                switch (checkbox_value) {

                    case '1': {
                        discount = parseFloat(element.find('.discount-percent').find('input').val().replaceAll(',', ''))
                        if (discount > 100) {
                            _msgalert.error("Chiết khấu tối đa 100%")
                        }
                    } break
                }
            })
           

        });
       
    },
    Detail: function () {
        var product_id = $('#product_detail').val()
        if (product_id != null && product_id != undefined && product_id.trim() != '') {
            _product_function.POST('/Product/ProductDetail', { product_id: $('#product_detail').val() }, function (result) {
                if (result.is_success && result.data) {
                    product_detail.RenderExistsProduct(JSON.parse(result.data), result.product_group)

                }
                else {
                    window.location.href = '/error'
                }
            });
        }
        else {
            $('.btn-update').html('Thêm mới')
            product_detail.RenderAddNewProduct()

        }


    },

    RenderAddNewProduct: function () {
        $('#product-attributes-box').html('')
        $('#discount-groupbuy').hide()
        $('.btn-add-discount-groupbuy').closest('.row').show()

        var html = ''
        $(_product_constants.VALUES.DefaultSpecificationValue).each(function (index, item) {
            switch (item.type) {
                case 1: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_Input
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)

                        .replaceAll('{value}', '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type)
                        .replaceAll('{name}', item.name)
                        .replaceAll('{wrap_input}', html_item)


                } break;
                case 2: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_DateTime
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)

                        .replaceAll('{value}', '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type).replaceAll('{name}', item.name).replaceAll('{wrap_input}', html_item)


                } break;
                default: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_SelectOptions
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)
                        .replaceAll('{value}', '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type).replaceAll('{name}', item.name).replaceAll('{wrap_input}', html_item)




                } break;
            }
        })
        $('.specifications-box').html(html)
        product_detail.RenderRowAttributeTablePrice()
        $('#images .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', _product_constants.VALUES.ProductDetail_Max_Image))
        $('#avatar .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', '1'))
        $('#videos .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', '1'))
        $('#videos .list span').html('Thêm video (<nw class="count">0</nw>/1)')
        product_detail.ShowProductTab()
        $('.add-product').removeClass('placeholder')
        $('.add-product').removeClass('box-placeholder')
        $('#single-product-amount').show()
        $('#product-attributes-price').hide()
        $('#product-attributes-all-price').closest('.item-edit').hide()
        $('.select2').each(function (index, item) {
            var element = $(this)
            element.select2({
                minimumResultsForSearch: Infinity
            });
        })
        $('#group-product-selection').html('')
        $('#group-id input').attr('placeholder', 'Chọn ngành hàng')
        $('#group-id input').attr('data-id', '-1')
        _product_function.POST('/Product/GroupProduct', { group_id: 1 }, function (result) {
            if (result.is_success && result.data) {
                $('#them-nganhhang .bg-box .row').html('')
                var html = _product_constants.HTML.ProductDetail_GroupProduct_colmd4
                var html_item = ''
                $(result.data).each(function (index, item) {
                    html_item += _product_constants.HTML.ProductDetail_GroupProduct_colmd4_Li
                        .replaceAll('{id}', item.id).replaceAll('{name}', item.name)
                })
                html = html.replace('{li}', html_item).replaceAll('{name}', 'HuloToy').replaceAll('{level}', '0')
                $('#them-nganhhang .bg-box .row').html(html)
            }
        });
        $('#specifications .datepicker-input').each(function (index, item) {
            var element = $(this)
            SingleDatePicker(element, 'down')
        })
    },
    RenderExistsProduct: function (product, group_string) {
        //--Init
        $('#product_detail').attr('data-status', product.status)
        $('.add-product').removeClass('placeholder')
        $('.add-product').removeClass('box-placeholder')
        $('#images .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', _product_constants.VALUES.ProductDetail_Max_Image))
        $('#avatar .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', '1'))
        $('#videos .list').html(_product_constants.HTML.ProductDetail_Images_AddImagesButton.replace('{0}', '0').replace('{max}', '1'))
        $('#videos .list span').html('Thêm video (<nw class="count">0</nw>/1)')
        product_detail.ShowProductTab()

        //-- Image
        $(product.images).each(function (index, item) {
            if (item == null || item.trim() == '') return true
            var img_src = _product_function.CorrectImage(item)

            $('#images .list').prepend(_product_constants.HTML.ProductDetail_Images_Item.replaceAll('{src}', img_src).replaceAll('{id}', '-1'))
            $('#images .items .count').html($('#images .items .count').closest('.list').find('.magnific_popup').length)

        })
        //-- Avatar
        var img_src = _product_function.CorrectImage(product.avatar)

        $('#avatar .list').prepend(_product_constants.HTML.ProductDetail_Images_Item.replaceAll('{src}', img_src).replaceAll('{id}', '-1'))
        $('#avatar .items .count').html($('#avatar .items .count').closest('.list').find('.magnific_popup').length)

        $(product.videos).each(function (index, item) {
            if (item == null || item.trim() == '') return true
            var img_src = _product_function.CorrectImage(item)
            $('#videos .list').prepend(_product_constants.HTML.ProductDetail_Video_Item.replaceAll('{src}', img_src).replaceAll('{id}', '-1'))
            $('#videos .items .count').html($('#videos .items .count').closest('.list').find('.magnific_popup').length)

        })

        $('#product-name input').val(product.name)

        //-- Group Product
        $('#group-id input').attr('data-id', product.group_product_id)
        $('#group-id input').val(group_string)
        _product_function.POST('/Product/GroupProduct', { group_id: 1 }, function (result) {
            if (result.is_success && result.data) {
                $('#them-nganhhang .bg-box .row').html('')
                var html = _product_constants.HTML.ProductDetail_GroupProduct_colmd4
                var html_item = ''
                $(result.data).each(function (index, item) {
                    html_item += _product_constants.HTML.ProductDetail_GroupProduct_colmd4_Li
                        .replaceAll('{id}', item.id).replaceAll('{name}', item.name)
                })
                html = html.replace('{li}', html_item).replaceAll('{name}', 'HuloToy').replaceAll('{level}', '0')
                $('#them-nganhhang .bg-box .row').html(html)
                //$('.select-group-product').trigger('click')
                //$('#them-nganhhang .col-md-4').first().find('li[data-id="' + product.group_product_id.split(',')[0] + '"]').trigger('click')
                //var group_split = product.group_product_id.split(',')
                //for (var i = 0; i < group_split.length; i++) {
                //    setTimeout(function () {
                //        $('#them-nganhhang li').removeClass('active')
                //        $('#them-nganhhang li[data-id="' + group_split[i] + '"]').addClass('active')
                //        debugger
                //    },200);
                //}

                let _group_name = $('#them-nganhhang li[data-id="' + product.group_product_id.split(',')[0] + '"]').attr('data-name')
                $(product.group_product_id.split(',')).each(function (level, item_id) {
                  
                    
                    _product_function.POST('/Product/GroupProduct', { group_id: parseInt(item_id) }, function (result) {
                        if (result.is_success && result.data && result.data.length > 0) {
                            var html = _product_constants.HTML.ProductDetail_GroupProduct_colmd4
                            var html_item = ''
                            var name=''
                            $(result.data).each(function (index, item) {
                                html_item += _product_constants.HTML.ProductDetail_GroupProduct_colmd4_Li
                                    .replaceAll('{id}', item.id).replaceAll('{name}', item.name)
                                if (parseInt(item_id) == item.parentId) name = item.name;
                            })
                            html = html.replace('{li}', html_item).replaceAll('{name}', _group_name).replaceAll('{level}', (level))
                            $('#them-nganhhang .bg-box .row').append(html)
                            _group_name = name;
                        }
                    });
                    setTimeout(function () {
                        $('#them-nganhhang li[data-id="' + item_id + '"]').addClass('active')
                       var html_selected_popup=''
                        $('#them-nganhhang .col-md-4').each(function (index, item) {
                            var element = $(this)
                            var selected = element.find('ul').find('.active').attr('data-name')
                            if (index >= ($('#them-nganhhang .col-md-4').length - 1)) {
                                html_selected_popup += _product_constants.HTML.ProductDetail_GroupProduct_ResultSelected.replaceAll('{name}', element.find('ul').find('.active').attr('data-name'))
                            } else {

                                html_selected_popup += _product_constants.HTML.ProductDetail_GroupProduct_ResultDirection.replaceAll('{name}', selected)
                            }

                            lastest_group_id = element.find('.active').attr('data-id')
                            level = index
                            lastest_group_name = element.find('ul').find('.active').attr('data-name')
                        })
                        $('#group-product-selection').html(html_selected_popup)
                       /* $('#them-nganhhang li[data-id="' + item + '"]').click()*/
                    }, 500);
                })
            }
        });

        //-- Specification
        var html = ''
        
        $(_product_constants.VALUES.DefaultSpecificationValue).each(function (index, item) {
            var specification = [];

            if (product.specification != null && product.specification.length > 0) {
                specification = product.specification.filter(obj => {
                    return obj.attribute_id === item.id
                })
            }
            switch (item.type) {
                case 1: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_Input
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)
                        .replaceAll('{value}', specification.length > 0 ? specification[0].value != "null" ? specification[0].value:'' : '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type)
                        .replaceAll('{name}', item.name)
                        .replaceAll('{wrap_input}', html_item)


                } break;
                case 2: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_DateTime
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{id}', item.id)
                        .replaceAll('{value}', specification.length > 0 ? specification[0].value != "null" ? specification[0].value : '' : '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type).replaceAll('{name}', item.name).replaceAll('{wrap_input}', html_item)


                } break;
                default: {
                    var html_item = _product_constants.HTML.ProductDetail_Specification_Row_Item_SelectOptions
                        .replaceAll('{placeholder}', ('Nhập ' + item.name))
                        .replaceAll('{dataid}', (specification.length > 0 ? specification[0].type_ids : ''))
                        .replaceAll('{id}', item.id)
                        .replaceAll('{value}', specification.length > 0 ? specification[0].value != "null" ? specification[0].value : '' : '')

                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item
                        .replaceAll('{type}', item.type).replaceAll('{name}', item.name).replaceAll('{wrap_input}', html_item)




                } break;
            }
        })

        $('.specifications-box').html(html)
        $('#specifications .datepicker-input').each(function (index, item) {
            var element = $(this)
            SingleDatePicker(element, 'down')
        })
        $('#description textarea').val(product.description)
        $('#main-price input').val(_product_function.Comma(product.price))
        $('#main-amount input').val(_product_function.Comma(product.amount))
        $('#main-stock input').val(_product_function.Comma(product.quanity_of_stock))
        $('#main-sku input').val(product.sku)
        $('#main-profit input').val(_product_function.Comma(product.profit))
        if (product.attributes != undefined && product.attributes.length > 0) {
            $('#single-product-amount').hide()
            $('#product-attributes-box').show()
        } else {
            $('#single-product-amount').show()
            $('#product-attributes-box').hide()
        }
        if (product.attributes == null || product.attributes.length <= 0) {
            $('#product-attributes-price').closest('.item-edit').hide()
        }
        else {
            //Attribute:
            $('#product-attributes-box').html('')
            for (var i = 0; i < product.attributes.length; i++) {
                product_detail.AddProductAttributes()
            }
            var old_attribute = false
            $('.product-attributes').each(function (index, item) {
                var element = $(this)
                var attribute = product.attributes.filter(obj => {
                    return obj._id == index
                })
                if (attribute.length <= 0 || old_attribute == true) {
                    attribute = product.attributes.filter(obj => {
                        return obj._id == ('' + ( + 1))
                    })
                    old_attribute = true;
                }
                element.find('.edit-attribute-name').val(attribute[0].name)
                element.find('.edit-attributes-name-confirm').trigger('click')
                //element.find('.row-attributes-value').html('')
                var attribute_detail = product.attributes_detail.filter(obj => {
                    return obj.attribute_id == index
                })
                if (attribute_detail.length <= 0 || old_attribute == true) {
                    attribute_detail = product.attributes_detail.filter(obj => {
                        return obj.attribute_id == ('' + (index+1))
                    })
                }
                var first = true
                $(attribute_detail).each(function (index_detail, item_detail) {
                    $(element.find('.row-attributes-value').find('.col-md-6').find('.attributes-name')).each(function (index_element, item_element) {
                        if (index_element == index_detail) {
                            var element_attr_detail = $(this)
                            if (item_detail.img != null) {

                                var img_src = _product_function.CorrectImage(item_detail.img)

                              
                                var html =`<div class="items magnific_popup" data-id="-1" bis_skin_checked="1">
                                <button type="button" class="delete"><i class="icofont-close-line"></i></button>
                                <a class="thumb_img thumb_1x1 magnific_thumb">
                                    <img src="{img_src}">
                                </a>
                            </div>`
                                element_attr_detail.closest('.col-md-6').find('#image_row_item').find('.list').prepend(html.replaceAll('{img_src}', img_src))

                            }
                            element_attr_detail.val(item_detail.name)
                            element_attr_detail.attr('data-name',item_detail.name)
                            element_attr_detail.trigger('keyup')
                        }
                    })
                })
            })
            product_detail.RenderRowAttributeTablePrice()
            _product_function.POST('/Product/ProductSubListing', { parent_id: product._id }, function (result) {
                if (result.is_success && result.data) {
                    $('#product-attributes-price tbody tr').each(function (index, item) {
                        var element = $(this)
                        var list = result.data
                        for (var i = 0; i < product.attributes.length; i++) {
                            list = list.filter(obj => {
                                //return obj.variation_attributes.includes({ level: i, name: element.attr('data-attribute-'+i)})
                                return obj.variation_detail.some(e => e.id == i && e.name == element.attr('data-attribute-' + i))
                            })
                        }
                        if (list.length <= 0) {
                            list = result.data
                            for (var i = 0; i < product.attributes.length; i++) {
                                var attr_value = element.attr('data-attribute-' + i);
                                list = list.filter(obj => {
                                    //return obj.variation_attributes.includes({ level: i, name: element.attr('data-attribute-'+i)})
                                    return obj.variation_detail.some(e => e.id == ('' + (i + 1)) && e.name == attr_value)
                                })
                            }
                        }
                        if (list.length > 0) {
                            element.attr('data-id', list[0]._id)
                            element.find('.td-price').find('input').val(list[0].price == 0 ? null: Comma(list[0].price))
                            element.find('.td-profit').find('input').val(list[0].profit == 0 ? null :Comma(list[0].profit))
                            element.find('.td-amount').find('input').val(list[0].amount == 0 ? null :Comma(list[0].amount))
                            element.find('.td-stock').find('input').val(list[0].quanity_of_stock == 0 ? null :Comma(list[0].quanity_of_stock))
                            element.find('.td-sku').find('input').val(list[0].sku)
                        }
                        
                    })

                }
            });
        }



        if (product.discount_group_buy != undefined && product.discount_group_buy.length > 0) {
            $('.btn-add-discount-groupbuy').closest('.col-md-6').hide()
            $('#discount-groupbuy').show()
            for (var i = 1; i <= product.discount_group_buy.length; i++) {
                $('#discount-groupbuy .btn-add').trigger('click')
            }
            $('#discount-groupbuy tbody .discount-groupbuy-row').each(function (index, item) {
                var element = $(this)
                var selected = product.discount_group_buy[index]
                element.find('.quanity-from').find('input').val(Comma(selected.from))
                element.find('.quanity-to').find('input').val(Comma(selected.to))
                element.find('input[name="discount-type-' + (index + 1) + '"][value=' + (selected.type <= 0 ? 0 : selected.type) + ']').prop('checked', 'checked')
                switch (selected.type) {
                    case 1: {
                        element.find('.discount-percent').find('input').val(Comma(selected.discount))

                    } break;
                    default: {
                        element.find('.discount-number').find('input').val(Comma(selected.discount))
                    } break;

                }
            })
        } else {
            $('.btn-add-discount-groupbuy').closest('.col-md-6').show()
            $('#discount-groupbuy').hide()
        }
        $('#other-information input[name="preorder_status"][value=' + product.preorder_status + ']').prop('checked', 'checked')
        $('#condition_of_product .select2').val(product.condition_of_product).trigger('change')
        $('#sku input').val(product.sku)

    },
    RenderSpecificationLi: function (element) {
        var html = ''
        var type = element.closest('.col-md-6').find('.namesp').attr('data-attr-id')
        var name = element.closest('.col-md-6').find('.input_search').val()

        _product_function.POST('/Product/GetSpecificationByName', { type: type, name: name }, function (result) {
            if (result.is_success && result.data && result.data.length > 0) {
                var current_value = element.closest('.col-md-6').find('.namesp').find('.input-select-option').attr('data-value')
                if (current_value == undefined) current_value = ''
                $(result.data).each(function (index, item) {
                    html += _product_constants.HTML.ProductDetail_Specification_Row_Item_SelectOptions_NewOptions
                        .replaceAll('{option-name}', 'specification-' + type)
                        .replaceAll('{value}', item._id)
                        .replaceAll('{checked}', current_value.includes(item._id) ? 'checked' : '')
                        .replaceAll('{name}', item.attribute_name)
                })
            }
            element.closest('.col-md-6').find('ul').html(html)

        });
    },
    RenderSpecificationSelectOption: function (element) {
        var value = ''
        var html = ''
        element.closest('ul').find('input:checked').each(function (index, item) {
            var checkbox_element = $(this)
            value += checkbox_element.val()
            html += checkbox_element.closest('li').find('span').html()
            if (index < (element.closest('ul').find('input:checked').length - 1)) {
                value += ','
                html += ','
            }
        });
        element.closest('.col-md-6').find('.namesp').find('.input-select-option').attr('data-value', value)
        element.closest('.col-md-6').find('.namesp').find('.input-select-option').val(html)
    },
    RenderOnSelectGroupProduct: function (element_selected) {
        var html_selected_popup = ''
        var lastest_group_id = 0
        var level = 0
        var lastest_group_name = ''
        var selected_md4_level = parseInt(element_selected.closest('.col-md-4').attr('data-level'))
        $('#them-nganhhang .col-md-4').each(function (index, item) {
            var element = $(this)
            if (index > parseInt(selected_md4_level)) {
                element.remove()
            }
        })
        $('#them-nganhhang .col-md-4').each(function (index, item) {
            var element = $(this)
            var selected = element.find('ul').find('.active').attr('data-name')
            if (index >= ($('#them-nganhhang .col-md-4').length - 1)) {
                html_selected_popup += _product_constants.HTML.ProductDetail_GroupProduct_ResultSelected.replaceAll('{name}', element.find('ul').find('.active').attr('data-name'))
            } else {

                html_selected_popup += _product_constants.HTML.ProductDetail_GroupProduct_ResultDirection.replaceAll('{name}', selected)
            }

            lastest_group_id = element.find('.active').attr('data-id')
            level = index
            lastest_group_name = element.find('ul').find('.active').attr('data-name')
        })
        $('#group-product-selection').html(html_selected_popup)

        _product_function.POST('/Product/GroupProduct', { group_id: parseInt(lastest_group_id) }, function (result) {
            if (result.is_success && result.data && result.data.length > 0) {
                var html = _product_constants.HTML.ProductDetail_GroupProduct_colmd4
                var html_item = ''
                $(result.data).each(function (index, item) {
                    html_item += _product_constants.HTML.ProductDetail_GroupProduct_colmd4_Li
                        .replaceAll('{id}', item.id).replaceAll('{name}', item.name)
                })
                html = html.replace('{li}', html_item).replaceAll('{name}', lastest_group_name).replaceAll('{level}', (level + 1))
                $('#them-nganhhang .bg-box .row').append(html)

            }
            
        });
    },

    RenderSingleProductAmount: function () {
        var price = parseFloat($('#main-price input').val().replaceAll(',', ''))
        var profit = parseFloat($('#main-profit input').val().replaceAll(',', ''))
        $('#main-amount input').val(_product_function.Comma(price + profit))
    },
    GetImagesCount: function () {
        return (($('#images .list .items').length) - 1);
    },
    ShowProductTab: function () {
        $('#specification-disabled').hide()
        $('#selling-information-disabled').hide()
        $('#other-information-disabled').hide()

        $('#specifications').show()
        $('#selling-information').show()
        $('#other-information').show()
    },
    HideProductTab: function () {
        $('#specification-disabled').show()
        $('#selling-information-disabled').show()
        $('#other-information-disabled').show()

        $('#specifications').hide()
        $('#selling-information').hide()
        $('#other-information').hide()
    },
    Cancel: function () {
        var title = 'Xác nhận hủy sản phẩm';
        var description = 'Bạn có chắc chắn muốn hủy chỉnh sửa/ thêm mới sản phẩm này?';

        _msgconfirm.openDialog(title, description, function () {
            window.location.href = '/Product'

        });
    },
    Hide: function () {
        var title = 'Xác nhận ẩn sản phẩm';
        var description = 'Bạn có chắc chắn muốn ẩn sản phẩm này?';

        _msgconfirm.openDialog(title, description, function () {
            window.location.href = '/Product'

        });
    },
    AddProductImages: function (element) {
        switch (element.closest('.flex-lg-nowrap').attr('id')) {
            case 'images':
            case 'avatar': {
                var max_item = _product_constants.VALUES.ProductDetail_Max_Image
                if ((element.closest('.flex-lg-nowrap').find('.magnific_popup').length - 1) >= max_item) {
                    _msgalert.error('Số lượng ảnh vượt quá giới hạn')
                    element.val(null)
                }
                else {
                    if ($.inArray(element.val().split('.').pop().toLowerCase(), _product_constants.VALUES.ImageExtension) == -1) {
                        _msgalert.error("Vui lòng chỉ upload các định dạng sau: " + _product_constants.VALUES.ImageExtension.join(', '));
                        return
                    }
                    if ((element.closest('.flex-lg-nowrap').find('.magnific_popup').length - 1 + element[0].files.length) > max_item) {
                        _msgalert.error('Số lượng ảnh vượt quá giới hạn')
                        element.val(null)
                        return
                    }
                    $(element[0].files).each(function (index, item) {

                        var reader = new FileReader();
                        reader.onload = function (e) {
                            element.closest('.list').prepend(_product_constants.HTML.ProductDetail_Images_Item.replaceAll('{src}', e.target.result).replaceAll('{id}', '-1'))
                            element.closest('.items').find('.count').html(element.closest('.list').find('.magnific_popup').length)

                        }
                        reader.readAsDataURL(item);
                    });
                    element.val(null)

                }
            } break
            case 'videos': {
                var max_item = _product_constants.VALUES.ProductDetail_Max_Avt
                if ((element.closest('.flex-lg-nowrap').find('.magnific_popup').length - 1 )>= max_item) {
                    _msgalert.error('Số lượng video vượt quá giới hạn')
                    element.val(null)
                }
                else {
                    if ($.inArray(element.val().split('.').pop().toLowerCase(), _product_constants.VALUES.VideoExtension) == -1) {
                        _msgalert.error("Vui lòng chỉ upload các định dạng sau: " + _product_constants.VALUES.VideoExtension.join(', '));
                        return
                    }
                    if ((element.closest('.flex-lg-nowrap').find('.magnific_popup').length - 1 + element[0].files.length) > max_item) {
                        _msgalert.error('Số lượng Video vượt quá giới hạn')
                        element.val(null)
                        return
                    }
                    $(element[0].files).each(function (index, item) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            element.closest('.list').prepend(_product_constants.HTML.ProductDetail_Video_Item.replaceAll('{src}', e.target.result).replaceAll('{id}', '-1'))
                            element.closest('.items').find('.count').html(element.closest('.list').find('.magnific_popup').length)

                        }
                        reader.readAsDataURL(item);
                    });
                    element.val(null)

                }

            } break
            case 'image_row_item': {
     
                if ((element.closest('.flex-lg-nowrap').find('.magnific_popup').length - 1) >= 1) {
                    _msgalert.error('Số lượng ảnh vượt quá giới hạn')
                    element.val(null)
                }
                else {
                    if ($.inArray(element.val().split('.').pop().toLowerCase(), _product_constants.VALUES.ImageExtension) == -1) {
                        _msgalert.error("Vui lòng chỉ upload các định dạng sau: " + _product_constants.VALUES.ImageExtension.join(', '));
                        return
                    }
                    if ((element.closest('.flex-lg-nowrap').find('.magnific_popup').length - 1 + element[0].files.length) > max_item) {
                        _msgalert.error('Số lượng ảnh vượt quá giới hạn')
                        element.val(null)
                        return
                    }
                    $(element[0].files).each(function (index, item) {

                        var reader = new FileReader();
                        reader.onload = function (e) {
                            element.closest('.list').prepend(_product_constants.HTML.ProductDetail_Images_Item.replaceAll('{src}', e.target.result).replaceAll('{id}', '-1'))
            
                        }
                        reader.readAsDataURL(item);
                    });
                    element.val(null)

                }
            } break
        }



    },
    Delete: function () {
        var _id = $('#product_detail').val() == undefined || $('#product_detail').val().trim() == '' ? null : $('#product_detail').val()
        _product_function.POST('/Product/DeleteProductByID', { product_id: _id }, function (result) {
            if (result.is_success) {
                _msgalert.success('Xóa sản phẩm thành công')
                window.location.href = '/product';

            }
        });
    },
    Summit: function () {
        var validate = product_detail.ValidateProduct()
        if (validate == false) {
            return
        }
        _global_function.AddLoading();
        var model = {
            _id: $('#product_detail').val() == undefined || $('#product_detail').val().trim() == '' ? null : $('#product_detail').val(),
            status: $('#product_detail').attr('data-status') == undefined || $('#product_detail').attr('data-status').trim() == '' ? null : $('#product_detail').attr('data-status'),
            code: $('#product_detail').attr('data-code') == undefined || $('#product_detail').attr('data-code').trim() == '' ? null : $('#product_detail').attr('data-code'),
            price: $('#main-price input').val() == undefined || $('#main-price input').val().trim() == '' ? 0 : parseFloat($('#main-price input').val().replaceAll(',', '')),
            profit: $('#main-profit input').val() == undefined || $('#main-profit input').val().trim() == '' ? 0 : parseFloat($('#main-profit input').val().replaceAll(',', '')),
            amount: $('#main-amount input').val() == undefined || $('#main-amount input').val().trim() == '' ? 0 : parseFloat($('#main-amount input').val().replaceAll(',', '')),
            discount: 0,
            quanity_of_stock: $('#main-stock input').val() == undefined || $('#main-stock input').val().trim() == '' ? 0 : parseInt($('#main-stock input').val().replaceAll(',', ''))
        }
        model.images = []
        $('#images .list .items').each(function (index, item) {
            var element_image = $(this)
            if (element_image.find('img').length > 0) {
                //model.images.push(element_image.find('img').attr('src'))
                var data_src = element_image.find('img').attr('src')
                if (data_src == null || data_src == undefined || data_src.trim() == '') return true
                if (_product_function.CheckIfImageVideoIsLocal(data_src)) {
                    var result = _product_function.POSTSynchorus('/Product/SummitImages', { data_image: data_src })
                    if (result != undefined && result.data != undefined && result.data.trim() != '') {
                        model.images.push(result.data)
                    } else {
                        model.images.push(data_src)
                    }
                }
                else {
                    model.images.push(data_src)
                }
            }
        })
        model.avatar = $('#avatar .list .items').first().find('img').attr('src')
        if (_product_function.CheckIfImageVideoIsLocal(model.avatar)) {
            var result = _product_function.POSTSynchorus('/Product/SummitImages', { data_image: model.avatar })
            if (result != undefined && result.data != undefined && result.data.trim() != '') {
                model.avatar = result.data
            }
        }
        var result = _product_function.POSTSynchorus('/Files/SummitImages', { data_image: $('#avatar .list .items').first().find('img').attr('src') })
        if (result != undefined && result.data != undefined && result.data.trim() != '') {
            model.avatar = result.data
        } else {
            model.avatar = $('#avatar .list .items').first().find('img').attr('src')
        }
      
        model.videos = []
        $('#videos .items .magnific_thumb').each(function (index, item) {
            var element_image = $(this)
            //model.videos.push(element_image.find('video').find('source').attr('src'))
            var data_src = element_image.find('video').find('source').attr('src')
            if (data_src == null || data_src == undefined || data_src.trim() == '') return true
            if (_product_function.CheckIfImageVideoIsLocal(data_src)) {
                const byteCharacters = atob(data_src.split('base64,')[1]);
                const byteNumbers = new Array(byteCharacters.length);
                for (let i = 0; i < byteCharacters.length; i++) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                const byteArray = new Uint8Array(byteNumbers);
                const blob = new Blob([byteArray], { type: "video/mp4" });

                //// Create a FormData object to send via AJAX
                var formData = new FormData();
                formData.append('request', blob, 'video.mp4'); // Append the Blob as a file

                var result = _product_function.POSTFileSynchorus('/Files/SummitVideo', formData)

                if (result != undefined && result.data != undefined && result.data.trim() != '') {
                    model.videos.push(result.data)
                }
            }

        })
        model.name = $('#product-name input').val()
        model.group_product_id = $('#group-id input').attr('data-id')
        model.description = $('#description textarea').val()
        model.specification = []
        $('#specifications .namesp').each(function (index, item) {
            var element = $(this)
            
            model.specification.push({
                _id: '-1',
                attribute_id: element.attr('data-attr-id'),
                value_type: element.attr('data-type'),
                value: element.find('input').val(),
                type_ids: element.find('input').attr('data-value'),
            })

        })



        model.discount_group_buy = []
        $('#discount-groupbuy tbody .discount-groupbuy-row').each(function (index, item) {
            var element = $(this)
            var from = element.find('.quanity-from').find('input').val() == undefined || element.find('.quanity-from').find('input').val().trim() == '' ? 0 : parseInt(element.find('.quanity-from').find('input').val().replaceAll(',', ''))
            var to = element.find('.quanity-to').find('input').val() == undefined || element.find('.quanity-to').find('input').val().trim() == '' ? 0 : parseInt(element.find('.quanity-to').find('input').val().replaceAll(',', ''))
            var to = element.find('.quanity-to').find('input').val() == undefined || element.find('.quanity-to').find('input').val().trim() == '' ? 0 : parseInt(element.find('.quanity-to').find('input').val().replaceAll(',', ''))
            var checkbox_value = element.find('input[name="discount-type-' + (index + 1) + '"]:checked').val()

            var discount = 0
            switch (checkbox_value) {
                case '0': {
                    discount = parseFloat(element.find('.discount-number').find('input').val().replaceAll(',', ''))
                } break
                case '1': {
                    discount = parseFloat(element.find('.discount-percent').find('input').val().replaceAll(',', ''))
                    if (discount > 100) {
                        _msgalert.error("Chiết khấu tối đa 100%")
                    }
                    return false
                } break
            }
            model.discount_group_buy.push({
                from: from,
                to: to,
                discount: discount,
                type: parseInt(checkbox_value)
            })
        })

        model.variations = []
        if (!$('#product-attributes-price').closest('.item-edit').is(':hidden')) {
            var attribute_model = product_detail.GetAttributeItem()
            model.attributes = attribute_model.attributes
            model.attributes_detail = attribute_model.attributes_detail
            $('#product-attributes-price tbody tr').each(function (index, index) {
                var element = $(this)
                var var_id = element.attr('data-id')
                if (var_id == undefined) var_id = ''
                var price = parseFloat(element.find('.td-price').find('input').val().replaceAll(',', ''))
                var profit = parseFloat(element.find('.td-profit').find('input').val().replaceAll(',', ''))
                var amount = parseFloat(element.find('.td-amount').find('input').val().replaceAll(',', ''))
                var quanity_of_stock = parseFloat(element.find('.td-stock').find('input').val().replaceAll(',', ''))
                var variation = {
                    _id: var_id,
                    variation_attributes: [],
                    price: (price == undefined || isNaN(price) || price <= 0) ? null : price,
                    profit: (profit == undefined || isNaN(profit) || profit <= 0) ? null : profit,
                    amount: (amount == undefined || isNaN(amount) || amount <= 0) ? null : amount,
                    quanity_of_stock: (quanity_of_stock == undefined || isNaN(quanity_of_stock) || quanity_of_stock <= 0) ? null : quanity_of_stock,
                    sku: element.find('.td-sku').find('input').val(),
                }
                for (var i = 0; i < $('.product-attributes').length; i++) {
                    var attr_value = element.attr('data-attribute-' + i)

                    variation.variation_attributes.push({
                        id: i,
                        name: attr_value
                    })
                }
                model.variations.push(variation)
            })

        }


        //model.preorder_status = $('input[name="preorder_status"]:checked').val() == '1' ? 1 : 0
        //model.condition_of_product = $('#condition_of_product').find(':selected').val()
        //model.sku = $('#sku input').val()

        _product_function.POST('/Product/Summit', { request: model }, function (result) {
            if (result.is_success) {
                _global_function.RemoveLoading()
                _msgalert.success(result.msg)
                setTimeout(function () {
                    window.location.href = '/product';
                }, 2000);
            }
            else {
                _global_function.RemoveLoading()

                _msgalert.error(result.msg)

            }
        });

    },
    GetLevelOfAttributesBox: function (element) {
        var result = 0
        $('.product-attributes').each(function (index, item) {
            if ($(this).is(element)) {
                result = index
                return false
            }
        })
        return result
    },
    Validate: function () {
        var validated = true
        if (va)

            return validated
    },
    AddProductAttributes: function () {
        var attribute_max_count = 2
       
        $('.attributes-name').each(function (index, item) {
            
            if ($('.attributes-name-' + index_attributes) .length > 0)
                index_attributes++
        })
        if ($('.product-attributes').length < attribute_max_count) {
            $('#product-attributes-box').append(_product_constants.HTML.ProductDetail_Attribute_Row.replaceAll('{html}', _product_constants.HTML.ProductDetail_Attribute_Row_Item.replaceAll("{index}", index_attributes)))
            //if ($('.product-attributes').length < attribute_max_count) {
            //    $('#product-attributes-box').append(_product_constants.HTML.ProductDetail_Attribute_Row_Add_Attributes)
            //}
        }
        if ($('.product-attributes').length < attribute_max_count ) {
            $('.btn-add-attributes').show()
        }
        else {
            $('.btn-add-attributes').hide()
        }

    },
    ValidateProduct: function () {
        var success = true;
        var value = $('#product-name input').val()
        var description_textarea = $('#description textarea').val()
        var group_id_input = $('#group-id input').val()
        var main_profit_input = $('#main-profit input').val()
        var main_price_input = $('#main-price input').val()
        //-- product-name:
        if (value == undefined || value.trim() == '') {
            _msgalert.error('Tên sản phẩm không được bỏ trống')
            success = false
        } else if (value.length > 120) {
            _msgalert.error('Tên sản phẩm không được quá 120 ký tự')
            success = false
        }
        if (description_textarea == undefined || description_textarea.trim() == '') {
            _msgalert.error('Mô tả sản phẩm không được bỏ trống')
            success = false
        } 
        if(group_id_input == undefined || group_id_input.trim() == '') {
            _msgalert.error('Ngành hàng không được bỏ trống')
            success = false
        }
        if (main_profit_input == undefined || group_id_input.trim() == '') {
            _msgalert.error('Lợi nhuận không được bỏ trống')
            success = false
        }
        if (main_price_input == undefined || group_id_input.trim() == '') {
            _msgalert.error('Giá nhập không được bỏ trống')
            success = false
        }
        if (!success) return success
        //-- images:
        var max_item = _product_constants.VALUES.ProductDetail_Max_Image
        if ($('#images .flex-lg-nowrap .magnific_popup').length >= max_item) {
            _msgalert.error('Số lượng ảnh vượt quá giới hạn')
            success = false
        } else if ($('#images .magnific_popup').length ==0) {
            _msgalert.error('Chưa có ảnh sản phẩm')
            success = false
        }
        if (!success) return success
        //-- avt
        max_item = _product_constants.VALUES.ProductDetail_Max_Avt
        if ($('#avatar .flex-lg-nowrap .magnific_popup').length >= max_item) {
            _msgalert.error('Số lượng ảnh đại diện vượt quá giới hạn')
            success = false
        }
        else if ($('#avatar .magnific_popup').length == 0) {
            _msgalert.error('Chưa có ảnh đại diện sản phẩm')
            success = false
        }
        if (!success) return success
        //-- videos
        max_item = _product_constants.VALUES.ProductDetail_Max_Avt
        if ($('#videos .flex-lg-nowrap .magnific_popup').length >= max_item) {
            _msgalert.error('Số lượng video vượt quá giới hạn')
            success = false
        }
        if (!success) return success


        return success
    },
    ValidateAttributesInput: function (element) {
        var success = true;
        element.closest('.row-attributes-value').find('.attributes-name').each(function (index, item) {
            var input_element = $(this)
            if (input_element.val() == undefined || input_element.val().trim() == '') {
                if (success) success = false
                input_element.closest('.relative').find('.error').show()
                input_element.closest('.relative').find('.error').html('Không được để trống ô')
            }
        })
        return success
    },
    RenderSelectedGroupProduct: function () {
        var html_selected_input = ''
        var group_selected = ''
        $('#them-nganhhang .col-md-4').each(function (index, item) {
            var element = $(this)
            var selected = element.find('ul').find('.active').attr('data-name')
            if (element.find('ul').find('.active').attr('data-id') == undefined) return true
            if (index >= ($('#them-nganhhang .col-md-4').length - 1)) {
                html_selected_input += selected
                group_selected += element.find('ul').find('.active').attr('data-id')
            } else {

                html_selected_input += selected + ' > '
                group_selected += element.find('ul').find('.active').attr('data-id') + ','

            }
        })
        $('#group-id input').val(html_selected_input)
        $('#group-id input').attr('data-id', group_selected)

    },
    RenderRowAttributeTablePrice: function () {
        var html = ''
        var product_attributes = []
        var product_attributes3 = []
        var attributes_name = []
        $('.product-attributes').each(function (index, item) {
            var element = $(this)
            var product_attribute_by_id = {
                id: index,
                data_values: [],
            }
            if (index == 1) {
                element.find('.attributes-name').each(function (index, item) {
                    var element_input = $(this)
                    var product_attribute_by_id2 = {
                        id: index,
                        data_values: [],
                    }
                    if (element_input != undefined && element_input.val() != undefined && element_input.val().trim() != '') {
                        product_attribute_by_id2.data_values.push(element_input.val())
                        if (product_attribute_by_id2.data_values.length > 0)
                            product_attributes3.push(product_attribute_by_id2)
                    }

                })
            }

            element.find('.attributes-name').each(function (index, item) {
                var element_input = $(this)
                var product_attribute_by_dataid = {
                    dataid: 0,
                    data_values: "",
                }
                if (element_input != undefined && element_input.val() != undefined && element_input.val().trim() != '') {
                    product_attribute_by_id.data_values.push(element_input.val())

                    product_attribute_by_dataid.dataid = element_input.attr('data-id')
                    product_attribute_by_dataid.data_values = element_input.val()
                    attributes_name.push(product_attribute_by_dataid)
                }

            })
            if (product_attribute_by_id.data_values.length > 0)
            product_attributes.push(product_attribute_by_id)
        })
        var combination_array = []
        if (product_attributes.length > 0) {
            combination_array = product_attributes[0].data_values.map(v => [].concat(v))
            if (product_attributes.length > 1) {
                for (var i = 1; i < product_attributes.length; i++) {
                    var array2 = product_attributes[i].data_values;
                    combination_array = combination_array.flatMap(d => array2.map(v => [].concat(d, v)))

                }
            }
        }
        
        if (combination_array.length > 0) {
            var name = combination_array[0][0]
            var model = product_detail.GetAttributeItem()
            $(combination_array).each(function (index, item) {
                var html_attribute_attr = ''
                var html_td_attribute = ''

                $(item).each(function (index_attribute, attribute_name) {
                    var row_span = model.attributes_detail.filter(obj => {
                        return obj.attribute_id == index_attribute
                    }).length
                    if (row_span <= 0) row_span = 1
                    var data_id = 0;
                    $(attributes_name).each(function (index, data_item) {
                        if (data_item.data_values.trim() == attribute_name.trim())
                            data_id = data_item.dataid
                    })
                    var html_item = ''
                    if (row_span < 2) {
                        html_item = _product_constants.HTML.ProductDetail_Attribute_Price_Tr_Td
                            .replaceAll('{i}', index_attribute)
                            .replaceAll('{class-name}', _global_function.RemoveSpecialCharacter_ky_tu_DB(attribute_name.trim()).replaceAll(' ', '-'))
                            .replaceAll('{name}', attribute_name.trim())
                            .replaceAll('{row_span}', 'rowspan="' + row_span + '"')
                            .replaceAll('{data-id}', data_id)
                    } else {

                        html_item = _product_constants.HTML.ProductDetail_Attribute_Price_Tr_Td
                            .replaceAll('{i}', index_attribute)
                            .replaceAll('{class-name}', _global_function.RemoveSpecialCharacter_ky_tu_DB(attribute_name.trim()).replaceAll(' ', '-'))
                            .replaceAll('{name}', attribute_name.trim())
                            .replaceAll('{row_span}', 'rowspan="' + row_span + '"')
                            .replaceAll('{data-id}', data_id)
                    }
                    html_attribute_attr += 'data-attribute-' + index_attribute + '="' + attribute_name.trim() + '" '
                    html_td_attribute += html_item;

                })




                if (item[0].toLowerCase().trim() == name && html.trim()!='') {
                    html += _product_constants.HTML.ProductDetail_Attribute_Price_TrSub
                        .replaceAll('data-attribute-0="Phân loại 1" data-attribute-1="Phân loại 2-2"', html_attribute_attr)
                        .replaceAll('{td_arrtibute}', html_td_attribute)
                }
                else {
                    name = item[0].toLowerCase().trim()
                    html += _product_constants.HTML.ProductDetail_Attribute_Price_TrMain
                        .replaceAll('data-attribute-0="Phân loại 1" data-attribute-1="Phân loại 2-1"', html_attribute_attr)
                        .replaceAll('{td_arrtibute}', html_td_attribute)
                }

            })

        }
        $('#product-attributes-price tbody').html(html)
        $('#product-attributes-price .th-attributes').hide()
        for (var i = 0; i < $('.product-attributes').length; i++) {
            $('#product-attributes-price .th-attribute-' + i).show()
            $('#product-attributes-price .th-attribute-' + i).html($($('.product-attributes')[i]).find('h6').find('b').text())
            if (product_attributes3.length == 0) {
                $('#product-attributes-price .th-attribute-2').hide()
            }
        }
        //var list_filter = GetAttributeList(0)
        //$(list_filter).each(function (index, item) {
        //    HideAttributes(item, 0)
        //})
        product_detail.HideProductAttributeCell()
    },

    ApplyAllPriceToTable: function () {
        $('#product-attributes-price .td-price input').val(Comma($('#product-attributes-all-price .td-price input').val()))
        $('#product-attributes-price .td-profit input').val(Comma($('#product-attributes-all-price .td-profit input').val()))
        $('#product-attributes-price .td-stock input').val(Comma($('#product-attributes-all-price .td-stock input').val()))
        $('#product-attributes-price .td-sku input').val($('#product-attributes-all-price .td-sku input').val())
        var price = isNaN(parseFloat($('#product-attributes-all-price .td-price input').val().replaceAll(',', ''))) ? 0 : parseFloat($('#product-attributes-all-price .td-price input').val().replaceAll(',', ''))
        var profit = isNaN(parseFloat($('#product-attributes-all-price .td-profit input').val().replaceAll(',', ''))) ? 0 : parseFloat($('#product-attributes-all-price .td-profit input').val().replaceAll(',', ''))
        $('#product-attributes-price .td-amount input').val(Comma(price + profit))
        //$('#product-attributes-all-price .td-price input').val('')
        //$('#product-attributes-all-price .td-profit input').val('')
        //$('#product-attributes-all-price .td-stock input').val('')
        //$('#product-attributes-all-price .td-sku input').val('')
    },
    UpdateRowData: function (tr) {
        if (tr.find('.td-price').length > 0 && tr.find('.td-profit').length > 0 && tr.find('.td-amount').length > 0) {
            var price = isNaN(parseFloat(tr.find('.td-price').find('input').val().replaceAll(',', ''))) ? 0 : parseFloat(tr.find('.td-price').find('input').val().replaceAll(',', ''))
            var profit = isNaN(parseFloat(tr.find('.td-profit').find('input').val().replaceAll(',', ''))) ? 0 : parseFloat(tr.find('.td-profit').find('input').val().replaceAll(',', ''))

            tr.find('.td-amount').find('input').val(Comma(price + profit))
        }

    },
    GetAttributeItem: function () {
        var model = {

        }
        model.attributes = []
        $('.product-attributes').each(function (index, item) {
            var element = $(this)
         
            model.attributes.push({
                _id: index,
                name: element.find('h6').find('b').html(),
            })

        })
        model.attributes_detail = []
        $('#product-attributes-box .attributes-name').each(function (index_2, item_2) {
            var element = $(this)
            var img_src = element.closest('.col-md-6 ').find('#image_row_item').find('img').attr('src')
            if (img_src != undefined && _product_function.CheckIfImageVideoIsLocal(img_src)) {
                var result = _product_function.POSTSynchorus('/Product/SummitImages', { data_image: img_src })
                if (result != undefined && result.data != undefined && result.data.trim() != '') {
                    img_src = result.data
                }
            }
            var value = element.val()
            if (value != undefined && value.trim() != '') {
                model.attributes_detail.push({
                    attribute_id: product_detail.GetLevelOfAttributesBox(element.closest('.product-attributes')),
                    img: img_src == undefined ? '' : img_src,
                    name: element.val()
                })
            }
        })
        return model
    },
    AddRowAttributeTablePrice: function (id) {
        var html = ''
        var product_attributes = []
        var product_attributes2 = []
        var product_attributes3 = []
        var attributes_name = []
        var text = $('.attributes-name-' + id).val();
        var level = -1
        var parent_product_element = $('.attributes-name-' + id).closest('.product-attributes')
        $('.product-attributes').each(function (index, item) {
            var element = $(this)
            var product_attribute_by_id = {
                id: index,
                data_values: [],
            }
            if (index == 0) {
                var product_attribute_by_id2 = {
                    id: index,
                    data_values: [],
                }
                element.find('.attributes-name').each(function (index, item) {
                    var element_input = $(this)
                    if (element_input != undefined && element_input.val() != undefined && element_input.val().trim() != '') {
                        product_attribute_by_id2.data_values.push(element_input.val())
                    }

                })
                if (product_attribute_by_id2.data_values.length > 0)
                    product_attributes2.push(product_attribute_by_id2)
            } else {
                var product_attribute_by_id2 = {
                    id: index,
                    data_values: [],
                }
                element.find('.attributes-name').each(function (index, item) {
                    var element_input = $(this)
                    if (element_input != undefined && element_input.val() != undefined && element_input.val().trim() != '') {
                        product_attribute_by_id2.data_values.push(element_input.val())
                    }

                })
                if (product_attribute_by_id2.data_values.length > 0)
                    product_attributes3.push(product_attribute_by_id2)
            }
            if (parent_product_element[0] == element[0]) {
                level = index
            }
            element.find('.attributes-name').each(function (index, item) {
                var element_input = $(this)
                var product_attribute_by_dataid = {
                    dataid: 0,
                    data_values: "",
                }
                if (element_input != undefined && element_input.val() != undefined && element_input.val().trim() != '' && text.trim() == element_input.val().trim()) {
                    product_attribute_by_id.data_values.push(element_input.val())

                    product_attribute_by_dataid.dataid = element_input.attr('data-id')
                    product_attribute_by_dataid.data_values = element_input.val()
                    attributes_name.push(product_attribute_by_dataid)
                }

            })
            if (product_attribute_by_id.data_values.length > 0)
                product_attributes.push(product_attribute_by_id)
            
        })
        if (product_attributes3.length > 0 && product_attributes3[0].data_values.length == 1) {
            product_detail.RenderRowAttributeTablePrice()
            return true;
        }
        if (product_attributes.length > 0 && product_attributes3.length > 0 && product_attributes3[0].id != product_attributes[0].id) {
            product_attributes.push(product_attributes3[0])
        }
        var combination_array = []
        if (product_attributes.length > 0) {
            combination_array = product_attributes[0].data_values.map(v => [].concat(v))
            if (product_attributes.length > 1) {
                for (var i = 1; i < product_attributes.length; i++) {
                    var array2 = product_attributes[i].data_values;
                    combination_array = combination_array.flatMap(d => array2.map(v => [].concat(d, v)))

                }
            }
        }
        var combination_array2 = []
        if (product_attributes2.length > 0) {
            combination_array2 = product_attributes2[0].data_values.map(v => [].concat(v))
            if (product_attributes2.length > 1) {
                for (var i = 1; i < product_attributes2.length; i++) {
                    var array2 = product_attributes2[i].data_values;
                    combination_array2 = combination_array2.flatMap(d => array2.map(v => [].concat(d, v)))

                }
            }
        }
        if (combination_array.length > 0) {
            var name = combination_array[0][0]
            var model = product_detail.GetAttributeItem()
            $(combination_array).each(function (index, item) {
                var html_attribute_attr = ''
                var html_td_attribute = ''

                $(item).each(function (index_attribute, attribute_name) {
                    var row_span = model.attributes_detail.filter(obj => {
                        return obj.attribute_id == (index_attribute + 1)
                    }).length
                    if (row_span <= 0) row_span = 1
                    var data_id = 0;
                    $(attributes_name).each(function (index, data_item) {
                        if (data_item.data_values.trim() == attribute_name.trim())
                            data_id = data_item.dataid
                    })
                    var html_item = ''
                    if (product_attributes2.length > 0 && product_attributes2[0].id != product_attributes[0].id) {
                        if (row_span > 1) {
                            html_item = _product_constants.HTML.ProductDetail_Attribute_Price_Tr_Td_hide
                                .replaceAll('{i}', 0)
                                .replaceAll('{row_span}', 'rowspan="' + 1 + '"')
                            
                            html_item += _product_constants.HTML.ProductDetail_Attribute_Price_Tr_Td
                                .replaceAll('{i}', 1)
                                .replaceAll('{class-name}', _global_function.RemoveSpecialCharacter_ky_tu_DB(attribute_name.trim()).replaceAll(' ', '-'))
                                .replaceAll('{data_attribute}', attribute_name.trim())
                                .replaceAll('{name}', attribute_name.trim())
                                .replaceAll('{row_span}', 'rowspan="' + 1 + '"')
                                .replaceAll('{data-id}', data_id)
                        } else {

                            html_item = _product_constants.HTML.ProductDetail_Attribute_Price_Tr_Td
                                .replaceAll('{i}', 0)
                                .replaceAll('{class-name}', _global_function.RemoveSpecialCharacter_ky_tu_DB(attribute_name.trim()).replaceAll(' ', '-'))
                                .replaceAll('{data_attribute}', attribute_name.trim())
                                .replaceAll('{name}', attribute_name.trim())
                                .replaceAll('{row_span}', 'rowspan="' + 1 + '"')
                                .replaceAll('{data-id}', data_id)
                        }

                        html_attribute_attr += ' data-attribute-0="{data_attribute}" data-attribute-1="' + attribute_name.trim() + '" '
                        html_td_attribute += html_item;
                    }
                    else
                    {
                        if (row_span > 1) {
                            html_item = _product_constants.HTML.ProductDetail_Attribute_Price_Tr_Td
                                .replaceAll('{i}', index_attribute)
                                .replaceAll('{class-name}', _global_function.RemoveSpecialCharacter_ky_tu_DB(attribute_name.trim()).replaceAll(' ', '-'))
                                .replaceAll('{data_attribute}', attribute_name.trim())
                                .replaceAll('{name}', attribute_name.trim())
                                .replaceAll('{row_span}', 'rowspan="' + row_span + '"')
                                .replaceAll('{data-id}', data_id)
                            html_attribute_attr += 'data-attribute-' + index_attribute + '="' + attribute_name.trim() + '" '
                            html_td_attribute += html_item;
                        } else {

                            html_item = _product_constants.HTML.ProductDetail_Attribute_Price_Tr_Td
                                .replaceAll('{i}', index_attribute)
                                .replaceAll('{class-name}', _global_function.RemoveSpecialCharacter_ky_tu_DB(attribute_name.trim()).replaceAll(' ', '-'))
                                .replaceAll('{data_attribute}', attribute_name.trim())
                                .replaceAll('{name}', attribute_name.trim())
                                .replaceAll('{row_span}', 'rowspan="' + row_span + '"')
                                .replaceAll('{data-id}', data_id)
                            html_attribute_attr += ' data-attribute-0="' + item[0]+'" data-attribute-' + index_attribute + '="' + attribute_name.trim() + '" '
                            html_td_attribute += html_item;
                        }
                      
                    }

                })
                if (product_attributes[0].id > 0 && level > 0) {
                    html += _product_constants.HTML.ProductDetail_Attribute_Price_TrSub
                        .replaceAll('data-attribute-0="Phân loại 1" data-attribute-1="Phân loại 2-2"', html_attribute_attr)
                        .replaceAll('{td_arrtibute}', html_td_attribute)
                }
                else {
                    if (item[0].toLowerCase().trim() == name && level > 0) {
                        html += _product_constants.HTML.ProductDetail_Attribute_Price_TrSub
                            .replaceAll('data-attribute-0="Phân loại 1" data-attribute-1="Phân loại 2-2"', html_attribute_attr)
                            .replaceAll('{td_arrtibute}', html_td_attribute)
                    } else {
                        level++;
                        name = item[0].toLowerCase().trim()
                        html += _product_constants.HTML.ProductDetail_Attribute_Price_TrMain
                            .replaceAll('data-attribute-0="Phân loại 1" data-attribute-1="Phân loại 2-1"', html_attribute_attr)
                            .replaceAll('{td_arrtibute}', html_td_attribute)
                    }
               
                }

            })

        }
        if (product_attributes.length > 0 && product_attributes2.length > 0 && product_attributes2[0].id != product_attributes[0].id) {
            if (combination_array2.length > 0) {
                var name = combination_array2[0][0]

                $(combination_array2).each(function (index, item) {
                    var last_element = null;
                    var dem = 0;
                    var htmlrow=''
                    $(item).each(function (index_attribute, attribute_name) {
                        var attributeid = 0;
                        $('.row-attributes-value').find('.col-md-6').find('.attributes-name').each(function (index, item) {
                            var element_attributes_name = $(this)
                            if (element_attributes_name.val().trim() == attribute_name.trim()) {
                                attributeid = element_attributes_name.attr('data-id')
                            }
                        })
                        
                 
                        var rowspan = $('.' + _global_function.RemoveSpecialCharacter_ky_tu_DB(attribute_name.trim()).replaceAll(' ', '-')).attr('rowspan')
                        $('.' + _global_function.RemoveSpecialCharacter_ky_tu_DB(attribute_name.trim()).replaceAll(' ', '-')).attr('rowspan', parseFloat(rowspan) + 1)
                        $('.tr-sub').each(function (index_td, item_td) {
                            var element = $(this)
                            var attr_value = element.attr('data-attribute-0')
                            if (attr_value.trim() == attribute_name.trim()) {
                                last_element = element;
                                htmlrow = html.replaceAll('{class-name}', _global_function.RemoveSpecialCharacter_ky_tu_DB(attribute_name.trim()).replaceAll(' ', '-')).replaceAll('{name}', attribute_name.trim()).replaceAll('{data-id}', attributeid).replaceAll('{data_attribute}', attribute_name.trim());
                                dem++;
                            }
                        })
                        if (dem == 0) {
                            $('.tr-main').each(function (index_td, item_td) {
                                var element = $(this)
                                var attr_value = element.attr('data-attribute-0')
                                if (attr_value.trim() == attribute_name.trim()) {
                                    last_element = element
                                    htmlrow = html.replaceAll('{class-name}', _global_function.RemoveSpecialCharacter_ky_tu_DB(attribute_name.trim()).replaceAll(' ', '-')).replaceAll('{name}', attribute_name.trim()).replaceAll('{data-id}', attributeid).replaceAll('{data_attribute}', attribute_name.trim());
                                    dem++;
                                }
                            })
                        }
                    })
                    $(htmlrow).insertAfter(last_element)

                })
            }
        } else {
            $('#product-attributes-price tbody').append(html)
        }

        $('#product-attributes-price .th-attributes').hide()
        for (var i = 0; i < $('.product-attributes').length; i++) {
            $('#product-attributes-price .th-attribute-' + i).show()
            $('#product-attributes-price .th-attribute-' + i).html($($('.product-attributes')[i]).find('h6').find('b').text())
            if (product_attributes3.length == 0 ) {
                $('#product-attributes-price .th-attribute-1').hide()
            }
        }
        //var list_filter = GetAttributeList(1)
        //$(list_filter).each(function (index, item) {
        //    HideAttributes(item, 1)
        //})
        product_detail.HideProductAttributeCell()

    },
    DeleteRowAttributeTablePrice: function (attribute_name, id) {
        var changed_attr_0=[]
        $('#product-attributes-price tbody tr').each(function (index_td, item_td) {
            var element = $(this)

            var attr_by_id = element.attr('data-attribute-' + id)
            if (attr_by_id.trim() == attribute_name.trim()) {
                if (element.hasClass('tr-main') && !changed_attr_0.includes(element.attr('data-attribute-0').trim()) ) {
                    changed_attr_0.push( element.attr('data-attribute-0').trim())
                   
                }
                element.remove()
            }
        })
        $(changed_attr_0).each(function (index, attribute_0) {
            $('#product-attributes-price tbody .tr-sub').each(function (index_td, item_td) {
                var element_sub = $(this)
                var sub_attr_by_id = element_sub.attr('data-attribute-0')
                if (sub_attr_by_id.trim() == attribute_0) {
                    element_sub.removeClass('tr-sub')
                    element_sub.addClass('tr-main')
                    return false
                }
            })
        })
       
        $('#product-attributes-price tbody .td-attribute-0').show()
        $('#product-attributes-price tbody .td-attribute-1').show()
        product_detail.HideProductAttributeCell()
    },
    HideProductAttributeCell: function () {
        var list_level_0 = []
        var list_level_1 = []
        $($('#product-attributes-box .row-attributes-value')[0]).find('input[type="text"]').each(function (index, item) {
            var element = $(this)
            if (element.val() != undefined && element.val().trim()!='')
            list_level_0.push(element.val())
        })
        if ($($('#product-attributes-box .row-attributes-value')[1]).is(":visible") && $($('#product-attributes-box .row-attributes-value')[1]).length > 0) {
            $($('#product-attributes-box .row-attributes-value')[1]).find('input[type="text"]').each(function (index, item) {
                var element = $(this)
                if (element.val() != undefined && element.val().trim() != '')
                list_level_1.push(element.val())
            })
        }
        
        $('#product-attributes-price tbody tr').each(function (index_td, item_td) {
            var element = $(this)
            if (element.hasClass('tr-main')) {
                element.find('.td-attribute-0').show()
                element.find('.td-attribute-0').attr('rowspan', (list_level_1.length <= 1 ? 1 : list_level_1.length))
                element.find('.td-attribute-1').attr('rowspan', 1)
            }
            else if (element.hasClass('tr-sub')) {
                element.find('.td-attribute-0').hide()
                element.find('.td-attribute-1').attr('rowspan', 1)
            }
            if ($('#product-attributes-box .row-attributes-value').length > 1) {
                element.find('.td-attribute-1').show()
            } else {
                element.find('.td-attribute-1').hide()
            }
        })

    }

}
function SingleDatePicker(element, dropdown_position = 'down') {
    var today = new Date();
    var yyyy = today.getFullYear();
    var mm = today.getMonth() + 1; // Months start at 0!
    var dd = today.getDate();
    var yyyy_max = yyyy + 10;
    if (dd < 10) dd = '0' + dd;
    if (mm < 10) mm = '0' + mm;
    var time_now = dd + '/' + mm + '/' + yyyy;
    var max_range = '31/12/' + yyyy_max;

    element.daterangepicker({
        singleDatePicker: true,
        autoApply: true,
        showDropdowns: true,
        drops: dropdown_position,
        minDate: time_now,
        maxDate: max_range,
        locale: {
            format: 'DD/MM/YYYY'
        }
    }, function (start, end, label) {


    });
    if (element.val() != undefined && element.val().trim() != '') {
        element.data('daterangepicker').setStartDate();
    }

}
//function HideAttributes(name, i) {
//    var find = $('#product-attributes-price tbody tr[data-attribute-' + i + '="' + name + '"]')
//    if (find.length <0 ) return;
//    var first = true;

//    find.each(function (index, item) {
//        var element = $(this)
//        if (first) {
//            first = false
//        } else {
//            for (var field_index = 0; field_index <= i; field_index++) {
//                $(element.find('.td-attribute-' + field_index)).hide()
//            }
//        }

//    })
//    //var list = GetAttributeList(++i)
//    //$(list).each(function (index, item) {
//    //    HideAttributes(item,i)
//    //})
//}
//function GetAttributeList(index) {
//    var list = []
//    $($('#product-attributes-box .row-attributes-value')[index]).find('input[type="text"]').each(function (index_item, item) {
//        var element = $(this)
//        list.push(element.val())

//    })
//    return list
//}
function Comma(number) { //function to add commas to textboxes
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
}
