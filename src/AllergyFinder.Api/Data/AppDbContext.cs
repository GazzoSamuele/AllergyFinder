using AllergyFinder.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AllergyFinder.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Allergen> Allergens => Set<Allergen>();

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
    }
}