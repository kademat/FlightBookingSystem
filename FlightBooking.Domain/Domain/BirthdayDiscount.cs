using FlightBooking.Domain.Interfaces;
using FlightBooking.Domain.Models;

public class BirthdayDiscount : IDiscountCriteria
{
    public bool IsApplicable(Flight flight, DateTime? buyerBirthDate)
    {
        if (!buyerBirthDate.HasValue)
        {
            return false; // Without birth date - there is no discount
        }

        // Ignored potential time zone discrepancies. For example, due to time differences (e.g., in Australia), 
        // a person might already have their birthday in their local time zone while it is still the previous day in another.
        // Ideally, for a production-grade solution, I would address such edge cases and discuss the requirements further.
        // Here, a simplified approach is used by comparing only the dates without considering time zones.
        return flight.DepartureTime.Date == buyerBirthDate.Value.Date;
    }

    public decimal GetDiscountAmount()
    {
        return 5m;
    }

    public string GetDescription() => "Birthday Discount";
}