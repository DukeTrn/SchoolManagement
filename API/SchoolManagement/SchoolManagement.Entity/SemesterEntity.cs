﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Entity
{
    [Table("Semesters")]
    public class SemesterEntity
    {
        [Key]
        public string SemesterId { get; set; } = string.Empty;
        public string SemesterName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty; // nien khoa
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }

        // 1-N Semester-Conducts
        public ICollection<ConductEntity> Conducts { get; set; } = null!;

        // 1-N Semester-Assignments
        public ICollection<AssignmentEntity> Assignments { get; set; } = null!;

        // 1-N Semester-Assessments
        public ICollection<AssessmentEntity> Assessments { get; set; } = null!;
    }
}
