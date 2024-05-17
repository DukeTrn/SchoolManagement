using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using SchoolManagement.Service.Intention.ResetPassword;
using SchoolManagement.Model;

namespace SchoolManagement.Service.ResetPassword
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailConfiguration _emailConfig;

        public EmailService(ILogger<EmailService> logger,
            EmailConfiguration emailConfig)
        {
            _logger = logger;
            _emailConfig = emailConfig;
        }

        //public async Task SendEmailAsync(string email, string subject, string message)
        //{
        //    try
        //    {
        //        // Tạo một message email
        //        var emailMessage = new MimeMessage();
        //        emailMessage.From.Add(new MailboxAddress("Your Name", "your-email@example.com"));
        //        emailMessage.To.Add(new MailboxAddress("", email)); // Địa chỉ email của người nhận
        //        emailMessage.Subject = subject; // Tiêu đề email

        //        // Tạo phần nội dung email
        //        var bodyBuilder = new BodyBuilder();
        //        bodyBuilder.HtmlBody = message; // Nội dung email dạng HTML

        //        emailMessage.Body = bodyBuilder.ToMessageBody();

        //        // Thiết lập thông tin SMTP client
        //        using (var client = new SmtpClient())
        //        {
        //            await client.ConnectAsync("smtp.example.com", 587, false); // SMTP server và cổng
        //            await client.AuthenticateAsync("ducdevil2001@gmail.com", "bothsavage2001"); // Tài khoản email và mật khẩu

        //            // Gửi email
        //            await client.SendAsync(emailMessage);

        //            // Đóng kết nối
        //            await client.DisconnectAsync(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý ngoại lệ khi gửi email không thành công
        //        _logger.LogError("Error sending email: {ex}", ex);
        //        throw;
        //    }
        //}

        public void SendEmail(Message message)
        {
            var data = CreateEmailMessage(message);
            Send(data);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("School Management System", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending email: {ex}", ex.Message);
                throw;
            }
            finally
            {
                client.Disconnect(true);
                //client.Dispose();
            }
        }
    }
}
