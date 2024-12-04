public class TicketPrice
{
    public decimal BasePrice { get; set; }

    public TicketPrice(decimal basePrice)
    {
        BasePrice = basePrice;
    }
}