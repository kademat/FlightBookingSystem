using FlightBooking.Domain.Interfaces;
using FlightBooking.Domain.Models;

public class BirthdayDiscount : IDiscountCriteria
{
    public bool IsApplicable(Flight flight, DateTime purchaseDate, DateTime? buyerBirthDate)
    {
        if (!buyerBirthDate.HasValue)
        {
            return false; // no birth date - no discount
        }

        return flight.DepartureTime.Date == buyerBirthDate.Value.Date;
    }

    public decimal GetDiscountAmount()
    {
        return 5m;
    }
}