namespace AllergyFinder.Api.Models;

public class UserNeed
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public int AllergenId { get; set; }
    public Allergen? Allergen { get; set; }

    public NeedType Type { get; set; }

}