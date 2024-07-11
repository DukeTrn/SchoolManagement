using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Database.Migrations
{
    public partial class AddAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPermission = table.Column<bool>(type: "bit", nullable: false),
                    ClassDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SemesterId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_Attendances_ClassDetails_ClassDetailId",
                        column: x => x.ClassDetailId,
                        principalTable: "ClassDetails",
                        principalColumn: "ClassDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "SemesterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassDetailId",
                table: "Attendances",
                column: "ClassDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SemesterId",
                table: "Attendances",
                column: "SemesterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");
        }
    }
}
