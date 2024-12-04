using FlightBooking.Domain.Enums;
using FlightBooking.Domain.Models;

namespace FlightBooking.Domain.Interfaces;

public interface IDiscountService
{
    decimal CalculateDiscountedPrice(Flight flight, DateTime? buyerBirthDate, TenantGroup tenantGroup);
}