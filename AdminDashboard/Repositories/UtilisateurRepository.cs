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

        public UtilisateurRepository(ApplicationDbContext context, IJwtService jwtService, IRefreshTokenService refreshTokenService, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _jwtSettings = jwtSettings.Value;
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

            var storedRefreshToken = await _refreshTokenService.GetRefreshTokenAsync(refreshToken);
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
    }
}
