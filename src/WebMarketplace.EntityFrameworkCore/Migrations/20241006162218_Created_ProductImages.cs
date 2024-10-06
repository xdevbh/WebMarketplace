using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class Created_ProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppProductImages",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlobName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductImages", x => new { x.ProductId, x.BlobName });
                    table.ForeignKey(
                        name: "FK_AppProductImages_AppProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "AppProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppProductImages_ProductId_IsDefault",
                table: "AppProductImages",
                columns: new[] { "ProductId", "IsDefault" },
                unique: true,
                filter: "[IsDefault] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppProductImages");
        }
    }
}
