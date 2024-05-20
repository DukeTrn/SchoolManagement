using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.Department;
using SchoolManagement.Service.Intention;
using SchoolManagement.Share;

namespace SchoolManagement.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentEntity> _logger;
        private readonly SchoolManagementDbContext _context;

        public DepartmentService(ILogger<DepartmentEntity> logger,
            SchoolManagementDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        #region List departments
        /// <summary>
        /// Get list of all departments
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async ValueTask<PaginationModel<DepartmentDisplayModel>> GetAllDepartments(PageModel queryModel)
        {
            try
            {
                _logger.LogInformation("Start to get list departments.");
                int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
                int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;

                var query = _context.DepartmentEntities.AsQueryable();

                var paginatedData = await query
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                return new PaginationModel<DepartmentDisplayModel>
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
                _logger.LogError("An error occured while getting list of all departments. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Add new department
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask CreateDepartment(DepartmentAddModel model)
        {
            try
            {
                _logger.LogInformation("Start to create new department.");
                var existingDept = await _context.DepartmentEntities.FirstOrDefaultAsync(s => s.DepartmentId == model.DepartmentId);
                if (existingDept != null)
                {
                    _logger.LogInformation("Department has already existed");
                    throw ExistRecordException.ExistsRecord("Department ID already exists");
                }

                var newDept = new DepartmentEntity()
                {
                    DepartmentId = model.DepartmentId,
                    SubjectName = model.SubjectName,
                    Description = model.Description,
                    Notification = model.Notification,
                };

                _context.DepartmentEntities.Add(newDept);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating new department. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update a department by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async ValueTask UpdateDepartment(string id, DepartmentUpdateModel model)
        {
            try
            {
                _logger.LogInformation("Start to update a department.");
                var dept = await _context.DepartmentEntities.FirstOrDefaultAsync(s => s.DepartmentId == id);
                if (dept == null)
                {
                    throw new NotFoundException($"Department with ID {id} not found.");
                }

                dept.SubjectName = model.SubjectName;
                dept.Description = model.Description;
                dept.Notification = model.Notification;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while updating a department. Error: {ex}", ex);
                throw;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete dept by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async ValueTask DeleteDepartment(string id)
        {
            try
            {
                _logger.LogInformation("Start deleting department with ID {id}", id);

                var dept = await _context.DepartmentEntities.FirstOrDefaultAsync(s => s.DepartmentId == id);

                if (dept == null)
                {
                    throw new NotFoundException($"Student with ID {id} not found.");
                }

                _context.DepartmentEntities.Remove(dept);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Department with ID {id} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while deleting Department with ID {id}. Error: {ex}", id, ex);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Search function
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static List<FilterOperatorItemModel> BuildSearchFilter(string searchKey, params string[] properties)
        {
            List<FilterOperatorItemModel> filters = new();
            foreach (var prop in properties)
            {
                filters.Add(new FilterOperatorItemModel()
                {
                    KeyName = prop,
                    Values = new List<string> { searchKey },
                    KeyType = typeof(string),
                    Operator = FilterOperator.Contains
                });
            }
            return filters;
        }
    }
}
