using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountServer.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountDbId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginProviderUserId = table.Column<string>(nullable: false),
                    LoginProviderType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountDbId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_LoginProviderUserId_LoginProviderType",
                table: "Account",
                columns: new[] { "LoginProviderUserId", "LoginProviderType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
