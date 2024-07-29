using AdminDashboard.models;
using Task = System.Threading.Tasks.Task;

namespace AdminDashboard.Interfaces;

public interface IRefreshTokenService
{
    Task<RefreshToken> GenerateRefreshTokenAsync(string userId);
    Task<RefreshToken> GetRefreshTokenAsync(string token);
    Task RevokeRefreshTokenAsync(string token);
}