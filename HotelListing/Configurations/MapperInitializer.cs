using AutoMapper;
using HotelListing.DTOs;
using HotelListing.DTOs.CountryDTO;
using HotelListing.DTOs.HotelDTO;

namespace HotelListing.Configurations;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateMap<Country, GetCountryDTO>();
        CreateMap<Hotel, GetHotelDTO>();
    }
}
