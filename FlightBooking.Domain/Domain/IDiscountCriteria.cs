using FlightBooking.Domain.Models;

namespace FlightBooking.Domain.Domain;

public interface IDiscountCriteria
{
    bool IsApplicable(Flight flight, DateTime purchaseDate, DateTime? buyerBirthDate);
    decimal GetDiscountAmount();
}