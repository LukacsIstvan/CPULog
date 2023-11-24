using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CPULogServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ip = table.Column<string>(nullable: true),
                    SensorTimer = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CPUData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientModelId = table.Column<int>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Temperature = table.Column<float>(nullable: true),
                    Load = table.Column<float>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPUData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CPUData_Clients_ClientModelId",
                        column: x => x.ClientModelId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CPUData_ClientModelId",
                table: "CPUData",
                column: "ClientModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CPUData");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
