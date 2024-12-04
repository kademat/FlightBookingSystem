using FlightBooking.Domain.Models;

public interface IFlightRepository
{
    void AddFlight(Flight flight);
    void UpdateFlight(string flightId, Flight updatedFlight);
    void RemoveFlight(string flightId);
    IEnumerable<Flight> SearchFlights(string? from = null, string? to = null, DateTime? departureDate = null, DayOfWeek[]? daysOfWeek = null);
    Flight? GetFlightById(string flightId);
}