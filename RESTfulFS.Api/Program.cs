using RESTfulFS.Api.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace RESTfulFS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .LoadSampleData()
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
