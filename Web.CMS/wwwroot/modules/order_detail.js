var _order_detail_create = {
    Initialization: function () {
        _order_detail_create.ClientSuggesstion()
        _order_detail_common.Select2WithFixedOptionAndNoSearch($("#branch"))
        $('#main-staff').select2();
        $('#sub-staff').select2();

        _order_detail_create.DynamicBindClientInput();
        _order_detail_create.UserSuggesstion();
    },
    ClientSuggesstion: function () {
        $("#client-select").select2({
            ajax: {
                url: "/Contract/ClientSuggestion",
                type: "post",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        txt_search: params.term,
                    }

                    return query;
                },
                processResults: function (response) {
                    
                    var data_modified = $.map(response.data, function (obj) {
                        /*
                        if (obj.clienttype <= 0 || obj.clienttype == undefined) {
                            obj.disabled = true; // or use logical statement
                            return null;
                        }*/
                        return obj;
                    });
                    return { results: data_modified };

                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; },
            minimumInputLength: 1,
            templateResult: _order_detail_create.ClientTemplateResult,
            templateSelection: _order_detail_create.ClientTemplateSelection

        });
        
    },
    ClientTemplateResult: function (item) {
        if (item.loading) {
            return item.text;
        }
        var type_name = '';
        var color_text = '';

        if ([1, 2, 3, 4].includes(item.clienttype)) {
            type_name = 'Khách hàng B2B'
        }
        else if (item.clienttype == 5) type_name = 'Khách hàng B2C'
        else if (item.clienttype == 6) type_name = 'Saler'
        var $container = $(
            _order_detail_html.html_option_client_suggesstion.replaceAll('{if_danger}', color_text).replaceAll('{Name}', item.clientname).replaceAll('{ClientType}', type_name).replaceAll('{Email}', item.email).replaceAll('{phone}', item.phone)
        );
        return $container;

    },
    ClientTemplateSelection: function (item) {
        $.ajax({
            url: "GetActiveContractByClientId",
            type: "post",
            data: { client_id: item.id},
            success: function (result) {
                if (result != undefined && result.status == 0) {
                    $('.error_client_select').hide()
                    $('#btn_summit_order').removeAttr('disabled')
                }
                else {
                    $('.error_client_select').show()
                    $('.error_client_select_p').html(result.msg)
                    $('#btn_summit_order').attr('disabled', 'disabled');
                }

            }
        });

       return (item.clientname + ' ( ' + item.phone + ' - ' + item.email + ' )')
    },
    ClientPreOptionSuggesstion: function (item) {
        if (item.loading) {
            return item.text;
        }
        var type_name = '';
        var color_text = ''
        if (item.clienttype <= 0 || item.clienttype == undefined) {
            type_name = 'Khách hàng chưa được kích hoạt'
            color_text = 'red'
        }
        else if (item.clienttype == 5) type_name = 'Khách hàng B2C'
        else type_name = 'Khách hàng B2B'
        return _order_detail_html.html_option_client_suggesstion.replaceAll('{if_danger}', color_text).replaceAll('{Name}', item.clientname).replaceAll('{ClientType}', type_name).replaceAll('{Email}', item.email).replaceAll('{phone}', item.phone)
    },
    Summit: function () {
        let Form = $("#form-create-order-manual")
        Form.validate({
            rules: {
                "client": {
                    required: true,
                },
                "order_label": {
                    required: true,
                }
            },
            messages: {
                "client": {
                    required: "Thông tin khách hàng không được để trống",
                },
                "order_label": {
                    required: "Nhãn đơn không được để trống",
                }
            },

        });

        if ($('#client-select').find(':selected').val() == undefined || parseInt($('#client-select').find(':selected').val()) <= 0) {
            _msgalert.error("Vui lòng nhập / chọn đúng khách hàng cho đơn hàng này");
            return;
        }
        var order_label = $('#order_label').val()
        if (order_label == undefined || order_label.trim()=='') {
            _msgalert.error("Vui lòng nhập đơn nhãn cho đơn hàng này");
            return;
        }
        if (order_label.trim().length > 60) {
            _msgalert.error("Nhãn đơn của đơn hàng không được vượt quá 60 ký tự");
            debugger;
            return;
        }
        var selected_branch = $('#branch').find(":selected").val();
        if (selected_branch == undefined || parseInt(selected_branch) <= 0) {
            _msgalert.error("Vui lòng chọn đúng chi nhánh cho đơn hàng");
            return;
        }
        $('#btn_summit_order').hide();
        $('.img_loading_summit').show();

        var summit_model = {
            client_id: $('#client-select').val(),
            main_sale_id: $("#main-staff").select2("val"),
            sub_sale_id: $("#sub-staff").select2("val"),
            branch: $('#branch').find(":selected").val(),
            order_source: "CMS",
            note: $('#note').val(),
            label: $('#order_label').val()
        };
        $.ajax({
            url: "/OrderManual/CreateManualOrder",
            type: "post",
            data: summit_model,
            success: function (result) {
                $('#img_loading_summit').hide();
                if (result.status != 0) {
                    _msgalert.error(result.msg);
                    $('#btn_summit_order').show();
                    return;
                }
                _msgalert.success(result.msg);
                $('#btn_summit_order').prop("onclick", null).off("click");;
                $('#btn_summit_order').text('Tạo đơn hàng thành công');
                $('#btn_summit_order').show();
                $('.img_loading_summit').hide();

                setTimeout(function () {
                    window.location.href = "/Order/" + result.order_id;
                }, 1000);
                return;

            }
        });

    },
    DynamicBindClientInput: function () {
        $('body').on('click', '.modal-order', function (event) {
            if (!$(event.target).hasClass('modal-dialog')) {
                $(event.target).closest('.modal').removeClass('show');
                setTimeout(function () {
                    $(event.target).closest('.modal').remove();
                }, 300);
            }

        });


    },
    UserSuggesstion: function () {
        $('#main-staff').select2();
        $('#sub-staff').select2();

        $.ajax({
            url: "/CustomerManager/UserSuggestion",
            type: "post",
            data: { txt_search: "" },
            success: function (result) {
                if (result != undefined && result.data != undefined && result.data.length > 0) {
                    result.data.forEach(function (item) {
                        $('#main-staff').append(_order_detail_html.html_user_option.replaceAll('{user_id}', item.id).replace('{user_email}', item.email).replace('{user_name}', item.username).replace('{user_phone}', item.phone == undefined ? "" : ' - '+ item.phone))
                        $('#sub-staff').append(_order_detail_html.html_user_option.replaceAll('{user_id}', item.id).replace('{user_email}', item.email).replace('{user_name}', item.username).replace('{user_phone}', item.phone == undefined ? "" : ' - '+item.phone))

                    });
                    $("#main-staff").trigger('change');
                    $("#sub-staff").trigger('change');
                    $('#main-staff').val(result.selected).trigger('change');
                }
                else {
                    $("#main-staff").trigger('change');
                    $("#sub-staff").trigger('change');
                }

            }
        });
    }
}
var _order_detail_create_service = {
    Initialization: function (hotel_booking_id) {
        $('body').on('click', '.onclick-button', function (event) {
            if (!$(this).hasClass("active")) {
                $(this).addClass("active");
                $(this).next('.form-down').slideDown();
                $('.form-down input').focus();

            } else {
                $(this).removeClass("active");
                $(this).next('.form-down').slideUp();
            }
        });
    },
    ServiceHotel: function (hotel_booking_id) {
        _global_function.AddLoading()
        if (hotel_booking_id == undefined || parseInt(hotel_booking_id) < 0) return;
        if ($('#AddHotelService').length) {
            $('#AddHotelService').removeClass('show')
            setTimeout(function () {
                $('#AddHotelService').remove();
            }, 300);

        }
        $.ajax({
            url: "AddHotelService",
            type: "post",
            data: { hotel_booking_id: hotel_booking_id },
            success: function (result) {
                $('body').append(result);
                setTimeout(function () {
                    _order_detail_create_service.StopScrollingBody();
                    _order_detail_hotel.Initialization(hotel_booking_id);
                    _global_function.RemoveLoading()

                }, 300);

            }
        });
    },
    FlyingTicket: function (order_id, group_fly) {
        _global_function.AddLoading()

        if ($('#FlyBooking-Service').length) {
            $('#FlyBooking-Service').removeClass('show')
            setTimeout(function () {
                $('#FlyBooking-Service').remove();
            }, 300);

        }
        $.ajax({
            url: "AddFlyBookingService",
            type: "post",
            data: {
                order_id: order_id,
                group_fly: group_fly
            },
            success: function (result) {
                $('body').append(result);
                setTimeout(function () {
                    _order_detail_create_service.StopScrollingBody();
                    _order_detail_fly.Initialization(order_id, group_fly);
                    _global_function.RemoveLoading()

                }, 300);

            }
        });
    },
    Tour: function (order_id, tour_id) {
        _global_function.AddLoading()

        if ($('#add-service-tour').length) {
            $('#add-service-tour').removeClass('show')
            setTimeout(function () {
                $('#add-service-tour').remove();
            }, 300);

        }
        $.ajax({
            url: "AddTourService",
            type: "post",
            data: {
                order_id: order_id,
                tour_id:tour_id
            },
            success: function (result) {
                $('body').append(result);
                setTimeout(function () {
                    _order_detail_create_service.StopScrollingBody();
                    _order_detail_tour.Initialization(order_id, tour_id);
                    _global_function.RemoveLoading()

                }, 300);

            }
        });
    },
    OtherService: function (order_id, booking_id) {
        _global_function.AddLoading()

        if ($('#add-service-other').length) {
            $('#add-service-other').removeClass('show')
            setTimeout(function () {
                $('#add-service-other').remove();
            }, 300);

        }
        $.ajax({
            url: "AddOtherService",
            type: "post",
            data: {
                order_id: order_id,
                other_booking_id: booking_id
            },
            success: function (result) {
                $('body').append(result);
                setTimeout(function () {
                    _order_detail_create_service.StopScrollingBody();
                    _order_detail_other.Initialization(order_id, booking_id);
                    _global_function.RemoveLoading()

                }, 300);

            }
        });
    },
    VinWonderService: function (order_id, booking_id) {
        _global_function.AddLoading()

        if ($('#add-service-other').length) {
            $('#add-service-other').removeClass('show')
            setTimeout(function () {
                $('#add-service-other').remove();
            }, 300);

        }
        $.ajax({
            url: "AddVinWonderService",
            type: "post",
            data: {
                order_id: order_id,
                booking_id: booking_id
            },
            success: function (result) {
                $('body').append(result);
                setTimeout(function () {
                    _order_detail_create_service.StopScrollingBody();
                    _order_detail_vinwonder.Initialization(order_id, booking_id);
                    _global_function.RemoveLoading()

                }, 300);

            }
        });
    },
    WaterSportService: function (order_id, booking_id) {
        if ($('#add-service-watersport').length) {
            $('#add-service-watersport').removeClass('show')
            setTimeout(function () {
                $('#add-service-watersport').remove();
            }, 300);

        }
        $.ajax({
            url: "AddWaterSportService",
            type: "post",
            data: {
                order_id: order_id,
                booking_id: booking_id
            },
            success: function (result) {
                $('body').append(result);
                setTimeout(function () {
                    _order_detail_create_service.StopScrollingBody();
                    _order_detail_watersport.Initialization(order_id, booking_id);
                }, 300);

            }
        });
    },
    StopScrollingBody: function () {
        $('body').addClass('stop-scrolling');
    },
    StartScrollingBody: function () {
        $('body').removeClass('stop-scrolling');

    },
    DeleteHotel: function (hotel_booking_id) {
        var title = _order_detail_html.confirmbox_delete_service_title.replaceAll('{service}', 'Khách sạn')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "DeleteService",
                type: "post",
                data: { service_type: 1, id: hotel_booking_id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    DeleteFlyBookingDetail: function (order_id,group_booking_id) {
        var title = _order_detail_html.confirmbox_delete_service_title.replaceAll('{service}', 'Vé máy bay')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "DeleteService",
                type: "post",
                data: { service_type: 3, id: group_booking_id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    DeleteTour: function (tour_id) {
        var title = _order_detail_html.confirmbox_delete_service_title.replaceAll('{service}', 'Tour')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "DeleteService",
                type: "post",
                data: { service_type: 5, id: tour_id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    DeleteOtherBookingDetail: function (id) {
        var title = _order_detail_html.confirmbox_delete_service_title.replaceAll('{service}', '')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "DeleteService",
                type: "post",
                data: { service_type: 9, id: id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    DeleteVinwondereBookingDetail: function (id) {
        var title = _order_detail_html.confirmbox_delete_service_title.replaceAll('{service}', '')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "DeleteService",
                type: "post",
                data: { service_type: 6, id: id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    DeleteWaterSportBookingDetail: function (id) {
        var title = _order_detail_html.confirmbox_delete_service_title.replaceAll('{service}', '')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "DeleteService",
                type: "post",
                data: { service_type: 9, id: id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    CancelHotel: function (hotel_booking_id) {
        var title = _order_detail_html.confirmbox_cancel_service_title.replaceAll('{service}', 'Khách sạn')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "CancelService",
                type: "post",
                data: { service_type: 1, id: hotel_booking_id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    CancelFlyBookingDetail: function (group_booking_id) {
        var title = _order_detail_html.confirmbox_cancel_service_title.replaceAll('{service}', 'Vé máy bay')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "CancelService",
                type: "post",
                data: { service_type: 3, id: group_booking_id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    CancelTour: function (tour_id) {
        var title = _order_detail_html.confirmbox_cancel_service_title.replaceAll('{service}', 'Tour')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "CancelService",
                type: "post",
                data: { service_type: 5, id: tour_id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    CancelOthers: function (id) {
        var title = _order_detail_html.confirmbox_cancel_service_title.replaceAll('{service}', '')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "CancelService",
                type: "post",
                data: { service_type: 9, id: id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    CancelVinwonder: function (id) {
        var title = _order_detail_html.confirmbox_cancel_service_title.replaceAll('{service}', '')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "CancelService",
                type: "post",
                data: { service_type: 6, id: id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
    CancelWaterSport: function (id) {
        var title = _order_detail_html.confirmbox_cancel_service_title.replaceAll('{service}', '')
        var description = _order_detail_html.confirmbox_delete_service_description
        _msgconfirm.openDialog(title, description, function () {
            _global_function.AddLoading()
            $.ajax({
                url: "CancelService",
                type: "post",
                data: { service_type: 9, id: id },
                success: function (result) {
                    if (result != undefined && result.status == 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            window.location.reload();
                        }, 300);
                    }
                    else {
                        _msgalert.error(result.msg);
                    }
                    _global_function.RemoveLoading()

                }
            })
        })
    },
}


