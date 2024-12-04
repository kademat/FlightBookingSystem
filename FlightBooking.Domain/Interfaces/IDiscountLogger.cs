public interface IDiscountLogger
{
    void Log(DiscountLog log);
    List<DiscountLog> GetLogs(string? flightId = null, DateTime? from = null, DateTime? to = null);
}