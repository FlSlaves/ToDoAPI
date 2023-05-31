using BLLToDo.DTO;
using Microsoft.AspNetCore.JsonPatch;

namespace ToDoAPI.BLLToDo.Services
{
    public interface IToDoService
    {
        Task InsertTask(string task, string description, string status, string owner);
        Task<List<ToDoListModelDTO>> GetTasks(string owner);
        Task UpdateTaskAs(int id, JsonPatchDocument patchDocument);
        Task DeleteTask(int id);
    }
}