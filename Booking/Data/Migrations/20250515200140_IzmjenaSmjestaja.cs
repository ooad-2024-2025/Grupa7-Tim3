using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Data.Migrations
{
    /// <inheritdoc />
    public partial class IzmjenaSmjestaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "balkon",
                table: "Smjestaj",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "bazen",
                table: "Smjestaj",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "brojSoba",
                table: "Smjestaj",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "klima",
                table: "Smjestaj",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "kuhinja",
                table: "Smjestaj",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "parking",
                table: "Smjestaj",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "tv",
                table: "Smjestaj",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "wifi",
                table: "Smjestaj",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "balkon",
                table: "Smjestaj");

            migrationBuilder.DropColumn(
                name: "bazen",
                table: "Smjestaj");

            migrationBuilder.DropColumn(
                name: "brojSoba",
                table: "Smjestaj");

            migrationBuilder.DropColumn(
                name: "klima",
                table: "Smjestaj");

            migrationBuilder.DropColumn(
                name: "kuhinja",
                table: "Smjestaj");

            migrationBuilder.DropColumn(
                name: "parking",
                table: "Smjestaj");

            migrationBuilder.DropColumn(
                name: "tv",
                table: "Smjestaj");

            migrationBuilder.DropColumn(
                name: "wifi",
                table: "Smjestaj");
        }
    }
}
