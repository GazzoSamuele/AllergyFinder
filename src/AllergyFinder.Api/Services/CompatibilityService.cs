using AllergyFinder.Api.Models;

namespace AllergyFinder.Api.Services;

public class CompatibilityService
{
    public CompatibilityStatus EvaluateNeed(NeedType type, AllergenPresence presence)
    {
        return (type, presence) switch
        {
            (NeedType.Allergy, AllergenPresence.Contains) => CompatibilityStatus.NotCompatible,
            (NeedType.Allergy, AllergenPresence.MayContainTraces) => CompatibilityStatus.ToVerify,

            (NeedType.Intolerance, AllergenPresence.Contains) => CompatibilityStatus.NotCompatible,
            (NeedType.Intolerance, AllergenPresence.MayContainTraces) => CompatibilityStatus.CompatibleWithWarning,

            (NeedType.Preference, AllergenPresence.Contains) => CompatibilityStatus.Penalized,
            (NeedType.Preference, AllergenPresence.MayContainTraces) => CompatibilityStatus.Compatible,


            (_, AllergenPresence.Unknown) => CompatibilityStatus.ToVerify,

            _ => throw new ArgumentOutOfRangeException(nameof(presence))
        };
    }

    public const int MaxDeclarationAgeMonths = 12;

    public DishCompatibility EvaluateDish(Dish dish, List<UserNeed> needs, DateTime now)
    {
        var status = CompatibilityStatus.Compatible;
        var reasons = new List<string>();

        if (dish.AllergensDeclaredAt is null)
        {
            status = CompatibilityStatus.ToVerify;
            reasons.Add("Allergeni mai dichiarati");
        }
        else if (dish.AllergensDeclaredAt < now.AddMonths(-MaxDeclarationAgeMonths))
        {
            status = CompatibilityStatus.ToVerify;
            reasons.Add("Dichiarazione più vecchia di 12 mesi");
        }

        foreach (var need in needs)
        {
            var dishAllergen = dish.Allergens.FirstOrDefault(da => da.AllergenId == need.AllergenId);
            if (dishAllergen is null)
            {
                continue;
            }

            var needStatus = EvaluateNeed(need.Type, dishAllergen.Presence);

            if (needStatus != CompatibilityStatus.Compatible)
            {
                reasons.Add($"{dishAllergen.Allergen?.Name}: {dishAllergen.Presence} ({need.Type})");
            }
            if (needStatus > status)
            {
                status = needStatus;
            }
        }

        return new DishCompatibility(status, reasons);
    }
}