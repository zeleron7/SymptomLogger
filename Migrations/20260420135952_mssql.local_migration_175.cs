using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SymptomLogger.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_175 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConditionId",
                table: "SymptomLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SymptomLog_ConditionId",
                table: "SymptomLog",
                column: "ConditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SymptomLog_Condition_ConditionId",
                table: "SymptomLog",
                column: "ConditionId",
                principalTable: "Condition",
                principalColumn: "ConditionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SymptomLog_Condition_ConditionId",
                table: "SymptomLog");

            migrationBuilder.DropIndex(
                name: "IX_SymptomLog_ConditionId",
                table: "SymptomLog");

            migrationBuilder.DropColumn(
                name: "ConditionId",
                table: "SymptomLog");
        }
    }
}
