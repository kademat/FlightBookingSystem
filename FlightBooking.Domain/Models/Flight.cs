public class Flight
{
    public string FlightId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public DateTime DepartureTime { get; set; }
    public DayOfWeek[] DaysOfWeek { get; set; }
    public TicketService TicketService { get; } = new();

    public Flight(string flightId, string from, string to, DateTime departureTime, DayOfWeek[] daysOfWeek)
    {
        FlightValidator.ValidateFlightId(flightId);

        FlightId = flightId;
        From = from;
        To = to;
        DepartureTime = departureTime;
        DaysOfWeek = daysOfWeek;
    }
}