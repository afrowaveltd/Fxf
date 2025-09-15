using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fxf.Blazor.Migrations
{
    /// <inheritdoc />
    public partial class AddHubActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignalRConnections_AspNetUsers_UserId",
                table: "SignalRConnections");

            migrationBuilder.CreateTable(
                name: "HubActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hub = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemoteIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubActivityLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HubActivityLogs_ConnectionId",
                table: "HubActivityLogs",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_HubActivityLogs_UserId",
                table: "HubActivityLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SignalRConnections_AspNetUsers_UserId",
                table: "SignalRConnections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignalRConnections_AspNetUsers_UserId",
                table: "SignalRConnections");

            migrationBuilder.DropTable(
                name: "HubActivityLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_SignalRConnections_AspNetUsers_UserId",
                table: "SignalRConnections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
