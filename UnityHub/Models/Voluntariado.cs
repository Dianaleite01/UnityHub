namespace UnityHub.Models
{
    public class Voluntariado : Utilizador
    {
        public string Data { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }

        // Relacionamentos
        public int OrganizacaoId { get; set; }
        public Organizacao Organizacao { get; set; }
    }
}
