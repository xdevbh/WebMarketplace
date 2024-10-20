using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMarketplace.Migrations
{
    /// <inheritdoc />
    public partial class Added_Order_Buyer_ShippingAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrders_AbpUsers_BuyerId",
                table: "AppOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_AppOrders_AppAddresses_AddressId",
                table: "AppOrders");

            migrationBuilder.DropIndex(
                name: "IX_AppOrders_AddressId",
                table: "AppOrders");

            migrationBuilder.DropIndex(
                name: "IX_AppOrders_BuyerId",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AppOrders");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "AppOrders",
                newName: "Buyer_Id");

            migrationBuilder.AddColumn<string>(
                name: "Buyer_Email",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Buyer_Name",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_City",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Country",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Email",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_FullName",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Line1",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Line2",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_Note",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_PhoneNumber",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_State",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_ZipCode",
                table: "AppOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Buyer_Email",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "Buyer_Name",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_City",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Country",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Email",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_FullName",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Line1",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Line2",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_Note",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_PhoneNumber",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_State",
                table: "AppOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress_ZipCode",
                table: "AppOrders");

            migrationBuilder.RenameColumn(
                name: "Buyer_Id",
                table: "AppOrders",
                newName: "BuyerId");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "AppOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AppOrders_AddressId",
                table: "AppOrders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOrders_BuyerId",
                table: "AppOrders",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrders_AbpUsers_BuyerId",
                table: "AppOrders",
                column: "BuyerId",
                principalTable: "AbpUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrders_AppAddresses_AddressId",
                table: "AppOrders",
                column: "AddressId",
                principalTable: "AppAddresses",
                principalColumn: "Id");
        }
    }
}
