using FlightBooking.Domain.Enums;
using FlightBooking.Domain.Models;
using FlightBooking.Domain.Services;
using Moq;
using NUnit.Framework;

[TestFixture]
public class FlightManagementServiceTests
{
    private FlightManagementService _flightManagementService;
    private IFlightRepository _flightRepository;
    private FlightBookingService _flightBookingService;
    private DiscountService _flightDiscountService;
    private DiscountManager _discountManager;
    private Mock<IDiscountLogger> _mockDiscountLogger;

    [SetUp]
    public void SetUp()
    {
        _flightRepository = new InMemoryFlightRepository();
        _mockDiscountLogger = new Mock<IDiscountLogger>();
        _discountManager = new DiscountManager(_mockDiscountLogger.Object);
        _flightDiscountService = new DiscountService(_discountManager);
        _flightManagementService = new FlightManagementService(_flightRepository);
        _flightBookingService = new FlightBookingService(_flightRepository, _flightDiscountService);
    }

    [Test]
    public void Should_Add_Flight()
    {
        var flightId = "KLM12345BCA";
        var flight = new Flight(flightId, "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.AddFlight(flight);

        var retrievedFlight = _flightManagementService.GetFlightById(flightId);

        Assert.IsNotNull(retrievedFlight);
        Assert.That(retrievedFlight.FlightId, Is.EqualTo(flightId));
    }

    [Test]
    public void Should_Update_Flight()
    {
        var flightId = "KLM12345BCA";
        var flight = new Flight(flightId, "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.AddFlight(flight);

        var updatedFlight = new Flight(flightId, "NYC", "SFO", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.UpdateFlight(flightId, updatedFlight);

        var retrievedFlight = _flightManagementService.GetFlightById(flightId);

        Assert.IsNotNull(retrievedFlight);
        Assert.That(retrievedFlight.To, Is.EqualTo("SFO"));
    }

    [Test]
    public void Should_Remove_Flight()
    {
        var flightId = "KLM12345BCA";
        var flight = new Flight(flightId, "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.AddFlight(flight);

        _flightManagementService.RemoveFlight(flightId);

        var retrievedFlight = _flightManagementService.GetFlightById(flightId);

        Assert.IsNull(retrievedFlight);
    }

    [Test]
    public void Should_Search_Flights_By_Criteria()
    {
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

        var results = _flightManagementService.SearchFlights("NYC", "LAX", mondayFlightDate, new[] { DayOfWeek.Monday });

        Assert.That(results.Count(), Is.EqualTo(1));
        Assert.That(results.First().FlightId, Is.EqualTo("KLM12345BCA"));
    }

    [Test]
    public void Should_Log_Discounts_For_Tenant_Group_A()
    {
        var flight = new Flight("KLM12345BCA", "NYC", "AFR", DateTime.Today, new[] { DayOfWeek.Thursday });
        flight.AddPrice(new FlightPrice(40m));
        _flightRepository.AddFlight(flight);

        var tenantGroup = TenantGroup.A;

        _flightBookingService.BookFlight(flight.FlightId, tenantGroup, DateTime.Today);

        _mockDiscountLogger.Verify(logger => logger.Log(It.Is<DiscountLog>(log =>
            log.FlightId == flight.FlightId &&
            log.AppliedDiscounts.Any())), // Ensures the log contains at least one discount
            Times.Once, // Ensures it was called exactly once
            "Expected logger to log discounts for Tenant Group A.");
    }

    [Test]
    public void Should_Not_Log_Discounts_For_Tenant_Group_B()
    {
        var flight = new Flight("KLM12345BCA", "NYC", "AFR", DateTime.Today, new[] { DayOfWeek.Thursday });
        flight.AddPrice(new FlightPrice(40m));
        _flightRepository.AddFlight(flight);

        var tenantGroup = TenantGroup.B;

        _flightBookingService.BookFlight(flight.FlightId, tenantGroup, DateTime.Today);

        _mockDiscountLogger.Verify(logger => logger.Log(It.IsAny<DiscountLog>()), Times.Never, "Expected logger not to log discounts for Tenant Group B.");
    }

    [Test]
    public void Should_Throw_Exception_When_Repository_Is_Empty()
    {
        Assert.Throws<KeyNotFoundException>(() =>
            _flightBookingService.BookFlight("KLM12345BCA", TenantGroup.A, DateTime.Today));
    }
}