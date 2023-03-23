using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Budget.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Budget");

            migrationBuilder.CreateTable(
                name: "ExpenseCategory",
                schema: "Budget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_ExpenseCategory", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Income",
                schema: "Budget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Income", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Expense",
                schema: "Budget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    ExpenseCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expense_ExpenseCategory_ExpenseCategoryId",
                        column: x => x.ExpenseCategoryId,
                        principalSchema: "Budget",
                        principalTable: "ExpenseCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Budget",
                table: "ExpenseCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Others" },
                    { 2, "Food" },
                    { 3, "Health" },
                    { 4, "Housing" },
                    { 5, "Transportation" },
                    { 6, "Education" },
                    { 7, "Entertainment" },
                    { 8, "Unexpected" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expense_Date",
                schema: "Budget",
                table: "Expense",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_Date_Description",
                schema: "Budget",
                table: "Expense",
                columns: new[] { "Date", "Description" });

            migrationBuilder.CreateIndex(
                name: "IX_Expense_Description",
                schema: "Budget",
                table: "Expense",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ExpenseCategoryId_Date",
                schema: "Budget",
                table: "Expense",
                columns: new[] { "ExpenseCategoryId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_Income_Date",
                schema: "Budget",
                table: "Income",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Income_Description",
                schema: "Budget",
                table: "Income",
                column: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expense",
                schema: "Budget");

            migrationBuilder.DropTable(
                name: "Income",
                schema: "Budget");

            migrationBuilder.DropTable(
                name: "ExpenseCategory",
                schema: "Budget");
        }
    }
}