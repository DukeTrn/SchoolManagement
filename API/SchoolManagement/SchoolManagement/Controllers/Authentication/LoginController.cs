using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Database;
using SchoolManagement.Model;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.Authentication;

namespace SchoolManagement.Web.Controllers.Authentication
{
    [ApiController, Route("api/user")]
    public class LoginController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SchoolManagementDbContext _context;
        private readonly IAccountService _accountService;

        public LoginController(ITokenService tokenService, 
            SchoolManagementDbContext context,
            IAccountService accountService)
        {
            _tokenService = tokenService;
            _context = context;
            _accountService = accountService;
        }
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Validate the user credentials (this is just a mock, replace with real validation)
            try
            {
                var account = await _accountService.ValidateAccountAsync(model.Username, model.Password);
                var token = _tokenService.GenerateToken(account);
                return Ok(new
                {
                    result = true,
                    message = "Authentication success",
                    Token = token
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
                //return Unauthorized(new { message = "Authentication failed: " + ex.Message });
            }
            catch (SecurityTokenException ex)
            {
                // Bắt và xử lý ngoại lệ khi tạo token không hợp lệ
                return BadRequest(new
                {
                    result = false,
                    message = "Token generation failed. " + ex.Message
                });
            }
            catch (Exception ex)
            {
                // Log lỗi không xác định
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing your request. {ex.Message}");
            }
        }
    }
}
