using FlightBooking.Domain.Enums;
using FlightBooking.Domain.Interfaces;

namespace FlightBooking.Domain.Services
{
    public class DiscountService : IDiscountService
    {
        private const decimal MinPrice = 20m;
        private readonly IDiscountManager _discountManager;

        public DiscountService(IDiscountManager discountManager)
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

            var flightBasePrice = flight.TicketService.GetBasePrice();

            if (!flightBasePrice.HasValue)
            {
                throw new ArgumentException($"There is no base price for flight id {flight.FlightId}.");
            }

            decimal discountedPrice = flightBasePrice.Value - totalDiscount;

            // Here it's assumed that if price after all discounts is below MinPrice there are no discounts at all.
            return discountedPrice < MinPrice ? flightBasePrice.Value : discountedPrice;
        }
    }
}