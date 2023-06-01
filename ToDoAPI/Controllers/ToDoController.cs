using BLLToDo.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using System.Xml.XPath;
using ToDoAPI.BLLToDo.Services;
using ToDoAPI.DALToDo.Models;


namespace ToDoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService toDoService;
        

        public ToDoController(IToDoService _toDoService)
        {
            toDoService = _toDoService;            
        }
        // Insert new Task (User??)
        [HttpPost("InsertTask")]
        public async Task<IActionResult> InsertTask([FromBody] ToDoListModel taskModel)
        {
            await toDoService.InsertTask(taskModel.Task, taskModel.Description, taskModel.Status, taskModel.Owner);
            return Ok();
        }

        //Get All tasks for Owner 
        [HttpGet("GetTask/{owner}")]
        public async Task<ActionResult<List<ToDoListModelDTO>>> GetTasks(string owner)
        {
            return Ok(await toDoService.GetTasks(owner));
        }

        //Треба буде на фронті сформувати patch JSON і відравити його на бек
        [HttpPatch("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] JsonPatchDocument patchDocument)
        {
           await toDoService.UpdateTaskAs(id, patchDocument);
           return Ok();
        }

        // Delete task by Id
        [HttpDelete("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await toDoService.DeleteTask(id);
            return Ok();
        }
    }
}