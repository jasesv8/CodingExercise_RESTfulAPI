using System;

namespace RESTfulFS.Models
{
    /// <summary>
    ///     Represents a booking.
    ///     Inherits from 'Availability', with the only difference being the addition of a BookingId.
    ///     Used in the 'view'.
    /// </summary>
    public class Booking : Availability
    {
        public int BookingId { get; set; }

        #region Methods

        /// <summary>
        ///     Static method to produce an array of Booking (for testing, sampling, etc).
        /// </summary>
        /// <returns>An array of Booking objects with preconfigured data</returns>
        public static Booking[] GenerateTestData() => new Booking[] {
            new Booking
            {
                BookingId = 1,
                FlightId = 1,
                Passengers = 6,
                BookingDate = DateTime.UtcNow.Date.AddDays(7)
            },
            new Booking
            {
                BookingId = 2,
                FlightId = 1,
                Passengers = 4,
                BookingDate = DateTime.UtcNow.Date.AddDays(8)
            },
            new Booking
            {
                BookingId = 3,
                FlightId = 3,
                Passengers = 4,
                BookingDate = DateTime.UtcNow.Date.AddDays(9)
            },
            new Booking
            {
                BookingId = 4,
                FlightId = 3,
                Passengers = 2,
                BookingDate = DateTime.UtcNow.Date.AddDays(10)
            } };

        #endregion

    }
}
