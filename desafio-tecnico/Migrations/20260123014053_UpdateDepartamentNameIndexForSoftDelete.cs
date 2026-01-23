using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace desafio_tecnico.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDepartamentNameIndexForSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_departaments_name",
                table: "departaments");

            migrationBuilder.CreateIndex(
                name: "IX_departaments_name",
                table: "departaments",
                column: "name",
                unique: true,
                filter: "\"is_deleted\" = false OR \"is_deleted\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_departaments_name",
                table: "departaments");

            migrationBuilder.CreateIndex(
                name: "IX_departaments_name",
                table: "departaments",
                column: "name",
                unique: true);
        }
    }
}
