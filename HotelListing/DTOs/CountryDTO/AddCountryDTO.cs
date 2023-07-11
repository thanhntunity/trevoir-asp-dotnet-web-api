using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs.CountryDTO;

public class AddCountryDTO
{
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long")]
    public string Name { get; set; }
    [Required]
    [StringLength(maximumLength: 2, ErrorMessage = "Country short name is too long")]
    public string ShortName { get; set; }
}