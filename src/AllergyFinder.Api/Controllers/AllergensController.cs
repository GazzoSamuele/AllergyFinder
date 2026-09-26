using AllergyFinder.Api.Data;
using AllergyFinder.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllergyFinder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AllergensController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Allergen>>> GetAll()
    {
        var allergens = await db.Allergens
            .AsNoTracking()
            .OrderBy(a => a.Id)
            .ToListAsync();
        return Ok(allergens);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Allergen>> GetById(int id)
    {
        var singleAllergen = await db.Allergens
            .FirstOrDefaultAsync(a => a.Id == id);

        if (singleAllergen is null)
        {
            return NotFound();
        }
        return Ok(singleAllergen);
    }
}