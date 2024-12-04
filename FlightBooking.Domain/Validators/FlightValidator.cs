using System.Text.RegularExpressions;

public static class FlightValidator
{
    // Added copmiled to improve performance
    private static readonly Regex FlightIdRegex = new(@"^[A-Z]{3}\d{5}[A-Z]{3}$", RegexOptions.Compiled);

    /// <summary>
    /// Validates the format of a flight ID.
    /// </summary>
    /// <param name="flightId">The flight ID to validate.</param>
    /// <exception cref="FormatException">Thrown when the flight ID is not in the expected format.</exception>
    public static void ValidateFlightId(string flightId)
    {
        if (string.IsNullOrEmpty(flightId) || !FlightIdRegex.IsMatch(flightId))
        {
            throw new FormatException("Incorrect flight ID. It should be in format: 3 capital letters + 5 digits + 3 capital letters.");
        }
    }
}