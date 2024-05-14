using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Database;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention.Authentication;

namespace SchoolManagement.Web.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SchoolManagementDbContext _context;

        public AuthController(ITokenService tokenService, SchoolManagementDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Validate the user credentials (this is just a mock, replace with real validation)
            var user = _context.AccountEntities.SingleOrDefault(p => p.UserName == model.Username && 
                p.Password == model.Password);
            if (user != null)
            {
                var token = _tokenService.GenerateToken("admin");
                return Ok(new {
                    result = true,
                    message = "Authentication success",
                    Token = token });
            }

            return Unauthorized();
        }
    }
}
