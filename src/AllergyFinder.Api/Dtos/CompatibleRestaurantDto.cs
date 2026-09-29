namespace AllergyFinder.Api.Dtos;

public record CompatibleRestaurantDto(
    int Id,
    string Name,
    string City,
    string CuisineType,
    int PriceLevel,
    int CompatibleDishes,
    int WarningDishes,
    int PenalizedDishes,
    int ToVerifyDishes,
    int NotCompatibleDishes);