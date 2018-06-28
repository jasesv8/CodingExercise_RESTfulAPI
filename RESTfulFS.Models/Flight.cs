
namespace RESTfulFS.Models
{
    /// <summary>
    ///     Represents a flight.
    ///     Used in the 'view'.
    /// </summary>
    public class Flight
    {
        public int FlightId { get; set; }
        public string FlightName { get; set; }
        public int SeatingCapacity { get; set; }

        #region Methods

        /// <summary>
        ///     Static method to produce an array of Flight (for testing, sampling, etc).
        /// </summary>
        /// <returns>An array of Flight objects with preconfigured data</returns>
        public static Flight[] GenerateTestData() => new Flight[] {
            new Flight
            {
                FlightId = 1,
                FlightName = "ACME001",
                SeatingCapacity = 6
            },
            new Flight
            {
                FlightId = 2,
                FlightName = "ACME002",
                SeatingCapacity = 6
            },
            new Flight
            {
                FlightId = 3,
                FlightName = "ACME003",
                SeatingCapacity = 4
            },
            new Flight
            {
                FlightId = 4,
                FlightName = "ACME004",
                SeatingCapacity = 4
            } };

        #endregion

    }
}
