using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace desafio_tecnico.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeCPFRGIndexForSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_employees_cpf",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_rg",
                table: "employees");

            migrationBuilder.CreateIndex(
                name: "IX_employees_cpf",
                table: "employees",
                column: "cpf",
                unique: true,
                filter: "\"is_deleted\" = false OR \"is_deleted\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_employees_rg",
                table: "employees",
                column: "rg",
                unique: true,
                filter: "\"is_deleted\" = false OR \"is_deleted\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_employees_cpf",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_rg",
                table: "employees");

            migrationBuilder.CreateIndex(
                name: "IX_employees_cpf",
                table: "employees",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_rg",
                table: "employees",
                column: "rg",
                unique: true);
        }
    }
}
