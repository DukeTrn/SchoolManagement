using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Database.Migrations
{
    public partial class ChangeEntityName2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SemesterClasses_Classes_ClassId",
                table: "SemesterClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_SemesterClasses_Semesters_SemesterId",
                table: "SemesterClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SemesterClasses",
                table: "SemesterClasses");

            migrationBuilder.RenameTable(
                name: "SemesterClasses",
                newName: "SemesterDetails");

            migrationBuilder.RenameIndex(
                name: "IX_SemesterClasses_SemesterId",
                table: "SemesterDetails",
                newName: "IX_SemesterDetails_SemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_SemesterClasses_ClassId",
                table: "SemesterDetails",
                newName: "IX_SemesterDetails_ClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SemesterDetails",
                table: "SemesterDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SemesterDetails_Classes_ClassId",
                table: "SemesterDetails",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_SemesterDetails_Semesters_SemesterId",
                table: "SemesterDetails",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SemesterDetails_Classes_ClassId",
                table: "SemesterDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SemesterDetails_Semesters_SemesterId",
                table: "SemesterDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SemesterDetails",
                table: "SemesterDetails");

            migrationBuilder.RenameTable(
                name: "SemesterDetails",
                newName: "SemesterClasses");

            migrationBuilder.RenameIndex(
                name: "IX_SemesterDetails_SemesterId",
                table: "SemesterClasses",
                newName: "IX_SemesterClasses_SemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_SemesterDetails_ClassId",
                table: "SemesterClasses",
                newName: "IX_SemesterClasses_ClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SemesterClasses",
                table: "SemesterClasses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SemesterClasses_Classes_ClassId",
                table: "SemesterClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_SemesterClasses_Semesters_SemesterId",
                table: "SemesterClasses",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId");
        }
    }
}
