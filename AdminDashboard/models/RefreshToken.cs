using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.models;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Token { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }

    [Required]
    public bool IsRevoked { get; set; } = false;

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedDate { get; set; }
}