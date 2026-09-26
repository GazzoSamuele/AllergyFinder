namespace AllergyFinder.Api.Models;

public class DishAllergen
{
    public int DishId { get; set; }
    public Dish? Dish { get; set; }

    public int AllergenId { get; set; }
    public Allergen? Allergen { get; set; }

    public AllergenPresence Presence { get; set; }
}