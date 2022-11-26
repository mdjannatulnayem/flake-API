using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace flakeAPI.Migrations;

/// <inheritdoc />
public partial class AddDataTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Location",
            columns: table => new
            {
                LocationId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Location", x => x.LocationId);
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
                LocationId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Weather", x => x.Time);
                table.ForeignKey(
                    name: "FK_Weather_Location_LocationId",
                    column: x => x.LocationId,
                    principalTable: "Location",
                    principalColumn: "LocationId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Weather_LocationId",
            table: "Weather",
            column: "LocationId");
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

