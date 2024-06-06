using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Common.Enum;
using SchoolManagement.Common.Exceptions;
using SchoolManagement.Model;
using SchoolManagement.Model.Account;
using SchoolManagement.Service.Intention;
using SchoolManagement.Service.Intention.ResetPassword;

namespace SchoolManagement.Web.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IEmailVerificationService _emailVerificationService;

        public AccountController(IAccountService accountService,
            IEmailVerificationService emailVerificationService)
        {
            _accountService = accountService;
            _emailVerificationService = emailVerificationService;
        }

        /// <summary>
        /// Roles: 1 (admin), 2 (GVCN), 3 (GV), 4 (HS)
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost, Route("all")]
        //[Authorize(Roles = nameof(RoleType.Admin))]
        public async ValueTask<IActionResult> GetAllAccounts([FromBody] AccountQueryModel queryModel)
        {
            try
            {
                var result = await _accountService.GetAllAccounts(queryModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Role type: 1 (admin), 2 (GVCN), 3 (GV), 4 (HS)
        /// </summary>
        [HttpPost, Route("create")]
        public async ValueTask<IActionResult> CreateAccount([FromBody] AccountAddModel queryModel)
        {
            try
            {
                await _accountService.CreateAccount(queryModel);
                return Ok(new { result = true, messageType = MessageType.Information });
            }
            catch (ExistRecordException)
            {
                return Ok(new { result = false, messageType = MessageType.Duplicated, message = "ID này đã tồn tại" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = false, messageType = MessageType.Error, message = ex });
            }
        }

        /// <summary>
        /// Use for testing (delete student/teacher first, then delete account)
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpDelete("{accountId}")]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            var result = await _accountService.DeleteAccountAsync(accountId);
            if (!result)
            {
                return NotFound(new { Message = "Không tìm thấy tài khoản này!" });
            }

            return Ok(new { result = true, message = "Xóa tài khoản thành công" });
        }

        #region Change password
        [HttpPut("{accountId}/changepassword")]
        public async Task<IActionResult> ChangePassword(Guid accountId, [FromBody] ChangePasswordModel model)
        {
            try
            {
                await _accountService.ChangePasswordAsync(accountId, model);
                return Ok(new { result = true, message = "Đổi mật khẩu thành công"});
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log lỗi không xác định
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing your request. {ex.Message}");
            }
        }

        [HttpPut("{accountId}/status/{isActive}")]
        public async Task<IActionResult> UpdateAccountStatus(Guid accountId, bool isActive)
        {
            var result = await _accountService.UpdateStatusAsync(accountId, isActive);
            if (!result)
            {
                return NotFound(new { Result = false, 
                    MessageType = MessageType.Error,
                    Message = "Không tìm thấy tài khoản." 
                });
            }

            return Ok(new {
                Result = true,
                MessageType = MessageType.Information,
                Message = "Cập nhật trạng thái tài khoản thành công!" });
        }
        #endregion

        #region Forget password (not completed)
        //[HttpPost("send-code")]
        //public async Task<IActionResult> SendVerificationCode(string email)
        //{
        //    // Gửi mã xác nhận qua email
        //    string verificationCode = GenerateRandomCode();
        //    bool sentSuccessfully = await _emailVerificationService.SendVerificationEmailAsync(email, verificationCode);

        //    if (sentSuccessfully)
        //        return Ok(new { Message = "Verification code sent successfully" });
        //    else
        //        return BadRequest(new { Message = "Failed to send verification code" });
        //}

        //[HttpPost("verify-code")]
        //public async Task<IActionResult> VerifyCode(string email, string verificationCode)
        //{
        //    // Xác minh mã xác nhận từ người dùng
        //    bool isCodeValid = await _emailVerificationService.VerifyCodeAsync(email, verificationCode);

        //    if (isCodeValid)
        //        return Ok(new { Message = "Verification code is valid" });
        //    else
        //        return BadRequest(new { Message = "Invalid verification code" });
        //}

        //private string GenerateRandomCode()
        //{
        //    // Đây là nơi bạn triển khai logic để tạo mã xác nhận ngẫu nhiên
        //    // Trong ví dụ này, chúng tôi giả định mã có 6 chữ số
        //    Random random = new Random();
        //    return random.Next(100000, 999999).ToString();
        //}
        #endregion

    }
}
