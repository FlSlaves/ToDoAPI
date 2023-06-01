using BLLToDo.DTO;
using BLLToDo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.BLLToDo.Services;
using ToDoAPI.DALToDo.Models;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthorizeService _authorizeService;
        public AuthorizeController(IAuthorizeService authorizeService)
        {
            _authorizeService = authorizeService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegParam paramUser)
        {
            return Ok(await _authorizeService.SignUp(paramUser));
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn1(UserLogParam paramUser)
        {
            return Ok(await _authorizeService.SignIn(paramUser));
        }

    }
}
