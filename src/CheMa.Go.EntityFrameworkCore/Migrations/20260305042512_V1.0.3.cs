using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheMa.Go.Migrations
{
    /// <inheritdoc />
    public partial class V103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HotelId",
                table: "AppPassengers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_AppPassengers_HotelId",
                table: "AppPassengers",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPassengers_AppHotels_HotelId",
                table: "AppPassengers",
                column: "HotelId",
                principalTable: "AppHotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPassengers_AppHotels_HotelId",
                table: "AppPassengers");

            migrationBuilder.DropIndex(
                name: "IX_AppPassengers_HotelId",
                table: "AppPassengers");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "AppPassengers");
        }
    }
}
