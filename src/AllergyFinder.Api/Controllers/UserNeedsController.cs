using AllergyFinder.Api.Data;
using AllergyFinder.Api.Dtos;
using AllergyFinder.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllergyFinder.Api.Controllers;

[ApiController]
[Route("api/users/{userId:int}/needs")]
public class UserNeedsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<UserNeedDto>>> GetAll(int userId)
    {
        if (!await db.Users.AnyAsync(u => u.Id == userId))
        {
            return NotFound();
        }
        var allNeeds = await db.UserNeeds
            .Where(es => es.UserId == userId)
            .OrderBy(es => es.AllergenId)
            .Select(n => new UserNeedDto(n.AllergenId, n.Allergen!.Code, n.Allergen.Name, n.Type))
            .ToListAsync();

        return Ok(allNeeds);
    }

    [HttpPut("{allergenId:int}")]
    public async Task<IActionResult> Set(int userId, int allergenId, SetUserNeedDto dto)
    {
        if (!await db.Users.AnyAsync(u => u.Id == userId))
        {
            return NotFound();
        }

        if (!await db.Allergens.AnyAsync(a => a.Id == allergenId))
        {
            return NotFound();
        }

        var existingNeed = await db.UserNeeds.FirstOrDefaultAsync(n => n.UserId == userId && n.AllergenId == allergenId);

        if (existingNeed is null)
        {
            db.UserNeeds.Add(new UserNeed { UserId = userId, AllergenId = allergenId, Type = dto.Type });
        }
        else
        {
            existingNeed.Type = dto.Type;
        }

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{allergenId:int}")]
    public async Task<IActionResult> Delete(int userId, int allergenId)
    {
        var need = await db.UserNeeds.FirstOrDefaultAsync(n => n.UserId == userId && n.AllergenId == allergenId);

        if (need is null)
        {
            return NotFound();
        }
        db.UserNeeds.Remove(need);

        await db.SaveChangesAsync();

        return NoContent();
    }
}