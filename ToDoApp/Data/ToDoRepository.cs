using ToDoApp.DbContexts;
using ToDoApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Data;

public class ToDoRepository : IToDoRepository
{
    private readonly ToDoDbContext _context;

    public ToDoRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public async Task<ToDoItem> AddAsync(ToDoItem todoItem)
    {
        await _context.ToDoItems.AddAsync(todoItem);
        await _context.SaveChangesAsync();

        return todoItem;
    }

    public async Task<IEnumerable<ToDoItem>> GetAllAsync()
    {
        IEnumerable<ToDoItem> items = await _context.ToDoItems.ToListAsync();
        return items;        
    }

    public async Task<ToDoItem> GetByIdAsync(int id)
    {
        return await _context.ToDoItems.FindAsync(id);
    }

    public async Task UpdateAsync(ToDoItem todoItem)
    {        
        _context.ToDoItems.Update(todoItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var todoItem = await GetByIdAsync(id);
        _context.ToDoItems.Remove(todoItem);
        await _context.SaveChangesAsync();
    }

    
}
