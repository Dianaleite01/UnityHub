using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UnityHub.Models
{
    public class Utilizadores : IdentityUser
    {

        /// <summary>
        /// Nome do Utilizador
        /// </summary>
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Nome { get; set; }

        /// <summary>
        /// número de telemóvel do Utilizador
        /// </summary>
        [Display(Name = "Telemóvel")]
        [StringLength(9)]
        // 913456789
        // +351913456789
        // 00351913456789
        [RegularExpression("9[1236][0-9]{7}",
             ErrorMessage = "o {0} só aceita 9 digitos")]
        public string Telemovel { get; set; }


        /// <summary>
        /// Data de Nascimento do utilizador
        /// </summary>
        [Display(Name = "Data Nascimento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateOnly DataNascimento { get; set; }

        /// <summary>
        /// cidade do utilizador
        /// </summary>
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Cidade { get; set; }

        /// <summary>
        /// país do utilizador
        /// </summary>
        [Display(Name = "País")]
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Pais { get; set; }

        // Relacionamento N-M com Candidaturas (um Utilizador pode ter várias candidaturas, mas uma candidatura está associada a apenas um utilizador)
        public ICollection<Candidaturas> Candidaturas { get; set; }

        // Relacionamento M-N com Voluntariados (um Utilizador pode estar associado a vários voluntariados)
        public ICollection<Vagas> ListaVagas { get; set; }


    }
}
