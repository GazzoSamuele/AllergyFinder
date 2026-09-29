using AllergyFinder.Api.Models;

namespace AllergyFinder.Api.Dtos;

public record SetUserNeedDto
{
    public required NeedType Type { get; init; }
}