using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs;

public class CountryDTO
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long")]
    public string Name { get; set; }
    [Required]
    [StringLength(maximumLength: 2, ErrorMessage = "Country short name is too long")]
    public string ShortName { get; set; }
    public List<HotelDTO> Hotels { get; set; }
}

public class CreateCountryDTO
{
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Country name is too long")]
    public string Name { get; set; }
    [Required]
    [StringLength(maximumLength: 2, ErrorMessage = "Country short name is too long")]
    public string ShortName { get; set; }
}
