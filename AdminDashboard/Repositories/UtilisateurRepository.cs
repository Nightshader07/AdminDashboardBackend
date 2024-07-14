using AdminDashboard.Data;
using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Repositories;

public class UtilisateurRepository: IUtilisateur
{
    private readonly ApplicationDbContext _context;

    public UtilisateurRepository(ApplicationDbContext context)
    {
        _context = context;
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
        Utilisateur? utilisateur = _context.Utilisateurs.Find(id);
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
        Utilisateur? utilisateur = GetByEmail(email);
        if (utilisateur == null)
        {
            return null; 
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, utilisateur.Password);
        if (isPasswordValid)
        {
            return utilisateur;
        }

        return null;    }
}