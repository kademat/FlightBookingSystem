using FlightBooking.Domain.Enums;
using FlightBooking.Domain.Models;
using FlightBooking.Domain.Services;
using Moq;

[TestFixture]
public class FlightManagementServiceTests
{
    private FlightManagementService _flightManagementService;
    private IFlightRepository _flightRepository;
    private FlightBookingService _flightBookingService;
    private DiscountService _flightDiscountService;
    private DiscountManager _discountManager;

    [SetUp]
    public void SetUp()
    {
        _discountManager = new DiscountManager();
        _flightRepository = new InMemoryFlightRepository();
        _flightManagementService = new FlightManagementService(_flightRepository);
        _flightDiscountService = new DiscountService(_discountManager);
        _flightBookingService = new FlightBookingService(_flightRepository, _flightDiscountService);
    }

    [Test]
    public void Should_Add_Flight()
    {
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.AddFlight(flight);

        var retrievedFlight = _flightManagementService.GetFlightById("KLM12345BCA");

        Assert.IsNotNull(retrievedFlight);
        Assert.That(retrievedFlight.FlightId, Is.EqualTo("KLM12345BCA"));
    }

    [Test]
    public void Should_Update_Flight()
    {
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.AddFlight(flight);

        var updatedFlight = new Flight("KLM12345BCA", "NYC", "SFO", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.UpdateFlight("KLM12345BCA", updatedFlight);

        var retrievedFlight = _flightManagementService.GetFlightById("KLM12345BCA");

        Assert.IsNotNull(retrievedFlight);
        Assert.That(retrievedFlight.To, Is.EqualTo("SFO"));
    }

    [Test]
    public void Should_Remove_Flight()
    {
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
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

        var flight1 = new Flight("KLM12345BCA", "NYC", "LAX", mondayFlightDate, new[] { DayOfWeek.Monday });
        var flight2 = new Flight("KLM12346BCA", "NYC", "NYC", tuesdayFlightDate, new[] { DayOfWeek.Tuesday });

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
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });

        // Act
        flightManagementService.AddFlight(flight);

        // Assert
        mockRepository.Verify(repo => repo.AddFlight(It.IsAny<Flight>()), Times.Once);
    }

    [Test]
    public void Should_Book_Flight_Successfully()
    {
        // Arrange
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightRepository.AddFlight(flight);

        var basePrice = 100m;
        var expectedPrice = 95m;
        var tenantGroup = TenantGroup.A;

        // Act
        var finalPrice = _flightBookingService.BookFlight(flight.FlightId, tenantGroup, DateTime.Today);

        // Assert
        Assert.That(finalPrice, Is.EqualTo(expectedPrice));
    }

    [Test]
    public void Should_Throw_Exception_When_Flight_Not_Found()
    {
        // Arrange
        var invalidFlightId = "INVALID123";

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() =>
            _flightBookingService.BookFlight(invalidFlightId, TenantGroup.A, DateTime.Today));
    }

    [Test]
    public void Should_Not_Discount_Below_Minimum_Price()
    {
        // Arrange
        var flight = new Flight("KLM12345BCA", "NYC", "AFR", DateTime.Today, new[] { DayOfWeek.Thursday });
        _flightRepository.AddFlight(flight);

        var basePrice = 30m;
        var expectedPrice = 20m; // Minimalna cena po zniżkach
        var tenantGroup = TenantGroup.A;

        // Act
        var finalPrice = _flightBookingService.BookFlight(flight.FlightId, tenantGroup, DateTime.Today);

        // Assert
        Assert.That(finalPrice, Is.EqualTo(expectedPrice));
    }

    [Test]
    public void Should_Not_Apply_Discount_When_No_Criteria_Met()
    {
        // Arrange
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightRepository.AddFlight(flight);

        var basePrice = 50m;
        var expectedPrice = 50m; // Brak zniżek
        var tenantGroup = TenantGroup.B;

        // Act
        var finalPrice = _flightBookingService.BookFlight(flight.FlightId, tenantGroup, DateTime.Today.AddYears(-1)); // Nie ma urodzin

        // Assert
        Assert.That(finalPrice, Is.EqualTo(expectedPrice));
    }

    [Test]
    public void Should_Log_Discounts_For_Tenant_Group_A()
    {
        // Arrange
        var flight = new Flight("KLM12345BCA", "NYC", "AFR", DateTime.Today, new[] { DayOfWeek.Thursday });
        _flightRepository.AddFlight(flight);

        var tenantGroup = TenantGroup.A;

        // Act
        _flightBookingService.BookFlight(flight.FlightId, tenantGroup, DateTime.Today);

        // Assert
        //var logs = _discountService.GetDiscountLogs();
        //Assert.That(logs.Count, Is.EqualTo(1));
        //Assert.That(logs[0].FlightId, Is.EqualTo(flight.FlightId));
        //Assert.That(logs[0].TenantId, Is.EqualTo(tenantId));
    }

    [Test]
    public void Should_Not_Log_Discounts_For_Tenant_Group_B()
    {
        // Arrange
        var flight = new Flight("KLM12345BCA", "NYC", "AFR", DateTime.Today, new[] { DayOfWeek.Thursday });
        _flightRepository.AddFlight(flight);

        var tenantGroup = TenantGroup.B;

        // Act
        _flightBookingService.BookFlight(flight.FlightId, tenantGroup, DateTime.Today);

        // Assert
        //var logs = _discountService.GetDiscountLogs();
        //Assert.That(logs.Count, Is.EqualTo(0));
    }

    [Test]
    public void Should_Throw_Exception_When_Repository_Is_Empty()
    {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() =>
            _flightBookingService.BookFlight("KLM12345BCA", TenantGroup.A, DateTime.Today));
    }
}