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

        public decimal CalculateDiscountedPrice(decimal basePrice, Flight flight, DateTime purchaseDate, DateTime? buyerBirthDate, TenantGroup tenantGroup)
        {
            var (totalDiscount, appliedDiscounts) = _discountManager.ApplyDiscounts(
                flight,
                purchaseDate,
                buyerBirthDate,
                logDiscounts: tenantGroup == TenantGroup.A
            );

            decimal discountedPrice = basePrice - totalDiscount;

            return discountedPrice < MinPrice ? MinPrice : discountedPrice;
        }

        public List<DiscountLog> GetDiscountLogs() => _discountManager.GetLogs();
    }
}