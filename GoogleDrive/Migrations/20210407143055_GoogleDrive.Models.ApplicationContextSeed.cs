using Microsoft.EntityFrameworkCore.Migrations;

namespace GoogleDrive.Migrations
{
    public partial class GoogleDriveModelsApplicationContextSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TrainingFile",
                columns: new[] { "FileID", "FileName", "FilePath" },
                values: new object[] { 1, "Temp1", "Test1" });

            migrationBuilder.InsertData(
                table: "TrainingFile",
                columns: new[] { "FileID", "FileName", "FilePath" },
                values: new object[] { 2, "Temp2", "Test2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TrainingFile",
                keyColumn: "FileID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TrainingFile",
                keyColumn: "FileID",
                keyValue: 2);
        }
    }
}
