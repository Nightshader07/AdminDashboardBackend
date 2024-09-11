using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AdminDashboard.Data;
using AdminDashboard.DTOs;
using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AdminDashboard.Repositories
{
    public class UtilisateurRepository : IUtilisateur
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtService> _logger;
        public UtilisateurRepository(ApplicationDbContext context, IJwtService jwtService, IRefreshTokenService refreshTokenService, IOptions<JwtSettings> jwtSettings, ILogger<JwtService> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public List<Utilisateur> GetAll()
        {
            return _context.Utilisateurs.ToList();
        }

        public Utilisateur GetByEmail(string email)
        {
            return _context.Utilisateurs.FirstOrDefault(u => u.Email == email);
        }

        public Utilisateur Add(Utilisateur utilisateur)
        {
            utilisateur.Password = BCrypt.Net.BCrypt.HashPassword(utilisateur.Password);
            _context.Utilisateurs.Add(utilisateur);
            _context.SaveChanges();
            return utilisateur;
        }

        public Utilisateur RemoveById(long id)
        {
            Utilisateur utilisateur = _context.Utilisateurs.Find(id);
            _context.Utilisateurs.Remove(utilisateur);
            _context.SaveChanges();
            return utilisateur;
        }

        public Utilisateur Update(Utilisateur utilisateur)
        {
            _context.Entry(utilisateur).State = EntityState.Modified;
            _context.SaveChanges();
            return utilisateur;
        }

        public Utilisateur GetById(long id)
        {
            return _context.Utilisateurs.Find(id);
        }

        public Utilisateur Authentificate(string email, string password)
        {
            Utilisateur utilisateur = GetByEmail(email);
            if (utilisateur == null)
            {
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, utilisateur.Password);
            if (isPasswordValid)
            {
                return utilisateur;
            }

            return null;
        }

        public async Task<RefreshTokenDto> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var storedRefreshToken =  _refreshTokenService.GetRefreshToken(refreshToken);
            if (storedRefreshToken == null || storedRefreshToken.UserId != userId || storedRefreshToken.ExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            // Generate new JWT token
            var newJwtToken = _jwtService.GenerateJwtToken(userId);

            // Revoke the used refresh token and generate a new one
            await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
            var newRefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(userId);

            // Return tokens to the client
            return new RefreshTokenDto
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // We don't care about the lifetime of the expired token
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        public bool IsTokenValid(string token)
        {
             var tokenHandler = new JwtSecurityTokenHandler();
    var key = Convert.FromBase64String(_jwtSettings.Key);

    try
    {
        // Decode the token to see its raw content
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        if (jwtToken == null)
        {
            Console.WriteLine("Invalid token format.");
            return false;
        }

        // Log the token claims for inspection
        Console.WriteLine("Token Claims:");
        foreach (var claim in jwtToken.Claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }

        // Validate the token
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // Assuming you're not validating the issuer in this context
            ValidateAudience = false, // Assuming you're not validating the audience in this context
            ClockSkew = TimeSpan.Zero // No tolerance for expiration time
        }, out SecurityToken validatedToken);

        // Log the validated token details
        var validatedJwtToken = validatedToken as JwtSecurityToken;
        if (validatedJwtToken == null)
        {
            Console.WriteLine("Token validation failed: Not a valid JWT token.");
            return false;
        }

        // Extract and log the user ID from the "sub" claim
        var userId = validatedJwtToken.Claims.First(x => x.Type == "sub").Value;
        Console.WriteLine($"Validated User ID: {userId}");

        return true;
    }
    catch (SecurityTokenExpiredException)
    {
        Console.WriteLine("Token validation failed: Token has expired.");
        return false;
    }
    catch (SecurityTokenInvalidSignatureException)
    {
        Console.WriteLine("Token validation failed: Invalid signature.");
        return false;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Token validation failed: {ex.Message}");
        return false;
    }
        }
    }
}
