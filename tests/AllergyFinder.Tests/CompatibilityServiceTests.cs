using AllergyFinder.Api.Models;
using AllergyFinder.Api.Services;

namespace AllergyFinder.Tests;

public class CompatibilityServiceTests
{
    private readonly CompatibilityService _service = new();

    [Theory]
    [InlineData(NeedType.Allergy, AllergenPresence.Contains, CompatibilityStatus.NotCompatible)]
    [InlineData(NeedType.Allergy, AllergenPresence.MayContainTraces, CompatibilityStatus.ToVerify)]
    [InlineData(NeedType.Allergy, AllergenPresence.Unknown, CompatibilityStatus.ToVerify)]

    [InlineData(NeedType.Intolerance, AllergenPresence.Contains, CompatibilityStatus.NotCompatible)]
    [InlineData(NeedType.Intolerance, AllergenPresence.MayContainTraces, CompatibilityStatus.CompatibleWithWarning)]
    [InlineData(NeedType.Intolerance, AllergenPresence.Unknown, CompatibilityStatus.ToVerify)]

    [InlineData(NeedType.Preference, AllergenPresence.Contains, CompatibilityStatus.Penalized)]
    [InlineData(NeedType.Preference, AllergenPresence.MayContainTraces, CompatibilityStatus.Compatible)]
    [InlineData(NeedType.Preference, AllergenPresence.Unknown, CompatibilityStatus.ToVerify)]
    public void EvaluateNeed_ReturnsExpectedStatus(
        NeedType type, AllergenPresence presence, CompatibilityStatus expected)
    {
        var result = _service.EvaluateNeed(type, presence);

        Assert.Equal(expected, result);
    }

    private static readonly DateTime Now = new(2026, 9, 28, 12, 0, 0, DateTimeKind.Utc);
    private static readonly Allergen Gluten = new() { Id = 1, Code = "GLUTEN", Name = "Cereali contenenti glutine" };
    private static readonly Allergen Soy = new() { Id = 6, Code = "SOY", Name = "Soia" };
    private static readonly Allergen Milk = new() { Id = 7, Code = "MILK", Name = "Latte (incluso il lattosio)" };

    [Fact]
    public void EvaluateDish_StaleDeclarationAndAllergyContains_IsNotCompatible()
    {
        // Arrange
        var dish = new Dish
        {
            Name = "Margherita",
            AllergensDeclaredAt = Now.AddMonths(-18),
            Allergens =
            [
                new DishAllergen { AllergenId = Gluten.Id, Allergen = Gluten, Presence = AllergenPresence.Contains },
            ]
        };
        List<UserNeed> needs =
        [
            new UserNeed { AllergenId = Gluten.Id, Type = NeedType.Allergy },
        ];

        // Act
        var result = _service.EvaluateDish(dish, needs, Now);

        // Assert
        Assert.Equal(CompatibilityStatus.NotCompatible, result.Status);
        Assert.Equal(2, result.Reasons.Count);
    }

    [Fact]
    public void EvaluateDish_NeverDeclared_IsToVerify()
    {
        // Arrange
        var dish = new Dish
        {
            Name = "Polpo e patate",
            AllergensDeclaredAt = null,
        };
        List<UserNeed> needs =
        [
            new UserNeed { AllergenId = Gluten.Id, Type = NeedType.Allergy },
        ];

        // Act
        var result = _service.EvaluateDish(dish, needs, Now);

        // Assert
        Assert.Equal(CompatibilityStatus.ToVerify, result.Status);
        Assert.Single(result.Reasons);
    }

    [Fact]
    public void EvaluateDish_RecentDeclarationWithoutUserAllergens_IsCompatible()
    {
        // Arrange
        var dish = new Dish
        {
            Name = "Tagliata",
            AllergensDeclaredAt = Now.AddMonths(-1),
        };
        List<UserNeed> needs =
        [
            new UserNeed { AllergenId = Gluten.Id, Type = NeedType.Allergy },
        ];

        // Act
        var result = _service.EvaluateDish(dish, needs, Now);

        // Assert
        Assert.Equal(CompatibilityStatus.Compatible, result.Status);
        Assert.Empty(result.Reasons);
    }

    [Fact]
    public void EvaluateDish_StaleDeclarationWithoutUserAllergens_IsToVerify()
    {
        // Arrange
        var dish = new Dish
        {
            Name = "Insalata mista",
            AllergensDeclaredAt = Now.AddMonths(-18),
        };
        List<UserNeed> needs =
        [
            new UserNeed { AllergenId = Gluten.Id, Type = NeedType.Allergy },
        ];

        // Act
        var result = _service.EvaluateDish(dish, needs, Now);

        // Assert
        Assert.Equal(CompatibilityStatus.ToVerify, result.Status);
        Assert.Single(result.Reasons);
    }

    [Fact]
    public void EvaluateDish_IntoleranceTracesAndPreferenceContains_IsCompatibleWithWarning()
    {
        // Arrange
        var dish = new Dish
        {
            Name = "Tiramisù senza glutine",
            AllergensDeclaredAt = Now.AddMonths(-1),
            Allergens =
            [
                new DishAllergen { AllergenId = Soy.Id, Allergen = Soy, Presence = AllergenPresence.MayContainTraces },
                new DishAllergen { AllergenId = Milk.Id, Allergen = Milk, Presence = AllergenPresence.Contains },
            ]
        };
        List<UserNeed> needs =
        [
            new UserNeed { AllergenId = Soy.Id, Type = NeedType.Intolerance },
            new UserNeed { AllergenId = Milk.Id, Type = NeedType.Preference },
        ];

        // Act
        var result = _service.EvaluateDish(dish, needs, Now);

        // Assert
        Assert.Equal(CompatibilityStatus.CompatibleWithWarning, result.Status);
        Assert.Equal(2, result.Reasons.Count);
    }
}
