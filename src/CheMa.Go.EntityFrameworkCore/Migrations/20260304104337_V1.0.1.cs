using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheMa.Go.Migrations
{
    /// <inheritdoc />
    public partial class V101 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlightInfo_ArrivalAirport",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "FlightInfo_ArrivalTime",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "FlightInfo_DepartureAirport",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "FlightInfo_DepartureTime",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "FlightInfo_FlightNumber",
                table: "AppOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FlightInfo_ArrivalAirport",
                table: "AppOrders",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FlightInfo_ArrivalTime",
                table: "AppOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlightInfo_DepartureAirport",
                table: "AppOrders",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FlightInfo_DepartureTime",
                table: "AppOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlightInfo_FlightNumber",
                table: "AppOrders",
                type: "character varying(7)",
                maxLength: 7,
                nullable: true);
        }
    }
}
