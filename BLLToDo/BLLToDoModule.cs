using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoAPI.DALToDo;

namespace BLLToDo
{
    public class BLLToDo
    {
        public static void Load(IServiceCollection service, IConfiguration configuration)
        {
            DALToDoModule.Load(service, configuration);
        }
    }
}