using System.ComponentModel.DataAnnotations;

namespace AllergyFinder.Api.Dtos;

public record CreateRestaurantDto(
    [Required, MaxLength(150)] string Name,
    [Required, MaxLength(100)] string City,
    [Required, MaxLength(50)] string CuisineType,
    [Range(1, 3)] int PriceLevel);