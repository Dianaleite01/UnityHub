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
    public class CandidaturasAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CandidaturasAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Candidaturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidaturas>>> GetCandidaturas()
        {

            return await _context.Candidaturas.ToListAsync();
        }

        // GET: api/Candidaturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Candidaturas>> GetCandidaturas(int id)
        {
            var candidatura = await _context.Candidaturas.FindAsync(id);

            if (candidatura == null)
            {
                return NotFound();
            }

            return candidatura;

        }
    }
}
