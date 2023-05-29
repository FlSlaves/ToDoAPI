using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Runtime.CompilerServices;
using ToDoAPI.DTO;
using ToDoAPI.Models;
using ToDoAPI.Models.Data;

namespace ToDoAPI.Services
{
    public class ToDoService : IToDoService
    {
        private readonly TodoDbContext _context;

        public ToDoService(TodoDbContext context)
        {
            _context = context;
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

                _context.ToDoList1.Remove(task);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex) 
            {
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
                return await tasks;
            }
            catch
            (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertTask(string task, string description, string status)
        {
            try
            {
                var toDoList = new ToDoListModel()
                {
                    Task = task,
                    Description = description,
                    Status = status,
                    Owner = "IDKowner"
                };

                _context.ToDoList1.Add(toDoList);
                await _context.SaveChangesAsync();
            }
            catch
            (DbUpdateException ex)
            {
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
                }
            }
            catch
            (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
