namespace AllergyFinder.Api.Services;

public record DishCompatibility(CompatibilityStatus Status, List<string> Reasons);

