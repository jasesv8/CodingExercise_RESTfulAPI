using RESTfulFS.Infrastructure;
using RESTfulFS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace RESTfulFS.Api.Extensions
{
    /// <summary>
    ///     This extension method allows us to prime our database with sample data.
    ///     It will be invoked when the process starts from the BuildWebHost call.
    /// </summary>
    public static class IWebHostExtension
    {
        public static IWebHost LoadSampleData(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<FlightsDBContext>();

                context.Flights.AddRange(FlightEntity.GenerateTestData());
                context.Bookings.AddRange(BookingEntity.GenerateTestData());

                context.SaveChanges();
            }

            return webHost;
        }
    }
}
