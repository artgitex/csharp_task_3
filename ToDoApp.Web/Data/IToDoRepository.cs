using ToDoApp.Web.Models;

namespace ToDoApp.Web.Data;

public interface IToDoRepository
{
    Task<ToDoItem> CreateAsync(ToDoItem toDoItem);    
    Task<ToDoItem> GetByIdAsync(int id);
    Task UpdateAsync(ToDoItem toDoItem);
    Task RemoveAsync(int id);
    IQueryable<ToDoItem> SetQueryable();
}
