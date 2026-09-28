using AllergyFinder.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AllergyFinder.Api.Data;

public static class DevDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await SeedRestaurantsAsync(db);
        await SeedUsersAsync(db);
    }
    private static async Task SeedRestaurantsAsync(AppDbContext db)
    {
        if (await db.Restaurants.AnyAsync())
        {
            return;
        }

        var allergens = await db.Allergens.ToDictionaryAsync(a => a.Code);
        var now = DateTime.UtcNow;

        var trattoria = new Restaurant
        {
            Name = "Trattoria del Borgo",
            City = "Torino",
            CuisineType = "Piemontese",
            PriceLevel = 2,
            Dishes =
            [
                new Dish
                {
                    Name = "Agnolotti del plin al sugo d'arrosto",
                    Price = 14.00m,
                    AllergensDeclaredAt = now.AddMonths(-1),
                    Allergens =
                    [
                        new DishAllergen { Allergen = allergens["GLUTEN"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["EGGS"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["MILK"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["CELERY"], Presence = AllergenPresence.MayContainTraces }
                    ]
                },
                new Dish
                {
                    Name = "Vitello tonnato",
                    Price = 12.00m,
                    AllergensDeclaredAt = now.AddMonths(-1),
                    Allergens =
                    [
                        new DishAllergen { Allergen = allergens["FISH"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["EGGS"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["MUSTARD"], Presence = AllergenPresence.Contains },
                    ]
                },
                new Dish
                {
                    Name = "Bonet",
                    Price = 6.00m,
                    AllergensDeclaredAt = now.AddMonths(-1),
                    Allergens =
                    [
                        new DishAllergen { Allergen = allergens["EGGS"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["MILK"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["TREE_NUTS"], Presence = AllergenPresence.Contains }
                    ]
                }

            ]
        };

        var senzaGlutineBistrot = new Restaurant
        {
            Name = "Senza Glutine Bistrot",
            City = "Pinerolo",
            CuisineType = "Cucina senza glutine",
            PriceLevel = 2,
            Dishes = [

                new Dish
                {
                    Name = "Risotto ai funghi",
                    Price = 13.00m,
                    AllergensDeclaredAt = now.AddMonths(-2),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["MILK"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["CELERY"], Presence = AllergenPresence.Contains },
                    ]

                },

                new Dish
                {
                    Name = "Tagliata di manzo con rucola",
                    Price = 18.00m,
                    AllergensDeclaredAt = now.AddMonths(-2),
                    Allergens = []

                },

                new Dish
                {
                    Name = "Tiramisù senza glutine",
                    Price = 6.50m,
                    AllergensDeclaredAt = now.AddMonths(-2),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["EGGS"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["MILK"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["SOY"], Presence = AllergenPresence.MayContainTraces },
                    ]

                }
            ]
        };

        var sushiPo = new Restaurant
        {
            Name = "Sushi Po",
            City = "Moncalieri",
            CuisineType = "Giapponese",
            PriceLevel = 3,
            Dishes = [

                new Dish
                {
                    Name = "Nigiri misti",
                    Price = 16.00m,
                    AllergensDeclaredAt = now.AddMonths(-3),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["FISH"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["SOY"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["SESAME"], Presence = AllergenPresence.MayContainTraces },
                    ]

                },

                new Dish
                {
                    Name = "Tempura di gamberi",
                    Price = 14.00m,
                    AllergensDeclaredAt = now.AddMonths(-3),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["CRUSTACEANS"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["GLUTEN"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["EGGS"], Presence = AllergenPresence.Contains },
                    ]

                },

                new Dish
                {
                    Name = "Edamame",
                    Price = 5.00m,
                    AllergensDeclaredAt = now.AddMonths(-3),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["SOY"], Presence = AllergenPresence.Contains },
                    ]

                }
            ]
        };
        var pizzeriaMole = new Restaurant
        {
            Name = "Pizzeria Mole",
            City = "Torino",
            CuisineType = "Pizzeria",
            PriceLevel = 1,
            Dishes = [

                new Dish
                {
                    Name = "Margherita",
                    Price = 7.00m,
                    AllergensDeclaredAt = now.AddMonths(-18),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["GLUTEN"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["MILK"], Presence = AllergenPresence.Contains },
                    ]

                },

                new Dish
                {
                    Name = "Marinara",
                    Price = 5.50m,
                    AllergensDeclaredAt = now.AddMonths(-18),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["GLUTEN"], Presence = AllergenPresence.Contains },
                    ]

                },

                new Dish
                {
                    Name = "Calzone",
                    Price = 9.00m,
                    AllergensDeclaredAt = now.AddMonths(-18),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["GLUTEN"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["MILK"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["EGGS"], Presence = AllergenPresence.MayContainTraces },
                    ]

                }
            ]
        };

        var osteriaSanSalvario = new Restaurant
        {
            Name = "Osteria San Salvario",
            City = "Torino",
            CuisineType = "Mediterranea",
            PriceLevel = 2,
            Dishes = [

                new Dish
                {
                    Name = "Polpo e patate",
                    Price = 15.00m,
                    AllergensDeclaredAt = null,
                    Allergens = []

                },

                new Dish
                {
                    Name = "Spaghetti alle vongole",
                    Price = 14.00m,
                    AllergensDeclaredAt = null,
                    Allergens = []

                },

            ]
        };
        var greenKitchen = new Restaurant
        {
            Name = "Green Kitchen",
            City = "Piossasco",
            CuisineType = "Cucina Vegana",
            PriceLevel = 1,
            Dishes = [

                new Dish
                {
                    Name = "Buddha bowl",
                    Price = 11.00m,
                    AllergensDeclaredAt = now.AddMonths(-1),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["SESAME"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["SOY"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["CELERY"], Presence = AllergenPresence.Unknown },
                    ]

                },

                new Dish
                {
                    Name = "Hummus con pane pita",
                    Price = 8.00m,
                    AllergensDeclaredAt = now.AddMonths(-1),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["SESAME"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["GLUTEN"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["LUPIN"], Presence = AllergenPresence.MayContainTraces },
                    ]

                },

                new Dish
                {
                    Name = "Burger di ceci",
                    Price = 12.00m,
                    AllergensDeclaredAt = now.AddMonths(-1),
                    Allergens = [
                        new DishAllergen { Allergen = allergens["GLUTEN"], Presence = AllergenPresence.Contains },
                        new DishAllergen { Allergen = allergens["MUSTARD"], Presence = AllergenPresence.Unknown },
                    ]

                }
            ]
        };

        db.Restaurants.AddRange(trattoria, senzaGlutineBistrot, sushiPo, pizzeriaMole, osteriaSanSalvario, greenKitchen);
        await db.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync())
        {
            return;
        }

        var allergens = await db.Allergens.ToDictionaryAsync(a => a.Code);

        var samuele = new User
        {
            Name = "Samuele",
            Needs =
            [
                new UserNeed { Allergen = allergens["GLUTEN"], Type = NeedType.Allergy },
                new UserNeed { Allergen = allergens["MILK"], Type = NeedType.Intolerance },
            ]
        };

        var fabio = new User
        {
            Name = "Fabio",
            Needs =
            [
                new UserNeed { Allergen = allergens["CRUSTACEANS"], Type = NeedType.Allergy },
                new UserNeed { Allergen = allergens["MILK"], Type = NeedType.Preference },
            ]
        };

        var anna = new User
        {
            Name = "Anna",
            Needs =
            [
                new UserNeed { Allergen = allergens["EGGS"], Type = NeedType.Allergy },
                new UserNeed { Allergen = allergens["SOY"], Type = NeedType.Intolerance },
            ]
        };

        var elena = new User
        {
            Name = "Elena",
            Needs =
            [
                new UserNeed { Allergen = allergens["PEANUTS"], Type = NeedType.Allergy },
                new UserNeed { Allergen = allergens["TREE_NUTS"], Type = NeedType.Intolerance },
                new UserNeed { Allergen = allergens["CELERY"], Type = NeedType.Allergy },
            ]
        };

        db.Users.AddRange(samuele, fabio, anna, elena);
        await db.SaveChangesAsync();
    }
}