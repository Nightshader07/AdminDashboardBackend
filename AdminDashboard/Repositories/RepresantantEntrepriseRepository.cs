using System.Data.Entity;
using AdminDashboard.Data;
using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Repositories;

public class RepresantantEntrepriseRepository : IRepresantantEntreprise
{
    private readonly ApplicationDbContext _context;

    public RepresantantEntrepriseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<RepresentantEntreprise> GetAll()
    {
        return _context.RepresentantEntreprises.ToList();;
    }

    public async Task<RepresentantEntreprise> GetByEmail(string email)
    {
        return await _context.RepresentantEntreprises.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<RepresentantEntreprise> Add(RepresentantEntreprise representantEntreprise)
    {
        try
        {
            representantEntreprise.Password = BCrypt.Net.BCrypt.HashPassword(representantEntreprise.Password);
            await _context.RepresentantEntreprises.AddAsync(representantEntreprise);
            await _context.SaveChangesAsync();
            return representantEntreprise;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new Exception("Concurrency error occurred while adding the representant.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while updating the database.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the representant.", ex);
        }
    }

     public async Task<RepresentantEntreprise> RemoveById(long id)
        {
            var representantEntreprise = await _context.RepresentantEntreprises.FindAsync(id);
            if (representantEntreprise != null)
            {
                _context.RepresentantEntreprises.Remove(representantEntreprise);
                await _context.SaveChangesAsync();
            }
            return representantEntreprise;
        }

        public async Task<RepresentantEntreprise> Update(RepresentantEntreprise representantEntreprise)
        {
            var existingRepresentant = await _context.RepresentantEntreprises.FindAsync(representantEntreprise.Id);
            if (existingRepresentant == null)
            {
                return null;
            }
            existingRepresentant.Username = representantEntreprise.Username;
            existingRepresentant.Email = representantEntreprise.Email;
            existingRepresentant.PhoneNumber = representantEntreprise.PhoneNumber;
            existingRepresentant.CompanyName = representantEntreprise.CompanyName;
            existingRepresentant.Location = representantEntreprise.Location;
            existingRepresentant.DomainName = representantEntreprise.DomainName;
            existingRepresentant.Type = representantEntreprise.Type;
            existingRepresentant.LogoURL = representantEntreprise.LogoURL;

            if (!string.IsNullOrWhiteSpace(representantEntreprise.Password))
            {
                existingRepresentant.Password = BCrypt.Net.BCrypt.HashPassword(representantEntreprise.Password);
            }

            _context.RepresentantEntreprises.Update(existingRepresentant);
            await _context.SaveChangesAsync();
            return existingRepresentant;
        }

        public async Task<RepresentantEntreprise> GetById(long id)
        {
            return await _context.RepresentantEntreprises.FindAsync(id);
        }

        public async Task<RepresentantEntreprise> Authentificate(string email, string password)
        {
            var representantEntreprise = await _context.RepresentantEntreprises.FirstOrDefaultAsync(e => e.Email == email);
            if (representantEntreprise == null || !BCrypt.Net.BCrypt.Verify(password, representantEntreprise.Password))
            {
                return null;
            }
            return representantEntreprise;
        }
}