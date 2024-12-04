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

        var ticket = ReserveTicket(flight);
        var finalPrice = _discountService.CalculateDiscountedPrice(flight, buyerBirthDate, tenantGroup);
        ConfirmPurchase(flight, ticket);

        return finalPrice;
    }

    /// <summary>
    /// Reserves a ticket for the flight.
    /// </summary>
    /// <param name="flight">The flight to reserve a ticket for</param>
    /// <returns>The reserved ticket</returns>
    /// <exception cref="InvalidOperationException">Thrown if no tickets are available</exception>
    private TicketPrice ReserveTicket(Flight flight)
    {
        var ticket = flight.PeekTicket(); // Simulates reservation without removal
        if (ticket == null)
        {
            throw new InvalidOperationException($"No tickets available for flight {flight.FlightId}.");
        }

        return ticket;
    }

    /// <summary>
    /// Confirms the purchase by removing the reserved ticket from the flight.
    /// </summary>
    /// <param name="flight">The flight to confirm the purchase for</param>
    /// <param name="ticket">The ticket to be removed</param>
    private void ConfirmPurchase(Flight flight, TicketPrice ticket)
    {
        flight.SellTicket(ticket);
    }
}