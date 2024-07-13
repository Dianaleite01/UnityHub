using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnityHub.Migrations;

namespace UnityHub.Models
{
    public class Vagas
    {
        public Vagas()
        {
            VagasCategorias = new HashSet<VagaCategoria>();
            ListaCandidaturas = new HashSet<Candidaturas>();
        }
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
        [Display(Name = "Período")]
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

        // Propriedade auxiliar para upload de arquivo
        [NotMapped]
        [Display(Name = "Fotografia")]
        public IFormFile Foto { get; set; }

        // Propriedade para armazenar o caminho da fotografia
        public string Fotografia { get; set; }

        // Relacionamento N-M com Candidaturas (um Voluntariado pode ter várias candidaturas)
        public ICollection<Candidaturas> ListaCandidaturas { get; set; }

        // Relacionamento M-N com Categoria
        public ICollection<VagaCategoria> VagasCategorias { get; set; }

        // Propriedade auxiliar para receber IDs de categorias selecionadas
        [NotMapped]
        public List<int> CategoriaIds { get; set; }
    }
}
