namespace AllergyFinder.Api.Dtos;

public record RestaurantCompatibilityDto(
    int Id,
    string Name,
    string City,
    string CuisineType,
    int PriceLevel,
    List<DishCompatibilityDto> CompatibleDishes,
    List<DishCompatibilityDto> ToVerifyDishes,
    List<DishCompatibilityDto> NotCompatibleDishes);