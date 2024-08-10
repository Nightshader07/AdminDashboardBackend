using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDashboard.models;

public class Utilisateur
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Required]
    public String Username { get; set; }
    
    [Required]
    public String Password { get; set; }
    
    [Required]
    [EmailAddress]
    public String Email { get; set; }
    
    [Phone]
    public String PhoneNumber { get; set; }

    public ICollection<Tache> Taches { get; set; } = new List<Tache>();
}