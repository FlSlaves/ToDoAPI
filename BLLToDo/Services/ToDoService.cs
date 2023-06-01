using BLLToDo.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoAPI.DALToDo.Models;
using ToDoAPI.DALToDo.Models.Data;

namespace ToDoAPI.BLLToDo.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ILogger<ToDoService> _logger;
        private readonly TodoDbContext _context;

        public ToDoService(TodoDbContext context, ILogger<ToDoService> logger)
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
                _logger.LogInformation("Task deleted at:" + DateTime.Now);
            }
            catch(DbUpdateException ex) 
            {
                _logger.LogError(ex, "Failed to delete a at:" + DateTime.Now);
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
                _logger.LogInformation("Task got at:" + DateTime.Now);
                return await tasks;
            }
            catch
            (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to get at:" + DateTime.Now);
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
                _logger.LogInformation("Task inserted at:" + DateTime.Now);
            }
            catch
            (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to insert at:" + DateTime.Now);
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
                    _logger.LogInformation("Task updated at:" + DateTime.Now);
                }
            }
            catch
            (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to update at:" + DateTime.Now);
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
