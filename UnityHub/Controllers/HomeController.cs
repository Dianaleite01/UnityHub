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
        //Método GET para carregar a página inicial. Ele procura todas as vagas e suas respectivas categorias da BD
        public async Task<IActionResult> Index()
        {
            // Inclui as categorias associadas a cada vaga (usando relacionamentos do Entity Framework) e retorna a lista de vagas à view.
            var vagas = await _context.Vagas.Include(v => v.VagasCategorias)
                                            .ThenInclude(vc => vc.Categoria)
                                            .ToListAsync();
            return View(vagas);
        }

        // GET: Home/Details/5
        // Método GET para exibir detalhes de uma vaga específica. O acesso é restrito apenas para utilizadores autenticados.
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

        // Método GET para exibir a página "Sobre" do site. Apenas retorna uma view simples.
        public IActionResult About()
        {
            ViewData["Title"] = "Sobre";
            return View();
        }

        // Método GET para exibir a página de contato. Apenas retorna uma view simples.
        public IActionResult Contact()
        {
            return View();
        }

        // Método GET para exibir a página de política de privacidade.
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
