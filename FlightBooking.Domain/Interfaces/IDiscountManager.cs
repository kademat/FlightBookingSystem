using FlightBooking.Domain.Enums;

namespace FlightBooking.Domain.Interfaces;

public interface IDiscountManager
{
    public void AddDiscountCriteria(IDiscountCriteria discountCriteria);

    public void RegisterDiscountFactory(IDiscountFactory factory);

    public (decimal totalDiscount, List<string> appliedDiscounts) ApplyDiscounts(Flight flight, DateTime? buyerBirthDate, bool logDiscounts);
}