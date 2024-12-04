using FlightBooking.Domain.Interfaces;

public class DiscountFactory : IDiscountFactory
{
    public IEnumerable<IDiscountCriteria> CreateDiscounts()
    {
        return new List<IDiscountCriteria>
        {
            new BirthdayDiscount(),
            new AfricaFlightDiscount()
        };
    }
}