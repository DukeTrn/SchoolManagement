using Microsoft.Extensions.Logging;
using SchoolManagement.Database;
using SchoolManagement.Service.Intention.ResetPassword;

namespace SchoolManagement.Service.ResetPassword
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IEmailService _emailService;
        private readonly SchoolManagementDbContext _context;
        private readonly ILogger<EmailVerificationService> _logger; 

        public EmailVerificationService(IEmailService emailService,
            SchoolManagementDbContext context,
            ILogger<EmailVerificationService> logger)
        {
            _emailService = emailService;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> SendVerificationEmailAsync(string email, string verificationCode)
        {
            try
            {
                string subject = "Password Reset Verification";
                string body = $"Your verification code is: {verificationCode}";

                await _emailService.SendEmailAsync(email, subject, body);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending verification email: {ex}", ex);
                return false;
            }
        }

        public async Task<bool> VerifyCodeAsync(string email, string verificationCode)
        {
            // Đây là nơi bạn triển khai logic xác minh mã xác thực từ người dùng
            // Trong ví dụ này, chúng tôi giả định rằng mã luôn hợp lệ
            return await Task.FromResult(true);
            // Tìm thông tin người dùng dựa trên địa chỉ email
            //var user = await _context.AccountEntities.FirstOrDefaultAsync(u => u.Email == email);

            //// Kiểm tra xem có người dùng nào với địa chỉ email đã cho hay không
            //if (user == null)
            //    return false;

            //// Lấy mã xác nhận gần nhất của người dùng
            //var latestVerification = await _context.VerificationCodes
            //                                        .Where(vc => vc.UserId == user.Id)
            //                                        .OrderByDescending(vc => vc.CreatedAt)
            //                                        .FirstOrDefaultAsync();

            //// Kiểm tra xem mã xác nhận đã hết hạn hay chưa
            //if (latestVerification == null || DateTime.UtcNow > latestVerification.ExpiresAt)
            //    return false;

            //// Kiểm tra xem mã xác nhận từ người dùng có khớp với mã gửi đến hay không
            //if (latestVerification.Code != verificationCode)
            //    return false;

            //// Xác nhận mã xác thực thành công, bạn có thể thực hiện các hành động tiếp theo ở đây
            //return true;
        }
    }
}
