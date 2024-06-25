using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDashboard.models;

public class Utilisateur
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Required]
    private String Username { get; set; }
    
    [Required]
    private String Password { get; set; }
    
    [Required]
    [EmailAddress]
    private String Email { get; set; }
    
    [Phone]
    private String PhoneNumber { get; set; }
}