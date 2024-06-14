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
            try
            {
                var account = await _accountService.ValidateAccountAsync(model.Username, model.Password);
                var token = _tokenService.GenerateToken(account);
                return Ok(new
                {
                    result = true,
                    message = "Authentication success",
                    token = token,
                    accoundId = account.AccountId,
                    role = account.Role.ToString()
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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
