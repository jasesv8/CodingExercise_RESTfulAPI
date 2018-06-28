using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTfulFS.Services;

namespace RESTfulFS.Api.Controllers
{
    /// <summary>
    ///     The BookingsController is exposed via the /api/bookings route.
    ///     Provides access to the list of bookings.
    ///     Formats output as JSON.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;

        #region Constructor

        /// <summary>
        ///     Allows for injection of IBookingService
        /// </summary>
        /// <param name="bookingService">Instance of IBookingService implementation</param>
        public BookingsController(
            IBookingService bookingService
            )
        {
            _bookingService = bookingService;
        }

        #endregion

        #region Methods - GET

        // GET 
        /// <summary>
        ///     Route : /api/bookings
        ///     Method : GET
        /// </summary>
        /// <param name="ct">CancellationToken as part of asynchronous call</param>
        /// <returns>List of existing bookings (IEnumerable<Booking>)</returns>
        [HttpGet(Name = nameof(GetBookingsAsync))]
        public async Task<IActionResult> GetBookingsAsync(
            CancellationToken ct)
        {
            var bookings = await _bookingService.GetBookingsAsync(ct);
            if (bookings.Count() == 0) return NotFound();

            return Ok(bookings);
        }

        #endregion
    }
}