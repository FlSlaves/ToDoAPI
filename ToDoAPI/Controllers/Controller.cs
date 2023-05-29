using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using System.Xml.XPath;
using ToDoAPI.DTO;
using ToDoAPI.Models;
using ToDoAPI.Models.Data;

namespace ToDoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly TodoDbContext Context;

        public ToDoController(TodoDbContext todoContext)
        {
            Context = todoContext;
        }

        // Insert new Task (User??)
        [HttpPost("InsertTask/{task}/{description}/{status}")]
        public async Task<IActionResult> InsertTask(string task, string description, string status)
        {
            var service = new ToDoListService(Context);
            return await service.InsertTask(task, description, status);
        }

        //Get All tasks for Owner 
        [HttpGet("GetTask/{owner}")]
        public async Task<ActionResult<List<ToDoListModelDTO>>> GetTasks(string owner)
        {
            var service = new ToDoListService(Context);
            return await service.GetTasks(owner);
        }

        //Треба буде на фронті сформувати patch JSON і відравити його на бек
        [HttpPatch("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] JsonPatchDocument<ToDoListModel> patchDocument)
        {
            var service = new ToDoListService(Context);
            return await service.UpdateTask(id, patchDocument);
        }

        // Delete task by Id
        [HttpDelete("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var service = new ToDoListService(Context);
            return await service.DeleteTask(id);
        }
    }
}