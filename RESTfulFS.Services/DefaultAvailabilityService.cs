using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using RESTfulFS.Infrastructure;
using RESTfulFS.Models;
using Microsoft.EntityFrameworkCore;

namespace RESTfulFS.Services
{
    /// <summary>
    ///     Provides methods for retrieving Availability objects, which are constructed based on existing Flight and Booking information.
    ///     Makes use of AutoMapper (.ProjectTo) extensions.
    /// </summary>
    public class DefaultAvailabilityService : IAvailabilityService
    {
        private readonly FlightsDBContext _context;

        #region Constructor

        /// <summary>
        ///     Constructor that allows the passing of a FlightsDBContext
        /// </summary>
        /// <param name="context">FlightsDBContext to be used in this service</param>
        public DefaultAvailabilityService(FlightsDBContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Determines availability of bookings on flights, in the form of IEnumerable<Availability>.
        ///     (Intended for later use as part of an operation to create a Booking).
        /// </summary>
        /// <param name="fromDate">Date from which availability is to be checked</param>
        /// <param name="toDate">Date to which availability is to be checked</param>
        /// <param name="passengers">Number of passengers</param>
        /// <param name="ct">CancellationToken as part of asynchronous call</param>
        /// <returns>List of available bookings based on the arguments supplied</returns>
        public async Task<IEnumerable<Availability>> GetAvailabilityAsync(
            DateTime fromDate, 
            DateTime toDate, 
            int passengers, 
            CancellationToken ct)
        {
            // Retrieve the list of flights
            var flights = _context.Flights
                            .AsQueryable<FlightEntity>()
                            .ProjectTo<Flight>();

            // Create an empty List for Availabilty objects ... we'll fill as we go
            var availability = new List<Availability>();

            // For each flight, determine whether we have an availability, based on;
            //  - existing bookings
            //  - number of passengers already booked
            //  - the seating capacity of the flight
            foreach (var flight in flights)
            {
                for (DateTime bookingDate = fromDate; bookingDate <= toDate; bookingDate = bookingDate.AddDays(1))
                {
                    // Sum the number of passengers that are already booked on this date, for this flight
                    var bookedPassengers = await _context.Bookings
                                    .AsQueryable<BookingEntity>()
                                    .ProjectTo<Booking>()
                                    .Where<Booking>(b => b.FlightId == flight.FlightId && b.BookingDate == bookingDate)
                                    .SumAsync(b => b.Passengers);

                    // If there's room for our requested passenger number on this date, for this flight, then list the availability
                    if ((bookedPassengers + passengers) <= flight.SeatingCapacity)
                    {
                        availability.Add(new Availability
                        {
                            FlightId = flight.FlightId,
                            BookingDate = bookingDate,
                            Passengers = passengers
                        });
                    }
                }
            }

            return availability.ToArray();
        }

        /// <summary>
        ///     Determines if the query arguments have issues.
        ///     Guards against null arguments.
        ///     Determines suitability of provided (not null) arguments;
        ///         'fromDate' must be today or beyond
        ///         'toDate' must be equal to 'fromDate' or beyond
        ///         Range between 'fromDate' and 'toDate' cannot exceed 14 days
        ///         'pasengers' must be 1 or more
        /// </summary>
        /// <param name="fromDate">(Nullable) Date from which availability is to be checked (taken from querystring)</param>
        /// <param name="toDate">(Nullable) Date to which availability is to be checked (taken from querystring)</param>
        /// <param name="passengers">(Nullable) Number of passengers (taken from querystring)</param>
        /// <returns>A null string if arguments are valid, otherwise a string that represents the issue with the arguments.</returns>
        public string QueryArgsIssues(DateTime? fromDate, DateTime? toDate, int? passengers)
        {
            // Invalid if fromDate is null or less than today
            if ((fromDate == null) || (((DateTime)fromDate).CompareTo(DateTime.UtcNow.Date) < 0))
                return Constants.QUERYARGSISSUES_FROMDATE;

            // Invalid if toDate is null or less than fromDate
            if ((toDate == null) || (((DateTime)toDate).CompareTo(fromDate) < 0))
                return Constants.QUERYARGSISSUES_TODATE;

            // Invalid if date range exceeds 2 weeks
            if (((DateTime)toDate).Subtract((DateTime)fromDate).TotalDays > 14)
                return Constants.QUERYARGSISSUES_DATERANGE;

            // Invalid if passengers is null or 0
            if ((passengers == null) || (passengers < 1))
                return Constants.QUERYARGSISSUES_PASSENGERS;

            return null;
        }

        #endregion
    }
}
