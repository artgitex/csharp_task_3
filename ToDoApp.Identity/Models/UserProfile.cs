using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Identity.Models;

public class UserProfile
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Age { get; set; }

    [Required]
    public string EmploymentDate { get; set; }

    public byte[]? Photo { get; set; }
}
