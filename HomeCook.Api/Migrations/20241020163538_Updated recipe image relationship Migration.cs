using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCook.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedrecipeimagerelationshipMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeImages_RecipeImageId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_RecipeImageId",
                table: "Recipes");

            migrationBuilder.AddColumn<Guid>(
                name: "RecipeId",
                table: "RecipeImages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeImages_RecipeId",
                table: "RecipeImages",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeImages_Recipes_RecipeId",
                table: "RecipeImages",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeImages_Recipes_RecipeId",
                table: "RecipeImages");

            migrationBuilder.DropIndex(
                name: "IX_RecipeImages_RecipeId",
                table: "RecipeImages");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "RecipeImages");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_RecipeImageId",
                table: "Recipes",
                column: "RecipeImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeImages_RecipeImageId",
                table: "Recipes",
                column: "RecipeImageId",
                principalTable: "RecipeImages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
