using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs.UserDTO;

public class LoginUserDTO
{
    [Required]
    [DataType(DataType.EmailAddress)] 
    public string Email { get; set; }
    [Required]
    [MaxLength(15, ErrorMessage = "Your password could not exceed 15 characters"), MinLength(5, ErrorMessage = "Your password must be at least 5 characters")]
    public string Password { get; set; }
}
