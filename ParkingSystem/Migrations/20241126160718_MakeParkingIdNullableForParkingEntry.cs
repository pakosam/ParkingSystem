using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingSystem.Migrations
{
    /// <inheritdoc />
    public partial class MakeParkingIdNullableForParkingEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingEntries_Parkings_ParkingId",
                table: "ParkingEntries");

            migrationBuilder.DropIndex(
                name: "IX_ParkingEntries_ParkingId",
                table: "ParkingEntries");

            migrationBuilder.AlterColumn<int>(
                name: "ParkingId",
                table: "ParkingEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParkingId",
                table: "ParkingEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingEntries_ParkingId",
                table: "ParkingEntries",
                column: "ParkingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingEntries_Parkings_ParkingId",
                table: "ParkingEntries",
                column: "ParkingId",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
