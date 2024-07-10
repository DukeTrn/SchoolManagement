using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;
using System.Xml;

namespace SchoolManagement.Service
{
    public class TimetableService : ITimetableService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly SchoolManagementDbContext _context;

        public TimetableService(ILogger<AccountService> logger, 
            SchoolManagementDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async ValueTask CreateTimetableAsync(TimetableCreateModel model)
        {
            try
            {
                _logger.LogInformation("Starting to create new timetable entry for classId in {semester}: {classId}", 
                    model.ClassId, model.SemesterId);

                var classId = await _context.ClassEntities.FindAsync(model.ClassId);
                if (classId == null)
                {
                    var errorMessage = $"Class with ID {model.ClassId} not found.";
                    _logger.LogWarning(errorMessage);
                    throw new NotFoundException(errorMessage);
                }
                var semester = await _context.SemesterEntities.FindAsync(model.SemesterId);
                if (semester == null)
                {
                    var errorMessage = $"Semester with ID {model.SemesterId} not found.";
                    _logger.LogWarning(errorMessage);
                    throw new NotFoundException(errorMessage);
                }
                var subject = await _context.SubjectEntities.FindAsync(model.SubjectId);
                if (subject == null)
                {
                    var errorMessage = $"Subject with ID {model.SubjectId} not found.";
                    _logger.LogWarning(errorMessage);
                    throw new NotFoundException(errorMessage);
                }
                var assignment = await _context.AssignmentEntities
                                .FirstOrDefaultAsync(a => a.ClassId == model.ClassId
                                        && a.SemesterId == model.SemesterId
                                        && a.SubjectId == model.SubjectId);

                // Kiểm tra trùng lặp thời gian của Timetable
                var conflictTimetable = await _context.TimetableEntities
                    .Where(t => t.Assignment.ClassId == model.ClassId 
                                && t.Assignment.SemesterId == model.SemesterId
                                && t.DayOfWeek == model.DayOfWeek
                                && t.Period == model.Period)
                    .FirstOrDefaultAsync();

                if (conflictTimetable != null)
                {
                    var errorMessage = "Thời khóa biểu bị trùng lịch!";
                    _logger.LogWarning(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                var newTimetable = new TimetableEntity
                {
                    TimetableId = Guid.NewGuid(),
                    AssignmentId = assignment.AssignmentId,
                    DayOfWeek = model.DayOfWeek,
                    Period = model.Period,
                };

                _context.TimetableEntities.Add(newTimetable);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully created new timetable entry for AssignmentId: {AssignmentId}", assignment.AssignmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating the timetable entry. Error: {Error}", ex.Message);
                throw;
            }
        }


        public async ValueTask UpdateTimetableAsync(Guid timetableId, TimetableUpdateModel model)
        {
            try
            {
                _logger.LogInformation($"Updating timetable with ID {timetableId}.");

                var existingTimetable = await _context.TimetableEntities
                    .Include(t => t.Assignment)
                    .FirstOrDefaultAsync(t => t.TimetableId == timetableId);
                if (existingTimetable == null)
                {
                    _logger.LogWarning($"Timetable with ID {timetableId} not found.");
                    throw new NotFoundException("Không tìm thấy thời khóa biểu này");
                }
                if (existingTimetable.Assignment == null)
                {
                    _logger.LogWarning($"Assignment for timetable with ID {timetableId} not found.");
                    throw new NotFoundException("Không tìm thấy phân công giảng dạy cho thời khóa biểu này");
                }

                // Kiểm tra trùng lặp thời gian
                var conflictTimetable = await _context.TimetableEntities
                    .Where(t => t.Assignment.SemesterId == existingTimetable.Assignment.SemesterId
                                && t.DayOfWeek == model.DayOfWeek
                                && t.Period == model.Period
                                && t.TimetableId != timetableId)
                    .FirstOrDefaultAsync();

                if (conflictTimetable != null)
                {
                    var errorMessage = "Thời khóa biểu bị trùng lịch.";
                    _logger.LogWarning(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                existingTimetable.DayOfWeek = model.DayOfWeek;
                existingTimetable.Period = model.Period;

                _context.TimetableEntities.Update(existingTimetable);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Timetable with ID {timetableId} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating timetable with ID {timetableId}. Error: {ex.Message}");
                throw;
            }
        }

        public async ValueTask DeleteTimetableAsync(Guid timetableId)
        {
            try
            {
                _logger.LogInformation($"Attempting to delete timetable with ID {timetableId}.");

                // Tìm kiếm timetable với ID tương ứng
                var timetable = await _context.TimetableEntities.FindAsync(timetableId);
                if (timetable == null)
                {
                    _logger.LogWarning($"Timetable with ID {timetableId} not found.");
                    throw new NotFoundException("Timetable not found.");
                }

                // Xóa timetable khỏi cơ sở dữ liệu
                _context.TimetableEntities.Remove(timetable);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Timetable with ID {timetableId} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting timetable with ID {timetableId}. Error: {ex.Message}");
                throw;
            }
        }


        public async ValueTask<TimetableDisplayModel> GetTimetableByIdAsync(Guid timetableId)
        {
            try
            {
                _logger.LogInformation($"Attempting to fetch timetable with ID {timetableId}.");

                var timetable = await _context.TimetableEntities
                    .Include(t => t.Assignment)
                        .ThenInclude(a => a.Class)
                    .Include(t => t.Assignment)
                        .ThenInclude(a => a.Subject)
                    .Include(t => t.Assignment)
                        .ThenInclude(a => a.Teacher)
                    .FirstOrDefaultAsync(t => t.TimetableId == timetableId);

                if (timetable == null)
                {
                    _logger.LogWarning($"Timetable with ID {timetableId} not found.");
                    throw new NotFoundException("Timetable not found.");
                }

                var timetableDisplayModel = new TimetableDisplayModel
                {
                    TimetableId = timetable.TimetableId,
                    AssignmentId = timetable.AssignmentId,
                    ClassName = timetable.Assignment.Class.ClassName,
                    SubjectName = timetable.Assignment.Subject.SubjectName,
                    TeacherName = timetable.Assignment.Teacher.FullName,
                    DayOfWeek = timetable.DayOfWeek,
                    Period = timetable.Period,
                };

                _logger.LogInformation($"Successfully fetched timetable with ID {timetableId}.");
                return timetableDisplayModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching timetable with ID {timetableId}. Error: {ex.Message}");
                throw;
            }
        }


        public async ValueTask<List<TimetableDisplayModel>> GetTimetablesByClassIdAsync(string classId, string semesterId)
        {
            try
            {
                var timetables = await _context.TimetableEntities
                    .Where(t => t.Assignment.ClassId == classId && t.Assignment.SemesterId == semesterId)
                    .Include(t => t.Assignment.Class)
                    .Include(t => t.Assignment.Subject)
                    .Include(t => t.Assignment.Teacher)
                    .OrderBy(t => t.DayOfWeek)
                    .ThenBy(t => t.Period)
                    .Select(t => new TimetableDisplayModel
                    {
                        TimetableId = t.TimetableId,
                        AssignmentId = t.AssignmentId,
                        ClassName = t.Assignment.Class.ClassName,
                        SubjectName = t.Assignment.Subject.SubjectName,
                        TeacherName = t.Assignment.Teacher.FullName,
                        DayOfWeek = t.DayOfWeek,
                        Period = t.Period,
                    }).ToListAsync();

                return timetables;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching timetables for ClassId {classId} and SemesterId {semesterId}. Error: {ex.Message}");
                throw;
            }
        }


        public async ValueTask<List<TimetableDisplayModel>> GetTimetablesByTeacherAccountIdAsync(Guid accountId)
        {
            try
            {
                var timetables = await _context.TimetableEntities
                    .Where(t => t.Assignment.Teacher.AccountId == accountId)
                    .Include(t => t.Assignment)
                        .ThenInclude(a => a.Class)
                    .Include(t => t.Assignment)
                        .ThenInclude(a => a.Subject)
                    .Include(t => t.Assignment)
                        .ThenInclude(a => a.Teacher)
                    .Select(t => new TimetableDisplayModel
                    {
                        TimetableId = t.TimetableId,
                        AssignmentId = t.AssignmentId,
                        ClassName = t.Assignment.Class.ClassName,
                        SubjectName = t.Assignment.Subject.SubjectName,
                        TeacherName = t.Assignment.Teacher.FullName,
                        DayOfWeek = t.DayOfWeek,
                        Period = t.Period
                    })
                    .OrderBy(t => t.DayOfWeek)
                    .ThenBy(t => t.Period)
                    .ToListAsync();

                return timetables;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving timetables for teacher. Error: {Error}", ex.Message);
                throw;
            }
        }

    }
}
