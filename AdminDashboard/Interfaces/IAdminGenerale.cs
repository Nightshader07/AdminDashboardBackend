using AdminDashboard.models;

namespace AdminDashboard.Interfaces
{
    public interface IAdminGeneraleRepository
    {
        List<AdminGenerale> GetAll();
        AdminGenerale GetByEmail(string email);
        AdminGenerale Add(AdminGenerale adminGenerale);
        AdminGenerale RemoveById(long id);
        AdminGenerale Update(AdminGenerale adminGenerale);
        AdminGenerale GetById(long id);
        AdminGenerale Authentificate(string email, string password);
    }
}
