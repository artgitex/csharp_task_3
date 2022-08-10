using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models;

public class ToDoItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Task { get; set; }

    [Required]
    public string EndDate { get; set; }
    
    public string Status { get; set; }

    [Required]
    public string Assignee { get; set; }
}
