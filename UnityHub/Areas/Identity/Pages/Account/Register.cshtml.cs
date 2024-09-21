
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using UnityHub.Data;
using UnityHub.Models;

namespace UnityHub.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Utilizadores> _signInManager;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<Utilizadores> userManager,
            SignInManager<Utilizadores> signInManager,
            ILogger<RegisterModel> logger,
             ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        //public IList<AuthenticationScheme> ExternalLogins { get; set; }

 
        public class InputModel
        {
            [Required(ErrorMessage = "Escreva um {0} válido, por favor,.")]
            [EmailAddress(ErrorMessage = "Não corresponde à sintaxe de um {0}")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "O Nome é obrigatório.")]
            [Display(Name = "Nome")]
            public string Nome { get; set; }

            [Required(ErrorMessage = "O Telemóvel é obrigatório.")]
            [Display(Name = "Telemóvel")]
            public string Telemovel { get; set; }

            [Display(Name = "Data de Nascimento")]
            [Required(ErrorMessage = "A data de nascimento é obrigatória")]
            [DataType(DataType.Date)]
            public DateTime DataNascimento { get; set; }

            [Display(Name = "Cidade")]
            public string Cidade { get; set; }

            [Display(Name = "País")]
            public string Pais { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new Utilizadores
                {

                    Nome = Input.Nome,
                    Telemovel = Input.Telemovel,
                    DataNascimento = DateOnly.FromDateTime(Input.DataNascimento),
                    Cidade = Input.Cidade,
                    Pais = Input.Pais,
                    LockoutEnabled = true,
                    LockoutEnd = DateTime.Now.AddDays(10000)
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                await _context.SaveChangesAsync();

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Se o registo for bem-sucedido, faz o login automático
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redireciona para a página inicial
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError("Erro de validação: {Message}", error.ErrorMessage);
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}