public class InMemoryDiscountLogger : IDiscountLogger
{
    private readonly List<DiscountLog> _logs = new();

    public void Log(DiscountLog log)
    {
        _logs.Add(log);
    }

    public List<DiscountLog> GetLogs(string? flightId = null, DateTime? from = null, DateTime? to = null)
    {
        return _logs.Where(log =>
            (string.IsNullOrEmpty(flightId) || log.FlightId == flightId) &&
            (!from.HasValue || log.LogDate >= from.Value) &&
            (!to.HasValue || log.LogDate <= to.Value))
            .ToList();
    }
}