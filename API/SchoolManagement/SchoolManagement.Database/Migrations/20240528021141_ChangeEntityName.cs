using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Database.Migrations
{
    public partial class ChangeEntityName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SemesterClasses_Classes_ClassId",
                table: "SemesterClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_SemesterClasses_Semesters_SemesterId",
                table: "SemesterClasses");

            migrationBuilder.AlterColumn<string>(
                name: "SemesterId",
                table: "SemesterClasses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ClassId",
                table: "SemesterClasses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SemesterClasses_Classes_ClassId",
                table: "SemesterClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_SemesterClasses_Semesters_SemesterId",
                table: "SemesterClasses");

            migrationBuilder.AlterColumn<string>(
                name: "SemesterId",
                table: "SemesterClasses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClassId",
                table: "SemesterClasses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SemesterClasses_Classes_ClassId",
                table: "SemesterClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SemesterClasses_Semesters_SemesterId",
                table: "SemesterClasses",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "SemesterId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
