using AdminDashboard.Data;
using AdminDashboard.Interfaces;
using AdminDashboard.models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Repositories;

public class ColumnRepository : IColumn
{
    private readonly ApplicationDbContext _context;

    public ColumnRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Column> GetAll()
    {
        return _context.Columns.ToList();
    }

    public Column Add(Column column)
    {
        _context.Columns.Add(column);
        _context.SaveChanges();
        return column;
    }

    public Column RemoveById(long id)
    {
        Column column = GetById(id);
        _context.Columns.Remove(column);
        _context.SaveChanges();
        return column;
    }

    public Column Update(Column column)
    {
        _context.Entry(column).State = EntityState.Modified;
        _context.SaveChanges();
        return column;
    }

    public Column? GetById(long id)
    {
        return _context.Columns.Find(id);
    }
}