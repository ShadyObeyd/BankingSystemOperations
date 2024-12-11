using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystemOperations.Data.Migrations
{
    /// <inheritdoc />
    public partial class MadeExternalIdUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ExternalId",
                table: "Transactions",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_ExternalId",
                table: "Transactions");
        }
    }
}
