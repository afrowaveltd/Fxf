using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fxf.Blazor.Migrations
{
    /// <inheritdoc />
    public partial class WorkerResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkerResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Successful = table.Column<bool>(type: "bit", nullable: false),
                    LastStatus = table.Column<int>(type: "int", nullable: false),
                    CycleChecks_SettingsLoaded = table.Column<bool>(type: "bit", nullable: false),
                    CycleChecks_StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CycleChecks_EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CycleChecks_DefaultTranslationFound = table.Column<bool>(type: "bit", nullable: false),
                    CycleChecks_IgnoredLanguagesFound = table.Column<bool>(type: "bit", nullable: false),
                    CycleChecks_LibreLanguagesCount = table.Column<int>(type: "int", nullable: false),
                    CycleChecks_FrontendTranslations_StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CycleChecks_FrontendTranslations_EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CycleChecks_FrontendTranslations_OldFileFound = table.Column<bool>(type: "bit", nullable: false),
                    CycleChecks_FrontendTranslations_TranslationsNeeded = table.Column<int>(type: "int", nullable: false),
                    CycleChecks_BackendTranslations_StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CycleChecks_BackendTranslations_EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CycleChecks_BackendTranslations_OldFileFound = table.Column<bool>(type: "bit", nullable: false),
                    CycleChecks_BackendTranslations_TranslationsNeeded = table.Column<int>(type: "int", nullable: false),
                    LanguagesTranslations_TranslationsNeeded = table.Column<int>(type: "int", nullable: false),
                    LanguagesTranslations_TranslationsDone = table.Column<int>(type: "int", nullable: false),
                    LanguagesTranslations_TranslationErrors = table.Column<int>(type: "int", nullable: false),
                    LanguagesTranslations_StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LanguagesTranslations_EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TranslationRequests_LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslationRequests_ToAdd = table.Column<int>(type: "int", nullable: false),
                    TranslationRequests_ToRemove = table.Column<int>(type: "int", nullable: false),
                    TranslationRequests_ToUpdate = table.Column<int>(type: "int", nullable: false),
                    TranslationResults_LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslationResults_SuccessfulTranslations = table.Column<int>(type: "int", nullable: false),
                    TranslationResults_TranslationErrors = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Backend_ServerRequestedTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToAdd = table.Column<int>(type: "int", nullable: false),
                    ToRemove = table.Column<int>(type: "int", nullable: false),
                    ToUpdate = table.Column<int>(type: "int", nullable: false),
                    WorkerResultsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backend_ServerRequestedTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Backend_ServerRequestedTranslations_WorkerResults_WorkerResultsId",
                        column: x => x.WorkerResultsId,
                        principalTable: "WorkerResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Backend_ServerResultOfTranslating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuccessfulTranslations = table.Column<int>(type: "int", nullable: false),
                    TranslationErrors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkerResultsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backend_ServerResultOfTranslating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Backend_ServerResultOfTranslating_WorkerResults_WorkerResultsId",
                        column: x => x.WorkerResultsId,
                        principalTable: "WorkerResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FailedTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkerResultsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FailedTranslations_WorkerResults_WorkerResultsId",
                        column: x => x.WorkerResultsId,
                        principalTable: "WorkerResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Frontend_ServerRequestedTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToAdd = table.Column<int>(type: "int", nullable: false),
                    ToRemove = table.Column<int>(type: "int", nullable: false),
                    ToUpdate = table.Column<int>(type: "int", nullable: false),
                    WorkerResultsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frontend_ServerRequestedTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Frontend_ServerRequestedTranslations_WorkerResults_WorkerResultsId",
                        column: x => x.WorkerResultsId,
                        principalTable: "WorkerResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Frontend_ServerResultOfTranslating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuccessfulTranslations = table.Column<int>(type: "int", nullable: false),
                    TranslationErrors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkerResultsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frontend_ServerResultOfTranslating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Frontend_ServerResultOfTranslating_WorkerResults_WorkerResultsId",
                        column: x => x.WorkerResultsId,
                        principalTable: "WorkerResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerErrorMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkerStatus = table.Column<int>(type: "int", nullable: false),
                    WorkerResultsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerErrorMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerErrorMessages_WorkerResults_WorkerResultsId",
                        column: x => x.WorkerResultsId,
                        principalTable: "WorkerResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Backend_ServerRequestedTranslations_WorkerResultsId",
                table: "Backend_ServerRequestedTranslations",
                column: "WorkerResultsId");

            migrationBuilder.CreateIndex(
                name: "IX_Backend_ServerResultOfTranslating_WorkerResultsId",
                table: "Backend_ServerResultOfTranslating",
                column: "WorkerResultsId");

            migrationBuilder.CreateIndex(
                name: "IX_FailedTranslations_WorkerResultsId",
                table: "FailedTranslations",
                column: "WorkerResultsId");

            migrationBuilder.CreateIndex(
                name: "IX_Frontend_ServerRequestedTranslations_WorkerResultsId",
                table: "Frontend_ServerRequestedTranslations",
                column: "WorkerResultsId");

            migrationBuilder.CreateIndex(
                name: "IX_Frontend_ServerResultOfTranslating_WorkerResultsId",
                table: "Frontend_ServerResultOfTranslating",
                column: "WorkerResultsId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerErrorMessages_WorkerResultsId",
                table: "WorkerErrorMessages",
                column: "WorkerResultsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Backend_ServerRequestedTranslations");

            migrationBuilder.DropTable(
                name: "Backend_ServerResultOfTranslating");

            migrationBuilder.DropTable(
                name: "FailedTranslations");

            migrationBuilder.DropTable(
                name: "Frontend_ServerRequestedTranslations");

            migrationBuilder.DropTable(
                name: "Frontend_ServerResultOfTranslating");

            migrationBuilder.DropTable(
                name: "WorkerErrorMessages");

            migrationBuilder.DropTable(
                name: "WorkerResults");
        }
    }
}
