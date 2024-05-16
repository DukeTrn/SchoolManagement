namespace SchoolManagement.Service.Intention.ResetPassword
{
    public interface IEmailService
    {
        //Task<bool> SendEmailAsync(string email, string subject, string body);
        Task SendEmailAsync(string email, string subject, string message);
    }
}
