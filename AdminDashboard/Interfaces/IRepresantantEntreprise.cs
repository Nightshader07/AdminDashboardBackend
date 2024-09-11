using AdminDashboard.models;

namespace AdminDashboard.Interfaces;

public interface IRepresantantEntreprise
{
    List<RepresentantEntreprise> GetAll();
    Task<RepresentantEntreprise> GetByEmail(string email);
    Task<RepresentantEntreprise> Add(RepresentantEntreprise representantEntreprise);
    Task<RepresentantEntreprise> RemoveById(long id);
    Task<RepresentantEntreprise> Update(RepresentantEntreprise representantEntreprise);
    Task<RepresentantEntreprise> GetById(long id);
    Task<RepresentantEntreprise> Authentificate(string email,string password);
}