using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface IAssignmentService
    {
        ValueTask<IEnumerable<AssignmentDisplayModel>> GetListAssignments(int grade, string semesterId, int subjectId, AssignmentQueryModel queryModel);
        ValueTask CreateAssignment(AssignmentAddModel model);
        ValueTask UpdateAssignment(Guid assignmentId, string classId);
        ValueTask DeleteAssignment(Guid assignmentId);
    }
}
