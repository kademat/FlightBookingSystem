[TestFixture]
public class FlightValidatorTests
{
    [Test]
    public void ValidateFlightId_ShouldThrowException_WhenIdIsInvalid()
    {
        Assert.Throws<ArgumentException>(() => FlightValidator.ValidateFlightId("12345"));
    }

    [Test]
    public void ValidateDates_ShouldThrowException_WhenDatesAreInvalid()
    {
        Assert.Throws<ArgumentException>(() => FlightValidator.ValidateDates(DateTime.Now, DateTime.Now.AddHours(-1)));
    }

    [Test]
    public void ValidatePassengerCount_ShouldThrowException_WhenCountExceedsCapacity()
    {
        Assert.Throws<ArgumentException>(() => FlightValidator.ValidatePassengerCount(150, 100));
    }
}