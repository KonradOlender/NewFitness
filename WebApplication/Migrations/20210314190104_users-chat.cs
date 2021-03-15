using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class userschat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_chats_ChatId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_messages_chats_ChatId",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChatId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "messages",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "chatUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ChatId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatUsers", x => new { x.ChatId, x.UserId });
                    table.ForeignKey(
                        name: "FK_chatUsers_chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chatUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chatUsers_UserId",
                table: "chatUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_chats_ChatId",
                table: "messages",
                column: "ChatId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_chats_ChatId",
                table: "messages");

            migrationBuilder.DropTable(
                name: "chatUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "messages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChatId",
                table: "AspNetUsers",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_chats_ChatId",
                table: "AspNetUsers",
                column: "ChatId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_messages_chats_ChatId",
                table: "messages",
                column: "ChatId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
