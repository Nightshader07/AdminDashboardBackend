using AdminDashboard.models;

namespace AdminDashboard.Interfaces;

public interface IColumn
{
    public List<Column> GetAll();
    public Column Add(Column column);
    public Column RemoveById(long id);
    public Column Update(Column column);
    public Column? GetById(long id);
    
}