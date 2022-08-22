using ToDoApp.Web.DbContexts;
using ToDoApp.Web.Models;

namespace ToDoApp.Web.Data;

public class ToDoRepository : IToDoRepository
{
    private readonly ToDoDbContext _context;
    private readonly IUserRepository _userRepository;

    public ToDoRepository(ToDoDbContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }
    public async Task<ToDoItem> CreateAsync(ToDoItem toDoItem)
    {
        _context.ToDoItems.Add(toDoItem);
        await _context.SaveChangesAsync();

        return toDoItem;
    }

    public async Task<ToDoItem> GetByIdAsync(int id)
    {
       var toDoItem = await _context.ToDoItems.FindAsync(id);

        return toDoItem;
    }

    public async Task RemoveAsync(int id)
    {
        var toDoItem = await _context.ToDoItems.FindAsync(id);
        _context.ToDoItems.Remove(toDoItem);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(ToDoItem toDoItem)
    {        
        _context.ToDoItems.Update(toDoItem);
        await _context.SaveChangesAsync();       
    }

    public IQueryable<ToDoItem> SetQueryable()
    {        
        return _context.Set<ToDoItem>().AsQueryable();
    }
}
