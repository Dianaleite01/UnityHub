using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnityHub.Data;
using UnityHub.Models;
using System.IO;

namespace UnityHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VagasAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public VagasAPI(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: api/Vagas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vagas>>> GetVagas()
        {
            return await _context.Vagas
                .Include(v => v.VagasCategorias)
                .ThenInclude(vc => vc.Categoria)
                .ToListAsync();
        }

        // GET: api/Vagas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vagas>> GetVagas(int id)
        {
            var vaga = await _context.Vagas
                .Include(v => v.VagasCategorias)
                .ThenInclude(vc => vc.Categoria)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vaga == null)
            {
                return NotFound();
            }

            return vaga;
        }

        // POST: api/Vagas
        [HttpPost]
        public async Task<ActionResult<Vagas>> PostVagas([FromForm] Vagas vaga, [FromForm] IFormFile Fotografia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (Fotografia != null && Fotografia.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Fotografia.FileName);
                    string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Fotografia.CopyToAsync(fileStream);
                    }

                    vaga.Fotografia = fileName;
                }
                else
                {
                    return BadRequest("O campo Fotografia é obrigatório.");
                }

                vaga.VagasCategorias = vaga.CategoriaIds.Select(id => new VagaCategoria { CategoriaId = id }).ToList();

                _context.Vagas.Add(vaga);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetVagas), new { id = vaga.Id }, vaga);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao criar vaga: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar vaga.");
            }
        }


        // PUT: api/Vagas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVagas(int id, [FromForm] Vagas vaga, [FromForm] IFormFile Fotografia)
        {
            if (id != vaga.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingVaga = await _context.Vagas
                    .Include(v => v.VagasCategorias)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (existingVaga == null)
                {
                    return NotFound();
                }

                existingVaga.Nome = vaga.Nome;
                existingVaga.PeriodoVoluntariado = vaga.PeriodoVoluntariado;
                existingVaga.Local = vaga.Local;
                existingVaga.Descricao = vaga.Descricao;

                if (Fotografia != null && Fotografia.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Fotografia.FileName);
                    string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Fotografia.CopyToAsync(fileStream);
                    }

                    existingVaga.Fotografia = fileName;
                }

                existingVaga.VagasCategorias.Clear();
                foreach (var categoriaId in vaga.CategoriaIds)
                {
                    existingVaga.VagasCategorias.Add(new VagaCategoria { VagaId = id, CategoriaId = categoriaId });
                }

                _context.Entry(existingVaga).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VagasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao atualizar vaga: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar vaga.");
            }
        }

        // DELETE: api/Vagas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVagas(int id)
        {
            var vaga = await _context.Vagas
                .Include(v => v.VagasCategorias)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vaga == null)
            {
                return NotFound();
            }

            try
            {
                _context.Vagas.Remove(vaga);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao deletar vaga: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar vaga.");
            }
        }
        private bool VagasExists(int id)
        {
            return _context.Vagas.Any(e => e.Id == id);
        }
    }
}