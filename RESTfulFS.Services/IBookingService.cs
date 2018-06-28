using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RESTfulFS.Models;

namespace RESTfulFS.Services
{
    /// <summary>
    ///     The IBookingService interface defines what we want a BookingService object to provide.
    ///     The definition of this interface facilitates dependency injection.
    /// </summary>
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookingsAsync(
            CancellationToken ct);
    }
}
