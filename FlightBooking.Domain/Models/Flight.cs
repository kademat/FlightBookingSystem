namespace FlightBooking.Domain.Models;

public class Flight
{
    public string FlightId { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public DateTime DepartureDate { get; private set; }
    public DateTime ArrivalDate { get; private set; }
    public DateTime DepartureTime { get; set; }
    public DayOfWeek[] DaysOfWeek { get; set; }
    public int PassengerCount { get; private set; }
    public int MaxCapacity { get; private set; }


    public Flight(string flightId, string from, string to, DateTime departureDate, DateTime arrivalDate, DateTime departureTime, DayOfWeek[] daysOfWeek, int passengerCount, int maxCapacity)
    {
        FlightValidator.ValidateFlightId(flightId);
        FlightValidator.ValidateDates(departureDate, arrivalDate);
        FlightValidator.ValidatePassengerCount(passengerCount, maxCapacity);

        FlightId = flightId;
        From = from;
        To = to;
        DepartureDate = departureDate;
        ArrivalDate = arrivalDate;
        DepartureTime = departureTime;
        DaysOfWeek = daysOfWeek;
        PassengerCount = passengerCount;
        MaxCapacity = maxCapacity;
    }
}