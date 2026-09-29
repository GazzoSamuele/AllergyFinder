using AllergyFinder.Api.Services;

namespace AllergyFinder.Api.Dtos;

public record DishCompatibilityDto(
    int Id,
    string Name,
    decimal Price,
    CompatibilityStatus Status,
    List<string> Reasons);