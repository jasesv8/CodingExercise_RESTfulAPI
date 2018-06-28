using AutoMapper;
using RESTfulFS.Models;

namespace RESTfulFS.Services
{
    /// <summary>
    ///     MappingProfile inherits Profile (from the AutoMapper namespace).
    ///     Provides a mapping profile that;
    ///         Maps from FlightEntity to Flight
    ///         Maps from BookingEntity to Booking
    ///     To be passed to the AddProfile call when configuring AutoMapper.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        ///     Default Constructor (no args)
        /// </summary>
        public MappingProfile()
        {
            CreateMap<FlightEntity, Flight>()
                .ForMember(dest => dest.FlightId, opt => opt.MapFrom(src => src.FlightId))
                .ForMember(dest => dest.FlightName, opt => opt.MapFrom(src => src.FlightName))
                .ForMember(dest => dest.SeatingCapacity, opt => opt.MapFrom(src => src.SeatingCapacity));

            CreateMap<BookingEntity, Booking>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
                .ForMember(dest => dest.FlightId, opt => opt.MapFrom(src => src.FlightId))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate))
                .ForMember(dest => dest.Passengers, opt => opt.MapFrom(src => src.Passengers));
        }
    }
}
