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
    public class DefaultFlightServiceTests : IDisposable
    {
        private readonly FlightsDBContext _context;

        #region Constructor and Dispose

        public DefaultFlightServiceTests()
        {
            // Configure an instance of the FlightsDBContext and 'in memory' database.
            // NOTE : Ensure that the name passed to UseInMemoryDatabase is unique to this test class!
            var optionsBuilder = new DbContextOptionsBuilder<FlightsDBContext>();
            optionsBuilder.UseInMemoryDatabase("DefaultFlightServiceTests");
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

        #region Tests - GetFlightsAsync

        [Fact]
        public async Task GetFlightsAsync_WhenHasData()
        {
            // Create test data before the call
            var flights = FlightEntity.GenerateTestData();
            _context.Flights.AddRange(flights);
            _context.SaveChanges();

            // Make the call
            DefaultFlightService service = new DefaultFlightService(_context);
            var result = await service.GetFlightsAsync(CancellationToken.None);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(flights.Length, Enumerable.Count<Flight>(result));
        }

        [Fact]
        public async Task GetFlightsAsync_WhenNoData()
        {
            // Create test data before the call
            _context.Flights.RemoveRange(_context.Flights);
            _context.SaveChanges();

            // Make the call
            DefaultFlightService service = new DefaultFlightService(_context);
            var result = await service.GetFlightsAsync(CancellationToken.None);

            // Validate the result
            Assert.NotNull(result);
            Assert.Equal(0, Enumerable.Count<Flight>(result));
        }

        #endregion
    }
}
