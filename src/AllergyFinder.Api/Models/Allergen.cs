namespace AllergyFinder.Api.Models;

public class Allergen
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }

}