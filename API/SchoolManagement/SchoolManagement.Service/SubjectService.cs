using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Service
{
    public class SubjectService : ISubjectService
    {
        private readonly ILogger<SubjectEntity> _logger;
        private readonly SchoolManagementDbContext _context;

        public SubjectService(ILogger<SubjectEntity> logger, SchoolManagementDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async ValueTask<List<SubjectDisplayModel>> GetSubjectsByGrade(int grade)
        {
            try
            {
                _logger.LogInformation("Start to get list of all subjects by grade.");
                var subjects = await _context.SubjectEntities
                                     .Where(s => s.Grade == grade)
                                     .ToListAsync();

                var displayModels = subjects.Select(s => new SubjectDisplayModel
                {
                    Id = s.SubjectId,
                    SubjectName = s.SubjectName,
                    Grade = s.Grade,
                    Description = s.Description
                }).ToList();

                _logger.LogInformation("Success to get list of all subjects by grade.");
                return displayModels;

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting list of all subjects by grade. Error: {ex}", ex);
                throw;
            }
        }


        public async ValueTask CreateSubject(SubjectAddModel model)
        {
            try
            {
                _logger.LogInformation("Start to create a new subject.");

                // Kiểm tra xem môn học đã tồn tại chưa
                var existingSubject = await _context.SubjectEntities
                                                    .FirstOrDefaultAsync(s => s.SubjectName == model.SubjectName && s.Grade == model.Grade);

                if (existingSubject != null)
                {
                    _logger.LogInformation("Subject already exists.");
                    throw ExistRecordException.ExistsRecord("Subject already exists");
                }

                // Tạo đối tượng môn học mới từ dữ liệu đầu vào
                var newSubject = new SubjectEntity
                {
                    SubjectName = model.SubjectName,
                    Grade = model.Grade,
                    Description = model.Description
                };

                // Thêm môn học mới vào cơ sở dữ liệu
                _context.SubjectEntities.Add(newSubject);
                await _context.SaveChangesAsync();

                _logger.LogInformation("New subject created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a new subject. Error: {Ex}", ex);
                throw;
            }
        }

        public async ValueTask UpdateSubject(int subjectId, SubjectUpdateModel model)
        {
            try
            {
                _logger.LogInformation("Start updating subject {SubjectName} - {Grade} with ID: {SubjectId}", model.SubjectName, model.Grade, subjectId);

                // Kiểm tra xem môn học có tồn tại không
                var subject = await _context.SubjectEntities.FindAsync(subjectId);
                if (subject == null)
                {
                    _logger.LogError("Subject with ID: {SubjectId} not found", subjectId);
                    throw new KeyNotFoundException($"Subject with ID: {subjectId} not found");
                }

                // Cập nhật thông tin môn học
                subject.SubjectName = model.SubjectName;
                subject.Grade = model.Grade;
                subject.Description = model.Description;

                _context.SubjectEntities.Update(subject);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated subject {SubjectName} - {Grade} with ID: {SubjectId}", model.SubjectName, model.Grade, subjectId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating subject with ID: {SubjectId}. Error: {ex}", subjectId, ex);
                throw;
            }
        }

        public async ValueTask DeleteSubject(int subjectId)
        {
            try
            {
                _logger.LogInformation("Start deleting subject with ID: {SubjectId}", subjectId);

                // Kiểm tra xem môn học có tồn tại không
                var subject = await _context.SubjectEntities.FindAsync(subjectId);
                if (subject == null)
                {
                    _logger.LogError("Subject with ID: {SubjectId} not found", subjectId);
                    throw new KeyNotFoundException($"Subject with ID: {subjectId} not found");
                }

                // Xóa môn học
                _context.SubjectEntities.Remove(subject);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted subject with ID: {SubjectId}", subjectId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting subject with ID: {SubjectId}. Error: {ex}", subjectId, ex);
                throw;
            }
        }

        // Check data?
    }
}
