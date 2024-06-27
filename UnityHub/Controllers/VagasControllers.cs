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
    public class VagasControllers : Controller
    {
        private readonly ApplicationDbContext _context;

        public VagasControllers(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vagas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vagas.ToListAsync());
        }

        // GET: Vagas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vagas = await _context.Vagas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vagas == null)
            {
                return NotFound();
            }

            return View(vagas);
        }

        // GET: Vagas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vagas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,PeriodoVoluntariado,Local,Descricao")] Vagas vagas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vagas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vagas);
        }

        // GET: Vagas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vagas = await _context.Vagas.FindAsync(id);
            if (vagas == null)
            {
                return NotFound();
            }
            return View(vagas);
        }

        // POST: Vagas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,PeriodoVoluntariado,Local,Descricao")] Vagas vagas)
        {
            if (id != vagas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vagas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VagasExists(vagas.Id))
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
            return View(vagas);
        }

        // GET: Vagas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vagas = await _context.Vagas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vagas == null)
            {
                return NotFound();
            }

            return View(vagas);
        }

        // POST: Vagas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vagas = await _context.Vagas.FindAsync(id);
            if (vagas != null)
            {
                _context.Vagas.Remove(vagas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VagasExists(int id)
        {
            return _context.Vagas.Any(e => e.Id == id);
        }
    }
}
