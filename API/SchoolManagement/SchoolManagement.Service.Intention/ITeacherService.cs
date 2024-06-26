﻿using SchoolManagement.Common.Enum;
using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface ITeacherService
    {
        ValueTask<PaginationModel<TeacherDisplayModel>> GetAllTeachers(TeacherQueryModel queryModel);
        ValueTask<PaginationModel<TeacherDisplayModel>> GetAllTeachers(string departmentId, TeacherQueryModel queryModel);
        ValueTask<PaginationModel<TeacherDisplayModel>> GetAllTeachersInDeptByAccountId(Guid accountId, TeacherQueryModel queryModel);
        ValueTask<IEnumerable<TeacherFilterModel>> GetAllTeachersFilter();
        ValueTask<IEnumerable<TeacherHeadsModel>> GetDepartmentHeadsAndDeputies(string departmentId);
        ValueTask<TeacherFullDisplayModel> GetTeacherById(string id);
        ValueTask<TeacherFullDisplayModel> GetTeacherByAccountId(Guid id);
        ValueTask CreateTeacher(TeacherAddModel model);
        ValueTask UpdateTeacher(string id, TeacherUpdateModel model);
        ValueTask DeleteTeacher(string id);
        Task<byte[]> ExportToExcelAsync(TeacherExportQueryModel queryModel);
        ValueTask UpdateTeacherRoll(string teacherId, RoleType newRole);
    }
}
