using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTfulFS.Services;
using RESTfulFS.Models;

namespace RESTfulFS.Api.Controllers
{
    /// <summary>
    ///     The FlightsController is exposed via the /api/flights route.
    ///     Provides access to the list of flights.
    ///     Formats output as JSON.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FlightsController : Controller
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IFlightService _flightService;

        #region Constructor

        /// <summary>
        ///     Allows for injection of IAvailabilityService and IFlightService
        /// </summary>
        /// <param name="availabilityService">Instance of IAvailabilityService implementation</param>
        /// <param name="flightService">Instance of IFlightService implementation</param>
        public FlightsController(
            IAvailabilityService availabilityService,
            IFlightService flightService
            )
        {
            _availabilityService = availabilityService;
            _flightService = flightService;
        }

        #endregion

        #region Methods - GET

        // GET 
        /// <summary>
        ///     Route : /api/flights
        ///     Method : GET
        /// </summary>
        /// <param name="ct">CancellationToken as part of asynchronous call</param>
        /// <returns>List of existing flights (IEnumerable<Flight>)</returns>
        // GET /api/flights
        [HttpGet(Name = nameof(GetFlightsAsync))]
        public async Task<IActionResult> GetFlightsAsync(
            CancellationToken ct)
        {
            var flights = await _flightService.GetFlightsAsync(ct);
            if (flights.Count() == 0) return NotFound();

            return Ok(flights);
        }

        /// <summary>
        ///     Route : /api/flights/availability
        ///     Method : GET
        /// </summary>
        /// <param name="fromDate">(Nullable) Date from which availability is to be checked (taken from querystring)</param>
        /// <param name="toDate">(Nullable) Date to which availability is to be checked (taken from querystring)</param>
        /// <param name="passengers">(Nullable) Number of passengers (taken from querystring)</param>
        /// <param name="ct">CancellationToken as part of asynchronous call</param>
        /// <returns>List of available bookings based on the arguments supplied</returns>
        [HttpGet("availability", Name = nameof(GetFlightsAvailabilityAsync))]
        public async Task<IActionResult> GetFlightsAvailabilityAsync(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int? passengers,
            CancellationToken ct)
        {
            // First validate the query arguments, since it's possible that they're null or invalid
            var queryArgIssues = _availabilityService.QueryArgsIssues(fromDate, toDate, passengers);
            if (queryArgIssues != null) return BadRequest(
                new ApiError("Invalid query parameters.",queryArgIssues)
                );

            // Once we've determined that the arguments are valid, we'll cast them to 'non nullable' types and call our underyling service
            var flightAvailability = await _availabilityService.GetAvailabilityAsync(
                (DateTime)fromDate,
                (DateTime)toDate,
                (int)passengers,
                ct
                );
            if (flightAvailability.Count() == 0) return NotFound();

            return Ok(flightAvailability);
        }

        #endregion

    }
}