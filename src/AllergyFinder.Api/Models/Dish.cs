namespace AllergyFinder.Api.Models;

public class Dish
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int RestaurantId { get; set; }

    public Restaurant? Restaurant { get; set; }

    public DateTime? AllergensDeclaredAt { get; set; }

    public List<DishAllergen> Allergens { get; set; } = [];
}