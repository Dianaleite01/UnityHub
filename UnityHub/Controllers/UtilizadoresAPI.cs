using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityHub.Data;
using UnityHub.Models;
using UnityHub.ViewModels;

namespace UnityHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UtilizadoresAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly SignInManager<Utilizadores> _signInManager;
        private readonly string _jwtSecret;

        public UtilizadoresAPI(ApplicationDbContext context, UserManager<Utilizadores> userManager, SignInManager<Utilizadores> signInManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSecret = configuration.GetValue<string>("JwtSecret");
        }

        // Método para originar um token JWT para um utilizador
        private string GenerateJwtToken(Utilizadores user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim("id", user.Id.ToString()),
                    new System.Security.Claims.Claim("email", user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // POST: api/UtilizadoresAPI/Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        var token = GenerateJwtToken(user);
                        return Ok(new { token });
                    }
                    else if (result.IsLockedOut)
                    {
                        return BadRequest(new { message = "User account locked out." });
                    }
                    else if (result.IsNotAllowed)
                    {
                        return BadRequest(new { message = "You are not allowed to login." });
                    }
                    else
                    {
                        return BadRequest(new { message = "Invalid login attempt." });
                    }
                }
                return BadRequest(new { message = "User not found." });
            }
            return BadRequest(ModelState);
        }

        // POST: api/UtilizadoresAPI/Register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
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
                    var token = GenerateJwtToken(user);
                    return Ok(new { token });
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }

        // POST: api/UtilizadoresAPI/Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logout realizado com sucesso!" });
        }

        // Método GET para obter todos os utilizadores
        [HttpGet("list")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Utilizadores>>> GetUtilizadores()
        {
            var utilizadores = await _context.Utilizadores
                .AsNoTracking()
                .ToListAsync();
            return Ok(utilizadores);
        }
    }
}
