using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class Add_Company_CompanyIdentificationNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyIdentificationNumber",
                table: "AppCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AppCompanies_CompanyIdentificationNumber",
                table: "AppCompanies",
                column: "CompanyIdentificationNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppCompanies_CompanyIdentificationNumber",
                table: "AppCompanies");

            migrationBuilder.DropColumn(
                name: "CompanyIdentificationNumber",
                table: "AppCompanies");
        }
    }
}
