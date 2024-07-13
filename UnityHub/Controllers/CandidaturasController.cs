using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnityHub.Data;
using UnityHub.Models;

namespace UnityHub.Controllers
{
    public class CandidaturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidaturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Candidaturas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Candidaturas.Include(c => c.Utilizador).Include(c => c.Vaga);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Candidaturas/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Candidaturas/Create
        public IActionResult Create()
        {
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizadores, "Id", "Nome");
            ViewData["VagaFK"] = new SelectList(_context.Vagas, "Id", "Nome");
            return View();
        }

        // POST: Candidaturas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Estado,VagaFK,UtilizadorFK")] Candidaturas candidaturas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candidaturas);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewData["UtilizadorFK"] = new SelectList(_context.Utilizadores, "Id", "Nome", candidaturas.UtilizadorFK);
            ViewData["VagaFK"] = new SelectList(_context.Vagas, "Id", "Nome", candidaturas.VagaFK);
            return View(candidaturas);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidaturas = await _context.Candidaturas.FindAsync(id);
            if (candidaturas != null)
            {
                _context.Candidaturas.Remove(candidaturas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidaturasExists(int id)
        {
            return _context.Candidaturas.Any(e => e.Id == id);
        }
    }
}
