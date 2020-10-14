using Microsoft.EntityFrameworkCore.Migrations;

namespace UserDashboard.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_messages_messageId",
                table: "comments");

            migrationBuilder.RenameColumn(
                name: "messageId",
                table: "comments",
                newName: "MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_comments_messageId",
                table: "comments",
                newName: "IX_comments_MessageId");

            migrationBuilder.AlterColumn<int>(
                name: "MessageId",
                table: "comments",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_messages_MessageId",
                table: "comments",
                column: "MessageId",
                principalTable: "messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_messages_MessageId",
                table: "comments");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "comments",
                newName: "messageId");

            migrationBuilder.RenameIndex(
                name: "IX_comments_MessageId",
                table: "comments",
                newName: "IX_comments_messageId");

            migrationBuilder.AlterColumn<int>(
                name: "messageId",
                table: "comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_comments_messages_messageId",
                table: "comments",
                column: "messageId",
                principalTable: "messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
