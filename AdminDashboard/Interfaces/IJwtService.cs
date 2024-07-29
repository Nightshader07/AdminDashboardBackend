namespace AdminDashboard.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(string userId);
}