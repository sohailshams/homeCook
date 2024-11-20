using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCook.Api.Migrations
{
    /// <inheritdoc />
    public partial class Updatedmodelsandrelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeImages_Recipes_FoodId",
                table: "RecipeImages");

            migrationBuilder.DropColumn(
                name: "Buyer_Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "BuyerId",
                table: "Recipes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityAvailable",
                table: "Recipes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SellerId",
                table: "Recipes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "FoodId",
                table: "RecipeImages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_BuyerId",
                table: "Recipes",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_SellerId",
                table: "Recipes",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeImages_Recipes_FoodId",
                table: "RecipeImages",
                column: "FoodId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Users_BuyerId",
                table: "Recipes",
                column: "BuyerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Users_SellerId",
                table: "Recipes",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeImages_Recipes_FoodId",
                table: "RecipeImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Users_BuyerId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Users_SellerId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_BuyerId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_SellerId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "QuantityAvailable",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "Buyer_Role",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FoodId",
                table: "RecipeImages",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeImages_Recipes_FoodId",
                table: "RecipeImages",
                column: "FoodId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
