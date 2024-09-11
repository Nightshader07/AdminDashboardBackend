using AdminDashboard.models;
using Task = System.Threading.Tasks.Task;

namespace AdminDashboard.Interfaces;

public interface IRefreshTokenService
{
    Task<RefreshToken> GenerateRefreshTokenAsync(string userId);
    RefreshToken GetRefreshToken(string token);
    Task<bool> RevokeRefreshTokenAsync(string token);
    
}