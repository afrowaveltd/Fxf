using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fxf.Blazor.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Hub",
                table: "HubActivityLogs",
                newName: "HubType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HubType",
                table: "HubActivityLogs",
                newName: "Hub");
        }
    }
}
