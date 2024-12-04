using FlightBooking.Domain.Enums;
using FlightBooking.Domain.Interfaces;
using FlightBooking.Domain.Services;
using Moq;

namespace FlightBooking.Tests.Services
{
    [TestFixture]
    public class DiscountServiceTests
    {
        private DiscountManager _discountManager;
        private Mock<IDiscountLogger> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<IDiscountLogger>();
            _discountManager = new DiscountManager(_mockLogger.Object);
        }

        [Test]
        public void Should_Apply_Mocked_Discount()
        {
            // Arrange
            var basePrice = 50m;
            var discount = 5m;
            var expectedFinalPrice = basePrice - discount;

            var mockCriteria = new Mock<IDiscountCriteria>();
            mockCriteria.Setup(c => c.IsApplicable(It.IsAny<Flight>(), It.IsAny<DateTime?>()))
                        .Returns(true);
            mockCriteria.Setup(c => c.GetDiscountAmount())
                        .Returns(discount);

            _discountManager.AddDiscountCriteria(mockCriteria.Object);
            var discountService = new DiscountService(_discountManager);

            var flight = CreateFlight();
            flight.TicketService.AddTicket(new TicketPrice(basePrice: basePrice) );


            // Act
            var finalPrice = discountService.CalculateDiscountedPrice(flight, null, TenantGroup.A);

            // Assert
            Assert.That(finalPrice, Is.EqualTo(expectedFinalPrice), "The mocked discount was not applied correctly.");
        }

        [TestCase(30, 30, false, false, Description = "No discounts applied")]
        [TestCase(30, 25, true, false, Description = "Birthday discount only")]
        [TestCase(30, 20, true, true, Description = "Both discounts applied")]
        [TestCase(21, 21, true, true, Description = "Both discounts applied, but final price cannot be below 20")]
        [TestCase(26, 26, true, true, 
            Description = "Both discounts applied, but final price cannot be below 20 - use case where I decided to not include any discounts if after all discounts price would be below 20")]
        public void Should_Apply_Discounts_Based_On_Criteria(decimal basePrice, decimal expectedPrice, bool isBirthday, bool isAfricaOnThursday)
        {
            // Arrange
            if (isBirthday)
                _discountManager.AddDiscountCriteria(new BirthdayDiscount());
            if (isAfricaOnThursday)
                _discountManager.AddDiscountCriteria(new AfricaFlightDiscount());

            var discountService = new DiscountService(_discountManager);

            var thursday = GetNextThursday(DateTime.Today);
            // Flight to Africa (Cairo) on Thursday
            var flight = CreateFlight(to: "CAI", departureTime: thursday);
            flight.TicketService.AddTicket(new TicketPrice(basePrice: basePrice));
            var birthdayDate = thursday; // It turns out that it's also a birthday. This is not a DRY violation; I wanted to show a case where we have two discounts.
            // Thursday + Africa + birthday are together by "coincidance"

            // Act
            var finalPrice = discountService.CalculateDiscountedPrice(flight, birthdayDate, TenantGroup.A);

            // Assert
            Assert.That(finalPrice, Is.EqualTo(expectedPrice), $"The calculated price should be {expectedPrice}.");
        }



        [Test]
        public void Should_Not_Log_Discounts_When_Disabled()
        {
            // Arrange
            var flightId = "LOG12345NOT";
            var flight = CreateFlight(flightId: flightId);

            _discountManager.AddDiscountCriteria(new BirthdayDiscount());
            _discountManager.AddDiscountCriteria(new AfricaFlightDiscount());

            var buyerBirthDate = DateTime.Today;
            // Act

            var (totalDiscount, appliedDiscounts) = _discountManager.ApplyDiscounts(flight, buyerBirthDate, logDiscounts: false);

            //// Assert
            _mockLogger.Verify(logger => logger.Log(It.IsAny<DiscountLog>()),
                Times.Never, "Logger should not log any discount details when logging is disabled.");
        }

        [Test]
        public void EmptyDiscountFactory_ShouldNotGiveDiscount()
        {
            // Arrange
            var flightId = "LOG12345NOT";
            var flight = CreateFlight(flightId: flightId);

            var mockDiscountFactory = new Mock<IDiscountFactory>();
            mockDiscountFactory
                .Setup(factory => factory.CreateDiscounts())
                .Returns([]); // No discounts for this test case

            // Register the mocked factory in the DiscountManager
            _discountManager.RegisterDiscountFactory(mockDiscountFactory.Object);

            var buyerBirthDate = DateTime.Today;

            // Act
            var (totalDiscount, appliedDiscounts) = _discountManager.ApplyDiscounts(flight, buyerBirthDate, logDiscounts: true);

            // Assert
            Assert.That(totalDiscount, Is.EqualTo(0));
        }

        [Test]
        public void Should_Log_Discounts_When_Enabled()
        {
            // Arrange
            var flightId = "LOG12345NOW";
            var flight = CreateFlight(flightId: flightId);

            _discountManager.AddDiscountCriteria(new BirthdayDiscount());
            _discountManager.AddDiscountCriteria(new AfricaFlightDiscount());

            var buyerBirthDate = DateTime.Today;
            // Act
            var (totalDiscount, appliedDiscounts) = _discountManager.ApplyDiscounts(flight, buyerBirthDate, logDiscounts: true);

            // Assert
            _mockLogger.Verify(logger => logger.Log(It.Is<DiscountLog>(log =>
                log.FlightId == flightId &&
                log.TotalDiscount == totalDiscount &&
                log.AppliedDiscounts.SequenceEqual(appliedDiscounts))),
                Times.Once, "Logger should log the discount details once.");
        }

        private Flight CreateFlight(
            string flightId = "KLM12345BCA",
            string from = "NYC",
            string to = "LAX",
            DateTime departureTime = default,
            DayOfWeek[]? daysOfWeek = null)
        {

            if (departureTime == default)
            {
                const int twentyDaysAhead = 20;
                departureTime = DateTime.Today.AddDays(twentyDaysAhead);
            }
            daysOfWeek ??= [DayOfWeek.Monday];


            var flight = new Flight(flightId, from, to, departureTime, daysOfWeek);
            return flight;
        }

        private static DateTime GetNextThursday(DateTime startDate)
        {
            int daysUntilThursday = ((int)DayOfWeek.Thursday - (int)startDate.DayOfWeek + 7) % 7;
            return startDate.AddDays(daysUntilThursday);
        }
    }
}