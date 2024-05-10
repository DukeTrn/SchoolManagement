﻿using SchoolManagement.Entity;
using SchoolManagement.Model;
using System.Data;

namespace SchoolManagement.Service.Intention
{
    public interface IStudentService
    {
        ValueTask<PaginationModel<StudentDisplayModel>> GetAllStudents(StudentQueryModel queryModel);
        ValueTask<StudentFullDetailModel> GetStudentById(string studentId);
        ValueTask CreateStudent(StudentAddModel model);
        ValueTask UpdateStudent(string id, StudentUpdateModel model);
        ValueTask DeleteStudent(string id);
        //ValueTask<string> ExportStudents(ExportQueryModel queryModel);
        DataTable ExportStudentsTest();
    }
}
