using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Azure.Core;
using Recipe.Repositories.Interface;
using Recipe.Models.DTO;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenRepository _tokenRepository;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    IsAdmin = false
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok("Registration successful.");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest("Invalid model state.");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Identifier);
            if (user == null)
            {
                // User with email not found, try with username
                user = await _userManager.FindByNameAsync(model.Identifier);
            }

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    var jwtToken = _tokenRepository.CreateJwtToken(user, roles.ToList());
                    var response = new LoginResponseDTO
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };
                    return Ok(response);
                    //return Ok("Login successful.");
                }
            }

            return BadRequest("Invalid login attempt.");
        }

        [HttpGet]
        [Route("check-registration")]
        public async Task<IActionResult> CheckRegistration([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Ok("User is registered.");
            }
            return NotFound("User is not registered.");
        }
    }
}
