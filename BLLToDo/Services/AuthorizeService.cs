using BLLToDo.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLLToDo.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _options;
        public AuthorizeService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options;
        }

        public async Task<ResponseMessage> SignUp(UserRegParam paramUser)
        {
            var userExists = await _userManager.FindByNameAsync(paramUser.UserName);

            if (userExists != null)
                return new ResponseMessage() { Status = "Error", Message = "User already exists!" };

            var user = new IdentityUser()
            {
                UserName = paramUser.UserName,
                Email = paramUser.Email
            };
            var result = await _userManager.CreateAsync(user, paramUser.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("Email", paramUser.Email));
                claims.Add(new Claim("Name", paramUser.UserName));
                await _userManager.AddClaimsAsync(user, claims);
            }
            else
            {
                return new ResponseMessage() { Status = "Error", Message = "Some errors, pleace check your data" };
            }
            return new ResponseMessage() { Message = "User Registered!" };
        }
        public async Task<ResponseMessage> SignIn(UserLogParam paramUser)
        {
            var user = await _userManager.FindByNameAsync(paramUser.UserName);
            var result = await _signInManager.PasswordSignInAsync(user, paramUser.Password, false, false);
            if (result.Succeeded)
            {
                IEnumerable<Claim> claims = await _userManager.GetClaimsAsync(user);
                var token = GetToken(claims);
                return new ResponseMessage() { Token = new JwtSecurityTokenHandler().WriteToken(token), Message = "Login success!" };

            }
            return new ResponseMessage() { Status = "Error", Message = "User not found!" };
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> cl)
        {
            var claims = cl.ToList();
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options["JWT:SecretKey"]));
            var token = new JwtSecurityToken(
                issuer: _options["JWT:ValidIssuer"],
                audience: _options["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
