using System.ComponentModel.DataAnnotations;

namespace UnityHub.Models
{
    public class Vagas
    {
        /// <summary>
        /// Chave Primária (PK)
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Título da Vaga de Voluntariado
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string PeriodoVoluntariado { get; set; }

        /// <summary>
        /// Local da Vaga de Voluntariado
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Local { get; set; }

        /// <summary>
        /// Descrição detalhada da vaga de voluntariado
        /// </summary>
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        // Relacionamento N-M com Candidaturas (um Voluntariado pode ter várias candidaturas)
        public ICollection<Candidaturas> ListaCandidaturas { get; set; }

        // Relacionamento M-N com Categoria
        public ICollection<Categorias> Categorias { get; set; }
    }
}
