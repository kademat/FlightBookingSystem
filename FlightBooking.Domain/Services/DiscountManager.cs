using FlightBooking.Domain.Interfaces;
using FlightBooking.Domain.Models;

public class DiscountManager
{
    private readonly List<IDiscountCriteria> _discountCriteriaList = new();
    private readonly List<DiscountLog> _discountLogs = new();

    public void AddDiscountCriteria(IDiscountCriteria discountCriteria)
    {
        _discountCriteriaList.Add(discountCriteria);
    }

    public (decimal totalDiscount, List<string> appliedDiscounts) ApplyDiscounts(
        Flight flight,
        DateTime purchaseDate,
        DateTime? buyerBirthDate,
        bool logDiscounts,
        string tenantId)
    {
        decimal totalDiscount = 0m;
        var appliedDiscounts = new List<string>();

        foreach (var criteria in _discountCriteriaList)
        {
            if (criteria.IsApplicable(flight, purchaseDate, buyerBirthDate))
            {
                totalDiscount += criteria.GetDiscountAmount();
                appliedDiscounts.Add(criteria.GetDescription());
            }
        }

        if (logDiscounts)
        {
            var log = new DiscountLog(flight.FlightId, tenantId)
            {
                AppliedDiscounts = appliedDiscounts
            };
            _discountLogs.Add(log);
        }

        return (totalDiscount, appliedDiscounts);
    }

    public List<DiscountLog> GetLogs() => _discountLogs;
}