using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnityHub.Data;
using UnityHub.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnityHub.Controllers
{
    [Authorize(Roles = "admin")]
    public class VagasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public VagasController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Vagas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vagas.Include(v => v.VagasCategorias);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vagas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaga = await _context.Vagas
                .Include(v => v.VagasCategorias)
                    .ThenInclude(vc => vc.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vaga == null)
            {
                return NotFound();
            }

            return View(vaga);
        }


        // GET: Vagas/Create
        public IActionResult Create()
        {
            ViewBag.Categorias = _context.Categorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList();
            return View();
        }

        // POST: Vagas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,PeriodoVoluntariado,Local,Descricao,CategoriaIds")] Vagas vagas, IFormFile foto)
        {
            ModelState.Remove("Foto");
            ModelState.Remove("Fotografia");

            if (ModelState.IsValid)
            {
                if (foto != null && foto.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + foto.FileName;
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(fileStream);
                    }

                    vagas.Fotografia = fileName;
                }
                else
                {
                    ModelState.AddModelError("Foto", "O campo Fotografia é obrigatório.");
                    ViewBag.Categorias = _context.Categorias.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nome
                    }).ToList();
                    return View(vagas);
                }

                vagas.VagasCategorias = vagas.CategoriaIds
                    .Select(id => new VagaCategoria { CategoriaId = id, Vaga = vagas })
                    .ToList();

                _context.Add(vagas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = _context.Categorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList();
            return View(vagas);
        }

        // GET: Vagas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vagas == null)
            {
                return NotFound();
            }

            var vagas = await _context.Vagas.Include(v => v.VagasCategorias).FirstOrDefaultAsync(m => m.Id == id);
            if (vagas == null)
            {
                return NotFound();
            }

            vagas.CategoriaIds = vagas.VagasCategorias.Select(vc => vc.CategoriaId).ToList();
            ViewBag.Categorias = _context.Categorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList();
            return View(vagas);
        }

        // POST: Vagas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,PeriodoVoluntariado,Local,Descricao,CategoriaIds")] Vagas vagas, IFormFile foto)
        {
            if (id != vagas.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Foto");
            ModelState.Remove("Fotografia");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingVagas = await _context.Vagas
                        .Include(v => v.VagasCategorias)
                        .FirstOrDefaultAsync(v => v.Id == id);
                    if (existingVagas == null)
                    {
                        return NotFound();
                    }

                    if (foto != null && foto.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + foto.FileName;
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await foto.CopyToAsync(fileStream);
                        }

                        existingVagas.Fotografia = fileName;
                    }

                    existingVagas.Nome = vagas.Nome;
                    existingVagas.PeriodoVoluntariado = vagas.PeriodoVoluntariado;
                    existingVagas.Local = vagas.Local;
                    existingVagas.Descricao = vagas.Descricao;

                    existingVagas.VagasCategorias.Clear();
                    foreach (var categoriaId in vagas.CategoriaIds)
                    {
                        existingVagas.VagasCategorias.Add(new VagaCategoria { VagaId = id, CategoriaId = categoriaId });
                    }

                    _context.Update(existingVagas);
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

            ViewBag.Categorias = _context.Categorias.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            }).ToList();
            return View(vagas);
        }

        // GET: Vagas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vagas == null)
            {
                return NotFound();
            }

            var vagas = await _context.Vagas
                .Include(v => v.VagasCategorias)
                .ThenInclude(vc => vc.Categoria)
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
            var vagas = await _context.Vagas
                .Include(v => v.VagasCategorias)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vagas != null)
            {
                _context.Vagas.Remove(vagas);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VagasExists(int id)
        {
            return _context.Vagas.Any(e => e.Id == id);
        }
    }
}
