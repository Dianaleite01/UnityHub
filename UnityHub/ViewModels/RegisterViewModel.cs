using System.ComponentModel.DataAnnotations;

namespace UnityHub.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [Display(Name = "Telemóvel")]
        [StringLength(9)]
        [RegularExpression("9[1236][0-9]{7}", ErrorMessage = "o {0} só aceita 9 digitos")]
        public string Telemovel { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data Nascimento")]
        public DateOnly DataNascimento { get; set; }

        [Required]
        [StringLength(50)]
        public string Cidade { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "País")]
        public string Pais { get; set; }
    }
}
