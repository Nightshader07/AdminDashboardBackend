using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.models;

public class Employe : Utilisateur
{
    [Required]
    public DateTime JoinDate { get; set; }

    [Required]
    public Niveau Niveau { get; set; }
}
public enum Niveau
{
    JUNIOR,
    SENIOR,
    EXPERT
}