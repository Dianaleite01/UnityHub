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

        public RegisterModel(
            UserManager<Utilizadores> userManager,
            SignInManager<Utilizadores> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Escreva um email válido, por favor.")]
            [EmailAddress(ErrorMessage = "Não corresponde à sintaxe de um email.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "A Password é obrigatória.")]
            [StringLength(100, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Password")]
            [Compare("Password", ErrorMessage = "A password e a confirmação não coincidem.")]
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

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        //método executado ao submeter o formulário de registo
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            //verificar se os dados do formulário estão válidos
            if (ModelState.IsValid)
            {
                //cria um novo objeto de utilizador com os dados fornecidos no formulário
                var user = new Utilizadores
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nome = Input.Nome,
                    Telemovel = Input.Telemovel,
                    DataNascimento = DateOnly.FromDateTime(Input.DataNascimento),
                    Cidade = Input.Cidade,
                    Pais = Input.Pais,
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Utilizador criou uma nova conta com password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                // Adiciona os erros ao ModelState para serem exibidos
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Se chegamos aqui, algo falhou, reexibe o formulário com os erros
            return Page();
        }
    }
}
