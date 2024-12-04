using FlightBooking.Domain.Enums;
using FlightBooking.Domain.Models;
using FlightBooking.Domain.Services;

[TestFixture]
public class FlightManagementServiceIntegrationTests
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
    public void Should_Complete_Flight_Booking_Flow()
    {
        // Arrange
        var flight = new Flight("KLM12345BCA", "NYC", "LAX", DateTime.Today, new[] { DayOfWeek.Monday });
        _flightManagementService.AddFlight(flight);

        var basePrice = 100m;
        var tenantGroup = TenantGroup.A;

        // Act
        var finalPrice = _flightBookingService.BookFlight(flight.FlightId, tenantGroup, DateTime.Today);

        // Assert
        Assert.That(finalPrice, Is.LessThan(basePrice)); // Cena powinna uwzględniać zniżki
        //var logs = _discountService.GetDiscountLogs();
        //Assert.That(logs.Count, Is.EqualTo(1)); // Powinien być zalogowany zakup
    }
}