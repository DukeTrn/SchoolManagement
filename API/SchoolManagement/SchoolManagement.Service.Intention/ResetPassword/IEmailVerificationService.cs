namespace SchoolManagement.Service.Intention.ResetPassword
{
    public interface IEmailVerificationService
    {
        Task<bool> SendVerificationEmailAsync(string email, string verificationCode);
        Task<bool> VerifyCodeAsync(string email, string verificationCode);
    }
}
