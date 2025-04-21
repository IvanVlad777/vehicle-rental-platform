using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Vin = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    PricePerKmInEuro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerDayInEuro = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Vin);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartOdometerKm = table.Column<int>(type: "int", nullable: true),
                    EndOdometerKm = table.Column<int>(type: "int", nullable: true),
                    StartBatterySoc = table.Column<int>(type: "int", nullable: true),
                    EndBatterySoc = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleVin = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentals_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rentals_Vehicles_VehicleVin",
                        column: x => x.VehicleVin,
                        principalTable: "Vehicles",
                        principalColumn: "Vin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Telemetrys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehicleVin = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telemetrys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telemetrys_Vehicles_VehicleVin",
                        column: x => x.VehicleVin,
                        principalTable: "Vehicles",
                        principalColumn: "Vin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CustomerId",
                table: "Rentals",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_VehicleVin",
                table: "Rentals",
                column: "VehicleVin");

            migrationBuilder.CreateIndex(
                name: "IX_Telemetrys_VehicleVin",
                table: "Telemetrys",
                column: "VehicleVin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "Telemetrys");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
