using AllergyFinder.Api.Data;
using AllergyFinder.Api.Dtos;
using AllergyFinder.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllergyFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(AppDbContext db, CompatibilityService compatibility) : ControllerBase
{
    [HttpGet("compatible")]
    public async Task<ActionResult<List<CompatibleRestaurantDto>>> GetCompatible(int userId)
    {
        // 1. L'utente con le sue esigenze
        var user = await db.Users
            .AsNoTracking()
            .Include(u => u.Needs)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return NotFound();
        }

        // 2. I ristoranti con piatti, allergeni dei piatti e nome di ogni allergene
        var restaurants = await db.Restaurants
            .AsNoTracking()
            .Include(r => r.Dishes)
                .ThenInclude(d => d.Allergens)
                    .ThenInclude(da => da.Allergen)
            .ToListAsync();

        var now = DateTime.UtcNow;
        var response = new List<CompatibleRestaurantDto>();

        // 3. Per ogni ristorante: valuta i piatti e conta gli stati
        foreach (var restaurant in restaurants)
        {
            var results = restaurant.Dishes
                .Select(d => compatibility.EvaluateDish(d, user.Needs, now))
                .ToList();

            response.Add(new CompatibleRestaurantDto(
                restaurant.Id,
                restaurant.Name,
                restaurant.City,
                restaurant.CuisineType,
                restaurant.PriceLevel,
                CompatibleDishes: results.Count(r => r.Status == CompatibilityStatus.Compatible),
                WarningDishes: results.Count(r => r.Status == CompatibilityStatus.CompatibleWithWarning),
                PenalizedDishes: results.Count(r => r.Status == CompatibilityStatus.Penalized),
                ToVerifyDishes: results.Count(r => r.Status == CompatibilityStatus.ToVerify),
                NotCompatibleDishes: results.Count(r => r.Status == CompatibilityStatus.NotCompatible)
            ));
        }

        var ranked = response
            .OrderByDescending(r => r.CompatibleDishes + r.WarningDishes)
            .ThenByDescending(r => r.PenalizedDishes)
            .ThenByDescending(r => r.ToVerifyDishes)
            .ThenBy(r => r.Name)
            .ToList();

        return Ok(ranked);
    }

    [HttpGet("{id:int}/compatibility")]
    public async Task<ActionResult<RestaurantCompatibilityDto>> GetCompatibility(int id, int userId)
    {
        var user = await db.Users
            .AsNoTracking()
            .Include(u => u.Needs)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return NotFound();
        }

        var restaurant = await db.Restaurants
            .AsNoTracking()
            .Include(r => r.Dishes)
                .ThenInclude(d => d.Allergens)
                    .ThenInclude(da => da.Allergen)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant is null)
        {
            return NotFound();
        }

        var now = DateTime.UtcNow;
        var dishes = new List<DishCompatibilityDto>();

        foreach (var dish in restaurant.Dishes)
        {
            var result = compatibility.EvaluateDish(dish, user.Needs, now);

            dishes.Add(new DishCompatibilityDto(
                dish.Id,
                dish.Name,
                dish.Price,
                result.Status,
                result.Reasons
            ));
        }

        return Ok(new RestaurantCompatibilityDto(
            restaurant.Id,
            restaurant.Name,
            restaurant.City,
            restaurant.CuisineType,
            restaurant.PriceLevel,
            CompatibleDishes: dishes.Where(d => d.Status <= CompatibilityStatus.CompatibleWithWarning).ToList(),
            ToVerifyDishes: dishes.Where(d => d.Status == CompatibilityStatus.ToVerify).ToList(),
            NotCompatibleDishes: dishes.Where(d => d.Status == CompatibilityStatus.NotCompatible).ToList()
        ));
    }
}