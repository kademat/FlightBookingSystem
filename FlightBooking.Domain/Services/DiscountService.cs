using FlightBooking.Domain.Enums;
using FlightBooking.Domain.Models;

namespace FlightBooking.Domain.Services
{
    public class DiscountService
    {
        private const decimal MinPrice = 20m;
        private readonly DiscountManager _discountManager;

        public DiscountService(DiscountManager discountManager)
        {
            _discountManager = discountManager;
        }

        public decimal CalculateDiscountedPrice(Flight flight, DateTime? buyerBirthDate, TenantGroup tenantGroup)
        {
            var (totalDiscount, appliedDiscounts) = _discountManager.ApplyDiscounts(
                flight,
                buyerBirthDate,
                logDiscounts: tenantGroup == TenantGroup.A
            );

            var flightPrice = flight.GetPrice();

            if (!flightPrice.HasValue)
            {
                throw new ArgumentException($"There is no price for flight id {flight.FlightId}. Maybe there are no tickets?");
            }

            decimal discountedPrice = flightPrice.Value - totalDiscount;

            // Here it's assumed that if price after all discounts is below MinPrice there are no discounts at all.
            return discountedPrice < MinPrice ? flightPrice.Value : discountedPrice;
        }
    }
}