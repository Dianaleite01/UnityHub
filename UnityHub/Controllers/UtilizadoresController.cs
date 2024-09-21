using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UnityHub.Data;
using UnityHub.Models;
using UnityHub.ViewModels;
using UnityHub.Data;
using UnityHub.Models;
using UnityHub.ViewModels;

namespace UnityHub.Controllers
{
    [Authorize]
    public class UtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly SignInManager<Utilizadores> _signInManager;

        public UtilizadoresController(ApplicationDbContext context, UserManager<Utilizadores> userManager, SignInManager<Utilizadores> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Método GET para exibir a lista de utilizadores (apenas para administradores)
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilizadores.ToListAsync());
        }

        // Método GET para exibir detalhes de um utilizador específico (apenas para administradores)
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizadores == null)
            {
                return NotFound();
            }

            return View(utilizadores);
        }

        // Método GET para exibir o formulário de edição de um utilizador específico (apenas para administradores)
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadores = await _context.Utilizadores.FindAsync(id);
            if (utilizadores == null)
            {
                return NotFound();
            }

            // Verifica se o usuário é o administrador e impede a edição
            if (utilizadores.UserName == "admin@UnityHub.pt")
            {
                return Forbid();
            }

            return View(utilizadores);
        }

        // Método POST para editar um utilizador específico (apenas para administradores)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,Telemovel,Email,DataNascimento,Cidade,Pais")] Utilizadores utilizadores)
        {
            if (id != utilizadores.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizadores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadoresExists(utilizadores.Id))
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
            return View(utilizadores);
        }

        // Método GET para exibir o formulário de eliminação de um utilizador específico (apenas para administradores)
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizadores == null)
            {
                return NotFound();
            }

            // Verifica se o usuário é o administrador e impede a eliminação
            if (utilizadores.UserName == "admin@UnityHub.pt")
            {
                return Forbid();
            }

            return View(utilizadores);
        }

        // Método POST para confirmar a eliminação de um utilizador específico (apenas para administradores)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var utilizadores = await _context.Utilizadores.FindAsync(id);

            // Verifica se o usuário é o administrador e impede a eliminação
            if (utilizadores.UserName == "admin@UnityHub.pt")
            {
                return Forbid();
            }

            if (utilizadores != null)
            {
                _context.Utilizadores.Remove(utilizadores);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método para verificar se um utilizador existe
        private bool UtilizadoresExists(string id)
        {
            return _context.Utilizadores.Any(e => e.Id == id);
        }

        
        // Método GET para exibir o formulário de login (acesso anónimo permitido)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // Método POST para efetuar o logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Método GET para exibir o formulário de perfil do utilizador
        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Busca as candidaturas do utilizador
            var candidaturas = await _context.Candidaturas
                .Include(c => c.Vaga)
                .Where(c => c.UtilizadorFK == user.Id)
                .ToListAsync();

            var model = new ProfileViewModel
            {
                Nome = user.Nome,
                Telemovel = user.Telemovel,
                DataNascimento = user.DataNascimento,
                Cidade = user.Cidade,
                Pais = user.Pais,
                Email = user.Email,
                Candidaturas = candidaturas // Adiciona as candidaturas ao modelo
            };

            return View(model);
        }

        // Método POST para atualizar o perfil do utilizador
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Perfil(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.Nome = model.Nome;
            user.Telemovel = model.Telemovel;
            user.DataNascimento = model.DataNascimento;
            user.Cidade = model.Cidade;
            user.Pais = model.Pais;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        // Método GET para exibir o formulário de edição de perfil
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                Nome = user.Nome,
                Telemovel = user.Telemovel,
                DataNascimento = user.DataNascimento,
                Cidade = user.Cidade,
                Pais = user.Pais,
                Email = user.Email
            };

            return View(model);
        }

        // Método POST para atualizar o perfil do utilizador
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.Nome = model.Nome;
            user.Telemovel = model.Telemovel;
            user.DataNascimento = model.DataNascimento;
            user.Cidade = model.Cidade;
            user.Pais = model.Pais;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction("Perfil");
        }
    }
}
