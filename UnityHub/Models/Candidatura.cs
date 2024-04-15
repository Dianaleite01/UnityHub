using System.ComponentModel.DataAnnotations.Schema;

namespace UnityHub.Models
{
    public class Candidatura
    {
        public int CandidaturaId { get; set; }
        public DateTime DataCandidatura { get; set; }
        public string Estado { get; set; }

        // Relacão com Voluntariado 
        [ForeignKey("Voluntariado")]
        public int VoluntariadoId { get; set; }
        public Voluntariado Voluntariado { get; set; }
    }
}
