using FlightBooking.Domain.Interfaces;
using FlightBooking.Domain.Models;
using FlightBooking.Domain.Services;
using Moq;

namespace FlightBooking.Tests.Services
{
    [TestFixture]
    public class DiscountServiceTests
    {
        private DiscountService _discountService;
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
            discountManager.RegisterCriteria(mockCriteria.Object);

            var discountService = new DiscountService(discountManager);

            var basePrice = 30m;
            var flight = new Flight("TEST123", "POL", "AFR", DateTime.Today, new[] { DayOfWeek.Thursday });

            // Act
            var finalPrice = discountService.CalculateDiscountedPrice(basePrice, flight, DateTime.Today, null);

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
                discountManager.RegisterCriteria(new BirthdayDiscount());
            if (isAfricaOnThursday)
                discountManager.RegisterCriteria(new AfricaFlightDiscount());

            var discountService = new DiscountService(discountManager);

            var flightDate = GetNextThursday(DateTime.Today);
            var flight = new Flight("TEST123", "POL", isAfricaOnThursday ? "AFR" : "EU", flightDate, new[] { DayOfWeek.Thursday });

            // Act
            var finalPrice = discountService.CalculateDiscountedPrice(basePrice, flight, flightDate, flightDate);

            // Assert
            Assert.That(finalPrice, Is.EqualTo(expectedPrice), $"The calculated price should be {expectedPrice}.");
        }

        private DateTime GetNextThursday(DateTime startDate)
        {
            int daysUntilThursday = ((int)DayOfWeek.Thursday - (int)startDate.DayOfWeek + 7) % 7;
            return startDate.AddDays(daysUntilThursday);
        }
    }
}