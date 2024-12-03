using FlightBooking.Domain.Services;

namespace FlightBooking.Tests.Services
{
    [TestFixture]
    public class DiscountServiceTests
    {
        private DiscountService _discountService;

        [SetUp]
        public void SetUp()
        {
            _discountService = new DiscountService();
        }

        [Test]
        public void Should_Apply_Birthday_Discount()
        {
            // Arrange
            var basePrice = 30m;
            var flightDate = DateTime.Today;
            var buyerBirthDate = DateTime.Today; // Today is buyer's birthday
            var isToAfricaOnThursday = false; // Not applicable for this test

            // Act
            var finalPrice = _discountService.CalculateDiscountedPrice(basePrice, flightDate, buyerBirthDate, isToAfricaOnThursday);

            // Assert
            Assert.That(finalPrice, Is.EqualTo(25m), "The birthday discount was not applied correctly.");
        }

        [Test]
        public void Should_Apply_Both_Discounts_And_Not_Fall_Below_Minimum_Price()
        {
            // Arrange
            var basePrice = 30m;
            var flightDate = DateTime.Today; // Flight date is also buyer's birthday
            var buyerBirthDate = DateTime.Today;
            var isToAfricaOnThursday = true; // Applies second discount

            // Act
            var finalPrice = _discountService.CalculateDiscountedPrice(basePrice, flightDate, buyerBirthDate, isToAfricaOnThursday);

            // Assert
            Assert.That(finalPrice, Is.EqualTo(20m), "The discounts were not applied correctly or the price fell below the minimum.");
        }

        [Test]
        public void Should_Not_Apply_Discount_If_Under_Minimum()
        {
            // Arrange
            var basePrice = 21m;
            var flightDate = DateTime.Today;
            var buyerBirthDate = DateTime.Today; // Birthday discount applies
            var isToAfricaOnThursday = true; // Africa discount applies

            // Act
            var finalPrice = _discountService.CalculateDiscountedPrice(basePrice, flightDate, buyerBirthDate, isToAfricaOnThursday);

            // Assert
            Assert.That(finalPrice, Is.EqualTo(20m), "Price should not fall below the minimum.");
        }

        [Test]
        public void Should_Not_Apply_Discount_If_No_Criteria_Met()
        {
            // Arrange
            var basePrice = 30m;
            var flightDate = DateTime.Today;
            var buyerBirthDate = DateTime.Today.AddYears(-1); // Not buyer's birthday
            var isToAfricaOnThursday = false; // Not to Africa on Thursday

            // Act
            var finalPrice = _discountService.CalculateDiscountedPrice(basePrice, flightDate, buyerBirthDate, isToAfricaOnThursday);

            // Assert
            Assert.That(finalPrice, Is.EqualTo(30m), "No discount should have been applied.");
        }
    }
}