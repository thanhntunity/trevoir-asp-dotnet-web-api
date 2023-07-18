using AutoMapper;
using HotelListing.DTOs;
using HotelListing.DTOs.CountryDTO;
using HotelListing.DTOs.HotelDTO;
using HotelListing.DTOs.UserDTO;

namespace HotelListing.Configurations;

public class MapperInitializer : Profile
{
    public MapperInitializer()
    {
        CreateMap<Country, GetCountryDTO>();
        CreateMap<Hotel, GetHotelDTO>();
        CreateMap<RegisterUserDTO, User>();
        CreateMap<LoginUserDTO, User>();
    }
}
