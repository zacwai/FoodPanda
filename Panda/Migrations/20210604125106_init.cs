using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Panda.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "promocodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeCreate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promocodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "promoters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeCreate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    NumRegistered = table.Column<int>(nullable: false),
                    NumVerified = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promoters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "members",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PromoterId = table.Column<int>(nullable: false),
                    PromoCodeId = table.Column<int>(nullable: false),
                    TimeCreate = table.Column<DateTime>(nullable: false),
                    TimeUpdate = table.Column<DateTime>(nullable: false),
                    Contact = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    VerifySecret = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_members_promocodes_PromoCodeId",
                        column: x => x.PromoCodeId,
                        principalTable: "promocodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_members_promoters_PromoterId",
                        column: x => x.PromoterId,
                        principalTable: "promoters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_members_Contact",
                table: "members",
                column: "Contact",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_members_PromoCodeId",
                table: "members",
                column: "PromoCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_members_PromoterId",
                table: "members",
                column: "PromoterId");

            migrationBuilder.CreateIndex(
                name: "IX_promocodes_Code",
                table: "promocodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_promoters_Username",
                table: "promoters",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "members");

            migrationBuilder.DropTable(
                name: "promocodes");

            migrationBuilder.DropTable(
                name: "promoters");
        }
    }
}
