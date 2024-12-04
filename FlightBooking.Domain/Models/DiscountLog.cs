public class DiscountLog
{
    public string FlightId { get; set; }
    public DateTime LogDate { get; set; }
    public List<string> AppliedDiscounts { get; set; } = new();
    public decimal TotalDiscount { get; set; }
    public DateTime? BuyerBirthDate { get; set; }

    public DiscountLog(string flightId)
    {
        FlightId = flightId;
        LogDate = DateTime.Now;
    }
}