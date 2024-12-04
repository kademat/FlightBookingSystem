[TestFixture]
public class FlightValidatorTests
{

    [TestCase("")]
    [TestCase("12345678901")] // lenght is ok, but format is not correct
    [TestCase("A333CCC")] // too short
    [TestCase("ASX3333CCXDC")] // too long
    public void ValidateFlightId_ShouldThrowFormatException_WhenIdIsInvalid(string flightId)
    {
        Assert.Throws<FormatException>(() => FlightValidator.ValidateFlightId(flightId));
    }
}