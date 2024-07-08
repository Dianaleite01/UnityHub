using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using UnityHub.Data;
using UnityHub.Models;
using UnityHub.ViewModels;

namespace UnityHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadoresAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly SignInManager<Utilizadores> _signInManager;
        private readonly ILogger<UtilizadoresAPI> _logger;

        public UtilizadoresAPI(ApplicationDbContext context, UserManager<Utilizadores> userManager, SignInManager<Utilizadores> signInManager, ILogger<UtilizadoresAPI> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: api/UtilizadoresAPI
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "API Utilizadores está funcionando!" });
        }

        // GET: api/UtilizadoresAPI/List
        [HttpGet("list")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Utilizadores.ToListAsync();
            return Ok(users);
        }

        // POST: api/UtilizadoresAPI/Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            _logger.LogInformation("Attempting to register user with email: {Email}", model.Email);

            if (ModelState.IsValid)
            {
                var user = new Utilizadores
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nome = model.Nome,
                    Telemovel = model.Telemovel,
                    DataNascimento = model.DataNascimento,
                    Cidade = model.Cidade,
                    Pais = model.Pais
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User registered successfully: {Email}", model.Email);
                    return Ok(new { message = "Registro realizado com sucesso!" });
                }
                _logger.LogWarning("User registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(result.Errors);
            }
            _logger.LogWarning("Invalid model state for registration: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }

        // POST: api/UtilizadoresAPI/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            _logger.LogInformation("Attempting to login user with email: {Email}", model.Email);

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in successfully: {Email}", model.Email);
                        return Ok(new { message = "Login realizado com sucesso!" });
                    }
                    else if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out: {Email}", model.Email);
                        return BadRequest(new { message = "User account locked out." });
                    }
                    else if (result.IsNotAllowed)
                    {
                        _logger.LogWarning("User login not allowed: {Email}", model.Email);
                        return BadRequest(new { message = "You are not allowed to login." });
                    }
                    else
                    {
                        _logger.LogWarning("Invalid login attempt: {Email}", model.Email);
                        return BadRequest(new { message = "Invalid login attempt." });
                    }
                }
                _logger.LogWarning("User not found: {Email}", model.Email);
                return BadRequest(new { message = "User not found." });
            }
            _logger.LogWarning("Invalid model state for login: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }

        // POST: api/UtilizadoresAPI/Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out successfully.");
            return Ok(new { message = "Logout realizado com sucesso!" });
        }
    }
}
