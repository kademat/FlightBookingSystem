using FlightBooking.Domain.Enums;
using FlightBooking.Domain.Interfaces;
using FlightBooking.Domain.Models;
using FlightBooking.Domain.Services;
using Moq;

namespace FlightBooking.Tests.Services
{
    [TestFixture]
    public class DiscountServiceTests
    {
        private DiscountService? _discountService;
        private DiscountManager _discountManager;

        [SetUp]
        public void SetUp()
        {
            _discountManager = new DiscountManager();
        }

        [Test]
        public void Should_Apply_Mocked_Discount()
        {
            // Arrange
            var mockCriteria = new Mock<IDiscountCriteria>();
            mockCriteria.Setup(c => c.IsApplicable(It.IsAny<Flight>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>()))
                        .Returns(true);
            mockCriteria.Setup(c => c.GetDiscountAmount())
                        .Returns(5m);

            var discountManager = new DiscountManager();
            discountManager.AddDiscountCriteria(mockCriteria.Object);

            var discountService = new DiscountService(discountManager);

            var basePrice = 30m;
            var flight = new Flight("KLM12345BCA", "POL", "AFR", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today, new[] { DayOfWeek.Thursday }, 5, 120);

            // Act
            var finalPrice = discountService.CalculateDiscountedPrice(basePrice, flight, DateTime.Today, null, "A", TenantGroup.A);

            // Assert
            Assert.That(finalPrice, Is.EqualTo(25m), "The mocked discount was not applied correctly.");
        }

        [TestCase(30, 20, true, true, Description = "Both discounts applied")]
        [TestCase(30, 25, true, false, Description = "Birthday discount only")]
        [TestCase(30, 30, false, false, Description = "No discounts applied")]
        public void Should_Apply_Discounts_Based_On_Criteria(decimal basePrice, decimal expectedPrice, bool isBirthday, bool isAfricaOnThursday)
        {
            // Arrange
            var discountManager = new DiscountManager();
            if (isBirthday)
                discountManager.AddDiscountCriteria(new BirthdayDiscount());
            if (isAfricaOnThursday)
                discountManager.AddDiscountCriteria(new AfricaFlightDiscount());

            var discountService = new DiscountService(discountManager);

            var flightDate = GetNextThursday(DateTime.Today);
            var flight = new Flight("KLM12345BCA", "POL", "AFR", flightDate, flightDate.AddDays(1), flightDate, new[] { DayOfWeek.Thursday }, 5, 120);

            // Act
            var finalPrice = discountService.CalculateDiscountedPrice(basePrice, flight, flightDate, flightDate, "A", TenantGroup.A);

            // Assert
            Assert.That(finalPrice, Is.EqualTo(expectedPrice), $"The calculated price should be {expectedPrice}.");
        }

        [Test]
        public void Should_Log_Discounts_For_Tenant_Group_A()
        {
            // Arrange
            var discountManager = new DiscountManager();
            discountManager.AddDiscountCriteria(new BirthdayDiscount());
            discountManager.AddDiscountCriteria(new AfricaFlightDiscount());

            var discountService = new DiscountService(discountManager);

            var flight = new Flight("KLM12345BCA", "POL", "AFR", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today, new[] { DayOfWeek.Thursday }, 5, 120);
            var buyerBirthDate = DateTime.Today;

            // Act
            var finalPrice = discountService.CalculateDiscountedPrice(
                basePrice: 30m,
                flight: flight,
                purchaseDate: DateTime.Today,
                buyerBirthDate: buyerBirthDate,
                tenantId: "TENANT_A",
                tenantGroup: TenantGroup.A
            );

            var logs = discountService.GetDiscountLogs();

            // Assert
            Assert.That(logs.Count, Is.EqualTo(1));
            Assert.That(logs[0].AppliedDiscounts, Contains.Item("Discount for birthday."));
        }

        [Test]
        public void Should_Not_Log_Discounts_For_Tenant_Group_B()
        {
            // Arrange
            var discountManager = new DiscountManager();
            discountManager.AddDiscountCriteria(new BirthdayDiscount());
            discountManager.AddDiscountCriteria(new AfricaFlightDiscount());

            var discountService = new DiscountService(discountManager);

            var flight = new Flight("KLM12345BCA", "POL", "AFR", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today, new[] { DayOfWeek.Thursday }, 5, 120);
            var buyerBirthDate = DateTime.Today;

            // Act
            var finalPrice = discountService.CalculateDiscountedPrice(
                basePrice: 30m,
                flight: flight,
                purchaseDate: DateTime.Today,
                buyerBirthDate: buyerBirthDate,
                tenantId: "TENANT_B",
                tenantGroup: TenantGroup.B
            );

            var logs = discountService.GetDiscountLogs();

            // Assert
            Assert.That(logs.Count, Is.EqualTo(0));
        }

        private DateTime GetNextThursday(DateTime startDate)
        {
            int daysUntilThursday = ((int)DayOfWeek.Thursday - (int)startDate.DayOfWeek + 7) % 7;
            return startDate.AddDays(daysUntilThursday);
        }
    }
}