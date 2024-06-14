using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnityHub.Models
{
    public class Candidaturas
    {
        /// <summary>
        /// Chave Primária (PK)
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Estado da candidatura (ex: Pendente, Aprovada, Rejeitada)
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// Chave Estrangeira para a Vaga
        /// </summary>
        [ForeignKey("Vaga")]
        public int VagaFK { get; set; }
        public Vagas Vaga { get; set; }


        /// <summary>
        /// Chave Estrangeira para o Utilizador
        /// </summary>        
        [ForeignKey("Utilizador")]
        public int UtilizadorFK { get; set; }
        public Utilizadores Utilizador { get; set; }
    }
}
