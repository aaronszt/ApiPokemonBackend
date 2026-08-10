using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pokemon.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    image = table.Column<string>(type: "text", nullable: true),
                    hp = table.Column<int>(type: "integer", nullable: false),
                    attack = table.Column<int>(type: "integer", nullable: false),
                    defense = table.Column<int>(type: "integer", nullable: false),
                    speed = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    custom = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__pokemon__3213E83F159B77B9", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pokemon_types_relation",
                columns: table => new
                {
                    PokemonTypesId = table.Column<int>(type: "integer", nullable: false),
                    PokemonsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pokemon_types_relation", x => new { x.PokemonTypesId, x.PokemonsId });
                    table.ForeignKey(
                        name: "FK_pokemon_types_relation_Pokemons_PokemonsId",
                        column: x => x.PokemonsId,
                        principalTable: "Pokemons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pokemon_types_relation_Types_PokemonTypesId",
                        column: x => x.PokemonTypesId,
                        principalTable: "Types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pokemon_types_relation_PokemonsId",
                table: "pokemon_types_relation",
                column: "PokemonsId");

            migrationBuilder.CreateIndex(
                name: "IX_Types_name",
                table: "Types",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pokemon_types_relation");

            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
