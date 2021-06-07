using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Panda.Migrations
{
    public partial class addsomesmstime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreate",
                table: "smslogs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeCreate",
                table: "smslogs");
        }
    }
}
