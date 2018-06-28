using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RESTfulFS.Infrastructure;
using RESTfulFS.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace RESTfulFS.Services
{
    /// <summary>
    ///     Provides methods for retrieving BookingEntity objects and mapping them to Booking objects.
    ///     Makes use of AutoMapper (.ProjectTo) extensions.
    /// </summary>
    public class DefaultBookingService : IBookingService
    {
        private readonly FlightsDBContext _context;

        #region Constructor

        /// <summary>
        ///     Constructor that allows the passing of a FlightsDBContext
        /// </summary>
        /// <param name="context">FlightsDBContext to be used in this service</param>
        public DefaultBookingService(FlightsDBContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Retrieves BookingEntity objects from the persistence context and maps them to Booking objects.
        /// </summary>
        /// <param name="ct">CancellationToken as part of asynchronous call</param>
        /// <returns>List of existing bookings (IEnumerable<Booking>)</returns>
        public async Task<IEnumerable<Booking>> GetBookingsAsync(CancellationToken ct)
        {
            return await _context.Bookings
                        .AsQueryable<BookingEntity>()
                        .ProjectTo<Booking>()
                        .ToArrayAsync(ct);
        }

        #endregion

    }
}
