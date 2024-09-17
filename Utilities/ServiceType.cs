using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public enum ServiceType
    {
        PRODUCT_FLY_TICKET = 3,
        BOOK_HOTEL_ROOM = 2,
        //  CAR_RENTAL = 4,
        BOOK_HOTEL_ROOM_VIN = 1,
        Tour = 5,
        VinWonder = 6,
        // AmusementParkTickets = 7,
        //  FoodandBeverageService = 8,
        Other = 9,
        WaterSport = 10
    }
    public enum SubServiceType
    {
        PRODUCT_FLY_TICKET = 31,
        BOOK_HOTEL_ROOM_VIN = 30,
        Tour = 32,
        Other = 33,
    }
    public enum EmailType
    {
        PRODUCT_FLY_TICKET = 3,
        BOOK_HOTEL_ROOM = 2,
        CAR_RENTAL = 4,
        BOOK_HOTEL_ROOM_VIN = 1,
        Tour = 5,
        VinWonder = 6,
        DON_HANG = 0,
        DON_HANG_Fly = 33,
        Supplier = 8,
        SaleDH = 9,//guiwe sale vs điều hành

    }
}
