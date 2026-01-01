using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCook.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFoodImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "FoodImages",
                newName: "PublicId");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "FoodImages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "FoodImages");

            migrationBuilder.RenameColumn(
                name: "PublicId",
                table: "FoodImages",
                newName: "Image");
        }
    }
}
