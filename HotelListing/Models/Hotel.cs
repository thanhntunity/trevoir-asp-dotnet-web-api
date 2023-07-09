namespace HotelListing;

public class Hotel
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public Country? Country { get; set; }
    public required int CountryId { get; set;}
    public double? Rating { get; set; }
}
