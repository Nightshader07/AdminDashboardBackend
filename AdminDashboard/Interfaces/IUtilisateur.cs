using AdminDashboard.DTOs;
using AdminDashboard.models;

namespace AdminDashboard.Interfaces;

public interface IUtilisateur
{
    List<Utilisateur> GetAll();
    Utilisateur GetByEmail(string email);
    Utilisateur Add(Utilisateur utilisateur);
    Utilisateur RemoveById(long id);
    Utilisateur Update(Utilisateur utilisateur);
    Utilisateur GetById(long id);
    Utilisateur Authentificate(string email, string password);
    Task<RefreshTokenDto> RefreshTokenAsync(string token, string refreshToken);
    bool IsTokenValid(string refreshToken);
}