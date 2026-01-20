using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace desafio_tecnico.Migrations
{
    /// <inheritdoc />
    public partial class FixManagerDepartament : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_departaments_manager_id",
                table: "departaments");

            migrationBuilder.CreateIndex(
                name: "IX_departaments_manager_id",
                table: "departaments",
                column: "manager_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_departaments_manager_id",
                table: "departaments");

            migrationBuilder.CreateIndex(
                name: "IX_departaments_manager_id",
                table: "departaments",
                column: "manager_id");
        }
    }
}
