using System;
using System.Collections.Generic;
using System.Text;

namespace RESTfulFS.Models
{
    /// <summary>
    ///     Represents a potential booking.
    ///     Only ever used in the 'view' to represent information that will ultimately become a booking.
    /// </summary>
    public class Availability
    {
        public int FlightId { get; set; }
        public DateTime BookingDate { get; set; }
        public int Passengers { get; set; }
    }
}
