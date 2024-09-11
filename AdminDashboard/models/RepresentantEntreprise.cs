using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.models;

public class RepresentantEntreprise : Utilisateur
{
    [Required]
    public string LogoURL { get; set; }

    [Required]
    public string CompanyName { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    public string DomainName { get; set; }

    [Required]
    public string Type { get; set; }
}