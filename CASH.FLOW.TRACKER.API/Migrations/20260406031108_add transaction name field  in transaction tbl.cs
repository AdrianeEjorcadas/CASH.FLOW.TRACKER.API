using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CASH.FLOW.TRACKER.API.Migrations
{
    /// <inheritdoc />
    public partial class addtransactionnamefieldintransactiontbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionName",
                table: "Transactions");
        }
    }
}
