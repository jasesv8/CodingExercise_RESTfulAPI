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
    public class BookingsControllerTests : IDisposable
    {
        private Mock<IBookingService> _mockBookingService;

        #region Constructor and Dispose

        public BookingsControllerTests()
        {
            _mockBookingService = new Mock<IBookingService>();
        }

        public void Dispose()
        {
            // clean up any objects that were instantiated in the constructor
        }

        #endregion

        #region Tests - GetBookingsAsync

        [Fact]
        public async Task GetBookingsAsync_WhenNoData()
        {
            // Create test data and setup the mock object before the call
            IEnumerable<Booking> bookings = new Booking[] { };
            _mockBookingService.Setup(o => o.GetBookingsAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(bookings));
            BookingsController controller = new BookingsController(_mockBookingService.Object);

            // Make the call
            var result = await controller.GetBookingsAsync(CancellationToken.None);

            // Validate the outcome
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetBookingsAsync_WhenHasData()
        {
            // Create test data and setup the mock object before the call
            IEnumerable<Booking> bookings = Booking.GenerateTestData();
            _mockBookingService.Setup(o => o.GetBookingsAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(bookings));
            BookingsController controller = new BookingsController(_mockBookingService.Object);

            // Make the call
            var result = await controller.GetBookingsAsync(CancellationToken.None);

            // Validate the outcome
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        #endregion

    }
}
