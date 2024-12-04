public class DiscountLog(string flightId)
{
    public string FlightId { get; set; } = flightId;
    public string TenantId { get; set; }
    public List<Discount> Discounts { get; } = new();
}