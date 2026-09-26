namespace AllergyFinder.Api.Models;

public class Restaurant
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string CuisineType { get; set; }
    public int PriceLevel { get; set; }

    public List<Dish> Dishes { get; set; } = [];
}