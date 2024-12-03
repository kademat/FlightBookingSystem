using FlightBooking.Domain.Models;

public class FlightManagementService
{
    private readonly IFlightRepository _flightRepository;

    public FlightManagementService(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
    }

    public void AddFlight(Flight flight)
    {
        _flightRepository.AddFlight(flight);
    }

    public void UpdateFlight(string flightId, Flight updatedFlight)
    {
        _flightRepository.UpdateFlight(flightId, updatedFlight);
    }

    public void RemoveFlight(string flightId)
    {
        _flightRepository.RemoveFlight(flightId);
    }

    public IEnumerable<Flight> SearchFlights(string? from, string? to, DateTime? departureDate, DayOfWeek[]? daysOfWeek)
    {
        return _flightRepository.SearchFlights(from, to, departureDate, daysOfWeek);
    }

    public Flight? GetFlightById(string flightId)
    {
        return _flightRepository.GetFlightById(flightId);
    }
}