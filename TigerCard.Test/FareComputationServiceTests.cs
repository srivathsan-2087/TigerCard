using Moq;

using TigerCard.Business;
using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

using Xunit;

namespace TigerCard.Test
{
    public class FareComputationServiceTests
    {
        private FareComputationService service;
        private Mock<IFareAggregator> fareAggregatorMock;

        public FareComputationServiceTests()
        {
            this.fareAggregatorMock = new Mock<IFareAggregator>();

            this.service = new FareComputationService(this.fareAggregatorMock.Object);
        }

        [Fact]
        public void ComputeFares_WhenCalled_ShouldCallFareAggregator()
        {
            // Actors
            var journeys = new Journey[0];

            this.fareAggregatorMock.Setup(m => m.AggregateTotalFare(journeys)).Returns(55m);

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(55m, totalFare);
        }
    }
}