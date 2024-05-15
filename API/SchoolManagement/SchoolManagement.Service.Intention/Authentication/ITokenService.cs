using SchoolManagement.Entity;

namespace SchoolManagement.Service.Intention.Authentication
{
    public interface ITokenService
    {
        string GenerateToken(AccountEntity entity);
    }
}
