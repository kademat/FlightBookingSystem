public class FlightPrice
{
    public string FlightId { get; set; }
    public DateTime Date { get; set; }
    public decimal BasePrice { get; set; }

    public FlightPrice(string flightId, DateTime date, decimal basePrice)
    {
        FlightId = flightId;
        Date = date;
        BasePrice = basePrice;
    }
}