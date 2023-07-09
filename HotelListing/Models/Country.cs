namespace HotelListing;

public class Country
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string ShortName { get; set; }
    public List<Hotel> Hotels { get; set; }
}
