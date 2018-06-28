using RESTfulFS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace RESTfulFS.Infrastructure
{
    /// <summary>
    ///     Application specific DbContext (Entity Framework).
    ///     Specifies two DbSets;
    ///         FlightEntity - for persisted flight information
    ///         BookingEntity - for persisted booking information
    /// </summary>
    public class FlightsDBContext : DbContext
    {
        public FlightsDBContext(DbContextOptions<FlightsDBContext> options) : base(options) { }
        public DbSet<FlightEntity> Flights { get; set; }
        public DbSet<BookingEntity> Bookings { get; set; }
    }
}
