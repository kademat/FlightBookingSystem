using FlightBooking.Domain.Models;
using Moq;

[TestFixture]
public class FlightManagementServiceTests
{
    private FlightManagementService _flightManagementService;
    private IFlightRepository _flightRepository;

    [SetUp]
    public void SetUp()
    {
        _flightRepository = new InMemoryFlightRepository();
        _flightManagementService = new FlightManagementService(_flightRepository);
    }

    [Test]
    public void Should_Add_Flight()
    {
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today, new[] { DayOfWeek.Monday }, 5, 120);
        _flightManagementService.AddFlight(flight);

        var retrievedFlight = _flightManagementService.GetFlightById("KLM12345BCA");

        Assert.IsNotNull(retrievedFlight);
        Assert.That(retrievedFlight.FlightId, Is.EqualTo("KLM12345BCA"));
    }

    [Test]
    public void Should_Update_Flight()
    {
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today, new[] { DayOfWeek.Monday }, 5, 120);
        _flightManagementService.AddFlight(flight);

        var updatedFlight = new Flight("KLM12345BCA", "NYC", "SFO", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today, new[] { DayOfWeek.Monday }, 5, 120);
        _flightManagementService.UpdateFlight("KLM12345BCA", updatedFlight);

        var retrievedFlight = _flightManagementService.GetFlightById("KLM12345BCA");

        Assert.IsNotNull(retrievedFlight);
        Assert.That(retrievedFlight.To, Is.EqualTo("SFO"));
    }

    [Test]
    public void Should_Remove_Flight()
    {
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today, new[] { DayOfWeek.Monday }, 5, 120);
        _flightManagementService.AddFlight(flight);

        _flightManagementService.RemoveFlight("KLM12345BCA");

        var retrievedFlight = _flightManagementService.GetFlightById("KLM12345BCA");

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

        var flight1 = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today, new[] { DayOfWeek.Monday }, 5, 120);
        var flight2 = new Flight("KLM12346BCA", "NYC", "NYC", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today, new[] { DayOfWeek.Tuesday }, 5, 120);
        _flightManagementService.AddFlight(flight1);
        _flightManagementService.AddFlight(flight2);

        // Act
        var results = _flightManagementService.SearchFlights("NYC", "LAX", mondayFlightDate, new[] { DayOfWeek.Monday });

        // Assert
        Assert.That(results.Count(), Is.EqualTo(1));
        Assert.That(results.First().FlightId, Is.EqualTo("KLM12345BCA"));
    }

    [Test]
    public void Should_Call_AddFlight_On_Repository()
    {
        // Arrange
        var mockRepository = new Mock<IFlightRepository>();
        var flightManagementService = new FlightManagementService(mockRepository.Object);
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today, new[] { DayOfWeek.Monday }, 5, 120);

        // Act
        flightManagementService.AddFlight(flight);

        // Assert
        mockRepository.Verify(repo => repo.AddFlight(It.IsAny<Flight>()), Times.Once);
    }
}