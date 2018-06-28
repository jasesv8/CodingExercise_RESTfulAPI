using Microsoft.EntityFrameworkCore;
using RESTfulFS.Infrastructure;
using System;
using Xunit;
using RESTfulFS.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace RESTfulFS.Services.Tests
{
    public class DefaultAvailabilityServiceTests : IDisposable
    {
        private readonly FlightsDBContext _context;

        #region Constructor and Dispose

        public DefaultAvailabilityServiceTests()
        {
            // Configure an instance of the FlightsDBContext and 'in memory' database.
            // NOTE : Ensure that the name passed to UseInMemoryDatabase is unique to this test class!
            var optionsBuilder = new DbContextOptionsBuilder<FlightsDBContext>();
            optionsBuilder.UseInMemoryDatabase("DefaultAvailabilityServiceTests");
            _context = new FlightsDBContext(optionsBuilder.Options);

            // Create test data before any calls
            var flights = FlightEntity.GenerateTestData();
            _context.Flights.AddRange(flights);
            var bookings = BookingEntity.GenerateTestData();
            _context.Bookings.AddRange(bookings);
            _context.SaveChanges();

            // Call the helper class to initialise AutoMapper since it handles multithreading.
            AutoMapperHelper.Initialize();
        }

        public void Dispose()
        {
            // clean up any objects that were instantiated in the constructor
            _context.Flights.RemoveRange(_context.Flights);
            _context.Bookings.RemoveRange(_context.Bookings);
            _context.SaveChanges();
            _context.Dispose();
            // Call the helper class to Reset AutoMapper since it handles multithreading.
            AutoMapperHelper.Reset();
        }

        #endregion

        #region Tests - QueryArgsIssues

        [Fact]
        public void QueryArgsIssues_WhenBadParams_fromDate_null()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = service.QueryArgsIssues(null, null, null);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(Constants.QUERYARGSISSUES_FROMDATE, result);
        }

        [Fact]
        public void QueryArgsIssues_WhenBadParams_fromDate_invalid()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = service.QueryArgsIssues(DateTime.UtcNow.Date.AddDays(-1), null, null);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(Constants.QUERYARGSISSUES_FROMDATE, result);
        }

        [Fact]
        public void QueryArgsIssues_WhenBadParams_toDate_null()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = service.QueryArgsIssues(DateTime.UtcNow.Date, null, null);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(Constants.QUERYARGSISSUES_TODATE, result);
        }

        [Fact]
        public void QueryArgsIssues_WhenBadParams_toDate_invalid()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = service.QueryArgsIssues(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(-1), null);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(Constants.QUERYARGSISSUES_TODATE, result);
        }

        [Fact]
        public void QueryArgsIssues_WhenBadParams_DateRange_invalid()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = service.QueryArgsIssues(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(15), null);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(Constants.QUERYARGSISSUES_DATERANGE, result);
        }

        [Fact]
        public void QueryArgsIssues_WhenBadParams_passengers_null()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = service.QueryArgsIssues(DateTime.UtcNow.Date, DateTime.UtcNow.Date, null);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(Constants.QUERYARGSISSUES_PASSENGERS, result);
        }

        [Fact]
        public void QueryArgsIssues_WhenBadParams_passengers_invalid()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = service.QueryArgsIssues(DateTime.UtcNow.Date, DateTime.UtcNow.Date, 0);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(Constants.QUERYARGSISSUES_PASSENGERS, result);
        }

        #endregion

        #region Tests - GetAvailabilityAsync

        [Fact]
        public async Task GetAvailabilityAsync_NoBookings_HasCapacity()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = await service.GetAvailabilityAsync(
                DateTime.UtcNow.Date.AddDays(6),
                DateTime.UtcNow.Date.AddDays(6),
                6,
                CancellationToken.None);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(2, Enumerable.Count<Availability>(result));
            Assert.NotNull(result.First(availability => availability.FlightId == 1));
            Assert.NotNull(result.First(availability => availability.FlightId == 2));
        }

        [Fact]
        public async Task GetAvailabilityAsync_NoBookings_NoCapacity()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = await service.GetAvailabilityAsync(
                DateTime.UtcNow.Date.AddDays(6),
                DateTime.UtcNow.Date.AddDays(6),
                7,
                CancellationToken.None);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(0, Enumerable.Count<Availability>(result));
        }

        [Fact]
        public async Task GetAvailabilityAsync_HasBookings_HasCapacity()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = await service.GetAvailabilityAsync(
                DateTime.UtcNow.Date.AddDays(7),
                DateTime.UtcNow.Date.AddDays(7),
                6,
                CancellationToken.None);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(1, Enumerable.Count<Availability>(result));
            Assert.NotNull(result.First(availability => availability.FlightId == 2));
        }

        [Fact]
        public async Task GetAvailabilityAsync_HasBookings_NoCapacity()
        {
            // Make the call
            DefaultAvailabilityService service = new DefaultAvailabilityService(_context);
            var result = await service.GetAvailabilityAsync(
                DateTime.UtcNow.Date.AddDays(7),
                DateTime.UtcNow.Date.AddDays(7),
                7,
                CancellationToken.None);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(0, Enumerable.Count<Availability>(result));
        }

        #endregion
    }
}
