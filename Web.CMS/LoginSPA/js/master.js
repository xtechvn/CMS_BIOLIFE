
"use strict";
var sc = sc || {};
var domain_api = "https://qc-login.adavigo.com/";
sc.login = (function () {
    var my_login = {};    
    my_login.load_page = function(){
          
        $(document).ready(function() {                  
                       
            $("#btn_login").off('click').on( "click", function(e) {
                my_login.login();                               

            });
            $("#btn_send_otp").off('click').on( "click", function(e) {
                my_login.verify_code();               
            });
            $("#btn_qr_code").off('click').on( "click", function(e) {
                my_login.get_qr();               
            });   
            $("#txt_password").on('keypress', function(e) {
                if(e.which == 13) {
                    my_login.login();      
                }
            });
            $("#txt_otp").on('keypress', function(e) {
                if(e.which == 13) {
                    $("#btn_send_otp").click();
                }
            });
            $(".btn_download #btn_code").off('click').on( "click", function(e) {
                $(".close").click();
                $(".verity_code").removeClass("dn");
                $(".login_form").addClass("dn");                
                $("#txt_otp").focus();           
            }); 
            
            $("#popup_choose_company #btn_accept_choose").off('click').on( "click", function(e) {
                var token = localStorage.getItem("token_authent");
                var company_choose = $("#cbo_choose_company").val();

                $.ajax({
                    url: domain_api + 'api/authent/send_company_choose.json',
                    type: "POST",
                    dataType: 'json',
                    data: { 
                        token : token,
                        company_choose : company_choose                       
                    },
                    success: function (result) {    
                        debugger;        
                        if(result.status == 0){
                            window.location.href = result.url_redirect; 
                        }                               
                    }
                });  
               
            }); 
            
        });
        
        document.getElementById('download_qr').addEventListener('click', function() {
            // Lấy URL của ảnh
            var imageUrl = $('#img_qr_code img').attr('src');
            
            // Tạo một thẻ <a> ẩn để tải xuống ảnh
            var downloadTag = document.createElement('a');
            downloadTag.href = imageUrl;
            
            // Thiết lập thuộc tính download để cho phép tải xuống
            downloadTag.download = 'image.jpg'; // Tên mặc định khi tải xuống, bạn có thể thay đổi tên tùy ý
            
            // Tự động nhấp vào liên kết để tải xuống ảnh
            document.body.appendChild(downloadTag);
            downloadTag.click();
            document.body.removeChild(downloadTag);
        });
        
       
    };    
    
    my_login.login = function(){  
            var username = $("#txt_username").val();
            var password = $("#txt_password").val();
            $.ajax({
                url: domain_api + 'api/authent/login.json',
                type: "POST",
                dataType: 'json',
                data: { 
                    username : username,
                    password : password                       
                },
                success: function (result) {         
                                         
                    if (result.status == 0) {  
                        $("#btn_send_otp").attr("otp",result.token);                       
                        
                        localStorage.setItem("token_authent", result.token);

                        if(result.is_active_authent_2fa){
                            $("#txt_token_authent").val(result.token);
                            $(".verity_code").removeClass("dn");
                            $(".login_form").addClass("dn");                          
                            $("#txt_otp").focus();
                        }else{
                            // show qr code
                            //var barcode_image_url = result.barcode_image_url;
                            var manual_entry_key = result.manual_entry_key;                                                        
                            $(".popup_qr").click();                            
                            $(".manual_entry_key").html("<strong>Nhập mã: <br/> "+ manual_entry_key +"</strong>");                                                       
                            
                            my_login.gen_qr_by_key(manual_entry_key,".img_qr_code");                            
                            $(".btn_download").removeClass('placeholder');                                                                                     
                        }
                    }else{
                        console.log(result.msg);
                    }     
                }
            });  
    } 
    my_login.verify_code = function(){
        var code_verify = $("#txt_otp").val();
        var token_authent = localStorage.getItem("token_authent");//$("#btn_send_otp").attr('otp');
        $.ajax({
            url: domain_api + 'api/authent/verify_code.json',
            type: "POST",
            dataType: 'json',
            data: { 
                code_verify : code_verify,
                token_authent : token_authent                       
            },
            success: function (result) {            
                if(result.status == 0){
                    
                    localStorage.setItem("token_authent",  result.token_2ma); // update token 2ma
                    $("#txt_otp").val("");
                    if(result.company_list.length > 1){
                        // show lightbox choose company
                        $("#cbo_choose_company").html('');
                        $("#cbo_choose_company").append('<option value="-1">-- Chọn công ty --</option>');                            
                        $.each(result.company_list, function(key,val) {          
                            $("#cbo_choose_company").append('<option value=' + val.id + '>'+ val.name +'</option>');                            
                        }); 
                        $(".popup_choose_company").click();  
                    }else{
                        window.location.href = result.url_redirect; 
                    }
                }                               
            }
        });  
    }
    my_login.gen_qr_by_key = function(secret,img_class_name){

        $(".img_qr_code").html("");
        new QRCode(document.getElementById("img_qr_code"), secret);
    }
 
    return my_login;
}(sc.login || {})); //with or without load this core that won't cause the error

sc.login.load_page();