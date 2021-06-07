using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Panda.Migrations
{
    public partial class smslog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "smslogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OK = table.Column<bool>(nullable: false),
                    To = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    MsgId = table.Column<string>(nullable: true),
                    Msg = table.Column<string>(nullable: true),
                    ErrMsg = table.Column<string>(nullable: true),
                    RawResp = table.Column<string>(nullable: true),
                    Balance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_smslogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "smslogs");
        }
    }
}
