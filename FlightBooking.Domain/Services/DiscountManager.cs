using FlightBooking.Domain.Domain;
using FlightBooking.Domain.Models;

public class DiscountManager
{
    private readonly List<IDiscountCriteria> _discountCriteriaList = new();

    public void RegisterCriteria(IDiscountCriteria criteria)
    {
        _discountCriteriaList.Add(criteria);
    }

    public decimal ApplyDiscounts(Flight flight, DateTime purchaseDate, DateTime? buyerBirthDate)
    {
        decimal totalDiscount = 0m;

        foreach (var criteria in _discountCriteriaList)
        {
            if (criteria.IsApplicable(flight, purchaseDate, buyerBirthDate))
            {
                totalDiscount += criteria.GetDiscountAmount();
            }
        }

        return totalDiscount;
    }
}