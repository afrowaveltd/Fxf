using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fxf.Blazor.Migrations
{
    /// <inheritdoc />
    public partial class AddedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CycleChecks_DefaultTranslationFound",
                table: "WorkerResults",
                newName: "CycleChecks_DefaultServerTranslationFound");

            migrationBuilder.AddColumn<bool>(
                name: "CycleChecks_DefaultClientTranslationFound",
                table: "WorkerResults",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CycleChecks_DefaultClientTranslationFound",
                table: "WorkerResults");

            migrationBuilder.RenameColumn(
                name: "CycleChecks_DefaultServerTranslationFound",
                table: "WorkerResults",
                newName: "CycleChecks_DefaultTranslationFound");
        }
    }
}
