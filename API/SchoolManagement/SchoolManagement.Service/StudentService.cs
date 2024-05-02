using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Common.Enum;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Model;
using SchoolManagement.Model.Student;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Data;
using SchoolManagement.Share;

namespace SchoolManagement.Service
{
    public class StudentService : IStudentService
    {
        private readonly ILogger<StudentService> _logger;
        private readonly SchoolManagementDbContext _context;
        private readonly IEntityFilterService<StudentEntity> _filterBuilder;

        public StudentService(ILogger<StudentService> logger, 
            IEntityFilterService<StudentEntity> filterBuilder,
            SchoolManagementDbContext context)
        {
            _logger = logger;
            _filterBuilder = filterBuilder;
            _context = context;
        }

        /// <summary>
        /// Get list of all students
        /// </summary>
        /// <param name="queryModel"></param>
        public async ValueTask<PaginationModel<StudentModel>> GetAllStudents(StudentQueryModel queryModel)
        {
            _logger.LogInformation("Start to get list students.");
            int pageNumber = queryModel.PageNumber != null && queryModel.PageNumber.Value > 0 ? queryModel.PageNumber.Value : 1;
            int pageSize = queryModel.PageSize != null && queryModel.PageSize.Value > 0 ? queryModel.PageSize.Value : 10;
            FilterModel filter = new();
            var query = _context.StudentEntities.FromSqlRaw("SELECT * FROM dbo.Students").ToList();


            var paginatedData = query
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            return new PaginationModel<StudentModel>
            {
                TotalCount = query.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                //TotalPageCount = (int)Math.Ceiling((double)TotalCount / pageSize),
                DataList = from item in paginatedData
                           select item.ToModel()
            };
        }

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
