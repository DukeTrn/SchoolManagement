using Microsoft.Extensions.Logging;
using SchoolManagement.Database;
using SchoolManagement.Entity;
using SchoolManagement.Service.Intention;

namespace SchoolManagement.Service
{
    public class ConductService : IConductService
    {
        private readonly ILogger<ConductService> _logger;
        private readonly SchoolManagementDbContext _context;
        
        public ConductService(ILogger<ConductService> _logger,
            SchoolManagementDbContext _context)
        {
            this._logger = _logger;
            this._context = _context;
        }

        
    }
}
