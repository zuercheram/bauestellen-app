using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baustellen.App.Projects.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Zip = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Telefon = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    CreatedByOid = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedByOid = table.Column<int>(type: "integer", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManagerName = table.Column<string>(type: "text", nullable: true),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Commissioning = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ObjectStreet = table.Column<string>(type: "text", nullable: true),
                    ObjectNumber = table.Column<string>(type: "text", nullable: true),
                    ObjectZip = table.Column<string>(type: "text", nullable: true),
                    ObjectCity = table.Column<string>(type: "text", nullable: true),
                    Lon = table.Column<string>(type: "text", nullable: true),
                    Lat = table.Column<string>(type: "text", nullable: true),
                    CreatedByOid = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedByOid = table.Column<int>(type: "integer", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProject",
                columns: table => new
                {
                    CustomersId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProject", x => new { x.CustomersId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_CustomerProject_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProject_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalLinks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProject_ProjectsId",
                table: "CustomerProject",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLinks_ProjectId",
                table: "ExternalLinks",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProject");

            migrationBuilder.DropTable(
                name: "ExternalLinks");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
