namespace ToDoApp.Models;

public class PhotoUpload
{
    public IFormFile file { get; set; }
    public string ToDoItem { get; set; }
}
