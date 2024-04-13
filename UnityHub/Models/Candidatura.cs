namespace UnityHub.Models
{
    public class Candidatura
    {
        public int CandidaturaId { get; set; }
        public DateTime DataCandidatura { get; set; }
        public string Estado { get; set; }

        // Relacão com Voluntario
        public int VoluntarioId { get; set; }
        public Voluntario Voluntario { get; set; }

        // Relacão com Voluntariado 
        public int VoluntariadoId { get; set; }
        public Voluntariado Voluntariado { get; set; }
    }
}
