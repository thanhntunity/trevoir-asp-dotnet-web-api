using System.ComponentModel.DataAnnotations;
using HotelListing.DTOs.CountryDTO;

namespace HotelListing.DTOs.HotelDTO;

public class GetHotelDTO
{
    public int Id { get; set; }
    [Required]
    [StringLength(maximumLength: 150, ErrorMessage = "Hotel name is too long")]
    public string Name { get; set; }
    [Required]
    [StringLength(maximumLength: 250, ErrorMessage = "Address name is too long")]
    public string Address { get; set; }
    [Required]
    [Range(1,5)]
    public double Rating { get; set; }
    [Required]
    public int CountryId { get; set; }
    public GetCountryDTO Country { get; set; }
}