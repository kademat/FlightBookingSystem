using FlightBooking.Domain.Models;

namespace FlightBooking.Domain.Services
{
    public class DiscountService
    {
        private readonly DiscountManager _discountManager;

        public DiscountService(DiscountManager discountManager)
        {
            _discountManager = discountManager;
        }

        public decimal CalculateDiscountedPrice(decimal basePrice, Flight flight, DateTime purchaseDate, DateTime? buyerBirthDate)
        {
            decimal totalDiscount = _discountManager.ApplyDiscounts(flight, purchaseDate, buyerBirthDate);

            decimal discountedPrice = basePrice - totalDiscount;

            return discountedPrice < 20m ? 20m : discountedPrice;
        }
    }
}