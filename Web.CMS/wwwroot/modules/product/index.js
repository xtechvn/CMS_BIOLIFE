
$(document).ready(function () {
    product_index.Initialization()
})
var product_index = {
    Model: {
        keyword: '',
        group_id: -1,
        page_index: 1,
        page_size: 10
    },
    Initialization: function () {
        var model = [{ url: '/', name: 'Trang chủ' }, { url: '/productv2', name: 'Quản lý sản phẩm', activated: true }]
        _global_function.RenderBreadcumb(model)
        product_index.Listing();
        product_index.DynamicBind()
        $('#product_list').closest('.table-responsive').addClass('placeholder')
        $('.hanmuc').closest('.flex-lg-nowrap').addClass('placeholder')
    },
    DynamicBind: function () {
        $('body').on('click', '.btn-search-product', function () {
            product_index.Listing();
        });
        $('body').on('click', '.btn-add-product', function () {
            window.location.href = '/product/detail'
        });

        $('body').on('click', '.product-edit, .name-product', function () {
            var element = $(this)
            var product_id = element.closest('tr').attr('data-id')
            if (product_id != null && product_id != undefined && product_id.trim() != '') {
                window.location.href = '/product/detail/' + product_id

            }
        });
    
        $('body').on('click', '.product-remove-sp', function () {
            var element = $(this)
            var product_id = element.closest('tr').attr('data-id')

            var title = 'Xác nhận ẩn sản phẩm';
            var description = 'Bạn có chắc chắn muốn ẩn sản phẩm này?';

            _msgconfirm.openDialog(title, description, function () {
                if (product_id != null && product_id != undefined && product_id.trim() != '') {
                    _product_function.POST('/Product/UpdateProductStatus', { product_id: product_id, status: 2 }, function (result) {
                        if (result.is_success) {
                            _msgalert.success(result.msg)
                            setTimeout(function () {
                                window.location.href = '/product'
                            }, 1000);
                        }
                        else {
                            _msgalert.error(result.msg)
                        }
                    });
                }

            });
           
        });
        $('body').on('click', '.product-open-sp', function () {
            var element = $(this)
            var product_id = element.closest('tr').attr('data-id')

            var title = 'Xác nhận hiển thị sản phẩm';
            var description = 'Bạn có chắc chắn muốn hiển thị sản phẩm này?';

            _msgconfirm.openDialog(title, description, function () {
                if (product_id != null && product_id != undefined && product_id.trim() != '') {
                    _product_function.POST('/Product/UpdateProductStatus', { product_id: product_id, status: 1 }, function (result) {
                        if (result.is_success) {
                            _msgalert.success(result.msg)
                            setTimeout(function () {
                                window.location.href = '/product'
                            }, 1000);
                        }
                        else {
                            _msgalert.error(result.msg)
                        }
                    });
                }

            });
           
        });
        $('body').on('click', '.product-remove-sp2', function () {
            var element = $(this)
            var product_id = element.closest('tr').attr('data-id')
            var title = 'Xác nhận xóa sản phẩm';
            var description = 'Bạn có chắc chắn muốn xóa sản phẩm này?';
            _msgconfirm.openDialog(title, description, function () {
                if (product_id != null && product_id != undefined && product_id.trim() != '') {
                    _product_function.POST('/Product/UpdateProductStatus', { product_id: product_id, status: 3 }, function (result) {
                        if (result.is_success) {
                            _msgalert.success(result.msg)
                            setTimeout(function () {
                                window.location.href = '/product'
                            }, 1000);
                        }
                        else {
                            _msgalert.error(result.msg)
                        }
                    });
                }
            });
        });
        $('body').on('click', '.product-copy-sp', function () {
            var element = $(this)
            var product_id = element.closest('tr').attr('data-id')
            var title = 'Xác nhận sao chép sản phẩm';
            var description = 'Bạn có chắc chắn muốn sao chép sản phẩm này?';
            _msgconfirm.openDialog(title, description, function () {
                if (product_id != null && product_id != undefined && product_id.trim() != '') {
                    _product_function.POST('/Product/CopyProductByID', { product_id: product_id }, function (result) {
                        if (result.is_success) {
                            _msgalert.success(result.msg)
                            setTimeout(function () {
                                window.location.href = '/product'
                            }, 1000);
                        }
                        else {
                            _msgalert.error(result.msg)
                        }
                    });
                }
            });

        });
        $('body').on('click', '.product-copy', function () {
            var element = $(this)
            var product_id = element.closest('tr').attr('data-id')
            if (product_id != null && product_id != undefined && product_id.trim() != '') {
                window.location.href = '/product/CopyProductByID/' + product_id

            }
        });
        $('body').on('click', '.xemthem', function () {
            var element = $(this)
            var data_id = element.attr('data-main-id')
            var count = 0
            $('#product_list .sub-product').each(function (index, item) {
                var element_compare = $(this)
                if (element_compare.attr('data-id') == data_id) {
                    if (count >= parseInt(element.find('nw').attr('data-count'))) return false;
                    else if (element_compare.is(':hidden')) {
                        element_compare.show()
                        count++
                    }
                }
            })
            if (parseInt(element.find('nw').attr('data-count')) - count > 0) {
                element.find('nw').html('Xem thêm (còn ' + (parseInt(element.find('nw').attr('data-count')) - count) + ' phân loại)')
            }
            else {
                element.find('nw').html('')
                element.find('.icofont-simple-down').hide()
            }
        });

    },

    Listing: function () {
        var request = {
            keyword: $('#input-search-product-name').val(),
            group_id: -1,
            page_index: product_index.Model.page_index,
            page_size: parseInt($('#item-per-page').find(':selected').val())
        }
        _product_function.POST('/Product/ProductListing', request, function (result) {
            if (result.is_success && result.data && result.data.length > 0) {
                product_index.RenderSearch(JSON.parse(result.data), JSON.parse(result.subdata))
              
                $('.hanmuc').closest('.flex-lg-nowrap').find('.count').html(JSON.parse(result.data).length)
            }
            else {
                $('#product_list').html('')

            }
            $('#product_list').closest('.table-responsive').removeClass('placeholder')
            $('.hanmuc').closest('.flex-lg-nowrap').removeClass('placeholder')
        });

    },
    RenderSearch: function (main_products, sub_products) {
        var html = ''
        main_products.sort(function (a, b) {
            // Turn your strings into dates, and then subtract them
            // to get a value that is either negative, positive, or zero.
            return new Date(b.updated_last) - new Date(a.updated_last);
        });
        $(main_products).each(function (index, item) {
            var img_src = item.avatar
            if (img_src != null && !img_src.includes(_product_constants.VALUES.StaticDomain)
                && !img_src.includes("data:image")
                && !img_src.includes("http"))
                img_src = _product_constants.VALUES.StaticDomain + item.avatar

            var html_item = _product_constants.HTML.Product
            html_item = html_item.replaceAll('{id}', item._id)
            html_item = html_item.replaceAll('{avatar}', img_src)
            html_item = html_item.replaceAll('{name}', item.name)
            html_item = html_item.replaceAll('{attribute}', '')
            var amount_html = '0'
            if (item.amount_max != undefined
                && item.amount_max != null
                && item.amount_min != undefined
                && item.amount_min != null) {
                amount_html = _product_function.Comma(item.amount_min) + ' - ' + _product_function.Comma(item.amount_max)
            }
            else if (item.amount != undefined
                && item.amount != null && item.amount > 0) {
                amount_html = _product_function.Comma(item.amount)

            }
            html_item = html_item.replaceAll('{amount}', amount_html)

            html_item = html_item.replaceAll('{stock}', _product_function.Comma(item.quanity_of_stock))

            html_item = html_item.replaceAll('{order_count}', '')
            var html_variations = ''

            var variation = sub_products.filter(obj => {
                return obj.parent_product_id.trim() == item._id
            })
            if (variation && variation.length > 0) {
                var amount = []
                var quanity_stock = []
                $(variation).each(function (index, sub_item) {
                    var html_sub_item = _product_constants.HTML.SubProduct
                        .replaceAll('{id}', item._id)
                        .replaceAll('{main_id}', item.parent_product_id)
                        .replaceAll('{name}', sub_item.name)
                        .replaceAll('{sku}', sub_item.sku == null ? "" : sub_item.sku)
                        .replaceAll('{amount}', _product_function.Comma(sub_item.amount) + ' đ')
                        .replaceAll('{stock}', _product_function.Comma(sub_item.quanity_of_stock))
                        .replaceAll('{order_count}', '')
                        .replaceAll('{display}', index > 1 ? 'display:none;' : '')
                    var html_sub_attr = ''

                    //var result = jsObjects.filter(obj => {
                    //    return obj.b === 6
                    //})
                    var sub_attr_img = []
                    $(sub_item.variation_detail).each(function (index_variation_attributes, variation_attributes_item) {
                        var attribute = sub_item.attributes.filter(obj => {
                            return obj._id == variation_attributes_item.id
                        })
                        var attribute_detail = sub_item.attributes_detail.filter(obj => {
                            return (obj.attribute_id == variation_attributes_item.id && obj.name == variation_attributes_item.name)
                        })
                        if (attribute != null && attribute.length > 0 && attribute[0].img != null && attribute[0].img != undefined && attribute[0].img.trim() != '') {
                            sub_attr_img.push(attribute[0].img)
                        }
                        if (attribute_detail != null && attribute_detail.length > 0 && attribute_detail[0].img != null && attribute_detail[0].img != undefined && attribute_detail[0].img.trim() != '') {
                            sub_attr_img.push(attribute_detail[0].img)
                        }
                        if (attribute != null && attribute.length > 0 && attribute_detail != null && attribute_detail.length > 0)
                        html_sub_attr += '' + attribute[0].name + ': ' + attribute_detail[0].name
                        if (index_variation_attributes < ($(sub_item.attributes_detail).length - 1)) {
                            html_sub_attr += '<br /> '
                        }

                    })
                    var img_src_sub = ''
                    if (sub_attr_img.length > 0) {
                        img_src_sub = sub_attr_img[0]
                        if (!img_src_sub.includes(_product_constants.VALUES.StaticDomain)
                            && !img_src_sub.includes("data:image")
                            && !img_src_sub.includes("http"))
                            img_src_sub = _product_constants.VALUES.StaticDomain + sub_attr_img[0]
                    }

                    html_sub_item = html_sub_item.replaceAll('{attribute}', 'Phân loại hàng:')
                    html_sub_item = html_sub_item.replaceAll('{attribute_detail}', html_sub_attr)
                    html_sub_item = html_sub_item.replaceAll('{avatar}', sub_attr_img.length > 0 ? img_src_sub : img_src)
                    html_variations += html_sub_item
                    amount.push(sub_item.amount)
                    quanity_stock.push(sub_item.quanity_of_stock)
                });
                if ($(variation).length > 2) {
                    html_variations += _product_constants.HTML.SubProductViewMore
                        .replaceAll('{count}', ($(variation).length - 2))
                        .replaceAll('{count_item}', ($(variation).length - 2))
                        .replaceAll('{main_id}', (item._id))

                }
                const sum_stock = quanity_stock.reduce((partialSum, a) => partialSum + a, 0);
                var max = Math.max(...amount);
                var min = Math.min(...amount);
                html_item = html_item.replaceAll('{amount}', _product_function.Comma(min) + ' đ - ' + _product_function.Comma(max) + ' đ')
                html_item = html_item.replaceAll('{stock}', _product_function.Comma(sum_stock))


            }
            html += html_item
            html += html_variations

        });
        $('#product_list').html(html)

    }

}
