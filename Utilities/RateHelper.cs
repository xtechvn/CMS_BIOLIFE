using System;
using Utilities.Contants;

namespace Utilities
{
    public static class RateHelper
    {
        public static double GetFlyBookingProfitVNDValueByTotalProfit(double total_profit, double total_people,int fly_count)
        {
            return total_profit / (double)total_people / (double)fly_count;
        }
        public static double GetFlyBookingProfitValuePerPerson(double profit,int unit_id, double total_price=0)
        {
            try
            {
                switch (unit_id)
                {
                    case (int)PriceUnitType.VND:
                        {
                            return profit;
                        }
                    case (int)PriceUnitType.PERCENT:
                        {
                            return (total_price * profit / (double)100);
                        }
                }
            }
            catch (Exception)
            {
              
            }
            return (double)55000;
        }

    }
}
