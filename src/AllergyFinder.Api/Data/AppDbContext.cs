using AllergyFinder.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AllergyFinder.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Allergen> Allergens => Set<Allergen>();
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<DishAllergen> DishAllergens => Set<DishAllergen>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Allergen>(entity =>
        {
            entity.Property(a => a.Code).HasMaxLength(30);
            entity.Property(a => a.Name).HasMaxLength(100);
            entity.HasIndex(a => a.Code).IsUnique();

            entity.HasData(
                new Allergen
                { Id = 1, Code = "GLUTEN", Name = "Cereali contenenti glutine" },

                new Allergen
                { Id = 2, Code = "CRUSTACEANS", Name = "Crostacei" },

                new Allergen
                { Id = 3, Code = "EGGS", Name = "Uova" },

                new Allergen
                { Id = 4, Code = "FISH", Name = "Pesce" },

                new Allergen
                { Id = 5, Code = "PEANUTS", Name = "Arachidi" },

                new Allergen
                { Id = 6, Code = "SOY", Name = "Soia" },

                new Allergen
                { Id = 7, Code = "MILK", Name = "Latte (incluso il lattosio)" },

                new Allergen
                { Id = 8, Code = "TREE_NUTS", Name = "Frutta a guscio" },

                new Allergen
                { Id = 9, Code = "CELERY", Name = "Sedano" },

                new Allergen
                { Id = 10, Code = "MUSTARD", Name = "Senape" },

                new Allergen
                { Id = 11, Code = "SESAME", Name = "Semi di sesamo" },

                new Allergen
                { Id = 12, Code = "SULPHITES", Name = "Anidride solforosa e solfiti" },

                new Allergen
                { Id = 13, Code = "LUPIN", Name = "Lupini" },

                new Allergen
                { Id = 14, Code = "MOLLUSCS", Name = "Molluschi" }


            );
        });

        modelBuilder.Entity<Restaurant>(entity =>
{
    entity.Property(r => r.Name).HasMaxLength(150);
    entity.Property(r => r.City).HasMaxLength(100);
    entity.Property(r => r.CuisineType).HasMaxLength(50);
    entity.ToTable(t => t.HasCheckConstraint(
        "ck_restaurants_price_level", "price_level BETWEEN 1 AND 3"));

    entity.HasMany(r => r.Dishes)
        .WithOne(d => d.Restaurant)
        .HasForeignKey(d => d.RestaurantId)
        .OnDelete(DeleteBehavior.Cascade);
});

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.Property(d => d.Name).HasMaxLength(150);
            entity.Property(d => d.Price).HasPrecision(8, 2);
        });

        modelBuilder.Entity<DishAllergen>(entity =>
        {
            entity.HasKey(da => new { da.DishId, da.AllergenId });
            entity.Property(da => da.Presence).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(da => da.Dish)
                .WithMany(d => d.Allergens)
                .HasForeignKey(da => da.DishId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(da => da.Allergen)
                .WithMany()
                .HasForeignKey(da => da.AllergenId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}