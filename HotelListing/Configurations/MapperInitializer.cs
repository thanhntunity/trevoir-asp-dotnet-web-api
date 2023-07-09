using AutoMapper;
using HotelListing.DTOs;

namespace HotelListing.Configurations;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        // ReverseMap() means two-way mapping
        CreateMap<Country, CountryDTO>().ReverseMap();
        CreateMap<Country, CreateCountryDTO>().ReverseMap();
        CreateMap<Hotel, HotelDTO>().ReverseMap();
        CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
    }
}
