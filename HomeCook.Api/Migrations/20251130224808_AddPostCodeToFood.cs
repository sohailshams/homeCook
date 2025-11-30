using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCook.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPostCodeToFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "Foods",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Foods");
        }
    }
}
