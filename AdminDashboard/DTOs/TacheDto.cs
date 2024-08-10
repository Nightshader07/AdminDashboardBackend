namespace AdminDashboard.DTOs;

public class TacheDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime Deadline { get; set; }
    public long UtilisateurId { get; set; }
    public long ColumnId { get; set; }
}