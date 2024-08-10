using AdminDashboard.Data;
using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Repositories;

public class TacheRepository: ITache
{
    private readonly ApplicationDbContext _context;

    public TacheRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Tache> GetAll()
    {
        return _context.Taches.ToList();
    }

    public Tache Add(Tache tache)
    {
        _context.Taches.Add(tache);
        _context.SaveChanges();
        return tache;
    }

    public Tache RemoveById(long id)
    {
        Tache tache = GetById(id);
        _context.Taches.Remove(tache);
        _context.SaveChanges();
        return tache;
    }

    public Tache Update(Tache tache)
    {
        _context.Entry(tache).State = EntityState.Modified;
        _context.SaveChanges();
        return tache;
    }

    public Tache GetById(long id)
    {
        return _context.Taches.Find(id);
    }
}