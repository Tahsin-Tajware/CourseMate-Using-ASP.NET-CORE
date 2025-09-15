using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseMate.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedByUserId",
                table: "Comments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Comments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRemovedByModerator",
                table: "Comments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DeletedByUserId",
                table: "Comments",
                column: "DeletedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_DeletedByUserId",
                table: "Comments",
                column: "DeletedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_DeletedByUserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_DeletedByUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsRemovedByModerator",
                table: "Comments");
        }
    }
}
