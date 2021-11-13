using TigerCard.Business.Models;

using Xunit;

namespace TigerCard.Test.Models
{
    public class JourneyFareTests
    {
        [Fact]
        public void Constructor_WhenCalled_ShouldInitializeProperties()
        {
            // Actors
            var isPeakFare = true;
            var price = 5m;

            // Activity
            var journeyFare = new JourneyFare(isPeakFare, price);

            // Asserts
            Assert.Equal(FareCapType.None, journeyFare.AppliedCapType);
            Assert.Equal(price, journeyFare.Price);
            Assert.True(isPeakFare);
        }

        [Fact]
        public void ApplyCappedPrice_WhenCalledWithoutPrice_ShouldSetPriceToZero()
        {// Actors
            var isPeakFare = true;
            var expectedPrice = 0m;
            var expectedCapType = FareCapType.Daily;
            var journeyFare = new JourneyFare(isPeakFare, 6m);

            // Activity
            journeyFare.ApplyCappedPrice(capType: expectedCapType);

            // Asserts
            Assert.Equal(expectedCapType, journeyFare.AppliedCapType);
            Assert.Equal(expectedPrice, journeyFare.Price);
            Assert.True(isPeakFare);
        }

        [Fact]
        public void ApplyCappedPrice_WhenCalledWithoutCapType_ShouldSetCapTypeToNone()
        {// Actors
            var isPeakFare = true;
            var expectedPrice = 7m;
            var expectedCapType = FareCapType.None;
            var journeyFare = new JourneyFare(isPeakFare, 6m);
            journeyFare.ApplyCappedPrice(capType: FareCapType.Daily);

            // Activity
            journeyFare.ApplyCappedPrice(expectedPrice);

            // Asserts
            Assert.Equal(expectedCapType, journeyFare.AppliedCapType);
            Assert.Equal(expectedPrice, journeyFare.Price);
            Assert.True(isPeakFare);
        }

        [Fact]
        public void ApplyCappedPrice_WhenCalledWithoutCapType_ShouldChangePriceAndCapType()
        {
            // Actors
            var isPeakFare = true;
            var expectedPrice = 7m;
            var expectedCapType = FareCapType.Daily;
            var journeyFare = new JourneyFare(isPeakFare, 6m);

            // Activity
            journeyFare.ApplyCappedPrice(expectedPrice, expectedCapType);

            // Asserts
            Assert.Equal(expectedCapType, journeyFare.AppliedCapType);
            Assert.Equal(expectedPrice, journeyFare.Price);
            Assert.True(isPeakFare);
        }
    }
}
