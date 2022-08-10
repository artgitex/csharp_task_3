using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Pages;

public class IndexModel : PageModel
{
    private readonly IToDoRepository _todoRepository;        
    private readonly RazorRenderService _renderService;
    private readonly ILogger<IndexModel> _logger;
    public IndexModel(ILogger<IndexModel> logger, IToDoRepository todoRepository, RazorRenderService renderService)
    {
        _logger = logger;
        _todoRepository = todoRepository;            
        _renderService = renderService;
    }
    public IEnumerable<ToDoItem> todoItems { get; set; }

    public void OnGet()
    {

    }
    public async Task<PartialViewResult> OnGetViewAllPartial()
    {
        todoItems = await _todoRepository.GetAllAsync();
        return new PartialViewResult
        {
            ViewName = "../ToDoList/Index",
            ViewData = new ViewDataDictionary<IEnumerable<ToDoItem>>(ViewData, todoItems)
        };
    }
            
    public async Task<JsonResult> OnGetCreateOrEditAsync(int id = 0)
    {
        if (id == 0)
            return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("../ToDoList/CreateOrEdit", new ToDoItem()) });
        else
        {                
            var todoItem = await _todoRepository.GetByIdAsync(id);
            return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("../ToDoList/CreateOrEdit", todoItem) });         
        }
    }
            
    public async Task<JsonResult> OnPostCreateOrEditAsync(int id, ToDoItem todoItem)
    {
        
        if (ModelState.IsValid)
        {
            if (id == 0)
            {
                await _todoRepository.AddAsync(todoItem);                    
            }
            else
            {
                await _todoRepository.UpdateAsync(todoItem);                    
            }

            todoItems = await _todoRepository.GetAllAsync();
            var html = await _renderService.ToStringAsync("../ToDoList/Index", todoItems);
            return new JsonResult(new { isValid = true, html = html });
        }
        else
        {
            var html = await _renderService.ToStringAsync("../ToDoList/CreateOrEdit", todoItems);
            return new JsonResult(new { isValid = false, html = html });
        }        
    }
      
    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {        
        await _todoRepository.DeleteAsync(id);

        todoItems = await _todoRepository.GetAllAsync();
        var html = await _renderService.ToStringAsync("../ToDoList/Index", todoItems);
        return new JsonResult(new { isValid = true, html = html });
    }    
}