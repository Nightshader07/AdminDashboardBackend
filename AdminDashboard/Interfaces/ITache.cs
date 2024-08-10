using AdminDashboard.models;

namespace AdminDashboard.Interfaces;

public interface ITache
{
    List<Tache> GetAll();
    Tache Add(Tache tache);
    Tache RemoveById(long id);
    Tache Update(Tache tache);
    Tache GetById(long id);
}