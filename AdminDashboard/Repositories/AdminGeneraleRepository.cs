using AdminDashboard.Data;
using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Repositories;

public class AdminGeneraleRepository:IAdminGenerale
{
    private readonly ApplicationDbContext _context;

    public AdminGeneraleRepository(ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
    }

    public List<models.AdminGenerale> GetAll()
    {
        return _context.AdminGenerales.ToList();
    }

    public models.AdminGenerale GetByEmail(string email)
    {
        return _context.AdminGenerales.FirstOrDefault(a => a.Email == email);
    }

    public models.AdminGenerale Add(models.AdminGenerale adminGenerale)
    {
        adminGenerale.Password = BCrypt.Net.BCrypt.HashPassword(adminGenerale.Password);
        _context.AdminGenerales.Add(adminGenerale);
        _context.SaveChanges();
        return adminGenerale;
    }

    public models.AdminGenerale RemoveById(long id)
    {
        var adminGenerale = _context.AdminGenerales.Find(id);
        if (adminGenerale!=null)
        {
            _context.AdminGenerales.Remove(adminGenerale);
            _context.SaveChanges();
        }

        return adminGenerale;
    }

    public models.AdminGenerale Update(models.AdminGenerale adminGenerale)
    {
        _context.Entry(adminGenerale).State = EntityState.Modified;
        _context.SaveChanges();
        return adminGenerale;
    }

    public models.AdminGenerale GetById(long id)
    {
        models.AdminGenerale? adminGenerale = _context.AdminGenerales.Find(id);
        return adminGenerale;
    }

    public models.AdminGenerale Authentificate(string email, string password)
    {
        models.AdminGenerale? adminGenerale = GetByEmail(email);
        if (adminGenerale == null)
        {
            return null; 
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, adminGenerale.Password);
        if (isPasswordValid)
        {
            return adminGenerale;
        }

        return null;
    }
}