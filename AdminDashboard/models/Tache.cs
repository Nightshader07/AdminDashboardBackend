using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDashboard.models;

public class Tache
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required] 
    public String Name { get; set; }
    
    [Required] 
    public DateTime Deadline { get; set; }
    
    // Foreign key
    [ForeignKey("Utilisateur")]
    public long utilisateurId { get; set; }

    // Navigation property
    public  Utilisateur? Utilisateur { get; set; }
    // Foreign key
    [ForeignKey("Column")]
    public long ColumnId { get; set; }

    // Navigation property
    public  Column? Column { get; set; }
}