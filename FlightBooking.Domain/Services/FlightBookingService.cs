using FlightBooking.Domain.Enums;
using FlightBooking.Domain.Models;
using FlightBooking.Domain.Services;

public class FlightBookingService
{
    private readonly IFlightRepository _flightRepository;
    private readonly DiscountService _discountService;

    public FlightBookingService(IFlightRepository flightRepository, DiscountService discountService)
    {
        _flightRepository = flightRepository;
        _discountService = discountService;
    }

    public decimal BookFlight(string flightId, TenantGroup tenantGroup, DateTime? buyerBirthDate)
    {
        var flight = _flightRepository.GetFlightById(flightId);
        if (flight == null)
        {
            throw new KeyNotFoundException($"Flight with ID {flightId} not found.");
        }

        var basePrice = GetBasePrice(flight);
        var finalPrice = _discountService.CalculateDiscountedPrice(basePrice, flight, DateTime.Now, buyerBirthDate, tenantGroup);

        return finalPrice;
    }

    private decimal GetBasePrice(Flight flight)
    {
        return 100m;
    }
}