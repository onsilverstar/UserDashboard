using Microsoft.EntityFrameworkCore.Migrations;

namespace UserDashboard.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "comments");
        }
    }
}
