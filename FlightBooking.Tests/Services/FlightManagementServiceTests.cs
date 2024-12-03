using FlightBooking.Domain.Models;
using Moq;

[TestFixture]
public class FlightManagementServiceTests
{
    private FlightManagementService _flightManagementService;
    private FlightRepository _flightRepository;

    [SetUp]
    public void SetUp()
    {
        _flightRepository = new FlightRepository();
        _flightManagementService = new FlightManagementService(_flightRepository);
    }

    [Test]
    public void Should_Add_Flight()
    {
        var flight = new Flight("TEST123", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.AddFlight(flight);

        var retrievedFlight = _flightManagementService.GetFlightById("TEST123");

        Assert.IsNotNull(retrievedFlight);
        Assert.That(retrievedFlight.FlightId, Is.EqualTo("TEST123"));
    }

    [Test]
    public void Should_Update_Flight()
    {
        var flight = new Flight("TEST123", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.AddFlight(flight);

        var updatedFlight = new Flight("TEST123", "NYC", "SFO", DateTime.Today.AddDays(1), new[] { DayOfWeek.Tuesday });
        _flightManagementService.UpdateFlight("TEST123", updatedFlight);

        var retrievedFlight = _flightManagementService.GetFlightById("TEST123");

        Assert.IsNotNull(retrievedFlight);
        Assert.That(retrievedFlight.To, Is.EqualTo("SFO"));
    }

    [Test]
    public void Should_Remove_Flight()
    {
        var flight = new Flight("TEST123", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.AddFlight(flight);

        _flightManagementService.RemoveFlight("TEST123");

        var retrievedFlight = _flightManagementService.GetFlightById("TEST123");

        Assert.IsNull(retrievedFlight);
    }

    [Test]
    public void Should_Search_Flights_By_Criteria()
    {
        // Arrange
        var mondayFlightDate = DateTime.Today;
        while (mondayFlightDate.DayOfWeek != DayOfWeek.Monday)
        {
            mondayFlightDate = mondayFlightDate.AddDays(1);
        }

        var tuesdayFlightDate = mondayFlightDate.AddDays(1);

        var flight1 = new Flight("TEST001", "NYC", "LAX", mondayFlightDate, new[] { DayOfWeek.Monday });
        var flight2 = new Flight("TEST002", "LAX", "NYC", tuesdayFlightDate, new[] { DayOfWeek.Tuesday });
        _flightManagementService.AddFlight(flight1);
        _flightManagementService.AddFlight(flight2);

        // Act
        var results = _flightManagementService.SearchFlights("NYC", "LAX", mondayFlightDate, new[] { DayOfWeek.Monday });

        // Assert
        Assert.That(results.Count(), Is.EqualTo(1));
        Assert.That(results.First().FlightId, Is.EqualTo("TEST001"));
    }

    [Test]
    public void Should_Call_AddFlight_On_Repository()
    {
        // Arrange
        var mockRepository = new Mock<IFlightRepository>();
        var flightManagementService = new FlightManagementService(mockRepository.Object);
        var flight = new Flight("TEST123", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });

        // Act
        flightManagementService.AddFlight(flight);

        // Assert
        mockRepository.Verify(repo => repo.AddFlight(It.IsAny<Flight>()), Times.Once);
    }
}