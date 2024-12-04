using FlightBooking.Domain.Enums;

namespace FlightBooking.Domain.Interfaces;

public interface IFlightBookingService
{
    public decimal BookFlight(string flightId, TenantGroup tenantGroup, DateTime? buyerBirthDate);

}