using HotelListing.DTOs.UserDTO;

namespace HotelListing.Services.AuthService;

public interface IAuthService
{
    Task<bool> ValidateUser(LoginUserDTO loginUserDTO);
    Task<string> CreateToken();
}
