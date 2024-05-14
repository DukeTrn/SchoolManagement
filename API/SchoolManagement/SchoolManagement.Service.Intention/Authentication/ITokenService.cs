namespace SchoolManagement.Service.Intention.Authentication
{
    public interface ITokenService
    {
        string GenerateToken(string userId);
    }
}
