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
    public class DefaultBookingServiceTests : IDisposable
    {
        private readonly FlightsDBContext _context;

        #region Constructor and Dispose
        
        public DefaultBookingServiceTests()
        {
            // Configure an instance of the FlightsDBContext and 'in memory' database.
            // NOTE : Ensure that the name passed to UseInMemoryDatabase is unique to this test class!
            var optionsBuilder = new DbContextOptionsBuilder<FlightsDBContext>();
            optionsBuilder.UseInMemoryDatabase("DefaultBookingServiceTests");
            _context = new FlightsDBContext(optionsBuilder.Options);

            // Call the helper class to initialise AutoMapper since it handles multithreading.
            AutoMapperHelper.Initialize();
        }

        public void Dispose()
        {
            // clean up any objects that were instantiated in the constructor
            _context.Dispose();
            // Call the helper class to Reset AutoMapper since it handles multithreading.
            AutoMapperHelper.Reset();
        }

        #endregion

        #region Tests - GetBookingsAsync

        [Fact]
        public async Task GetBookingsAsync_WhenHasData()
        {
            // Create test data before the call
            var bookings = BookingEntity.GenerateTestData();
            _context.Bookings.AddRange(bookings);
            _context.SaveChanges();

            // Make the call
            DefaultBookingService service = new DefaultBookingService(_context);
            var result = await service.GetBookingsAsync(CancellationToken.None);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(bookings.Length, Enumerable.Count<Booking>(result));
        }

        [Fact]
        public async Task GetBookingsAsync_WhenNoData()
        {
            // Create test data before the call
            _context.Bookings.RemoveRange(_context.Bookings);
            _context.SaveChanges();

            // Make the call
            DefaultBookingService service = new DefaultBookingService(_context);
            var result = await service.GetBookingsAsync(CancellationToken.None);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(0, Enumerable.Count<Booking>(result));
        }

        #endregion
    }
}
