using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Contants
{
    public enum LogActivityType
    {
        ERROR=-1,
        CHANGE_ORDER_CMS=0,
        LOGIN_CMS = 1,
        CHANGE_ORDER_BY_KERRRY=2,
        PRODUCT_DETAIL_CHANGE=3,
       AUTOMATIC_PURCHASE_CHANGE=4
    }
  
    public static class LogActivityBSONDocuments
    {
        public static string CMS = "UsersLogActivity";
        public static string FE_Common = "UsersLogActivityFE";
        public static string API = "UsersLogActivityAPI";

    }

}
