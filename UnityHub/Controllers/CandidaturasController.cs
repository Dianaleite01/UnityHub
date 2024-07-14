using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnityHub.Data;
using UnityHub.Models;

namespace UnityHub.Controllers
{
    [Authorize]
    public class CandidaturasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly ILogger<CandidaturasController> _logger;

        public CandidaturasController(ApplicationDbContext context, UserManager<Utilizadores> userManager, ILogger<CandidaturasController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Candidaturas
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var applicationDbContext = _context.Candidaturas
                .Include(c => c.Utilizador)
                .Include(c => c.Vaga)
                .AsNoTracking();

            var pagedData = await applicationDbContext
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(pagedData);
        }

        // GET: Todas as candidaturas (apenas para administradores)
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> All(int pageNumber = 1, int pageSize = 10)
        {
            var candidaturas = await _context.Candidaturas
                .Include(c => c.Vaga)
                .Include(c => c.Utilizador)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(candidaturas);
        }

        // POST: Candidaturas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int VagaFK)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Utilizadores");
            }

            // Verificação para garantir que o administrador não se candidate a uma vaga
            if (await _userManager.IsInRoleAsync(user, "admin"))
            {
                // Mostrar uma mensagem de erro
                TempData["ErrorMessage"] = "Administradores não podem candidatar-se a vagas.";
                return RedirectToAction("Details", "Home", new { id = VagaFK });
            }

            // Verificação para garantir que o utilizador não se candidate à mesma vaga mais de uma vez
            var existingCandidatura = await _context.Candidaturas
                .FirstOrDefaultAsync(c => c.UtilizadorFK == user.Id && c.VagaFK == VagaFK);

            if (existingCandidatura != null)
            {
                // Mostrar uma mensagem de erro
                TempData["ErrorMessage"] = "Já se candidatou a esta vaga.";
                return RedirectToAction("Details", "Home", new { id = VagaFK });
            }

            var candidatura = new Candidaturas
            {
                UtilizadorFK = user.Id,
                VagaFK = VagaFK,
                Estado = "Pendente"
            };

            if (ModelState.IsValid)
            {
                _context.Add(candidatura);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(candidatura);
        }

        // GET: Candidaturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidaturas = await _context.Candidaturas.FindAsync(id);
            if (candidaturas == null)
            {
                return NotFound();
            }
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizadores, "Id", "Nome", candidaturas.UtilizadorFK);
            ViewData["VagaFK"] = new SelectList(_context.Vagas, "Id", "Nome", candidaturas.VagaFK);
            return View(candidaturas);
        }

        // POST: Candidaturas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Estado,VagaFK,UtilizadorFK")] Candidaturas candidaturas)
        {
            if (id != candidaturas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidaturas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidaturasExists(candidaturas.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizadores, "Id", "Nome", candidaturas.UtilizadorFK);
            ViewData["VagaFK"] = new SelectList(_context.Vagas, "Id", "Nome", candidaturas.VagaFK);
            return View(candidaturas);
        }

        // Método para aceitar uma candidatura (apenas para administradores)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Accept(int id)
        {
            var candidatura = await _context.Candidaturas.FindAsync(id);
            if (candidatura == null)
            {
                return NotFound();
            }

            candidatura.Estado = "Aceite";
            _context.Update(candidatura);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        // Método para rejeitar uma candidatura (apenas para administradores)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var candidatura = await _context.Candidaturas.FindAsync(id);
            if (candidatura == null)
            {
                return NotFound();
            }

            candidatura.Estado = "Rejeitada";
            _context.Update(candidatura);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

        // GET: Candidaturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidaturas = await _context.Candidaturas
                .Include(c => c.Utilizador)
                .Include(c => c.Vaga)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidaturas == null)
            {
                return NotFound();
            }

            return View(candidaturas);
        }

        // POST: Candidaturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidaturas = await _context.Candidaturas.FindAsync(id);
            if (candidaturas != null && (candidaturas.Estado == "Rejeitada" || candidaturas.Estado == "Aceite"))
            {
                _context.Candidaturas.Remove(candidaturas);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(All));
        }

        private bool CandidaturasExists(int id)
        {
            return _context.Candidaturas.Any(e => e.Id == id);
        }
    }
}
