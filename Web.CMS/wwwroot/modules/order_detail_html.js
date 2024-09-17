var _order_detail_html = {
    html_user_option: '<option class="select2-results__option" value="{user_id}">{user_name} - {user_email}{user_phone}</option>',
    html_tour_option: '<option class="" value="{tour_product_id}" {selected}>{tour_name}</option>',
    html_airport_option: '<option class="select2-results__option" value="{code}">{description} - ({code})</option>',
    html_hotel_option: ' <option {if_selected} value="{hotel_id}">{name}</option>',
    html_new_room_guest: `
            <tr class="servicemanual-hotel-roomguest-row">
                    <td class="servicemanual-hotel-roomguest-order">{order}</td>
                    <td><input type="text" class="form-control servicemanual-hotel-roomguest-name" name="servicemanual-hotel-roomguest-name" value="{room_guest_name}"></td>
                    <td>
                        <select class="select2 servicemanual-hotel-roomguest-type servicemanual-hotel-roomguest-type-new" style="width: 100%;">
                            <option value="0" selected="selected">Người lớn</option>
							<option value="1">Trẻ em</option>
							<option value="2" >Em bé</option>
                        </select>
                    </td>
               <td><input class="form-control datepicker-input servicemanual-hotel-roomguest-birthday" type="text" name="servicemanual-hotel-roomguest-birthday" value="{room_guest_birthday}"></td>
                <td> <select class="select2 servicemanual-hotel-roomguest-roomselect-new servicemanual-hotel-roomguest-roomselect" style="width: 100%;"> </select> </td>
                <td class="text-right"><input type="text" class="form-control servicemanual-hotel-roomguest-note"value="{room_guest_note}"></td>
                <td><a class="fa fa-plus-circle green" href="javascript:;" onclick="_order_detail_hotel.AddHotelRoomGuest();"></a>
                <a class="red fa fa-times" href="javascript:;" onclick="_order_detail_hotel.DeleteHotelGuest($(this));"></a>
                </td> </tr>`,
    html_select_guest_to_room_option: '<option value="{room_id}" {if_selected}>Phòng {room_order}</option>',
    html_loading_gif: '<img src="/images/icons/loading.gif" style="width: 55px;height: 55px;margin-left: 10px;margin-bottom: 10px;" class="img_loading_summit coll">',
    summit_confirmbox_title: 'Thông báo xác nhận',
    summit_confirmbox_create_hotel_service_description: "Các thông tin về dịch vụ khách sạn được thêm mới này sẽ được lưu lại. Bạn có chắc chắn không?",
    summit_confirmbox_create_fly_booking_service_description: "Các thông tin về dịch vụ vé máy bay {is_new} này sẽ được lưu lại. Bạn có chắc chắn không?",
    summit_confirmbox_create_tour_service_description: "Các thông tin về dịch vụ Tour {is_new} này sẽ được lưu lại. Bạn có chắc chắn không?",
    summit_confirmbox_create_other_service_description: "Các thông tin về dịch vụ này sẽ được lưu lại. Bạn có chắc chắn không?",
    summit_confirmbox_create_watersport_service_description: "Các thông tin về dịch vụ Thể thao biển sẽ được lưu lại. Bạn có chắc chắn không?",
    html_option_client_suggesstion: '<div style=" font-weight: bold; font-size: 14px; text-transform: uppercase; ">{Name}</div><div style="font-size: 12px; color:{if_danger};">{ClientType}</div><div style="font-size: 12px;">Email: {Email}</div><div style="font-size: 12px;">SĐT: {phone}</div>',
    html_airport_code_option: '<option value="{airport_code}" {if_selected}>{airport_code} ({airport_name})</option>',
    html_airline_option: '<option value="{airline_code}" {if_selected}>{airline_code} ({airline_name})</option>',
    html_service_hotel_newroom: `
        <tr class="servicemanual-hotel-room-tr" data-room-id="{{new_room_id}}" data-room-type-id="" data-room-type-code="">
        <td class="servicemanual-hotel-room-td-order">{{room_order}}</td>
        <td> <input class="form-control servicemanual-hotel-room-type-name" type="text" name="servicemanual-hotel-room-code" placeholder="Nhập tên hạng phòng" value=""> </td>
        <td class="servicemanual-hotel-room-td-rates-code">
            <div class="d-flex align-center servicemanual-hotel-room-div-code">
                <a class="fa fa-trash-o txt_14 mr-2 delete-room-rates-button" style="display: none;" href="javascript:;"></a>
                <a class="fa fa-plus-circle green mr-1 green add-room-rates-button" href="javascript:;"></a>
                <input class="form-control servicemanual-hotel-room-rates-code" type="text" data-rate-id="{{new_rate_id}}" name="servicemanual-hotel-room-rates-code" placeholder="Nhập tên gói" value="">
            </div>
        </td>
        <td class="servicemanual-hotel-room-td-rates-daterange">
            <div class="d-flex align-center"> <input class="form-control servicemanual-hotel-room-rates-daterange" type="text" data-rate-id="{{new_rate_id}}" name="servicemanual-hotel-room-rates-daterange" value=""> </div>
        </td>
        <td class="servicemanual-hotel-room-td-rates-operator-price">
            <div class="d-flex align-center">
                <input class="form-control currency servicemanual-hotel-room-rates-operator-price text-right" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-operator-price" placeholder="Nhập giá nhập vào">
            </div>
        </td>
        <td class="servicemanual-hotel-room-td-rates-sale-price">
            <div class="d-flex align-center">
                <input class="form-control currency servicemanual-hotel-room-rates-sale-price text-right" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-sale-price" placeholder="Nhập giá bán">
            </div>
        </td>
        <td class="servicemanual-hotel-room-td-rates-nights">
            <div class="d-flex align-center">
                <input class="form-control currency servicemanual-hotel-room-rates-nights text-right input-disabled-background" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-nights" disabled value="0">
            </div>
        </td>
        <td class="servicemanual-hotel-room-td-number-of-rooms">
            <div class="d-flex align-center">
                <input class="form-control currency servicemanual-hotel-room-number-of-rooms text-right" type="text" name="servicemanual-hotel-room-number-of-rooms" value="1">
            </div>
        </td>
        <td class="servicemanual-hotel-room-td-rates-operator-amount">
                    <div class="d-flex align-center">
                        <input class="input-disabled-background form-control currency servicemanual-hotel-room-rates-operator-amount text-right" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-operator-amount" disabled value="0">
                    </div>
                </td>
        <td class="servicemanual-hotel-room-td-rates-total-amount">
            <div class="d-flex align-center">
                <input class="form-control currency servicemanual-hotel-room-rates-total-amount text-right input-disabled-background" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-total-amount" disabled value="0">
            </div>
        </td>
        <td class="servicemanual-hotel-room-td-rates-profit">
            <div class="d-flex align-center">
                <input class="form-control currency servicemanual-hotel-room-rates-profit text-right input-disabled-background" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-profit" disabled value="0">
            </div>
        </td>
        <td class="txt_14">
            <a class="fa fa-files-o mr-1 servicemanual-hotel-room-clone-room" href="javascript:;"></a>
            <a class="fa fa-trash-o servicemanual-hotel-room-delete-room" href="javascript:;"></a>
        </td>
    </tr>
    `,
    html_service_hotel_newrates_code: '<div class="d-flex align-center servicemanual-hotel-room-div-code"> <a class="fa fa-trash-o txt_14 mr-2 delete-room-rates-button" href="javascript:;"></a> <a class="fa fa-plus-circle green mr-1 green add-room-rates-button" href="javascript:;"></a> <input class="form-control servicemanual-hotel-room-rates-code" type="text" data-rate-id="{{new_rate_id}}" name="servicemanual-hotel-room-rates-code" placeholder="Nhập tên gói" value=""> </div>',
    html_service_hotel_newrates_daterange: '<div class="d-flex align-center"> <input class="form-control servicemanual-hotel-room-rates-daterange" type="text" data-rate-id="{{new_rate_id}}" name="servicemanual-hotel-room-rates-daterange" value=""> </div>',
    html_service_hotel_newrates_operatorprice:'<div class="d-flex align-center"> <input class="form-control currency servicemanual-hotel-room-rates-operator-price text-right" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-operator-price" placeholder="Nhập giá nhập vào"> </div>',
    html_service_hotel_newrates_saleprice:'<div class="d-flex align-center"> <input class="form-control currency servicemanual-hotel-room-rates-sale-price text-right" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-sale-price" placeholder="Nhập giá bán"> </div>',
    html_service_hotel_newrates_nights:'<div class="d-flex align-center"> <input class="form-control currency servicemanual-hotel-room-rates-nights text-right input-disabled-background" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-nights" disabled value="0"> </div>',
    html_service_hotel_newrates_totalamount:'<div class="d-flex align-center"> <input class="form-control currency servicemanual-hotel-room-rates-total-amount text-right input-disabled-background" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-total-amount" disabled value="0"> </div>',
    html_service_hotel_newrates_profit:'<div class="d-flex align-center"> <input class="form-control currency servicemanual-hotel-room-rates-profit text-right input-disabled-background" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-profit" disabled value="0"> </div>',
    html_service_hotel_newrates_operator_amount: `
     <div class="d-flex align-center">
                        <input class="input-disabled-background form-control currency servicemanual-hotel-room-rates-operator-amount text-right" data-rate-id="{{new_rate_id}}" type="text" name="servicemanual-hotel-room-rates-operator-amount" disabled value="0">
                    </div>
`,
    html_service_hotel_extrapackage_newextra_package:`
<tr class="servicemanual-hotel-extrapackage-tr" data-extrapackage-id="0">
    <td class="servicemanual-hotel-extrapackage-td-order">{{order}}</td>
    <td>
        <input class="form-control servicemanual-hotel-extrapackage-type-name" type="text" name="servicemanual-hotel-extrapackage-code" placeholder="Nhập nội dung dịch vụ" value="">
    </td>
    <td class="servicemanual-hotel-extrapackage-td-code">
        <div class="d-flex align-center  servicemanual-hotel-extrapackage-div-code">
            <input class="form-control servicemanual-hotel-extrapackage-code" type="text" name="servicemanual-hotel-extrapackage-code" placeholder="Nhập tên gói" value="">
        </div>
    </td>
    <td class="servicemanual-hotel-extrapackage-td-daterange">
        <div class="d-flex align-center">
            <input class="form-control servicemanual-hotel-extrapackage-daterange" type="text" name="servicemanual-hotel-extrapackage-daterange" value="">
        </div>
    </td>
    <td class="servicemanual-hotel-extrapackage-td-operator-price">
        <div class="d-flex align-center">
            <input class="form-control currency servicemanual-hotel-extrapackage-operator-price text-right" type="text" name="servicemanual-hotel-extrapackage-operator-price" placeholder="Nhập giá nhập vào">
        </div>
    </td>
    <td class="servicemanual-hotel-extrapackage-td-sale-price">
        <div class="d-flex align-center">
            <input class="form-control currency servicemanual-hotel-extrapackage-sale-price text-right" type="text" name="servicemanual-hotel-extrapackage-sale-price" placeholder="Nhập giá bán">
        </div>
    </td>
    <td class="servicemanual-hotel-extrapackage-td-nights">
        <div class="d-flex align-center">
            <input class="input-disabled-background form-control currency servicemanual-hotel-extrapackage-nights text-right " type="text" name="servicemanual-hotel-extrapackage-nights" disabled value="0">
        </div>
    </td>
    <td class="servicemanual-hotel-extrapackage-td-number-of-extrapackages">
        <div class="d-flex align-center">
            <input class="form-control currency servicemanual-hotel-extrapackage-number-of-extrapackages text-right" type="text" name="servicemanual-hotel-extrapackage-number-of-extrapackages" value="1">
        </div>
    </td>
    <td class="servicemanual-hotel-extrapackage-td-operator-amount">

        <div class="d-flex align-center">
            <input class="input-disabled-background form-control currency servicemanual-hotel-extrapackage-operator-amount text-right" type="text" name="servicemanual-hotel-extrapackage-operator-amount" disabled value="0">
        </div>
    </td>
    <td class="servicemanual-hotel-extrapackage-td-total-amount">
        <div class="d-flex align-center">
            <input class="input-disabled-background form-control currency servicemanual-hotel-extrapackage-total-amount text-right" type="text" name="servicemanual-hotel-extrapackage-total-amount" disabled value="">
        </div>
    </td>
    <td class="servicemanual-hotel-extrapackage-td-profit">
        <div class="d-flex align-center">
            <input class="input-disabled-background form-control currency servicemanual-hotel-extrapackage-profit text-right" type="text" name="servicemanual-hotel-extrapackage-profit" disabled value="">
        </div>
    </td>
    <td class="txt_14">
        <a class="fa fa-trash-o servicemanual-hotel-extrapackage-delete-extrapackage" href="javascript:;"></a>
    </td>
</tr>`,

    html_servicefly_extrapackage_newpackage_tr: `
         <tr class="service-fly-extrapackage-row" data-extra-package-id="-1">
                    <td class="service-fly-extrapackage-order">@(++index)</td>
                    <td>
                        <select class="form-control service-fly-extrapackage-packagename-select" style="width:100% !important">
                            <option value="adt_amount">Người lớn</option>
                            <option value="chd_amount">Trẻ em (2-14 tuổi)</option>
                            <option value="inf_amount">Em bé (0-2 tuổi)</option>
                        </select>
                    </td>
                    <td> <input class="form-control text-right currency service-fly-extrapackage-baseprice" type="text" name="service-fly-extrapackage-baseprice" value=""></td>
                    <td> <input class="form-control text-right currency service-fly-extrapackage-saleprice" type="text" name="service-fly-extrapackage-saleprice" value=""></td>
                    <td> <input class="form-control text-right currency service-fly-extrapackage-quantity" type="text" name="service-fly-extrapackage-quantity" value=""></td>
                    <td class="text-right"> <input class="form-control text-right currency service-fly-extrapackage-amount" disabled style="background-color: lightgray;" value=""></td>
                    <td class="text-right service-fly-packages-profit-row">
                        <input class="form-control text-right currency service-fly-extrapackage-profit" disabled type="text" style="background-color: lightgray;" value="">
                    </td>
                    <td class="text-right">
                        <a class="fa fa-trash-o" href="javascript:;" onclick="_order_detail_fly.DeleteFlyBookingExtraPackage($(this));"></a>
                    </td>
                </tr>
        `,
    html_servicefly_passenger_newpassenger_tr: '<tr class="service-fly-passenger-row" data-passenger-id="@p.Id"> <td class="service-fly-passenger-number">@(++index)</td> <td> <select class="select2 service-fly-passenger-genre service-fly-passenger-genre-new" name="service-fly-passenger-genre" style="width:100% !important"> <option value="0" {genre_male_if_selected}>Nam</option> <option value="1"{genre_female_if_selected} >Nữ</option> </select> </td> <td class="text-right"><input type="text" class="form-control service-fly-passenger-name" name="service-fly-passenger-name" value="@(p.Name)"></td> <td> <div class="datepicker-wrap" style="width:100%"> <input class="form-control datepicker-input service-fly-passenger-birthday" type="text" name="service-fly-passenger-birthday" value="@(p.Birthday!=null?((DateTime)p.Birthday).ToString("dd/MM/yyyy"): DateTime.Now.ToString("dd/MM/yyyy"))"> </div> </td> <td class="text-right"><input type="text" class="form-control service-fly-passenger-note" value="@p.Note"></td> <td> <a class="red" href="javascript:;" onclick="_order_detail_fly.DeleteFlyBookingPassenger($(this));"><i class="fa fa-times"></i></a> &nbsp; </td> </tr>',
    html_serviceflyextrapackage_newpackage_new_select_option: '<option value="{package_id}" {package_name}</option>',
    html_service_fly_extrapackage_new_extra_package_tr: `
        <tr class="service-fly-extrapackage-row" data-extra-package-id="0">
                    <td class="service-fly-extrapackage-order">@(++index)</td>
                    <td>
                        <input type="text" class="form-control service-fly-extrapackage-packagename-select-input" style="width:100% !important" value="">

                    <td> <input class="form-control text-right currency service-fly-extrapackage-baseprice" type="text" name="service-fly-extrapackage-baseprice" value=""></td>
                    <td> <input class="form-control text-right currency service-fly-extrapackage-saleprice" type="text" name="service-fly-extrapackage-saleprice" value=""></td>
                    <td> <input class="form-control text-right currency service-fly-extrapackage-quantity" type="text" name="service-fly-extrapackage-quantity" value=""></td>
                    <td class="text-right"> <input class="form-control text-right currency service-fly-extrapackage-amount" disabled style="background-color: lightgray;" value=""></td>
                    <td class="text-right service-fly-packages-profit-row">
                        <input class="form-control text-right currency service-fly-extrapackage-profit" disabled type="text" style="background-color: lightgray;" value="">
                    </td>
                    <td class="text-right">
                        <a class="fa fa-trash-o" href="javascript:;" onclick="_order_detail_fly.DeleteFlyBookingExtraPackage($(this));"></a>
                    </td>
                </tr>

        `,

    html_service_tour_packages_add_extra_package_tr: `
        <tr class="service-tour-extrapackage-row" data-extra-package-id="@item.Id">
        <td class="service-tour-extrapackage-order">@(++index)</td>
        <td> <input type="text" class="form-control service-tour-extrapackage-packagename-select-input" style="width:100% !important" value="@item.PackageName"> </td>
        <td>
            <input class="form-control text-right currency service-tour-extrapackage-baseprice" type="text" name="service-tour-extrapackage-baseprice" style="display:none;" value="@(((double)item.BasePrice).ToString("N0"))">
            <input class="form-control text-right currency service-tour-extrapackage-quantity" type="text" name="service-tour-extrapackage-quantity" value="@(((int)item.Quantity).ToString("N0"))">
        </td>
        <td class="text-right"> <input class="form-control text-right currency service-tour-extrapackage-price" value="0"></td>
        <td class="text-right"> <input class="form-control text-right currency service-tour-extrapackage-amount" value=""></td>
        <td class="text-right service-tour-extrapackage-profit-row"> <input class="form-control text-right currency service-tour-extrapackage-profit" disabled type="text" value=""></td>
        <td class="text-right"> <a class="fa fa-trash-o" href="javascript:;" onclick="_order_detail_tour.DeleteTourPackage($(this));"></a> </td>
    </tr>
    `,
    html_service_tour_add_guest_tr:'<tr class="service-tour-guest-row new_tour_guest_row" data-extra-guest-id="0"> <td class="service-tour-guest-order">@(++index)</td> <td class=""> <input type="text" class="form-control service-tour-guest-name" style="width:100% !important" value="{guest_name}"> </td> <td> <select class="select2 service-tour-guest-gender" name="" style="width: 100%;"> <option value="0" {genre_male_if_selected}> Nam</option> <option value="1" {genre_female_if_selected}> Nữ</option> </select> </td> <td> <div class="datepicker-wrap" style="width:100%"> <input class="form-control datepicker-input service-tour-guest-birthday" type="text" name="service-tour-guest-birthday" value="{birthday}"> </div> </td> <td><input class="form-control service-tour-guest-cccd" type="text" name="service-tour-guest-cccd" value="{cccd}"></td> <td><input class="form-control service-tour-guest-room-number" type="text" name="service-tour-guest-room-number" value="{room_number}"></td> <td><input type="text" class="form-control service-tour-guest-note" value="{note}"></td> <td> <a class="red" href="javascript:;" onclick="_order_detail_tour.DeleteTourGuest($(this));"><i class="fa fa-times"></i></a> </td> </tr>',

    html_service_other_new_packages: `
  <tr class="service-other-packages-row" data-extra-package-id="0">
        <td class="service-other-packages-order">@(++index)</td>
        <td>
            <input type="text" class="form-control service-other-packages-packagename" style="width:100% !important" value="">
        </td>
        <td> <input class="form-control text-right currency service-other-packages-baseprice" type="text" name="service-other-packages-baseprice" value=""></td>
        <td> <input class="form-control text-right currency service-other-packages-saleprice" type="text" name="service-other-packages-saleprice" value=""></td>
        <td> <input class="form-control text-right currency service-other-packages-quantity" type="text" name="service-other-packages-quantity" value=""></td>


        <td class="text-right"> <input class="form-control text-right currency service-other-packages-amount" style="background-color: lightgray;" disabled value=""></td>
        <td class="text-right service-other-packages-profit-row">
            <input class="form-control text-right currency service-other-packages-profit" style="background-color: lightgray;" type="text" value="">
        </td>
        <td class="text-right">
            <a class="fa fa-trash-o" href="javascript:;" onclick="_order_detail_other.DeleteOtherBookingpackages($(this));"></a>
        </td>
    </tr>
            `,
    html_service_vinwonder_new_packages: `
  <tr class="service-vinwonder-packages-row" data-extra-package-id="0">
        <td class="service-vinwonder-packages-order">@(++index)</td>
        <td>
            <input type="text" class="form-control service-vinwonder-packages-packagename" style="width:100% !important" value="">
        </td>
        <td>
            <input class="form-control service-vinwonder-packages-date new-service-vinwonder-packages-date" type="text" name="service-vinwonder-packages-date" value="">
        </td>
        <td> <input class="form-control text-right currency service-vinwonder-extrapackage-baseprice" type="text" name="service-vinwonder-extrapackage-baseprice" value=""></td>
        <td> <input class="form-control text-right currency service-vinwonder-extrapackage-saleprice" type="text" name="service-vinwonder-extrapackage-saleprice" value=""></td>
        <td> <input class="form-control text-right currency service-vinwonder-extrapackage-quantity" type="text" name="service-vinwonder-extrapackage-quantity" value=""></td>
        <td class="text-right"> <input class="form-control text-right currency service-vinwonder-packages-amount" style="background-color: lightgray;" disabled value=""></td>

        <td class="text-right service-vinwonder-packages-profit-row">
            <input class="form-control text-right currency service-vinwonder-packages-profit" style="background-color: lightgray;" disabled type="text" value="">
        </td>

        <td class="text-right">
            <a class="fa fa-trash-o" href="javascript:;" onclick="_order_detail_vinwonder.DeletevinwonderBookingpackages($(this));"></a>
        </td>
    </tr>
    `,
    html_service_vinwonder_add_guest_tr:'<tr class="service-vinwonder-guest-row new_vinwonder_guest_row" data-extra-guest-id="0"> <td class="service-vinwonder-guest-order">@(++index)</td> <td class=""> <input type="text" class="form-control service-vinwonder-guest-name" style="width:100% !important" value="{guest-name}"> </td> <td> <input type="text" class="form-control service-vinwonder-guest-email" style="width:100% !important" value="{email}"> </td> <td><input class="form-control service-vinwonder-guest-phone" type="text" name="service-vinwonder-guest-phone" value="{phone}"></td> <td><input type="text" class="form-control service-vinwonder-guest-note" value="{note}"></td> <td> <a class="red" href="javascript:;" onclick="_order_detail_vinwonder.DeletevinwonderGuest($(this));"><i class="fa fa-times"></i></a> </td> </tr>',
    html_vinwonder_location_option: '<option class="" {if_selected} value="{id}">{location_name}</option>',

    element_name: "attachment-image-choice-list",
    confirmbox_delete_service_title: 'Xác nhận xóa dịch vụ {service}',
    confirmbox_cancel_service_title: 'Xác nhận hủy dịch vụ {service}',
    confirmbox_delete_service_description: "Các thông tin về dịch vụ này sẽ bị xóa. Bạn có chắc chắn không?",
    html_service_watersport_new_packages:`

<tr class="service-watersport-packages-row" data-extra-package-id="0">
            <td class="service-watersport-packages-order">@(++index)</td>
            <td>
                <select  class="select select2 service-watersport-service-type service-watersport-service-type-new" name="service-watersport-service-type" style="width: 100%;">
            {option}


                </select>
            </td>
            <td> <input class="form-control text-right currency service-watersport-packages-baseprice" type="text" name="service-watersport-packages-baseprice" value=""></td>
            <td> <input class="form-control text-right currency service-watersport-packages-quantity" type="text" name="service-watersport-packages-quantity" value=""></td>


            <td class="text-right"> <input class="form-control text-right currency service-watersport-packages-amount" style="background-color: lightgray;" disabled value=""></td>
        <td><textarea class="form-control style-width2 textarea service-watersport-packages-note"></textarea></td>

            <td class="text-right">
                <a class="fa fa-trash-o" href="javascript:;" onclick="_order_detail_watersport.DeletewatersportBookingpackages($(this));"></a>
            </td>
        </tr>
`,
}
