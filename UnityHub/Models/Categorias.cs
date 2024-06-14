using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnityHub.Models
{
    public class Categorias
    {
        /// <summary>
        /// Chave Primária (PK)
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do Utilizador
        /// </summary>
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Nome { get; set; }

        // Relacionamento M-N com Vagas
        public ICollection<Vagas> VagasCategorias { get; set; }
    }
}
