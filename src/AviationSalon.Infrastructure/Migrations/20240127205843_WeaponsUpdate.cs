using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AviationSalon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WeaponsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Aircrafts_AircraftId",
                table: "Weapons");

            migrationBuilder.AlterColumn<int>(
                name: "AircraftId",
                table: "Weapons",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Aircrafts_AircraftId",
                table: "Weapons",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "AircraftId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Aircrafts_AircraftId",
                table: "Weapons");

            migrationBuilder.AlterColumn<int>(
                name: "AircraftId",
                table: "Weapons",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Aircrafts_AircraftId",
                table: "Weapons",
                column: "AircraftId",
                principalTable: "Aircrafts",
                principalColumn: "AircraftId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
