namespace UnityHub.Models
{
    public class Voluntariado : Utilizador
    {
        public string AreaDeInteresse { get; set; }
        public string Habilidades { get; set; }
        public string Disponibilidade { get; set; }
        public string Biografia { get; set; }

        // Relacionamentos
        public ICollection<Candidatura> Candidaturas { get; set; }
    }
}
