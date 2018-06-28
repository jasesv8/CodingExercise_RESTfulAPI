using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RESTfulFS.Models;

namespace RESTfulFS.Services
{
    /// <summary>
    ///     The IAvailabilityService interface defines what we want a AvailabilityService object to provide.
    ///     The definition of this interface facilitates dependency injection.
    /// </summary>
    public interface IAvailabilityService
    {
        Task<IEnumerable<Availability>> GetAvailabilityAsync(
            DateTime fromDate,
            DateTime toDate,
            int passengers,
            CancellationToken ct);

        string QueryArgsIssues(DateTime? fromDate, DateTime? toDate, int? passengers);
    }
}
