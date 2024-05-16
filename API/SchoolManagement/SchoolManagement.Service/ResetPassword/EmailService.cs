using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using SchoolManagement.Service.Intention.ResetPassword;

namespace SchoolManagement.Service.ResetPassword
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                // Tạo một message email
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Your Name", "your-email@example.com"));
                emailMessage.To.Add(new MailboxAddress("", email)); // Địa chỉ email của người nhận
                emailMessage.Subject = subject; // Tiêu đề email

                // Tạo phần nội dung email
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = message; // Nội dung email dạng HTML

                emailMessage.Body = bodyBuilder.ToMessageBody();

                // Thiết lập thông tin SMTP client
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.example.com", 587, false); // SMTP server và cổng
                    await client.AuthenticateAsync("ducdevil2001@gmail.com", "bothsavage2001"); // Tài khoản email và mật khẩu

                    // Gửi email
                    await client.SendAsync(emailMessage);

                    // Đóng kết nối
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ khi gửi email không thành công
                _logger.LogError("Error sending email: {ex}", ex);
                throw;
            }
        }
    }
}
