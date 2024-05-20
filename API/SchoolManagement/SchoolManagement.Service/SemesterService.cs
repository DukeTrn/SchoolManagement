using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Database;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;
using SchoolManagement.Share;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Entity;
using SchoolManagement.Model.Semester;

namespace SchoolManagement.Service
{
    public class SemesterService : ISemesterService
    {
        private readonly ILogger<SemesterService> _logger;
        private readonly SchoolManagementDbContext _context;

        public SemesterService(ILogger<SemesterService> logger, SchoolManagementDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Get list of all semesters
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        #region List semesters
        public async ValueTask<PaginationModel<SemesterDisplayModel>> GetAllSemesters(PageModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list semesters.");
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                var query = _context.SemesterEntities.AsQueryable();
                query = query.OrderByDescending(s => s.SemesterId);

                var paginatedData = await query
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                return new PaginationModel<SemesterDisplayModel>
                {
                    TotalCount = query.Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    DataList = from item in paginatedData
                               select item.ToModel()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting list of all semesters. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Add new semester
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask CreateSemester(SemesterAddModel model)
        {
            try
            {
                _logger.LogInformation("Start to create new semester.");
                var existSem = await _context.SemesterEntities.FirstOrDefaultAsync(s => s.SemesterId == model.SemesterId);
                if (existSem != null)
                {
                    _logger.LogInformation("Semester has already existed");
                    throw ExistRecordException.ExistsRecord("Semester ID already exists");
                }

                var newSem = new SemesterEntity()
                {
                    SemesterId = model.SemesterId,
                    SemesterName = model.SemesterName,
                    AcademicYear = DateTime.Now.Year.ToString() + " - " + DateTime.Now.AddYears(1).Year.ToString(),
                    TimeStart = model.TimeStart,
                    TimeEnd = model.TimeEnd,
                };

                _context.SemesterEntities.Add(newSem);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating new Semester. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update a semester by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask UpdateSemester(string id, SemesterUpdateModel model)
        {
            try
            {
                _logger.LogInformation("Start to update a semester.");
                var sem = await _context.SemesterEntities.FirstOrDefaultAsync(s => s.SemesterId == id);
                if (sem == null)
                {
                    throw new NotFoundException($"Semester with ID {id} not found.");
                }

                sem.SemesterName = model.SemesterName;
                sem.TimeStart = model.TimeStart;
                sem.TimeEnd = model.TimeEnd;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while updating a Semester. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete semester by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask DeleteSemester(string id)
        {
            try
            {
                _logger.LogInformation("Start deleting Semester with ID {id}", id);

                var sem = await _context.SemesterEntities.FirstOrDefaultAsync(s => s.SemesterId == id);

                if (sem == null)
                {
                    throw new NotFoundException($"Semester with ID {id} not found.");
                }

                _context.SemesterEntities.Remove(sem);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Semester with ID {id} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting Semester with ID {id}. Error: {ex}", id, ex);
                throw;
            }
        }
        #endregion
    }
}
