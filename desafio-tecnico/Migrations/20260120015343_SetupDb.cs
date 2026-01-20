using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace desafio_tecnico.Migrations
{
    /// <inheritdoc />
    public partial class SetupDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departaments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    manager_id = table.Column<int>(type: "integer", nullable: true),
                    higher_departament_id = table.Column<int>(type: "integer", nullable: true),
                    createdAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "NOW()"),
                    updatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departaments", x => x.id);
                    table.CheckConstraint("chk_higher_departament_not_self", "higher_departament_id IS NULL OR higher_departament_id <> id");
                    table.ForeignKey(
                        name: "FK_departaments_departaments_higher_departament_id",
                        column: x => x.higher_departament_id,
                        principalTable: "departaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    cpf = table.Column<string>(type: "CHAR(11)", maxLength: 11, nullable: false),
                    rg = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    departament_id = table.Column<int>(type: "integer", nullable: false),
                    createdAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "NOW()"),
                    updatedAt = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.id);
                    table.ForeignKey(
                        name: "FK_employees_departaments_departament_id",
                        column: x => x.departament_id,
                        principalTable: "departaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_departaments_higher_departament_id",
                table: "departaments",
                column: "higher_departament_id");

            migrationBuilder.CreateIndex(
                name: "IX_departaments_manager_id",
                table: "departaments",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "IX_departaments_name",
                table: "departaments",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_cpf",
                table: "employees",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_departament_id",
                table: "employees",
                column: "departament_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_rg",
                table: "employees",
                column: "rg",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_departaments_employees_manager_id",
                table: "departaments",
                column: "manager_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departaments_employees_manager_id",
                table: "departaments");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "departaments");
        }
    }
}
