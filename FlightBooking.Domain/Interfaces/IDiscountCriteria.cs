using FlightBooking.Domain.Models;

namespace FlightBooking.Domain.Interfaces;

public interface IDiscountCriteria
{
    bool IsApplicable(Flight flight, DateTime purchaseDate, DateTime? buyerBirthDate);
    decimal GetDiscountAmount();
}