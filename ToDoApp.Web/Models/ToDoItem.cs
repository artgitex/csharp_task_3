using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Web.Models;

public class ToDoItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Task { get; set; }

    [Required]
    [DisplayName("End Date")]
    public string EndDate { get; set; }

    [Required]
    public string Status { get; set; }

    [Required]
    public string Assignee { get; set; }

    public byte[]? Photo { get; set; }
}
