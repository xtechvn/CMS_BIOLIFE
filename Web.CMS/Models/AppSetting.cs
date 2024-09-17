using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB.CMS.Models
{
    public class AppSettings
    {
        #region Old Version:
        public string API_TRANSACTION { get; set; }
        public string API_GET_BILL_NO { get; set; }
        public string API_GET_INVOICE_REQUEST_NO { get; set; }
        public string API_ADD_CONTRACTPAY { get; set; }
        public string KEY_TOKEN_API_MANUAL { get; set; }
        public string API_LOG_ACTIVITY { get; set; }
        public string API_ADAVIGO_URL { get; set; }
        public string API_INSERT_LOG_CMS { get; set; }
        public string API_INSERT_LOG_ACTIVITY { get; set; }

        public string API_GET_LOG_CMS { get; set; }
        public string KEY_TOKEN_API_B2B { get; set; }
        public string API_GET_OTHERS_HOTELS { get; set; }
        public string API_GET_VIN_HOTELS { get; set; }
        public string API_GET_HOTEL_ROOM_DETAIL { get; set; }
        public string API_CLEAR_CACHE_BY_KEY { get; set; }
        public string Catalog_General { get; set; }
        public string Collection_RoomProviders { get; set; }
        public string Token_Part { get; set; }
        public string API_GET_ORDER_BY_ORDERID { get; set; }
        public string API_ALLCODE { get; set; }
        public string API_ORDER_LIST { get; set; }
        public string API_Send_Email_Reset_Password { get; set; }
        public string API_Re_Send_Order { get; set; }
        public string Get_Order_no { get; set; }
        public string Get_Product_Category_By_Parent_Id { get; set; }
        public string send_Message { get; set; }
        public string Notify_Get_List { get; set; }
        public string Notify_update_status { get; set; }
        public int Vinwonder_groupproduct_parentID { get; set; }
        public string LockBookingVinwonder { get; set; }
        public string EmailVinWonderTicket { get; set; }
        public string ChromeLocalPath { get; set; }
        public string API_IMAGE_KEY { get; set; }
        public string API_STATIC_UPLOAD { get; set; }
        public string IMAGE_DOMAIN { get; set; }


        public string API_STATIC_UPLOADFILE { get; set; }
        public long SUPPLIERID_ADAVIGO { get; set; }

        public string ROLE_KE_TOAN_TRUONG { get; set; }
        public string ROLE_TRUONG_BO_PHAN { get; set; }
        public int Programs_Menuid { get; set; }
        public string API_UpdateUser { get; set; }
        public string API_GetUserDetail { get; set; }

        public string API_Gen_Qr { get; set; }
        public string API_ADAVIGO_URL_LOGIN { get; set; }

        public string API_ChangePassword { get; set; }
        public string List_Department_ks { get; set; }
        public string upload_QR_payment_order { get; set; }
        public int Department_Operator_Hotel { get; set; }
        public int Department_Operator_Fly { get; set; }
        public int Department_Operator_Tour { get; set; }
        public int Department_Operator_WaterSport { get; set; }
        #endregion
        public string EncryptApi { get; set; }
        //public string API_FR { get; set; }
        public string KEY_ENCODE_TOKEN_PUT_QUEUE { get; set; }
        public string STATIC_IMAGE_DOMAIN { get; set; }
        public string KEY_CMS_UPLOAD { get; set; }
        public string KEY_TOKEN_API { get; set; }
        public string KEY_TOKEN_CMS { get; set; }
        public string API_SYNC_ARTICLE { get; set; }
        public string API_SYNC_CATEGORY { get; set; }
        public string API_URL { get; set; }
        public string ROLE_ADMIN { get; set; }
        public string LoginURL { get; set; }
        public string SIZE_IMG { get; set; }
        public int LoginTokenExprire { get; set; }
        public string AES_KEY { get; set; }
        public string AES_IV { get; set; }
       
        public string Images_Location { get; set; }
        public string Images_URL { get; set; }

      

        
    }
}
