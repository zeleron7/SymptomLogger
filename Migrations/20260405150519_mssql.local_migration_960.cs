using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SymptomLogger.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_960 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Symptom");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "SymptomLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SleepHours",
                table: "SymptomLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StressLevel",
                table: "SymptomLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isFlare",
                table: "SymptomLog",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Symptom",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BodySystem",
                table: "Symptom",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Condition",
                columns: table => new
                {
                    ConditionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    System = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IcdCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condition", x => x.ConditionId);
                });

            migrationBuilder.CreateTable(
                name: "ConditionSymptom",
                columns: table => new
                {
                    ConditionSymptomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsHallmark = table.Column<bool>(type: "bit", nullable: false),
                    ConditionId = table.Column<int>(type: "int", nullable: false),
                    SymptomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionSymptom", x => x.ConditionSymptomId);
                    table.ForeignKey(
                        name: "FK_ConditionSymptom_Condition_ConditionId",
                        column: x => x.ConditionId,
                        principalTable: "Condition",
                        principalColumn: "ConditionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConditionSymptom_Symptom_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptom",
                        principalColumn: "SymptomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConditionSymptom_ConditionId",
                table: "ConditionSymptom",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionSymptom_SymptomId",
                table: "ConditionSymptom",
                column: "SymptomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConditionSymptom");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "SymptomLog");

            migrationBuilder.DropColumn(
                name: "SleepHours",
                table: "SymptomLog");

            migrationBuilder.DropColumn(
                name: "StressLevel",
                table: "SymptomLog");

            migrationBuilder.DropColumn(
                name: "isFlare",
                table: "SymptomLog");

            migrationBuilder.DropColumn(
                name: "BodySystem",
                table: "Symptom");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Symptom",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Symptom",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
