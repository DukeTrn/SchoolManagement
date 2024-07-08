using Microsoft.EntityFrameworkCore;
using SchoolManagement.Entity;

namespace SchoolManagement.Database
{
    public class SchoolManagementDbContext : DbContext
    {
        public SchoolManagementDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AccountEntity> AccountEntities { get; set; } = null!;
        public DbSet<StudentEntity> StudentEntities { get; set; } = null!;
        public DbSet<TeacherEntity> TeacherEntities { get; set; } = null!;
        public DbSet<DepartmentEntity> DepartmentEntities { get; set; } = null!;
        public DbSet<ClassEntity> ClassEntities { get; set; } = null!;
        public DbSet<ClassDetailEntity> ClassDetailEntities { get; set; } = null!;
        public DbSet<ConductEntity> ConductEntities { get; set; } = null!;
        public DbSet<SemesterEntity> SemesterEntities { get; set; } = null!;
        public DbSet<AssessmentEntity> AssessmentEntities { get; set; } = null!;
        public DbSet<SubjectEntity> SubjectEntities { get; set; } = null!;
        public DbSet<AssignmentEntity> AssignmentEntities { get; set; } = null!;
        public DbSet<SemesterDetailEntity> SemesterDetailEntities { get; set; } = null!;
        public DbSet<TimetableEntity> TimetableEntities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API
            modelBuilder.Entity<AccountEntity>()
                .HasKey(a => a.AccountId);

            // Mối quan hệ 1-1 giữa User và Student
            modelBuilder.Entity<StudentEntity>()
                .HasOne(s => s.Account)
                .WithOne(u => u.Student)
                .HasForeignKey<StudentEntity>(s => s.AccountId);

            // Mối quan hệ 1-1 giữa User và Teacher
            modelBuilder.Entity<TeacherEntity>()
                .HasOne(t => t.Account)
                .WithOne(u => u.Teacher)
                .HasForeignKey<TeacherEntity>(t => t.AccountId);

            // Mối quan hệ 1-N giữa Department và Teacher
            modelBuilder.Entity<DepartmentEntity>()
                .HasMany(d => d.Teachers) // Mỗi Department có nhiều Teachers
                .WithOne(t => t.Department) // Mỗi Teacher thuộc về một Department
                .HasForeignKey(t => t.DepartmentId); // Khóa ngoại trong bảng TeacherEntity là DepartmentId

            // Mối quan hệ N-1 giữa Class và Teacher
            modelBuilder.Entity<ClassEntity>()
                .HasOne(c => c.HomeroomTeacher) // Mỗi lớp có một giáo viên chủ nhiệm
                .WithMany(t => t.Classes) // Một giáo viên có thể làm chủ nhiệm của nhiều lớp
                .HasForeignKey(c => c.HomeroomTeacherId); // Khóa ngoại trong bảng ClassEntity là HomeroomTeacherId

            // Mối quan hệ N-1 giữa ClassDetail và Student
            modelBuilder.Entity<ClassDetailEntity>()
                .HasOne(cd => cd.Student)
                .WithMany(s => s.ClassDetails)
                .HasForeignKey(cd => cd.StudentId);

            // Mối quan hệ N-1 giữa ClassDetail và Class
            modelBuilder.Entity<ClassDetailEntity>()
                .HasOne(cd => cd.Class)
                .WithMany(c => c.ClassDetails)
                .HasForeignKey(cd => cd.ClassId);

            // Mối quan hệ N-1 giữa Conduct và Student
            modelBuilder.Entity<ConductEntity>()
                .HasOne(c => c.Student)
                .WithMany(s => s.Conducts)
                .HasForeignKey(c => c.StudentId);

            // Mối quan hệ N-1 giữa Conduct và Semester
            modelBuilder.Entity<ConductEntity>()
                .HasOne(c => c.Semester)
                .WithMany(s => s.Conducts)
                .HasForeignKey(c => c.SemesterId);

            // Mối quan hệ N-1 giữa Assessment và ClassDetail
            modelBuilder.Entity<AssessmentEntity>()
                .HasOne(a => a.ClassDetail)
                .WithMany(cd => cd.Assessments)
                .HasForeignKey(a => a.ClassDetailId);

            // Mối quan hệ N-1 giữa Assessment và Subject
            modelBuilder.Entity<AssessmentEntity>()
                .HasOne(a => a.Subject)
                .WithMany(s => s.Assessments)
                .HasForeignKey(a => a.SubjectId);

            // Mối quan hệ N-1 giữa Assessment và Semester
            modelBuilder.Entity<AssessmentEntity>()
                .HasOne(a => a.Semester)
                .WithMany(s => s.Assessments)
                .HasForeignKey(a => a.SemesterId);

            // Mối quan hệ N-1 giữa Assignment và Class
            modelBuilder.Entity<AssignmentEntity>()
                .HasOne(a => a.Class)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.ClassId);

            // Mối quan hệ N-1 giữa Assignment và Teacher
            modelBuilder.Entity<AssignmentEntity>()
                .HasOne(a => a.Teacher)
                .WithMany(t => t.Assignments)
                .HasForeignKey(a => a.TeacherId);

            // Mối quan hệ N-1 giữa Assignment và Subject
            modelBuilder.Entity<AssignmentEntity>()
                .HasOne(a => a.Subject)
                .WithMany(s => s.Assignments)
                .HasForeignKey(a => a.SubjectId);

            // Mối quan hệ N-1 giữa Assignment và Semester
            modelBuilder.Entity<AssignmentEntity>()
                .HasOne(a => a.Semester)
                .WithMany(s => s.Assignments)
                .HasForeignKey(a => a.SemesterId);
            // Mối quan hệ N-1 giữa SemesterClass và Class
            modelBuilder.Entity<SemesterDetailEntity>()
                .HasOne(cd => cd.Class)
                .WithMany(s => s.SemClassIds)
                .HasForeignKey(cd => cd.ClassId);

            // Mối quan hệ N-1 giữa SemesterClass và Semester
            modelBuilder.Entity<SemesterDetailEntity>()
                .HasOne(cd => cd.Semester)
                .WithMany(c => c.SemClassIds)
                .HasForeignKey(cd => cd.SemesterId);

            // Timetable N-1 Assignment
            modelBuilder.Entity<TimetableEntity>()
                .HasOne(t => t.Assignment)
                .WithMany(a => a.Timetables)
                .HasForeignKey(t => t.AssignmentId);
        }
    }
}