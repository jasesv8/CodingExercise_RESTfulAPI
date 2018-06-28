using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RESTfulFS.Models
{
    /// <summary>
    ///     FlightEntity represents a Flight that will be persisted.
    ///     It inherits from EntityBase which contains the CreatedDate and ModifiedDate properties that are relevant for persisted entities.
    /// </summary>
    public class FlightEntity : EntityBase
    {
        [Key]
        public int FlightId { get; set; }
        public string FlightName { get; set; }
        public int SeatingCapacity { get; set; }

        #region Methods

        /// <summary>
        ///     Static method to produce an array of FlightEntity (for testing, sampling, etc).
        /// </summary>
        /// <returns>An array of FlightEntity objects with preconfigured data</returns>
        public static FlightEntity[] GenerateTestData()
        {
            List<FlightEntity> flightEntities = new List<FlightEntity>();

            foreach (var flight in Flight.GenerateTestData())
            {
                flightEntities.Add(new FlightEntity
                {
                    FlightId = flight.FlightId,
                    FlightName = flight.FlightName,
                    SeatingCapacity = flight.SeatingCapacity
                });
            }

            return flightEntities.ToArray();
        }

        #endregion
    }
}
