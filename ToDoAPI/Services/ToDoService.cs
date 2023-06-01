using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Runtime.CompilerServices;
using ToDoAPI.Controllers;
using ToDoAPI.DTO;
using ToDoAPI.Models;
using ToDoAPI.Models.Data;

namespace ToDoAPI.Services
{
    public class ToDoService : IToDoService
    {
        private readonly TodoDbContext _context;
        private readonly ILogger<ToDoController> _logger;
        public ToDoService(TodoDbContext context, ILogger<ToDoController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task DeleteTask(int id)
        {
            try
            {
                var task = await _context.ToDoList1.FindAsync(id);
                if (task != null)
                {
                    _context.ToDoList1.Remove(task);
                    await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted task at:" + DateTime.Now);
            }
            catch(DbUpdateException ex) 
            {
                _logger.LogCritical(ex, "Failed to delete a task at:" + DateTime.Now);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ToDoListModelDTO>> GetTasks(string owner)
        {
            try
            {
                var tasks = _context.ToDoList1
                .Where(t => t.Owner == owner)
                .Select(t => new ToDoListModelDTO()
                {
                    Id = t.Id,
                    Task = t.Task,
                    Description = t.Description,
                    Status = t.Status,
                    Owner = t.Owner
                }).ToListAsync();
                _logger.LogInformation("Got task at:" + DateTime.Now);
                return await tasks;
            }
            catch
            (DbUpdateException ex)
            {
                _logger.LogCritical(ex, "Failed to get a task at:" + DateTime.Now);
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertTask(string task, string description, string status, string owner)
        {
            try
            {
                var toDoList = new ToDoListModel()
                {
                    Task = task,
                    Description = description,
                    Status = status,
                    Owner = owner
                };

                _context.ToDoList1.Add(toDoList);                
                await _context.SaveChangesAsync();
                _logger.LogInformation("Inserted task at:" + DateTime.Now);
            }
            catch
            (DbUpdateException ex)
            {
                _logger.LogCritical(ex, "Failed to insert a task at:" + DateTime.Now);
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateTaskAs(int id, JsonPatchDocument patchDocument)
        {
            try
            {
                // Знайдіть запис у базі даних за id
                var task = await _context.ToDoList1.FindAsync(id);
                if (task != null)
                {
                    patchDocument.ApplyTo(task);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Updated task at:" + DateTime.Now);
                }
            }
            catch
            (DbUpdateException ex)
            {
                _logger.LogCritical(ex, "Failed to Update a task at:" + DateTime.Now);
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
