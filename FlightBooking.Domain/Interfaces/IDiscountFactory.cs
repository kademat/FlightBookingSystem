namespace FlightBooking.Domain.Interfaces;

public interface IDiscountFactory
{
    IEnumerable<IDiscountCriteria> CreateDiscounts();
}