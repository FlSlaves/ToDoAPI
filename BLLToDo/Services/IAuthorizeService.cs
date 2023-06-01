using BLLToDo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLToDo.Services
{
    public interface IAuthorizeService
    {
        Task<ResponseMessage> SignUp(UserRegParam paramUser);
        Task<ResponseMessage> SignIn(UserLogParam paramUser);
    }
}
