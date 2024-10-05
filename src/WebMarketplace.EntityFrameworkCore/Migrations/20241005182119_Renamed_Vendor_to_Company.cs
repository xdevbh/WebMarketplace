using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class Renamed_Vendor_to_Company : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrders_AppVendors_VendorId",
                table: "AppOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProducts_AppVendors_VendorId",
                table: "AppProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_AppVendorUsers_AppVendors_VendorId",
                table: "AppVendorUsers");

            migrationBuilder.DropTable(
                name: "AppVendors");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "AppVendorUsers",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_AppVendorUsers_VendorId_UserId",
                table: "AppVendorUsers",
                newName: "IX_AppVendorUsers_CompanyId_UserId");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "AppProducts",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_AppProducts_VendorId",
                table: "AppProducts",
                newName: "IX_AppProducts_CompanyId");

            migrationBuilder.RenameColumn(
                name: "VendorName",
                table: "AppOrders",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "AppOrders",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_AppOrders_VendorId",
                table: "AppOrders",
                newName: "IX_AppOrders_CompanyId");

            migrationBuilder.CreateTable(
                name: "AppCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCompanies_AppAddresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "AppAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCompanies_AddressId",
                table: "AppCompanies",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCompanies_DisplayName",
                table: "AppCompanies",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCompanies_Name",
                table: "AppCompanies",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrders_AppCompanies_CompanyId",
                table: "AppOrders",
                column: "CompanyId",
                principalTable: "AppCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProducts_AppCompanies_CompanyId",
                table: "AppProducts",
                column: "CompanyId",
                principalTable: "AppCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppVendorUsers_AppCompanies_CompanyId",
                table: "AppVendorUsers",
                column: "CompanyId",
                principalTable: "AppCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrders_AppCompanies_CompanyId",
                table: "AppOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProducts_AppCompanies_CompanyId",
                table: "AppProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_AppVendorUsers_AppCompanies_CompanyId",
                table: "AppVendorUsers");

            migrationBuilder.DropTable(
                name: "AppCompanies");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "AppVendorUsers",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_AppVendorUsers_CompanyId_UserId",
                table: "AppVendorUsers",
                newName: "IX_AppVendorUsers_VendorId_UserId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "AppProducts",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_AppProducts_CompanyId",
                table: "AppProducts",
                newName: "IX_AppProducts_VendorId");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "AppOrders",
                newName: "VendorName");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "AppOrders",
                newName: "VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_AppOrders_CompanyId",
                table: "AppOrders",
                newName: "IX_AppOrders_VendorId");

            migrationBuilder.CreateTable(
                name: "AppVendors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVendors_AppAddresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "AppAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppVendors_AddressId",
                table: "AppVendors",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVendors_DisplayName",
                table: "AppVendors",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppVendors_Name",
                table: "AppVendors",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrders_AppVendors_VendorId",
                table: "AppOrders",
                column: "VendorId",
                principalTable: "AppVendors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProducts_AppVendors_VendorId",
                table: "AppProducts",
                column: "VendorId",
                principalTable: "AppVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppVendorUsers_AppVendors_VendorId",
                table: "AppVendorUsers",
                column: "VendorId",
                principalTable: "AppVendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
