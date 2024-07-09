using SchoolManagement.Model;

namespace SchoolManagement.Service.Intention
{
    public interface ITimetableService
    {
        ValueTask CreateTimetableAsync(TimetableCreateModel model);
        ValueTask UpdateTimetableAsync(Guid timetableId, TimetableUpdateModel model);
        ValueTask DeleteTimetableAsync(Guid timetableId);
        ValueTask<TimetableDisplayModel> GetTimetableByIdAsync(Guid timetableId);
        ValueTask<List<TimetableDisplayModel>> GetTimetablesByClassIdAsync(string classId, string semesterId);
        ValueTask<List<TimetableDisplayModel>> GetTimetablesByTeacherAccountIdAsync(Guid accountId);

    }
}
