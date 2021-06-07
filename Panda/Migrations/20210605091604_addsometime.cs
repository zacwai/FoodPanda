using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Panda.Migrations
{
    public partial class addsometime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_members_promocodes_PromoCodeId",
                table: "members");

            migrationBuilder.DropIndex(
                name: "IX_members_PromoCodeId",
                table: "members");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeUpdate",
                table: "promoters",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeRedeem",
                table: "promocodes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeUpdate",
                table: "promoters");

            migrationBuilder.DropColumn(
                name: "TimeRedeem",
                table: "promocodes");

            migrationBuilder.CreateIndex(
                name: "IX_members_PromoCodeId",
                table: "members",
                column: "PromoCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_members_promocodes_PromoCodeId",
                table: "members",
                column: "PromoCodeId",
                principalTable: "promocodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
