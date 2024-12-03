using System.Text.RegularExpressions;

public class FlightValidator
{
    public static void ValidateFlightId(string flightId)
    {
        if (string.IsNullOrEmpty(flightId) || !Regex.IsMatch(flightId, @"^[A-Z]{3}\d{5}[A-Z]{3}$"))
        {
            throw new ArgumentException("Incorrect flight ID. It should be in format: 3 capital letters + 5 digits + 3 capital letters.");
        }
    }

    public static void ValidateDates(DateTime departureDate, DateTime arrivalDate)
    {
        if (departureDate >= arrivalDate)
        {
            throw new ArgumentException("Departure date should be before arrival date.");
        }
    }

    public static void ValidatePassengerCount(int passengerCount, int maxCapacity)
    {
        if (passengerCount < 0 || passengerCount > maxCapacity)
        {
            throw new ArgumentException($"Max passangers ({passengerCount}) is above max capacity ({maxCapacity}).");
        }
    }
}