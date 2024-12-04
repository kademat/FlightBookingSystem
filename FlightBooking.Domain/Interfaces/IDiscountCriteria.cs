﻿using FlightBooking.Domain.Models;

namespace FlightBooking.Domain.Interfaces;

public interface IDiscountCriteria
{
    bool IsApplicable(Flight flight, DateTime flightDate, DateTime? buyerBirthDate);
    decimal GetDiscountAmount();
    string GetDescription();
}