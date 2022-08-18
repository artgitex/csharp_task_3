using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Web.Models.Requests;

public class RegisterRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }

    [Required]
    public string Age { get; set; }

    [Required]
    public string EmploymentDate { get; set; }

    public byte[]? Photo { get; set; }
}
