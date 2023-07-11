using System.ComponentModel.DataAnnotations;
using HotelListing.DTOs.HotelDTO;

namespace HotelListing.DTOs.CountryDTO;

public class GetCountryDTO
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long")]
    public string Name { get; set; }
    [Required]
    [StringLength(maximumLength: 2, ErrorMessage = "Country short name is too long")]
    public string ShortName { get; set; }
    public List<GetHotelDTO> Hotels { get; set; }
}