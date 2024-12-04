using FlightBooking.Domain.Interfaces;

public class DiscountFactory
{
    public static IEnumerable<IDiscountCriteria> CreateDiscounts()
    {
        return new List<IDiscountCriteria>
        {
            new BirthdayDiscount(),
            new AfricaFlightDiscount()
        };
    }
}