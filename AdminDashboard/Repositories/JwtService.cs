using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdminDashboard.DTOs;
using AdminDashboard.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<JwtService> _logger;

    public JwtService(IOptions<JwtSettings> jwtSettings, ILogger<JwtService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public string GenerateJwtToken(string userId)
    {
        try
        {
            // Decode the Base64 key
            _logger.LogInformation(_jwtSettings.Key);
            var keyBytes = Convert.FromBase64String(_jwtSettings.Key);
            _logger.LogInformation("JWT Key Length: {KeyLength} bytes", keyBytes.Length);

            if (keyBytes.Length < 16)
            {
                _logger.LogError("JWT Key is too short. It must be at least 16 bytes (128 bits) long.");
                throw new InvalidOperationException("JWT Key is too short. It must be at least 16 bytes (128 bits) long.");
            }

            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, "Invalid Base64 string for JWT Key.");
            throw;
        }
    }

}
