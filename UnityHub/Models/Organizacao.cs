namespace UnityHub.Models
{
    public class Organizacao
    {
        public int OrganizacaoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }

        // Relacão com Voluntariado 
        public ICollection<Voluntariado> Voluntariados { get; set; }
    }
}
