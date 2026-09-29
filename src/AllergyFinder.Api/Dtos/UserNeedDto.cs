using AllergyFinder.Api.Models;

namespace AllergyFinder.Api.Dtos;

public record UserNeedDto(int AllergenId, string AllergenCode, string AllergenName, NeedType Type);