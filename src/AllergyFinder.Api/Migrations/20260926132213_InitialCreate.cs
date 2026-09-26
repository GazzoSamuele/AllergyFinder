using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AllergyFinder.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "allergens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_allergens", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "allergens",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { 1, "GLUTEN", "Cereali contenenti glutine" },
                    { 2, "CRUSTACEANS", "Crostacei" },
                    { 3, "EGGS", "Uova" },
                    { 4, "FISH", "Pesce" },
                    { 5, "PEANUTS", "Arachidi" },
                    { 6, "SOY", "Soia" },
                    { 7, "MILK", "Latte (incluso il lattosio)" },
                    { 8, "TREE_NUTS", "Frutta a guscio" },
                    { 9, "CELERY", "Sedano" },
                    { 10, "MUSTARD", "Senape" },
                    { 11, "SESAME", "Semi di sesamo" },
                    { 12, "SULPHITES", "Anidride solforosa e solfiti" },
                    { 13, "LUPIN", "Lupini" },
                    { 14, "MOLLUSCS", "Molluschi" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_allergens_code",
                table: "allergens",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "allergens");
        }
    }
}
