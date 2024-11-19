using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCook.Api.Migrations
{
    /// <inheritdoc />
    public partial class Updatedmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Recipes_RecipeId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeImages_Recipes_RecipeId",
                table: "RecipeImages");

            migrationBuilder.RenameColumn(
                name: "RecipeImageId",
                table: "Recipes",
                newName: "FoodImageId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RecipeImages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "RecipeImages",
                newName: "FoodId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeImages_RecipeId",
                table: "RecipeImages",
                newName: "IX_RecipeImages_FoodId");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "Orders",
                newName: "FoodId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_RecipeId",
                table: "Orders",
                newName: "IX_Orders_FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Recipes_FoodId",
                table: "Orders",
                column: "FoodId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeImages_Recipes_FoodId",
                table: "RecipeImages",
                column: "FoodId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Recipes_FoodId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeImages_Recipes_FoodId",
                table: "RecipeImages");

            migrationBuilder.RenameColumn(
                name: "FoodImageId",
                table: "Recipes",
                newName: "RecipeImageId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RecipeImages",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "FoodId",
                table: "RecipeImages",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeImages_FoodId",
                table: "RecipeImages",
                newName: "IX_RecipeImages_RecipeId");

            migrationBuilder.RenameColumn(
                name: "FoodId",
                table: "Orders",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_FoodId",
                table: "Orders",
                newName: "IX_Orders_RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Recipes_RecipeId",
                table: "Orders",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeImages_Recipes_RecipeId",
                table: "RecipeImages",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
