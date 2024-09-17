using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
    public class AllCodeType
    {
        public static string UTM_SOURCE = "UTM_SOURCE";
        public static string PAYMENT_STATUS = "PAYMENT_STATUS";
        public static string PAYMENT_TYPE = "PAYMENT_TYPE";
        public static string PAY_TYPE = "PAY_TYPE";
        public static string ARTICLE_STATUS = "ARTICLE_STATUS";
        public static string GROUP_PROVIDER_TYPE = "GROUP_PROVIDER_TYPE";
        public static string CLIENT_TYPE = "CLIENT_TYPE";
        public static string SERVICE_TYPE = "SERVICE_TYPE";
        public static string SYSTEM_TYPE = "SYSTEM_TYPE";
        public static string DEPOSITHISTORY_TYPE = "DEPOSITHISTORY_TYPE";
        public static string DEPOSIT_STATUS = "DEPOSIT_STATUS";
        public static string ALLOTMENT_TYPE = "ALLOTMENT_TYPE";
        public static string AGENCY_TYPE = "AGENCY_TYPE";
        public static string PERMISION_TYPE = "PERMISION_TYPE";
        public static string VERIFY_STATUS = "VERIFY_STATUS";
        public static string BRANCH_CODE = "BRANCH_CODE";
        public static string CONTRACT_PAY_TYPE = "CONTRACT_PAY_TYPE";
        public static string ORDER_STATUS = "ORDER_STATUS";
        public static string CONTRACT_STATUS = "CONTRACT_STATUS";
        public static string TOUR_TYPE = "TOUR_TYPE";
        public static string ORGANIZING_TYPE = "ORGANIZING_TYPE";
        public static string PAYMENT_VOUCHER_TYPE = "PAYMENT_VOUCHER_TYPE";
        public static string PAYMENT_REQUEST_STATUS = "PAYMENT_REQUEST_STATUS";
        public static string INVOICE_REQUEST_STATUS = "INVOICE_REQUEST_STATUS";
        public static string BOOKING_HOTEL_STATUS = "BOOKING_HOTEL_STATUS";
        public static string BOOKING_HOTEL_ROOM_STATUS = "BOOKING_HOTEL_ROOM_STATUS";
        public static string DEBT_TYPE = "DEBT_TYPE";
        public static string TourPackageOptional = "TourPackageOptional";
        public static string SERVICE_TYPE_OTHER = "SERVICE_TYPE_OTHER";
        public static string ORDER_DEBT_STATUS = "ORDER_DEBT_STATUS";
        public static string CONTRACTPAY_DEBT_STATUS = "CONTRACTPAY_DEBT_STATUS";
        public static string PROGRAM_STATUS = "PROGRAM_STATUS";
        public static string WATER_SPORT_TYPE = "WATER_SPORT_TYPE";
        public static string SERVICE_TYPE_OTHER_MAIN = "SERVICE_TYPE_OTHER_MAIN";
        public static string DEBT_STATISTIC_STATUS = "DEBT_STATISTIC_STATUS";
        public static string HOTEL_GUEST_TYPE = "HOTEL_GUEST_TYPE";
        public static string WEEKDAY_TYPE = "WEEKDAY_TYPE";
        public static string PROJECT_TYPE = "PROJECT_TYPE";


        public static string INDUSTRY_SPECIAL_TYPE = "INDUSTRY_SPECIAL_TYPE";
        public static string WEIGHT_UNIT = "WEIGHT_UNIT";
        public static string PRODUCT_SPECIFICATION = "PRODUCT_SPECIFICATION";
    }
    public class AllCodeDescription
    {
        public static string WATER_SPORT = "Service_Type thể thao biển";

    }

    public class CommonConstant
    {
        public enum CommonStatus
        {
            ACTIVE = 1,
            INACTIVE = 0
        }
        public enum CommonGender
        {
            MALE = 1,
            FEMALE = 0
        }
        public enum FlyBookingDetailType
        {
            GO = 0, // chiều đi
            BACK = 1 // chiều về
        }
        public const string PersonType_ADULT = "ADT";
        public const string PersonType_CHILDREN = "CHD";
        public const string PersonType_INFANT = "INF";
    }
}
