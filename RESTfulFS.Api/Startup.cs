using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RESTfulFS.Api.Filters;
using RESTfulFS.Infrastructure;
using RESTfulFS.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using AutoMapper;

namespace RESTfulFS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add a DbContext to use so that we've got some data to access.
            // We're going to use an 'in memory' database.
            services.AddDbContext<FlightsDBContext>(opt => {
                opt.UseInMemoryDatabase("RESTfulFS");
            });

            services.AddMvc(opt =>
            {
                // Add our custom IExceptionFilter so that when errors occur, we can return them to the client as JSON with appropriate HTTP status.
                opt.Filters.Add(typeof(JsonExceptionFilter));
            })
            .AddJsonOptions(opt =>
            {
                // These should be the defaults, but we can be explicit:
                opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                opt.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
            });

            // Let's enforce our routing pattern matching to lowercase only.
            services.AddRouting(opt => opt.LowercaseUrls = true);

            // We'll use AutoMapper to manage mappings between our entities from the DB and the objects that we'll use in our 'view'.
            services.AddAutoMapper();

            // We'll setup mappings to our concrete types for depedency injection.
            services.AddScoped<IAvailabilityService, DefaultAvailabilityService>();
            services.AddScoped<IFlightService, DefaultFlightService>();
            services.AddScoped<IBookingService, DefaultBookingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
