using RESTfulFS.Api.Controllers;
using RESTfulFS.Models;
using RESTfulFS.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RESTfulFS.Api.Tests
{
    public class FlightsControllerTests : IDisposable
    {
        private Mock<IAvailabilityService> _mockAvailabilityService;
        private Mock<IFlightService> _mockFlightService;

        #region Constructor and Dispose

        public FlightsControllerTests()
        {
            // Using 'Moq', we'll mock the IAvailabilityService and IFlightService
            _mockAvailabilityService = new Mock<IAvailabilityService>();
            _mockFlightService = new Mock<IFlightService>();
        }

        public void Dispose()
        {
            // clean up any objects that were instantiated in the constructor
        }

        #endregion

        #region Tests - GetFlightsAsync

        [Fact]
        public async Task GetFlightsAsync_WhenNoData()
        {
            // Create test data and setup the mock object before the call
            IEnumerable<Flight> flights = new Flight[] { };
            _mockFlightService.Setup(o => o.GetFlightsAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(flights));
            FlightsController controller = new FlightsController(_mockAvailabilityService.Object, _mockFlightService.Object);

            // Make the call
            var result = await controller.GetFlightsAsync(CancellationToken.None);

            // Validate the outcome
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetFlightsAsync_WhenHasData()
        {
            // Create test data and setup the mock object before the call
            IEnumerable<Flight> flights = Flight.GenerateTestData();
            _mockFlightService.Setup(o => o.GetFlightsAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(flights));
            FlightsController controller = new FlightsController(_mockAvailabilityService.Object, _mockFlightService.Object);

            // Make the call
            var result = await controller.GetFlightsAsync(CancellationToken.None);

            // Validate the outcome
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        #endregion

        #region Tests - GetFlightsAvailabilityAsync

        [Fact]
        public async Task GetFlightsAvailabilityAsync_WhenBadParams()
        {
            // Create test data and setup the mock object before the call
            string queryArgIssues = "Has Issues!";
            _mockAvailabilityService.Setup(o => o.QueryArgsIssues(
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<int?>()
                )).Returns(queryArgIssues);
            FlightsController controller = new FlightsController(_mockAvailabilityService.Object, _mockFlightService.Object);

            // Make the call
            var result = await controller.GetFlightsAvailabilityAsync(null, null, null, CancellationToken.None);

            // Validate the outcome
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetFlightsAvailabilityAsync_WhenHasAvailability()
        {
            // Create test data and setup the mock object before the call
            string queryArgIssues = null;
            _mockAvailabilityService.Setup(o => o.QueryArgsIssues(
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<int?>()
                )).Returns(queryArgIssues);

            IEnumerable<Availability> availabilities = new Availability[] {
                new Availability {
                    FlightId = 1,
                    BookingDate = DateTime.UtcNow.Date,
                    Passengers = 2
                }};
            _mockAvailabilityService.Setup(o => o.GetAvailabilityAsync(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).Returns(Task.FromResult(availabilities));
            FlightsController controller = new FlightsController(_mockAvailabilityService.Object, _mockFlightService.Object);

            // Make the call
            var result = await controller.GetFlightsAvailabilityAsync(DateTime.UtcNow.Date, DateTime.UtcNow.Date, 2, CancellationToken.None);

            // Validate the outcome
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetFlightsAvailabilityAsync_WhenNoAvailability()
        {
            // Create test data and setup the mock object before the call
            string queryArgIssues = null;
            _mockAvailabilityService.Setup(o => o.QueryArgsIssues(
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<int?>()
                )).Returns(queryArgIssues);

            IEnumerable<Availability> availabilities = new Availability[] { };
            _mockAvailabilityService.Setup(o => o.GetAvailabilityAsync(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).Returns(Task.FromResult(availabilities));
            FlightsController controller = new FlightsController(_mockAvailabilityService.Object, _mockFlightService.Object);

            // Make the call
            var result = await controller.GetFlightsAvailabilityAsync(DateTime.UtcNow.Date, DateTime.UtcNow.Date, 2, CancellationToken.None);

            // Validate the outcome
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion
    }
}
