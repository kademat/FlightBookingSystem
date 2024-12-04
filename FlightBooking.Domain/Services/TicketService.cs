using System.Net.Sockets;

public class TicketService
{
    private readonly Queue<TicketPrice> _tickets = new();

    public void AddTicket(TicketPrice ticket)
    {
        _tickets.Enqueue(ticket);
    }

    public void SellTicket(TicketPrice ticket)
    {
        if (_tickets.Count == 0 || !_tickets.Peek().Equals(ticket))
        {
            throw new InvalidOperationException($"The ticket is no longer available for flight.");
        }

        _tickets.Dequeue();
    }

    public TicketPrice? PeekTicket()
    {
        return _tickets.Count > 0 ? _tickets.Peek() : null;
    }

    public decimal? GetBasePrice()
    {
        return _tickets.Count > 0 ? _tickets.Peek().BasePrice : null;
    }

    public int TicketsCount => _tickets.Count;
}