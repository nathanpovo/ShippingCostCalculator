using Microsoft.EntityFrameworkCore.Migrations;

namespace ShippingCostCalculator.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Couriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourierId = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<float>(type: "REAL", nullable: false),
                    Length = table.Column<float>(type: "REAL", nullable: false),
                    Width = table.Column<float>(type: "REAL", nullable: false),
                    Height = table.Column<float>(type: "REAL", nullable: false),
                    ShippingCost = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingData_Couriers_CourierId",
                        column: x => x.CourierId,
                        principalTable: "Couriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Couriers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Cargo4You" });

            migrationBuilder.InsertData(
                table: "Couriers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "ShipFaster" });

            migrationBuilder.InsertData(
                table: "Couriers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "MaltaShip" });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingData_CourierId",
                table: "ShippingData",
                column: "CourierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingData");

            migrationBuilder.DropTable(
                name: "Couriers");
        }
    }
}
