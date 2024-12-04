public class DiscountLog
{
    public string FlightId { get; set; }
    public string TenantId { get; set; }
    public List<string> AppliedDiscounts { get; set; } = new();

    public DiscountLog(string flightId, string tenantId)
    {
        FlightId = flightId;
        TenantId = tenantId;
    }
}