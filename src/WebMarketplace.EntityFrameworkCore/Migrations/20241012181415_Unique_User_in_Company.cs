using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class Unique_User_in_Company : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppCompanyMemberships_UserId",
                table: "AppCompanyMemberships");

            migrationBuilder.CreateIndex(
                name: "IX_AppCompanyMemberships_UserId",
                table: "AppCompanyMemberships",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppCompanyMemberships_UserId",
                table: "AppCompanyMemberships");

            migrationBuilder.CreateIndex(
                name: "IX_AppCompanyMemberships_UserId",
                table: "AppCompanyMemberships",
                column: "UserId");
        }
    }
}
