using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoAPI.DALToDo;
using ToDoAPI.DALToDo.Models.Data;

namespace ToDoAPI.DALToDo
{
    public class DALToDoModule
    {
        public static void Load(IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<TodoDbContext>(option =>
            option.UseSqlServer(configuration.GetConnectionString("MyConnection")));
        }
    }
}