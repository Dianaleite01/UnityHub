using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnityHub.Data;
using UnityHub.Models;

namespace UnityHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VagasAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<VagasAPI> _logger;

        // Construtor para injetar dependências necessárias
        public VagasAPI(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<VagasAPI> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // Método HTTP GET para obter todas as vagas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VagaDTO>>> GetVagas()
        {
            try
            {
                _logger.LogInformation("Fetching vagas from database.");
                var vagas = await _context.Vagas
                    .Include(v => v.VagasCategorias) // Incluir relação com VagasCategorias
                    .ThenInclude(vc => vc.Categoria) // Incluir relação com Categoria
                    .ToListAsync();

                var vagasDTO = vagas.Select(v => new VagaDTO
                {
                    Id = v.Id,
                    Nome = v.Nome,
                    PeriodoVoluntariado = v.PeriodoVoluntariado,
                    Local = v.Local,
                    Descricao = v.Descricao,
                    Fotografia = v.Fotografia,
                    Categorias = v.VagasCategorias.Select(vc => vc.CategoriaId).ToList() // Mapeamento de categorias
                }).ToList();

                _logger.LogInformation("Returning {Count} vagas.", vagasDTO.Count);
                return Ok(vagasDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching vagas.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching vagas.");
            }
        }

        // Método HTTP GET para obter uma vaga específica por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<VagaDTO>> GetVagas(int id)
        {
            var vaga = await _context.Vagas
                .Include(v => v.VagasCategorias) // Incluir relação com VagasCategorias
                .ThenInclude(vc => vc.Categoria) // Incluir relação com Categoria
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vaga == null)
            {
                return NotFound();
            }

            var vagaDTO = new VagaDTO
            {
                Id = vaga.Id,
                Nome = vaga.Nome,
                PeriodoVoluntariado = vaga.PeriodoVoluntariado,
                Local = vaga.Local,
                Descricao = vaga.Descricao,
                Fotografia = vaga.Fotografia,
                Categorias = vaga.VagasCategorias.Select(vc => vc.CategoriaId).ToList() // Mapeamento de categorias
            };

            return Ok(vagaDTO);
        }

        // Método HTTP POST para criar uma nova vaga
        [HttpPost]
        public async Task<ActionResult<Vagas>> PostVagas([FromBody] VagaDTO vagaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var vaga = new Vagas
                {
                    Nome = vagaDTO.Nome,
                    PeriodoVoluntariado = vagaDTO.PeriodoVoluntariado,
                    Local = vagaDTO.Local,
                    Descricao = vagaDTO.Descricao,
                    Fotografia = ProcessImage(vagaDTO.Fotografia, _hostEnvironment.WebRootPath) // Processar imagem
                };

                // Mapear categorias para a nova vaga
                vaga.VagasCategorias = vagaDTO.Categorias.Select(categoriaId => new VagaCategoria { CategoriaId = categoriaId }).ToList();

                _context.Vagas.Add(vaga);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetVagas), new { id = vaga.Id }, vaga);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar vaga: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar vaga.");
            }
        }

        // Método HTTP PUT para atualizar uma vaga existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVagas(int id, [FromBody] VagaDTO vagaDTO)
        {
            if (id != vagaDTO.Id)
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
                    .Include(v => v.VagasCategorias) // Incluir relação com VagasCategorias
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (existingVaga == null)
                {
                    return NotFound();
                }

                existingVaga.Nome = vagaDTO.Nome;
                existingVaga.PeriodoVoluntariado = vagaDTO.PeriodoVoluntariado;
                existingVaga.Local = vagaDTO.Local;
                existingVaga.Descricao = vagaDTO.Descricao;
                existingVaga.Fotografia = ProcessImage(vagaDTO.Fotografia, _hostEnvironment.WebRootPath); // Processar imagem

                // Atualizar categorias da vaga existente
                existingVaga.VagasCategorias.Clear();
                foreach (var categoriaId in vagaDTO.Categorias)
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
                _logger.LogError(ex, "Erro ao atualizar vaga: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar vaga.");
            }
        }

        // Método HTTP DELETE para eliminar uma vaga por ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVagas(int id)
        {
            var vaga = await _context.Vagas
                .Include(v => v.VagasCategorias) // Incluir relação com VagasCategorias
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
                _logger.LogError(ex, "Erro ao deletar vaga: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar vaga.");
            }
        }

        // Método auxiliar para verificar se uma vaga existe
        private bool VagasExists(int id)
        {
            return _context.Vagas.Any(e => e.Id == id);
        }

        // Método para processar imagens
        private string ProcessImage(string base64Image, string webRootPath)
        {
            if (string.IsNullOrEmpty(base64Image))
                return null;

            if (base64Image.StartsWith("data:image/"))
            {
                int startIndex = base64Image.IndexOf("/") + 1;
                int endIndex = base64Image.IndexOf(";");
                string extFoto = base64Image.Substring(startIndex, endIndex - startIndex);

                string base64String = base64Image.Substring(base64Image.IndexOf(',') + 1);

                byte[] imageBytes = Convert.FromBase64String(base64String);

                string fileName = Guid.NewGuid().ToString() + "." + extFoto;
                string filePath = Path.Combine(webRootPath, "images", fileName);

                // Verifica se o diretório "images" existe, e cria-o se não existir
                if (!Directory.Exists(Path.Combine(webRootPath, "images")))
                {
                    Directory.CreateDirectory(Path.Combine(webRootPath, "images"));
                }

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    fs.Write(imageBytes, 0, imageBytes.Length);
                }

                return fileName;
            }

            return base64Image; // Caso a imagem já esteja no formato correto
        }
    }
}
