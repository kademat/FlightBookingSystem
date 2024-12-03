using FlightBooking.Domain.Models;
using System.Collections.Generic;
using System.Linq;

public class FlightRepository : IFlightRepository
{
    private readonly List<Flight> _flights = new();

    public void AddFlight(Flight flight)
    {
        if (_flights.Any(f => f.FlightId == flight.FlightId))
        {
            throw new InvalidOperationException($"Flight with ID {flight.FlightId} already exists.");
        }
        _flights.Add(flight);
    }

    public void UpdateFlight(string flightId, Flight updatedFlight)
    {
        var existingFlight = _flights.FirstOrDefault(f => f.FlightId == flightId);
        if (existingFlight == null)
        {
            throw new KeyNotFoundException($"Flight with ID {flightId} not found.");
        }

        _flights.Remove(existingFlight);
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

    public IEnumerable<Flight> SearchFlights(
        string? from = null,
        string? to = null,
        DateTime? departureDate = null,
        DayOfWeek[]? daysOfWeek = null)
    {
        return _flights.Where(f =>
            (string.IsNullOrEmpty(from) || f.From.Equals(from, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(to) || f.To.Equals(to, StringComparison.OrdinalIgnoreCase)) &&
            (!departureDate.HasValue || f.DepartureTime.Date == departureDate.Value.Date) &&
            (daysOfWeek == null || daysOfWeek.Length == 0 || daysOfWeek.Contains(f.DepartureTime.DayOfWeek))
        );
    }

    public Flight? GetFlightById(string flightId)
    {
        return _flights.FirstOrDefault(f => f.FlightId == flightId);
    }
}