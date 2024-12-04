using FlightBooking.Domain.Models;

public class InMemoryFlightRepository : IFlightRepository
{
    private readonly List<Flight> _flights = new();
    private readonly List<TicketPrice> _flightPrices = new();

    public void AddFlight(Flight flight)
    {
        _flights.Add(flight);
    }

    public void UpdateFlight(string flightId, Flight updatedFlight)
    {
        var flight = _flights.FirstOrDefault(f => f.FlightId == flightId);
        if (flight == null)
        {
            throw new KeyNotFoundException($"Flight with ID {flightId} not found.");
        }
        _flights.Remove(flight);
        _flights.Add(updatedFlight);
    }

    public void RemoveFlight(string flightId)
    {
        var flight = _flights.FirstOrDefault(f => f.FlightId == flightId);
        if (flight == null)
        {
            throw new KeyNotFoundException($"Flight with ID {flightId} not found.");
        }
        _flights.Remove(flight);
    }

    public IEnumerable<Flight> SearchFlights(string? from, string? to, DateTime? departureDate, DayOfWeek[]? daysOfWeek)
    {
        return _flights.Where(f =>
            (string.IsNullOrEmpty(from) || f.From == from) &&
            (string.IsNullOrEmpty(to) || f.To == to) &&
            (!departureDate.HasValue || f.DepartureTime.Date == departureDate.Value.Date) &&
            (daysOfWeek == null || daysOfWeek.Contains(f.DepartureTime.DayOfWeek))
        );
    }

    public Flight? GetFlightById(string flightId)
    {
        return _flights.FirstOrDefault(f => f.FlightId == flightId);
    }
}