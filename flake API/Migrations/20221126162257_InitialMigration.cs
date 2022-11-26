using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flakeAPI.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Location",
            columns: table => new
            {
                State = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Location", x => x.State);
            });

        migrationBuilder.CreateTable(
            name: "Weather",
            columns: table => new
            {
                Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                Temperature = table.Column<double>(type: "float", nullable: false),
                Humidity = table.Column<double>(type: "float", nullable: false),
                Pressure = table.Column<double>(type: "float", nullable: false),
                Latitude = table.Column<double>(type: "float", nullable: false),
                Longitude = table.Column<double>(type: "float", nullable: false),
                LocationState = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Weather", x => x.Time);
                table.ForeignKey(
                    name: "FK_Weather_Location_LocationState",
                    column: x => x.LocationState,
                    principalTable: "Location",
                    principalColumn: "State",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Location",
            column: "State",
            values: new object[]
            {
                "barisal",
                "chittagong",
                "dhaka",
                "dinajpur",
                "mymensingh",
                "rajshahi",
                "rangpur",
                "sylhet"
            });

        migrationBuilder.CreateIndex(
            name: "IX_Weather_LocationState",
            table: "Weather",
            column: "LocationState");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Weather");

        migrationBuilder.DropTable(
            name: "Location");
    }
}
