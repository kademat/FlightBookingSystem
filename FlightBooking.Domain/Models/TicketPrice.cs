public class TicketPrice
{
    public decimal BasePrice { get; set; }
    // posibility to add different currencies, but just a simple example here

    public TicketPrice(decimal basePrice)
    {
        BasePrice = basePrice;
    }
}