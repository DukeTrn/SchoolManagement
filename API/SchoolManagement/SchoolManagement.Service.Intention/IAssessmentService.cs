using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IAssessmentService
    {
        ValueTask<PaginationModel<AssessmentDetailModel>> GetListStudentsInAssessment(int grade, string semesterId, string classId, PageQueryModel queryModel);
        ValueTask<IEnumerable<AssessmentScoreDisplayModel>> GetListSubjectAndScore(int grade, string semesterId, string classDetailId);
        ValueTask<AverageScoreModel> GetAverageScoresForSemester(int grade, string semesterId, string classDetailId);
        ValueTask<AverageScoreForAcademicYearModel> GetAverageScoreForAcademicYear(int grade, string classDetailId, string academicYear);
        ValueTask CreateAssessments(List<AssessmentAddModel> models);
        ValueTask UpdateAssessment(Guid id, AssessmentUpdateModel model);
        ValueTask DeleteAssessment(Guid id);
    }
}
