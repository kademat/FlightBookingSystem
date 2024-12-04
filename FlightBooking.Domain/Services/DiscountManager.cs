using FlightBooking.Domain.Interfaces;
using FlightBooking.Domain.Models;

public class DiscountManager
{
    private readonly List<IDiscountCriteria> _discountCriteriaList = new();
    private readonly IDiscountLogger _logger;

    public DiscountManager(IDiscountLogger logger)
    {
        _logger = logger;
    }

    public void AddDiscountCriteria(IDiscountCriteria discountCriteria)
    {
        _discountCriteriaList.Add(discountCriteria);
    }

    public void RegisterDiscountFactory(IDiscountFactory factory)
    {
        var discounts = factory.CreateDiscounts();
        foreach (var discount in discounts)
        {
            AddDiscountCriteria(discount);
        }
    }

    public (decimal totalDiscount, List<string> appliedDiscounts) ApplyDiscounts(
        Flight flight,
        DateTime? buyerBirthDate,
        bool logDiscounts)
    {
        decimal totalDiscount = 0m;
        var appliedDiscounts = new List<string>();

        foreach (var criteria in _discountCriteriaList)
        {
            if (criteria.IsApplicable(flight, buyerBirthDate))
            {
                totalDiscount += criteria.GetDiscountAmount();
                appliedDiscounts.Add(criteria.GetDescription());
            }
        }

        if (logDiscounts)
        {
            var log = new DiscountLog(flight.FlightId)
            {
                AppliedDiscounts = new List<string>(appliedDiscounts),
                TotalDiscount = totalDiscount,
                BuyerBirthDate = buyerBirthDate
            };
            _logger.Log(log);
        }

        return (totalDiscount, appliedDiscounts);
    }
}