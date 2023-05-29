using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using ToDoAPI.DTO;
using ToDoAPI.Models;
using ToDoAPI.Models.Data;

namespace ToDoAPI.Controllers
{
    public class ToDoListService : ControllerBase
    {
        private readonly TodoDbContext _context;

        public ToDoListService(TodoDbContext context)
        {
            _context = context;
        }

        // Insert new Task (User??)
        public async Task<IActionResult> InsertTask(string task, string description, string status)
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

                return StatusCode((int)HttpStatusCode.Created);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        //Get All tasks for Owner 
        public async Task<ActionResult<List<ToDoListModelDTO>>> GetTasks(string owner)
        {
            try
            {
                var tasks = await _context.ToDoList1
                    .Where(t => t.Owner == owner)
                    .Select(t => new ToDoListModelDTO()
                    {
                        Id = t.Id,
                        Task = t.Task,
                        Description = t.Description,
                        Status = t.Status,
                        Owner = t.Owner
                    }).ToListAsync();

                if (tasks.Any())
                {
                    return Ok(tasks);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        //Треба буде на фронті сформувати patch JSON і відравити його на бек
        public async Task<IActionResult> UpdateTask(int id, [FromBody] JsonPatchDocument<ToDoListModel> patchDocument)
        {
            try
            {
                // Знайдіть запис у базі даних за id
                var task = await _context.ToDoList1.FindAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                patchDocument.ApplyTo(task);
                await _context.SaveChangesAsync();

                return Ok(task);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // Delete task by Id
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _context.ToDoList1.FindAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                _context.ToDoList1.Remove(task);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
