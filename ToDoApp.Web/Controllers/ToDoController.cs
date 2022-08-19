﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Web.DbContexts;
using ToDoApp.Web.Models;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;
using ToDoApp.Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace ToDoApp.Web.Controllers;

public class ToDoController : Controller
{
    private readonly ToDoDbContext _context;
    private readonly IToDoRepository _toDoRepository;
    private readonly IUserRepository _userRepository;    

    public ToDoController(ToDoDbContext context, IToDoRepository toDoRepository, IUserRepository userRepository)
    {
        _context = context;
        _toDoRepository = toDoRepository;
        _userRepository = userRepository;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> GetData()
    {              
        var draw = Request.Form["draw"].FirstOrDefault();
        var start = Request.Form["start"].FirstOrDefault();
        var length = Request.Form["length"].FirstOrDefault();
        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        int skip = start != null ? Convert.ToInt32(start) : 0;
        int recordsTotal = 0;

        var todoItems = _toDoRepository.SetQueryable();

        /*
        foreach (ToDoItem row in todoItems)
        {
            if (row.Photo != null)
                row.Photo = this.GetImage(Convert.ToBase64String(row.Photo));                
        }
        */

        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
        {
            todoItems = todoItems.OrderBy(sortColumn + " " + sortColumnDirection);
        }
        if (!string.IsNullOrEmpty(searchValue))
        {                
            todoItems = todoItems.Where(m => m.Task.Contains(searchValue)
                                        || m.EndDate.Contains(searchValue)
                                        || m.Status.Contains(searchValue)
                                        || m.Assignee.Contains(searchValue));                
        }
        
        recordsTotal = await todoItems.CountAsync();

        var jsonData = new
        {
            draw = draw,
            recordsFiltered = recordsTotal,
            recordsTotal = recordsTotal,
            data = await todoItems
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync()
        };
        return Ok(jsonData);
    }

    public byte[] GetImage(string sBase64String)
    {        
        byte[] bytes = null;
        if (!string.IsNullOrEmpty(sBase64String))
        {
            bytes = Convert.FromBase64String(sBase64String);
        }

        return bytes;
    }
           
    [HttpGet]
    //[Authorize] 
    public async Task<IActionResult> Index()
    {        
        return View();            
    }
            
    public async Task<IActionResult> AddOrEdit(int id = 0)
    {
        
        if (id == 0)
        {
            return View(new ToDoItem());
        }
        else
        {            
            return View(await _toDoRepository.GetByIdAsync(id));
        }
    }        
   
    [HttpPost]        
    public async Task<IActionResult> AddOrEdit(ToDoItem todoItem)
    { 
        if (todoItem.Id == 0)
        {
            await _toDoRepository.CreateAsync(todoItem);           
            TempData["AlertMessage"] = "Created Successfully";
        }
        else
        {
           await _toDoRepository.UpdateAsync(todoItem);
           TempData["AlertMessage"] = "Updated Successfully";
        }       

        return Json(new { success = true });
    }


    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        await _toDoRepository.RemoveAsync(id);
        
        return Json(new { success = true });
    }
}
