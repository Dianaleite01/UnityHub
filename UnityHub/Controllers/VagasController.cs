using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnityHub.Data;
using UnityHub.Models;
using Microsoft.AspNetCore.Authorization;


namespace UnityHub.Controllers
{
    //apenas os utilizadores com estes roles podem aceder a este controller
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
            //carrega todas as vagas do banco de dados, incluindo as categorias 
            var applicationDbContext = _context.Vagas.Include(v => v.VagasCategorias);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vagas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vagas == null)
            {
                return NotFound();
            }

            // Carrega os detalhes de uma vaga específica, incluindo as categorias que lhe estão associadas
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

        // GET: Vagas/Create
        public IActionResult Create()
        {
            //preparar a view de criação de vaga, incluindo a lista de categorias disponiveis
            ViewData["CategoriaIds"] = new SelectList(_context.Categorias, "Id", "Nome");
            return View();
        }

        // POST: Vagas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,PeriodoVoluntariado,Local,Descricao,Foto,CategoriaIds")] Vagas vagas)
        {
            if (ModelState.IsValid)
            {
                //lidar com a fotografia
                if (vagas.Foto != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(vagas.Foto.FileName);
                    string extension = Path.GetExtension(vagas.Foto.FileName);
                    string webRootPath = _hostEnvironment.WebRootPath;
                    string uniqueFileName = $"{fileName}_{DateTime.Now.ToString("yymmssfff")}{extension}";
                    string filePath = Path.Combine(webRootPath, "images", uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await vagas.Foto.CopyToAsync(fileStream);
                    }
                    vagas.Fotografia = uniqueFileName;
                }
                //associa as categorias selecionadas à nova vaga
                vagas.VagasCategorias = new List<VagaCategoria>();
                foreach (var categoriaId in vagas.CategoriaIds)
                {
                    var categoria = await _context.Categorias.FindAsync(categoriaId);
                    if (categoria != null)
                    {
                        vagas.VagasCategorias.Add(new VagaCategoria { Vaga = vagas, CategoriaId = categoriaId });
                    }
                }

                _context.Add(vagas); //adiciona a nova vaga ao contexto
                await _context.SaveChangesAsync(); //Salva as alterações na bd
                return RedirectToAction(nameof(Index)); //redireciona para a pagina da lista de vagas
            }
            ViewData["CategoriaIds"] = new SelectList(_context.Categorias, "Id", "Nome", vagas.CategoriaIds);
            return View(vagas);
        }

        // GET: Vagas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vagas == null)
            {
                return NotFound();
            }

            //carrega a vaga existente para a editar, incluindo as categorias
            var vagas = await _context.Vagas.Include(v => v.VagasCategorias).FirstOrDefaultAsync(m => m.Id == id);
            if (vagas == null)
            {
                return NotFound();
            }

            vagas.CategoriaIds = vagas.VagasCategorias.Select(vc => vc.CategoriaId).ToList();
            ViewData["CategoriaIds"] = new SelectList(_context.Categorias, "Id", "Nome", vagas.CategoriaIds);
            return View(vagas);
        }

        // POST: Vagas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,PeriodoVoluntariado,Local,Descricao,Foto,CategoriaIds")] Vagas vagas)
        {
            if (id != vagas.Id)
            {
                return NotFound();
            }

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

                    if (vagas.Foto != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(vagas.Foto.FileName);
                        string extension = Path.GetExtension(vagas.Foto.FileName);
                        string webRootPath = _hostEnvironment.WebRootPath;
                        string uniqueFileName = $"{fileName}_{DateTime.Now.ToString("yymmssfff")}{extension}";
                        string filePath = Path.Combine(webRootPath, "images", uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await vagas.Foto.CopyToAsync(fileStream);
                        }

                        existingVagas.Fotografia = uniqueFileName;
                    }

                    //atualiza os dados com os novos valores
                    existingVagas.Nome = vagas.Nome;
                    existingVagas.PeriodoVoluntariado = vagas.PeriodoVoluntariado;
                    existingVagas.Local = vagas.Local;
                    existingVagas.Descricao = vagas.Descricao;

                    //limpa e recra a associação com as categorias selecionadas
                    existingVagas.VagasCategorias.Clear();
                    foreach (var categoriaId in vagas.CategoriaIds)
                    {
                        var categoria = await _context.Categorias.FindAsync(categoriaId);
                        if (categoria != null)
                        {
                            existingVagas.VagasCategorias.Add(new VagaCategoria { VagaId = id, CategoriaId = categoriaId });
                        }
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
            ViewData["CategoriaIds"] = new SelectList(_context.Categorias, "Id", "Nome", vagas.CategoriaIds);
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
