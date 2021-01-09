using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BloggerAPIs.DDL;
using BloggerAPIs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BloggerAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationSettings _appSettings;

        //Using default dependency injection provided by asp .net core
        public ApplicationUsersController(UserManager<ApplicationUser> userManager,
            IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("Register")]
        //POST : api/ApplicationUsers/Register
        public async Task<Object> PostApplicationUser(ApplicationUserModel model)
        {
            //cast model
            var applicationUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };
            try
            {
                //create user in database
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("Login")]
        //POST : api/ApplicationUsers/Login
        //Use this api to get bearer token to access other apis of the application
        //bearer token will be of JWT type
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    // writing this logic here as I consider that application would have a single way to get access token to use the application apart from that we can move this to a service in bigger application
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim("UserID", user.Id.ToString()),
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)),
                            SecurityAlgorithms.HmacSha256Signature)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    return Ok(new {token});
                }
                else
                {
                    return BadRequest(new {message = "Username or password is incorrect"});
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}