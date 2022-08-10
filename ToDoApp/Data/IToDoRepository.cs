using ToDoApp.Models;

namespace ToDoApp.Data;

public interface IToDoRepository
{
    Task<ToDoItem> GetByIdAsync(int id);
    Task<IEnumerable<ToDoItem>> GetAllAsync();
    Task<ToDoItem> AddAsync(ToDoItem todoItem);
    Task UpdateAsync(ToDoItem todoItem);
    Task DeleteAsync(int id);
}
