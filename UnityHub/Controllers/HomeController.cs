using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityHub.Data;
using UnityHub.Models;

namespace UnityHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Home
        //M�todo GET para carregar a p�gina inicial. Ele procura todas as vagas e suas respectivas categorias da BD
        public async Task<IActionResult> Index()
        {
            // Inclui as categorias associadas a cada vaga (usando relacionamentos do Entity Framework) e retorna a lista de vagas � view.
            var vagas = await _context.Vagas.Include(v => v.VagasCategorias)
                                            .ThenInclude(vc => vc.Categoria)
                                            .ToListAsync();
            return View(vagas);
        }

        // GET: Home/Details/5
        // M�todo GET para exibir detalhes de uma vaga espec�fica. O acesso � restrito apenas para utilizadores autenticados.
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // procura a vaga na BD, incluindo as categorias associadas a ela.
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

        // M�todo GET para exibir a p�gina "Sobre" do site. Apenas retorna uma view simples.
        public IActionResult About()
        {
            ViewData["Title"] = "Sobre";
            return View();
        }

        // M�todo GET para exibir a p�gina de contato. Apenas retorna uma view simples.
        public IActionResult Contact()
        {
            return View();
        }

        // M�todo GET para exibir a p�gina de pol�tica de privacidade.
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
