using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SymptomLogger.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_119 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Symptom",
                newName: "SymptomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SymptomId",
                table: "Symptom",
                newName: "Id");
        }
    }
}
