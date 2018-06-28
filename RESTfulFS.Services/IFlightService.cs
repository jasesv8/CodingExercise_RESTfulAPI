using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RESTfulFS.Models;

namespace RESTfulFS.Services
{
    /// <summary>
    ///     The IFlightService interface defines what we want a FlightService object to provide.
    ///     The definition of this interface facilitates dependency injection.
    /// </summary>
    public interface IFlightService
    {
        Task<IEnumerable<Flight>> GetFlightsAsync(
            CancellationToken ct);
    }
}
