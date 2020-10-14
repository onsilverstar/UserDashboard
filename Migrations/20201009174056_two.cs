using Microsoft.EntityFrameworkCore.Migrations;

namespace UserDashboard.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "content",
                table: "messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "content",
                table: "comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "content",
                table: "comments");
        }
    }
}
