using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class Created_CompanyImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppProductImages_ProductId_IsDefault",
                table: "AppProductImages");

            migrationBuilder.CreateTable(
                name: "AppCompanyImages",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlobName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCompanyImages", x => new { x.CompanyId, x.BlobName });
                    table.ForeignKey(
                        name: "FK_AppCompanyImages_AppCompanies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "AppCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCompanyImages_CompanyId_IsDefault",
                table: "AppCompanyImages",
                columns: new[] { "CompanyId", "IsDefault" },
                unique: true,
                filter: "[IsDefault] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppCompanyImages");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductImages_ProductId_IsDefault",
                table: "AppProductImages",
                columns: new[] { "ProductId", "IsDefault" },
                unique: true,
                filter: "[IsDefault] = 1");
        }
    }
}
