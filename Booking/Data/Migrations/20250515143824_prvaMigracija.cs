using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Data.Migrations
{
    /// <inheritdoc />
    public partial class prvaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ime = table.Column<int>(type: "int", nullable: false),
                    prezime = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<int>(type: "int", nullable: false),
                    lozinka = table.Column<int>(type: "int", nullable: false),
                    adresa = table.Column<int>(type: "int", nullable: false),
                    brojTelefona = table.Column<int>(type: "int", nullable: false),
                    uloga = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Recenzija",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idSmjestaja = table.Column<int>(type: "int", nullable: false),
                    idGosta = table.Column<int>(type: "int", nullable: false),
                    ocjena = table.Column<float>(type: "real", nullable: false),
                    recenzija = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzija", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacija",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idSmjestaja = table.Column<int>(type: "int", nullable: false),
                    idGosta = table.Column<int>(type: "int", nullable: false),
                    datumRezervacije = table.Column<DateTime>(type: "datetime2", nullable: false),
                    pocetakBoravka = table.Column<DateTime>(type: "datetime2", nullable: false),
                    krajBoravka = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cijena = table.Column<float>(type: "real", nullable: false),
                    rezervacijaOtkazana = table.Column<bool>(type: "bit", nullable: false),
                    datumOtkazivanja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacija", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Smjestaj",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idVlasnika = table.Column<int>(type: "int", nullable: false),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lokacija = table.Column<int>(type: "int", nullable: false),
                    tipSmjestaja = table.Column<int>(type: "int", nullable: false),
                    cijenaZaJednuNoc = table.Column<float>(type: "real", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smjestaj", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "Recenzija");

            migrationBuilder.DropTable(
                name: "Rezervacija");

            migrationBuilder.DropTable(
                name: "Smjestaj");
        }
    }
}
