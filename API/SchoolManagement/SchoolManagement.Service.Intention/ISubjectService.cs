using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface ISubjectService
    {
        ValueTask<List<SubjectDisplayModel>> GetSubjectsByGrade(int grade);
        ValueTask CreateSubject(SubjectAddModel model);
        ValueTask UpdateSubject(int subjectId, SubjectUpdateModel model);
        ValueTask DeleteSubject(int subjectId);
    }
}
