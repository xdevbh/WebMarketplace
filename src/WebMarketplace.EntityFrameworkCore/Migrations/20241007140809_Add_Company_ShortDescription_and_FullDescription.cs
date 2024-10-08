using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class Add_Company_ShortDescription_and_FullDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AppCompanies",
                newName: "ShortDescription");

            migrationBuilder.AddColumn<string>(
                name: "FullDescription",
                table: "AppCompanies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullDescription",
                table: "AppCompanies");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "AppCompanies",
                newName: "Description");
        }
    }
}
