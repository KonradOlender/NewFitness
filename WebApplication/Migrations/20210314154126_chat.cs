using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class chat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    ChatId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_messages_chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChatId",
                table: "AspNetUsers",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_ChatId",
                table: "messages",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_chats_ChatId",
                table: "AspNetUsers",
                column: "ChatId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_chats_ChatId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "chats");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChatId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "AspNetUsers");
        }
    }
}
