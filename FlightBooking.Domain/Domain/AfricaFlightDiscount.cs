using FlightBooking.Domain.Interfaces;
using FlightBooking.Domain.Models;

public class AfricaFlightDiscount : IDiscountCriteria
{
    public bool IsApplicable(Flight flight, DateTime purchaseDate, DateTime? buyerBirthDate)
    {
        // should be fixed - just first implementation
        bool isAfricaFlight = flight.To.ToUpper().StartsWith("AFR");

        bool isThursday = flight.DepartureTime.DayOfWeek == DayOfWeek.Thursday;

        return isAfricaFlight && isThursday;
    }

    public decimal GetDiscountAmount()
    {
        return 5m;
    }

    public string GetDescription() => "Discount for flights to Africa on Thursday";
}