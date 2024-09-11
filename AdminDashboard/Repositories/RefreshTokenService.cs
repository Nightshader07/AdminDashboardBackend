using System.Data.Entity;
using System.Security.Cryptography;
using AdminDashboard.Data;
using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Task = System.Threading.Tasks.Task;

namespace AdminDashboard.Repositories;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = userId,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            CreatedDate = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public RefreshToken GetRefreshToken(string token)
    {
        return _context.RefreshTokens.SingleOrDefault(rt => rt.Token == token && !rt.IsRevoked);
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == token);
        if (refreshToken != null)
        {
            refreshToken.IsRevoked = true;
            refreshToken.RevokedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}