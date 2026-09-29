using AllergyFinder.Api.Data;
using AllergyFinder.Api.Dtos;
using AllergyFinder.Api.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AllergyFinder.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllergyFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(AppDbContext db, CompatibilityService compatibility) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RestaurantDto>>> GetAll()
    {
        var restaurants = await db.Restaurants
            .OrderBy(r => r.Name)
            .Select(r => new RestaurantDto(r.Id, r.Name, r.City, r.CuisineType, r.PriceLevel))
            .ToListAsync();
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RestaurantDto>> GetById(int id)
    {
        var singleRestourant = await db.Restaurants
            .Where(r => r.Id == id)
            .Select(r => new RestaurantDto(r.Id, r.Name, r.City, r.CuisineType, r.PriceLevel))
            .FirstOrDefaultAsync();

        if (singleRestourant is null)
        {
            return NotFound();
        }
        return Ok(singleRestourant);
    }

    [HttpPost]

    public async Task<ActionResult<RestaurantDto>> Create(CreateRestaurantDto dto)
    {
        var restaurant = new Restaurant
        {
            Name = dto.Name,
            City = dto.City,
            CuisineType = dto.CuisineType,
            PriceLevel = dto.PriceLevel
        };

        db.Restaurants.Add(restaurant);
        await db.SaveChangesAsync();

        var result = new RestaurantDto(restaurant.Id, restaurant.Name, restaurant.City, restaurant.CuisineType, restaurant.PriceLevel);

        return CreatedAtAction(nameof(GetById), new { id = restaurant.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateRestaurantDto dto)
    {
        var restaurant = await db.Restaurants.FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant is null)
        {
            return NotFound();
        }

        restaurant.Name = dto.Name;
        restaurant.City = dto.City;
        restaurant.CuisineType = dto.CuisineType;
        restaurant.PriceLevel = dto.PriceLevel;

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var restaurant = await db.Restaurants.FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant is null)
        {
            return NotFound();
        }
        db.Restaurants.Remove(restaurant);

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("compatible")]
    public async Task<ActionResult<List<CompatibleRestaurantDto>>> GetCompatible([BindRequired] int userId)
    {
        var user = await db.Users
            .AsNoTracking()
            .Include(u => u.Needs)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return NotFound();
        }

        var restaurants = await db.Restaurants
            .AsNoTracking()
            .Include(r => r.Dishes)
                .ThenInclude(d => d.Allergens)
                    .ThenInclude(da => da.Allergen)
            .ToListAsync();

        var now = DateTime.UtcNow;
        var response = new List<CompatibleRestaurantDto>();

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
    public async Task<ActionResult<RestaurantCompatibilityDto>> GetCompatibility(int id, [BindRequired] int userId)
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