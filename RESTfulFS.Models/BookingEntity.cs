using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RESTfulFS.Models
{
    /// <summary>
    ///     BookingEntity represents a Booking that will be persisted.
    ///     It inherits from EntityBase which contains the CreatedDate and ModifiedDate properties that are relevant for persisted entities.
    /// </summary>
    public class BookingEntity : EntityBase
    {
        [Key]
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public DateTime BookingDate { get; set; }
        public int Passengers { get; set; }

        #region Methods

        /// <summary>
        ///     Static method to produce an array of BookingEntity (for testing, sampling, etc).
        /// </summary>
        /// <returns>An array of BookingEntity objects with preconfigured data</returns>
        public static BookingEntity[] GenerateTestData()
        {
            List<BookingEntity> bookingEntities = new List<BookingEntity>();

            foreach (var booking in Booking.GenerateTestData())
            {
                bookingEntities.Add(new BookingEntity
                {
                    BookingId = booking.BookingId,
                    FlightId = booking.FlightId,
                    BookingDate = booking.BookingDate,
                    Passengers = booking.Passengers
                });
            }

            return bookingEntities.ToArray();
        }

        #endregion

    }
}
