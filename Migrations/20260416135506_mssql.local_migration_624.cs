using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SymptomLogger.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_624 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SymptomLog_Symptom_SymptomId",
                table: "SymptomLog");

            migrationBuilder.DropIndex(
                name: "IX_SymptomLog_SymptomId",
                table: "SymptomLog");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "SymptomLog");

            migrationBuilder.DropColumn(
                name: "SymptomId",
                table: "SymptomLog");

            migrationBuilder.AlterColumn<string>(
                name: "IcdCode",
                table: "Condition",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SymptomLogEntry",
                columns: table => new
                {
                    SymptomLogEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    SymptomName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SymptomLogId = table.Column<int>(type: "int", nullable: false),
                    SymptomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomLogEntry", x => x.SymptomLogEntryId);
                    table.ForeignKey(
                        name: "FK_SymptomLogEntry_SymptomLog_SymptomLogId",
                        column: x => x.SymptomLogId,
                        principalTable: "SymptomLog",
                        principalColumn: "SymptomLogId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SymptomLogEntry_Symptom_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptom",
                        principalColumn: "SymptomId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SymptomLogEntry_SymptomId",
                table: "SymptomLogEntry",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomLogEntry_SymptomLogId",
                table: "SymptomLogEntry",
                column: "SymptomLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SymptomLogEntry");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                table: "SymptomLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SymptomId",
                table: "SymptomLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "IcdCode",
                table: "Condition",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomLog_SymptomId",
                table: "SymptomLog",
                column: "SymptomId");

            migrationBuilder.AddForeignKey(
                name: "FK_SymptomLog_Symptom_SymptomId",
                table: "SymptomLog",
                column: "SymptomId",
                principalTable: "Symptom",
                principalColumn: "SymptomId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
