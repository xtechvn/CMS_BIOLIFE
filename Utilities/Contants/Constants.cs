using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Utilities.Contants
{
    public class Constants
    {
        public const string Success = "SUCCESS";
        public const string Fail = "FAIL";
        public const string Error = "ERROR";
       

        public enum NoteType
        {
            ORDER = 1,
            ORDER_ITEM = 2
        }
       
        public enum Chart_Revenu_Type
        {
            Week = 1,
            Month = 2,
        }
        public enum Chart_Label_Type
        {
            Today = 1,
            Yesterday = 2,
            Week = 3,
            Month = 4,
        }
        public enum Chart_Type_Label
        {
            Revenu = 1,
            Quantity = 2,
        }
        public enum Month
        {
            Monday = DayOfWeek.Monday,
        }

        public enum AllCodeTypeEqualsPROJECT_TYPESortById 
        {
            Default = 0,
        }
    }
}
