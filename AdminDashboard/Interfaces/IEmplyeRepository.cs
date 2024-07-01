using AdminDashboard.models;

namespace AdminDashboard.Interfaces;

public interface IEmplyeRepository
{
    public List<Employe> GetAll();
    public Employe GetByEmail(string email);
    public Employe Add(Employe employe);
    public Employe RemoveById(long id);
    public Employe Update(Employe employe);
    public Employe GetById(long id);
    public Employe Authentificate(string email,string password);

}