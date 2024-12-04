using FlightBooking.Domain.Domain;
using FlightBooking.Domain.Enums;
using FlightBooking.Domain.Interfaces;
using FlightBooking.Domain.Models;

public class AfricaFlightDiscount : IDiscountCriteria
{
    public bool IsApplicable(Flight flight, DateTime flightDate, DateTime? buyerBirthDate)
    {
        bool isAfricaFlight = AirportContinentMapper.GetContinent(flight.To) == Continent.Africa;
        bool isThursday = flight.DepartureTime.DayOfWeek == DayOfWeek.Thursday;

        return isAfricaFlight && isThursday;
    }

    public decimal GetDiscountAmount()
    {
        return 5m;
    }

    public string GetDescription() => "Applies a discount for flights to Africa departing on Thursday.";
}