using System;

namespace FlightBooking.Domain.Services
{
    public class DiscountService
    {
        private const decimal MinPrice = 20m;
        private const decimal DiscountAmount = 5m;

        public decimal CalculateDiscountedPrice(decimal basePrice, DateTime flightDate, DateTime? buyerBirthDate, bool isToAfricaOnThursday)
        {
            decimal discountedPrice = basePrice;

            if (buyerBirthDate.HasValue && buyerBirthDate.Value.Date == flightDate.Date)
            {
                discountedPrice -= DiscountAmount;
            }

            if (isToAfricaOnThursday)
            {
                discountedPrice -= DiscountAmount;
            }

            return discountedPrice < MinPrice ? MinPrice : discountedPrice;
        }
    }
}