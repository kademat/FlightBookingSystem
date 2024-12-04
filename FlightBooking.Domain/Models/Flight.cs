namespace FlightBooking.Domain.Models;

public class Flight
{
    public string FlightId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public DateTime DepartureTime { get; set; }
    public DayOfWeek[] DaysOfWeek { get; set; }
    public Queue<TicketPrice> _tickets { get; } = new();


    public Flight(string flightId, string from, string to, DateTime departureTime, DayOfWeek[] daysOfWeek)
    {
        FlightValidator.ValidateFlightId(flightId);

        FlightId = flightId;
        From = from;
        To = to;
        DepartureTime = departureTime;
        DaysOfWeek = daysOfWeek;
    }

    /// <summary>
    /// Adds a ticket to the queue.
    /// </summary>
    /// <param name="price">Price of the ticket</param>
    public void AddTicket(TicketPrice price)
    {
        _tickets.Enqueue(price);
    }

    public void SellTicket(TicketPrice ticket)
    {
        if (_tickets.Count == 0 || !_tickets.Peek().Equals(ticket))
        {
            throw new InvalidOperationException($"The ticket is no longer available for flight {FlightId}.");
        }

        _tickets.Dequeue();
    }

    /// <summary>
    /// Gets the number of tickets currently available.
    /// </summary>
    public int TicketsCount => _tickets.Count;

    public TicketPrice? PeekTicket()
    {
        // For prod environment it should be reserved, here for simplicity it's just peeked
        // to show process simulation
        return _tickets.Count > 0 ? _tickets.Peek() : null;
    }


    public decimal? GetBasePrice()
    {
        return _tickets.Count > 0 ? _tickets.Peek().BasePrice : null;
    }
}