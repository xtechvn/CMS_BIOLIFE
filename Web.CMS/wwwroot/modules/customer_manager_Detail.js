

$(document).ready(function () {

    var user_Id = $('#id_userid').val();
    if (user_Id != null) {
        _customer_manager_Detail.Loaddata();
    }
    
});

var _customer_manager_Detail = {
    Loaddata: function () {
        let _searchModel = {
            id: $('#id_userid').val()
        };
        var objSearch = this.SearchParam;

        objSearch = _searchModel;

        this.Search(objSearch);
   
        this.SearchPaymentAccount(objSearch);
        this.SearchDetailCustomerManager(objSearch);
        this.SearchContract(objSearch);
        let _searchModel2 = {
            ClientId: $('#id_userid').val(), 
            OrderNo: null,
            StartDateFrom: null,
            StartDateTo: null,
            EndDateFrom: null,
            EndDateTo: null,
            Note: null,
            UtmSource: null,
            ServiceType: null,
            Status: null,
            CreateTime: null,
            CreateName: null,
            HINHTHUCTT: null,
            Sale: null,
            BoongKingCode: null,
            sysTemType: -1,
            StatusTab: 99,
            PageIndex: 1,
            pageSize: 20,
            currentPage:1,
        };
        this.SearchOrder(_searchModel2);
    },
    ReLoad: function () {
        let _searchModel = {
            id: $('#id_userid').val()
        };
        var objSearch = this.SearchParam;

        objSearch = _searchModel;

       
        this.SearchPaymentAccount(objSearch);
       
    },
   
    SearchPaymentAccount: function (input) {
        $.ajax({
            url: "/CustomerManager/ListPaymentAccount",
            type: "Post",
            data: input,
            success: function (result) {
                $('#imgLoading_deposit_history').hide();
                $('#grid_data_payment_account').html(result);
            }
        });
    },

    Search: function (input) {
        $.ajax({
            url: "/CustomerManager/ListDepositHistory",
            type: "Post",
            data: input,
            success: function (result) {
                $('#imgLoading_deposit_history').hide();
                $('#grid_data_deposit_history').html(result);
            }
        });
    }, 
    SearchContract: function (input) {
        $.ajax({
            url: "/CustomerManager/ListContractPay",
            type: "Post",
            data: input,
            success: function (result) {
                $('#imgLoading_Contract').hide();
                $('#grid_data_Contract').html(result);
            }
        });
    },
    SearchOrder: function (input) {
        $.ajax({
            url: "/CustomerManager/ListOrderbyClientid",
            type: "post",
            data: input,
            success: function (result) {
                $('#imgLoading').hide();
                $('#grid_data_order').html(result);

                $('#select2-selectPaggingOptions-container').html(input.pageSize + " kết quả/trang");
                $('#select2-selectPaggingOptions-container').prop('title', input.pageSize + " kết quả/trang");
                $('#selectPaggingOptions option[value=' + input.pageSize + ']').prop("selected", true);
                $('#List-Order-Status').html('');
                $('#List-Order-Amount').html('');

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log("Status: " + textStatus);
            }
        });
    },
    SearchDetailCustomerManager: function (input) {

        $.ajax({
            url: "/CustomerManager/DetailCustomerManager",
            type: "Post",
            data: input,
            success: function (result) {
                $('#imgLoading_Customer_Manager').hide();
                $('#grid_detail_Customer_Manager').html(result);
            }
        });
    },
    DepositHistoryOnPaging: function (value) {
        let _searchModel = {
            id: $('#id_userid').val(),
            currentPage: value,
        };
        var objSearch = this.SearchParam
        objSearch = _searchModel
        this.Search(objSearch);
    },

    OrederOnPaging: function (value) {
        let _searchModel = {
            id: $('#id_userid').val(),
            currentPage: value,
        };
        var objSearch = this.SearchParam
        objSearch = _searchModel
        this.SearchOrder(objSearch);
    },
    ContractOnPaging: function (value) {
        let _searchModel = {
            id: $('#id_userid').val(),
            currentPage: value,
        };
        var objSearch = this.SearchParam
        objSearch = _searchModel
        this.SearchContract(objSearch);
    },
    PaymentAccountOnPaging: function (value) {
        let _searchModel = {
            id: $('#id_userid').val(),
            currentPage: value,
        };
        var objSearch = this.SearchParam
        objSearch = _searchModel
        this.SearchPaymentAccount(objSearch);
    },

    OpenPopupPaymentAccount: function (id) {
        var user_Id = $('#id_userid').val();
        
        let title = 'Thêm mới/Cập nhật tài khoản thanh toán';
        let url = '/PaymentAccount/Detail';
        let param = {
            id: id,
            user_Id: user_Id
        };
        
        _magnific.OpenSmallPopup(title, url, param);
   
    },
    OnSetupPaymentAccount: function () {
        let FromCreate = $('#form-create');
        FromCreate.validate({
            rules: {
                AccountNumber: {
                    required: true,
                    number: true,
                },
                AccountName: "required",
                BankId: "required",
              
            },
            messages: {
                AccountNumber: {
                    required:"Số tài khoản không được để trống",
                    number:"Vui lòng nhập đúng định dạng"
                },
                AccountName: "Thông tin chủ tài khoản không được để trống",
                BankId: "Tên ngân hàng không được để trống",
               
            }
        });
        if (FromCreate.valid()) {
            let form = document.getElementById('form-create');
            var DataModel = new FormData(form);
            $.ajax({
                url: "/PaymentAccount/Setup",
                type: "POST",
                data: DataModel,
                processData: false,
                contentType: false,
                success: function (result) {
                    if (result.stt_code === 1) {
                        _msgalert.error(result.msg);
                    } else {
                        _msgalert.success(result.msg);
                        $.magnificPopup.close();
                        _customer_manager_Detail.ReLoad();
                    }
                },
                error: function (jqXHR) {
                },
                complete: function (jqXHR, status) {
                }
            });
        }

    },
   DeletePaymentAccount: function (id) {
       let url = '/PaymentAccount/DeleteBankingAccount';
       let title = 'Xác nhận xóa thông tin thanh toán';
       let description = 'Bạn có chắc chắn muốn thông tin thanh toán?';
       _msgconfirm.openDialog(title, description, function () {
           _ajax_caller.post(url, { id: id }, function (result) {
               _global_function.AddLoading()
               if (result.isSuccess) {
                   _msgalert.success(result.message);
                   _global_function.RemoveLoading()
                   _customer_manager_Detail.Loaddata();
               } else {
                   _msgalert.error(result.message);
                   _global_function.RemoveLoading()
               }
           });
       });
    },
    SetActive: function (status) {
        $('#data_order').removeClass('active')
        $('#data_payment_account').removeClass('active')
        $('#data_deposit_history').removeClass('active')
        $('#data_Contract').removeClass('active')
        if (status == 1)
            $('#data_order').addClass('active')  
        if (status == 2)
            $('#data_payment_account').addClass('active');
        if (status == 3)
            $('#data_deposit_history').addClass('active')
        if (status == 4)
            $('#data_Contract').addClass('active')       
    },  
    OnStatuse: function (value) {
        if (value == 1) {
            $('#grid_data_order').show();
            $('#grid_data_payment_account').hide();
            $('#grid_data_deposit_history').hide();
            $('#grid_data_Contract').hide();
            $('#bt_data_payment_account').hide();
        }
        if (value == 2) {
            $('#grid_data_order').hide();
            $('#grid_data_payment_account').show();
            $('#grid_data_deposit_history').hide();
            $('#grid_data_Contract').hide();
            $('#bt_data_payment_account').show();
        }
        if (value == 3) {
            $('#grid_data_order').hide();
            $('#grid_data_payment_account').hide();
            $('#grid_data_deposit_history').show();
            $('#grid_data_Contract').hide();
            $('#bt_data_payment_account').hide();
        }
        if (value == 4) {
            $('#grid_data_order').hide();
            $('#grid_data_payment_account').hide();
            $('#grid_data_deposit_history').hide();
            $('#grid_data_Contract').show();
            $('#bt_data_payment_account').hide();
        }
        
    },
    OnUpdataClient: function () {
        var Id = $('#CustomerManager_Id').val();
        var JoinDate = $('#JoinDate').val();
        var id_ClientType = $('#id_ClientType').val();
        var optradio = $('input[name="optradio"]:checked').val();
        var Client_name = $('#Client_name').val();
        var Maso_Id = $('#Maso_Id').val();
        var phone = $('#phone').val();
        var email = $('#email').val();
        var DiaChi_giaodich = $('#DiaChi_giaodich').val();
        var DC_hoadon = $('#DC_hoadon').val();
        var id_loaikhach = $('#id_loaikhach').val();
        var id_nhomkhach = $('#id_nhomkhach').val();
        var pass = $('#pass').val();
        var pass_backup = $('#pass_backup').val();
        var So_tk = $('#So_tk').val();
        var Name_tk = $('#Name_tk').val();
        var Name_nh = $('#Name_nh').val();
        var diachi_chinhanh = $('#diachi_chinhanh').val();
        var Note = $('#Note').val();
        var user_Id = $('#id_userid').val();
        var ClientCode = $('#ClientCode').val();
        let FromCreate = $('#CustomerManager_Detail');
        FromCreate.validate({
            rules: {

                "Client_name": "required",
                
                "phone": {
                    required: true,
                    number: true,
                },
                "email": {
                    required: true,
                    email: true,
                },
               
                "id_loaikhach": "required",
                "id_nhomkhach": "required",
                "pass": "required",
                "pass_backup_err": "required",
                "So_tk": { number: true, },

            },
            messages: {
                "Client_name": "Tên khách hàng không được bỏ trống",
               
                "phone": {
                    required: "Số điện thoại không được bỏ trống",
                    number: "Nhập đúng định dạng số",
                },
                "email": {
                    required: "Email không được bỏ trống",
                    email: "Nhạp đúng định dạng email",
                },
                
                "id_loaikhach": "Vui lòng chọn loại khách hàng",
                "id_nhomkhach": "Vui lòng chọn nhóm khách hàng",
                "pass": "không được bỏ trống",
                "pass_backup_err": "không được bỏ trống",
                "So_tk": {
                    number: "Nhập đúng định dạng số",
                },
            }
        });
        if (FromCreate.valid()) {
            var object_summit = {
                Id: Id,
                JoinDate: JoinDate,
                Client_name: Client_name,
                id_ClientType: id_loaikhach,
                AgencyType: optradio,
                phone: phone,
                email: email,
                diachi_chinhanh: diachi_chinhanh,
                DC_hoadon: DC_hoadon,
                DiaChi_giaodich: DiaChi_giaodich,
                id_loaikhach: optradio,
                id_nhomkhach: id_nhomkhach,
                pass: pass,
                pass_backup: pass_backup,
                So_tk: So_tk,
                Name_tk: Name_tk,
                Name_nh: Name_nh,
                Note: Note,
                Maso_Id: Maso_Id,
                ClientCode: ClientCode,
            }
            let data = JSON.stringify(object_summit)
            $.ajax({
                url: '/CustomerManager/Setup',
                type: "post",
                data: { data },
                success: function (result) {

                    if (result.stt_code === 1) {
                        _msgalert.error(result.msg);
                    }

                    if (result.stt_code === 0) {
                        _msgalert.success(result.msg);
                        $.magnificPopup.close();
                        _customer_manager_Detail.Loaddata();

                    }
                }
            });
        }
    },
    OnUpdataUserAgent: function () {
        
        let FromCreate = $("#UserAgent_Detail");
        FromCreate.validate({
            rules: {

                UserId_new: "required",
            },
            messages: {
                UserId_new: "Tên nhân viên mới không được bỏ trống",
            }
        });
        if (FromCreate.valid()) {
            var UserId_new = $('#UserId_new').select2("val");
            var UserId = UserId_new[0];
            var user_Id = $('#id_userid').val();
            var Client_Id = $('#UserAgent_Client').val();
            let _searchModel = {
                id: user_Id
            };
            var objSearch = this.SearchParam;

            objSearch = _searchModel;
            $.ajax({
                url: "/CustomerManager/UpdatalUserAgent",
                type: "POST",
                data: {
                    id: user_Id,
                    userId: UserId,
                    clientId: Client_Id
                },
                success: function (result) {

                    if (result.stt_code === 1) {
                        _msgalert.error(result.msg);
                    }

                    if (result.stt_code === 0) {
                        _msgalert.success(result.msg);
                        setTimeout(function () {
                            $.magnificPopup.close();
                            window.location.reload();
                        }, 500);
                       

                    }
                }
            });
        }
    },
    OpenPopupUserAgent: function (id,clientid) {
        let title = 'Đổi nhân viên phụ trách';
        if (id == 0) {
            title = 'Thêm mới nhân viên phụ trách';
        } 
        let url = '/CustomerManager/DetailUserAgent';
        let param = {
            user_Id: id,
            client: clientid
        };
        _magnific.OpenSmallPopup(title, url, param);
    },
    SendMailResetPasswordb2b: function (id) {
        $.ajax({
            url: '/CustomerManager/SendMailResetPasswordb2b',
            type: "post",
            data: { id: id },
            success: function (result) {
                if (result.status === 0) {
                    _msgalert.success(result.msg);
                } else {
                    _msgalert.error(result.msg);
                }
            }
        });
    },

}

