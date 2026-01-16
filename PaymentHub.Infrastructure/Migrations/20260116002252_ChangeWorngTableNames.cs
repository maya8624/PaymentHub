using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayPalIntegration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWorngTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Business_OrderId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Business",
                table: "Business");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "Business",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_OrderId",
                table: "Payment",
                newName: "IX_Payment_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Order_OrderId",
                table: "Payment",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Order_OrderId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Business");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_OrderId",
                table: "Customers",
                newName: "IX_Customers_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Business",
                table: "Business",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Business_OrderId",
                table: "Customers",
                column: "OrderId",
                principalTable: "Business",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
