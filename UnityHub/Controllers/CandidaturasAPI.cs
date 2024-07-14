using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityHub.Data;
using UnityHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace UnityHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidaturasAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;

        public CandidaturasAPI(ApplicationDbContext context, UserManager<Utilizadores> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Método GET para obter todas as candidaturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidaturas>>> GetCandidaturas()
        {
            return await _context.Candidaturas
                .Include(c => c.Utilizador)
                .Include(c => c.Vaga)
                .AsNoTracking()
                .ToListAsync();
        }

        // Método GET para obter uma candidatura específica pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Candidaturas>> GetCandidaturas(int id)
        {
            var candidatura = await _context.Candidaturas
                .Include(c => c.Utilizador)
                .Include(c => c.Vaga)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (candidatura == null)
            {
                return NotFound();
            }

            return candidatura;
        }

        // Método GET para verificar se o utilizador atual já se candidatou a uma vaga específica
        [HttpGet("hasApplied/{vagaId}")]
        [Authorize]
        public async Task<ActionResult<bool>> HasApplied(int vagaId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var existingCandidatura = await _context.Candidaturas
                .AnyAsync(c => c.UtilizadorFK == user.Id && c.VagaFK == vagaId);

            return Ok(existingCandidatura);
        }

        // Método POST para criar uma nova candidatura
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Candidaturas>> CreateCandidatura([FromBody] CreateCandidaturaRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Verificação para garantir que o administrador não se candidate a uma vaga
            if (await _userManager.IsInRoleAsync(user, "admin"))
            {
                return BadRequest("Administradores não podem candidatar-se a vagas.");
            }

            // Verificação para garantir que o utilizador não se candidate à mesma vaga mais de uma vez
            var existingCandidatura = await _context.Candidaturas
                .FirstOrDefaultAsync(c => c.UtilizadorFK == user.Id && c.VagaFK == request.VagaFK);

            if (existingCandidatura != null)
            {
                return BadRequest("Já se candidatou a esta vaga.");
            }

            var candidatura = new Candidaturas
            {
                UtilizadorFK = user.Id,
                VagaFK = request.VagaFK,
                Estado = "Pendente"
            };

            if (ModelState.IsValid)
            {
                _context.Candidaturas.Add(candidatura);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCandidaturas), new { id = candidatura.Id }, candidatura);
            }

            return BadRequest(ModelState);
        }

        public class CreateCandidaturaRequest
        {
            public int VagaFK { get; set; }
        }

        // Método PUT para editar uma candidatura existente
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditCandidatura(int id, [FromBody] Candidaturas candidatura)
        {
            if (id != candidatura.Id)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            _context.Entry(candidatura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidaturasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Método GET para verificar se o utilizador atual é administrador
        [HttpGet("isAdmin")]
        [Authorize]
        public async Task<ActionResult<bool>> IsAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "admin");
            return Ok(isAdmin);
        }

        // Método POST para aceitar uma candidatura
        [HttpPost("accept/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AcceptCandidatura(int id)
        {
            var candidatura = await _context.Candidaturas.FindAsync(id);
            if (candidatura == null)
            {
                return NotFound();
            }

            candidatura.Estado = "Aceite";
            _context.Entry(candidatura).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método POST para rejeitar uma candidatura
        [HttpPost("reject/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RejectCandidatura(int id)
        {
            var candidatura = await _context.Candidaturas.FindAsync(id);
            if (candidatura == null)
            {
                return NotFound();
            }

            candidatura.Estado = "Rejeitada";
            _context.Entry(candidatura).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método DELETE para eliminar uma candidatura
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCandidatura(int id)
        {
            var candidatura = await _context.Candidaturas.FindAsync(id);
            if (candidatura == null)
            {
                return NotFound();
            }

            _context.Candidaturas.Remove(candidatura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Método para verificar se uma candidatura existe
        private bool CandidaturasExists(int id)
        {
            return _context.Candidaturas.Any(e => e.Id == id);
        }
    }
}
