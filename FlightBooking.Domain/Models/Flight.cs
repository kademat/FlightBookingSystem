namespace FlightBooking.Domain.Models;

public class Flight
{
    public string FlightId { get; set; } // Validated ID
    public string From { get; set; }
    public string To { get; set; }
    public DateTime DepartureTime { get; set; }
    public DayOfWeek[] DaysOfWeek { get; set; }

    public Flight(string flightId, string from, string to, DateTime departureTime, DayOfWeek[] daysOfWeek)
    {
        // Should be a validation for flightId
        FlightId = flightId;
        From = from;
        To = to;
        DepartureTime = departureTime;
        DaysOfWeek = daysOfWeek;
    }
}