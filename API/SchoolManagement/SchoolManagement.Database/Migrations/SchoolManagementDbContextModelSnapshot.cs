﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolManagement.Database;

#nullable disable

namespace SchoolManagement.Database.Migrations
{
    [DbContext(typeof(SchoolManagementDbContext))]
    partial class SchoolManagementDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SchoolManagement.Entity.AccountEntity", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHashed")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("SchoolManagement.Entity.AssessmentEntity", b =>
                {
                    b.Property<Guid>("AssessmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClassDetailId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Feedback")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Score")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SemesterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("AssessmentId");

                    b.HasIndex("ClassDetailId");

                    b.HasIndex("SemesterId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Assessments");
                });

            modelBuilder.Entity("SchoolManagement.Entity.AssignmentEntity", b =>
                {
                    b.Property<Guid>("AssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClassId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SemesterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<string>("TeacherId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AssignmentId");

                    b.HasIndex("ClassId");

                    b.HasIndex("SemesterId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("SchoolManagement.Entity.ClassDetailEntity", b =>
                {
                    b.Property<string>("ClassDetailId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClassId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ClassDetailId");

                    b.HasIndex("ClassId");

                    b.HasIndex("StudentId");

                    b.ToTable("ClassDetails");
                });

            modelBuilder.Entity("SchoolManagement.Entity.ClassEntity", b =>
                {
                    b.Property<string>("ClassId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AcademicYear")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<string>("HomeroomTeacherId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ClassId");

                    b.HasIndex("HomeroomTeacherId")
                        .IsUnique();

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("SchoolManagement.Entity.ConductEntity", b =>
                {
                    b.Property<Guid>("ConductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ConductName")
                        .HasColumnType("int");

                    b.Property<string>("Feedback")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SemesterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ConductId");

                    b.HasIndex("SemesterId");

                    b.HasIndex("StudentId");

                    b.ToTable("Conducts");
                });

            modelBuilder.Entity("SchoolManagement.Entity.DepartmentEntity", b =>
                {
                    b.Property<string>("DepartmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("SchoolManagement.Entity.SemesterEntity", b =>
                {
                    b.Property<string>("SemesterId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AcademicYear")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SemesterName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TimeEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeStart")
                        .HasColumnType("datetime2");

                    b.HasKey("SemesterId");

                    b.ToTable("Semesters");
                });

            modelBuilder.Entity("SchoolManagement.Entity.StudentEntity", b =>
                {
                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AcademicYear")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ethnic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherJob")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatherPhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MotherEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MotherJob")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MotherName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MotherPhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("StudentId");

                    b.HasIndex("AccountId")
                        .IsUnique()
                        .HasFilter("[AccountId] IS NOT NULL");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("SchoolManagement.Entity.SubjectEntity", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubjectId"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<string>("SubjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubjectId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("SchoolManagement.Entity.TeacherEntity", b =>
                {
                    b.Property<string>("TeacherId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClassId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ethnic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TimeEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeStart")
                        .HasColumnType("datetime2");

                    b.HasKey("TeacherId");

                    b.HasIndex("AccountId")
                        .IsUnique()
                        .HasFilter("[AccountId] IS NOT NULL");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("SchoolManagement.Entity.AssessmentEntity", b =>
                {
                    b.HasOne("SchoolManagement.Entity.ClassDetailEntity", "ClassDetail")
                        .WithMany("Assessments")
                        .HasForeignKey("ClassDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManagement.Entity.SemesterEntity", "Semester")
                        .WithMany("Assessments")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManagement.Entity.SubjectEntity", "Subject")
                        .WithMany("Assessments")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClassDetail");

                    b.Navigation("Semester");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("SchoolManagement.Entity.AssignmentEntity", b =>
                {
                    b.HasOne("SchoolManagement.Entity.ClassEntity", "Class")
                        .WithMany("Assignments")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManagement.Entity.SemesterEntity", "Semester")
                        .WithMany("Assignments")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManagement.Entity.SubjectEntity", "Subject")
                        .WithMany("Assignments")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManagement.Entity.TeacherEntity", "Teacher")
                        .WithMany("Assignments")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Semester");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("SchoolManagement.Entity.ClassDetailEntity", b =>
                {
                    b.HasOne("SchoolManagement.Entity.ClassEntity", "Class")
                        .WithMany("ClassDetails")
                        .HasForeignKey("ClassId");

                    b.HasOne("SchoolManagement.Entity.StudentEntity", "Student")
                        .WithMany("ClassDetails")
                        .HasForeignKey("StudentId");

                    b.Navigation("Class");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("SchoolManagement.Entity.ClassEntity", b =>
                {
                    b.HasOne("SchoolManagement.Entity.TeacherEntity", "HomeroomTeacher")
                        .WithOne("Class")
                        .HasForeignKey("SchoolManagement.Entity.ClassEntity", "HomeroomTeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HomeroomTeacher");
                });

            modelBuilder.Entity("SchoolManagement.Entity.ConductEntity", b =>
                {
                    b.HasOne("SchoolManagement.Entity.SemesterEntity", "Semester")
                        .WithMany("Conducts")
                        .HasForeignKey("SemesterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManagement.Entity.StudentEntity", "Student")
                        .WithMany("Conducts")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Semester");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("SchoolManagement.Entity.StudentEntity", b =>
                {
                    b.HasOne("SchoolManagement.Entity.AccountEntity", "Account")
                        .WithOne("Student")
                        .HasForeignKey("SchoolManagement.Entity.StudentEntity", "AccountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("SchoolManagement.Entity.TeacherEntity", b =>
                {
                    b.HasOne("SchoolManagement.Entity.AccountEntity", "Account")
                        .WithOne("Teacher")
                        .HasForeignKey("SchoolManagement.Entity.TeacherEntity", "AccountId");

                    b.HasOne("SchoolManagement.Entity.DepartmentEntity", "Department")
                        .WithMany("Teachers")
                        .HasForeignKey("DepartmentId");

                    b.Navigation("Account");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("SchoolManagement.Entity.AccountEntity", b =>
                {
                    b.Navigation("Student")
                        .IsRequired();

                    b.Navigation("Teacher")
                        .IsRequired();
                });

            modelBuilder.Entity("SchoolManagement.Entity.ClassDetailEntity", b =>
                {
                    b.Navigation("Assessments");
                });

            modelBuilder.Entity("SchoolManagement.Entity.ClassEntity", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("ClassDetails");
                });

            modelBuilder.Entity("SchoolManagement.Entity.DepartmentEntity", b =>
                {
                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("SchoolManagement.Entity.SemesterEntity", b =>
                {
                    b.Navigation("Assessments");

                    b.Navigation("Assignments");

                    b.Navigation("Conducts");
                });

            modelBuilder.Entity("SchoolManagement.Entity.StudentEntity", b =>
                {
                    b.Navigation("ClassDetails");

                    b.Navigation("Conducts");
                });

            modelBuilder.Entity("SchoolManagement.Entity.SubjectEntity", b =>
                {
                    b.Navigation("Assessments");

                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("SchoolManagement.Entity.TeacherEntity", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("Class")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
