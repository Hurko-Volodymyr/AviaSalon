using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AviationSalon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AircraftsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YearOfManufacture",
                table: "Aircrafts",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Manufacturer",
                table: "Aircrafts",
                newName: "ImageFileName");

            migrationBuilder.AddColumn<int>(
                name: "MaxHeight",
                table: "Aircrafts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Range",
                table: "Aircrafts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxHeight",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "Range",
                table: "Aircrafts");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Aircrafts",
                newName: "YearOfManufacture");

            migrationBuilder.RenameColumn(
                name: "ImageFileName",
                table: "Aircrafts",
                newName: "Manufacturer");
        }
    }
}
