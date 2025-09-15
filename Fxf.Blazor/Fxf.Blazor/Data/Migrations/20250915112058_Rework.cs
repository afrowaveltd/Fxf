using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fxf.Blazor.Migrations
{
	/// <inheritdoc/>
	public partial class Rework : Migration
	{
		/// <inheritdoc/>
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				 name: "BackendTranslationRequests_RequestedTranslations");

			migrationBuilder.DropTable(
				 name: "BackendTranslationResults");

			migrationBuilder.DropTable(
				 name: "FrontendTranslationResults_RequestedTranslations");

			migrationBuilder.DropTable(
				 name: "FrontendTranslationResults_ResultOfTranslating");

			migrationBuilder.DropTable(
				 name: "SignalRConnections");

			migrationBuilder.DropTable(
				 name: "WorkerErrors");

			migrationBuilder.DropTable(
				 name: "BackendTranslationRequests");

			migrationBuilder.DropTable(
				 name: "FrontendTranslationResults");

			migrationBuilder.RenameColumn(
				 name: "CleanupResults_StartTime",
				 table: "WorkerResults",
				 newName: "CycleChecks_FrontendTranslations_StartTime");

			migrationBuilder.RenameColumn(
				 name: "CleanupResults_EndTime",
				 table: "WorkerResults",
				 newName: "CycleChecks_FrontendTranslations_EndTime");

			migrationBuilder.RenameColumn(
				 name: "CleanupResults_ServerOldLanguageStored",
				 table: "WorkerResults",
				 newName: "CycleChecks_FrontendTranslations_OldFileFound");

			migrationBuilder.RenameColumn(
				 name: "CleanupResults_OldTranslationResultsDeleted",
				 table: "WorkerResults",
				 newName: "TranslationResults_SuccessfulTranslations");

			migrationBuilder.RenameColumn(
				 name: "CleanupResults_ClientOldLanguageStored",
				 table: "WorkerResults",
				 newName: "CycleChecks_DefaultServerTranslationFound");

			migrationBuilder.AddColumn<DateTime>(
				 name: "CycleChecks_BackendTranslations_EndTime",
				 table: "WorkerResults",
				 type: "datetime2",
				 nullable: false,
				 defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<bool>(
				 name: "CycleChecks_BackendTranslations_OldFileFound",
				 table: "WorkerResults",
				 type: "bit",
				 nullable: false,
				 defaultValue: false);

			migrationBuilder.AddColumn<DateTime>(
				 name: "CycleChecks_BackendTranslations_StartTime",
				 table: "WorkerResults",
				 type: "datetime2",
				 nullable: false,
				 defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<int>(
				 name: "CycleChecks_BackendTranslations_TranslationsNeeded",
				 table: "WorkerResults",
				 type: "int",
				 nullable: false,
				 defaultValue: 0);

			migrationBuilder.AddColumn<bool>(
				 name: "CycleChecks_DefaultClientTranslationFound",
				 table: "WorkerResults",
				 type: "bit",
				 nullable: false,
				 defaultValue: false);

			migrationBuilder.AddColumn<int>(
				 name: "CycleChecks_FrontendTranslations_TranslationsNeeded",
				 table: "WorkerResults",
				 type: "int",
				 nullable: false,
				 defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				 name: "LanguagesTranslations_TranslationErrors",
				 table: "WorkerResults",
				 type: "int",
				 nullable: false,
				 defaultValue: 0);

			migrationBuilder.AddColumn<string>(
				 name: "TranslationRequests_LanguageCode",
				 table: "WorkerResults",
				 type: "nvarchar(max)",
				 nullable: false,
				 defaultValue: "");

			migrationBuilder.AddColumn<int>(
				 name: "TranslationRequests_ToAdd",
				 table: "WorkerResults",
				 type: "int",
				 nullable: false,
				 defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				 name: "TranslationRequests_ToRemove",
				 table: "WorkerResults",
				 type: "int",
				 nullable: false,
				 defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				 name: "TranslationRequests_ToUpdate",
				 table: "WorkerResults",
				 type: "int",
				 nullable: false,
				 defaultValue: 0);

			migrationBuilder.AddColumn<string>(
				 name: "TranslationResults_LanguageCode",
				 table: "WorkerResults",
				 type: "nvarchar(max)",
				 nullable: false,
				 defaultValue: "");

			migrationBuilder.AddColumn<string>(
				 name: "TranslationResults_TranslationErrors",
				 table: "WorkerResults",
				 type: "nvarchar(max)",
				 nullable: false,
				 defaultValue: "");

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

			migrationBuilder.CreateIndex(
				 name: "IX_Backend_ServerRequestedTranslations_WorkerResultsId",
				 table: "Backend_ServerRequestedTranslations",
				 column: "WorkerResultsId");

			migrationBuilder.CreateIndex(
				 name: "IX_Backend_ServerResultOfTranslating_WorkerResultsId",
				 table: "Backend_ServerResultOfTranslating",
				 column: "WorkerResultsId");

			migrationBuilder.CreateIndex(
				 name: "IX_Frontend_ServerRequestedTranslations_WorkerResultsId",
				 table: "Frontend_ServerRequestedTranslations",
				 column: "WorkerResultsId");

			migrationBuilder.CreateIndex(
				 name: "IX_Frontend_ServerResultOfTranslating_WorkerResultsId",
				 table: "Frontend_ServerResultOfTranslating",
				 column: "WorkerResultsId");
		}

		/// <inheritdoc/>
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				 name: "Backend_ServerRequestedTranslations");

			migrationBuilder.DropTable(
				 name: "Backend_ServerResultOfTranslating");

			migrationBuilder.DropTable(
				 name: "Frontend_ServerRequestedTranslations");

			migrationBuilder.DropTable(
				 name: "Frontend_ServerResultOfTranslating");

			migrationBuilder.DropColumn(
				 name: "CycleChecks_BackendTranslations_EndTime",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "CycleChecks_BackendTranslations_OldFileFound",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "CycleChecks_BackendTranslations_StartTime",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "CycleChecks_BackendTranslations_TranslationsNeeded",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "CycleChecks_DefaultClientTranslationFound",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "CycleChecks_FrontendTranslations_TranslationsNeeded",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "LanguagesTranslations_TranslationErrors",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "TranslationRequests_LanguageCode",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "TranslationRequests_ToAdd",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "TranslationRequests_ToRemove",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "TranslationRequests_ToUpdate",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "TranslationResults_LanguageCode",
				 table: "WorkerResults");

			migrationBuilder.DropColumn(
				 name: "TranslationResults_TranslationErrors",
				 table: "WorkerResults");

			migrationBuilder.RenameColumn(
				 name: "CycleChecks_FrontendTranslations_StartTime",
				 table: "WorkerResults",
				 newName: "CleanupResults_StartTime");

			migrationBuilder.RenameColumn(
				 name: "CycleChecks_FrontendTranslations_EndTime",
				 table: "WorkerResults",
				 newName: "CleanupResults_EndTime");

			migrationBuilder.RenameColumn(
				 name: "TranslationResults_SuccessfulTranslations",
				 table: "WorkerResults",
				 newName: "CleanupResults_OldTranslationResultsDeleted");

			migrationBuilder.RenameColumn(
				 name: "CycleChecks_FrontendTranslations_OldFileFound",
				 table: "WorkerResults",
				 newName: "CleanupResults_ServerOldLanguageStored");

			migrationBuilder.RenameColumn(
				 name: "CycleChecks_DefaultServerTranslationFound",
				 table: "WorkerResults",
				 newName: "CleanupResults_ClientOldLanguageStored");

			migrationBuilder.CreateTable(
				 name: "BackendTranslationRequests",
				 columns: table => new
				 {
					 Id = table.Column<int>(type: "int", nullable: false)
							.Annotation("SqlServer:Identity", "1, 1"),
					 StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					 EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					 OldFileFound = table.Column<bool>(type: "bit", nullable: false),
					 TranslationsNeeded = table.Column<int>(type: "int", nullable: false),
					 TranslationsId = table.Column<int>(type: "int", nullable: false)
				 },
				 constraints: table =>
				 {
					 table.PrimaryKey("PK_BackendTranslationRequests", x => x.Id);
					 table.ForeignKey(
							  name: "FK_BackendTranslationRequests_WorkerResults_TranslationsId",
							  column: x => x.TranslationsId,
							  principalTable: "WorkerResults",
							  principalColumn: "Id",
							  onDelete: ReferentialAction.Cascade);
				 });

			migrationBuilder.CreateTable(
				 name: "FrontendTranslationResults",
				 columns: table => new
				 {
					 Id = table.Column<int>(type: "int", nullable: false)
							.Annotation("SqlServer:Identity", "1, 1"),
					 StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					 EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					 OldFileFound = table.Column<bool>(type: "bit", nullable: false),
					 TranslationsNeeded = table.Column<int>(type: "int", nullable: false),
					 TranslationsId = table.Column<int>(type: "int", nullable: false)
				 },
				 constraints: table =>
				 {
					 table.PrimaryKey("PK_FrontendTranslationResults", x => x.Id);
					 table.ForeignKey(
							  name: "FK_FrontendTranslationResults_WorkerResults_TranslationsId",
							  column: x => x.TranslationsId,
							  principalTable: "WorkerResults",
							  principalColumn: "Id",
							  onDelete: ReferentialAction.Cascade);
				 });

			migrationBuilder.CreateTable(
				 name: "SignalRConnections",
				 columns: table => new
				 {
					 Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
					 UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
					 ConnectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					 DisconnectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
					 UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
					 IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
					 Hub = table.Column<int>(type: "int", nullable: false),
					 IsAuthenticated = table.Column<bool>(type: "bit", nullable: false)
				 },
				 constraints: table =>
				 {
					 table.PrimaryKey("PK_SignalRConnections", x => x.Id);
					 table.ForeignKey(
							  name: "FK_SignalRConnections_AspNetUsers_UserId",
							  column: x => x.UserId,
							  principalTable: "AspNetUsers",
							  principalColumn: "Id",
							  onDelete: ReferentialAction.SetNull);
				 });

			migrationBuilder.CreateTable(
				 name: "WorkerErrors",
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
					 table.PrimaryKey("PK_WorkerErrors", x => x.Id);
					 table.ForeignKey(
							  name: "FK_WorkerErrors_WorkerResults_WorkerResultsId",
							  column: x => x.WorkerResultsId,
							  principalTable: "WorkerResults",
							  principalColumn: "Id",
							  onDelete: ReferentialAction.Cascade);
				 });

			migrationBuilder.CreateTable(
				 name: "BackendTranslationRequests_RequestedTranslations",
				 columns: table => new
				 {
					 TranslationsId = table.Column<int>(type: "int", nullable: false),
					 Id = table.Column<int>(type: "int", nullable: false)
							.Annotation("SqlServer:Identity", "1, 1"),
					 LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
					 Phrase = table.Column<string>(type: "nvarchar(max)", nullable: false),
					 ChangeType = table.Column<int>(type: "int", nullable: false)
				 },
				 constraints: table =>
				 {
					 table.PrimaryKey("PK_BackendTranslationRequests_RequestedTranslations", x => new { x.TranslationsId, x.Id });
					 table.ForeignKey(
							  name: "FK_BackendTranslationRequests_RequestedTranslations_BackendTranslationRequests_TranslationsId",
							  column: x => x.TranslationsId,
							  principalTable: "BackendTranslationRequests",
							  principalColumn: "Id",
							  onDelete: ReferentialAction.Cascade);
				 });

			migrationBuilder.CreateTable(
				 name: "BackendTranslationResults",
				 columns: table => new
				 {
					 Id = table.Column<int>(type: "int", nullable: false)
							.Annotation("SqlServer:Identity", "1, 1"),
					 LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
					 Successful = table.Column<bool>(type: "bit", nullable: false),
					 Phrase = table.Column<string>(type: "nvarchar(max)", nullable: false),
					 TranslationsId = table.Column<int>(type: "int", nullable: false)
				 },
				 constraints: table =>
				 {
					 table.PrimaryKey("PK_BackendTranslationResults", x => x.Id);
					 table.ForeignKey(
							  name: "FK_BackendTranslationResults_BackendTranslationRequests_TranslationsId",
							  column: x => x.TranslationsId,
							  principalTable: "BackendTranslationRequests",
							  principalColumn: "Id",
							  onDelete: ReferentialAction.Cascade);
				 });

			migrationBuilder.CreateTable(
				 name: "FrontendTranslationResults_RequestedTranslations",
				 columns: table => new
				 {
					 TranslationsId = table.Column<int>(type: "int", nullable: false),
					 Id = table.Column<int>(type: "int", nullable: false)
							.Annotation("SqlServer:Identity", "1, 1"),
					 LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
					 Phrase = table.Column<string>(type: "nvarchar(max)", nullable: false),
					 ChangeType = table.Column<int>(type: "int", nullable: false)
				 },
				 constraints: table =>
				 {
					 table.PrimaryKey("PK_FrontendTranslationResults_RequestedTranslations", x => new { x.TranslationsId, x.Id });
					 table.ForeignKey(
							  name: "FK_FrontendTranslationResults_RequestedTranslations_FrontendTranslationResults_TranslationsId",
							  column: x => x.TranslationsId,
							  principalTable: "FrontendTranslationResults",
							  principalColumn: "Id",
							  onDelete: ReferentialAction.Cascade);
				 });

			migrationBuilder.CreateTable(
				 name: "FrontendTranslationResults_ResultOfTranslating",
				 columns: table => new
				 {
					 TranslationsId = table.Column<int>(type: "int", nullable: false),
					 Id = table.Column<int>(type: "int", nullable: false)
							.Annotation("SqlServer:Identity", "1, 1"),
					 LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
					 Successful = table.Column<bool>(type: "bit", nullable: false),
					 Phrase = table.Column<string>(type: "nvarchar(max)", nullable: false)
				 },
				 constraints: table =>
				 {
					 table.PrimaryKey("PK_FrontendTranslationResults_ResultOfTranslating", x => new { x.TranslationsId, x.Id });
					 table.ForeignKey(
							  name: "FK_FrontendTranslationResults_ResultOfTranslating_FrontendTranslationResults_TranslationsId",
							  column: x => x.TranslationsId,
							  principalTable: "FrontendTranslationResults",
							  principalColumn: "Id",
							  onDelete: ReferentialAction.Cascade);
				 });

			migrationBuilder.CreateIndex(
				 name: "IX_BackendTranslationRequests_TranslationsId",
				 table: "BackendTranslationRequests",
				 column: "TranslationsId",
				 unique: true);

			migrationBuilder.CreateIndex(
				 name: "IX_BackendTranslationResults_TranslationsId",
				 table: "BackendTranslationResults",
				 column: "TranslationsId");

			migrationBuilder.CreateIndex(
				 name: "IX_FrontendTranslationResults_TranslationsId",
				 table: "FrontendTranslationResults",
				 column: "TranslationsId",
				 unique: true);

			migrationBuilder.CreateIndex(
				 name: "IX_SignalRConnections_UserId",
				 table: "SignalRConnections",
				 column: "UserId");

			migrationBuilder.CreateIndex(
				 name: "IX_WorkerErrors_WorkerResultsId",
				 table: "WorkerErrors",
				 column: "WorkerResultsId");
		}
	}
}