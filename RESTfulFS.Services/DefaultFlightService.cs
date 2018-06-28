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
    ///     Provides methods for retrieving FlightEntity objects and mapping them to Flight objects.
    ///     Makes use of AutoMapper (.ProjectTo) extensions.
    /// </summary>
    public class DefaultFlightService : IFlightService
    {
        private readonly FlightsDBContext _context;

        #region Constructor

        /// <summary>
        ///     Constructor that allows the passing of a FlightsDBContext
        /// </summary>
        /// <param name="context">FlightsDBContext to be used in this service</param>
        public DefaultFlightService(FlightsDBContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Retrieves FlightEntity objects from the persistence context and maps them to Flight objects.
        /// </summary>
        /// <param name="ct">CancellationToken as part of asynchronous call</param>
        /// <returns>List of existing flights (IEnumerable<Flight>)</returns>
        public async Task<IEnumerable<Flight>> GetFlightsAsync(CancellationToken ct)
        {
            return await _context.Flights
                        .AsQueryable<FlightEntity>()
                        .ProjectTo<Flight>()
                        .ToArrayAsync(ct);
        }

        #endregion

    }
}
