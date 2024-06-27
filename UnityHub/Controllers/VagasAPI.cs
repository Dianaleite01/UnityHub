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

        public VagasAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Vagas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vagas>>> GetVagas()
        {

            return await _context.Vagas.ToListAsync();
        }

        // GET: api/Vagas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vagas>> GetVagas(int id)
        {
            var vaga = await _context.Vagas.FindAsync(id);

            if (vaga == null)
            {
                return NotFound();
            }

            return vaga;

        }

        // POST: api/Vagas
        [HttpPost]
        public async Task<ActionResult<Vagas>> PostVagas(Vagas vaga)
        {
            _context.Vagas.Add(vaga);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVagas), new { id = vaga.Id }, vaga);
        }

        // PUT: api/Vagas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVagas(int id, Vagas vaga)
        {
            if (id != vaga.Id)
            {
                return BadRequest();
            }

            _context.Entry(vaga).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            return NoContent();
        }

        // DELETE: api/Vagas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVagas(int id)
        {
            var vaga = await _context.Vagas.FindAsync(id);
            if (vaga == null)
            {
                return NotFound();
            }

            _context.Vagas.Remove(vaga);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VagasExists(int id)
        {
            return _context.Vagas.Any(e => e.Id == id);
        }
    }
}
