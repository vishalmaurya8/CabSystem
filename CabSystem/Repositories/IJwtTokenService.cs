namespace CabSystem.Repositories
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(string email, string role, int userId);
    }
}
