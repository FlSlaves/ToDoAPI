using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.DTO;

namespace ToDoAPI.Services
{
    public interface IToDoService
    {
        Task InsertTask(string task, string description, string status);
        Task<List<ToDoListModelDTO>> GetTasks(string owner);
        Task UpdateTaskAs(int id, JsonPatchDocument patchDocument);
        Task DeleteTask(int id);
    }
}
